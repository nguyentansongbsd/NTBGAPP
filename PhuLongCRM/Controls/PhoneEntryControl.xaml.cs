using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class PhoneEntryControl : MainEntry
    {
        public static readonly BindableProperty PhoneNumProperty = BindableProperty.Create(nameof(PhoneNum), typeof(string), typeof(PhoneEntryControl), "+84-", BindingMode.TwoWay);
        public string PhoneNum { get => (string)GetValue(PhoneNumProperty); set => SetValue(PhoneNumProperty, value); }

        public PhoneEntryControl()
        {
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            entryPhone.SetBinding(MainEntry.TextProperty, new Binding("PhoneNum") { Source = this });
        }

    }
}
