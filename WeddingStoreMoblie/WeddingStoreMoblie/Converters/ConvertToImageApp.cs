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
            string myImage = value as string;
            if (String.IsNullOrEmpty(myImage))
            {
                return ImageSource.FromResource(Constant.ImagePatch + "noimage.png");
            }
            else
            {
                Console.WriteLine(Constant.ImagePatch + myImage);
                return ImageSource.FromResource(Constant.ImagePatch + myImage);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
