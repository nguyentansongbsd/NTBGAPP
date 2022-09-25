using System;
using System.Collections.Generic;
using System.Windows.Input;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class DatePickerControl : ContentView
    {
        public event EventHandler DateSelected;

        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(DatePickerControl), null,BindingMode.TwoWay);
        public DateTime? Date
        {
            get { 
                return (DateTime?)GetValue(DateProperty);
            }
            set
            {
                SetValue(DateProperty, value);
                OnAccept();
            }
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(DatePickerControl), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SpinnerFormatProperty = BindableProperty.Create(nameof(SpinnerFormat), typeof(string), typeof(DatePickerControl), null, BindingMode.TwoWay);
        public string SpinnerFormat { get => (string)GetValue(SpinnerFormatProperty); set => SetValue(SpinnerFormatProperty, value); }

        public static readonly BindableProperty DefaultDisplayProperty = BindableProperty.Create(nameof(DefaultDisplay), typeof(DateTime), typeof(DatePickerControl), DateTime.Now, BindingMode.TwoWay);
        public DateTime DefaultDisplay { get => (DateTime)GetValue(DefaultDisplayProperty); set => SetValue(DefaultDisplayProperty, value); }

        public static readonly BindableProperty DisplayFormatProperty = BindableProperty.Create(nameof(DisplayFormat), typeof(string), typeof(DatePickerControl), null, BindingMode.TwoWay);
        public string DisplayFormat { get => (string)GetValue(DisplayFormatProperty); set => SetValue(DisplayFormatProperty, value); }

        public ICommand Accept { get; set; }

        public DatePickerControl()
        {
            InitializeComponent();
            this.Accept = new Command(this.onAccept);
        }

        private void onAccept(object obj)
        {
            if (Date == null)
            {
                Date = DateTime.Now;
            }
        }

        private void OnAccept()
        {
            DateSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
