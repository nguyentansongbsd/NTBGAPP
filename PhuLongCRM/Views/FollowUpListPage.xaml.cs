using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class FollowUpListPage : ContentPage
    {
        public FollowUpListPageViewModel viewModel;
        public FollowUpListPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListPageViewModel();
            LoadingHelper.Show();
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as FollowUpModel;
            FollowDetailPage followDetailPage = new FollowDetailPage(item.bsd_followuplistid);
            followDetailPage.OnLoaded = async(isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(followDetailPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    LoadingHelper.Hide();
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
