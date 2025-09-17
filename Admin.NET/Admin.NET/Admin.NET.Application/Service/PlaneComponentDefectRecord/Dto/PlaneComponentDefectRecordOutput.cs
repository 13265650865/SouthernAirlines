using Newtonsoft.Json;

namespace Admin.NET.Application;

/// <summary>
/// 飞机部件缺陷记录输出参数
/// </summary>
public class PlaneComponentDefectRecordOutput
{
   /// <summary>
   /// 主键Id
   /// </summary>
   public long Id { get; set; }

   public long? PlaneFleetId { get; set; }

   /// <summary>
   /// 机队代号
   /// </summary>
   public string PlaneFleetCode { get; set; }

   /// <summary>
   /// 飞机Id
   /// </summary>
   public long PlaneId { get; set; }

   /// <summary>
   /// 飞机注册号
   /// </summary>
   public string PlaneRegisId { get; set; }

   /// <summary>
   /// 部件Id
   /// </summary>
   public long ComponentId { get; set; }

   /// <summary>
   /// 部件件号
   /// </summary>
   public string ComponentPartNum { get; set; }

   /// <summary>
   /// 部件CMM
   /// </summary>
   [JsonProperty("cmm")]
   public string CMM { get; set; }

   /// <summary>
   /// 部件中文缺陷描述
   /// </summary>
   public string? ChineseDefectDescription { get; set; }

   public List<PlaneComponentDefectRecordDetailOutput> Details { get; set; }
}

public class PlaneComponentDefectRecordDetailOutput
{
   /// <summary>
   /// Id
   /// </summary>
   public long Id { get; set; }

   /// <summary>
   /// 记录Id
   /// </summary>
   public long RecordId { get; set; }

   /// <summary>
   /// 部件英文缺陷描述
   /// </summary>
   public string? EnglishDefectDescription { get; set; }

   /// <summary>
   /// 部件描述
   /// </summary>
   public string? ComponenDescription { get; set; }

   /// <summary>
   /// 替换部件Id
   /// </summary>
   public long? ReplaceComponentId { get; set; }

   /// <summary>
   /// 替换部件件号
   /// </summary>
   public string ReplaceComponentPartNum { get; set; }

   /// <summary>
   /// 数量
   /// </summary>
   public int? Quantity { get; set; }

   /// <summary>
   /// 单位
   /// </summary>
   public string? Unit { get; set; }
}