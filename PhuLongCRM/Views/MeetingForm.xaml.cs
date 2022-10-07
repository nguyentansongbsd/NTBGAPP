using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeetingForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public MeetingViewModel viewModel;
        private Guid MeetId; 
        private bool IsInit;
        public MeetingForm()
        {
            InitializeComponent();
            Init();
            SetPreOpen();
            Create();
        }

        public MeetingForm(Guid id)
        {
            InitializeComponent();
            Init();
            SetPreOpen();
            MeetId = id;
            Update();
        }

        private void Init()
        {
            LoadingHelper.Show();
            BindingContext = viewModel = new MeetingViewModel();
            // kiểm tra page trước là page nào
            var page_before = App.Current.MainPage.Navigation.NavigationStack.Last()?.GetType().Name;
            if(page_before == "ContactDetailPage" || page_before == "AccountDetailPage" 
                || page_before == "LeadDetailPage" || page_before == "QueuesDetialPage")
            {
                if (page_before == "ContactDetailPage" && ContactDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(ContactDetailPage.FromCustomer.Val))
                {
                    viewModel.CustomerMapping = ContactDetailPage.FromCustomer;
                    Lookup_Required.IsVisible = false;
                    RequiredMapping.IsVisible = true;
                    Lookup_Customer.IsVisible = false;
                    RegardingMapping.IsVisible = true;
                    Lookup_Option.ne_customer = Guid.Parse(viewModel.CustomerMapping.Val);
                }
                else if (page_before == "LeadDetailPage" && LeadDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(LeadDetailPage.FromCustomer.Val))
                {
                    viewModel.CustomerMapping = LeadDetailPage.FromCustomer;
                    Lookup_Required.IsVisible = false;
                    RequiredMapping.IsVisible = true;
                    Lookup_Customer.IsVisible = false;
                    RegardingMapping.IsVisible = true;
                    Lookup_Option.ne_customer = Guid.Parse(viewModel.CustomerMapping.Val);
                }
                else if (page_before == "AccountDetailPage" && AccountDetailPage.FromCustomer != null && !string.IsNullOrWhiteSpace(AccountDetailPage.FromCustomer.Val))
                {
                    viewModel.CustomerMapping = AccountDetailPage.FromCustomer;
                    Lookup_Required.IsVisible = false;
                    RequiredMapping.IsVisible = true;
                    Lookup_Customer.IsVisible = false;
                    RegardingMapping.IsVisible = true;
                    Lookup_Option.ne_customer = Guid.Parse(viewModel.CustomerMapping.Val);
                }
                //else if (page_before == "QueuesDetialPage" && QueuesDetialPage.FromQueue != null && !string.IsNullOrWhiteSpace(QueuesDetialPage.FromQueue.Val))
                //{
                //    viewModel.CustomerMapping = QueuesDetialPage.FromQueue;
                //    viewModel.Customer = QueuesDetialPage.CustomerFromQueue;
                //    viewModel.Customer.Selected = true; // phân biệt customer là required của queue
                //    lb_requiredMapping.Text = QueuesDetialPage.CustomerFromQueue.Label;
                //    Lookup_Option.ne_customer = Guid.Parse(QueuesDetialPage.CustomerFromQueue.Val);
                //    Lookup_Required.IsVisible = false;
                //    RequiredMapping.IsVisible = true;
                //    Lookup_Customer.IsVisible = false;
                //    RegardingMapping.IsVisible = true;
                //}
                else
                {
                    Lookup_Required.IsVisible = true;
                    RequiredMapping.IsVisible = false;
                    Lookup_Customer.IsVisible = true;
                    RegardingMapping.IsVisible = false;
                }
            }
            else
            {
                Lookup_Required.IsVisible = true;
                RequiredMapping.IsVisible = false;
                Lookup_Customer.IsVisible = true;
                RegardingMapping.IsVisible = false;
            }
        }

        private void SetPreOpen()
        {
            lookupCollectionType.PreOpenAsync = async () => {
                viewModel.CollectionTypes = CollectionTypeData.GetColectionData();
            };
            lookupProject.PreOpenAsync = async () => {
                await viewModel.LoadProjects();
            };
            lookupContract.PreOpenAsync = async () => {
                await viewModel.LoadContracts();
            };
        }

        private void Create()
        {
            this.Title = BtnSave.Text = Language.tao_moi_cuoc_hop_title;
            IsInit = true;
            BtnSave.Clicked += Create_Clicked;
        }

        private void Create_Clicked(object sender, EventArgs e)
        {
            SaveData(Guid.Empty);
        }

        private async void Update()
        {
            this.Title = BtnSave.Text = Language.cap_nhat_cuoc_hop_title;
            BtnSave.Clicked += Update_Clicked;
            await viewModel.loadDataMeet(this.MeetId);

            if (viewModel.MeetingModel.activityid != Guid.Empty)
            {
                viewModel.CollectionType = CollectionTypeData.GetCollectionTypeById(viewModel.MeetingModel.bsd_collectiontype);
                DatePickerStart.ReSetTime();
                DatePickerEnd.ReSetTime();
                IsInit = true;
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            SaveData(this.MeetId);
        }

        private async void SaveData(Guid id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.MeetingModel.subject))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_chu_de_cuoc_hop);
                return;
            }
            if (viewModel.CustomerMapping == null)
            {
                if (viewModel.Required == null || viewModel.Required.Count <= 0)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_nguoi_tham_du_bat_buoc);
                    return;
                }
            }

            if (viewModel.CollectionType == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_lich_hen);
                return;
            }

            if (viewModel.MeetingModel.scheduledstart == null || viewModel.MeetingModel.scheduledend == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_va_thoi_gian_bat_dau);
                return;
            }
            if (viewModel.MeetingModel.scheduledstart != null && viewModel.MeetingModel.scheduledend != null)
            {
                if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                    return;
                }
            }

            if (DatePickerStart.IsTimeNull)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                return;
            }

            if (DatePickerEnd.IsTimeNull)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc);
                return;
            }

            if (viewModel.CustomerMapping == null)
            {
                if (viewModel.Optional != null && viewModel.Optional.Count > 0)
                {
                    if (!CheckCusomer(viewModel.Required, viewModel.Optional))
                    {
                        ToastMessageHelper.ShortMessage(Language.nguoi_tham_du_bat_buoc_phai_khac_nguoi_tham_du_khong_bat_buoc);
                        return;
                    }
                }
            }
            else
            {
                if (viewModel.Optional != null && viewModel.Optional.Count > 0)
                {
                    if (!CheckCusomer(null, viewModel.Optional, viewModel.CustomerMapping))
                    {
                        ToastMessageHelper.ShortMessage(Language.nguoi_tham_du_bat_buoc_phai_khac_nguoi_tham_du_khong_bat_buoc);
                        return;
                    }
                }
            }        

            LoadingHelper.Show();

            if (id == Guid.Empty)
            {
                if (await viewModel.createMeeting())
                {
                    if (Dashboard.NeedToRefreshMeet.HasValue) Dashboard.NeedToRefreshMeet = true;
                    if (ActivityList.NeedToRefreshMeet.HasValue) ActivityList.NeedToRefreshMeet = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    //if (QueuesDetialPage.NeedToRefreshActivity.HasValue) QueuesDetialPage.NeedToRefreshActivity = true;
                    ToastMessageHelper.ShortMessage(Language.tao_cuoc_hop_thanh_cong);
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
                if (await viewModel.UpdateMeeting(id))
                {
                    if (Dashboard.NeedToRefreshMeet.HasValue) Dashboard.NeedToRefreshMeet = true;
                    if (ActivityList.NeedToRefreshMeet.HasValue) ActivityList.NeedToRefreshMeet = true;
                    if (LichLamViecTheoThang.NeedToRefresh.HasValue) LichLamViecTheoThang.NeedToRefresh = true;
                    if (LichLamViecTheoTuan.NeedToRefresh.HasValue) LichLamViecTheoTuan.NeedToRefresh = true;
                    if (LichLamViecTheoNgay.NeedToRefresh.HasValue) LichLamViecTheoNgay.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    if (AccountDetailPage.NeedToRefreshActivity.HasValue) AccountDetailPage.NeedToRefreshActivity = true;
                    if (LeadDetailPage.NeedToRefreshActivity.HasValue) LeadDetailPage.NeedToRefreshActivity = true;
                    //if (QueuesDetialPage.NeedToRefreshActivity.HasValue) QueuesDetialPage.NeedToRefreshActivity = true;
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
                if (viewModel.MeetingModel.scheduledend != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        //viewModel.MeetingModel.scheduledstart = viewModel.MeetingModel.scheduledend;
                    }
                    if (viewModel.MeetingModel.isalldayevent)
                    {
                        viewModel.MeetingModel.isalldayevent = false;
                    }
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend.Value.AddDays(-1)) == 0)
                    {
                        viewModel.MeetingModel.isalldayevent = true;
                    }
                }
            }
        }

        private void DatePickerEnd_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.MeetingModel.scheduledstart != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        //viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart;
                    }
                    if (viewModel.MeetingModel.isalldayevent)
                    {
                        viewModel.MeetingModel.isalldayevent = false;
                    }
                    // chưa word ok, do control
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend.Value.AddDays(-1)) == 0)
                    {
                        viewModel.MeetingModel.isalldayevent = true;
                    }
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                }
            }
        }

        private void AllDayEvent_changeChecked(object sender, EventArgs e)
        {
            if (viewModel.MeetingModel.scheduledstart != null)
            {
                if (viewModel.MeetingModel.isalldayevent)
                {
                    var timeStart = viewModel.MeetingModel.scheduledstart.Value;
                    viewModel.MeetingModel.timeStart = new TimeSpan(timeStart.Hour, timeStart.Minute, timeStart.Second);
                    if (viewModel.MeetingModel.scheduledend != null)
                    {
                        var actualdurationminutes = Math.Round((viewModel.MeetingModel.scheduledend.Value - viewModel.MeetingModel.scheduledstart.Value).TotalMinutes);
                        viewModel.MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
                    }
                    else
                    {
                        viewModel.MeetingModel.scheduleddurationminutes = 0;
                    }

                    viewModel.MeetingModel.scheduledstart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 7, 0, 0);
                    DatePickerStart.ReSetTime();
                    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddDays(1);
                    DatePickerEnd.ReSetTime();
                }
                else
                {
                    //var dateStart = viewModel.MeetingModel.scheduledstart.Value;
                    //TimeSpan timeStart = viewModel.MeetingModel.timeStart;

                    //viewModel.MeetingModel.scheduledstart = null;
                    //if (viewModel.MeetingModel.timeStart != new TimeSpan(0, 0, 0))
                    //    viewModel.MeetingModel.scheduledstart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
                    //else
                    //    viewModel.MeetingModel.scheduledstart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, dateStart.Hour, dateStart.Minute, dateStart.Second);

                    //viewModel.MeetingModel.scheduledend = null;
                    //if (viewModel.MeetingModel.scheduleddurationminutes > 0)
                    //    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(viewModel.MeetingModel.scheduleddurationminutes);
                    //else
                    //    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(1);
                }
            }
            else
            {
                viewModel.MeetingModel.isalldayevent = false;
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
            }    
        }

        private bool CheckCusomer(List<OptionSetFilter> required = null, List<OptionSetFilter> option = null, OptionSet customer = null)
        {
            // kiểm tra từ kh hàng- kh liên quan k check
            if (required != null && option != null)
            {
                if (required.Where(x => option.Any(s => s == x)).ToList().Count > 0)
                    return false;
                else
                    return true;
            }
            else if (required != null && customer != null)
            {
                if (required.Where(x => x.Val == customer.Val).ToList().Count > 0)
                    return false;
                else
                    return true;
            }
            else if (option != null && customer != null)
            {
                if (option.Where(x => x.Val == customer.Val).ToList().Count > 0)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        private void ClearDate_Clicked(object sender, EventArgs e)
        {
            viewModel.MeetingModel.isalldayevent = false;
        }

        private async void lookupProject_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            await viewModel.LoadContracts();
        }

        private void lookupContract_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.Contract != null)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.Contract.account_name))
                    viewModel.CustomerMapping = new OptionSet { Label = viewModel.Contract.account_name, Val = viewModel.Contract.customerid.ToString(), Title = viewModel.CodeAccount };
                else if (!string.IsNullOrWhiteSpace(viewModel.Contract.contact_name))
                    viewModel.CustomerMapping = new OptionSet { Label = viewModel.Contract.contact_name, Val = viewModel.Contract.customerid.ToString(), Title = viewModel.CodeContac };
                if (viewModel.Contract.project_id != Guid.Empty)
                    viewModel.Project = new OptionSet { Val = viewModel.Contract.project_id.ToString(), Label = viewModel.Contract.project_name };
                if (viewModel.Contract.unit_id != Guid.Empty)
                    viewModel.Unit = new OptionSet { Val = viewModel.Contract.unit_id.ToString(), Label = viewModel.Contract.unit_name };
            }
        }

        private void lookupCollectionType_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if(viewModel.CollectionType != null)
            {
                if(viewModel.CollectionType.Val == "100000001" || viewModel.CollectionType.Val == "100000003" || viewModel.CollectionType.Val == "100000000")
                {
                    Lookup_Customer.IsVisible = false;
                    RegardingMapping.IsVisible = true;
                }   
                else
                {
                    Lookup_Customer.IsVisible = true;
                    RegardingMapping.IsVisible = false;
                }    
            }    
        }
    }
}