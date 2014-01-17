using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Dynamic;

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

            Gender f = (Gender)Enum.Parse(typeof(Gender), "Female");
            Console.WriteLine(f.GetDescriptionByName<Gender>());

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
