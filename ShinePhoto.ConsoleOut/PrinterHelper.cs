using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace ShinePhoto.ConsoleOut
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


    public enum Location
    {
        LAN,
        WAN,
        LOCAL
    }

    public class EnumPrintersCLass
    {
        [FlagsAttribute]
        public enum PrinterEnumFlags
        {
            PRINTER_ENUM_DEFAULT = 0x00000001,
            PRINTER_ENUM_LOCAL = 0x00000002,
            PRINTER_ENUM_CONNECTIONS = 0x00000004,
            PRINTER_ENUM_FAVORITE = 0x00000004,
            PRINTER_ENUM_NAME = 0x00000008,
            PRINTER_ENUM_REMOTE = 0x00000010,
            PRINTER_ENUM_SHARED = 0x00000020,
            PRINTER_ENUM_NETWORK = 0x00000040,
            PRINTER_ENUM_EXPAND = 0x00004000,
            PRINTER_ENUM_CONTAINER = 0x00008000,
            PRINTER_ENUM_ICONMASK = 0x00ff0000,
            PRINTER_ENUM_ICON1 = 0x00010000,
            PRINTER_ENUM_ICON2 = 0x00020000,
            PRINTER_ENUM_ICON3 = 0x00040000,
            PRINTER_ENUM_ICON4 = 0x00080000,
            PRINTER_ENUM_ICON5 = 0x00100000,
            PRINTER_ENUM_ICON6 = 0x00200000,
            PRINTER_ENUM_ICON7 = 0x00400000,
            PRINTER_ENUM_ICON8 = 0x00800000,
            PRINTER_ENUM_HIDE = 0x01000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PRINTER_INFO_1
        {
            int flags;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDescription;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pComment;
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool EnumPrinters(PrinterEnumFlags Flags, string Name, uint Level, IntPtr pPrinterEnum, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        public static PRINTER_INFO_1[] MyEnumPrinters(PrinterEnumFlags Flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            IntPtr pPrInfo4 = IntPtr.Zero;
            uint size = 0;

            if (EnumPrinters(Flags, null, 1, IntPtr.Zero, size, ref cbNeeded, ref cReturned))
            {
                return new PRINTER_INFO_1[] { };
            }
            if (cbNeeded != 0)
            {
                pPrInfo4 = Marshal.AllocHGlobal((int)cbNeeded + 128);
                size = cbNeeded + 128;
                EnumPrinters(Flags, null, 1, pPrInfo4, size, ref cbNeeded, ref cReturned);
                if (cReturned != 0)
                {
                    PRINTER_INFO_1[] printerInfo1 = new PRINTER_INFO_1[cReturned];
                    int offset = pPrInfo4.ToInt32();
                    Type type = typeof(PRINTER_INFO_1);
                    int increment = Marshal.SizeOf(type);
                    for (int i = 0; i < cReturned; i++)
                    {
                        printerInfo1[i] = (PRINTER_INFO_1)Marshal.PtrToStructure(new IntPtr(offset), type);
                        offset += increment;
                    }
                    Marshal.FreeHGlobal(pPrInfo4);
                    return printerInfo1;
                }
            }

            return new PRINTER_INFO_1[] { };
        }

        public static void Check()
        {
            PRINTER_INFO_1[] printers = MyEnumPrinters(PrinterEnumFlags.PRINTER_ENUM_REMOTE);
            foreach (PRINTER_INFO_1 printer in printers)
            {
                if (-1 == printer.pName.IndexOf("!!"))
                {
                    Console.WriteLine(printer.pName);
                }
                else
                {
                    uint cbNeeded = 0;
                    uint cReturned = 0;
                    IntPtr pPrInfo4 = IntPtr.Zero;
                    uint size = 0;
                    string pNewName = printer.pName;
                    EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_NAME, pNewName, 1, IntPtr.Zero, size, ref cbNeeded, ref cReturned);
                    if (cbNeeded != 0)
                    {
                        pPrInfo4 = Marshal.AllocHGlobal((int)cbNeeded + 128);
                        size = cbNeeded + 128;
                        EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_NAME, pNewName, 1, pPrInfo4, size, ref cbNeeded, ref cReturned);
                        PRINTER_INFO_1[] printerInfo1 = new PRINTER_INFO_1[cReturned];
                        int offset = pPrInfo4.ToInt32();
                        Type type = typeof(PRINTER_INFO_1);
                        int increment = Marshal.SizeOf(type);
                        for (int i = 0; i < cReturned; i++)
                        {
                            printerInfo1[i] = (PRINTER_INFO_1)Marshal.PtrToStructure(new IntPtr(offset), type);
                            offset += increment;
                            Console.WriteLine(printerInfo1[i].pName);
                        }
                        Marshal.FreeHGlobal(pPrInfo4);
                    }
                }
            }
        }
    }


}
