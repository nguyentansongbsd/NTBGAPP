using FFImageLoading.Forms;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadDetailPage : ContentPage
    {
        public static bool? NeedToRefreshLeadDetail = null;
        public Action<bool> OnCompleted;
        private LeadDetailPageViewModel viewModel;
        private Guid Id;
        public static OptionSet FromCustomer = null;
        public static bool? NeedToRefreshActivity = null;
        public LeadDetailPage(Guid id,bool isFromQRCode = false)
        {
            InitializeComponent();
            this.Title = Language.thong_tin_khach_hang_title;
            this.Id = id;
            this.BindingContext = viewModel = new LeadDetailPageViewModel();
            viewModel.IsFromQRCode = isFromQRCode;
            NeedToRefreshLeadDetail = false;
            NeedToRefreshActivity = false;
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());
            SetButtonFloatingButton(viewModel.singleLead);

            if (viewModel.singleLead.leadid != Guid.Empty)
            {
                FromCustomer = new OptionSet { Val = viewModel.singleLead.leadid.ToString(), Label = viewModel.singleLead.lastname, Title = viewModel.CodeLead };
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        protected async override void OnAppearing()
        {
            if (NeedToRefreshLeadDetail == true)
            {
                await viewModel.LoadOneLead(Id.ToString());
                NeedToRefreshLeadDetail = false;
            }
            if (NeedToRefreshActivity == true)
            {
                LoadingHelper.Show();
                //viewModel.PageCase = 1;
                //viewModel.Cares?.Clear();
                //await viewModel.LoadCase();
                viewModel.PageMeetings = 1;
                viewModel.Meetings.Clear();
                await viewModel.LoadMeetings();
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
            base.OnAppearing();
        }

        private void SetButtonFloatingButton(LeadFormModel lead)
        {
            //if (lead != null)
            //{
            //    viewModel.ButtonCommandList.Clear();
            //    if (string.IsNullOrWhiteSpace(lead.bsd_qrcode) && lead.statuscode != "3" && lead.statuscode != "4" && lead.statuscode != "5" && lead.statuscode != "6" && lead.statuscode != "7")
            //    {
            //        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_qr_code, "FontAwesomeSolid", "\uf029", null, GenerateQRCode));
            //    }

            //    if (lead.statuscode == "3") // qualified
            //    {
            //        if (lead.account_id != Guid.Empty)
            //        {
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.di_den_kh_doanh_nghiep, "FontAwesomeRegular", "\uf1ad", null, GoToAccount));
            //            floatingButtonGroup.IsVisible = true;
            //        }
            //        if (lead.contact_id != Guid.Empty)
            //        {
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.di_den_kh_ca_nhan, "FontAwesomeRegular", "\uf2c1", null, GoToContact));
            //            floatingButtonGroup.IsVisible = true;
            //        }
            //    }
            //    else if (lead.statuscode == "4" || lead.statuscode == "5" || lead.statuscode == "6" || lead.statuscode == "7")
            //    {
            //        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.kich_hoat_lai_kh, "FontAwesomeSolid", "\uf1b8", null, ReactivateLead));
            //    }
            //    else
            //    {
            //        // hỏi lại sts
            //        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
            //        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
            //        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));
            //        if (lead.leadqualitycode == 3)
            //        {
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.khong_chuyen_doi, "FontAwesomeSolid", "\uf05e", null, LeadDisQualify));
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat, "FontAwesomeRegular", "\uf044", null, Update));
            //        }
            //        else
            //        {
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chuyen_doi_khach_hang, "FontAwesomeSolid", "\uf542", null, LeadQualify));
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.khong_chuyen_doi, "FontAwesomeSolid", "\uf05e", null, LeadDisQualify));
            //            viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat, "FontAwesomeRegular", "\uf044", null, Update));
            //        }
            //    }
            //}
            //else
            //    floatingButtonGroup.IsVisible = false;
        }

        private async void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            LeadForm leadForm = new LeadForm(viewModel.singleLead.leadid);
            leadForm.CheckSingleLead = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(leadForm);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
                }
            };
        }

        private async void LeadQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool _isID = await viewModel.CheckID(viewModel.singleLead.bsd_identitycardnumberid);
            CrmApiResponse apiResponse = await viewModel.Qualify(viewModel.singleLead.leadid);
            if (apiResponse.IsSuccess == true)
            {
                if (_isID)
                {
                    if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                    if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                    if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                    if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                    await viewModel.LoadOneLead(Id.ToString());
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong); 
                }
                else
                    ToastMessageHelper.ShortMessage(Language.so_cmnd_so_cccd_so_ho_chieu_da_duoc_su_dung);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(apiResponse.ErrorResponse.error.message);
            }
        }

        private async void LeadDisQualify(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.mat_khach_hang, Language.khong_lien_lac_duoc, Language.khong_quan_tam, Language.da_huy };
            
            string aws = await DisplayActionSheet(Language.tuy_chon_khong_chuyen_doi, Language.huy, null, options);

            if (aws == Language.mat_khach_hang)
            {
                viewModel.LeadStatusCode = 4;
            }
            else if (aws == Language.khong_lien_lac_duoc)
            {
                viewModel.LeadStatusCode = 5;
            }
            else if (aws == Language.khong_quan_tam)
            {
                viewModel.LeadStatusCode = 6;
            }
            else if (aws == Language.da_huy)
            {
                viewModel.LeadStatusCode = 7;
            }

            if (viewModel.LeadStatusCode != 0)
            {
                viewModel.LeadStateCode = 2;
                bool isSuccess = await viewModel.UpdateStatusCodeLead();
                if (isSuccess)
                {
                    if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                    if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                    await viewModel.LoadOneLead(Id.ToString());
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                }
            }
            
            LoadingHelper.Hide();
        }

        private async void ReactivateLead(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.LeadStateCode = 0;
            viewModel.LeadStatusCode = 1;
            bool isSuccess = await viewModel.UpdateStatusCodeLead();
            if (isSuccess)
            {
                if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                await viewModel.LoadOneLead(Id.ToString());
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
            LoadingHelper.Hide();
        }

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone.Replace(" ", "").Replace("+84-", "").Replace("84",""); // thêm sdt ở đây
            if (phone != string.Empty)
            {
                SmsMessage sms = new SmsMessage(null, phone);
                await Sms.ComposeAsync(sms);
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleLead.mobilephone.Replace(" ","").Replace("+84-","").Replace("84", ""); // thêm sdt ở đây
            if (phone != string.Empty)
            {
                await Launcher.OpenAsync($"tel:{phone}");
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        // Tab Thong tin
        private async Task LoadDataThongTin(string leadid)
        {
            if (leadid != null && viewModel.singleLead == null)
                await viewModel.LoadOneLead(leadid);
        }

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            //viewModel.LoadPhongThuy();
            //if (viewModel.PhongThuy.gioi_tinh != 0 && viewModel.PhongThuy.gioi_tinh != 100000000 && viewModel.PhongThuy.nam_sinh != 0)
            //    phongthuy_info.IsVisible = true;
            //else
            //    phongthuy_info.IsVisible = false;
        }

        private void ShowImage_Tapped(object sender, EventArgs e)
        {
            //LookUpImagePhongThuy.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            //if (LookUpImagePhongThuy.IsVisible)
            //{
            //    LookUpImagePhongThuy.IsVisible = false;
            //    return true;
            //}
            return base.OnBackButtonPressed();
        }

        private void Close_LookUpImagePhongThuy_Clicked(object sender, EventArgs e)
        {
            //LookUpImagePhongThuy.IsVisible = false;
        }

        #endregion
        private void GoToContact(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.contact_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ContactDetailPage newPage = new ContactDetailPage(viewModel.singleLead.contact_id);
                newPage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
                    }
                };
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
            }
        }

        private void GoToAccount(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.account_id != Guid.Empty)
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(viewModel.singleLead.account_id);
                newPage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
                    }
                };
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
            }
        }

        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleLead != null && viewModel.singleLead.leadid != Guid.Empty)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
        }

        private void CareItem_Tapped(object sender, EventArgs e)
        {
            var item = (ActivityListModel)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
        }

        private async void ShowMoreCare_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCase();
            LoadingHelper.Hide();
        }

        private async void ShowMoreMeetings_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageMeetings++;
            await viewModel.LoadMeetings();
            LoadingHelper.Hide();
        }

        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }

        private async void GenerateQRCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleLead.bsd_customercode))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_cap_nhat_ma_khach_hang_de_tao_ma_qr);
                return;
            }

            LoadingHelper.Show();
            List<string> info = new List<string>();
            info.Add(viewModel.singleLead.bsd_customercode);
            info.Add("lead");
            info.Add(viewModel.singleLead.leadid.ToString());
            string uriQrCode = $"https://api.qrserver.com/v1/create-qr-code/?size=150%C3%97150&data={string.Join(",",info)}";

            var bytearr = await DowloadImageToByteArrHelper.Download(uriQrCode);
            string base64 = System.Convert.ToBase64String(bytearr);

            bool isSuccess = await viewModel.SaveQRCode(base64);
            if (isSuccess)
            {
                viewModel.singleLead.bsd_qrcode = base64;
                ToastMessageHelper.ShortMessage(Language.tao_qr_code_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.tao_qr_code_that_bai);
            }
        }

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    TabThongTin.IsVisible = true;
                    TabCustomerCare.IsVisible = false;
                    TabMeeting.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    if (viewModel.Cares == null)
                    {
                        LoadingHelper.Show();
                        await viewModel.LoadCase();
                        LoadingHelper.Hide();
                    }
                    TabThongTin.IsVisible = false;
                    TabCustomerCare.IsVisible = true;
                    TabMeeting.IsVisible = false;
                }
                else if ((int)e.Item == 2)
                {
                    if (viewModel.Meetings.Count == 0)
                    {
                        LoadingHelper.Show();
                        await viewModel.LoadMeetings();
                        LoadingHelper.Hide();
                    }
                    
                    TabThongTin.IsVisible = false;
                    TabCustomerCare.IsVisible = false;
                    TabMeeting.IsVisible = true;
                }
            }
        }

        private async void floatingButtonGroup_ClickedEvent(object sender, EventArgs e)
        {
            var lead = await viewModel.LoadStatusLead();
            SetButtonFloatingButton(lead);
        }

        private void NewMeeting_Clicked(object sender, EventArgs e)
        {
            NewMeet(sender, e);
        }
    }
}