using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace ShinePhoto.Helpers
{
    /// <summary>
    /// 通过 WMI 查询本地计算机信息
    /// </summary>
    public class SystemInfoHelper
    { /// <summary>
        /// 获取操作系统序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSerialNumber()
        {
            string result = "";
            ManagementClass mClass = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moCollection = mClass.GetInstances();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["SerialNumber"].ToString() + " ";
            }
            return result;
        }
        /// <summary>
        /// 查询CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            string result = "";
            ManagementClass mClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moCollection = mClass.GetInstances();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["ProcessorId"].ToString() + " ";
            }
            return result;
        }
        /// <summary>
        /// 查询硬盘编号
        /// </summary>
        /// <returns></returns>
        public static string GetMainHardDiskId()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["SerialNumber"].ToString() + " ";
            }
            return result;
        }

        /// <summary>
        /// 主板编号
        /// </summary>
        /// <returns></returns>
        public static string GetMainBoardId()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root/CIMV2",
                    "SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["SerialNumber"].ToString() + " ";
            }
            return result;
        }

        /// <summary>
        /// 主板编号
        /// </summary>
        /// <returns></returns>
        public static string GetNetworkAdapterId()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL)AND (Manufacturer <> 'Microsoft'))");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["MACAddress"].ToString() + " ";
            }
            return result;
        }

        /// <summary>
        /// 主板编号
        /// </summary>
        /// <returns></returns>
        public static string GetGroupName()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root/CIMV2", "SELECT * FROM Win32_Group");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                result += mObject["Name"].ToString() + " ";
            }
            return result;
        }

        /// <summary>
        /// 获取本地驱动器信息
        /// </summary>
        /// <returns></returns>
        public static string GetDriverInfo()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root/CIMV2", "SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection moCollection = searcher.Get();
            foreach (ManagementObject mObject in moCollection)
            {
                //mObject["DriveType"]共有6中可能值，分别代表如下意义：
                //1:No type   2:Floppy disk   3:Hard disk
                //4:Removable drive or network drive   5:CD-ROM   6:RAM disk
                //本处只列出固定驱动器（硬盘分区）的情况
                if (mObject["DriveType"].ToString() == "3")
                {
                    result += string.Format("Name={0},FileSystem={1},Size={2},FreeSpace={3} ", mObject["Name"].ToString(),
                        mObject["FileSystem"].ToString(), mObject["Size"].ToString(), mObject["FreeSpace"].ToString());
                }
            }
            return result;
        }
    }
}
