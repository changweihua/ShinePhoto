using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinePhoto.Models
{
    /// <summary>
    /// 图片文件模型
    /// </summary>
    public struct FileModel
    {
        /// <summary>
        /// 文件全路径
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }
    }
}
