using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Dynamic;
using JCS;

namespace ShinePhoto.ConsoleOut
{
    class Program
    {
        static void Main(string[] args)
        {
            //var flag = System.Text.RegularExpressions.Regex.IsMatch("{pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.quill.png}", @"light");

            //Console.WriteLine(flag);

            //var result = System.Text.RegularExpressions.Regex.Replace("{pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.quill.png}", "light", "dark");

            //Console.WriteLine(result);

            //var str = @"C:\Users\ChangWeihua\Pictures\Eye-Fi\2013-12-13";
            //Console.WriteLine(str.Substring(str.LastIndexOf('\\')));

            //Gender f = (Gender)Enum.Parse(typeof(Gender), "Female");
            //Console.WriteLine(f.GetDescriptionByName<Gender>());

            string name = "Microsoft XPS Document Writer";

            PrinterHelper.GetPrinter(out name).ForEach(_ => Console.WriteLine(_));
            Console.WriteLine("------------------------------------------------");
            EnumPrintersCLass.MyEnumPrinters(EnumPrintersCLass.PrinterEnumFlags.PRINTER_ENUM_CONNECTIONS).ToList().ForEach(_ => Console.WriteLine(_.pName));

            StringBuilder sb = new StringBuilder(String.Empty);
            sb.AppendLine("Operation System Information");
            sb.AppendLine("----------------------------");
            sb.AppendLine(String.Format("Name = {0}", OSVersionInfo.Name));
            sb.AppendLine(String.Format("Edition = {0}", OSVersionInfo.Edition));
            if (OSVersionInfo.ServicePack != string.Empty)
                sb.AppendLine(String.Format("Service Pack = {0}", OSVersionInfo.ServicePack));
            else
                sb.AppendLine("Service Pack = None");
            sb.AppendLine(String.Format("Version = {0}", OSVersionInfo.VersionString));
            sb.AppendLine(String.Format("ProcessorBits = {0}", OSVersionInfo.ProcessorBits));
            sb.AppendLine(String.Format("OSBits = {0}", OSVersionInfo.OSBits));
            sb.AppendLine(String.Format("ProgramBits = {0}", OSVersionInfo.ProgramBits));

            Console.WriteLine(sb.ToString()); 

            Console.ReadKey(true);
        }
    }

    enum Gender
    {
        [Description("男")]
        Male,
        [Description("女")]
        Female
    }



    public static class EnumExtension
    {
        public static List<dynamic> GetAllItems(this Type enumName)
        {
            List<dynamic> list = new List<dynamic>();
            // get enum fileds
            FieldInfo[] fields = enumName.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                {
                    continue;
                }
                // get enum value
                int value = (int)enumName.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                string text = field.Name;
                string description = string.Empty;
                object[] array = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (array.Length > 0)
                {
                    description = ((DescriptionAttribute)array[0]).Description;
                }
                else
                {
                    description = ""; //none description,set empty
                }
                //add to list
                dynamic obj = new ExpandoObject();
                obj.Value = value;
                obj.Text = text;
                obj.Description = description;
                list.Add(obj);
            }
            return list;
        }

        public static string GetDescriptionByName<T>(this T enumItemName)
        {
            FieldInfo fi = enumItemName.GetType().GetField(enumItemName.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return enumItemName.ToString();
            }
        }
    }

}
