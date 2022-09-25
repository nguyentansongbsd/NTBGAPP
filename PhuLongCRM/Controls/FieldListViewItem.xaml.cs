using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FieldListViewItem : Grid
    {
        // title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FieldListViewItem), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        // value
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FieldListViewItem), null, BindingMode.TwoWay);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        
        // title color
        public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(FieldListViewItem), Color.Gray, BindingMode.TwoWay);
        public Color TitleTextColor { get => (Color)GetValue(TitleTextColorProperty); set => SetValue(TitleTextColorProperty, value); }
        // value color
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FieldListViewItem), Color.Black, BindingMode.TwoWay);
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
        // title font
        public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes), typeof(FieldListViewItem), FontAttributes.None, BindingMode.TwoWay);
        public FontAttributes TitleFontAttributes { get => (FontAttributes)GetValue(TitleFontAttributesProperty); set => SetValue(TitleFontAttributesProperty, value); }
        // value font
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FieldListViewItem), FontAttributes.None, BindingMode.TwoWay);
        public FontAttributes FontAttributes { get => (FontAttributes)GetValue(FontAttributesProperty); set => SetValue(FontAttributesProperty, value); }
        // value line mode
        public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(FieldListViewItem), LineBreakMode.TailTruncation, BindingMode.TwoWay);
        public LineBreakMode LineBreakMode { get => (LineBreakMode)GetValue(LineBreakModeProperty); set => SetValue(LineBreakModeProperty, value); }

        public FieldListViewItem()
        {
            InitializeComponent();
            lb_title.SetBinding(Label.TextProperty, new Binding("Title") { Source = this, StringFormat = "{0}: " });
            lb_title.SetBinding(Label.TextColorProperty, new Binding("TitleTextColor") { Source = this });
            lb_title.SetBinding(Label.FontAttributesProperty, new Binding("TitleFontAttributes") { Source = this });

            lb_text.SetBinding(Label.TextProperty, new Binding("Text") { Source = this });
            lb_text.SetBinding(Label.TextColorProperty, new Binding("TextColor") { Source = this });
            lb_text.SetBinding(Label.FontAttributesProperty, new Binding("FontAttributes") { Source = this });
            lb_text.SetBinding(Label.LineBreakModeProperty, new Binding("LineBreakMode") { Source = this });
        }
    }
}