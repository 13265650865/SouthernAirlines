using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.IO;
using System.Linq;
using Yitter.IdGenerator;
using Component = Admin.NET.Application.Entity.Component;

namespace Admin.NET.Application;

/// <summary>
/// 飞机设置服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class PlaneService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PlaneFleet> _fleetRep;
    private readonly SqlSugarRepository<Plane> _rep;
    private readonly SqlSugarRepository<Component> _componentRep;
    private readonly SqlSugarRepository<PlaneComponent> _planeComponentRep;

    public PlaneService(SqlSugarRepository<PlaneFleet> fleetRep
        , SqlSugarRepository<Plane> rep
        , SqlSugarRepository<Component> componentRep
        , SqlSugarRepository<PlaneComponent> planeComponentRep)
    {
        _fleetRep = fleetRep;
        _rep = rep;
        _componentRep = componentRep;
        _planeComponentRep = planeComponentRep;
    }

    /// <summary>
    /// 分页查询飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<PlaneBaseOutput>> Page(PlaneInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RegisId), u => u.RegisId.Contains(input.RegisId.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PlaneModelName), u => u.PlaneModelName.Contains(input.PlaneModelName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PlaneModelNo), u => u.PlaneModelNo.Contains(input.PlaneModelNo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.MSN), u => u.MSN.Contains(input.MSN.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.VARTAB), u => u.VARTAB.Contains(input.VARTAB.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AMM_EFF), u => u.AMM_EFF.Contains(input.AMM_EFF.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.IPC_EFF), u => u.IPC_EFF.Contains(input.IPC_EFF.Trim()))
                    .WhereIF(input.PlaneFleetId.HasValue, u => u.PlaneFleetId == input.PlaneFleetId)
                    .LeftJoin<PlaneFleet>((l, r) => l.PlaneFleetId == r.Id)
                    .Select((l, r) => new PlaneBaseOutput()
                    {
                        Id = l.Id,
                        RegisId = l.RegisId,
                        PlaneModelName = l.PlaneModelName,
                        PlaneModelNo = l.PlaneModelNo,
                        MSN = l.MSN,
                        VARTAB = l.VARTAB,
                        AMM_EFF = l.AMM_EFF,
                        IPC_EFF = l.IPC_EFF,
                        PlaneFleetId = l.PlaneFleetId,
                        PlaneFleetCode = r.Code,
                    });

        query = query.OrderBuilder(input);
        var res = await query.ToPagedListAsync(input.Page, input.PageSize);
        return res;
    }

    /// <summary>
    /// 增加飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddPlaneInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.RegisId.Equals(input.RegisId)).AnyAsync())
        {
            throw Oops.Bah("该注册号已创建");
        }
        var entity = input.Adapt<Plane>();
        await _rep.InsertAsync(entity);
        if (input.Components != null && input.Components.Count > 0)
        {
           var t=  await _planeComponentRep.InsertRangeAsync(input.Components.Select(o => new PlaneComponent()
            {
                Id = YitIdHelper.NextId(),
                PlaneId = entity.Id,
                ComponentId = o.ComponentId,
                CMM = o.CMM,
                EFF = o.EFF
            }).ToList());
        }
    }

    /// <summary>
    /// 删除飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeletePlaneInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdatePlaneInput input)
    {
        if (await _rep.AsQueryable().Where(o => o.Id != input.Id && !o.IsDelete && o.RegisId.Equals(input.RegisId)).AnyAsync())
        {
            throw Oops.Bah("该注册号已创建");
        }
        var entity = input.Adapt<Plane>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        var oldDetails = await _planeComponentRep.AsQueryable().Where(o => o.PlaneId == entity.Id).ToListAsync();
        var addDetails = input.Components.Where(o => !o.Id.HasValue).Select(o =>
        {
            var item = o.Adapt<PlaneComponent>();
            item.Id = YitIdHelper.NextId();
            item.PlaneId = entity.Id;
            return item;
        });
        if (addDetails.Count() > 0)
        {
            await _planeComponentRep.InsertRangeAsync(addDetails.ToList());
        }
        var deleteDetails = oldDetails.Where(o => !input.Components.Any(oi => oi.Id == o.Id));
        if (deleteDetails.Count() > 0)
        {
            await _planeComponentRep.DeleteByIdsAsync(deleteDetails.ToArray());
        }
        var updateDetails = input.Components.Where(o => oldDetails.Any(oi => oi.Id == o.Id &&
            (oi.ComponentId != o.ComponentId || oi.CMM != o.CMM || oi.EFF != o.EFF))).Select(o =>
        {
            var item = o.Adapt<PlaneComponent>();
            item.PlaneId = entity.Id;
            return item;
        });
        if (updateDetails.Count() > 0)
        {
            await _planeComponentRep.UpdateRangeAsync(updateDetails.ToList());
        }
    }

    /// <summary>
    /// 获取飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<PlaneOutput> Get([FromQuery] QueryByIdPlaneInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id);
        var output = entity.Adapt<PlaneOutput>();
        output.Components = await _planeComponentRep.AsQueryable()
            .Where(u => u.PlaneId == input.Id)
            .InnerJoin<Component>((r, l) => r.ComponentId == l.Id)
            .LeftJoin<ComponentCategory>((r, l, ll) => l.ComponentCategoryId == ll.Id)
            .Select((r, l, ll) => new PlaneComponentOutput()
            {
                Id = r.Id,
                ComponentId = r.ComponentId,
                CMM = r.CMM,
                EFF = r.EFF,
            }).ToListAsync();
        return output;
    }

    /// <summary>
    /// 获取飞机设置列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<PlaneBaseOutput>> List(PlaneInput input)
    {
        return await _rep.AsQueryable()
            .Where(o => !o.IsDelete)
            .WhereIF(!string.IsNullOrWhiteSpace(input.RegisId), u => u.RegisId.Contains(input.RegisId.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PlaneModelName), u => u.PlaneModelName.Contains(input.PlaneModelName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PlaneModelNo), u => u.PlaneModelNo.Contains(input.PlaneModelNo.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.MSN), u => u.MSN.Contains(input.MSN.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.VARTAB), u => u.VARTAB.Contains(input.VARTAB.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.AMM_EFF), u => u.AMM_EFF.Contains(input.AMM_EFF.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.IPC_EFF), u => u.IPC_EFF.Contains(input.IPC_EFF.Trim()))
            .WhereIF(input.PlaneFleetId.HasValue, u => u.PlaneFleetId == input.PlaneFleetId)
            .Select<PlaneBaseOutput>().ToListAsync();
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "Upload")]
    public async Task Upload([FromForm] UploadPlaneInput dto)
    {
        IImporter importer = new ExcelImporter();
        var importResult = await importer.Import<PlaneComponentImportDto>(dto.File.OpenReadStream());
        if (importResult.Exception != null)
        {
            throw importResult.Exception;
        }
        var planeList = new List<Plane>();
        if (dto.PlaneId.HasValue)
        {
            planeList = await _rep.AsQueryable().Where(o => o.Id == dto.PlaneId).ToListAsync();
        }
        else
        {
            planeList = await _rep.AsQueryable().Where(o => o.PlaneFleetId == dto.PlaneFleetId).ToListAsync();
        }
        if (planeList.Count == 0)
        {
            throw Oops.Bah("缺少机队或飞机信息");
        }
        var componentList = await _componentRep.AsQueryable()
            .Where(o => o.Type == 0 && importResult.Data.Select(oi => oi.PartNum).Contains(o.PartNum))
            .ToListAsync();
        var addComponentList = importResult.Data.Where(o => !componentList.Any(oi => oi.PartNum == o.PartNum))
            .Select(o =>
            {
                var item = o.Adapt<Component>();
                item.Type = 0;
                return item;
            }).ToList();
        if (addComponentList.Count > 0)
        {
            await _componentRep.InsertRangeAsync(addComponentList);
            componentList.AddRange(addComponentList);
        }
        try
        {
            _rep.Context.AsTenant().BeginTran();
            foreach (var plane in planeList)
            {
                await _planeComponentRep.DeleteAsync(o => o.PlaneId == plane.Id);
                var addList = importResult.Data.Select(o =>
                {
                    var component = componentList.First(oi => oi.PartNum == o.PartNum);
                    return new PlaneComponent()
                    {
                        Id = YitIdHelper.NextId(),
                        PlaneId = plane.Id,
                        ComponentId = component.Id,
                        CMM = o.CMM,
                        EFF = o.EFF
                    };
                }).ToList();
                await _planeComponentRep.InsertRangeAsync(addList);
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
        var byteArray = await importer.GenerateTemplateBytes<PlaneComponentImportDto>();
        return new FileStreamResult(new MemoryStream(byteArray), "application/octet-stream");
    }

    /// <summary>
    /// 分页查询飞机设置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "PagePlaneComponent")]
    public async Task<SqlSugarPagedList<PlaneComponentOutput>> PagePlaneComponent(PlaneComponentInput input)
    {
        var query = _rep.AsQueryable()
                    .InnerJoin<PlaneComponent>((p, pc) => p.Id == pc.PlaneId)
                    .LeftJoin<Component>((p, pc, c) => pc.ComponentId == c.Id)
                    .WhereIF(input.PlaneId.HasValue, (p, pc, c) => p.Id == input.PlaneId)
                    .WhereIF(!string.IsNullOrEmpty(input.PartNum), (p, pc, c) => c.PartNum.Contains(input.PartNum))
                    .WhereIF(!string.IsNullOrEmpty(input.CMM), (p, pc, c) => pc.CMM.Contains(input.CMM))
                    .Select((p, pc, c) => new PlaneComponentOutput()
                    {
                        Id = p.Id,
                        ComponentId = pc.ComponentId,
                        CMM = pc.CMM,
                        EFF = pc.EFF,
                        ComponentDescription = c.Description,
                        PartNum = c.PartNum,
                        FactoryName = c.FactoryName
                    });

        query = query.OrderBuilder(input);
        var res = await query.ToPagedListAsync(input.Page, input.PageSize);
        return res;
    }
}
