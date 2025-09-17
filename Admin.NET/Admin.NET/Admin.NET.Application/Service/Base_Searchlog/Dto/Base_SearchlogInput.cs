using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 爬虫数据统计基础输入参数
    /// </summary>
    public class Base_SearchlogBaseInput
    {
        /// <summary>
        /// hid
        /// </summary>
        public virtual string? hid { get; set; }
        
        /// <summary>
        /// checkin
        /// </summary>
        public virtual DateTime? checkin { get; set; }
        
        /// <summary>
        /// checkout
        /// </summary>
        public virtual DateTime? checkout { get; set; }
        
        /// <summary>
        /// adult
        /// </summary>
        public virtual bool? adult { get; set; }
        
        /// <summary>
        /// children
        /// </summary>
        public virtual bool? children { get; set; }
        
        /// <summary>
        /// 耗时
        /// </summary>
        public virtual int? spendtime { get; set; }
        
        /// <summary>
        /// 分类  0 预抓  1实时
        /// </summary>
        public virtual bool? category { get; set; }
        
        /// <summary>
        /// 是否有价
        /// </summary>
        public virtual bool? hasprice { get; set; }
        
        /// <summary>
        /// createTime
        /// </summary>
        public virtual DateTime? createTime { get; set; }
        
    }

    /// <summary>
    /// 爬虫数据统计分页查询输入参数
    /// </summary>
    public class Base_SearchlogInput : BasePageInput
    {
        /// <summary>
        /// createTime范围
        /// </summary>
        public List<string> AccessDateRange { get; set; }
    }

    /// <summary>
    /// 爬虫数据统计增加输入参数
    /// </summary>
    public class AddBase_SearchlogInput : Base_SearchlogBaseInput
    {
    }

    /// <summary>
    /// 爬虫数据统计删除输入参数
    /// </summary>
    public class DeleteBase_SearchlogInput : BaseIdInput
    {
    }

    /// <summary>
    /// 爬虫数据统计更新输入参数
    /// </summary>
    public class UpdateBase_SearchlogInput : Base_SearchlogBaseInput
    {
    }

    /// <summary>
    /// 爬虫数据统计主键查询输入参数
    /// </summary>
    public class QueryByIdBase_SearchlogInput : DeleteBase_SearchlogInput
    {

    }
