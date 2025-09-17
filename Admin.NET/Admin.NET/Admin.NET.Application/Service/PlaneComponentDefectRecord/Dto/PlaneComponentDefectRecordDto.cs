using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application;

/// <summary>
/// 飞机部件缺陷记录输出参数
/// </summary>
public class PlaneComponentDefectRecordDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 飞机Id
    /// </summary>
    public long PlaneId { get; set; }

    /// <summary>
    /// 部件Id
    /// </summary>
    public long? ComponentId { get; set; }

    /// <summary>
    /// 部件中文缺陷描述
    /// </summary>
    public string? ChineseDefectDescription { get; set; }
}

[ExcelExporter(Name = "Sheet1")]
public class PlaneComponentDefectRecordImportDto
{
    /// <summary>
    /// 飞机注册号
    /// </summary>
    [ImporterHeader(Name = "飞机注册号")]
    [ExporterHeader(DisplayName = "飞机注册号")]
    public string? RegisId { get; set; }

    /// <summary>
    /// 件号
    /// </summary>
    [ImporterHeader(Name = "件号")]
    [ExporterHeader(DisplayName = "件号")]
    public string? PartNum { get; set; }

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
}