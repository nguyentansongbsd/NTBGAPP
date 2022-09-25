using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class CustomerPage : ContentPage
    {
        public static bool? NeedToRefreshLead = null;
        public static bool? NeedToRefreshContact = null;
        public static bool? NeedToRefreshAccount = null;
        private LeadsContentView LeadsContentView;
        private ContactsContentview ContactsContentview;
        private AccountsContentView AccountsContentView;
        public CustomerPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            NeedToRefreshLead = false;
            NeedToRefreshContact = false;
            NeedToRefreshAccount = false;
            Init();
        }
        public async void Init()
        {
            if (LeadsContentView == null)
            {
                LeadsContentView = new LeadsContentView();
            }
            LeadsContentView.OnCompleted = async (IsSuccess) =>
            {
                CustomerContentView.Children.Add(LeadsContentView);
                LoadingHelper.Hide();
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (LeadsContentView != null && NeedToRefreshLead == true)
            {
                LoadingHelper.Show();
                await LeadsContentView.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshLead = false;
                LoadingHelper.Hide();
            }

            if (ContactsContentview != null && NeedToRefreshContact == true)
            {
                LoadingHelper.Show();
                await ContactsContentview.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshContact = false;
                LoadingHelper.Hide();
            }

            if (AccountsContentView != null && NeedToRefreshAccount == true)
            {
                LoadingHelper.Show();
                await AccountsContentView.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshAccount = false;
                LoadingHelper.Hide();
            }
        }

        private async void NewCustomer_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.khach_hang_tiem_nang_option, Language.khach_hang_ca_nhan_option, Language.khach_hang_doanh_nghiep_option };
            string asw = await DisplayActionSheet(Language.tao_khach_hang, Language.huy, null, options);
            if (asw == Language.khach_hang_tiem_nang_option)
            {
                await Navigation.PushAsync(new LeadForm());
            }
            else if (asw == Language.khach_hang_ca_nhan_option)
            {
                await Navigation.PushAsync(new ContactForm());
            }
            else if (asw == Language.khach_hang_doanh_nghiep_option)
            {
                await Navigation.PushAsync(new AccountForm());
            }
            LoadingHelper.Hide();
        }

        private void TabControl_IndexTab(object sender, Models.LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    if (LeadsContentView != null)
                    {
                        LeadsContentView.IsVisible = true;
                    }
                    if (AccountsContentView != null)
                    {
                        AccountsContentView.IsVisible = false;
                    }
                    if (ContactsContentview != null)
                    {
                        ContactsContentview.IsVisible = false;
                    }
                }
                else if ((int)e.Item == 1)
                {
                    if (ContactsContentview == null)
                    {
                        LoadingHelper.Show();
                        ContactsContentview = new ContactsContentview();
                    }
                    ContactsContentview.OnCompleted = (IsSuccess) =>
                    {
                        CustomerContentView.Children.Add(ContactsContentview);
                        LoadingHelper.Hide();
                    };
                    LeadsContentView.IsVisible = false;
                    ContactsContentview.IsVisible = true;
                    if (AccountsContentView != null)
                    {
                        AccountsContentView.IsVisible = false;
                    }
                }
                else if ((int)e.Item == 2)
                {
                    if (AccountsContentView == null)
                    {
                        LoadingHelper.Show();
                        AccountsContentView = new AccountsContentView();
                    }
                    AccountsContentView.OnCompleted = (IsSuccess) =>
                    {
                        CustomerContentView.Children.Add(AccountsContentView);
                        LoadingHelper.Hide();
                    };
                    LeadsContentView.IsVisible = false;
                    AccountsContentView.IsVisible = true;
                    if (ContactsContentview != null)
                    {
                        ContactsContentview.IsVisible = false;
                    }
                }
            }  
        }
    }
}
