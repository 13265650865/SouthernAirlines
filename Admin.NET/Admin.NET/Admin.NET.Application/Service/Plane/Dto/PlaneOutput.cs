using Newtonsoft.Json;

namespace Admin.NET.Application;

/// <summary>
/// 飞机设置输出参数
/// </summary>
public class PlaneBaseOutput
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
   public string? PlaneModelNo { get; set; }

   /// <summary>
   /// MSN
   /// </summary>
   [JsonProperty("msn")]
   public string? MSN { get; set; }

   /// <summary>
   /// VARTAB
   /// </summary>
   [JsonProperty("vartab")]
   public string? VARTAB { get; set; }

   /// <summary>
   /// AMM_EFF
   /// </summary>
   [JsonProperty("ammEFF")]
   public string? AMM_EFF { get; set; }

   /// <summary>
   /// IPC_EFF
   /// </summary>
   [JsonProperty("ipcEFF")]
   public string? IPC_EFF { get; set; }

   /// <summary>
   /// 所属机队Id
   /// </summary>
   public long? PlaneFleetId { get; set; }

   /// <summary>
   /// 所属机队code
   /// </summary>
   public string PlaneFleetCode { get; set; }
}

public class PlaneComponentOutput
{
   public long Id { get; set; }

   public long? ComponentId { get; set; }

   public string? CMM { get; set; }

   public string? EFF { get; set; }

   /// <summary>
   /// 描述
   /// </summary>
   public string? ComponentDescription { get; set; }

   /// <summary>
   /// 件号
   /// </summary>
   public string PartNum { get; set; }

   /// <summary>
   /// 厂家
   /// </summary>
   public string? FactoryName { get; set; }
}

public class PlaneOutput : PlaneBaseOutput
{
   /// <summary>
   /// 关联部件集合
   /// </summary>
   public List<PlaneComponentOutput> Components { get; set; }
}