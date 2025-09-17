using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 访问数据明细基础输入参数
    /// </summary>
    public class Base_RefererBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        public virtual int id { get; set; }
        
        /// <summary>
        /// 来源
        /// </summary>
        public virtual string? referer { get; set; }
        
        /// <summary>
        /// userAgent
        /// </summary>
        public virtual string? userAgent { get; set; }
        
        /// <summary>
        /// cookie
        /// </summary>
        public virtual string? cookie { get; set; }
        
        /// <summary>
        /// url
        /// </summary>
        public virtual string? url { get; set; }
        
        /// <summary>
        /// ip
        /// </summary>
        public virtual string? ip { get; set; }
        
        /// <summary>
        /// IP地区
        /// </summary>
        public virtual string? ipArea { get; set; }
        
        /// <summary>
        /// 访问酒店
        /// </summary>
        public virtual string? hotelId { get; set; }
        
        /// <summary>
        /// createTime
        /// </summary>
        public virtual DateTime? createTime { get; set; }
        
    }

    /// <summary>
    /// 访问数据明细分页查询输入参数
    /// </summary>
    public class Base_RefererInput : BasePageInput
    {
        /// <summary>
        /// IP地区
        /// </summary>
        public string? ipArea { get; set; }
        
        /// <summary>
        /// 访问酒店
        /// </summary>
        public string? hotelId { get; set; }

        public DateTime? startTime { get; set; }

    public DateTime? endTime { get; set; }
    public int? ctime { get; set; }
}

    /// <summary>
    /// 访问数据明细增加输入参数
    /// </summary>
    public class AddBase_RefererInput : Base_RefererBaseInput
    {
    }

    /// <summary>
    /// 访问数据明细删除输入参数
    /// </summary>
    public class DeleteBase_RefererInput : BaseIdInput
    {
    }

    /// <summary>
    /// 访问数据明细更新输入参数
    /// </summary>
    public class UpdateBase_RefererInput : Base_RefererBaseInput
    {
    }

    /// <summary>
    /// 访问数据明细主键查询输入参数
    /// </summary>
    public class QueryByIdBase_RefererInput : DeleteBase_RefererInput
    {

    }
