using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid AccountId;
        public static bool? NeedToRefreshAccount = null;
        public static bool? NeedToRefreshMandatory = null;
        public static bool? NeedToRefreshQueues = null;
        public static bool? NeedToRefreshActivity = null;
        public static OptionSet FromCustomer = null;
        private AccountDetailPageViewModel viewModel;

        public AccountDetailPage(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            this.BindingContext = viewModel = new AccountDetailPageViewModel();
            NeedToRefreshAccount = false;
            NeedToRefreshMandatory = false;
            NeedToRefreshActivity = false;
            LoadingHelper.Show();
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(AccountId.ToString());

            if (viewModel.singleAccount.accountid != Guid.Empty)
            {
                SetButtonFloatingButton();
                FromCustomer = new OptionSet { Val = viewModel.singleAccount.accountid.ToString(), Label = viewModel.singleAccount.bsd_name, Title = viewModel.CodeAccount };
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshAccount == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadOneAccount(AccountId.ToString());
                viewModel.singleAccount.bsd_address = await SetAddress();
                if (viewModel.singleAccount.bsd_businesstype != null)
                {
                    viewModel.GetTypeById(viewModel.singleAccount.bsd_businesstype);
                }
                if (viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }
                NeedToRefreshAccount = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshMandatory == true)
            {
                LoadingHelper.Show();
                //viewModel.PageMandatory = 1;
                //viewModel.list_MandatorySecondary.Clear();
                //await LoadDataNguoiUyQuyen(AccountId.ToString());
                NeedToRefreshMandatory = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshQueues == true)
            {
                LoadingHelper.Show();
                viewModel.PageQueueing = 1;
                viewModel.list_thongtinqueing.Clear();
                await viewModel.LoadDSQueueingAccount(AccountId);
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshActivity == true)
            {
                LoadingHelper.Show();
                viewModel.PageMeeting = 1;
                viewModel.Meetings.Clear();
                await viewModel.LoadMeeting();
                viewModel.PageCase = 1;
                viewModel.Cares.Clear();
                await viewModel.LoadCase();
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
        }

        private void SetButtonFloatingButton()
        {
            if (viewModel.singleAccount.accountid != Guid.Empty)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));

                //if (viewModel.ButtonCommandList.Count > 0)
                //    viewModel.ButtonCommandList.Clear();

                //if (string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_imageqrcode))
                //{
                //    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_qr_code, "FontAwesomeSolid", "\uf029", null, GenerateQRCode));
                //}
                ////viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
                //viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                //viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));

                //if (viewModel.singleAccount.statuscode != "100000000")
                //    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, Update));

                //if (viewModel.singleAccount.statuscode == "2")
                //    floatingButtonGroup.IsVisible = false;
                //else
                //    floatingButtonGroup.IsVisible = true;
            }
        }

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleAccount == null)
            {
                await viewModel.LoadOneAccount(Id);

                viewModel.singleAccount.bsd_address = await SetAddress();

                if (viewModel.singleAccount.bsd_businesstype != null)
                {
                    viewModel.GetTypeById(viewModel.singleAccount.bsd_businesstype);
                }
                if (viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }
            }
        }

        private async Task<string> SetAddress()
        {
            List<string> listaddress = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_housenumberstreet))
            {
                listaddress.Add(viewModel.singleAccount.bsd_housenumberstreet);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.district_name))
            {
                listaddress.Add(viewModel.singleAccount.district_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.province_name))
            {
                listaddress.Add(viewModel.singleAccount.province_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_postalcode))
            {
                listaddress.Add(viewModel.singleAccount.bsd_postalcode);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.country_name))
            {
                listaddress.Add(viewModel.singleAccount.country_name);
            }

            string address = string.Join(", ", listaddress);

            return address;
        }

        private async void Website_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Xamarin.Essentials.Browser.OpenAsync(viewModel.singleAccount.websiteurl);
            LoadingHelper.Hide();
        }

        #region tab giao dich
        // tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (Id != null)
            {
                LoadingHelper.Show();
                if (viewModel.list_thongtinqueing.Count == 0 && viewModel.list_thongtinquotation.Count == 0 && viewModel.list_thongtincontract.Count == 0 && viewModel.Cares.Count == 0)
                {
                    await Task.WhenAll(
                        viewModel.LoadDSQueueingAccount(AccountId),
                        viewModel.LoadDSQuotationAccount(AccountId),
                        viewModel.LoadDSContractAccount(AccountId),
                        viewModel.LoadCase()
                        ); 
                }
                LoadingHelper.Hide();
            }
        }

        private async void ShowMoreQueueing_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageQueueing++;
            await viewModel.LoadDSQueueingAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreQuotation_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageQuotation++;
            await viewModel.LoadDSQuotationAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreContract_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageContract++;
            await viewModel.LoadDSContractAccount(AccountId);
            LoadingHelper.Hide();
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCase();
            LoadingHelper.Hide();
        }

        private void ChiTietDatCoc_Tapped(object sender, EventArgs e)
        {
            //var item = (ReservationListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            //if (item == null) return;

            //LoadingHelper.Show();
            //bool isReservation = false;
            //if (item.statuscode != 100000007)
            //    isReservation = true;
            //BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(item.quoteid, isReservation);
            //bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            //{
            //    if (isSuccess)
            //    {
            //        await Navigation.PushAsync(bangTinhGiaDetail);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            //    }
            //};
        }

        private void ItemHopDong_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(itemId);
            contractDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contractDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void CaseItem_Tapped(object sender, EventArgs e)
        {
            var item = (ActivityListModel)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
        }

        #endregion

        #region tab nguoi uy quyyen 

        private async Task LoadDataNguoiUyQuyen(string Id)
        {
            if (Id != null && viewModel.list_MandatorySecondary.Count <= 0)
            {
                await viewModel.Load_List_Mandatory_Secondary(Id);
            }
        }

        private async void DeleteMandatory_Clicked(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var a = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            MandatorySecondaryModel item = a.CommandParameter as MandatorySecondaryModel;
            var conform = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_xoa_nguoi_uye_quyen_khong, Language.dong_y, Language.huy);
            if (conform == false) return;
            LoadingHelper.Show();
            var IsSuccess = await viewModel.DeleteMandatory_Secondary(item);
            if (IsSuccess)
            {
                viewModel.list_MandatorySecondary.Remove(item);
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.da_xoa_nguoi_uy_quyen_duoc_chon);
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.xao_nguoi_uy_quyen_that_bai);
            }
        }

        private async void ShowMoreMandatory_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageMandatory++;
            await viewModel.Load_List_Mandatory_Secondary(AccountId.ToString());
            LoadingHelper.Hide();
        }

        #endregion
        private async void NhanTin_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleAccount.telephone1.Replace(" ", "").Replace("+84-", "").Replace("84", "");
            if (phone != string.Empty)
            {
                var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                if (checkVadate == true)
                {
                    SmsMessage sms = new SmsMessage(null, phone);
                    await Sms.ComposeAsync(sms);
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_sai_dinh_dang_vui_long_kiem_tra_lai);
                }
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {
            string phone = viewModel.singleAccount.telephone1.Replace(" ", "").Replace("+84-", "").Replace("84", "");
            if (phone != string.Empty)
            {
                await Launcher.OpenAsync($"tel:{phone}");
                // khong can check validate
                //var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                //if (checkVadate == true)
                //{
                //    await Launcher.OpenAsync($"tel:{phone}");
                //}
                //else
                //{
                //    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_sai_dinh_dang_vui_long_kiem_tra_lai);
                //}
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        private void NguoiDaiDien_Tapped(object sender, EventArgs e)
        {
            if (viewModel.PrimaryContact.contactid != null)
            {
                LoadingHelper.Show();
                ContactDetailPage newPage = new ContactDetailPage(viewModel.PrimaryContact.contactid);
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
        }

        private async void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            AccountForm newPage = new AccountForm(viewModel.singleAccount.accountid);
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

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            //QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            //queuesDetialPage.OnCompleted = async (IsSuccess) =>
            //{
            //    if (IsSuccess)
            //    {
            //        await Navigation.PushAsync(queuesDetialPage);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            //    }
            //};
        }

        private void CloseContentMandatorySecondary_Tapped(object sender, EventArgs e)
        {
            //ContentMandatorySecondary.IsVisible = false;
        }

        private void ListMandatorySecondary_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Grid grid = (Grid)sender;
            var item = (TapGestureRecognizer)grid.GestureRecognizers[0];
            viewModel.MandatorySecondary = item.CommandParameter as MandatorySecondaryModel;
            //ContentMandatorySecondary.IsVisible = true;
            LoadingHelper.Hide();
        }

        private async void ListMoreMandatory_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e.Item != null)
            {
                var item = e.Item as MandatorySecondaryModel;
                if (viewModel.list_MandatorySecondary.IndexOf(item) == viewModel.list_MandatorySecondary.Count() - 1)
                {
                    viewModel.isLoadMore = true;
                    viewModel.PageMandatory++;
                    await viewModel.Load_List_Mandatory_Secondary(this.AccountId.ToString());
                    viewModel.isLoadMore = false;
                    SetHeightListView();
                }
            }
        }

        private void SetHeightListView()
        {
            double height_item = (viewModel.list_MandatorySecondary.Count() * 110) + 50;
            double height_mb = ((DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 2 / 3) + 50;
            //if (height_item > height_mb)
            //{
            //    ListMandatory.HeightRequest = height_mb;
            //}
            //else
            //{
            //    ListMandatory.HeightRequest = height_item;
            //}
            //if (viewModel.list_MandatorySecondary.Count() == 0)
            //{
            //    lb_ListMandatory.IsVisible = true;
            //    ListMandatory.IsVisible = false;
            //}
            //else
            //{
            //    lb_ListMandatory.IsVisible = false;
            //    ListMandatory.IsVisible = true;
            //}
        }

        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }
        
        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleAccount != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
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
                    TabThongTin.IsVisible = true;
                    TabGiaoDich.IsVisible = false;
                    TabMeeting.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    await LoadDataGiaoDich(AccountId.ToString());
                    TabThongTin.IsVisible = false;
                    TabGiaoDich.IsVisible = true;
                    TabMeeting.IsVisible = false;
                }
                else if ((int)e.Item == 2)
                {
                    //await LoadDataNguoiUyQuyen(AccountId.ToString());
                    //SetHeightListView();
                    if (viewModel.Meetings.Count <= 0)
                    {
                        LoadingHelper.Show();
                        await viewModel.LoadMeeting();
                        LoadingHelper.Hide();
                    }
                    TabThongTin.IsVisible = false;
                    TabGiaoDich.IsVisible = false;
                    TabMeeting.IsVisible = true;
                }
            }
        }

        private async void GenerateQRCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_customercode))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_cap_nhat_ma_khach_hang_de_tao_ma_qr);
                return;
            }

            LoadingHelper.Show();
            List<string> info = new List<string>();
            info.Add(viewModel.singleAccount.bsd_customercode);
            info.Add("account");
            info.Add(viewModel.singleAccount.accountid.ToString());
            string uriQrCode = $"https://api.qrserver.com/v1/create-qr-code/?size=150%C3%97150&data={string.Join(",", info)}";

            var bytearr = await DowloadImageToByteArrHelper.Download(uriQrCode);
            string base64 = System.Convert.ToBase64String(bytearr);

            bool isSuccess = await viewModel.SaveQRCode(base64);
            if (isSuccess)
            {
                viewModel.singleAccount.bsd_imageqrcode = base64;
                ToastMessageHelper.ShortMessage(Language.tao_qr_code_thanh_cong);
                SetButtonFloatingButton();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.tao_qr_code_that_bai);
            }
        }

        private async void ShowMoreMeeting_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageMeeting++;
            await viewModel.LoadMeeting();
            LoadingHelper.Hide();
        }

        private void NewMeeting_Clicked(object sender, EventArgs e)
        {
            NewMeet(sender, e);
        }
    }
}