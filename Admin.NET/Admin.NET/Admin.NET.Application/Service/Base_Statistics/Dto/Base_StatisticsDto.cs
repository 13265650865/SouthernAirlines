namespace Admin.NET.Application;

    /// <summary>
    /// 数据统计信息输出参数
    /// </summary>
    public class Base_StatisticsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 访问url
        /// </summary>
        public string? Url { get; set; }
        
        /// <summary>
        /// IP地址
        /// </summary>
        public string? IPAddress { get; set; }
        
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime? AccessDate { get; set; }
        
        /// <summary>
        /// 来源
        /// </summary>
        public string? Source { get; set; }
        
        /// <summary>
        /// 访问页数
        /// </summary>
        public int? AccessCount { get; set; }
        
        /// <summary>
        /// 最后停留地址
        /// </summary>
        public string? LastAccessUrl { get; set; }
        
        /// <summary>
        /// 是否老访客
        /// </summary>
        public bool? IsOld { get; set; }
        
    }
