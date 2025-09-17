using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application;

/// <summary>
/// 飞机部件缺陷记录输出参数
/// </summary>
public class ComponentDefectRecordDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 部件Id
    /// </summary>
    public long? ComponentId { get; set; }

    /// <summary>
    /// 部件CMM
    /// </summary>
    public string CMM { get; set; }

    /// <summary>
    /// 部件中文缺陷描述
    /// </summary>
    public string? ChineseDefectDescription { get; set; }
}

[ExcelExporter(Name = "Sheet1")]
public class ComponentDefectRecordImportDto
{
    /// <summary>
    /// 件号
    /// </summary>
    [ImporterHeader(Name = "件号")]
    [ExporterHeader(DisplayName = "件号")]
    public string? PartNum { get; set; }

    /// <summary>
    /// 件号CMM
    /// </summary>
    [ImporterHeader(Name = "CMM")]
    [ExporterHeader(DisplayName = "CMM")]
    public string? CMM { get; set; }

    /// <summary>
    /// 部件中文缺陷描述
    /// </summary>
    [ImporterHeader(Name = "部件中文缺陷描述")]
    [ExporterHeader(DisplayName = "部件中文缺陷描述")]
    public string? ChineseDefectDescription { get; set; }

    /// <summary>
    /// 部件英文缺陷描述
    /// </summary>
    [ImporterHeader(Name = "部件英文缺陷描述")]
    [ExporterHeader(DisplayName = "部件英文缺陷描述")]
    public string? EnglishDefectDescription { get; set; }

    /// <summary>
    /// 部件描述
    /// </summary>
    [ImporterHeader(Name = "部件描述")]
    [ExporterHeader(DisplayName = "部件描述")]
    public string? ComponenDescription { get; set; }

    /// <summary>
    /// 替换部件件号
    /// </summary>
    [ImporterHeader(Name = "替换部件件号")]
    [ExporterHeader(DisplayName = "替换部件件号")]
    public string? ReplaceComponentPartNum { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    [ImporterHeader(Name = "数量")]
    [ExporterHeader(DisplayName = "数量")]
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    [ImporterHeader(Name = "单位")]
    [ExporterHeader(DisplayName = "单位")]
    public string? Unit { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader(DisplayName = "备注")]
    public string? Remark { get; set; }
}