using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// agdoa数据基础输入参数
    /// </summary>
    public class adgoa_room_fee_cacheBaseInput
    {
        /// <summary>
        /// HotelId
        /// </summary>
        public virtual string? HotelId { get; set; }
        
        /// <summary>
        /// RoomId
        /// </summary>
        public virtual string? RoomId { get; set; }
        
        /// <summary>
        /// RoomName
        /// </summary>
        public virtual string? RoomName { get; set; }
        
        /// <summary>
        /// MaxOccupancy
        /// </summary>
        public virtual int? MaxOccupancy { get; set; }
        
        /// <summary>
        /// 1人入住pc价格
        /// </summary>
        public virtual decimal? PC_Price { get; set; }
        
        /// <summary>
        /// 2入住pc价格
        /// </summary>
        public virtual decimal? PC_Price_Two { get; set; }
        
        /// <summary>
        /// 21日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_1 { get; set; }
        
        /// <summary>
        /// 22日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_2 { get; set; }
        
        /// <summary>
        /// 23日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_3 { get; set; }
        
        /// <summary>
        /// 24日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_4 { get; set; }
        
        /// <summary>
        /// 25日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_5 { get; set; }
        
        /// <summary>
        /// 26日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_6 { get; set; }
        
        /// <summary>
        /// 27日的pc价格
        /// </summary>
        public virtual decimal? PC_Price_N_Add_7 { get; set; }
        
        /// <summary>
        /// 28日的pc价格
        /// </summary>
        public virtual string? PC_Price_Consecutive_3 { get; set; }
        
        /// <summary>
        /// 29日的pc价格
        /// </summary>
        public virtual string? PC_Price_Consecutive_7 { get; set; }
        
        /// <summary>
        /// 20日app价格【1人入住】
        /// </summary>
        public virtual decimal? APP_Price { get; set; }
        
        /// <summary>
        /// 20日app价格【2人入住】
        /// </summary>
        public virtual decimal? APP_Price_Two { get; set; }
        
        /// <summary>
        /// 20日app价格【最大入住】
        /// </summary>
        public virtual decimal? APP_Price_Max { get; set; }
        
        /// <summary>
        /// 21日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_1 { get; set; }
        
        /// <summary>
        /// 22日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_2 { get; set; }
        
        /// <summary>
        /// 23日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_3 { get; set; }
        
        /// <summary>
        /// 24日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_4 { get; set; }
        
        /// <summary>
        /// 25日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_5 { get; set; }
        
        /// <summary>
        /// 26日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_6 { get; set; }
        
        /// <summary>
        /// 27日app价格
        /// </summary>
        public virtual decimal? APP_Price_N_Add_7 { get; set; }
        
        /// <summary>
        /// APP_Price_Consecutive_3
        /// </summary>
        public virtual string? APP_Price_Consecutive_3 { get; set; }
        
        /// <summary>
        /// APP_Price_Consecutive_7
        /// </summary>
        public virtual string? APP_Price_Consecutive_7 { get; set; }
        
        /// <summary>
        /// 20日-23日连住app总价
        /// </summary>
        public virtual decimal? APP_TotalPayment_Consecutive_3 { get; set; }
        
        /// <summary>
        /// 20日-27日连住app总价
        /// </summary>
        public virtual decimal? APP_TotalPayment_Consecutive_7 { get; set; }
        
        /// <summary>
        /// 20日-23日连住三天pc总价
        /// </summary>
        public virtual decimal? PC_TotalPayment_Consecutive_3 { get; set; }
        
        /// <summary>
        /// 20日-27日连住七天pc总价
        /// </summary>
        public virtual decimal? PC_TotalPayment_Consecutive_7 { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public virtual int? Status { get; set; }
        
    }

    /// <summary>
    /// agdoa数据分页查询输入参数
    /// </summary>
    public class adgoa_room_fee_cacheInput : BasePageInput
    {
        /// <summary>
        /// HotelId
        /// </summary>
        public string? HotelId { get; set; }
        
        /// <summary>
        /// RoomId
        /// </summary>
        public string? RoomId { get; set; }
        
        /// <summary>
        /// MaxOccupancy
        /// </summary>
        public int? MaxOccupancy { get; set; }
        
    }

    /// <summary>
    /// agdoa数据增加输入参数
    /// </summary>
    public class Addadgoa_room_fee_cacheInput : adgoa_room_fee_cacheBaseInput
    {
    }

    /// <summary>
    /// agdoa数据删除输入参数
    /// </summary>
    public class Deleteadgoa_room_fee_cacheInput : BaseIdInput
    {
    }

    /// <summary>
    /// agdoa数据更新输入参数
    /// </summary>
    public class Updateadgoa_room_fee_cacheInput : adgoa_room_fee_cacheBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// agdoa数据主键查询输入参数
    /// </summary>
    public class QueryByIdadgoa_room_fee_cacheInput : Deleteadgoa_room_fee_cacheInput
    {

    }
