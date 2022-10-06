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
        private ContactsContentview ContactsContentview;
        private AccountsContentView AccountsContentView; 
        public CustomerPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            NeedToRefreshContact = false;
            NeedToRefreshAccount = false;
            Init();
        }
        public async void Init()
        {
            if (ContactsContentview == null)
            {
                ContactsContentview = new ContactsContentview();
            }
            ContactsContentview.OnCompleted = async (IsSuccess) =>
            {
                CustomerContentView.Children.Add(ContactsContentview);
                LoadingHelper.Hide();
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
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
                    if (ContactsContentview != null)
                    {
                        ContactsContentview.IsVisible = true;
                    }
                    if (AccountsContentView != null)
                    {
                        AccountsContentView.IsVisible = false;
                    }
                }
                else if((int)e.Item == 1)
                {
                    ContactsContentview.IsVisible = false;
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
                    AccountsContentView.IsVisible = true;
                }
            }  
        }
    }
}
