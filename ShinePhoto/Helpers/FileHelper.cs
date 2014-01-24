using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace ShinePhoto.Helpers
{
    public class FileHelper
    {
        public bool DeleteFileTo()
        {
            Log4netLogger logger = new Log4netLogger(typeof(FileHelper));

            logger.Info("删除文件到回收站");
            string filepath = @"C:\Users\ChangWeihua\Desktop\OfficeTab.xaml";
            FileSystem.DeleteFile(filepath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            logger.Info("删除文件完成");

            logger.Info("删除文件夹到回收站");
            string dirpath = @"C:\Users\ChangWeihua\Desktop\images";
            FileSystem.DeleteDirectory(dirpath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            logger.Info("删除文件夹完成");

            return true;
        }
    }
}
