namespace Admin.NET.Application.Entity;

/// <summary>
/// 飞机表
/// </summary>
[SugarTable("Base_Airplane","飞机表")]
public class Plane  : EntityBase
{
    /// <summary>
    /// 注册号
    /// </summary>
    [SugarColumn(ColumnDescription = "注册号", Length = 50)]
    public string? RegisId { get; set; }
    
    /// <summary>
    /// 机型名
    /// </summary>
    [SugarColumn(ColumnDescription = "机型名称", Length = 50)]
    public string? PlaneModelName { get; set; }
    
    /// <summary>
    /// 机型型号
    /// </summary>
    [SugarColumn(ColumnName = "PlaneModel", ColumnDescription = "Model", Length = 50)]
    public string? PlaneModelNo { get; set; }
    
    /// <summary>
    /// 制造商序列号
    /// </summary>
    [SugarColumn(ColumnDescription = "MSN", Length = 50)]
    public string? MSN { get; set; }
    
    /// <summary>
    /// VARTAB
    /// </summary>
    [SugarColumn(ColumnDescription = "VARTAB", Length = 50)]
    public string? VARTAB { get; set; }
    
    /// <summary>
    /// 飞机维护手册有效性
    /// </summary>
    [SugarColumn(ColumnDescription = "AMM_EFF", Length = 50)]
    public string? AMM_EFF { get; set; }
    
    /// <summary>
    /// 图解零件目录有效性
    /// </summary>
    [SugarColumn(ColumnDescription = "IPC_EFF", Length = 50)]
    public string? IPC_EFF { get; set; }
    
    /// <summary>
    /// 所属机队Id
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "所属机队Id")]
    public long PlaneFleetId { get; set; }
}
