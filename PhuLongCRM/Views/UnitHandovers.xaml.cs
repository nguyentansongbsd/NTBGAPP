using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class UnitHandovers : ContentPage
    {
        public static bool? NeedRefresh { get; set; } = null;
        public UnitHandoversViewModel viewModel;
        public UnitHandovers()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitHandoversViewModel();
            NeedRefresh = false;
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedRefresh.HasValue && NeedRefresh.Value == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadOnRefreshCommandAsync();
                LoadingHelper.Hide();
            }
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

        private void BsdListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            Guid id = ((UnitHandoversModel)e.Item).bsd_handoverid;
            UnitHandoverPage unitHandover = new UnitHandoverPage(id);
            unitHandover.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
                {
                    await Navigation.PushAsync(unitHandover);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
    }
}
