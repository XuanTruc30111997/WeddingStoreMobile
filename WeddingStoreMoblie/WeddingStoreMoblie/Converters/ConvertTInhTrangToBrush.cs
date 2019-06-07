using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertTinhTrangToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tinhTrang = (int)value;
            if (tinhTrang == 0)
                return Color.Aqua;
            else
            {
                if (tinhTrang == 1)
                    return Color.Green;
                else
                    if (tinhTrang == 2)
                    return Color.Gray;
            }
            return Color.GreenYellow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
