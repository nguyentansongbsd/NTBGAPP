using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptanceList : ContentPage
    {
        private readonly AcceptanceListViewModel viewModel;
        public static bool? NeedToRefresh = null;
        public AcceptanceList()
        {
            InitializeComponent();
            BindingContext = viewModel = new AcceptanceListViewModel();
            NeedToRefresh = false;
            Init();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                await viewModel.LoadOnRefreshCommandAsync();
                NeedToRefresh = false;
            }
        }

        public async void Init()
        {
            LoadingHelper.Show();
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            AcceptanceListModel item = e.Item as AcceptanceListModel;
            AcceptanceDetailPage page = new AcceptanceDetailPage(item.bsd_acceptanceid);
            page.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(page);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }

            };
        }
        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }
        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }
    }
}