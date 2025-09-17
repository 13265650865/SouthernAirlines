using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Entity;


[SugarTable("adgoa_cache_2023-12-10", "agoda产品>房型维度价格_2023-12-10")]
public class Adgoa_Cache:EntityBaseId
{
    /// <summary>
    /// 酒店id
    /// </summary>
    [SugarColumn(ColumnDescription = "酒店id")]
    public string HotId { get; set; }


    /// <summary>
    /// 房间id
    /// </summary>
    [SugarColumn(ColumnDescription = "房间id")]
    public string RoomId { get; set; }

    /// <summary>
    /// 房间名称
    /// </summary>
    [SugarColumn(ColumnDescription = "房间名称")]
    public string RoomName { get; set; }


    /// <summary>
    /// 最大入住人数
    /// </summary>
    [SugarColumn(ColumnDescription = "最大入住人数")]
    public int MaxAdult { get; set; }
    /// <summary>
    /// pc价格
    /// </summary>
    [SugarColumn(ColumnDescription = "pc价格")]
    public decimal PC_Price { get; set; }

    /// <summary>
    /// app价格
    /// </summary>
    [SugarColumn(ColumnDescription = "app价格")]
    public decimal APP_Price { get; set; }

  

    /// <summary>
    /// 爬取成功
    /// </summary>
    [SugarColumn(ColumnDescription = "爬取成功")]
    public bool Reptile { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [SugarColumn(ColumnDescription = "错误信息")]
    public string ErrMsg { get; set; }

    /// <summary>
    /// 价格日期
    /// </summary>
    [SugarColumn(ColumnDescription = "价格日期")]
    public string ArrivaData { get; set; }
}
