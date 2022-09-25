using PhuLongCRM.Config;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListTaiLieuKinhDoanh : ContentPage
    {
        private readonly ListTaiLieuKinhDoanhViewModel viewModel;
        public ListTaiLieuKinhDoanh()
        {
            InitializeComponent();
            BindingContext = viewModel = new ListTaiLieuKinhDoanhViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }
        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListTaiLieuKinhDoanhModel val = e.Item as ListTaiLieuKinhDoanhModel;
            LoadingHelper.Show();
            TaiLieuKinhDoanhForm newPage = new TaiLieuKinhDoanhForm(val.salesliteratureid);
            newPage.CheckTaiLieuKinhDoanh = async (CheckTaiLieuKinhDoanh) =>
            {
                if (CheckTaiLieuKinhDoanh == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                LoadingHelper.Hide();
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
