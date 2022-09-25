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
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityList : ContentPage
    {
        public static bool? NeedToRefreshPhoneCall = null;
        public static bool? NeedToRefreshMeet = null;
        public static bool? NeedToRefreshTask = null;
        public Action<bool> OnCompleted;
        public ActivityListViewModel viewModel;
        public ActivityList()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityListViewModel();
            NeedToRefreshPhoneCall = false;
            NeedToRefreshMeet = false;
            NeedToRefreshTask = false;
            Init();
        }

        public async void Init()
        {
            viewModel.EntityName = "tasks";
            viewModel.entity = "task";
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Data != null && NeedToRefreshPhoneCall == true && viewModel.entity == "phonecall")
            {
                LoadingHelper.Show();
                viewModel.EntityName = "phonecalls";
                viewModel.entity = "phonecall";
                await viewModel.LoadOnRefreshCommandAsync();
                ActivityPopup.Refresh();
                NeedToRefreshPhoneCall = false;
                LoadingHelper.Hide();
            }
            if (viewModel.Data != null && NeedToRefreshMeet == true && viewModel.entity == "appointment")
            {
                LoadingHelper.Show();
                viewModel.EntityName = "appointments";
                viewModel.entity = "appointment";
                await viewModel.LoadOnRefreshCommandAsync();
                ActivityPopup.Refresh();
                NeedToRefreshMeet = false;
                LoadingHelper.Hide();
            }

            if (viewModel.Data != null && NeedToRefreshTask == true && viewModel.entity == "task")
            {
                LoadingHelper.Show();
                viewModel.EntityName = "tasks";
                viewModel.entity = "task";
                await viewModel.LoadOnRefreshCommandAsync();
                ActivityPopup.Refresh();
                NeedToRefreshTask = false;
                LoadingHelper.Hide();
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var item = e.Item as HoatDongListModel;
                ActivityPopup.ShowActivityPopup(item.activityid,item.activitytypecode);
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

        private async void NewActivity_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.tao_cong_viec, Language.tao_cuoc_hop, Language.tao_cuoc_goi };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.tao_cong_viec)
            {
                await Navigation.PushAsync(new TaskForm());
            }
            else if (asw == Language.tao_cuoc_goi)
            {
                await Navigation.PushAsync(new PhoneCallForm());
            }
            else if (asw == Language.tao_cuoc_hop)
            {
                await Navigation.PushAsync(new MeetingForm());
            }
            LoadingHelper.Hide();
        }
        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    LoadingHelper.Show();
                    if (viewModel.entity != "task")
                    {
                        viewModel.EntityName = "tasks";
                        viewModel.entity = "task";
                        await viewModel.LoadOnRefreshCommandAsync();
                    }
                    LoadingHelper.Hide();
                }
                else if ((int)e.Item == 1)
                {
                    LoadingHelper.Show();
                    if (viewModel.entity != "appointment")
                    {
                        viewModel.EntityName = "appointments";
                        viewModel.entity = "appointment";
                        await viewModel.LoadOnRefreshCommandAsync();
                    }

                    LoadingHelper.Hide();
                }
                else if ((int)e.Item == 2)
                {
                    LoadingHelper.Show();
                    if (viewModel.entity != "phonecall")
                    {
                        viewModel.EntityName = "phonecalls";
                        viewModel.entity = "phonecall";
                        await viewModel.LoadOnRefreshCommandAsync();
                    }
                    LoadingHelper.Hide();
                }
            }
        }
    }
}