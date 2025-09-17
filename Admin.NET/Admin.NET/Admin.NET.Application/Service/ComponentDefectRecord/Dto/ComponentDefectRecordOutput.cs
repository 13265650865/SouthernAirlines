using Newtonsoft.Json;

namespace Admin.NET.Application;

/// <summary>
/// 飞机部件缺陷记录输出参数
/// </summary>
public class ComponentDefectRecordOutput
{
   /// <summary>
   /// 主键Id
   /// </summary>
   public long Id { get; set; }

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

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    public List<ComponentDefectRecordDetailOutput> Details { get; set; }
}

public class ComponentDefectRecordDetailOutput
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
   public long ReplaceComponentId { get; set; }

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