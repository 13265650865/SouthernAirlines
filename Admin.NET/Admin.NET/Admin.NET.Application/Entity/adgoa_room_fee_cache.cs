using Admin.NET.Core;

namespace Admin.NET.Application.Entity;

/// <summary>
/// adgoa
/// </summary>
[SugarTable("adgoa_room_fee_cache", "adgoa")]
public class adgoa_room_fee_cache  : EntityBaseId
{
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? HotelId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? RoomId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? RoomName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? MaxOccupancy { get; set; }
    
    /// <summary>
    /// 1人入住pc价格
    /// </summary>
    [SugarColumn(ColumnDescription = "1人入住pc价格", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price { get; set; }
    
    /// <summary>
    /// 2入住pc价格
    /// </summary>
    [SugarColumn(ColumnDescription = "2入住pc价格", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_Two { get; set; }

    /// <summary>
    /// 最大入住pc价格
    /// </summary>
    [SugarColumn(ColumnDescription = "最大入住pc价格", Length = 10, DecimalDigits = 2)]
    public decimal?  PC_Price_Max { get; set; }

    /// <summary>
    /// 第n+1天的pc价格
    /// </summary>
    [SugarColumn(ColumnDescription = "第n+1天的pc价格", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_5 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_6 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_Price_N_Add_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? PC_Price_Consecutive_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? PC_Price_Consecutive_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_Two { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_Max { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_5 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_6 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_Price_N_Add_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? APP_Price_Consecutive_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? APP_Price_Consecutive_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_TotalPayment_Consecutive_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? APP_TotalPayment_Consecutive_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_TotalPayment_Consecutive_3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 10, DecimalDigits=2 )]
    public decimal? PC_TotalPayment_Consecutive_7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Status { get; set; }
    
}
