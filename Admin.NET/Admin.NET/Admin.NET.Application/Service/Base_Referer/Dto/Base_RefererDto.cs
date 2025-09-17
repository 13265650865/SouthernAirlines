namespace Admin.NET.Application;

    /// <summary>
    /// 访问数据明细输出参数
    /// </summary>
    public class Base_RefererDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 来源
        /// </summary>
        public string? referer { get; set; }
        
        /// <summary>
        /// userAgent
        /// </summary>
        public string? userAgent { get; set; }
        
        /// <summary>
        /// cookie
        /// </summary>
        public string? cookie { get; set; }
        
        /// <summary>
        /// url
        /// </summary>
        public string? url { get; set; }
        
        /// <summary>
        /// ip
        /// </summary>
        public string? ip { get; set; }
        
        /// <summary>
        /// IP地区
        /// </summary>
        public string? ipArea { get; set; }
        
        /// <summary>
        /// 访问酒店
        /// </summary>
        public string? hotelId { get; set; }
        
        /// <summary>
        /// createTime
        /// </summary>
        public DateTime? createTime { get; set; }
        
    }
