using Newtonsoft.Json;

namespace Admin.NET.Application;

/// <summary>
/// 部件基础输入参数
/// </summary>
public class ComponentBaseInput
{
    /// <summary>
    /// 部件类别
    /// </summary>
    public virtual long ComponentCategoryId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 件号
    /// </summary>
    public virtual string PartNum { get; set; }

    /// <summary>
    /// 厂家
    /// </summary>
    public virtual string? FactoryName { get; set; }

    /// <summary>
    /// EFF
    /// </summary>
    [JsonProperty("eff")]
    public virtual string? EFF { get; set; }

    /// <summary>
    /// CMM
    /// </summary>
    [JsonProperty("cmm")]
    public virtual string? CMM { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public virtual int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public virtual string? Unit { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }

    public virtual int? Type { get; set; }
}

/// <summary>
/// 部件分页查询输入参数
/// </summary>
public class ComponentInput : BasePageInput
{
    /// <summary>
    /// 部件类别
    /// </summary>
    public long ComponentCategoryId { get; set; }

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
    /// EFF
    /// </summary>
    [JsonProperty("eff")]
    public string? EFF { get; set; }

    /// <summary>
    /// CMM
    /// </summary>
    [JsonProperty("cmm")]
    public string CMM { get; set; }

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

    /// <summary>
    /// 部件类型
    /// </summary>
    public int? Type { get; set; }
}

/// <summary>
/// 部件增加输入参数
/// </summary>
public class AddComponentInput : ComponentBaseInput
{
}

/// <summary>
/// 部件删除输入参数
/// </summary>
public class DeleteComponentInput : BaseIdInput
{
}

/// <summary>
/// 部件更新输入参数
/// </summary>
public class UpdateComponentInput : ComponentBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// 部件主键查询输入参数
/// </summary>
public class QueryByIdComponentInput : DeleteComponentInput
{

}
