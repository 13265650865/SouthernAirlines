//using Admin.NET.Application.Const;
//using Admin.NET.Application.Entity;
//using Admin.NET.Application.Service;
//using Admin.NET.Application.Service.Base_Referer.Dto;
//using Admin.NET.Core;
//using Furion.DependencyInjection;
//using Furion.FriendlyException;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Logging;
//using NewLife.Log;
//using Newtonsoft.Json;
//using Org.BouncyCastle.Ocsp;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;

//namespace Admin.NET.Application;
///// <summary>
///// agdoa数据服务
///// </summary>
//[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
//public class adgoa_room_fee_cacheService : IDynamicApiController, ITransient
//{
//    private readonly SqlSugarRepository<adgoa_room_fee_cache> _rep;

//    private readonly SqlSugarRepository<Traveloke_cache> _travecache;

//    private readonly SqlSugarRepository<traveloke_room_fee> _traveloke_room_fee;

//    private ISqlSugarClient _db;
//    public adgoa_room_fee_cacheService(SqlSugarRepository<adgoa_room_fee_cache> rep,
//        SqlSugarRepository<Traveloke_cache> travecache,
//        SqlSugarRepository<traveloke_room_fee> traveloke_room_fee, ISqlSugarClient db
//        )
//    {
//        _rep = rep;
//        _travecache = travecache;
//        _traveloke_room_fee = traveloke_room_fee;
//        _db=db;
//    }

//    /// <summary>
//    /// 分页查询agdoa数据
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [ApiDescriptionSettings(Name = "Page")]
//    public async Task<SqlSugarPagedList<adgoa_room_fee_cacheOutput>> Page(adgoa_room_fee_cacheInput input)
//    {
//        var query= _rep.AsQueryable()
//                    .WhereIF(!string.IsNullOrWhiteSpace(input.HotelId), u => u.HotelId.Contains(input.HotelId.Trim()))
//                    .WhereIF(!string.IsNullOrWhiteSpace(input.RoomId), u => u.RoomId.Contains(input.RoomId.Trim()))
//                    .WhereIF(input.MaxOccupancy>0, u => u.MaxOccupancy == input.MaxOccupancy)
//                    .Where(a=>a.Status==1)
//                    .Select<adgoa_room_fee_cacheOutput>()
//;
//        query = query.OrderBuilder(input);
//        return await query.ToPagedListAsync(input.Page, input.PageSize);
//    }

//    /// <summary>
//    /// 增加agdoa数据
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [ApiDescriptionSettings(Name = "Add")]
//    public async Task Add(Addadgoa_room_fee_cacheInput input)
//    {
//        var entity = input.Adapt<adgoa_room_fee_cache>();
//        await _rep.InsertAsync(entity);
//    }

//    /// <summary>
//    /// 删除agdoa数据
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [ApiDescriptionSettings(Name = "Delete")]
//    public async Task Delete(Deleteadgoa_room_fee_cacheInput input)
//    {
//        //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
//        //await _rep.FakeDeleteAsync(entity);   //假删除
//    }

//    /// <summary>
//    /// 更新agdoa数据
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [ApiDescriptionSettings(Name = "Update")]
//    public async Task Update(Updateadgoa_room_fee_cacheInput input)
//    {
//        var entity = input.Adapt<adgoa_room_fee_cache>();
//        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
//    }

//    /// <summary>
//    /// 获取agdoa数据
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpGet]
//    [ApiDescriptionSettings(Name = "Detail")]
//    public async Task<adgoa_room_fee_cache> Get([FromQuery] QueryByIdadgoa_room_fee_cacheInput input)
//    {
//        return await _rep.GetFirstAsync(u => u.Id == input.Id);
//    }

//    /// <summary>
//    /// 获取agdoa数据列表
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpGet]
//    [ApiDescriptionSettings(Name = "List")]
//    public async Task<List<adgoa_room_fee_cacheOutput>> List([FromQuery] adgoa_room_fee_cacheInput input)
//    {
//        return await _rep.AsQueryable().Select<adgoa_room_fee_cacheOutput>().ToListAsync();
//    }



//    [HttpGet]
//    [AllowAnonymous]
//    [ApiDescriptionSettings(Name = "GetOption")]
//    public async Task<dynamic> Get()
//    {
//        var res = await _rep.AsQueryable().Where(a=>a.APP_Price.HasValue&& a.PC_Price.HasValue && a.Status==1).ToListAsync();
//        List<PieData> list = new List<PieData>();
//        var app_high = res.Where(a => a.APP_Price > a.PC_Price).Count();
//        var pc_high = res.Where(a => a.APP_Price < a.PC_Price).Count();
//        var apphigh = Convert.ToInt32(Convert.ToDouble(app_high) / Convert.ToDouble(res.Count) * 100);
//        var pchigh = Convert.ToInt32(Convert.ToDouble(pc_high) / Convert.ToDouble(res.Count) * 100);
        
//        list.Add(new PieData { Name= $"APP价格更高的占比{apphigh}%，房型条数", Value= res.Where(a=>a.APP_Price> a.PC_Price).Count() });
//        list.Add(new PieData { Name = $"PC价格更高的占比{pchigh}%，房型条数", Value = res.Where(a => a.APP_Price < a.PC_Price).Count() });
//        if (100 - apphigh - pchigh > 0)
//            list.Add(new PieData { Name = $"其他异常未获取到数据或满房占比{100 - apphigh - pchigh}%，房型条数", Value = res.Count()- app_high- pc_high });
//        ChartsOutput outp = new ChartsOutput()
//        {
//            //Title = new Title { Text = "20日APP,PC价格分析" },
//            Tooltip = new Tooltip { Trigger = "item" },
//            Legend = new Legend { Left = "center" },
//            Series = new Series
//            {
//                Name = "价格分析",
//                Type = "pie",
//                Radius = "50%",
//                Data = list
//            }
//        };
//        return outp;
//    }

