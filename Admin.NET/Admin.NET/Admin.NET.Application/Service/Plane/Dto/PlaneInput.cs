using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Admin.NET.Application;

public class PlaneComponentBaseInput
{
    public virtual long? Id { get; set; }

    public long? ComponentId { get; set; }

    [JsonProperty("cmm")]
    public string? CMM { get; set; }

    [JsonProperty("eff")]
    public string? EFF { get; set; }
}

/// <summary>
/// 飞机设置基础输入参数
/// </summary>
public class PlaneBaseInput
{
    /// <summary>
    /// 注册号
    /// </summary>
    public virtual string? RegisId { get; set; }

    /// <summary>
    /// 机型名
    /// </summary>
    public virtual string? PlaneModelName { get; set; }

    /// <summary>
    /// 机型号
    /// </summary>
    public virtual string? PlaneModelNo { get; set; }

    /// <summary>
    /// MSN
    /// </summary>
    [JsonProperty("msn")]
    public virtual string? MSN { get; set; }

    /// <summary>
    /// VARTAB
    /// </summary>
    [JsonProperty("vartab")]
    public virtual string? VARTAB { get; set; }

    /// <summary>
    /// AMM_EFF
    /// </summary>
    [JsonProperty("ammEFF")]
    public virtual string? AMM_EFF { get; set; }

    /// <summary>
    /// IPC_EFF
    /// </summary>
    [JsonProperty("ipcEFF")]
    public virtual string? IPC_EFF { get; set; }

    /// <summary>
    /// 所属机队Id
    /// </summary>
    public virtual long? PlaneFleetId { get; set; }

    /// <summary>
    /// 关键部件集合
    /// </summary>
    public virtual List<PlaneComponentBaseInput> Components { get; set; }
}

/// <summary>
/// 飞机设置分页查询输入参数
/// </summary>
public class PlaneInput : BasePageInput
{
    /// <summary>
    /// 注册号
    /// </summary>
    public string RegisId { get; set; }

    /// <summary>
    /// 机型名
    /// </summary>
    public string PlaneModelName { get; set; }

    /// <summary>
    /// 机型号
    /// </summary>
    public string PlaneModelNo { get; set; }

    /// <summary>
    /// MSN
    /// </summary>
    [JsonProperty("msn")]
    public string MSN { get; set; }

    /// <summary>
    /// VARTAB
    /// </summary>
    [JsonProperty("vartab")]
    public string VARTAB { get; set; }

    /// <summary>
    /// AMM_EFF
    /// </summary>
    [JsonProperty("ammEFF")]
    public string AMM_EFF { get; set; }

    /// <summary>
    /// IPC_EFF
    /// </summary>
    [JsonProperty("ipcEFF")]
    public string IPC_EFF { get; set; }

    /// <summary>
    /// 所属机队Id
    /// </summary>
    public long? PlaneFleetId { get; set; }
}

/// <summary>
/// 飞机设置增加输入参数
/// </summary>
public class AddPlaneInput : PlaneBaseInput
{
}

/// <summary>
/// 飞机设置删除输入参数
/// </summary>
public class DeletePlaneInput : BaseIdInput
{
}

/// <summary>
/// 飞机设置更新输入参数
/// </summary>
public class UpdatePlaneInput : PlaneBaseInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "主键Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// 飞机设置主键查询输入参数
/// </summary>
public class QueryByIdPlaneInput : DeletePlaneInput
{

}

public class UploadPlaneInput
{
    public IFormFile File { get; set; }
    public long PlaneFleetId { get; set; }
    public long? PlaneId { get; set; }
}

/// <summary>
/// 飞机部件分页查询输入参数
/// </summary>
public class PlaneComponentInput : BasePageInput
{
    public long? PlaneId { get; set; }

    public string? PartNum { get; set; }

    public string? CMM { get; set; }
}