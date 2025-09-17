namespace Admin.NET.Application.Entity;

/// <summary>
/// 部件缺陷记录
/// </summary>
[SugarTable("base_component_defect_record", "部件缺陷记录")]
public class ComponentDefectRecord : EntityBase
{
    /// <summary>
    /// 部件Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部件Id")]
    public long ComponentId { get; set; }

    /// <summary>
    /// 部件CMM
    /// </summary>
    [SugarColumn(ColumnDescription = "部件CMM")]
    public string CMM { get; set; }

    /// <summary>
    /// 部件中文缺陷描述
    /// </summary>
    [SugarColumn(ColumnDescription = "部件中文缺陷描述", Length = 500)]
    public string? ChineseDefectDescription { get; set; }


    /// <summary>
    /// 部件备注
    /// </summary>
    [SugarColumn(ColumnDescription = "部件备注", Length = 500)]
    public string? Remark { get; set; }
    
}
