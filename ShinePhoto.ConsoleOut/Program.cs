using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinePhoto.ConsoleOut
{
    class Program
    {
        static void Main(string[] args)
        {
            var flag = System.Text.RegularExpressions.Regex.IsMatch("{pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.quill.png}", @"light");

            Console.WriteLine(flag);

            var result = System.Text.RegularExpressions.Regex.Replace("{pack://application:,,,/ShinePhoto.Icons;Component/light/appbar.quill.png}", "light", "dark");

            Console.WriteLine(result);

            Console.ReadKey(true);
        }
    }
}
