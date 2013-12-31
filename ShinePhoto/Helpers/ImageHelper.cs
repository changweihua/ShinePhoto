using System;
using System.Drawing.Imaging;
using System.Drawing;
using ShinePhoto.Models;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShinePhoto.Helpers
{
    /// <summary>
    /// 图片信息帮助类
    /// </summary>
    public class ImageHelper
    {

        #region 控件截图

        public enum ImageFormat { JPG, BMP, PNG, GIF, TIF }

        public static void SaveToImage(FrameworkElement element, string fileName, ImageFormat format)
        {
            using (FileStream fs = new FileStream(@fileName, FileMode.Create))
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96.0, 96.0, System.Windows.Media.PixelFormats.Pbgra32);
                bmp.Render(element);

                BitmapEncoder encoder = null;

                switch (format)
                {
                    case ImageFormat.JPG:
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ImageFormat.PNG:
                        encoder = new PngBitmapEncoder();
                        break;
                    case ImageFormat.BMP:
                        encoder = new BmpBitmapEncoder();
                        break;
                    case ImageFormat.GIF:
                        encoder = new GifBitmapEncoder();
                        break;
                    case ImageFormat.TIF:
                        encoder = new TiffBitmapEncoder();
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);

            }
           
        }

        #endregion

        #region 获取图片 Exif 信息

        /// <summary>
        /// 获取图片 Exif 信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ExifModel FindExifinfo(string filePath)
        {
            ExifModel exif = new ExifModel();
            Image img = Image.FromFile(filePath);

            PropertyItem[] pt = img.PropertyItems;

            for (int i = 0; i < pt.Length; i++)
            {
                PropertyItem p = pt[i];

                switch (pt[i].Id)
                {
                    // 设备制造商 20. 
                    case 0x010F:
                        exif.Manufacturer = System.Text.ASCIIEncoding.ASCII.GetString(pt[i].Value);
                        break;
                    // 设备型号 25. 
                    case 0x0110:
                        exif.DeviceType = GetValueOfType2(p.Value);
                        break;
                    // 拍照时间 30.
                    case 0x0132:
                        exif.ShootTime = GetValueOfType2(p.Value);
                        break;
                    // .曝光时间 
                    case 0x829A:
                        exif.ExposureTime = GetValueOfType5(p.Value) + " sec";
                        break;
                    // ISO 40.  
                    case 0x8827:
                        exif.ISO = GetValueOfType3(p.Value);
                        break;
                    // 图像说明info.description
                    case 0x010E:
                        exif.Description = GetValueOfType2(p.Value);
                        break;
                    //相片的焦距
                    case 0x920a:
                        exif.FocalLength = GetValueOfType5A(p.Value) + " mm";
                        break;
                    //相片的光圈值
                    case 0x829D:
                        exif.Aperture = GetValueOfType5A(p.Value);
                        break;
                    default:
                        break;

                }

            }

            return exif;
        }

        public static string GetValueOfType2(byte[] b)// 对type=2 的value值进行读取
        {
            return System.Text.Encoding.ASCII.GetString(b);
        }
        private static string GetValueOfType3(byte[] b) //对type=3 的value值进行读取
        {
            if (b.Length != 2) return "unknow";
            return Convert.ToUInt16(b[1] << 8 | b[0]).ToString();
        }
        private static string GetValueOfType5(byte[] b) //对type=5 的value值进行读取
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            return fm.ToString() + "/" + fz.ToString() + " sec";
        }
        private static string GetValueOfType5A(byte[] b)//获取光圈的值
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            double temp = (double)fm / fz;
            return (temp).ToString();
        }

        #endregion

      
    }
}
