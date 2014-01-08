using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ShinePhoto.Converters
{
    public class ModuleModelIconConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var source = new System.Windows.Media.Imaging.BitmapImage(new Uri(value.ToString(), UriKind.RelativeOrAbsolute));

            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
