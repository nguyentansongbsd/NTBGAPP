using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Filter : RadBorder
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Filter), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(Filter), null, BindingMode.TwoWay, propertyChanged: ItemChanged);
        public object SelectedItem { get => (object)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); } }

        public static readonly BindableProperty SelectedIdsProperty = BindableProperty.Create(nameof(SelectedIds), typeof(List<string>), typeof(Filter), null, BindingMode.TwoWay, null, propertyChanged: ItemChanged);
        public List<string> SelectedIds { get => (List<string>)GetValue(SelectedIdsProperty); set { SetValue(SelectedIdsProperty, value); } }

        public static readonly BindableProperty NameDipslayProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(Filter), null, BindingMode.OneWay);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(Filter), null, BindingMode.TwoWay, null);
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public string NameDisplay { get => (string)GetValue(NameDipslayProperty); set { SetValue(NameDipslayProperty, value); } }

        public static readonly BindableProperty SelectedDipslayProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(Filter), null, BindingMode.OneWay);
        public string SelectedDisplay { get => (string)GetValue(SelectedDipslayProperty); set { SetValue(SelectedDipslayProperty, value); } }
        public bool Multiple { get; set; }

        public bool PreOpenOneTime { get; set; } = true;
        public BottomModal BottomModal { get; set; }
        private ListView lookUpListView;
        private Grid gridMain;
        private SearchBar searchBar;
        public Func<Task> PreOpenAsync;
        public Action PreOpen;
        public event EventHandler<LookUpChangeEvent> SelectedItemChange;
        public event EventHandler<LookUpChangeEvent> SelectedItemChanged;
        public event EventHandler<LookUpChangeEvent> SearchChanged;

        public Filter()
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

            if (lookUpListView == null)
            {
                SetUpListView();
            }
            else
            {
                lookUpListView.ItemsSource = ItemsSource.Cast<object>().ToList();
            }

            BottomModal.Title = Placeholder;
            BottomModal.ModalContent = gridMain;
            await BottomModal.Show();
        }

        public void setActive()
        {
            string name = null;
            if (Multiple)
            {
                if (this.SelectedIds != null && this.SelectedIds.Any() && ItemsSource != null)
                {
                    var selectedInSource = ItemsSource.Cast<OptionSet>().ToList().Where(x => SelectedIds.Any(s => s == x.Val)).ToList();
                    foreach (var item in selectedInSource)
                    {
                        item.Selected = true;
                    }
                    name = string.Join(", ", selectedInSource.Select(x => x.Label).ToArray());
                }
            }
            else
            {
                name = this.SelectedItem.GetType().GetProperty(this.NameDisplay)?.GetValue(this.SelectedItem, null)?.ToString();
            }

            if (name != null && name != Language.tat_ca && !name.Contains(Language.tat_ca))
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
            Filter control = (Filter)bindable;
            if (newValue != null)
            {
                control.setActive();
            }
            else
            {
                control.setUnActive();
            }
        }      

        private void SetUpListView()
        {
            StackLayout stSearchBar = new StackLayout();

            searchBar = new SearchBar();
            searchBar.TextChanged += SearchBar_TextChangedEventArgs;

            SearchBarFrame searchBarFrame = new SearchBarFrame();
            searchBarFrame.Content = searchBar;

            stSearchBar.Children.Add(searchBarFrame);

            lookUpListView = new ListView(ListViewCachingStrategy.RecycleElement);
            lookUpListView.BackgroundColor = Color.White;
            lookUpListView.HasUnevenRows = true;
            lookUpListView.SelectionMode = ListViewSelectionMode.None;
            lookUpListView.SeparatorVisibility = SeparatorVisibility.None;

            var dataTemplate = new DataTemplate(() =>
            {
                RadBorder st = new RadBorder();
                st.BorderThickness = new Thickness(0, 0, 0, 1);
                st.BorderColor = Color.FromHex("#eeeeee");
                st.Padding = 10;
                st.Margin = new Thickness(5, 0);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                st.Content = grid;

                Label lb = new Label();
                lb.TextColor = Color.Black;
                lb.FontSize = 16;
                lb.SetBinding(Label.TextProperty, NameDisplay);
                Grid.SetColumn(lb, 0);
                grid.Children.Add(lb);

                if (Multiple)
                {
                    Label labelCheck = new Label();
                    Grid.SetColumn(labelCheck, 1);
                    labelCheck.Text = "\uf00c";
                    labelCheck.TextColor = Color.DarkGreen;
                    labelCheck.FontFamily = "FontAwesomeSolid";
                    labelCheck.SetBinding(Label.IsVisibleProperty, SelectedDisplay);
                    grid.Children.Add(labelCheck);
                }

                return new ViewCell { View = st };
            });

            lookUpListView.ItemTemplate = dataTemplate;

            lookUpListView.ItemTapped += LookUpListView_ItemTapped;

            gridMain = new Grid();
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            stSearchBar.Padding = new Thickness(10, 10, 10, 0);
            gridMain.Children.Add(stSearchBar);
            Grid.SetRow(stSearchBar, 0);

            gridMain.Children.Add(lookUpListView);
            Grid.SetRow(lookUpListView, 1);
            lookUpListView.ItemsSource = ItemsSource.Cast<object>().ToList();          
            BottomModal.OnHide += BottomModal_OnHide;
        }

        private void BottomModal_OnHide(object sender, EventArgs e)
        {
            if (Multiple)
            {
                var checkedItems = ItemsSource.Cast<OptionSet>().ToList().Where(x => x.Selected).ToList();
                if (checkedItems.Any())
                {
                    var a = SelectedIdsChange(checkedItems);
                    if (SelectedIdsChange(checkedItems))
                    {
                        SelectedIds = checkedItems.Select(x => x.Val).ToList();
                        SelectedItemChanged?.Invoke(this, new LookUpChangeEvent());
                    }
                }
            }
        }

        private async void LookUpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(Multiple)
            {
                var item = e.Item as OptionSet;
                if(item.Label != null && item.Label == Language.tat_ca && item.Label.Contains(Language.tat_ca) && item.Selected == false)
                {
                    foreach(var i in ItemsSource.Cast<OptionSet>().ToList())
                    {
                        i.Selected = false;
                    }    
                }
                
                var selectedAll = ItemsSource.Cast<OptionSet>().ToList().Where(x => x.Val=="-1").FirstOrDefault();

                if (item.Label != null && item.Label != Language.tat_ca && !item.Label.Contains(Language.tat_ca) && selectedAll != null && selectedAll.Selected == true)
                {
                    selectedAll.Selected = false;
                }
                item.Selected = !item.Selected;
                SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
            }
            else
            {
                if (this.SelectedItem != e.Item)
                {
                    this.SelectedItem = e.Item;
                    SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                }
                await BottomModal.Hide();
            }
        }
        private void SearchBar_TextChangedEventArgs(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
                if (string.IsNullOrWhiteSpace(text))
                {
                    lookUpListView.ItemsSource = this.ItemsSource;
                }
                else
                {
                    lookUpListView.ItemsSource = this.ItemsSource.Cast<object>().ToList().Where(x => GetValObjDy(x, NameDisplay).ToString().ToLower().Contains(text.ToLower()));
                }
        }
      
        public object GetValObjDy(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public bool SelectedIdsChange(List<OptionSet> list)
        {
            if (SelectedIds != null && SelectedIds.Count > 0 && list != null)
            {
                int count = SelectedIds.Count;
                int down = 0;

                foreach (var item in list)
                {
                    if (!string.IsNullOrWhiteSpace(SelectedIds.Where(x => x == item.Val).FirstOrDefault()))
                        down++;
                }

                if (count == down && count == list.Count())
                    return false;
                else
                    return true;
            }
            return true;
        }
    }
}