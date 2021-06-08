using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 显示状态
    /// </summary>
    public enum ShowChange
    {
        /// <summary>
        /// 显示
        /// </summary>
        show = 0,
        /// <summary>
        /// 隐藏
        /// </summary>
        display = 1
    }

    /// <summary>
    /// 栏目所对应组件
    /// 针对仿真实验云内页
    /// </summary>
    public enum ModuleType
    {
        /// <summary>
        /// 新闻报导与教学示范案例
        /// </summary>
        [Display(Name = "新闻报导与教学示范案例")]
        News = 1,

        /// <summary>
        /// 实验教材
        /// </summary>
        [Display(Name = "实验教材")]
        ExperimentManual = 2,

        /// <summary>
        /// 实验教学刊物与论文发展
        /// </summary>
        [Display(Name = "实验教学刊物与论文发展")]
        TeachingJournals = 3,

        /// <summary>
        /// 物理实验仪器会展
        /// </summary>
        [Display(Name = "物理实验仪器会展")]
        InstrumentExhibition = 4
    }

}
