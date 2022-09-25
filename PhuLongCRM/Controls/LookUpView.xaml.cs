using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Models;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
namespace PhuLongCRM.Controls
{
    public partial class LookUpView : Grid
    {
        public event EventHandler<LookUpChangeEvent> SearchPress;
        public ListView lookUpListView { get; set; }

        public static readonly BindableProperty IsSearchPressProperty = BindableProperty.Create(nameof(IsSearchPress), typeof(bool), typeof(LookUpView), false, BindingMode.TwoWay);
        public bool IsSearchPress { get => (bool)GetValue(IsSearchPressProperty); set => SetValue(IsSearchPressProperty, value); }

        private bool ClearingSearchBar = false;
        private string text = string.Empty;

        public LookUpView()
        {
            InitializeComponent();
        }

        public void SetList<T>(List<T> list, string Name) where T : class
        {
            if (IsSearchPress == false && string.IsNullOrWhiteSpace(searchBar.Text) == false)
            {
                ClearingSearchBar = true;
                searchBar.Text = null;
                ClearingSearchBar = false;
            }

            if (lookUpListView == null && IsSearchPress == true)
            {
                searchBar.SearchButtonPressed += SearchBar_SearchButtonPressed;
            }

            if (lookUpListView == null)
            {
                lookUpListView = new ListView(ListViewCachingStrategy.RecycleElement);
                // lookUpListView = new ListView();
                lookUpListView.BackgroundColor = Color.White;
                lookUpListView.HasUnevenRows = true;
                lookUpListView.SelectionMode = ListViewSelectionMode.None;
                lookUpListView.SeparatorVisibility = SeparatorVisibility.None;

                var dataTemplate = new DataTemplate(() =>
                {
                    RadBorder st = new RadBorder();
                    st.BorderThickness = new Thickness(0, 0, 0, 1);
                    st.BorderColor = Color.FromHex("#f2f2f2");
                    st.Padding = new Thickness(15, 10);
                    Label lb = new Label();
                    lb.TextColor = Color.Black;
                    lb.FontSize = 16;
                    lb.SetBinding(Label.TextProperty, Name);
                    st.Content = lb;
                    return new ViewCell { View = st };
                });

                lookUpListView.ItemTemplate = dataTemplate;
                lookUpListView.ItemsSource = list;
                lookUpListView.ItemTapped += LookUpListView_ItemTapped;
                Grid.SetRow(lookUpListView, 1);
                this.Children.Add(lookUpListView);

                searchBar.TextChanged += async (object sender, TextChangedEventArgs e) =>
                {
                    if (ClearingSearchBar) return;

                    text = e.NewTextValue;
                    if (IsSearchPress == false)
                    {
                        if (string.IsNullOrWhiteSpace(text))
                        {
                            lookUpListView.ItemsSource = list;
                        }
                        else
                        {
                            lookUpListView.ItemsSource = list.Where(x => GetValObjDy(x, Name).ToString().ToLower().Contains(text.ToLower()));
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(text) && IsSearchPress == true)
                    {
                        SearchPress?.Invoke(sender, new LookUpChangeEvent() { Item = text });
                    }
                };
            }
            else
            {
                lookUpListView.ItemsSource = list;
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            SearchPress?.Invoke(sender, new LookUpChangeEvent() { Item = text });
        }

        private void LookUpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (searchBar.IsFocused)
            {
                searchBar.Unfocus();
            }
        }

        public object GetValObjDy(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public void FocusSearchBarOnTap()
        {
            searchBar.Focus();
        }
    }
}
