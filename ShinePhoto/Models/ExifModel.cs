using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinePhoto.Models
{
    /// <summary>
    /// EXIF 信息类
    /// </summary>
    public class ExifModel
    {
        /// <summary>
        /// 设备制造商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 拍照时间
        /// </summary>
        public string ShootTime { get; set; }
        /// <summary>
        /// 曝光时间
        /// </summary>
        public string ExposureTime { get; set; }
        /// <summary>
        /// ISO 值
        /// </summary>
        public string ISO { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 焦距
        /// </summary>
        public string FocalLength { get; set; }
        /// <summary>
        /// 光圈
        /// </summary>
        public string Aperture { get; set; }
    }
}
