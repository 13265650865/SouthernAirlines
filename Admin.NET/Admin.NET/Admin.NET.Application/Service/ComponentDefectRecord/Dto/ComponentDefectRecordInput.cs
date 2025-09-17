using Newtonsoft.Json;

namespace Admin.NET.Application;

public class ComponentDefectRecordDetailBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 记录Id
    /// </summary>
    public long? RecordId { get; set; }

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
    /// 数量
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string? Unit { get; set; }
}

/// <summary>
/// 飞机部件缺陷记录基础输入参数
/// </summary>
public class ComponentDefectRecordBaseInput
{
    /// <summary>
    /// 飞机Id
    /// </summary>
    public virtual long PlaneId { get; set; }

    /// <summary>
    /// 部件Id
    /// </summary>
    public virtual long? ComponentId { get; set; }

    /// <summary>
    /// 部件CMM
    /// </summary>
    [JsonProperty("cmm")]
    public virtual string CMM { get; set; }

    /// <summary>
    /// 部件中文缺陷描述
    /// </summary>
    public virtual string? ChineseDefectDescription { get; set; }

    public virtual List<ComponentDefectRecordDetailBaseInput> Details { get; set; }
}

/// <summary>
/// 飞机部件缺陷记录分页查询输入参数
/// </summary>
public class ComponentDefectRecordInput : BasePageInput
{
    /// <summary>
    /// 部件CMM
    /// </summary>
    [JsonProperty("cmm")]
    public string CMM { get; set; }

    /// <summary>
    /// 部件件号
    /// </summary>
    public string ComponentPartNum { get; set; }

    /// <summary>
    /// 中文缺陷描述
    /// </summary>
    public string ChineseDefectDescription { get; set; }

    /// <summary>
    /// 替换部件件号
    /// </summary>
    public string ReplaceComponentPartNum { get; set; }
}

/// <summary>
/// 飞机部件缺陷记录增加输入参数
/// </summary>
public class AddComponentDefectRecordInput : ComponentDefectRecordBaseInput
{
}

/// <summary>
/// 飞机部件缺陷记录删除输入参数
/// </summary>
public class DeleteComponentDefectRecordInput : BaseIdInput
{
}

/// <summary>
/// 飞机部件缺陷记录更新输入参数
/// </summary>
public class UpdateComponentDefectRecordInput : ComponentDefectRecordBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// 飞机部件缺陷记录主键查询输入参数
/// </summary>
public class QueryByIdComponentDefectRecordInput : DeleteComponentDefectRecordInput
{

}
