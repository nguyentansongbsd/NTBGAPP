using System;
using System.Globalization;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhuLongCRM.Converters
{
    public class UnitItemWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            var padding = thickness.Left;
            var width = (Application.Current.MainPage.Width - (padding * 2 + 8 + 8 + 20)) / 2; // padding * 2 la tong trai va phai, 8 la tong marrign trai phai cua 1 item unit
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
