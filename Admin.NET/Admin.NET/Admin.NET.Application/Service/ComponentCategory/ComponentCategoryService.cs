using Admin.NET.Application.Const;

namespace Admin.NET.Application;
/// <summary>
/// 部件类别服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class ComponentCategoryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<ComponentCategory> _rep;
    public ComponentCategoryService(SqlSugarRepository<ComponentCategory> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询部件类别
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<ComponentCategoryOutput>> Page(ComponentCategoryInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(u => !u.IsDelete)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
                    .Select<ComponentCategoryOutput>();

        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加部件类别
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddComponentCategoryInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.Name.Equals(input.Name)).AnyAsync())
        {
            throw Oops.Bah("该部件类别已创建");
        }
        input.Name = input.Name.Replace("（", "()").Replace("）", ")");
        var entity = input.Adapt<ComponentCategory>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除部件类别
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteComponentCategoryInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新部件类别
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateComponentCategoryInput input)
    {
        if (await _rep.AsQueryable().Where(o => o.Id != input.Id && !o.IsDelete && o.Name.Equals(input.Name)).AnyAsync())
        {
            throw Oops.Bah("该部件类别已创建");
        }
        input.Name = input.Name.Replace("（", "()").Replace("）", ")");
        var entity = input.Adapt<ComponentCategory>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取部件类别
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<ComponentCategory> Get([FromQuery] QueryByIdComponentCategoryInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取部件类别列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<ComponentCategoryOutput>> List([FromQuery] ComponentCategoryInput input)
    {
        return await _rep.AsQueryable()
            .Where(o => !o.IsDelete)
            .WhereIF(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
            .Select<ComponentCategoryOutput>().ToListAsync();
    }
}
