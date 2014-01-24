using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;

namespace ShinePhoto.Helpers
{
    public class PrinterHelper
    { 
        /// <summary>
        /// 获取局域网打印机列表
        /// </summary>
        /// <param name="DefaultPrinter">默认打印机</param>
        /// <returns>局域网中所有打印机列表</returns>
        public static List<string> GetPrinter(out string DefaultPrinter)
        {
            List<string> list = new List<string>();
            PrintDocument print = new PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;   //默认打印机名
            DefaultPrinter = sDefault;
            foreach (string sPrint in PrinterSettings.InstalledPrinters)    //获取所有打印机名称
            {
                list.Add(sPrint);
            }
            return list;
        }
    }
}
