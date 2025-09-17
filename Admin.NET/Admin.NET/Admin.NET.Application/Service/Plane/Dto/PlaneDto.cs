using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application;

/// <summary>
/// 飞机设置输出参数
/// </summary>
public class PlaneDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 注册号
    /// </summary>
    public string? RegisId { get; set; }

    /// <summary>
    /// 机型名称
    /// </summary>
    public string? PlaneModelName { get; set; }

    /// <summary>
    /// Model
    /// </summary>
    public string? PlaneModel { get; set; }

    /// <summary>
    /// MSN
    /// </summary>
    public string? MSN { get; set; }

    /// <summary>
    /// VARTAB
    /// </summary>
    public string? VARTAB { get; set; }

    /// <summary>
    /// AMM_EFF
    /// </summary>
    public string? AMM_EFF { get; set; }

    /// <summary>
    /// IPC_EFF
    /// </summary>
    public string? IPC_EFF { get; set; }

    /// <summary>
    /// 所属机队Id
    /// </summary>
    public long PlaneFleetId { get; set; }

}

[ExcelExporter(Name = "Sheet1")]
public class PlaneComponentImportDto
{
    /// <summary>
    /// 件号
    /// </summary>
    [ImporterHeader(Name = "件号")]
    [ExporterHeader(DisplayName = "件号")]
    public string? PartNum { get; set; }

    /// <summary>
    /// CMM
    /// </summary>
    [ImporterHeader(Name = "CMM")]
    [ExporterHeader(DisplayName = "CMM")]
    public string CMM { get; set; }

    /// <summary>
    /// 有效性
    /// </summary>
    [ImporterHeader(Name = "有效性")]
    [ExporterHeader(DisplayName = "有效性")]
    public string? EFF { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [ImporterHeader(Name = "描述")]
    [ExporterHeader(DisplayName = "描述")]
    public string? Description { get; set; }

    /// <summary>
    /// 厂家
    /// </summary>
    [ImporterHeader(Name = "厂家")]
    [ExporterHeader(DisplayName = "厂家")]
    public string? FactoryName { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    [ImporterHeader(Name = "数量")]
    [ExporterHeader(DisplayName = "数量")]
    public int? Quantity { get; set; }
}
