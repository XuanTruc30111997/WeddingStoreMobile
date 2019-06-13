using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                //return ImageSource.FromResource("WeddingStoreApp.Images.GroupExpand.png");
                return ImageSource.FromResource(Constant.ImagePatch + "GroupExpand.png");
            else
                //return ImageSource.FromResource("WeddingStoreApp.Images.GroupCollapse.png");
                return ImageSource.FromResource(Constant.ImagePatch + "GroupCollapse.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
