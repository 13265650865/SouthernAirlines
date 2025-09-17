using Admin.NET.Core;

namespace Admin.NET.Application.Entity;

/// <summary>
/// 数据统计表
/// </summary>
[SugarTable("Base_Statistics","数据统计表")]
public class Base_Statistics  : EntityBase
{
    /// <summary>
    /// 访问url
    /// </summary>
    [SugarColumn(ColumnDescription = "访问url", Length = 100)]
    public string? Url { get; set; }
    
    /// <summary>
    /// IP地址
    /// </summary>
    [SugarColumn(ColumnDescription = "IP地址", Length = 20)]
    public string? IPAddress { get; set; }

    /// <summary>
    /// IP区域
    /// </summary>
    [SugarColumn(ColumnDescription = "IP区域", Length = 50)]
    public string? IPArea { get; set; }

    /// <summary>
    /// 访问时间
    /// </summary>
    [SugarColumn(ColumnDescription = "访问时间")]
    public DateTime? AccessDate { get; set; }
    
    /// <summary>
    /// 来源
    /// </summary>
    [SugarColumn(ColumnDescription = "来源", Length = 100)]
    public string? Source { get; set; }
    
    /// <summary>
    /// 访问页数
    /// </summary>
    [SugarColumn(ColumnDescription = "访问页数")]
    public int? AccessCount { get; set; }
    
    /// <summary>
    /// 最后停留地址
    /// </summary>
    [SugarColumn(ColumnDescription = "最后停留地址", Length = 100)]
    public string? LastAccessUrl { get; set; }
    
    /// <summary>
    /// 是否老访客
    /// </summary>
    [SugarColumn(ColumnDescription = "是否老访客")]
    public bool? IsOld { get; set; }
    
}
