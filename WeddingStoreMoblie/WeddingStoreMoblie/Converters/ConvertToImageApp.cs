using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertToImageApp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value as String))
            {
                //return ImageSource.FromResource("WeddingStoreMoblie.Images.noimage.png");
                return ImageSource.FromResource(Constant.ImagePatch + "noimage.png");
            }
            else
                return ImageSource.FromResource(Constant.ImagePatch + "" + value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
