using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 酒店列表基础输入参数
    /// </summary>
    public class Base_HotelBaseInput
    {
        /// <summary>
        /// 酒店id
        /// </summary>
        public virtual long? HotId { get; set; }
        
        /// <summary>
        /// 酒店名称
        /// </summary>
        public virtual string? Name { get; set; }
        
        /// <summary>
        /// 评分
        /// </summary>
        public virtual string? Score { get; set; }
        
        /// <summary>
        /// 房间类型
        /// </summary>
        public virtual string? HouseType { get; set; }
        
        /// <summary>
        /// 床型
        /// </summary>
        public virtual string? BedType { get; set; }
        
        /// <summary>
        /// 最低价格
        /// </summary>
        public virtual decimal? Price { get; set; }
        
        /// <summary>
        /// 实际价格
        /// </summary>
        public virtual string? RPrice { get; set; }
        
        /// <summary>
        /// 窗户
        /// </summary>
        public virtual string? HasWindow { get; set; }
        
        /// <summary>
        /// 对应携程酒店url
        /// </summary>
        public virtual string? Url { get; set; }
        
        /// <summary>
        /// 价格日期
        /// </summary>
        public virtual DateTime? PDate { get; set; }
        
        /// <summary>
        /// 评论数
        /// </summary>
        public virtual int? ScoreCount { get; set; }
        
    }

    /// <summary>
    /// 酒店列表分页查询输入参数
    /// </summary>
    public class Base_HotelInput : BasePageInput
    {
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
         /// 价格日期范围
         /// </summary>
         public List<DateTime?> PDateRange { get; set; } 
        /// <summary>
        /// 评论数
        /// </summary>
        public int? ScoreCount { get; set; }
        
    }

    /// <summary>
    /// 酒店列表增加输入参数
    /// </summary>
    public class AddBase_HotelInput : Base_HotelBaseInput
    {
    }

    /// <summary>
    /// 酒店列表删除输入参数
    /// </summary>
    public class DeleteBase_HotelInput : BaseIdInput
    {
    }

    /// <summary>
    /// 酒店列表更新输入参数
    /// </summary>
    public class UpdateBase_HotelInput : Base_HotelBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 酒店列表主键查询输入参数
    /// </summary>
    public class QueryByIdBase_HotelInput : DeleteBase_HotelInput
    {

    }
