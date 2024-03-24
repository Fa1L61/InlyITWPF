using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlyITWPF.Models
{
    public class DoubleConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if (double.TryParse(value as string, out result))
            {
                return result;
            }
            return 0.0;
        }
    }
}
