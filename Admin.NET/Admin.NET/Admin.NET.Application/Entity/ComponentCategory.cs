namespace Admin.NET.Application.Entity;

/// <summary>
/// 部件类别
/// </summary>
[SugarTable("base_component_category","部件类别")]
public class ComponentCategory  : EntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 500)]
    public string? Name { get; set; }
    
}
