using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertToImageLib : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ahihi = value as string;
            if (!string.IsNullOrEmpty(ahihi))
            {
                return ImageSource.FromResource("WeddingStoreMyData.Images." + ahihi + ".jpg");
            }
            return ImageSource.FromResource("WeddingStoreMyData.Images.noimage.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
