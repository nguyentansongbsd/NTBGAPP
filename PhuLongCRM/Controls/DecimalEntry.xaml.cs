using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PhuLongCRM.Helper;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class DecimalEntry : Grid
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(DecimalEntry), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(DecimalEntry), null, BindingMode.TwoWay);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        //public decimal? Price { get; set; }

        public static readonly BindableProperty PriceProperty = BindableProperty.Create(nameof(Price), typeof(decimal?), typeof(DecimalEntry), null, BindingMode.TwoWay, propertyChanged: PriceChanged);
        public decimal? Price { get => (decimal?)GetValue(PriceProperty); set { SetValue(PriceProperty, value); } }

        public event EventHandler OnUnfocus;

        public DecimalEntry()
        {
            InitializeComponent();
            EntryPrice.BindingContext = this;

            if (Device.RuntimePlatform == Device.Android)
            {
                this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("Price") { Source = this, Converter = new Converters.NullToHideConverter() });
                BtnClear.Clicked += BtnClear_Clicked;
            }
            else
            {
                BtnClear.IsVisible = false;
            }
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            this.Price = null;
            this.Text = null;
        }

        public void SetPrice(decimal? price)
        {
            if (price.HasValue)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.Price = price.Value;
                    this.Text = DecimalHelper.DecimalToText(price);
                }
                else
                {
                    this.Price = price.Value;
                    this.Text = DecimalHelper.DecimalToText(price).Replace(".", "").Replace(',', '.');
                }

            }
            else
            {
                this.Price = null;
                this.Text = "";
            }
        }

        private static void PriceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((DecimalEntry)bindable).SetPrice((decimal?)newValue);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string text = e.NewTextValue;
                if (text.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                {
                    text = text.Substring(0, text.Length - 1) + ",";
                }

                text = text.Replace(",,", ",");

                if (string.IsNullOrWhiteSpace(text))
                {
                    this.Price = null;
                    return;
                }

                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (text.LastIndexOf(',') == text.Length - 1)
                    {
                        Text = text;
                        return;
                    }

                    var splitFullText = text.Split(',').Where(x => (string.IsNullOrWhiteSpace(x) == false)).ToArray();
                    NumberFormatInfo nfi = new CultureInfo("vi-VN", false).NumberFormat;
                    if (splitFullText.Length == 2)
                    {
                        var giaTriSauDauPhay = splitFullText[1];
                        if (giaTriSauDauPhay.Length == 1) // chi co 1 ky tu sau day phai.
                        {
                            nfi.NumberDecimalDigits = 1;

                            this.Price = DecimalHelper.TextToDecimal(text);
                            this.Text = this.Price.Value.ToString("N", nfi);
                        }
                        else if (giaTriSauDauPhay.Length == 2)
                        {
                            nfi.NumberDecimalDigits = 2;

                            this.Price = DecimalHelper.TextToDecimal(text);
                            this.Text = this.Price.Value.ToString("N", nfi);
                        }
                        else if (giaTriSauDauPhay.Length == 2)
                        {
                            nfi.NumberDecimalDigits = 2;

                            this.Price = DecimalHelper.TextToDecimal(text);
                            this.Text = this.Price.Value.ToString("N", nfi);
                        }
                        //else if (giaTriSauDauPhay.Length > 3)
                        //{
                        //    giaTriSauDauPhay = giaTriSauDauPhay.Substring(0, 3);

                        //    nfi.NumberDecimalDigits = 3;

                        //    string newText = splitFullText[0] + "," + giaTriSauDauPhay;
                        //    this.Price = DecimalHelper.TextToDecimal(newText);
                        //    this.Text = this.Price.Value.ToString("N", nfi);
                        //}
                        else if (giaTriSauDauPhay.Length > 2)
                        {
                            giaTriSauDauPhay = giaTriSauDauPhay.Substring(0, 2);

                            nfi.NumberDecimalDigits = 2;

                            string newText = splitFullText[0] + "," + giaTriSauDauPhay;
                            this.Price = DecimalHelper.TextToDecimal(newText);
                            this.Text = this.Price.Value.ToString("N", nfi);
                        }
                    }
                    else if (splitFullText.Length > 2)
                    {
                        string newText = text.Substring(0, text.LastIndexOf(','));
                        this.Price = DecimalHelper.TextToDecimal(newText);
                        this.Text = this.Price.Value.ToString("N", nfi);
                    }
                    else  // nho hon 2. ko co dau phay.
                    {

                        nfi.NumberDecimalDigits = 0;
                        this.Price = DecimalHelper.TextToDecimal(text);
                        this.Text = this.Price.Value.ToString("N", nfi);
                    }
                }
                else
                {
                    //if (text.LastIndexOf(',') == text.Length - 1)
                    //{
                    //    Text = text;
                    //    return;
                    //}
                    var split = text.Split('.');
                    if (split.Length == 2 && split[1].Length > 0)
                    {
                        if (split[1].Length > 2)
                        {
                            string newText = split[0] + "." + split[1].Substring(0, 2);
                            this.Text = newText;
                            this.Price = DecimalHelper.TextToDecimal(newText.Replace(".", ","));
                            return;
                        }
                    }
                    this.Price = DecimalHelper.TextToDecimal(text.Replace(".", ","));
                }
            }
            catch (Exception ex)
            {
                this.Text = e.OldTextValue;
            }
        }

        void EntryPrice_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            OnUnfocus?.Invoke(sender, e);
        }
    }
}
