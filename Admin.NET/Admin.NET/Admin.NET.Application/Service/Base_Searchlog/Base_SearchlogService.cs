using Admin.NET.Application.Const;
using Admin.NET.Core;
using Furion.DependencyInjection;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// 爬虫数据统计服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class Base_SearchlogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Base_Searchlog> _rep;
    public Base_SearchlogService(SqlSugarRepository<Base_Searchlog> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询爬虫数据统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<Base_SearchlogOutput>> Page(Base_SearchlogInput input)
    {
        var query= _rep.AsQueryable()
                    .Select<Base_SearchlogOutput>();
        if (input.AccessDateRange != null)
        {
            var endtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            var beginTime = endtime.AddMinutes(-10);
            query.Where(a => a.createTime >= endtime && a.createTime <= beginTime);
            //一天数据有百万+
            //var beginTime = input.AccessDateRange[0];
            //query.Where(a => a.createTime >= Convert.ToDateTime(input.AccessDateRange[0]) && a.createTime <= Convert.ToDateTime(input.AccessDateRange[1]));
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加爬虫数据统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBase_SearchlogInput input)
    {
        var entity = input.Adapt<Base_Searchlog>();
        await _rep.InsertAsync(entity);
    }

    ///// <summary>
    ///// 删除爬虫数据统计
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteBase_SearchlogInput input)
    //{
    //    var entity = input.Adapt<Base_Searchlog>();
    //    await _rep.FakeDeleteAsync(entity);   //假删除
    //}

    /// <summary>
    /// 更新爬虫数据统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBase_SearchlogInput input)
    {
        var entity = input.Adapt<Base_Searchlog>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取爬虫数据统计
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<Base_Searchlog> Get([FromQuery] QueryByIdBase_SearchlogInput input)
    {
        return null;
    }

    /// <summary>
    /// 获取爬虫数据统计列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<Base_SearchlogOutput>> List([FromQuery] Base_SearchlogInput input)
    {
        return await _rep.AsQueryable().Select<Base_SearchlogOutput>().ToListAsync();
    }





}

