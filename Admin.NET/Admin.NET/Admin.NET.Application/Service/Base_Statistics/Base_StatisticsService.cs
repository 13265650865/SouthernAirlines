using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Admin.NET.Application;
/// <summary>
/// 数据统计信息服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class Base_StatisticsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Base_Statistics> _rep;
    public Base_StatisticsService(SqlSugarRepository<Base_Statistics> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询数据统计信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<Base_StatisticsOutput>> Page(Base_StatisticsInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Source), u => u.Source.Contains(input.Source.Trim()))
                    .GroupBy(g=>g.IPAddress)
                    .Select<Base_StatisticsOutput>();
        if(input.AccessDateRange != null && input.AccessDateRange.Count >0)
        {
                DateTime? start= input.AccessDateRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.AccessDate > start);
                if (input.AccessDateRange.Count >1 && input.AccessDateRange[1].HasValue)
                {
                    var end = input.AccessDateRange[1].Value.AddDays(1);
                    query = query.Where(u => u.AccessDate < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }



    /// <summary>
    /// 增加数据统计信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBase_StatisticsInput input)
    {
        var entity = input.Adapt<Base_Statistics>();
        entity.IPArea =await GetIPArea(input.IPAddress);
        entity.IsOld=_rep.AsQueryable().First(a=>a.IPAddress==input.IPAddress)==null?false:true;
        entity.Source = string.IsNullOrEmpty(input.Source) ? "直接访问" : input.Source;
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除数据统计信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBase_StatisticsInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   
    }

    /// <summary>
    /// 更新数据统计信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBase_StatisticsInput input)
    {
        var entity = input.Adapt<Base_Statistics>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取数据统计信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<Base_Statistics> Get([FromQuery] QueryByIdBase_StatisticsInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取数据统计信息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<Base_StatisticsOutput>> List([FromQuery] Base_StatisticsInput input)
    {
        return await _rep.AsQueryable().Select<Base_StatisticsOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "DateList")]
    public async Task<List<DateOutput>> GetDate()
    {
        // || a.CreateTime.Value.Date == DateTime.Now.AddDays(-1).Date
        var data =await _rep.AsQueryable().Where(a => a.CreateTime.Value.Date == DateTime.Now.Date).ToListAsync();
        var gList = data.GroupBy(a => new { a.IPAddress, a.CreateTime.Value.Date })
                 .Select(g => new DateOutput
                 {
                     tDate = g.Key.Date,
                     tPV=g.Count(),
                     tF=g.GroupBy(c=>c.IPAddress).Count(),
                     tAccessCount=g.Sum(c=>c.AccessCount.Value),
                     tIPCount= g.GroupBy(c => c.IPAddress).Count()
                 }).ToList();
        return gList;
    }
    [AllowAnonymous]
    public async Task<string> GetIPArea(string IpAddress)
    {
        try
        {
            var url = $"https://www.ip138.com/iplookup.asp?ip={IpAddress}&action=2";
            HttpClient client = new HttpClient();
            var res = await client.GetByteArrayAsync(url);
            var html = Encoding.GetEncoding("gb2312").GetString(res);

            string pre = "var ip_result = {\"ASN归属地\":\"";
            int pos = html.IndexOf(pre);
            html = html.Substring(pos + pre.Length);
            html = html.Substring(0, html.IndexOf('"'));
            //string[] area = html.Split(new char[] { '省', '市', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return html;
        }
        catch
        {
            return string.Empty;
        }
    }

}

