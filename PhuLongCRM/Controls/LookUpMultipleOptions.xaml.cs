using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using PhuLongCRM.Models;
using Xamarin.Forms;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Controls
{
    public partial class LookUpMultipleOptions : Grid
    {
        public CenterModal CenterModal { get; set; }
        public event EventHandler OnSave;
        public event EventHandler OnDelete;

        public Func<Task> PreShow;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<OptionSetFilter>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null, propertyChanged: ItemSourceChange);
        public List<OptionSetFilter> ItemsSource { get => (List<OptionSetFilter>)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedIdsProperty = BindableProperty.Create(nameof(SelectedIds), typeof(List<string>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null, propertyChanged: ItemSourceChange);
        public List<string> SelectedIds { get => (List<string>)GetValue(SelectedIdsProperty); set { SetValue(SelectedIdsProperty, value); } }

        public static readonly BindableProperty ShowClearButtonProperty = BindableProperty.Create(nameof(ShowClearButton), typeof(bool), typeof(MainEntry), true, BindingMode.TwoWay);
        public bool ShowClearButton { get => (bool)GetValue(ShowClearButtonProperty); set { SetValue(ShowClearButtonProperty, value); } }

        public static readonly BindableProperty ShowCloseButtonProperty = BindableProperty.Create(nameof(ShowCloseButton), typeof(bool), typeof(MainEntry), true, BindingMode.TwoWay);
        public bool ShowCloseButton { get => (bool)GetValue(ShowCloseButtonProperty); set { SetValue(ShowCloseButtonProperty, value); } }

        private string _text;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }

        //edit
        public static readonly BindableProperty ListListViewProperty = BindableProperty.Create(nameof(ListListView), typeof(List<List<OptionSetFilter>>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null);
        public List<List<OptionSetFilter>> ListListView { get => (List<List<OptionSetFilter>>)GetValue(ListListViewProperty); set { SetValue(ListListViewProperty, value); } }

        public static readonly BindableProperty ListTabProperty = BindableProperty.Create(nameof(ListTab), typeof(List<string>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null);
        public List<string> ListTab { get => (List<string>)GetValue(ListTabProperty); set { SetValue(ListTabProperty, value); } }

        private ListView lookUpListView;
        private Grid gridMain;
        private Grid gridButton;
        private SearchBar searchBar;
        private Button saveButton, cancelButton;
        private List<RadBorder> ListRadBorderTab { get; set; }
        private List<Label> ListLabelTab { get; set; }
        private int indexTab { get; set; }
       // private List<List<OptionSet>> ListListView { get; set; }
        public LookUpMultipleOptions()
        {
            InitializeComponent();
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, new Binding("Placeholder") { Source = this });
            this.Entry.SetBinding(EntryNoneBorder.TextProperty, new Binding("Text") { Source = this });
        }

        private bool init = false;

        private async void OpenLookUp_Tapped(object sender1, EventArgs e1)
        {
            await Show();
        }

        public bool PreOpenOneTime { get; set; } = true;
        public async Task Show()
        {
            SetUpModal();
            await CenterModal.Show();
        }

        public async Task Hide() => await CenterModal.Hide();

        private void SetUpGridButton()
        {
            saveButton = new Button()
            {
                Text = Language.luu,
                BackgroundColor = (Color)App.Current.Resources["NavigationPrimary"],
                TextColor = Color.White,
                Padding = new Thickness(10, 5)
            };
            saveButton.Clicked += SaveButton_Clicked;


            Button deleteButton = new Button()
            {
                Text = Language.xoa,
                TextColor = (Color)App.Current.Resources["NavigationPrimary"],
                BackgroundColor = Color.White,
                BorderColor = (Color)App.Current.Resources["NavigationPrimary"],
                Padding = new Thickness(10, 5),
                BorderWidth = 1
            };
            deleteButton.SetBinding(Button.IsVisibleProperty, new Binding("ShowClearButton") { Source = this });
            deleteButton.Clicked += async (object sender, EventArgs e) =>
            {
                this.ClearData();
                await this.Hide();
                OnSave?.Invoke(this, EventArgs.Empty);
            };


            cancelButton = new Button()
            {
                Text = Language.dong,
                TextColor = (Color)App.Current.Resources["NavigationPrimary"],
                BackgroundColor = Color.White,
                BorderColor = (Color)App.Current.Resources["NavigationPrimary"],
                Padding = new Thickness(10, 5),
                BorderWidth = 1
            };
            cancelButton.SetBinding(Button.IsVisibleProperty, new Binding("ShowCloseButton") { Source = this });
            cancelButton.Clicked += CancelButton_Clicked;

            gridButton = new Grid()
            {
                ColumnSpacing = 5,
                Padding = new Thickness(5, 0, 5, 5)
            };

            gridButton.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            gridButton.Children.Add(cancelButton);
            gridButton.Children.Add(deleteButton);
            gridButton.Children.Add(saveButton);
            Grid.SetRow(cancelButton, 0);
            Grid.SetRow(deleteButton, 0);
            Grid.SetRow(saveButton, 0);

            if (deleteButton.IsVisible == false && cancelButton.IsVisible == false)
            {
                Grid.SetColumn(saveButton, 0);
            }
            else
            {
                gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                if (deleteButton.IsVisible == false)
                {
                    Grid.SetColumn(cancelButton, 0);
                    Grid.SetColumn(saveButton, 1);
                }
                else if (cancelButton.IsVisible == false)
                {
                    Grid.SetColumn(deleteButton, 0);
                    Grid.SetColumn(saveButton, 1);
                }
                else
                {
                    gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    Grid.SetColumn(cancelButton, 0);
                    Grid.SetColumn(deleteButton, 1);
                    Grid.SetColumn(saveButton, 2);
                }
            }
        }

        private void SetUpListView()
        {
            StackLayout stSearchBar = new StackLayout();          

            searchBar = new SearchBar();
            searchBar.Placeholder = Language.tim_kiem;
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
                lb.SetBinding(Label.TextProperty, "Label");
                Grid.SetColumn(lb, 0);
                grid.Children.Add(lb);


                Label labelCheck = new Label();
                Grid.SetColumn(labelCheck, 1);
                labelCheck.Text = "\uf00c";
                labelCheck.TextColor = Color.DarkGreen;
                labelCheck.FontFamily = "FontAwesomeSolid";
                labelCheck.SetBinding(Label.IsVisibleProperty, "Selected");
                grid.Children.Add(labelCheck);

                return new ViewCell { View = st };
            });
            lookUpListView.ItemTemplate = dataTemplate;

            lookUpListView.ItemTapped += LookUpListView_ItemTapped;

            //edit          

            if (ListListView != null && ListListView.Count>0 && ListTab != null && ListTab.Count>0)
            {
                ItemSourceForTabs();
                Grid tabs = SetUpTabs(ListTab);
                gridMain = new Grid();
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                Grid.SetRow(tabs, 0);
                gridMain.Children.Add(tabs);

                stSearchBar.Padding = new Thickness(10, 5, 10, 0);
                gridMain.Children.Add(stSearchBar);
                Grid.SetRow(stSearchBar, 1);

                gridMain.Children.Add(lookUpListView);
                Grid.SetRow(lookUpListView, 2);
                indexTab = 0;
                IndexTab(indexTab);
            }
            else
            {
                gridMain = new Grid();
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                stSearchBar.Padding = new Thickness(10, 10, 10, 0);
                gridMain.Children.Add(stSearchBar);
                Grid.SetRow(stSearchBar, 0);

                gridMain.Children.Add(lookUpListView);
                Grid.SetRow(lookUpListView, 1);
                lookUpListView.ItemsSource = ItemsSource;
            }
        }

        private void SearchBar_TextChangedEventArgs(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(text))
            {
                if (ListListView != null && ListListView.Count > 0 && ListTab != null && ListTab.Count > 0)
                {
                    lookUpListView.ItemsSource = this.ListListView[indexTab];
                }
                else
                {
                    lookUpListView.ItemsSource = this.ItemsSource;
                }
            }
            else
            {
                if (ListListView != null && ListListView.Count > 0 && ListTab != null && ListTab.Count > 0)
                {
                    var list = from Item in ListListView[indexTab]
                               where Item.Label.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.SDT != null && Item.SDT.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.CMND != null && Item.CMND.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.CCCD != null && Item.CCCD.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.HC != null && Item.HC.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.SoGPKD != null && Item.SoGPKD.ToString().ToLower().Contains(text.ToLower())
                               select Item;
                    lookUpListView.ItemsSource = list;
                }
                else
                {
                    var list = from Item in ItemsSource
                               where Item.Label.ToString().ToLower().Contains(text.ToLower())||
                               Item.SDT != null && Item.SDT.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.CMND != null && Item.CMND.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.CCCD != null && Item.CCCD.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.HC != null && Item.HC.ToString().ToLower().Contains(text.ToLower()) ||
                               Item.SoGPKD != null && Item.SoGPKD.ToString().ToLower().Contains(text.ToLower())
                               select Item;
                    lookUpListView.ItemsSource = list;
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            ItemsSource.Where(x => x.Selected == true).ToList().ForEach(x => x.Selected = false);
            if (SelectedIds != null)
            {
                ItemsSource.Where(x => SelectedIds.Any(val => val == x.Val)).ToList().ForEach(x => x.Selected = true);
            }
            await CenterModal.Hide();
        }

        public async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var checkedItems = ItemsSource.Where(x => x.Selected).ToList();
            if (checkedItems.Any())
            {
                string[] names = checkedItems.Select(x => x.Label).ToArray();
                SelectedIds = checkedItems.Select(x => x.Val).ToList();
                this.Text = string.Join(", ", names);
                SetList(checkedItems);
            }
            else
            {
                SelectedIds = null;
                this.Text = null;
                ClearFlexLayout();
            }

            await CenterModal.Hide();
            OnSave?.Invoke(this, EventArgs.Empty);
        }

        private void LookUpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as OptionSet;
            item.Selected = !item.Selected;
        }

        private void Clear_Clicked(object sender, EventArgs e) => ClearData();

        public void ClearData()
        {
            if (ItemsSource == null || ItemsSource.Any() == false) return;
            this.Text = null;
            ItemsSource.ForEach(x => x.Selected = false);
            SelectedIds = null;
            OnDelete?.Invoke(this, EventArgs.Empty);
        }

        public async void setData()
        {
            if(ItemsSource == null)
            {
                if (ListListView != null && ListListView.Count > 0 && ListTab != null && ListTab.Count > 0)
                {
                    ItemSourceForTabs();
                }   
                else
                {
                    if (PreShow != null)
                    {
                        await PreShow();
                        if (PreOpenOneTime)
                        {
                            PreShow = null;
                        }
                    }
                }    
            }    

            if (this.SelectedIds != null && this.SelectedIds.Any() && ItemsSource != null)
            {
                var selectedInSource = ItemsSource.Where(x => SelectedIds.Any(s => s == x.Val)).ToList();
                foreach (var item in selectedInSource)
                {
                    item.Selected = true;
                }
                this.Text = string.Join(", ", selectedInSource.Select(x => x.Label).ToArray());

                SetList(selectedInSource);
            }
            else
            {
                this.Text = null;
                ClearFlexLayout();
            }
        }
        public void SetList(List<OptionSetFilter> selectedInSource)
        {
            this.Entry.IsVisible = false;
            this.flexLayout.IsVisible = true;
            selectedInSource.Add(new OptionSetFilter()
            {
                Val = "0"
            });

            BindableLayout.SetItemsSource(flexLayout, selectedInSource);
            var last = flexLayout.Children.Last() as StackLayout;
            //var radBorder = flexLayout.Children.Last() as RadBorder;
            var radBorder = last.Children[0] as RadBorder;
            radBorder.BackgroundColor = Color.Gray;
            (radBorder.Content as Label).Text = "\uf00d";
            (radBorder.Content as Label).FontSize = Device.RuntimePlatform == Device.iOS ? 16 : 17;
            (radBorder.Content as Label).HorizontalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).VerticalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).FontFamily = "FontAwesomeSolid";
            (radBorder.Content as Label).TextColor = Color.White;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Clear_Clicked;

            radBorder.GestureRecognizers.Add(tap);
        }
        public void ClearFlexLayout()
        {
            flexLayout.IsVisible = false;
            Entry.IsVisible = true;
        }

        private static void ItemSourceChange(BindableObject bindable, object oldValue, object value)
        {
            LookUpMultipleOptions control = (LookUpMultipleOptions)bindable;
            control.setData();            
        }
      
        private void ItemSourceForTabs()
        {
            if (ItemsSource == null || ItemsSource.Count <= 0)
            {
                ItemsSource = new List<OptionSetFilter>();
                for (int i = 0; i < ListListView.Count; i++)
                {
                    ItemsSource.AddRange(ListListView[i]);
                }
            }
        }

        public async void SetUpModal()
        {
            if (PreShow != null)
            {
                await PreShow();
                if (PreOpenOneTime)
                {
                    PreShow = null;
                }
            }

            if (init == false)
            {
                SetUpGridButton();
                SetUpListView();
                init = true;
            }
            else
            {
                if (searchBar.Text != null && searchBar.Text.Length > 0)
                {
                    searchBar.Text = "";
                }
            }

            CenterModal.CustomCloseButton(CancelButton_Clicked);
            CenterModal.Title = Placeholder;
            CenterModal.Footer = gridButton;
            CenterModal.Body = gridMain;
        }

        public Grid SetUpTabs(List<string> tabs)
        {
            ListLabelTab = new List<Label>();
            ListRadBorderTab = new List<RadBorder>();
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            for (int i = 0; i < tabs.Count; i++)
            {
                RadBorder rd = new RadBorder();
                rd.Style = (Style)Application.Current.Resources["rabBorder_Tab"];
                Label lb = new Label();
                lb.Style = (Style)Application.Current.Resources["Lb_Tab"];
                lb.Text = tabs[i];
                rd.Content = lb;
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += Tab_Tapped;
                lb.GestureRecognizers.Add(tapGestureRecognizer);
                ListLabelTab.Add(lb);
                ListRadBorderTab.Add(rd);

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.Children.Add(rd);
                Grid.SetColumn(rd, i);
                Grid.SetRow(rd, 0);
            }
            BoxView boxView = new BoxView();
            boxView.HeightRequest = 1;
            boxView.BackgroundColor = Color.FromHex("F1F1F1");
            boxView.VerticalOptions = LayoutOptions.EndAndExpand;
            grid.Children.Add(boxView);
            Grid.SetColumn(boxView, 0);
            Grid.SetRow(boxView, 0);
            Grid.SetColumnSpan(boxView, tabs.Count);
            return grid;
        }
        private void Tab_Tapped(object sender, EventArgs e)
        {
            var button = sender as Label;
            indexTab = ListRadBorderTab.IndexOf(ListRadBorderTab.FirstOrDefault(x => x.Children.Last() == button));
            IndexTab(indexTab);
        }

        private void IndexTab(int index)
        {
            if (ListRadBorderTab != null && ListRadBorderTab.Count>0)
            {
                for (int i = 0; i < ListRadBorderTab.Count; i++)
                {
                    if (i == index)
                    {
                        VisualStateManager.GoToState(ListRadBorderTab[i], "Selected");
                        VisualStateManager.GoToState(ListLabelTab[i], "Selected");
                        lookUpListView.ItemsSource = ListListView[i];
                    }
                    else
                    {
                        VisualStateManager.GoToState(ListRadBorderTab[i], "Normal");
                        VisualStateManager.GoToState(ListLabelTab[i], "Normal");
                    }
                }
            }          
        }
    }
}
