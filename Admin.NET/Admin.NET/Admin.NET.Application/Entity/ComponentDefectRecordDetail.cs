namespace Admin.NET.Application.Entity;

/// <summary>
/// 部件缺陷详情记录
/// </summary>
[SugarTable("base_component_defect_record_detail", "飞机部件缺陷详情记录")]
public class ComponentDefectRecordDetail : EntityBaseId
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "记录Id")]
    public long RecordId { get; set; }

    /// <summary>
    /// 部件描述
    /// </summary>
    [SugarColumn(ColumnDescription = "部件描述", Length = 500)]
    public string? ComponenDescription { get; set; }

    /// <summary>
    /// 部件英文缺陷描述
    /// </summary>
    [SugarColumn(ColumnDescription = "部件英文缺陷描述", Length = 500)]
    public string? EnglishDefectDescription { get; set; }

    /// <summary>
    /// 替换部件Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "替换部件Id")]
    public long ReplaceComponentId { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnDescription = "数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [SugarColumn(ColumnDescription = "单位", Length = 20)]
    public string? Unit { get; set; }
}