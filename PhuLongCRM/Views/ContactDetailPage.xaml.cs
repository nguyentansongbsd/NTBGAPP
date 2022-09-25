using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.ViewModels;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh = null;
        public static bool? NeedToRefreshQueues = null;
        public static bool? NeedToRefreshActivity = null;
        public static OptionSet FromCustomer = null;
        private ContactDetailPageViewModel viewModel;
        private Guid Id;
        public ContactDetailPage(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactDetailPageViewModel();
            NeedToRefresh = false;
            NeedToRefreshActivity = false;
            Id = contactId;
            Init();
        }
        public async void Init()
        {
            await LoadDataThongTin(Id.ToString());
            
            if (viewModel.singleContact.contactid != Guid.Empty)
            {
                SetButtonFloatingButton();
                FromCustomer = new OptionSet { Val= viewModel.singleContact.contactid.ToString(), Label= viewModel.singleContact.bsd_fullname, Title= viewModel.CodeContac };
                OnCompleted(true);
            }
            else
                OnCompleted(false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                viewModel.singleContact = new ContactFormModel();
                await LoadDataThongTin(this.Id.ToString());
                viewModel.PhongThuy = null;
                LoadDataPhongThuy();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshQueues == true)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.list_danhsachdatcho.Clear();
                await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshActivity == true && viewModel.Cares != null)
            {
                LoadingHelper.Show();
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
            if (viewModel.singleContact.contactid != Guid.Empty)
            {
                if(viewModel.ButtonCommandList.Count > 0)
                    viewModel.ButtonCommandList.Clear();

                if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_qrcode))
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_qr_code, "FontAwesomeSolid", "\uf029", null, GenerateQRCode));
                }

                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));

                if (viewModel.singleContact.statuscode != "100000000")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat, "FontAwesomeRegular", "\uf044", null, EditContact));

                if (viewModel.singleContact.statuscode == "2")
                    floatingButtonGroup.IsVisible = false;
                else
                    floatingButtonGroup.IsVisible = true;
            }
        }

        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }

        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.singleContact != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
        }

        private void EditContact(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ContactForm newPage = new ContactForm(Id);
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

        // tab thong tin
        private async Task LoadDataThongTin(string Id)
        {
            if (Id != null && viewModel.singleContact.contactid == Guid.Empty)
            {
                await viewModel.loadOneContact(Id);
                if (viewModel.singleContact.gendercode != null)
                { 
                   viewModel.singleGender = ContactGender.GetGenderById(viewModel.singleContact.gendercode); 
                }
                if (viewModel.singleContact.bsd_localization != null)
                {
                    viewModel.SingleLocalization = AccountLocalization.GetLocalizationById(viewModel.singleContact.bsd_localization);
                }
                else
                {
                    viewModel.SingleLocalization = null;
                }
            }
        }

        private void CMNDFront_Tapped(object sender, EventArgs e)
        {
            if(viewModel.singleContact.bsd_mattruoccmnd_source != null)
            {
                new PhotoBrowser()
                {
                    Photos = viewModel.Photos,
                    StartIndex = 0,
                    EnableGrid = true
                }.Show();
            }
        }

        private void CMNDBehind_Tapped(object sender, EventArgs e)
        {
            if (viewModel.singleContact.bsd_matsaucmnd_source != null && viewModel.singleContact.bsd_mattruoccmnd_source != null)
            {
                new PhotoBrowser()
                {
                    Photos = viewModel.Photos,
                    StartIndex = 1,
                    EnableGrid = true
                }.Show();
            }
            else
            {
                if (viewModel.singleContact.bsd_matsaucmnd_source != null && viewModel.singleContact.bsd_mattruoccmnd_source == null)
                {
                    new PhotoBrowser()
                    {
                        Photos = viewModel.Photos,
                        StartIndex = 0,
                        EnableGrid = true
                    }.Show();
                }
            }
                
        }

        #region Tab giao dich
        private async Task LoadDataGiaoDich(string Id)
        {
            if (viewModel.list_danhsachdatcho == null || viewModel.list_danhsachdatcoc == null || viewModel.list_danhsachhopdong == null || viewModel.Cares == null)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.PageDanhSachDatCoc = 1;
                viewModel.PageDanhSachHopDong = 1;
                viewModel.PageCase = 1;

                viewModel.list_danhsachdatcho = new ObservableCollection<QueueFormModel>();
                viewModel.list_danhsachdatcoc = new ObservableCollection<ReservationListModel>();
                viewModel.list_danhsachhopdong = new ObservableCollection<ContractModel>();
                viewModel.Cares = new ObservableCollection<ActivityListModel>();

                await Task.WhenAll(
                   viewModel.LoadQueuesForContactForm(Id),
                   viewModel.LoadReservationForContactForm(Id),
                   viewModel.LoadOptoinEntryForContactForm(Id),
                   viewModel.LoadCase()
               );
                LoadingHelper.Hide();
            }          
        }
        // danh sach dat cho
        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach dat coc
        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadReservationForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        // danh sach hop dong
        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachHopDong++;
            await viewModel.LoadOptoinEntryForContactForm(viewModel.singleContact.contactid.ToString());
            LoadingHelper.Hide();
        }

        //Cham soc khach hang
        private async void ShowMoreChamSocKhachHang_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCase();
            LoadingHelper.Hide();
        }

        private void ChiTietDatCoc_Tapped(object sender, EventArgs e)
        {
            var item = (ReservationListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item == null) return;

            LoadingHelper.Show();
            bool isReservation = false;
            if (item.statuscode != 100000007)
                isReservation = true;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(item.quoteid, isReservation);
            bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(bangTinhGiaDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
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

        #region TabPhongThuy
        private void LoadDataPhongThuy()
        {
            if(viewModel.PhongThuy == null)
            {
               viewModel.PhongThuy = new PhongThuyModel();
               viewModel.LoadPhongThuy();
                if (viewModel.PhongThuy.gioi_tinh != 0 && viewModel.PhongThuy.gioi_tinh != 100000000 && viewModel.PhongThuy.nam_sinh != 0)//100000000
                    phongthuy_info.IsVisible = true;
                else
                    phongthuy_info.IsVisible = false;
            }
        }

        private void ShowImage_Tapped(object sender, EventArgs e)
        {
            LookUpImage.IsVisible = true;
            ImageDetail.Source = viewModel.PhongThuy.image;
        }

        protected override bool OnBackButtonPressed()
        {
            if (LookUpImage.IsVisible)
            {
                LookUpImage.IsVisible = false;
                return true;
            }
            FromCustomer = null;
            return base.OnBackButtonPressed();
        }

        private void Close_LookUpImage_Tapped(object sender, EventArgs e)
        {
            LookUpImage.IsVisible = false;
        }

        #endregion

        private async void NhanTin_Tapped(object sender, EventArgs e)
        {           
            string phone = viewModel.singleContact.mobilephone.Replace(" ", "").Replace("+84-", "").Replace("84", "");
            if (phone != string.Empty)
            {
                SmsMessage sms = new SmsMessage(null, phone);
                await Sms.ComposeAsync(sms);
                //var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                //if (checkVadate == true)
                //{
                //    SmsMessage sms = new SmsMessage(null, phone);
                //    await Sms.ComposeAsync(sms);
                //    LoadingHelper.Hide();
                //}
                //else
                //{
                //    LoadingHelper.Hide();
                //    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_sai_dinh_dang_vui_long_kiem_tra_lai);
                //}
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        private async void GoiDien_Tapped(object sender, EventArgs e)
        {          
            string phone = viewModel.singleContact.mobilephone.Replace(" ", "").Replace("+84-", "").Replace("84", "");
            if (phone != string.Empty)
            {
                await Launcher.OpenAsync($"tel:{phone}");
                //var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
                //if (checkVadate == true)
                //{
                //    await Launcher.OpenAsync($"tel:{phone}");
                //    LoadingHelper.Hide();
                //}
                //else
                //{
                //    LoadingHelper.Hide();
                //    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_sai_dinh_dang_vui_long_kiem_tra_lai);
                //}
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_co_so_dien_thoai_vui_long_kiem_tra_lai);
            }
        }

        private void ThongTinCongTy_Tapped(object sender, EventArgs e)
        {            
            if (!string.IsNullOrEmpty(viewModel.singleContact._parentcustomerid_value))
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(Guid.Parse(viewModel.singleContact._parentcustomerid_value));
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

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            queuesDetialPage.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queuesDetialPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
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
                    TabPhongThuy.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    await LoadDataGiaoDich(Id.ToString());
                    TabThongTin.IsVisible = false;
                    TabGiaoDich.IsVisible = true;
                    TabPhongThuy.IsVisible = false;
                }
                else if ((int)e.Item == 2)
                {
                    LoadDataPhongThuy();
                    TabThongTin.IsVisible = false;
                    TabGiaoDich.IsVisible = false;
                    TabPhongThuy.IsVisible = true;
                }
            }
        }

        private async void GenerateQRCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_customercode))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_cap_nhat_ma_khach_hang_de_tao_ma_qr);
                return;
            }

            LoadingHelper.Show();
            List<string> info = new List<string>();
            info.Add(viewModel.singleContact.bsd_customercode);
            info.Add("contact");
            info.Add(viewModel.singleContact.contactid.ToString());
            string uriQrCode = $"https://api.qrserver.com/v1/create-qr-code/?size=150%C3%97150&data={string.Join(",", info)}";

            var bytearr = await DowloadImageToByteArrHelper.Download(uriQrCode);
            string base64 = System.Convert.ToBase64String(bytearr);

            bool isSuccess = await viewModel.SaveQRCode(base64);
            if (isSuccess)
            {
                viewModel.singleContact.bsd_qrcode = base64;
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
    }
}