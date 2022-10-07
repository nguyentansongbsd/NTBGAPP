using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewMultiTabs : Grid
    {
        public ListViewMultiTabsViewModel viewModel;
        public Action<OptionSetFilter> ItemTapped { get; set; }
        public ListViewMultiTabs(string fetch, string entity, bool isSelect = false, List<OptionSetFilter> itemselected = null, bool moreInfo = false)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ListViewMultiTabsViewModel(fetch,entity);
            if (itemselected != null && itemselected.Count > 0)
                viewModel.ItemSelecteds = itemselected;
            this.SetUpDataTemplate(isSelect, moreInfo);
            this.LoadData();
        }
        private async void LoadData()
        {
            LoadingHelper.Show();
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }     
        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }
        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Key))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
            //var text = e.NewTextValue;
            //if (string.IsNullOrWhiteSpace(text))
            //{
            //    listView.ItemsSource = viewModel.Data;
            //}
            //else
            //{
            //    var list = from Item in viewModel.Data
            //               where Item.Label.ToString().ToLower().Contains(text.ToLower()) ||
            //               Item.SDT != null && Item.SDT.ToString().ToLower().Contains(text.ToLower()) ||
            //               Item.CMND != null && Item.CMND.ToString().ToLower().Contains(text.ToLower()) ||
            //               Item.CCCD != null && Item.CCCD.ToString().ToLower().Contains(text.ToLower()) ||
            //               Item.HC != null && Item.HC.ToString().ToLower().Contains(text.ToLower()) ||
            //               Item.SoGPKD != null && Item.SoGPKD.ToString().ToLower().Contains(text.ToLower())
            //               select Item;
            //    listView.ItemsSource = list;
            //    //listView.ItemsSource = viewModel.Data.Where(x => x.Label.ToLower().Contains(text.ToLower()));
            //}
        }
        public async void Refresh()
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }
        private void SetUpDataTemplate(bool isSelect, bool moreInfo)
        {
            var dataTemplate = new DataTemplate(() =>
            {
                TapGestureRecognizer item = new TapGestureRecognizer();
                item.Tapped += Item_Tapped;
                item.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding("."));

                Grid grid = new Grid();
                grid.BackgroundColor = Color.White; //Color.FromHex("#eeeeee");
                //grid.Padding = new Thickness(1, 0, 0, 0);
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                grid.Margin = new Thickness(0, 1, 0, 0);
                grid.Padding = new Thickness(10, 5, 10, 5);

                Label lb = new Label();
                //   lb.TextColor = (Color)App.Current.Resources["NavigationPrimary"];
                lb.TextColor = Color.FromHex("#444444");
                lb.FontSize = 15;
                // lb.FontAttributes = FontAttributes.Bold;
                lb.FontAttributes = FontAttributes.None;
                lb.SetBinding(Label.TextProperty, "Label");
                lb.BackgroundColor = Color.White;
                lb.VerticalOptions = LayoutOptions.Center;
                lb.LineBreakMode = LineBreakMode.TailTruncation;
                //  lb.Padding = 10;

                grid.Children.Add(lb);
                Grid.SetColumn(lb, 1);
                Grid.SetRow(lb, 0);

                Label lb_code = new Label();
                lb_code.TextColor = Color.Gray;
                lb_code.FontSize = 15;
                lb_code.SetBinding(Label.TextProperty, "CustomerCode");
                // lb_code.SetBinding(Label.TextProperty, new Binding("CustomerCode") { Source = grid, StringFormat = "({0})" });
                lb_code.BackgroundColor = Color.White;
                lb_code.VerticalOptions = LayoutOptions.Center;
                //  lb.Padding = 10;

                grid.Children.Add(lb_code);
                Grid.SetColumn(lb_code, 0);
                Grid.SetRow(lb_code, 0);

                if (moreInfo)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    FieldListViewItem lb_sdt = new FieldListViewItem();
                    lb_sdt.Title = Language.so_dien_thoai;
                    lb_sdt.SetBinding(FieldListViewItem.TextProperty, "SDT");
                    grid.Children.Add(lb_sdt);
                    Grid.SetColumn(lb_sdt, 0);
                    Grid.SetRow(lb_sdt, 1);
                    Grid.SetColumnSpan(lb_sdt, 2);

                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    FieldListViewItem lb_id = new FieldListViewItem();
                    lb_id.SetBinding(FieldListViewItem.TextProperty, "SoID");
                    lb_id.SetBinding(FieldListViewItem.TitleProperty, "TitleID");
                    grid.Children.Add(lb_id);
                    Grid.SetColumn(lb_id, 0);
                    Grid.SetRow(lb_id, 2);
                    Grid.SetColumnSpan(lb_id, 2);
                }

                if (isSelect == true)
                {
                    CheckBox checkbox = new CheckBox();
                   // checkbox.Color = Color.FromHex("#2196F3");
                    checkbox.SetBinding(CheckBox.IsCheckedProperty, "Selected");
                    checkbox.HorizontalOptions = LayoutOptions.End;
                    checkbox.VerticalOptions = LayoutOptions.Center;
                    checkbox.BackgroundColor = Color.White;
                    checkbox.Margin = new Thickness(0, 0, 10, 0);
                    checkbox.IsEnabled = false;
                    checkbox.Margin = 0;

                    grid.Children.Add(checkbox);
                    Grid.SetColumn(checkbox, 0);
                    Grid.SetRow(checkbox, 0);
                }

                Grid grid_tap = new Grid();
                grid_tap.GestureRecognizers.Add(item);
                grid.Children.Add(grid_tap);
                Grid.SetColumn(grid_tap, 0);
                Grid.SetRow(grid_tap, 0);
                Grid.SetColumnSpan(grid_tap, 2);
                if (moreInfo)
                {
                    Grid.SetRowSpan(grid_tap, 3);
                }

                return new ViewCell { View = grid };
            });

            listView.ItemTemplate = dataTemplate;
            listView.BackgroundColor = Color.FromHex("#eeeeee");
        }
        private void Item_Tapped(object sender, EventArgs e)
        {
            var item = (OptionSetFilter)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (searchBar.IsFocused)
            {
                searchBar.Unfocus();
            }
            ItemTapped?.Invoke(item);
        }
    }
}