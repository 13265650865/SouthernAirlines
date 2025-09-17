using Newtonsoft.Json;

namespace Admin.NET.Application;

/// <summary>
/// 部件输出参数
/// </summary>
public class ComponentOutput
{
   /// <summary>
   /// 主键Id
   /// </summary>
   public long Id { get; set; }

   /// <summary>
   /// 部件类别Id
   /// </summary>
   public long ComponentCategoryId { get; set; }

   /// <summary>
   /// 部件类别名
   /// </summary>
   public string ComponentCategoryName { get; set; }

   /// <summary>
   /// 描述
   /// </summary>
   public string? Description { get; set; }

   /// <summary>
   /// 件号
   /// </summary>
   public string PartNum { get; set; }

   /// <summary>
   /// 厂家
   /// </summary>
   public string? FactoryName { get; set; }

   /// <summary>
   /// 数量
   /// </summary>
   public int? Quantity { get; set; }

   /// <summary>
   /// 单位
   /// </summary>
   public string? Unit { get; set; }

   /// <summary>
   /// 备注
   /// </summary>
   public string? Remark { get; set; }

   public int? Type { get; set; }
}
