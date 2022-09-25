using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class ContactsContentview : ContentView
    {
        public Action<bool> OnCompleted;
        public ContactsContentviewViewmodel viewModel;
        public ContactsContentview()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactsContentviewViewmodel();
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
            var item = e.Item as ContactListModel;
            ContactDetailPage newPage = new ContactDetailPage(item.contactid);
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
        private void Filter_Tapped(object sender, EventArgs e)
        {
            FilterView.IsVisible = !FilterView.IsVisible;
        }

        private async void SelectFilter_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (TapGestureRecognizer)((Label)sender).GestureRecognizers[0];
            viewModel.KeyFilter = item.CommandParameter as string;
            // thay đổi icon
            if(viewModel.KeyFilter == "1")
            {
                label_official.FontAttributes = FontAttributes.Bold;
                label_potential.FontAttributes = FontAttributes.None;
                label_All.FontAttributes = FontAttributes.None;

                label_official.TextColor = Color.FromHex("1399D5");
                label_potential.TextColor = Color.FromHex("444444");
                label_All.TextColor = Color.FromHex("444444");
            }
            else if (viewModel.KeyFilter == "2")
            {
                label_potential.FontAttributes = FontAttributes.Bold;
                label_official.FontAttributes = FontAttributes.None;
                label_All.FontAttributes = FontAttributes.None;

                label_potential.TextColor = Color.FromHex("1399D5");
                label_official.TextColor = Color.FromHex("444444");
                label_All.TextColor = Color.FromHex("444444");
            }
            else if (viewModel.KeyFilter == "0")
            {
                label_All.FontAttributes = FontAttributes.Bold;
                label_potential.FontAttributes = FontAttributes.None;
                label_official.FontAttributes = FontAttributes.None;

                label_potential.TextColor = Color.FromHex("444444");
                label_official.TextColor = Color.FromHex("444444");
                label_All.TextColor = Color.FromHex("1399D5");
            }
            await viewModel.LoadOnRefreshCommandAsync();
            FilterView.IsVisible = false;
            LoadingHelper.Hide();
        }
    }
}
