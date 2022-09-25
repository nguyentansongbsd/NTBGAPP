using System;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class MainEntry : Entry
    {
        public static readonly BindableProperty ShowClearButtonProperty = BindableProperty.Create(nameof(ShowClearButton), typeof(bool), typeof(MainEntry), true, BindingMode.TwoWay);
        public bool ShowClearButton { get => (bool)GetValue(ShowClearButtonProperty); set { SetValue(ShowClearButtonProperty, value); } }

        public MainEntry()
        {
            TextColor = Color.Black;
            this.FontSize = 16;
            this.PlaceholderColor = Color.Gray;
            this.HeightRequest = 40;        
            this.FontFamily = "Segoe";
        }
    }
}
