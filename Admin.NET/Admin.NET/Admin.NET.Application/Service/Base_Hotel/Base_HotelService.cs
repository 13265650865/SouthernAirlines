using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Admin.NET.Core;
using Flurl.Http;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Nest;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessGetLiveInfoResponse.Types;

namespace Admin.NET.Application;
/// <summary>
/// 酒店列表服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class Base_HotelService : IDynamicApiController, Furion.DependencyInjection.ITransient
{
    private readonly SqlSugarRepository<Base_Hotel> _rep;
    public Base_HotelService(SqlSugarRepository<Base_Hotel> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询酒店列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<List<Base_HotelOutput>> Page(Base_HotelInput input)
    {
        var query = _rep.AsQueryable()
                     .WhereIF(input.HotId > 0, u => u.HotId == input.HotId)
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Score), u => u.Score.Contains(input.Score.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.HouseType), u => u.HouseType.Contains(input.HouseType.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.BedType), u => u.BedType.Contains(input.BedType.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.HasWindow), u => u.HasWindow.Contains(input.HasWindow.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Url), u => u.Url.Contains(input.Url.Trim()))
                     .WhereIF(input.ScoreCount > 0, u => u.ScoreCount == input.ScoreCount)
                     .WhereIF(input.Price.HasValue, u => u.Price >= input.Price.Value)
                     .Select<Base_HotelOutput>();
        ;
        if (input.PDateRange != null && input.PDateRange.Count > 0)
        {
            DateTime? start = input.PDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.PDate >= start);
            if (input.PDateRange.Count > 1 && input.PDateRange[1].HasValue)
            {
                var end = input.PDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.PDate < end);
            }
        }

        var list = query.GroupBy(a => new { a.HotId, a.Name, a.Score, a.ScoreCount, a.Price, a.Url }).Select(x => (new Base_HotelOutput
        {
            HotId = x.HotId,
            Name = x.Name,
            Score = x.Score,
            Price = x.Price,
            ScoreCount = x.ScoreCount,
            Url = x.Url,
        })).ToListAsync();
        return await list;
    }

    /// <summary>
    /// 增加酒店列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBase_HotelInput input)
    {
        var entity = input.Adapt<Base_Hotel>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除酒店列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBase_HotelInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新酒店列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBase_HotelInput input)
    {
        var entity = input.Adapt<Base_Hotel>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取酒店列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<List<Base_Hotel>> Get([FromQuery] QueryByIdBase_HotelInput input)
    {
        return await _rep.AsQueryable().Where(u => u.HotId == input.Id).ToListAsync();
    }




    /// <summary>
    /// 获取酒店列表列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<Base_HotelOutput>> List([FromQuery] Base_HotelInput input)
    {
        return await _rep.AsQueryable().Select<Base_HotelOutput>().ToListAsync();
    }
   
}

