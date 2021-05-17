using System;

namespace CloudWeb.Dto.Common
{

    /// <summary>
    /// 基础dto
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 创建者id
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 修改者id
        /// </summary>
        public int Modifier { get; set; }

    }
}
