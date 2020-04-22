using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace TimeTracker
{
    class HoursToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (System.Convert.ToInt32(value))
            {
                case 0:
                    return Brushes.Green;
                case 1:
                    return Brushes.Yellow;
                case 2:
                    return Brushes.Yellow;
                case 3:
                    return Brushes.Yellow;
                case 4:
                    return Brushes.Yellow;
                case 5:
                    return Brushes.Orange;
                case 6:
                    return Brushes.Orange;
                case 7:
                    return Brushes.Orange;
                case 8:
                    return Brushes.Red;
                default:
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
