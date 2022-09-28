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
    public partial class CenterPopup : ContentView
    {
        public static readonly BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(View), typeof(CenterPopup), null, propertyChanged: ContentChanged);
        private static void ContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            CenterPopup control = (CenterPopup)bindable;
            control.CreateBody();
        }
        public View Body
        {
            get { return (View)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CenterPopup), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

        public event EventHandler Open;
        public event EventHandler Close;
        public CenterPopup()
        {
            InitializeComponent();
            this.title.BindingContext = this;
            this.title.SetBinding(Label.TextProperty, "Title");
        }

        private void CreateBody()
        {
            if (Body != null)
            {
                grid_body.Children.Add(Body);
                Grid.SetColumn(Body, 0);
                Grid.SetRow(Body, 1);
            }
        }

        public void ShowCenterPopup()
        {
            EventHandler eventHandler = Open;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
            this.IsVisible = true;
        }

        public void CloseContent_Tapped(object sender, EventArgs e)
        {
            EventHandler eventHandler = Close;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
            this.IsVisible = false;
        }

        public void CloseContent()
        {
            EventHandler eventHandler = Close;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
            this.IsVisible = false;
        }
    }
}