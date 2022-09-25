using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneCallForm : ContentPage
    {
        public Action<bool> OnCompleted;
        private bool IsInit;
        public PhoneCallViewModel viewModel;
        private Guid PhoneCallId;

        public PhoneCallForm()
        {
            InitializeComponent();
            Init();
            Create();
        }
        public PhoneCallForm(Guid id)
        {
            InitializeComponent();
            Init();
            PhoneCallId = id;
            Update();
        }

        private void Init()
        {
            LoadingHelper.Show();
            BindingContext = viewModel = new PhoneCallViewModel();
            // kiểm tra page trước là page nào
            var page_before = App.Current.MainPage.Navigation.NavigationStack.Last()?.GetType().Name;
            if (page_before == "ContactDetailPage" || page_before == "AccountDetailPage"
                || page_before == "LeadDetailPage" || page_before == "QueuesDetialPage")
            {
                if (page_before == "ContactDetailPage" && ContactDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(ContactDetailPage.FromCustomer.Val))
                {
                    viewModel.CallTo = ContactDetailPage.FromCustomer;
                    viewModel.Customer = ContactDetailPage.FromCustomer;
                    Lookup_Customer.IsVisible = false;
                    CustomerMapping.IsVisible = true;
                    Lookup_CallTo.IsVisible = false;
                    CustomerMapping2.IsVisible = true;
                    Lookup_CallTo_SelectedItemChange(null, null);
                }
                else if (page_before == "AccountDetailPage" && AccountDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(AccountDetailPage.FromCustomer.Val))
                {
                    viewModel.CallTo = AccountDetailPage.FromCustomer;
                    viewModel.Customer = AccountDetailPage.FromCustomer;
                    Lookup_Customer.IsVisible = false;
                    CustomerMapping.IsVisible = true;
                    Lookup_CallTo.IsVisible = false;
                    CustomerMapping2.IsVisible = true;
                    Lookup_CallTo_SelectedItemChange(null, null);
                }
                else if (page_before == "LeadDetailPage" && LeadDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(LeadDetailPage.FromCustomer.Val))
                {
                    viewModel.CallTo = LeadDetailPage.FromCustomer;
                    viewModel.Customer = LeadDetailPage.FromCustomer;
                    Lookup_Customer.IsVisible = false;
                    CustomerMapping.IsVisible = true;
                    Lookup_CallTo.IsVisible = false;
                    CustomerMapping2.IsVisible = true;
                    Lookup_CallTo_SelectedItemChange(null, null);
                }
                else if (page_before == "QueuesDetialPage" && QueuesDetialPage.FromQueue != null && !string.IsNullOrWhiteSpace(QueuesDetialPage.FromQueue.Val))
                {
                    viewModel.Customer = QueuesDetialPage.FromQueue;
                    viewModel.CallTo = QueuesDetialPage.CustomerFromQueue;
                    Lookup_Customer.IsVisible = false;
                    CustomerMapping2.IsVisible = true;
                    Lookup_CallTo.IsVisible = true;
                    CustomerMapping.IsVisible = false;
                    Lookup_CallTo_SelectedItemChange(null, null);
                }
                else
                {
                    Lookup_Customer.IsVisible = true;
                    CustomerMapping.IsVisible = false;
                    Lookup_CallTo.IsVisible = true;
                    CustomerMapping2.IsVisible = false;
                }
            }
            else
            {
                Lookup_Customer.IsVisible = true;
                CustomerMapping.IsVisible = false;
                Lookup_CallTo.IsVisible = true;
                CustomerMapping2.IsVisible = false;
            }
        }

        private void Create()
        {
            this.Title = BtnSave.Text = Language.tao_moi_cuoc_goi_title;
            IsInit = true;
            BtnSave.Clicked += Create_Clicked;
        }

        private void Create_Clicked(object sender, EventArgs e)
        {
            SaveData(Guid.Empty);
        }

        private async void Update()
        {
            this.Title = BtnSave.Text = Language.cap_nhat_cuoc_goi_title;
            BtnSave.Clicked += Update_Clicked;
            await viewModel.loadPhoneCall(this.PhoneCallId);
            await viewModel.loadFromTo(this.PhoneCallId);
            if (viewModel.PhoneCellModel.activityid != Guid.Empty)
            {
                DatePickerStart.ReSetTime();
                DatePickerEnd.ReSetTime();
                OnCompleted?.Invoke(true);
                IsInit = true;
            }
            else
                OnCompleted?.Invoke(false);
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            SaveData(this.PhoneCallId);
        }

        private async void SaveData(Guid id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.PhoneCellModel.subject))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_chu_de_cuoc_goi);
                return;
            }
            if (viewModel.CallTo == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_nguoi_nhan_cuoc_goi);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.PhoneCellModel.phonenumber))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai);
                return;
            }
            if (viewModel.PhoneCellModel.scheduledstart == null || viewModel.PhoneCellModel.scheduledend == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_va_thoi_gian_bat_dau);
                return;
            }

            if (DatePickerStart.IsTimeNull)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                return;
            }

            if (viewModel.PhoneCellModel.scheduledstart != null && viewModel.PhoneCellModel.scheduledend != null)
            {
                if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart, viewModel.PhoneCellModel.scheduledend) != -1)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                    return;
                }
            }

            if (DatePickerEnd.IsTimeNull)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc);
                return;
            }

            //if (viewModel.Customer != null && viewModel.Customer.Val == viewModel.CallTo.Val)
            //{
            //    ToastMessageHelper.ShortMessage(Language.nguoi_nhan_cuoc_goi_phai_khac_nguoi_lien_quan);
            //    return;
            //}

            LoadingHelper.Show();

            if (id == Guid.Empty)
            {
                if (await viewModel.createPhoneCall())
                {
                    if (Dashboard.NeedToRefreshPhoneCall.HasValue) Dashboard.NeedToRefreshPhoneCall = true;
                    if (ActivityList.NeedToRefreshPhoneCall.HasValue) ActivityList.NeedToRefreshPhoneCall = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    if (QueuesDetialPage.NeedToRefreshActivity.HasValue) QueuesDetialPage.NeedToRefreshActivity = true;
                    ToastMessageHelper.ShortMessage(Language.tao_cuoc_goi_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                }
            }
            else
            {
                if (await viewModel.UpdatePhoneCall(id))
                {
                    if (Dashboard.NeedToRefreshPhoneCall.HasValue) Dashboard.NeedToRefreshPhoneCall = true;
                    if (ActivityList.NeedToRefreshPhoneCall.HasValue) ActivityList.NeedToRefreshPhoneCall = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    if (QueuesDetialPage.NeedToRefreshActivity.HasValue) QueuesDetialPage.NeedToRefreshActivity = true;
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_that_bai);
                }
            }
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date1 != null)
            {
                DateTime timeStart = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute, 0);
                DateTime timeEnd = new DateTime(date1.Value.Year, date1.Value.Month, date1.Value.Day, date1.Value.Hour, date1.Value.Minute, 0);
                int result = DateTime.Compare(timeStart, timeEnd);
                if (result < 0)
                    return -1;
                else if (result == 0)
                    return 0;
                else
                    return 1;
            }
            if (date == null && date1 != null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }

        private void DatePickerStart_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.PhoneCellModel.scheduledend != null)
                {
                    if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart, viewModel.PhoneCellModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        viewModel.PhoneCellModel.scheduledstart = viewModel.PhoneCellModel.scheduledend;
                    }
                }
            }
        }

        private void DatePickerEnd_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.PhoneCellModel.scheduledstart != null)
                {
                    if (this.compareDateTime(viewModel.PhoneCellModel.scheduledstart, viewModel.PhoneCellModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        viewModel.PhoneCellModel.scheduledend = viewModel.PhoneCellModel.scheduledstart;
                    }
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                }
            }
        }

        private async void Lookup_CallTo_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.CallTo != null)
            {
                if (viewModel.CallTo.Title == viewModel.CodeLead)
                {
                    await viewModel.LoadOneLead(viewModel.CallTo.Val);
                }
                else if (viewModel.CallTo.Title == viewModel.CodeContac)
                {
                    await viewModel.loadOneContact(viewModel.CallTo.Val);
                }
                else if (viewModel.CallTo.Title == viewModel.CodeAccount)
                {
                    await viewModel.LoadOneAccount(viewModel.CallTo.Val);
                }
            }
            else
            {
                viewModel.PhoneCellModel.phonenumber = string.Empty;
            }
        }
    }
}