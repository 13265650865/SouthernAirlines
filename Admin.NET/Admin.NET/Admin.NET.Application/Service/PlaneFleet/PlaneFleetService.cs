using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// 机队服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class PlaneFleetService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PlaneFleet> _rep;
    private readonly SqlSugarRepository<Plane> _planeRep;
    public PlaneFleetService(SqlSugarRepository<PlaneFleet> rep
        , SqlSugarRepository<Plane> planeRep
    )
    {
        _rep = rep;
        _planeRep = planeRep;
    }

    /// <summary>
    /// 分页查询机队
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<PlaneFleetOutput>> Page(PlaneFleetInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code.Trim()))
                    .Select<PlaneFleetOutput>();
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加机队
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddPlaneFleetInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.Code.Equals(input.Code)).AnyAsync())
        {
            throw Oops.Bah("该机队已存在");
        }
        var entity = input.Adapt<PlaneFleet>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除机队
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeletePlaneFleetInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新机队
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdatePlaneFleetInput input)
    {
        if (await _rep.AsQueryable().Where(o => input.Id != o.Id && !o.IsDelete && o.Code.Equals(input.Code)).AnyAsync())
        {
            throw Oops.Bah("该机队已存在");
        }
        var entity = input.Adapt<PlaneFleet>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取机队
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<PlaneFleet> Get([FromQuery] QueryByIdPlaneFleetInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取机队列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<PlaneFleetOutput>> List([FromQuery] PlaneFleetInput input)
    {
        return await _rep.AsQueryable()
            .Where(o => !o.IsDelete)
            .WhereIF(!string.IsNullOrEmpty(input.Code), o => o.Code.StartsWith(input.Code))
            .Select<PlaneFleetOutput>().ToListAsync();
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "Upload")]
    public async Task Upload([Required] IFormFile file)
    {
        IImporter importer = new ExcelImporter();
        var importResult = await importer.Import<PlaneFleetImportDto>(file.OpenReadStream());
        if (importResult.Exception != null)
        {
            throw importResult.Exception;
        }
        try
        {
            _rep.Context.AsTenant().BeginTran();
            List<PlaneFleet> addFleetList = new List<PlaneFleet>();
            foreach (var item in importResult.Data.GroupBy(o => o.PlaneFleetCode))
            {
                var fleet = await _rep.AsQueryable().Where(o => o.Code == item.Key).FirstAsync();
                if (fleet == null)
                {
                    fleet = new PlaneFleet()
                    {
                        Code = item.Key
                    };
                    await _rep.InsertAsync(fleet);
                }
                var regisIdList = item.Select(o => o.RegisId).ToList();
                var existsList = await _planeRep.AsQueryable().Where(o => regisIdList.Contains(o.RegisId)).ToListAsync();
                if (existsList.Count > 0)
                {
                    throw Oops.Bah($"以下注册号{string.Join(",", existsList.Select(o => o.RegisId))}已存在");
                }
                await _planeRep.InsertRangeAsync(item.Select(o =>
                {
                    var ii = o.Adapt<Plane>();
                    ii.PlaneFleetId = fleet.Id;
                    return ii;
                }).ToList());
            }
            _rep.Context.AsTenant().CommitTran();
        }
        catch (Exception ex)
        {
            _rep.Context.AsTenant().RollbackTran();
            throw ex;
        }
    }

    [HttpGet]
    [ApiDescriptionSettings(Name = "GetUploadTemplate")]
    public async Task<IActionResult> GetUploadTemplate()
    {
        IImporter importer = new ExcelImporter();
        var byteArray = await importer.GenerateTemplateBytes<PlaneFleetImportDto>();
        return new FileStreamResult(new MemoryStream(byteArray), "application/octet-stream");
    }
}
