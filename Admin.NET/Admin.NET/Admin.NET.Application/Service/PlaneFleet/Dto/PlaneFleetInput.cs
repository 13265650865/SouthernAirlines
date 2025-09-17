using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 机队基础输入参数
    /// </summary>
    public class PlaneFleetBaseInput
    {
        /// <summary>
        /// 机队代号
        /// </summary>
        public virtual string Code { get; set; }
        
    }

    /// <summary>
    /// 机队分页查询输入参数
    /// </summary>
    public class PlaneFleetInput : BasePageInput
    {
        /// <summary>
        /// 机队代号
        /// </summary>
        public string Code { get; set; }
        
    }

    /// <summary>
    /// 机队增加输入参数
    /// </summary>
    public class AddPlaneFleetInput : PlaneFleetBaseInput
    {
    }

    /// <summary>
    /// 机队删除输入参数
    /// </summary>
    public class DeletePlaneFleetInput : BaseIdInput
    {
    }

    /// <summary>
    /// 机队更新输入参数
    /// </summary>
    public class UpdatePlaneFleetInput : PlaneFleetBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 机队主键查询输入参数
    /// </summary>
    public class QueryByIdPlaneFleetInput : DeletePlaneFleetInput
    {

    }
