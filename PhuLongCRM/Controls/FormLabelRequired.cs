using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class FormLabelRequired : StackLayout
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FormLabelRequired), null, BindingMode.TwoWay);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public FormLabelRequired()
        {
            this.BindingContext = this;
            Grid.SetColumn(this, 0);
            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.Center;
            var titleLabel = new Label()
            {
                FontSize = 15,
                TextColor = Color.FromHex("#444444")
            };
            titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Text)));
            this.Children.Add(titleLabel);
            this.Children.Add(new Label()
            {
                Text = "*",
                FontSize = 16,
                TextColor = Color.Red
            });
        }
    }
}
