namespace Admin.NET.Application;

/// <summary>
/// agdoa数据输出参数
/// </summary>
public class adgoa_room_fee_cacheOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// HotelId
    /// </summary>
    public string? HotelId { get; set; }

    /// <summary>
    /// RoomId
    /// </summary>
    public string? RoomId { get; set; }

    /// <summary>
    /// RoomName
    /// </summary>
    public string? RoomName { get; set; }

    /// <summary>
    /// MaxOccupancy
    /// </summary>
    public int? MaxOccupancy { get; set; }

    /// <summary>
    /// 1人入住pc价格
    /// </summary>
    public decimal? PC_Price { get; set; }

    /// <summary>
    /// 2入住pc价格
    /// </summary>
    public decimal? PC_Price_Two { get; set; }
    /// <summary>
    /// 最大入住pc价格
    /// </summary>
    public decimal? PC_Price_Max { get; set; }

    /// <summary>
    /// 21日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_1 { get; set; }

    /// <summary>
    /// 22日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_2 { get; set; }

    /// <summary>
    /// 23日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_3 { get; set; }

    /// <summary>
    /// 24日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_4 { get; set; }

    /// <summary>
    /// 25日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_5 { get; set; }

    /// <summary>
    /// 26日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_6 { get; set; }

    /// <summary>
    /// 27日的pc价格
    /// </summary>
    public decimal? PC_Price_N_Add_7 { get; set; }

    /// <summary>
    /// 28日的pc价格
    /// </summary>
    public string? PC_Price_Consecutive_3 { get; set; }

    /// <summary>
    /// 29日的pc价格
    /// </summary>
    public string? PC_Price_Consecutive_7 { get; set; }

    /// <summary>
    /// 20日app价格【1人入住】
    /// </summary>
    public decimal? APP_Price { get; set; }

    /// <summary>
    /// 20日app价格【2人入住】
    /// </summary>
    public decimal? APP_Price_Two { get; set; }

    /// <summary>
    /// 20日app价格【最大入住】
    /// </summary>
    public decimal? APP_Price_Max { get; set; }

    /// <summary>
    /// 21日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_1 { get; set; }

    /// <summary>
    /// 22日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_2 { get; set; }

    /// <summary>
    /// 23日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_3 { get; set; }

    /// <summary>
    /// 24日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_4 { get; set; }

    /// <summary>
    /// 25日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_5 { get; set; }

    /// <summary>
    /// 26日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_6 { get; set; }

    /// <summary>
    /// 27日app价格
    /// </summary>
    public decimal? APP_Price_N_Add_7 { get; set; }

    /// <summary>
    /// APP_Price_Consecutive_3
    /// </summary>
    public string? APP_Price_Consecutive_3 { get; set; }

    /// <summary>
    /// APP_Price_Consecutive_7
    /// </summary>
    public string? APP_Price_Consecutive_7 { get; set; }

    /// <summary>
    /// 20日-23日连住app总价
    /// </summary>
    public decimal? APP_TotalPayment_Consecutive_3 { get; set; }

    /// <summary>
    /// 20日-27日连住app总价
    /// </summary>
    public decimal? APP_TotalPayment_Consecutive_7 { get; set; }

    /// <summary>
    /// 20日-23日连住三天pc总价
    /// </summary>
    public decimal? PC_TotalPayment_Consecutive_3 { get; set; }

    /// <summary>
    /// 20日-27日连住七天pc总价
    /// </summary>
    public decimal? PC_TotalPayment_Consecutive_7 { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public int? Status { get; set; }

}


