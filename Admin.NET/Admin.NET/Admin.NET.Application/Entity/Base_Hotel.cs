using Admin.NET.Core;

namespace Admin.NET.Application.Entity;

/// <summary>
/// 酒店信息表-携程
/// </summary>
[SugarTable("base_hotel","酒店信息表-携程")]
public class Base_Hotel  : EntityBase
{
    /// <summary>
    /// 酒店id
    /// </summary>
    [SugarColumn(ColumnDescription = "酒店id")]
    public long? HotId { get; set; }
    
    /// <summary>
    /// 酒店名称
    /// </summary>
    [SugarColumn(ColumnDescription = "酒店名称", Length = 255)]
    public string? Name { get; set; }
    
    /// <summary>
    /// 评分
    /// </summary>
    [SugarColumn(ColumnDescription = "评分", Length = 20)]
    public string? Score { get; set; }
    
    /// <summary>
    /// 房间类型
    /// </summary>
    [SugarColumn(ColumnDescription = "房间类型", Length = 100)]
    public string? HouseType { get; set; }
    
    /// <summary>
    /// 床型
    /// </summary>
    [SugarColumn(ColumnDescription = "床型", Length = 30)]
    public string? BedType { get; set; }
    
    /// <summary>
    /// 最低价格
    /// </summary>
    [SugarColumn(ColumnDescription = "最低价格", Length = 8, DecimalDigits=2 )]
    public decimal? Price { get; set; }
    
    /// <summary>
    /// 实际价格
    /// </summary>
    [SugarColumn(ColumnDescription = "实际价格", Length =1000 )]
    public string? RPrice { get; set; }
    
    /// <summary>
    /// 窗户
    /// </summary>
    [SugarColumn(ColumnDescription = "窗户", Length = 10)]
    public string? HasWindow { get; set; }
    
    /// <summary>
    /// 对应携程酒店url
    /// </summary>
    [SugarColumn(ColumnDescription = "对应携程酒店url", Length = 2000)]
    public string? Url { get; set; }
    
    /// <summary>
    /// 价格日期
    /// </summary>
    [SugarColumn(ColumnDescription = "价格日期")]
    public DateTime? PDate { get; set; }
    
    /// <summary>
    /// 评论数
    /// </summary>
    [SugarColumn(ColumnDescription = "评论数")]
    public int? ScoreCount { get; set; }

    /// <summary>
    /// 早餐
    /// </summary>
    [SugarColumn(ColumnDescription = "早餐")]
    public string? Breakfast { get; set; }
    /// <summary>
    /// 禁烟政策
    /// </summary>
    [SugarColumn(ColumnDescription = "禁烟政策")]
    public string? Smoking { get; set; }
    /// <summary>
    /// 取消规则
    /// </summary>
    [SugarColumn(ColumnDescription = "取消规则", Length = 4000)]
    public string? CancelRule { get; set; }
    
        
}
