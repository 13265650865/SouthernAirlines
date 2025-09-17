using System.IO;
using System.Linq;
using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Component = Admin.NET.Application.Entity.Component;

namespace Admin.NET.Application;
/// <summary>
/// 部件服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class ComponentService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Component> _rep;
    public ComponentService(SqlSugarRepository<Component> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询部件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<ComponentOutput>> Page(ComponentInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(u => !u.IsDelete)
                    .WhereIF(input.ComponentCategoryId > 0, u => u.ComponentCategoryId == input.ComponentCategoryId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PartNum), u => u.PartNum.Contains(input.PartNum.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.FactoryName), u => u.FactoryName.Contains(input.FactoryName.Trim()))
                    .WhereIF(input.Type.HasValue, u => u.Type == input.Type)
                    .LeftJoin<ComponentCategory>((l, r) => l.ComponentCategoryId == r.Id)
                    .Select((l, r) => new ComponentOutput()
                    {
                        Id = l.Id,
                        ComponentCategoryId = l.ComponentCategoryId,
                        ComponentCategoryName = r.Name,
                        Description = l.Description,
                        PartNum = l.PartNum,
                        FactoryName = l.FactoryName,
                        Quantity = l.Quantity,
                        Unit = l.Unit,
                        Remark = l.Remark,
                        Type = l.Type
                    });

        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加部件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddComponentInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && input.Type == o.Type && o.PartNum.Equals(input.PartNum)).AnyAsync())
        {
            throw Oops.Bah("该部件已创建");
        }
        var entity = input.Adapt<Component>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除部件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteComponentInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新部件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateComponentInput input)
    {
        if (await _rep.AsQueryable().Where(o => o.Id != input.Id && !o.IsDelete && input.Type == o.Type && o.PartNum.Equals(input.PartNum)).AnyAsync())
        {
            throw Oops.Bah("该部件已创建");
        }
        var entity = input.Adapt<Component>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取部件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<Component> Get([FromQuery] QueryByIdComponentInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取部件列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<ComponentOutput>> List([FromQuery] ComponentInput input)
    {
        return await _rep.AsQueryable()
            .Where(o => !o.IsDelete)
            .WhereIF(input.Type.HasValue, u => u.Type == input.Type)
            .Select<ComponentOutput>().ToListAsync();
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "Upload")]
    public async Task Upload([FromForm] UploadPlaneInput dto)
    {
        IImporter importer = new ExcelImporter();
        var importResult = await importer.Import<ComponentImportDto>(dto.File.OpenReadStream());
        if (importResult.Exception != null)
        {
            throw importResult.Exception;
        }
        var notPartNumList = importResult.Data.Select((value, index) => Tuple.Create(index + 2, value.PartNum)).Where(o => string.IsNullOrEmpty(o.Item2));
        if (notPartNumList.Count() > 0)
        {
            throw Oops.Bah($"模板中第{string.Join(",", notPartNumList.Select(o => o.Item1))}行部件号为空");
        }
        var duplicateList = importResult.Data.GroupBy(o => o.PartNum).Select(o => new
        {
            o.Key,
            Count = o.Count()
        }).Where(o => o.Count > 1);
        if (duplicateList.Count() > 0)
        {
            throw Oops.Bah($"模板中部件号'{string.Join("','", duplicateList.Select(o => o.Key))}'重复");
        }
        try
        {
            _rep.Context.AsTenant().BeginTran();
            foreach (var item in importResult.Data)
            {
                var existsObj = await _rep.AsQueryable().Where(o => !o.IsDelete && o.Type == 1 && o.PartNum.Equals(item.PartNum)).FirstAsync();
                if (existsObj != null)
                {
                    var entity = item.Adapt<Component>();
                    entity.Id = existsObj.Id;
                    entity.Type = 1;
                    await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    var entity = item.Adapt<Component>();
                    entity.Type = 1;
                    await _rep.InsertAsync(entity);
                }
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
        var byteArray = await importer.GenerateTemplateBytes<ComponentImportDto>();
        return new FileStreamResult(new MemoryStream(byteArray), "application/octet-stream");
    }
}
