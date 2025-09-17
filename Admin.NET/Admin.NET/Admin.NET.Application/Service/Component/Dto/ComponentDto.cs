using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application;

/// <summary>
/// 部件输出参数
/// </summary>
public class ComponentDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 部件类别
    /// </summary>
    public long ComponentCategoryId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 件号
    /// </summary>
    public string PartNum { get; set; }

    /// <summary>
    /// 厂家
    /// </summary>
    public string? FactoryName { get; set; }

    /// <summary>
    /// EFF
    /// </summary>
    public string? EFF { get; set; }

    /// <summary>
    /// CMM
    /// </summary>
    public string CMM { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

[ExcelExporter(Name = "Sheet1")]
public class ComponentImportDto
{
    /// <summary>
    /// 描述
    /// </summary>
    [ImporterHeader(Name = "描述")]
    [ExporterHeader(DisplayName = "描述")]
    public string? Description { get; set; }

    /// <summary>
    /// 件号
    /// </summary>
    [ImporterHeader(Name = "件号")]
    [ExporterHeader(DisplayName = "件号")]
    public string PartNum { get; set; }

    /// <summary>
    /// 厂家
    /// </summary>
    [ImporterHeader(Name = "厂家")]
    [ExporterHeader(DisplayName = "厂家")]
    public string? FactoryName { get; set; }

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