//    [HttpGet]
//    [AllowAnonymous]
//    [ApiDescriptionSettings(Name = "TraveLoko3")]
//    public async Task<dynamic> TraveLoko3()
//    {
//        List<string> list = new List<string>();
//        list= _travecache.AsQueryable().Select(a => a.hotid).Distinct().ToList();
//        List<List<string>> groupedLists = list
//                    .Select((value, index) => new { Value = value, Index = index })
//                    .GroupBy(x => x.Index / 10)
//                    .Select(group => group.Select(g => g.Value).ToList())
//                    .ToList();
//        Stopwatch stopwatch = new Stopwatch();
//        stopwatch.Start();
//        var beginTime = DateTime.Now.ToString("HH:mm:ss:fff");
//        await Parallel.ForEachAsync(groupedLists, new ParallelOptions()
//        {
//            MaxDegreeOfParallelism = 120
//        }, async (page, _) =>
//        {
//            await ProcessT(page);
//        });
//        return $"开始处理时间:{beginTime},  结束时间:{DateTime.Now.ToString("HH:mm:ss:fff")}3天内。总耗时：{stopwatch.ElapsedMilliseconds / 1000} 秒";
//    }
//    async Task<string> ProcessT(List<string> list, int type = 3)
//    {
//        DateTime nowdate;
//        DateTime start;
//        DateTime end;
//        nowdate = DateTime.Now.Date;
//        start = nowdate.Date;
//        end = nowdate.AddDays(3).Date;
//        int batchSize = 10;
//        for (int i = 0; i < list.Count; i += batchSize)
//        {
//            start = nowdate.Date; //重置while条件
//            Console.WriteLine($"excute...... ");
//            List<string> batch = list.Skip(i).Take(batchSize).ToList();
//            string result = string.Join(",", batch);
//            while (start < end)
//            {
//                var url = "http://47.57.247.123:8021/app/hotel/queryRate";
//                try
//                {
//                    var req = new traveloko_requestDto
//                    {
//                        hid = result,
//                        checkin = start.ToString("yyyy-MM-dd"),
//                        checkout = start.AddDays(1).ToString("yyyy-MM-dd"),
//                        adult = 1,
//                        roomcount = 1,
//                        children = 0,
//                        channel = 4
//                    };
//                    var res =await HttpHelper.PostAsync<traveloko_requestDto, t_room_price>(url, req);
//                    if (res != null)
//                    {
//                          var old1 = await _db.CopyNew().Queryable<traveloke_room_fee>().ToListAsync();
//                        //using (await _db.AsyncLock())//异步锁默认30秒超时
//                        //{
//                            foreach (var item in res.Result)
//                            {
//                                string key = $"cache_{item.Key}_{req.checkin}_{req.adult}";
//                                int exprie = 0;
//                                TimeSpan interval = start - nowdate;
//                                /*3天内， 缓存15分钟，3天-10天，缓存30分钟，10天以上缓存1小时*/
//                                if (type == 7)
//                                {
//                                    exprie = 30 * 60;
//                                }
//                                else if (type == 10)
//                                {
//                                    exprie = 60 * 60;
//                                }
//                                else
//                                {
//                                    exprie = 15 * 60;
//                                }
//                                //cache.Set(key, JsonConvert.SerializeObject(item.Value), exprie);
//                                List<traveloke_room_fee> liste = new List<traveloke_room_fee>();
//                                List<traveloke_room_fee> listo = new List<traveloke_room_fee>();
//                                foreach (var room in item.Value)
//                                {
//                                    var old =  old1.Where(a => a.hotid == item.Key && a.rid == room.RoomId && a.rateplanid == room.RatePlans[0].RatePlanId && a.price_date == req.checkin).FirstOrDefault();
//                                    if (old != null)
//                                    {
//                                        old.modifyTime = DateTime.Now;
//                                        old.price = room.RatePlans[0].MemberPrice;
//                                        old.price_date = req.checkin;
//                                        old.price_json = JsonConvert.SerializeObject(room.RatePlans);
//                                        listo.Add(old);
//                                    }
//                                    else
//                                    {
//                                        var entity = new traveloke_room_fee
//                                        {
//                                            createTime = DateTime.Now,
//                                            exprie = exprie,
//                                            hotid = item.Key,
//                                            rid = room.RoomId,
//                                            rateplanid = room.RatePlans[0].RatePlanId,
//                                            price = room.RatePlans[0].MemberPrice,
//                                            price_date = req.checkin,
//                                            price_json = JsonConvert.SerializeObject(room.RatePlans)  //先不要这json
//                                        };
//                                        liste.Add(entity);
//                                    }
//                                }
//                                if (listo.Any())
//                                    await _traveloke_room_fee.CopyNew().UpdateRangeAsync(listo);
//                                if (liste.Any())
//                                    await _traveloke_room_fee.CopyNew().InsertRangeAsync(liste);

//                            }
//                        //}
//                    }
//                }
//                catch (Exception ex)
//                {
//                    continue;
//                }
//                start = start.AddDays(1);
//            }
//        }

//        return "";
//    }
//}

