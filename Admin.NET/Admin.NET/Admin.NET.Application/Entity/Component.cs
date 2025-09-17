namespace Admin.NET.Application.Entity;

/// <summary>
/// 部件
/// </summary>
[SugarTable("base_Component", "部件")]
public class Component : EntityBase
{
    /// <summary>
    /// 部件类别
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "部件类别")]
    public long ComponentCategoryId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(ColumnDescription = "描述", Length = 500)]
    public string? Description { get; set; }

    /// <summary>
    /// 件号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "件号", Length = 50)]
    public string PartNum { get; set; }

    /// <summary>
    /// 厂家
    /// </summary>
    [SugarColumn(ColumnDescription = "厂家", Length = 50)]
    public string? FactoryName { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    [SugarColumn(ColumnDescription = "数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [SugarColumn(ColumnDescription = "单位", Length = 50)]
    public string? Unit { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 部件类型
    /// 0:飞机部件
    /// 1:缺陷部件
    /// </summary>
    [SugarColumn(ColumnDescription = "部件类型")]
    public int Type { get; set; } = 0;
}
