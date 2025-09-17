using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// 数据统计信息基础输入参数
/// </summary>
public class Base_StatisticsBaseInput
{
    /// <summary>
    /// 访问url
    /// </summary>
    public virtual string? Url { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public virtual string? IPAddress { get; set; }

    /// <summary>
    /// 访问时间
    /// </summary>
    public virtual DateTime? AccessDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 来源
    /// </summary>
    public virtual string? Source { get; set; }

    /// <summary>
    /// 访问页数
    /// </summary>
    public virtual int? AccessCount { get; set; } = 1;

    /// <summary>
    /// 最后停留地址
    /// </summary>
    public virtual string? LastAccessUrl { get; set; }

    /// <summary>
    /// 是否老访客
    /// </summary>
    public virtual bool? IsOld { get; set; }

}

/// <summary>
/// 数据统计信息分页查询输入参数
/// </summary>
public class Base_StatisticsInput : BasePageInput
{
    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime? AccessDate { get; set; }

    /// <summary>
    /// 访问时间范围
    /// </summary>
    public List<DateTime?> AccessDateRange { get; set; }
    /// <summary>
    /// 来源
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// 是否老访客
    /// </summary>
    public bool? IsOld { get; set; }

}

/// <summary>
/// 数据统计信息增加输入参数
/// </summary>
public class AddBase_StatisticsInput : Base_StatisticsBaseInput
{
}

/// <summary>
/// 数据统计信息删除输入参数
/// </summary>
public class DeleteBase_StatisticsInput : BaseIdInput
{
}

/// <summary>
/// 数据统计信息更新输入参数
/// </summary>
public class UpdateBase_StatisticsInput : Base_StatisticsBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// 数据统计信息主键查询输入参数
/// </summary>
public class QueryByIdBase_StatisticsInput : DeleteBase_StatisticsInput
{

}

public class DateOutput
{ 
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime? tDate { get; set; }
    /// <summary>
    /// 今日访问量
    /// </summary>
    public int tPV { get; set; }
    /// <summary>
    /// 今日访客
    /// </summary>
    public int tF { get; set; }
    /// <summary>
    /// 增长数(较昨日)
    /// </summary>
    public int tGrowth { get; set; }

    /// <summary>
    /// 今日访问次数
    /// </summary>
    public int tAccessCount { get; set; }

    /// <summary>
    /// IP数量
    /// </summary>
    public int tIPCount { get; set; }

    /// <summary>
    /// 访问路径集合
    /// </summary>
    public string[] ListIPAddr { get; set; }
  
}
