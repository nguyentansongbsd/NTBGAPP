using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadsContentView : ContentView
    {
        public LeadsContentViewViewModel viewModel;
        public Action<bool> OnCompleted;
        public LeadsContentView()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LeadsContentViewViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            if (viewModel.Data.Count > 0)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as LeadListModel;
            LeadDetailPage newPage = new LeadDetailPage(item.leadid);
            newPage.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(newPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private async void Search_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                Search_Pressed(null, EventArgs.Empty);
            }
        }

        private void Sort_Tapped(object sender, EventArgs e)
        {
            SortView.IsVisible = !SortView.IsVisible;
        }

        private async void SelectSort_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (TapGestureRecognizer)((Label)sender).GestureRecognizers[0];
            viewModel.KeySort = item.CommandParameter as string;
            // thay đổi icon
            if (viewModel.KeySort == "1")
            {
                viewModel.Create_on_sort = !viewModel.Create_on_sort;
                if (viewModel.Create_on_sort)
                {
                    icon_createon.Text = "\uf15d";
                    label_createon.Text = Language.ngay_tao_a_z;
                }
                else
                {
                    icon_createon.Text = "\uf882";
                    label_createon.Text = Language.ngay_tao_z_a;
                }
                label_createon.TextColor = Color.FromHex("1399D5");
                label_rating.TextColor = Color.FromHex("444444");
                label_status.TextColor = Color.FromHex("444444");
            }
            else if (viewModel.KeySort == "2")
            {
                viewModel.Rating_sort = !viewModel.Rating_sort;
                if (viewModel.Rating_sort)
                {
                    icon_rating.Text = "\uf15d";
                    label_rating.Text = Language.danh_gia_a_z;
                }
                else
                {
                    icon_rating.Text = "\uf882";
                    label_rating.Text = Language.danh_gia_z_a;
                }
                label_rating.TextColor = Color.FromHex("1399D5");
                label_createon.TextColor = Color.FromHex("444444");
                label_status.TextColor = Color.FromHex("444444");
            }
            else if (viewModel.KeySort == "3")
            {
                viewModel.Status_sort = !viewModel.Status_sort;
                if (viewModel.Status_sort)
                {
                    icon_status.Text = "\uf15d";
                    label_status.Text = Language.tinh_trang_a_z;
                }
                else
                {
                    icon_status.Text = "\uf882";
                    label_status.Text = Language.tinh_trang_z_a;
                }
                label_status.TextColor = Color.FromHex("1399D5");
                label_rating.TextColor = Color.FromHex("444444");
                label_createon.TextColor = Color.FromHex("444444");
            }
            await viewModel.LoadOnRefreshCommandAsync();
            SortView.IsVisible = false;
            LoadingHelper.Hide();
        }
    }
}