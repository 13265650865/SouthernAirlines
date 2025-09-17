namespace Admin.NET.Application;

    /// <summary>
    /// 酒店列表输出参数
    /// </summary>
    public class Base_HotelOutput
    {
       /// <summary>
       /// Id
       /// </summary>
       public long Id { get; set; }
    
       /// <summary>
       /// 酒店id
       /// </summary>
       public long? HotId { get; set; }
    
       /// <summary>
       /// 酒店名称
       /// </summary>
       public string? Name { get; set; }
    
       /// <summary>
       /// 评分
       /// </summary>
       public string? Score { get; set; }
    
       /// <summary>
       /// 房间类型
       /// </summary>
       public string? HouseType { get; set; }
    
       /// <summary>
       /// 床型
       /// </summary>
       public string? BedType { get; set; }
    
       /// <summary>
       /// 最低价格
       /// </summary>
       public decimal? Price { get; set; }
    
       /// <summary>
       /// 实际价格
       /// </summary>
       public string? RPrice { get; set; }
    
       /// <summary>
       /// 窗户
       /// </summary>
       public string? HasWindow { get; set; }
    
       /// <summary>
       /// 对应携程酒店url
       /// </summary>
       public string? Url { get; set; }
    
       /// <summary>
       /// 价格日期
       /// </summary>
       public DateTime? PDate { get; set; }
    
       /// <summary>
       /// 评论数
       /// </summary>
       public int? ScoreCount { get; set; }
    
    }

public class detail
{
    /// <summary>
    /// 房间类型
    /// </summary>
    public string? HouseType { get; set; }

    /// <summary>
    /// 床型
    /// </summary>
    public string? BedType { get; set; }

    /// <summary>
    /// 实际价格
    /// </summary>
    public string? RPrice { get; set; }

    /// <summary>
    /// 窗户
    /// </summary>
    public string? HasWindow { get; set; }
}
 

