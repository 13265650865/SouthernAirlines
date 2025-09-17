using System.IO;
using System.Linq;
using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Yitter.IdGenerator;
using Component = Admin.NET.Application.Entity.Component;

namespace Admin.NET.Application;

/// <summary>
/// 飞机部件缺陷记录服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class PlaneComponentDefectRecordService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<PlaneComponentDefectRecord> _rep;
    private readonly SqlSugarRepository<Plane> _planeRep;
    private readonly SqlSugarRepository<PlaneComponent> _planeComponentRep;
    private readonly SqlSugarRepository<PlaneComponentDefectRecordDetail> _detailRep;
    public PlaneComponentDefectRecordService(SqlSugarRepository<PlaneComponentDefectRecord> rep
        , SqlSugarRepository<Plane> planeRep
        , SqlSugarRepository<PlaneComponent> planeComponentRep
        , SqlSugarRepository<PlaneComponentDefectRecordDetail> detailRep
    )
    {
        _rep = rep;
        _planeRep = planeRep;
        _planeComponentRep = planeComponentRep;
        _detailRep = detailRep;
    }

    /// <summary>
    /// 分页查询飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<PlaneComponentDefectRecordOutput>> Page(PlaneComponentDefectRecordInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .InnerJoin<Plane>((pcdr, p) => pcdr.PlaneId == p.Id)
                    .InnerJoin<Component>((pcdr, p, c) => pcdr.ComponentId == c.Id)
                    .LeftJoin<PlaneComponentDefectRecordDetail>((pcdr, p, c, pcdrd) => pcdr.Id == pcdrd.RecordId)
                    .LeftJoin<PlaneFleet>((pcdr, p, c, pcdrd, pf) => p.PlaneFleetId == pf.Id)
                    .LeftJoin<Component>((pcdr, p, c, pcdrd, pf, rc) => pcdrd.ReplaceComponentId == rc.Id)
                    .WhereIF(input.PlaneFleetId.HasValue, (pcdr, p, c, pcdrd, pf, rc) => p.PlaneFleetId == input.PlaneFleetId)
                    .WhereIF(input.PlaneId.HasValue, (pcdr, p, c, pcdrd, pf, rc) => pcdr.PlaneId == input.PlaneId)
                    .WhereIF(!string.IsNullOrEmpty(input.ComponentPartNum), (pcdr, p, c, pcdrd, pf, rc) => c.PartNum.Contains(input.ComponentPartNum))
                    .WhereIF(!string.IsNullOrEmpty(input.CMM), (pcdr, p, c, pcdrd, pf, rc) => pcdr.CMM.Contains(input.CMM.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceComponentPartNum), (pcdr, p, c, pcdrd, pf, rc) => rc.PartNum.Contains(input.ReplaceComponentPartNum.Trim()))
                    .WhereIF(!string.IsNullOrEmpty(input.ChineseDefectDescription), (pcdr, p, c, pcdrd, pf, rc) => pcdr.ChineseDefectDescription.Contains(input.ChineseDefectDescription))
                    .Select((pcdr, p, c, pcdrd, pf, rc) => new PlaneComponentDefectRecordOutput()
                    {
                        Id = pcdr.Id,
                        PlaneFleetId = pf.Id,
                        PlaneFleetCode = pf.Code,
                        PlaneId = pcdr.PlaneId,
                        PlaneRegisId = p.RegisId,
                        ComponentId = pcdr.ComponentId,
                        ComponentPartNum = c.PartNum,
                        CMM = pcdr.CMM,
                        ChineseDefectDescription = pcdr.ChineseDefectDescription
                    }).Distinct();
        query = query.OrderBuilder(input);
        var result = await query.ToPagedListAsync(input.Page, input.PageSize);
        if (result.Items.Count() > 0)
        {
            var recordIdList = result.Items.Select(o => o.Id).ToList();
            var details = await _detailRep.AsQueryable()
                .InnerJoin<Component>((d, c) => d.ReplaceComponentId == c.Id)
                .Where((d, c) => recordIdList.Contains(d.RecordId))
                .Select((d, c) => new PlaneComponentDefectRecordDetailOutput()
                {
                    Id = d.Id,
                    RecordId = d.RecordId,
                    EnglishDefectDescription = d.EnglishDefectDescription,
                    ComponenDescription = d.ComponenDescription,
                    ReplaceComponentId = d.ReplaceComponentId,
                    ReplaceComponentPartNum = c.PartNum,
                    Quantity = d.Quantity,
                    Unit = d.Unit
                }).ToListAsync();
            foreach (var item in result.Items)
            {
                item.Details = details.Where(o => o.RecordId == item.Id).ToList();
            }
        }
        return result;
    }

    /// <summary>
    /// 增加飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddPlaneComponentDefectRecordInput input)
    {
        // if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.PlaneId == input.PlaneId && o.ComponentId == input.ComponentId
        //     && o.ChineseDefectDescription == input.ChineseDefectDescription).AnyAsync())
        // {
        //     throw Oops.Bah("该缺陷记录已创建");
        // }
        var entity = input.Adapt<PlaneComponentDefectRecord>();
        await _rep.InsertAsync(entity);
        var entityDetails = input.Details.Select(o => o.Adapt<PlaneComponentDefectRecordDetail>()).ToList();
        entityDetails.ForEach(o =>
        {
            o.Id = YitIdHelper.NextId();
            o.RecordId = entity.Id;
        });
        await _detailRep.InsertRangeAsync(entityDetails);
    }

    /// <summary>
    /// 删除飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeletePlaneComponentDefectRecordInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);//假删除
    }

    /// <summary>
    /// 更新飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdatePlaneComponentDefectRecordInput input)
    {
        // if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.Id != input.Id && o.PlaneId == input.PlaneId && o.ComponentId == input.ComponentId
        //     && o.ChineseDefectDescription == input.ChineseDefectDescription).AnyAsync())
        // {
        //     throw Oops.Bah("该缺陷记录已创建");
        // }
        var entity = input.Adapt<PlaneComponentDefectRecord>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        var oldDetails = await _detailRep.AsQueryable().Where(o => o.RecordId == entity.Id).ToListAsync();
        var addDetails = input.Details.Where(o => !o.Id.HasValue).Select(o =>
        {
            var item = o.Adapt<PlaneComponentDefectRecordDetail>();
            item.Id = YitIdHelper.NextId();
            item.RecordId = entity.Id;
            return item;
        });
        if (addDetails.Count() > 0)
        {
            await _detailRep.InsertRangeAsync(addDetails.ToList());
        }
        var deleteDetails = oldDetails.Where(o => !input.Details.Any(oi => oi.Id == o.Id));
        if (deleteDetails.Count() > 0)
        {
            await _detailRep.DeleteByIdsAsync(deleteDetails.ToArray());
        }
        var updateDetails = input.Details.Where(o => oldDetails.Any(oi => oi.Id == o.Id &&
            (oi.EnglishDefectDescription != o.EnglishDefectDescription ||
                oi.ComponenDescription != o.ComponenDescription ||
                oi.ReplaceComponentId != o.ReplaceComponentId ||
                oi.Quantity != o.Quantity ||
                oi.Unit != o.Unit))).Select(o =>
        {
            var item = o.Adapt<PlaneComponentDefectRecordDetail>();
            return item;
        });
        if (updateDetails.Count() > 0)
        {
            await _detailRep.UpdateRangeAsync(updateDetails.ToList());
        }
    }

    /// <summary>
    /// 获取飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<PlaneComponentDefectRecordOutput> Get([FromQuery] QueryByIdPlaneComponentDefectRecordInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id);
        if (entity == null)
        {
            return null;
        }
        else
        {
            var result = entity.Adapt<PlaneComponentDefectRecordOutput>();
            result.Details = (await _detailRep.AsQueryable()
                .Where(o => entity.Id == o.RecordId)
                .ToListAsync()).Select(o => o.Adapt<PlaneComponentDefectRecordDetailOutput>()).ToList();
            return result;
        }
    }

    /// <summary>
    /// 获取飞机部件缺陷记录列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<PlaneComponentDefectRecordOutput>> List([FromQuery] PlaneComponentDefectRecordInput input)
    {
        var result = await _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .InnerJoin<Plane>((pcdr, p) => pcdr.PlaneId == p.Id)
                    .InnerJoin<Component>((pcdr, p, c) => pcdr.ComponentId == c.Id)
                    .LeftJoin<PlaneComponentDefectRecordDetail>((pcdr, p, c, pcdrd) => pcdr.Id == pcdrd.RecordId)
                    .LeftJoin<PlaneFleet>((pcdr, p, c, pcdrd, pf) => p.PlaneFleetId == pf.Id)
                    .LeftJoin<Component>((pcdr, p, c, pcdrd, pf, rc) => pcdrd.ReplaceComponentId == rc.Id)
                    .WhereIF(input.PlaneFleetId.HasValue, (pcdr, p, c, pcdrd, pf, rc) => p.PlaneFleetId == input.PlaneFleetId)
                    .WhereIF(input.PlaneId.HasValue, (pcdr, p, c, pcdrd, pf, rc) => pcdr.PlaneId == input.PlaneId)
                    .WhereIF(!string.IsNullOrEmpty(input.ComponentPartNum), (pcdr, p, c, pcdrd, pf, rc) => c.PartNum.Contains(input.ComponentPartNum))
                    .WhereIF(!string.IsNullOrEmpty(input.CMM), (pcdr, p, c, pcdrd, pf, rc) => c.PartNum.Contains(input.CMM.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceComponentPartNum), (pcdr, p, c, pcdrd, pf, rc) => rc.PartNum.Contains(input.ReplaceComponentPartNum.Trim()))
                    .WhereIF(!string.IsNullOrEmpty(input.ChineseDefectDescription), (pcdr, p, c, pcdrd, pf, rc) => pcdr.ChineseDefectDescription.Contains(input.ChineseDefectDescription))
                    .Select((pcdr, p, c, pcdrd, pf, rc) => new PlaneComponentDefectRecordOutput()
                    {
                        Id = pcdr.Id,
                        PlaneFleetId = pf.Id,
                        PlaneFleetCode = pf.Code,
                        PlaneId = pcdr.PlaneId,
                        PlaneRegisId = p.RegisId,
                        ComponentId = pcdr.ComponentId,
                        ComponentPartNum = c.PartNum,
                        CMM = pcdr.CMM,
                        ChineseDefectDescription = pcdr.ChineseDefectDescription
                    }).Distinct().ToListAsync();
        if (result.Count() > 0)
        {
            var recordIdList = result.Select(o => o.Id).ToList();
            var details = (await _detailRep.AsQueryable()
                .Where(o => recordIdList.Contains(o.RecordId))
                .ToListAsync()).Select(o => o.Adapt<PlaneComponentDefectRecordDetailOutput>());
            foreach (var item in result)
            {
                item.Details = details.Where(o => o.RecordId == item.Id).ToList();
            }
        }
        return result;
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "Upload")]
    public async Task Upload([FromForm] IFormFile file)
    {
        IImporter importer = new ExcelImporter();
        var importResult = await importer.Import<PlaneComponentDefectRecordImportDto>(file.OpenReadStream());
        if (importResult.Exception != null)
        {
            throw importResult.Exception;
        }
        var regisIdList = importResult.Data.Select(oi => oi.RegisId).Distinct().ToList();
        var planeList = await _planeRep.AsQueryable().Where(o => !o.IsDelete && regisIdList.Contains(o.RegisId)).ToListAsync();
        if (planeList.Count == 0)
        {
            throw Oops.Bah("缺少飞机信息");
        }
        var planeIdList = planeList.Select(o => (long?)o.Id).ToList();
        var componentList = await _planeComponentRep.AsQueryable()
            .InnerJoin<Component>((pc, c) => pc.ComponentId == c.Id)
            .Where((pc, c) => planeIdList.Contains(pc.PlaneId))
            .Select((pc, c) => new
            {
                pc.PlaneId,
                pc.ComponentId,
                c.PartNum
            }).ToListAsync();
        if (componentList.Count == 0)
        {
            throw Oops.Bah("飞机未关联部件信息");
        }
        try
        {
            _rep.Context.AsTenant().BeginTran();
            foreach (var item in importResult.Data.GroupBy(o => new
            {
                o.RegisId,
                o.PartNum,
                o.ChineseDefectDescription
            }))
            {
                var plane = planeList.FirstOrDefault(o => o.RegisId == item.Key.RegisId);
                if (plane == null)
                {
                    throw Oops.Bah($"注册号‘{item.Key.RegisId}‘关联的飞机不存在");
                }
                var component = componentList.FirstOrDefault(o => o.PlaneId == plane.Id && o.PartNum == item.Key.PartNum);
                if (component == null)
                {
                    throw Oops.Bah($"注册号‘{item.Key.RegisId}‘关联的部件号‘{item.Key.PartNum}‘不存在");
                }
                if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.PlaneId == plane.Id && o.ComponentId == component.ComponentId
                    && o.ChineseDefectDescription == item.Key.ChineseDefectDescription).AnyAsync())
                {
                    throw Oops.Bah($"注册号‘{item.Key.RegisId}‘关联的部件号‘{item.Key.PartNum}‘的中文缺陷描述‘{item.Key.ChineseDefectDescription}‘已存在");
                }
                var entity = item.Key.Adapt<PlaneComponentDefectRecord>();
                entity.PlaneId = plane.Id;
                entity.ComponentId = component.ComponentId.Value;
                await _rep.InsertAsync(entity);
                foreach (var iitem in item)
                {
                    var entityDetail = iitem.Adapt<PlaneComponentDefectRecordDetail>();
                    entityDetail.RecordId = entity.Id;
                    await _detailRep.InsertAsync(entityDetail);
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
        var byteArray = await importer.GenerateTemplateBytes<ComponentDefectRecordImportDto>();
        return new FileStreamResult(new MemoryStream(byteArray), "application/octet-stream");
    }
}
