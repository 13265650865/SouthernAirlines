using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 部件类别基础输入参数
    /// </summary>
    public class ComponentCategoryBaseInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string? Name { get; set; }
        
    }

    /// <summary>
    /// 部件类别分页查询输入参数
    /// </summary>
    public class ComponentCategoryInput : BasePageInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        
    }

    /// <summary>
    /// 部件类别增加输入参数
    /// </summary>
    public class AddComponentCategoryInput : ComponentCategoryBaseInput
    {
    }

    /// <summary>
    /// 部件类别删除输入参数
    /// </summary>
    public class DeleteComponentCategoryInput : BaseIdInput
    {
    }

    /// <summary>
    /// 部件类别更新输入参数
    /// </summary>
    public class UpdateComponentCategoryInput : ComponentCategoryBaseInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 部件类别主键查询输入参数
    /// </summary>
    public class QueryByIdComponentCategoryInput : DeleteComponentCategoryInput
    {

    }
