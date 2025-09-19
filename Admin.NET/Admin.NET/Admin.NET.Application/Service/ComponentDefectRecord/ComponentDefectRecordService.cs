using System.IO;
using System.Linq;
using Admin.NET.Application.Const;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Bcpg.Sig;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;
using Yitter.IdGenerator;
using Component = Admin.NET.Application.Entity.Component;

namespace Admin.NET.Application;

/// <summary>
/// 部件缺陷记录服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class ComponentDefectRecordService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<ComponentDefectRecord> _rep;
    private readonly SqlSugarRepository<ComponentDefectRecordDetail> _detailRep;
    private readonly SqlSugarRepository<Component> _componentRep;

    public ComponentDefectRecordService(SqlSugarRepository<ComponentDefectRecord> rep
        , SqlSugarRepository<ComponentDefectRecordDetail> detailRep
        , SqlSugarRepository<Component> componentRep
    )
    {
        _rep = rep;
        _detailRep = detailRep;
        _componentRep = componentRep;
    }

    /// <summary>
    /// 分页查询部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<ComponentDefectRecordOutput>> Page(ComponentDefectRecordInput input)
    {
        var query = _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .InnerJoin<Component>((pcdr, c) => pcdr.ComponentId == c.Id)
                    .LeftJoin<ComponentDefectRecordDetail>((pcdr, c, pcdrd) => pcdr.Id == pcdrd.RecordId)
                    .LeftJoin<Component>((pcdr, c, pcdrd, rc) => pcdrd.ReplaceComponentId == rc.Id)
                    .WhereIF(!string.IsNullOrEmpty(input.ComponentPartNum), (pcdr, c, pcdrd, rc) => c.PartNum.Contains(input.ComponentPartNum))
                    .WhereIF(!string.IsNullOrEmpty(input.CMM), (pcdr, c, pcdrd, rc) => pcdr.CMM.Contains(input.CMM.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceComponentPartNum), (pcdr, c, pcdrd, rc) => rc.PartNum.Contains(input.ReplaceComponentPartNum.Trim()))
                    .WhereIF(!string.IsNullOrEmpty(input.ChineseDefectDescription), (pcdr, c, pcdrd, rc) => pcdr.ChineseDefectDescription.Contains(input.ChineseDefectDescription))
                    .Select((pcdr, c, pcdrd, rc) => new ComponentDefectRecordOutput()
                    {
                        Id = pcdr.Id,
                        ComponentId = pcdr.ComponentId,
                        ComponentPartNum = c.PartNum,
                        CMM = pcdr.CMM,
                        ChineseDefectDescription = pcdr.ChineseDefectDescription,
                        Remark=pcdr.Remark,
                    }).Distinct();

        query = query.OrderBuilder(input);
        var result = await query.ToPagedListAsync(input.Page, input.PageSize);
        if (result.Items.Count() > 0)
        {
            var recordIdList = result.Items.Select(o => o.Id).ToList();
            var details = await _detailRep.AsQueryable()
                .LeftJoin<Component>((pcdrd, c) => pcdrd.ReplaceComponentId == c.Id)
                .Where((pcdrd, c) => recordIdList.Contains(pcdrd.RecordId))
                .Select((pcdrd, c) => new ComponentDefectRecordDetailOutput()
                {
                    Id = pcdrd.Id,
                    RecordId = pcdrd.RecordId,
                    EnglishDefectDescription = pcdrd.EnglishDefectDescription,
                    ComponenDescription = pcdrd.ComponenDescription,
                    ReplaceComponentId = pcdrd.ReplaceComponentId,
                    ReplaceComponentPartNum = c.PartNum,
                    Quantity = pcdrd.Quantity,
                    Unit = pcdrd.Unit,
                    Remark=pcdrd.Remark,

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
    public async Task Add(AddComponentDefectRecordInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.ComponentId == input.ComponentId
            && o.CMM == input.CMM
            && o.ChineseDefectDescription == input.ChineseDefectDescription).AnyAsync())
        {
            throw Oops.Bah("该缺陷记录已创建");
        }
        var entity = input.Adapt<ComponentDefectRecord>();
        await _rep.InsertAsync(entity);
        var entityDetails = input.Details.Select(o => o.Adapt<ComponentDefectRecordDetail>()).ToList();
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
    public async Task Delete(DeleteComponentDefectRecordInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新飞机部件缺陷记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateComponentDefectRecordInput input)
    {
        if (await _rep.AsQueryable().Where(o => !o.IsDelete && o.Id != input.Id && o.ComponentId == input.ComponentId
            && o.CMM == input.CMM
            && o.ChineseDefectDescription == input.ChineseDefectDescription).AnyAsync())
        {
            throw Oops.Bah("该缺陷记录已创建");
        }
        var entity = input.Adapt<ComponentDefectRecord>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        var oldDetails = await _detailRep.AsQueryable().Where(o => o.RecordId == entity.Id).ToListAsync();
        var addDetails = input.Details.Where(o => !o.Id.HasValue).Select(o =>
        {
            var item = o.Adapt<ComponentDefectRecordDetail>();
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
            foreach (var item in deleteDetails)
            {
                await _detailRep.DeleteAsync(item);
            }
            
        }
        var updateDetails = input.Details.Where(o => oldDetails.Any(oi => oi.Id == o.Id &&
            (oi.EnglishDefectDescription != o.EnglishDefectDescription ||
                oi.ComponenDescription != o.ComponenDescription ||
                oi.ReplaceComponentId != o.ReplaceComponentId ||
                oi.Quantity != o.Quantity ||
                oi.Unit != o.Unit))).Select(o =>
        {
            var item = o.Adapt<ComponentDefectRecordDetail>();
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
    public async Task<ComponentDefectRecordOutput> Get([FromQuery] QueryByIdComponentDefectRecordInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id);
        if (entity == null)
        {
            return null;
        }
        else
        {
            var result = entity.Adapt<ComponentDefectRecordOutput>();
            result.Details = (await _detailRep.AsQueryable()
                .Where(o => entity.Id == o.RecordId)
                .ToListAsync()).Select(o => o.Adapt<ComponentDefectRecordDetailOutput>()).ToList();
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
    public async Task<List<ComponentDefectRecordOutput>> List([FromQuery] ComponentDefectRecordInput input)
    {
        var result = await _rep.AsQueryable()
                    .Where(o => !o.IsDelete)
                    .InnerJoin<Component>((pcdr, c) => pcdr.ComponentId == c.Id)
                    .LeftJoin<ComponentDefectRecordDetail>((pcdr, c, pcdrd) => pcdr.Id == pcdrd.RecordId)
                    .LeftJoin<Component>((pcdr, c, pcdrd, rc) => pcdrd.ReplaceComponentId == rc.Id)
                    .WhereIF(!string.IsNullOrEmpty(input.ComponentPartNum), (pcdr, c, pcdrd, rc) => c.PartNum.Contains(input.ComponentPartNum))
                    .WhereIF(!string.IsNullOrEmpty(input.CMM), (pcdr, c, pcdrd, rc) => c.PartNum.Contains(input.CMM.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceComponentPartNum), (pcdr, c, pcdrd, rc) => rc.PartNum.Contains(input.ReplaceComponentPartNum.Trim()))
                    .WhereIF(!string.IsNullOrEmpty(input.ChineseDefectDescription), (pcdr, c, pcdrd, rc) => pcdr.ChineseDefectDescription.Contains(input.ChineseDefectDescription))
                    .Select((pcdr, c, pcdrd, rc) => new ComponentDefectRecordOutput()
                    {
                        Id = pcdr.Id,
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
                .ToListAsync()).Select(o => o.Adapt<ComponentDefectRecordDetailOutput>());
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
        var importResult = await importer.Import<ComponentDefectRecordImportDto>(file.OpenReadStream());
        if (importResult.Exception != null)
        {
            throw importResult.Exception;
        }

        var partNumList = importResult.Data.Select(o => o.PartNum).Distinct();
        var replaceComponentPartNumList = importResult.Data.Select(o => o.ReplaceComponentPartNum).Distinct();
        var componentPartNumList = partNumList.Union(replaceComponentPartNumList).Distinct().ToList();

        var componentList = await _componentRep.AsQueryable()
            .Where(c => !c.IsDelete && componentPartNumList.Contains(c.PartNum))
            .ToListAsync();

        if (componentList.Count == 0)
        {
            throw Oops.Bah("部件件号信息不存在");
        }

        try
        {
            _rep.Context.AsTenant().BeginTran();

            // 按CMM和中文缺陷描述分组
            foreach (var item in importResult.Data.GroupBy(o => new
            {
                o.CMM,
                o.ChineseDefectDescription
            }))
            {
                // 检查是否已存在相同CMM和中文缺陷描述的主记录
                var existingRecord = await _rep.AsQueryable()
                    .Where(o => !o.IsDelete && o.CMM == item.Key.CMM
                             && o.ChineseDefectDescription == item.Key.ChineseDefectDescription)
                    .FirstAsync();

                if (existingRecord != null)
                {
                    // 已存在主记录，只添加明细
                    foreach (var iitem in item)
                    {
                        var replaceComponent = componentList.FirstOrDefault(o => o.PartNum == iitem.ReplaceComponentPartNum);
                        if (replaceComponent == null)
                        {
                            throw Oops.Bah($"替换部件号‘{iitem.ReplaceComponentPartNum}‘不存在");
                        }

                        var entityDetail = iitem.Adapt<ComponentDefectRecordDetail>();
                        entityDetail.ReplaceComponentId = replaceComponent.Id;
                        entityDetail.RecordId = existingRecord.Id;
                        await _detailRep.InsertAsync(entityDetail);
                    }
                    continue;
                }

                // 不存在主记录，创建新主记录和明细
                var firstItem = item.First();
                var component = componentList.FirstOrDefault(o => o.PartNum == firstItem.PartNum);
                if (component == null)
                {
                    throw Oops.Bah($"部件号‘{firstItem.PartNum}‘不存在");
                }

                var entity = new ComponentDefectRecord
                {
                    ComponentId = component.Id,
                    CMM = item.Key.CMM,
                    ChineseDefectDescription = item.Key.ChineseDefectDescription,
                    Remark = firstItem.Remark
                };

                await _rep.InsertAsync(entity);

                foreach (var iitem in item)
                {
                    var replaceComponent = componentList.FirstOrDefault(o => o.PartNum == iitem.ReplaceComponentPartNum);
                    if (replaceComponent == null)
                    {
                        throw Oops.Bah($"替换部件号‘{iitem.ReplaceComponentPartNum}‘不存在");
                    }

                    var entityDetail = iitem.Adapt<ComponentDefectRecordDetail>();
                    entityDetail.ReplaceComponentId = replaceComponent.Id;
                    entityDetail.EnglishDefectDescription=iitem.EnglishDefectDescription;
                    entityDetail.RecordId = entity.Id;
                    entity.Remark = iitem.Remark;
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

    [HttpGet]
    [ApiDescriptionSettings(Name = "Export")]
    public async Task<IActionResult> Export([FromQuery] long id)
    {
        IExporter exporter = new ExcelExporter();
        var byteArray = new byte[] { };
        var entity = await _rep.GetFirstAsync(u => u.Id == id);
        if (entity != null)
        {
            var details = await _detailRep.AsQueryable().Where(o => entity.Id == o.RecordId).ToListAsync();
            if (details.Count > 0)
            {
                var componentIds = details.Select(o => o.ReplaceComponentId).Distinct().ToList();
                componentIds.Add(entity.ComponentId);
                var components = await _componentRep.AsQueryable().Where(o => componentIds.Contains(o.Id)).ToListAsync();
                var component = components.FirstOrDefault(oi => oi.Id == entity.ComponentId);
                byteArray = await exporter.ExportAsByteArray(new List<ComponentDefectRecordImportDto>(details.Select(o =>
                {
                    var item = o.Adapt<ComponentDefectRecordImportDto>();
                    if (component != null)
                    {
                        item.PartNum = component.PartNum;
                        item.CMM = entity.CMM;
                        item.ChineseDefectDescription = entity.ChineseDefectDescription;
                        item.Remark = o.Remark;
                    }
                    var itemComponent = components.FirstOrDefault(oi => oi.Id == o.ReplaceComponentId);
                    if (itemComponent != null)
                    {
                        item.ReplaceComponentPartNum = itemComponent.PartNum;
                    }
                    return item;
                })));
            }
        }
        return new FileStreamResult(new MemoryStream(byteArray), "application/octet-stream");
    }
}
