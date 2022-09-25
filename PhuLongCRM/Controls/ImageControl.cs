using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class ImageControl : Image
    {
        public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(ImageControl), "");
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public static readonly BindableProperty TokenProperty = BindableProperty.Create(nameof(Token), typeof(string), typeof(ImageControl), "");
        public string Token
        {
            get { return (string)GetValue(TokenProperty); }
            set { SetValue(TokenProperty, value); }
        }
    }
}
