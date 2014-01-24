using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ionic.Zip;

namespace ShinePhoto.Helpers
{
    public class ZipHelper
    {
        public static BackupResult ZipByFolderName(string folderName, string saveFileName)
        {
            using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
            {
                zip.AddDirectory(folderName);
                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                zip.Save(saveFileName);
            }

            return BackupResult.Success;
        }

        #region Ionic.Zip压缩文件
        //压缩方法一  
        public static void ExeCompOne(string folderName, string saveFileName, string password)
        {
            //ZipFile实例化一个压缩文件保存路径的一个对象zip  
            using (ZipFile zip = new ZipFile(@saveFileName, Encoding.Default))
            {
                if (string.IsNullOrEmpty(password))
                {
                    //加密压缩  
                    zip.Password = password;
                }
                //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)  
                //zip.AddDirectory(@"E:\\yangfeizai\\" + "12051214544443");
                //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept  
                //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");  
                zip.Save();
            }
        }
        //压缩方法二  
        public static void ExeCompTwo(string folderName, string saveFileName, string password)
        {
            string FileName = DateTime.Now.ToString("yyMMddHHmmssff");
            //ZipFile实例化一个对象zip  
            using (ZipFile zip = new ZipFile())
            {
                if (string.IsNullOrEmpty(password))
                {
                    //加密压缩  
                    zip.Password = password;
                }
                //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)  
                zip.AddDirectory(@folderName);
                //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept  
                //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");  
                //用zip对象中Save重载方法保存压缩的文件，参数为保存压缩文件的路径  
                zip.Save(@saveFileName);
            }
        }
        #endregion

        #region //删除压缩包中的文件
        //3.从zip文件中删除一个文件,注意无法直接删除一个文件夹  
        public static void ExeDelete(string fileName, List<string> list)
        {
            using (ZipFile zip = ZipFile.Read(@fileName))
            {
                //zip["Jayzai.xml"] = null;  
                //删除zip对象中的一个文件  
                foreach (var item in list)
                {
                    zip.RemoveEntry(item);
                }
                zip.Save();
            }
        }
        #endregion  

        //从zip文件中解压出一个文件  
        public static void ExeSingleDeComp(List<string> list, string password, string folderName, string fileName)
        {
            using (ZipFile zip = ZipFile.Read(@fileName))
            {
                if (string.IsNullOrEmpty(password))
                {
                    //加密压缩  
                    zip.Password = password;
                }
                //Extract解压zip文件包的方法，参数是保存解压后文件的路基  
                foreach (var item in list)
                {
                    zip[item].Extract(@folderName);
                }
                
            }
        }

        //从zip文件中解压全部文件  
        public static void ExeAllDeComp(string fileName, string password, string folderName)
        {
            using (ZipFile zip = ZipFile.Read(@fileName))
            {
                if (string.IsNullOrEmpty(password))
                {
                    //加密压缩  
                    zip.Password = password;
                }
                foreach (ZipEntry entry in zip)
                {
                    //Extract解压zip文件包的方法，参数是保存解压后文件的路基  
                    entry.Extract(@folderName);
                }
            }  
        }  
    }

    public enum BackupResult
    {
        Success,
        Error
    }
}
