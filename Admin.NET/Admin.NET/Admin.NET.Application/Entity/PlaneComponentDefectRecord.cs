namespace Admin.NET.Application.Entity;

/// <summary>
/// 飞机部件缺陷记录
/// </summary>
[SugarTable("base_plane_component_defect_record", "飞机部件缺陷记录")]
public class PlaneComponentDefectRecord : EntityBase
{
    /// <summary>
    /// 飞机Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "飞机Id")]
    public long PlaneId { get; set; }

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
}
