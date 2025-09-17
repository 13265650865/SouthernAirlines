using Admin.NET.Core;

namespace Admin.NET.Application.Entity;

/// <summary>
/// 机队
/// </summary>
[SugarTable("base_plane_fleet","机队")]
public class PlaneFleet  : EntityBase
{
    /// <summary>
    /// 机队代号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "机队代号", Length = 50)]
    public string Code { get; set; }
    
}
