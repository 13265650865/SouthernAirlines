using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Admin.NET.Application.Service.Base_Referer.Dto;
using Admin.NET.Core;
using Elasticsearch.Net;
using FluentEmail.Core;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Nest;
using NewLife.Caching;
using NewLife.Reflection;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Crypto.Macs;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinExpressBusinessOrderAddRequest.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinMenuAddConditionalRequest.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CVOCRDrivingResponse.Types.CardPosition.Types;

namespace Admin.NET.Application;
/// <summary>
/// 来源统计服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class Base_RefererService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Base_Referer> _rep;
    private readonly SqlSugarRepository<Base_Searchlog> _sLog;
    private readonly SqlSugarRepository<Adgoa_Cache> _adgoa;
    static object lockObject = new object();
    public Base_RefererService(SqlSugarRepository<Base_Referer> rep, SqlSugarRepository<Base_Searchlog> sLog, SqlSugarRepository<Adgoa_Cache> adgod)
    {
        _rep = rep;
        _sLog = sLog;
        _adgoa = adgod;
    }

    /// <summary>
    /// 分页查询来源统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<Base_RefererOutput>> Page(Base_RefererInput input)
    {
        var query = _rep.AsQueryable().GroupBy(a => a.ip)
                    .Select<Base_RefererOutput>();
        if (input.ctime == 1)
        {
            var endtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var beginTime = endtime.AddMinutes(-10);
            query = query.Where(u => u.createTime >= beginTime && u.createTime <= endtime);
        }
        if (input.ctime == 2)
        {
            DateTime currentDate = DateTime.Now.Date;
            var endtime = currentDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            var beginTime = currentDate;
            query = query.Where(u => u.createTime >= beginTime && u.createTime <= endtime);
        }
        query = query.OrderBuilder(input);
        var res = query.ToList();
        res.ForEach(item =>
        {
            item.referer = string.IsNullOrEmpty(item.referer) ? "直接访问" : (item.referer.Contains("google") ? "google" : item.referer);
        });
        return res.ToPagedList(input.Page, input.PageSize);
        //return await query.ToPagedListAsync();

    }

    /// <summary>
    /// 增加来源统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBase_RefererInput input)
    {
        var entity = input.Adapt<Base_Referer>();
        await _rep.InsertAsync(entity);
    }

    ///// <summary>
    ///// 删除来源统计
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteBase_RefererInput input)
    //{
    //    //await _rep.FakeDeleteAsync(entity);   //假删除
    //}

    /// <summary>
    /// 更新来源统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBase_RefererInput input)
    {
        var entity = input.Adapt<Base_Referer>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取来源统计
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetOption")]
    public async Task<dynamic> Get([FromQuery] int chatsType, [FromQuery] int timeType)
    {
        if (timeType == 0)
        {
            var res = await _rep.AsQueryable().ToListAsync();
            res.ForEach(item =>
            {
                item.referer = string.IsNullOrEmpty(item.referer) ? "直接访问" : (item.referer.Contains("google") ? "google" : item.referer);
                item.userAgent = item.userAgent.Contains("Android") ? "Android" : "IOS";
            });
            if (chatsType == 0)
            {
                var listData = res.GroupBy(x => new { x.referer }).Select(x => new PieData { Name = string.IsNullOrEmpty(x.Key.referer) ? "直接访问" : x.Key.referer.Replace("www.", "").Replace("/", ""), Value = x.Count() }).ToList();
                ChartsOutput outp = new ChartsOutput()
                {
                    //Title = new Title { Text = "来源类型统计" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "来源分析",
                        Type = "pie",
                        Radius = "50%",
                        Data = listData
                    }
                };
                return outp;
            }
            else if (chatsType == 1)
            {
                var sysData = res.GroupBy(x => new { x.userAgent }).Select(x => new PieData { Name = x.Key.userAgent, Value = x.Count() }).ToList();
                ChartsOutput outp1 = new ChartsOutput()
                {
                    //Title = new Title { Text = "来源类型统计" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "系统分析",
                        Type = "pie",
                        Radius = "50%",
                        Data = sysData
                    }
                };
                return outp1;
            }
            else if (chatsType == 2)
            {
                var IpData = res.GroupBy(x => new { x.ipArea }).Select(x => new PieData { Name = x.Key.ipArea, Value = x.Count() }).ToList();
                ChartsOutput outp2 = new ChartsOutput()
                {
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "IP地址分析",
                        Type = "pie",
                        Radius = "50%",
                        Data = IpData
                    }
                };

                return outp2;
            }
            else if (chatsType == 3)
            {
                var sysData = res.GroupBy(x => new { x.hotelId }).Select(x => new PieData { Name = x.Key.hotelId, Value = x.Count() }).OrderByDescending(a => a.Value).Take(15).ToList();
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "酒店访问Top15统计", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "酒店维度分析",
                        Type = "pie",
                        Radius = "50%",
                        Data = sysData
                    }
                };
                return outp1;
            }
            else if (chatsType == 4)
            {
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "此数据只支持10分钟以内", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "爬虫数据表",
                        Type = "pie",
                        Radius = "50%",
                        Data = null
                    }
                };
                return outp1;
            }
            else if (chatsType == 5)
            {
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "预抓数量,支持10分钟内数据展示", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "爬虫数据表",
                        Type = "pie",
                        Radius = "50%",
                        Data = null
                    }
                };
                return outp1;
            }
            return null;
        }
        else if (timeType == 1)
        {
            var endtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var beginTime = endtime.AddMinutes(-10);
            var res = await _rep.AsQueryable().Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
            res.ForEach(item =>
            {
                item.referer = string.IsNullOrEmpty(item.referer) ? "直接访问" : (item.referer.Contains("google") ? "google" : item.referer);
                item.userAgent = item.userAgent.Contains("Android") ? "Android" : "IOS";
                item.ipArea = string.IsNullOrEmpty(item.ipArea) ? "暂未解析" : item.ipArea;
            });
            List<string> SenconTime = new List<string>();
            for (int i = 10; i >= 1; i--)
            {
                var val = endtime.Minute - i < 0 ? endtime.Minute - i + 60 : endtime.Minute - i;
                SenconTime.Add(val < 10 ? $"0{val}分" : $"{val}分");
            }
            var listData = res.GroupBy(x => new { x.createTime.Minute, x.ipArea });
            var legendData = listData.Select(a => a.Key.ipArea).Distinct().ToArray();
            if (chatsType == 0)
            {
                var listData0 = res.GroupBy(x => new { x.createTime.Minute, x.referer });
                var legendData0 = listData0.Select(a => a.Key.referer).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData0)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData0.First(a => a.Key.referer == item && a.Key.Minute == int.Parse(st.Replace("分", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分搜索来源统计", left = "center", top = "5%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = res.GroupBy(x => new { x.referer }).Select(a => a.Key.referer).Distinct().ToArray() },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 1)
            {
                var listData1 = res.GroupBy(x => new { x.createTime.Minute, x.userAgent });
                var legendData1 = listData1.Select(a => a.Key.userAgent).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData1)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData1.First(a => a.Key.userAgent == item && a.Key.Minute == int.Parse(st.Replace("分", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分,系统来源统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = res.GroupBy(x => new { x.referer }).Select(a => a.Key.referer).Distinct().ToArray() },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 2)
            {
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData.First(a => a.Key.ipArea == item && a.Key.Minute == int.Parse(st.Replace("分", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分IP地区统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = legendData },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 3)
            {
                var listData3 = res.GroupBy(x => new { x.createTime.Minute, x.hotelId });
                var legendData3 = listData3.Select(a => a.Key.hotelId).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData3)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData3.First(a => a.Key.hotelId == item && a.Key.Minute == int.Parse(st.Replace("分", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分,酒店访问统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = res.GroupBy(x => new { x.referer }).Select(a => a.Key.referer).Distinct().ToArray() },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 4)
            {
                var searchData = await _sLog.AsQueryable().Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
                searchData.ForEach(item =>
                {
                    item.hid = item.category == 0 ? "预抓" : "实时";
                });
                var listData4 = searchData.GroupBy(x => new { x.hid, x.createTime.Minute, });
                var legendData4 = new string[] { "预抓", "实时" };
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData4)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData4.First(a => a.Key.hid == item && a.Key.Minute == int.Parse(st.Replace("分", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput4 = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分,标识抓取类型统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = legendData4 },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput4;
            }
            else if (chatsType == 5)
            {
                var searchData = await _sLog.AsQueryable().Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
                var newData = searchData.Select(a => new { count = a.hid.Split(',').Length }).GroupBy(g => g.count).Select(a => new PieData { Name = $"预抓数量{a.Key.ToString()}", Value = a.Count() }).ToList().OrderBy(c=>c.Value);
                //var lineData = searchData.Select(a => new { count = a.hid.Split(',').Length }).GroupBy(g => g.count).Select(a => $"预抓数量{a.Key.ToString()}").ToList();
                //var linevalue= searchData.Select(a => new { count = a.hid.Split(',').Length }).GroupBy(g => g.count).Select(a => a.Count()).ToList();
                var outp1 = new
                {
                    Title = new Title { Text = "近10分钟内预抓数量排行", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new
                    {
                        Name = "爬虫数据表",
                        Type = "pie",
                        Radius = new string[] { "40%", "70%" },
                        lable = new { show = false, position = "center" },
                        emphasis = new { label = new { show = true, fontSize = 30, fontWeight = "bold" } },
                        Data = newData
                    }

                };
                //var outp1 = new
                //{
                //    xAxis = new { type = "category", data = lineData },
                //    yAxis = new { type = "value" },
                //    series = new { data = linevalue, type = "bar", showBackground=true, backgroundStyle=new { color= "rgba(180, 180, 180, 0.2)" } }
                //};
                return outp1;
            }
            return null;
        }
        else
        {
            DateTime currentDate = DateTime.Now.Date;
            var endtime = currentDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            var beginTime = currentDate;
            var res = await _rep.AsQueryable().Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
            res.ForEach(item =>
            {
                item.referer = string.IsNullOrEmpty(item.referer) ? "直接访问" : (item.referer.Contains("google") ? "google" : item.referer);
                item.userAgent = item.userAgent.Contains("Android") ? "Android" : "IOS";
                item.ipArea = string.IsNullOrEmpty(item.ipArea) ? "暂未解析" : item.ipArea;
            });
            List<string> SenconTime = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                SenconTime.Add($"{i}时");
            }
            if (chatsType == 0)
            {
                var listData0 = res.GroupBy(x => new { x.createTime.Hour, x.referer });
                var legendData0 = listData0.Select(a => a.Key.referer).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData0)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData0.First(a => a.Key.referer == item && a.Key.Hour == int.Parse(st.Replace("时", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分搜索来源统计", left = "center", top = "5%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = res.GroupBy(x => new { x.referer }).Select(a => a.Key.referer).Distinct().ToArray() },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 1)
            {
                var listData1 = res.GroupBy(x => new { x.createTime.Hour, x.userAgent });
                var legendData1 = listData1.Select(a => a.Key.userAgent).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData1)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData1.First(a => a.Key.userAgent == item && a.Key.Hour == int.Parse(st.Replace("时", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分,系统来源统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = res.GroupBy(x => new { x.referer }).Select(a => a.Key.referer).Distinct().ToArray() },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 2)
            {
                var listData = res.GroupBy(x => new { x.createTime.Hour, x.ipArea });
                var legendData = listData.Select(a => a.Key.ipArea).Distinct().ToArray();
                List<object> liobj = new List<object>();
                int idx = 0;
                foreach (var item in legendData)
                {
                    List<int> array = new List<int>();
                    foreach (var st in SenconTime)
                    {
                        try
                        {
                            var countData = listData.First(a => a.Key.ipArea == item && a.Key.Hour == int.Parse(st.Replace("时", "")));
                            array.Add(countData.Count());
                        }
                        catch
                        {
                            array.Add(0);
                        }
                    }
                    var s = new { name = item, type = "line", stack = $"Total{idx}", data = array.ToArray() };
                    liobj.Add(s);
                    idx++;
                }
                var lineOutput = new
                {
                    Title = new { Text = $"{beginTime}到{endtime}分IP地区统计", left = "center", top = "10%" },
                    Tooltip = new Tooltip { Trigger = "axis" },
                    Legend = new Legend { Data = legendData },
                    Grid = new
                    {
                        left = "10%",
                        right = "4%",
                        bottom = "3%",
                        containLabel = true
                    },
                    xAxis = new
                    {
                        type = "category",
                        boundaryGap = false,
                        data = SenconTime.ToArray()
                    },
                    yAxis = new
                    {
                        type = "value",
                    },
                    series = liobj.ToArray()

                };
                return lineOutput;
            }
            else if (chatsType == 3)
            {
                var re = res.GroupBy(x => new { x.hotelId }).Select(x => new PieData { Name = x.Key.hotelId, Value = x.Count() }).OrderByDescending(a => a.Value).Take(15).ToList();
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "酒店访问Top15统计", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "酒店维度分析",
                        Type = "pie",
                        Radius = "50%",
                        Data = re
                    }
                };
                return outp1;
            }
            else if (chatsType == 4)
            {
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "此数据只支持10分钟以内", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "爬虫数据表",
                        Type = "pie",
                        Radius = "50%",
                        Data = null
                    }
                };
                return outp1;
            }
            else if (chatsType == 5)
            {
                ChartsOutput outp1 = new ChartsOutput()
                {
                    Title = new Title { Text = "预抓数量,支持10分钟内数据展示", left = "center", top = "8%" },
                    Tooltip = new Tooltip { Trigger = "item" },
                    Legend = new Legend { Left = "center" },
                    Series = new Series
                    {
                        Name = "爬虫数据表",
                        Type = "pie",
                        Radius = "50%",
                        Data = null
                    }
                };
                return outp1;
            }
            var sysData = await _sLog.AsQueryable().Select(a => a.hid).ToListAsync();
            return null;
        }
    }

    /// <summary>
    /// 获取来源统计列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<dynamic> List([FromQuery] int timeType = 0, [FromQuery] int chatsType = 0)
    {
        var data = _rep.AsQueryable();
        if (timeType == 1)
        {
            var endtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var beginTime = endtime.AddMinutes(-10);
            if (chatsType == 4 || chatsType == 5)
            {
                var sData = await _sLog.AsQueryable().Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
                var average0 = sData.Where(a => a.category == 0).Sum(a => a.spendtime) / sData.Where(a => a.category == 0).ToList().Count;
                var average1 = sData.Where(a => a.category == 1).Sum(a => a.spendtime) / sData.Where(a => a.category == 1).ToList().Count;
                return new
                {
                    pv = sData.Count,
                    average = average0,
                    average1 = average1
                };
            }

            var outputData = await data.Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
            var icount = outputData.GroupBy(a => a.ip).Count();
            return new
            {
                pv = outputData.Count,
                ipcount = icount
            };
        }
        else if (timeType == 2)
        {
            DateTime currentDate = DateTime.Now.Date;
            var endtime = currentDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            var beginTime = currentDate;
            var outputData = await data.Where(a => a.createTime >= beginTime && a.createTime <= endtime).ToListAsync();
            var icount = outputData.GroupBy(a => a.ip).Count();
            return new
            {
                pv = outputData.Count,
                ipcount = icount
            };
        }
        var count = data.Count();
        var res = await data.GroupBy(a => a.ip).Select<Base_RefererOutput>().ToListAsync();

        return new
        {
            pv = count,
            ipcount = res.Count
        };
    }
    [AllowAnonymous]
    public async Task<string> StripUrl()
    {
        var list = _rep.AsQueryable().Where(a=>string.IsNullOrEmpty(a.adult)).ToList();
        foreach (var item in list)
        {
            var chenkIn = item.url.Split('&')[1].Split('=')[1];
            var chenkOut = item.url.Split('&')[2].Split('=')[1];
            var adult = item.url.Split('&')[3].Split('=')[1];
            var children = item.url.Split('&')[4].Split('=')[1];
            //请求天数
            var requestDay = Convert.ToDateTime(chenkOut).Day - Convert.ToDateTime(chenkIn).Day;
            if (requestDay < 0) { 
                if(requestDay==-30)
                    requestDay = requestDay + 31;
                else
                    requestDay = requestDay + 30;
            }
            var entity = _rep.GetById(item.Id);
            entity.requestDay = requestDay;            //统计入住天数占比
            entity.adult =adult;
            entity.children =Convert.ToInt32(children); //统计带儿童占比
            await _rep.UpdateAsync(entity);
        }
       
        return string.Empty;
    }

    [AllowAnonymous]
    public async Task GetIPArea()
    {
        var authorization = "AppCode/1a33c4c9748240668ca2e92f1300998a";
        var list = _rep.AsQueryable().Where(a => string.IsNullOrEmpty(a.ipArea)).ToList();
        foreach (var item in list)
        {
            try
            {
                #region ip138
                //var url = $"https://www.ip138.com/iplookup.asp?ip={item.ip}&action=2";
                //var html = Encoding.GetEncoding("gb2312").GetString(res);
                //string pre = "var ip_result = {\"ASN归属地\":\"";
                //int pos = html.IndexOf(pre);
                //html = html.Substring(pos + pre.Length);
                //html = html.Substring(0, html.IndexOf('"'));
                //string[] area = html.Split(new char[] { '省', '市', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                #endregion
                var nurl = $"https://ddm-ip.api.bdymkt.com/?ip={item.ip}";  //20W次调用  √√√
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Bce-Signature", authorization);
                var nres = await client.GetAsync(nurl);
                string responseContent = await nres.Content.ReadAsStringAsync();
                string country = JsonConvert.DeserializeObject<dynamic>(responseContent).data.nation;

                //var url = $"https://ipaddquery.api.bdymkt.com/ip/query?ip={item.ip}";  //2023-11-23剩下13W次调用  √√√
                //HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Add("X-Bce-Signature", authorization);
                //HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
                //var res = await client.PostAsync(url, content);
                //string responseContent = await res.Content.ReadAsStringAsync();
                //var country = JsonConvert.DeserializeObject<dynamic>(responseContent).data.country;

                var entity = _rep.GetById(item.Id);
                entity.ipArea = country.Replace("{","").Replace("}","");
                _rep.Update(entity);
            }
            catch
            {
                continue;
            }
        }
        var listIp = _rep.AsQueryable().Where(a => string.IsNullOrEmpty(a.hotelId)).ToList();
        foreach (var item in listIp)
        {
            var hotelId = item.url.Split('&')[0].Split('=')[1];
            var entity = _rep.GetById(item.Id);
            entity.hotelId = hotelId;
            _rep.Update(entity);
        }
    }

  
    void ProcessPartition(List<string> hotids)
    {
        var typePC = new string[] { "MobileApp", "Desktop" };
        var arrivaData = DateTime.Now.ToString("yyyy-MM-dd");
        var departureDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        var table_pix = $"_{arrivaData}";
        string filepath = $"C:\\Users\\Administrator\\Desktop\\agodalog\\{arrivaData}.txt";
        lock (lockObject)
        {
            //try
            //{
                foreach (var hotid in hotids)
                {
                    //File.AppendAllText(filepath, $"{hotid}开始:" + Environment.NewLine);
                    
                    foreach (var item in typePC)
                    {
                        var strVal = $"{{\"hotelIds\":\"{hotid}\",\"platform\":\"{item}\",\"arrivalDate\":\"{arrivaData}\",\"departureDate\":\"{departureDate}\",\"IntOccupancyInfo\":{{\"adults\":2,\"childAges\":\"\",\"children\":0}},\"rateplanId\":\"\",\"roomNum\":1}}";
                        var url = "http://119.23.254.4:8093/agoda/Agoda.ashx?type=GetRoomTypePrices&isCache=true&data=" + strVal;
                        try
                        {
                            var res = url.GetagodaAsync<AgodaRes>().Result;
                            var value = res.properties[0].rooms;
                            foreach (var prop in value)
                            {
                                var oldEntity = _adgoa.AsQueryable().Where(a => a.HotId == hotid && a.RoomId == prop.roomId && a.ArrivaData == arrivaData).First();
                                if (oldEntity != null)
                                {
                                    if (item == "MobileApp")
                                    {
                                        if (oldEntity.APP_Price < prop.totalPayment.inclusive)
                                            continue;
                                        oldEntity.APP_Price = prop.totalPayment.inclusive;
                                        _adgoa.Update(oldEntity);
                                    }
                                    if (item == "Desktop")
                                    {
                                        if (oldEntity.PC_Price != 0 && oldEntity.PC_Price < prop.totalPayment.inclusive)
                                            continue;
                                        oldEntity.PC_Price = prop.totalPayment.inclusive;
                                        _adgoa.Update(oldEntity);
                                    }

                                }
                                else
                                {
                                    var entity = new Adgoa_Cache
                                    {
                                        HotId = hotid,
                                        RoomId = prop.roomId,
                                        RoomName = prop.translatedRoomName,
                                        MaxAdult = prop.normalbedding + prop.extraBeds,
                                        APP_Price = item == "MobileApp" ? prop.totalPayment.inclusive : 0,
                                        PC_Price = item == "Desktop" ? prop.totalPayment.inclusive : 0,
                                        Reptile = true,
                                        ArrivaData = arrivaData
                                    };
                                    _adgoa.Insert(entity);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var entity = new Adgoa_Cache
                            {
                                HotId = hotid,
                                RoomId = "",
                                APP_Price = 0,
                                PC_Price = 0,
                                RoomName = string.Empty,
                                Reptile = false,
                                ErrMsg = ex.Message,
                                MaxAdult = 0,
                                ArrivaData = arrivaData
                            };
                            _adgoa.Insert(entity);
                            continue;
                        }
                    }
                }
            //}
            //catch(Exception ex)
            //{
                
            //    File.AppendAllText(filepath, ex.Message+ Environment.NewLine);
            //}
        }
        //foreach (var item in partition)
        //{
        //    Console.WriteLine($"Processing item: {item}, Thread: {Thread.CurrentThread.ManagedThreadId}");
        //}
    }

    static List<List<T>> PartitionList<T>(List<T> sourceList, int partitionCount)
    {
        int itemsPerPartition = (int)Math.Ceiling((double)sourceList.Count / partitionCount);
        return sourceList
            .Select((item, index) => new { item, index })
            .GroupBy(x => x.index / itemsPerPartition)
            .Select(g => g.Select(x => x.item).ToList())
            .ToList();
    }


    /// <summary>
    /// 酒店预抓数量排行
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "Hoteldetail")]
    public async Task<dynamic> TopHotel(Base_RefererInput input)
    {
        var res = await  _rep.AsQueryable()
            .WhereIF(input.startTime.HasValue,q=>q.createTime >= input.startTime)
            .WhereIF(input.endTime.HasValue, q => q.createTime <= input.endTime)
            .ToListAsync();
        var sysData = res.GroupBy(x => new { x.hotelId }).Select(x => new  { Name = x.Key.hotelId, Value = x.Count() }).OrderByDescending(a => a.Value).ToList();
        return sysData;
    }


}

