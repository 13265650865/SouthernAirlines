using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Nest;

namespace Admin.NET.Application;

/// <summary>
/// 机队输出参数
/// </summary>
public class PlaneFleetDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 机队代号
    /// </summary>
    public string Code { get; set; }
}

[ExcelExporter(Name = "Sheet1")]
public class PlaneFleetImportDto
{
    /// <summary>
    /// 机队
    /// </summary>
    [ImporterHeader(Name = "机队")]
    [ExporterHeader(DisplayName = "机队")]
    public string? PlaneFleetCode { get; set; }

    /// <summary>
    /// 注册号
    /// </summary>
    [ImporterHeader(Name = "注册号")]
    [ExporterHeader(DisplayName = "注册号")]
    public string? RegisId { get; set; }

    /// <summary>
    /// 机型名
    /// </summary>
    [ImporterHeader(Name = "机型")]
    [ExporterHeader(DisplayName = "机型")]
    public string? PlaneModelName { get; set; }

    /// <summary>
    /// 机型号
    /// </summary>
    [ImporterHeader(Name = "MODEL")]
    [ExporterHeader(DisplayName = "MODEL")]
    public string? PlaneModelNo { get; set; }

    /// <summary>
    /// MSN
    /// </summary>
    [ImporterHeader(Name = "MSN")]
    [ExporterHeader(DisplayName = "MSN")]
    public string? MSN { get; set; }

    /// <summary>
    /// LINENO
    /// </summary>
    [ImporterHeader(Name = "LINENO")]
    [ExporterHeader(DisplayName = "LINENO")]
    public string? LINENO { get; set; }

    /// <summary>
    /// VARTAB
    /// </summary>
    [ImporterHeader(Name = "VARTAB")]
    [ExporterHeader(DisplayName = "VARTAB")]
    public string? VARTAB { get; set; }

    /// <summary>
    /// ENG MODEL
    /// </summary>
    [ImporterHeader(Name = "ENG MODEL")]
    [ExporterHeader(DisplayName = "ENG MODEL")]
    public string? ENGMODEL { get; set; }

    /// <summary>
    /// ENGINE SN(original)
    /// </summary>
    [ImporterHeader(Name = "ENGINE SN(original)")]
    [ExporterHeader(DisplayName = "ENGINE SN(original)")]
    public string? ENGINESN { get; set; }

    /// <summary>
    /// APU MODEL
    /// </summary>
    [ImporterHeader(Name = "APU MODEL")]
    [ExporterHeader(DisplayName = "APU MODEL")]
    public string? APUMODEL { get; set; }

    /// <summary>
    /// AMM_EFF
    /// </summary>
    [ImporterHeader(Name = "AMM EFF")]
    [ExporterHeader(DisplayName = "AMM EFF")]
    public string? AMM_EFF { get; set; }

    /// <summary>
    /// IPC_EFF
    /// </summary>
    [ImporterHeader(Name = "IPC EFF")]
    [ExporterHeader(DisplayName = "IPC EFF")]
    public string? IPC_EFF { get; set; }

    /// <summary>
    /// 飞机到达日期
    /// </summary>
    [ImporterHeader(Name = "飞机到达日期")]
    [ExporterHeader(DisplayName = "飞机到达日期")]
    public DateTime? PlaneArrivalDate { get; set; }

    /// <summary>
    /// 厂家交付日期
    /// </summary>
    [ImporterHeader(Name = "厂家交付日期")]
    [ExporterHeader(DisplayName = "厂家交付日期")]
    public DateTime? FactoryDeliveryDate { get; set; }

    /// <summary>
    /// 起租日期
    /// </summary>
    [ImporterHeader(Name = "起租日期")]
    [ExporterHeader(DisplayName = "起租日期")]
    public DateTime? LeaseStartDate { get; set; }

    /// <summary>
    /// 应退租日期
    /// </summary>
    [ImporterHeader(Name = "应退租日期")]
    [ExporterHeader(DisplayName = "应退租日期")]
    public DateTime? LeaseEndDate { get; set; }

    /// <summary>
    /// 实际退租日期
    /// </summary>
    [ImporterHeader(Name = "实际退租日期")]
    [ExporterHeader(DisplayName = "实际退租日期")]
    public DateTime? LeaseActualEndDate { get; set; }

    /// <summary>
    /// 融资类型
    /// </summary>
    [ImporterHeader(Name = "融资类型")]
    [ExporterHeader(DisplayName = "融资类型")]
    public string? FinanceType { get; set; }

    /// <summary>
    /// 飞机持有人
    /// </summary>
    [ImporterHeader(Name = "飞机持有人")]
    [ExporterHeader(DisplayName = "飞机持有人")]
    public string? PlaneOwner { get; set; }

    /// <summary>
    /// 执管单位
    /// </summary>
    [ImporterHeader(Name = "执管单位")]
    [ExporterHeader(DisplayName = "执管单位")]
    public string? ManageUnit { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [ImporterHeader(Name = "状态")]
    [ExporterHeader(DisplayName = "状态")]
    public string? Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [ImporterHeader(Name = "备注")]
    [ExporterHeader(DisplayName = "备注")]
    public string? Remark { get; set; }
}