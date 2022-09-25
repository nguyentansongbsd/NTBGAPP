using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class FilterPicker : RadBorder
    {
        public Func<Task> PreOpenAsync;
        private LookUp _lookUp;
        public Action PreOpen;
        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FilterPicker), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FilterPicker), null, BindingMode.TwoWay, propertyChanged: ItemChanged);
        public object SelectedItem { get => (object)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); } }

        public static readonly BindableProperty NameDipslayProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(FilterPicker), null, BindingMode.OneWay);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FilterPicker), null, BindingMode.TwoWay, null);
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public BottomModal BottomModal { get; set; }

        public string NameDisplay { get => (string)GetValue(NameDipslayProperty); set { SetValue(NameDipslayProperty, value); } }

        public bool PreOpenOneTime { get; set; } = true;

        public FilterPicker()
        {
            InitializeComponent();
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += OnTapped;
            this.GestureRecognizers.Add(tap);

            this.lblText.SetBinding(Label.TextProperty, new Binding("Placeholder") { Source = this });
        }

        private async void OnTapped(object sender, EventArgs e)
        {
            if (PreOpenAsync != null)
            {
                await PreOpenAsync();
                if (PreOpenOneTime)
                {
                    PreOpenAsync = null;
                }
            }

            if (PreOpen != null)
            {
                PreOpen.Invoke();
                if (PreOpenOneTime)
                {
                    PreOpen = null;
                }
            }

            if (_lookUp == null)
            {
                _lookUp = new LookUp();
                _lookUp.SetBinding(LookUp.ItemsSourceProperty, new Binding("ItemsSource") { Source = this });
                _lookUp.SetBinding(LookUp.SelectedItemProperty, new Binding("SelectedItem") { Source = this });
                _lookUp.SetBinding(LookUp.PlaceholderProperty, new Binding("Placeholder") { Source = this });
                _lookUp.NameDisplay = this.NameDisplay;
                _lookUp.BottomModal = this.BottomModal;
                _lookUp.SelectedItemChange += SelectedItemChange;
            }
            await _lookUp.OpenModal();
        }

        public void setActive()
        {
            string name = this.SelectedItem.GetType().GetProperty(this.NameDisplay)?.GetValue(this.SelectedItem, null)?.ToString();
            if (name != null && name != Language.tat_ca)
            {
                lblText.Text = name;
                lblText.FontFamily = "SegoeBold";
                this.BorderColor = Color.Gray;
            }
            else
            {
                setUnActive();
            }
        }
        public void setUnActive()
        {
            lblText.Text = this.Placeholder;
            lblText.FontFamily = "Segoe";
            this.BorderColor = Color.LightGray;
        }

        private static void ItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FilterPicker control = (FilterPicker)bindable;
            if (newValue != null)
            {
                control.setActive();
            }
            else
            {
                control.setUnActive();
            }
        }
    }
}

