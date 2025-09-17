// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class traveloko_requestDto
{
   
        /// <summary>
        /// 
        /// </summary>
        public string hid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int roomcount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int children { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int channel { get; set; }
 
}


public class t_room_price
{

    public Dictionary<string, List<HotelItem>> Result { get; set; }
}
public class HotelItem
{

    public string modifyTime { get; set; }
    public string RoomId { get; set; }

    public string RoomName { get; set; }
    public string RoomNameEn { get; set; }
    public int WiredBroadband { get; set; }
    public int WirelessWideband { get; set; }
    public int MaxOccupancy { get; set; }
    public int BedType { get; set; }
    public string BedTypeDesc { get; set; }
    public int Window { get; set; }
    public int BathRoomType { get; set; }
    public string Area { get; set; }
    public List<RatePlansItem> RatePlans { get; set; }
}

public class RatePlansItem
{
    public string RatePlanId { get; set; }

    public int CustomerType { get; set; }

    public int InvoiceType { get; set; }

    public bool HourlyRoom { get; set; }

    public int Available { get; set; }

    public int MealType { get; set; }

    public int Breakfast { get; set; }

    public List<CancelRules> CancelRules { get; set; }

    public decimal CommissionRate { get; set; }

    public int Currency { get; set; }

    public int Quantity { get; set; }
    public decimal MemberPrice { get; set; }


    public int MaxOccupancy { get; set; }

    public decimal BasePrice { get; set; }

    public List<Dailys> Dailys { get; set; }

}

public class CancelRules
{

    public DateTime LastCancelTime { get; set; }
    public int DeductType { get; set; }
    public decimal DeductValue { get; set; }
    public DateTime EndTimeOrig { get; set; }

    public string DescOrig { get; set; }
}
public class Dailys
{
    public int Available { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }

    public decimal MemberPrice { get; set; }

    public decimal BasePrice { get; set; }

    public int Currency { get; set; }
    public int PayType { get; set; }
    public int DistributeMode { get; set; }
    public int MealType { get; set; }

    public string DateStr { get; set; }
}
