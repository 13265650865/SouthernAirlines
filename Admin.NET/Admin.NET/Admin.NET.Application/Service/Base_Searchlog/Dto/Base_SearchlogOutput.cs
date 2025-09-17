namespace Admin.NET.Application;

    /// <summary>
    /// 爬虫数据统计输出参数
    /// </summary>
    public class Base_SearchlogOutput
    {
       /// <summary>
       /// hid
       /// </summary>
       public string? hid { get; set; }
    
       /// <summary>
       /// checkin
       /// </summary>
       public DateTime? checkin { get; set; }
    
       /// <summary>
       /// checkout
       /// </summary>
       public DateTime? checkout { get; set; }
    
       /// <summary>
       /// adult
       /// </summary>
       public int? adult { get; set; }
    
       /// <summary>
       /// children
       /// </summary>
       public int? children { get; set; }
    
       /// <summary>
       /// 耗时
       /// </summary>
       public int? spendtime { get; set; }
    
       /// <summary>
       /// 分类  0 预抓  1实时
       /// </summary>
       public int? category { get; set; }
    
       /// <summary>
       /// 是否有价
       /// </summary>
       public bool? hasprice { get; set; }
    
       /// <summary>
       /// createTime
       /// </summary>
       public DateTime? createTime { get; set; }
    
    }
 

