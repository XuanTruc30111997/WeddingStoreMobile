using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == String.Empty)
            {
                //return ImageSource.FromResource("WeddingStoreMoblie.Images.noimage.png");
                return ImageSource.FromResource(Constant.ImagePatch + "noimage.png");
            }
            else
            {
                return ImageSource.FromStream(() => new MemoryStream(value as byte[]));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
