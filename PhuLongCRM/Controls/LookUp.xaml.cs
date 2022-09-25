using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Models;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class LookUp : Grid
    {
        public Func<Task> PreOpenAsync;
        public Action PreOpen;
        public event EventHandler<LookUpChangeEvent> SearchPress;

        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUp), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        public LookUpView _lookUpView;
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(LookUp), null, BindingMode.TwoWay);
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty NameDipslayProperty = BindableProperty.Create(nameof(NameDisplay), typeof(string), typeof(LookUp), null, BindingMode.TwoWay, propertyChanged: DisplayNameChang);
        public string NameDisplay { get => (string)GetValue(NameDipslayProperty); set { SetValue(NameDipslayProperty, value); } }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(LookUp), null, BindingMode.TwoWay, null);
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public static readonly BindableProperty IsSearchPressProperty = BindableProperty.Create(nameof(IsSearchPress), typeof(bool), typeof(LookUp), false, BindingMode.TwoWay);
        public bool IsSearchPress { get => (bool)GetValue(IsSearchPressProperty); set => SetValue(IsSearchPressProperty, value); }

        public ContentView ModalPopup { get; set; }
        public BottomModal BottomModal { get; set; }
        public bool FocusSearchBarOnTap = false;
        public bool PreOpenOneTime { get; set; } = true;

        public LookUp()
        {
            InitializeComponent();
            this.Entry.BindingContext = this;
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, "Placeholder");
            this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = this, Converter = new Converters.NullToHideConverter() });
        }
        public void Clear_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
            SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
        }
        public void HideClearButton()
        {
            BtnClear.IsVisible = false;
        }
        public async void OpenLookUp_Tapped(object sender, EventArgs e)
        {
            await OpenModal();
        }

        private static void DisplayNameChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUp control = (LookUp)bindable;
            control.Entry.SetBinding(EntryNoneBorder.TextProperty, "SelectedItem." + newValue);
        }

        public async Task OpenModal()
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

            if (this.ItemsSource == null || this.BottomModal == null) return;

            if (_lookUpView == null)
            {
                _lookUpView = new LookUpView();
                _lookUpView.IsSearchPress = IsSearchPress;
                _lookUpView.SetList(ItemsSource.Cast<object>().ToList(), NameDisplay);
                _lookUpView.lookUpListView.ItemTapped += async (lookUpSender, lookUpTapEvent) =>
                {
                    if (this.SelectedItem != lookUpTapEvent.Item)
                    {
                        this.SelectedItem = lookUpTapEvent.Item;
                        SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    }
                    await BottomModal.Hide();
                };
                _lookUpView.SearchPress += _lookUpView_SearchPress;
            }
            else
            {
                _lookUpView.SetList(ItemsSource.Cast<object>().ToList(), NameDisplay);
            }

            BottomModal.Title = Placeholder;
            BottomModal.ModalContent = _lookUpView;
            await BottomModal.Show();

            if (FocusSearchBarOnTap)
            {
                _lookUpView.FocusSearchBarOnTap();
            }
        }

        private void _lookUpView_SearchPress(object sender, LookUpChangeEvent e)
        {
            SearchPress?.Invoke(sender, e);
        }

        public void ResetItemSource()
        {
            _lookUpView.SetList(ItemsSource.Cast<object>().ToList(), NameDisplay);
        }
    }
}
