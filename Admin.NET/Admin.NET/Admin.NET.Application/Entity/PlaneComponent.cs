namespace Admin.NET.Application.Entity;

/// <summary>
/// 飞机部件
/// </summary>
[SugarTable("base_plane_component", "飞机部件")]
public class PlaneComponent : EntityBaseId
{
    /// <summary>
    /// 飞机Id
    /// </summary>
    [SugarColumn(ColumnDescription = "飞机Id")]
    public long? PlaneId { get; set; }

    /// <summary>
    /// 部件Id
    /// </summary>
    [SugarColumn(ColumnDescription = "部件Id")]
    public long? ComponentId { get; set; }

    /// <summary>
    /// 部件关联的CMM
    /// </summary>
    [SugarColumn(ColumnDescription = "部件关联的CMM")]
    public string? CMM { get; set; }

    /// <summary>
    /// 有效性
    /// </summary>
    [SugarColumn(ColumnDescription = "有效性")]
    public string? EFF { get; set; }
}
