using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertTinhTrangToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tinhTrang = (int)value;
            ImageSource retSource = null;
            switch (tinhTrang)
            {
                case 0:
                    retSource= ImageSource.FromResource("WeddingStoreMoblie.Images.ChuaTrangTri.png");
                    break;
                case 1:
                    retSource = ImageSource.FromResource("WeddingStoreMoblie.Images.DaTrangTri.png");
                    break;
                case 2:
                    retSource = ImageSource.FromResource("WeddingStoreMoblie.Images.DaThaoDo.png");
                    break;
            }
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
