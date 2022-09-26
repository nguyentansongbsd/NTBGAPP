using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using PhuLongCRM.ViewModels;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using System.Linq;
using PhuLongCRM.Resources;
using PhuLongCRM.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Media;
using System.Net.Http.Headers;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactForm : ContentPage
    {
        public Action<bool> OnCompleted;      
        private ContactFormViewModel viewModel;
        private Guid Id;

        public ContactForm()
        {
            InitializeComponent();
            this.Id = Guid.Empty;
            Init();
            Create();
        }

        public ContactForm(Guid contactId)
        {
            InitializeComponent();
            this.Id = contactId;
            Init();
            Update();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new ContactFormViewModel();
            viewModel.ContactType = viewModel.ContactTypes.SingleOrDefault(x => x.Val == "100000000");
            SetPreOpen();
        }

        private void Create()
        {
            this.Title = Language.tao_moi_khach_hang;
            btn_save_contact.Text = Language.tao_moi_khach_hang_ca_nhan;
            btn_save_contact.Clicked += CreateContact_Clicked;
            viewModel.CustomerStatusReason = CustomerStatusReasonData.GetCustomerStatusReasonById("1");
            lookUpTinhTrang.IsEnabled = false;
        }

        private void CreateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(null);
        }

        private async void Update()
        {
            await loadData(this.Id.ToString());
            this.Title = Language.cap_nhat_khach_hang;
            btn_save_contact.Text = Language.cap_nhat_khach_hang_ca_nhan;
            btn_save_contact.Clicked += UpdateContact_Clicked;
            lookUpTinhTrang.IsEnabled = false;

            if (viewModel.singleContact.contactid != Guid.Empty)
            {
                datePickerNgayCap.ReSetTime();
                datePickerNgayCapHoChieu.ReSetTime();
                customerCode.IsVisible = true;
                viewModel.CustomerStatusReason = CustomerStatusReasonData.GetCustomerStatusReasonById(viewModel.singleContact.statuscode);
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        private void UpdateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(this.Id.ToString());
        }

        public async Task loadData(string contactId)
        {
            LoadingHelper.Show();

            if (contactId != null)
            {
                await viewModel.LoadOneContact(contactId);
                if (viewModel.singleContact.gendercode != null)
                {
                    viewModel.singleGender = ContactGender.GetGenderById(viewModel.singleContact.gendercode);
                }
                if (viewModel.singleContact.bsd_localization != null)
                {
                    viewModel.singleLocalization = AccountLocalization.GetLocalizationById(viewModel.singleContact.bsd_localization);
                }
                if (viewModel.singleContact._parentcustomerid_value != null)
                {
                    viewModel.Account = new PhuLongCRM.Models.LookUp
                    {
                        Name = viewModel.singleContact.parentcustomerid_label,
                        Id = Guid.Parse(viewModel.singleContact._parentcustomerid_value)
                    };
                }
            }
            LoadingHelper.Hide();
        }

        private async void SaveData(string id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_fullname))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ho_ten);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleContact.mobilephone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai);               
                return;
            }

            string phone = string.Empty;
            phone = viewModel.singleContact.mobilephone.Contains("-") ? viewModel.singleContact.mobilephone.Split('-')[1] : viewModel.singleContact.mobilephone;

            if (phone.Length != 10)
            {
                ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
                return;
            }

            if (viewModel.singleGender == null || viewModel.singleGender.Val == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_gioi_tinh);
                return;
            }
            if (viewModel.singleLocalization == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_quoc_tich);
                return;
            }
            if (viewModel.singleContact.birthdate == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_sinh);
                return;
            }
            if (DateTime.Now.Year - DateTime.Parse(viewModel.singleContact.birthdate.ToString()).Year < 18)
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_tu_18_tuoi);
                return;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.emailaddress1))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(viewModel.singleContact.emailaddress1);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage(Language.email_sai_dinh_dang);
                    return;
                }

                if (!await viewModel.CheckEmail(viewModel.singleContact.emailaddress1, id))
                {
                    ToastMessageHelper.ShortMessage(Language.email_da_duoc_su_dung);
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.telephone1) && viewModel.singleContact.telephone1 != "+84")
            {
                string telephone = string.Empty;
                telephone = viewModel.singleContact.telephone1.Contains("-") ? viewModel.singleContact.telephone1.Split('-')[1] : viewModel.singleContact.telephone1;

                if (telephone.Length != 10)
                {
                    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(viewModel.Address1?.lineaddress))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_lien_lac);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Address2?.lineaddress))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_thuong_tru);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycard))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_cccd);
                return;
            }
            if (!await viewModel.CheckCMND(viewModel.singleContact.bsd_identitycardnumber, id))
            {
                ToastMessageHelper.ShortMessage(Language.so_cmnd_da_duoc_su_dung);
                viewModel.checkCMND = viewModel.singleContact.bsd_identitycardnumber;
                return;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycardnumber) && !await viewModel.CheckPassport(viewModel.singleContact.bsd_passport, id))
            {
                ToastMessageHelper.ShortMessage(Language.so_ho_chieu_da_duoc_su_sung);
                return;
            }

            if (!StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_identitycard, 12))
            {
                ToastMessageHelper.ShortMessage(Language.so_cccd_khong_hop_le_gioi_han_12_ky_tu);
                return;
            }

            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycardnumber) && !StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_identitycardnumber, 9))
            {
                ToastMessageHelper.ShortMessage(Language.so_cmnd_khong_hop_le_gioi_han_9_ky_tu);
                return;
            }

            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_passport) && !StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_passport, 8))
            {
                ToastMessageHelper.ShortMessage(Language.so_ho_chieu_khong_hop_le_gioi_han_8_ky_tu);
                return;
            }


            viewModel.singleContact.bsd_localization = viewModel.singleLocalization != null && viewModel.singleLocalization.Val != null ? viewModel.singleLocalization.Val : null;
            viewModel.singleContact.gendercode = viewModel.singleGender != null && viewModel.singleGender.Val != null ? viewModel.singleGender.Val : null;
            viewModel.singleContact._parentcustomerid_value = viewModel.Account != null && viewModel.Account.Id != null ? viewModel.Account.Id.ToString() : null;

            if (id == null)
            {
                LoadingHelper.Show();               
                var created = await viewModel.createContact(viewModel.singleContact);

                if (created != new Guid())
                {
                    if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    //if (QueueForm.NeedToRefresh.HasValue) QueueForm.NeedToRefresh = true;
                    await viewModel.PostCMND();
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.tao_khach_hang_ca_nhan_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.tao_khach_hang_ca_nhan_that_bai);
                }
            }
            else
            {
                LoadingHelper.Show();               
                var updated = await viewModel.updateContact(viewModel.singleContact);

                if (updated)
                {
                    LoadingHelper.Hide();
                    if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                    if (ContactDetailPage.NeedToRefresh.HasValue) ContactDetailPage.NeedToRefresh = true;
                    if (ContactDetailPage.NeedToRefreshActivity.HasValue) ContactDetailPage.NeedToRefreshActivity = true;
                    await viewModel.PostCMND();
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_that_bai);
                }
            }
        }

        public void SetPreOpen()
        {
            lookUpTinhTrang.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.CustomerStatusReasons = CustomerStatusReasonData.CustomerStatusReasons();
                LoadingHelper.Hide();
            };

            Lookup_GenderOptions.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                foreach (var item in ContactGender.GenderData())
                {
                    viewModel.GenderOptions.Add(item);
                }
                LoadingHelper.Hide();
            };
            Lookup_LocalizationOptions.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                AccountLocalization.Localizations();
                foreach (var item in AccountLocalization.LocalizationOptions)
                {
                    viewModel.LocalizationOptions.Add(item);
                }
                LoadingHelper.Hide();                
            }; 
           Lookup_Account.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAccountsLookup();
                LoadingHelper.Hide();
            };
        }

        private void Handle_LayoutChanged(object sender, System.EventArgs e)
        {
            var width = ((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 35) / 2;
            var tmpHeight = width * 2 / 3;
            MatTruocCMND.HeightRequest = tmpHeight;
            MatSauCMND.HeightRequest = tmpHeight;
        }

        public void MatTruocCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_mattruoccmnd_source != null)
            {
                menuItem.Add(new OptionSet { Label = Language.xem_anh_mat_truoc_cmnd, Val = "Front" ,Title="Show"});
            }
            menuItem.Add(new OptionSet { Label = Language.chup_anh, Val = "Front", Title = "Take" });
            menuItem.Add(new OptionSet { Label = Language.chon_anh_ty_thu_vien, Val = "Front", Title = "Select" });
            this.showMenuImageCMND(menuItem);
        }

        private void MatSauCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_matsaucmnd_source != null)
            {
                menuItem.Add(new OptionSet { Label = Language.xem_anh_mat_sau_cmnd, Val = "Behind", Title = "Show" });
            }
            menuItem.Add(new OptionSet { Label = Language.chup_anh, Val = "Behind", Title = "Take" });
            menuItem.Add(new OptionSet { Label = Language.chon_anh_ty_thu_vien, Val = "Behind", Title = "Select" });
            this.showMenuImageCMND(menuItem);
        }

        private void showMenuImageCMND(List<OptionSet> listItem)
        {
            popup_menu_imageCMND.ItemSource = listItem;

            popup_menu_imageCMND.focus();
        }

        async void MenuItem_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as OptionSet;
            popup_menu_imageCMND.unFocus();

            Stream resultStream;
            byte[] arrByte;
            string base64String;

            switch (item.Title)
            {
                case "Take":

                    PermissionStatus cameraStatus = await PermissionHelper.RequestCameraPermission();
                    if (cameraStatus == PermissionStatus.Granted)
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            SaveToAlbum = false,
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 600,
                        });

                        if (file == null)
                            return;

                        resultStream = file.GetStream();
                        using (var memoryStream = new MemoryStream())
                        {
                            resultStream.CopyTo(memoryStream);
                            arrByte = memoryStream.ToArray();
                        }
                        base64String = Convert.ToBase64String(arrByte);
                        if (item.Val == "Front") { viewModel.singleContact.bsd_mattruoccmnd_source = base64String; viewModel.singleContact.bsd_mattruoccmnd_base64 = base64String; }
                        else if (item.Val == "Behind") { viewModel.singleContact.bsd_matsaucmnd_source = base64String; viewModel.singleContact.bsd_matsaucmnd_base64 = base64String; }
                    }

                    break;
                case "Select":

                    PermissionStatus storageStatus = await PermissionHelper.RequestPhotosPermission();
                    if (storageStatus == PermissionStatus.Granted)
                    {
                        var file2 = await MediaPicker.PickPhotoAsync();
                        if (file2 == null)
                            return;

                        Stream result = await file2.OpenReadAsync();
                        if (result != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                result.CopyTo(memoryStream);
                                arrByte = memoryStream.ToArray();
                            }
                            base64String = Convert.ToBase64String(arrByte);
                            if (item.Val == "Front") { viewModel.singleContact.bsd_mattruoccmnd_source  = base64String; viewModel.singleContact.bsd_mattruoccmnd_base64 = base64String; }
                            else if (item.Val == "Behind") { viewModel.singleContact.bsd_matsaucmnd_source = base64String; viewModel.singleContact.bsd_matsaucmnd_base64 = base64String; }
                        }
                    }
                    break;
                default:
                    if (item.Val == "Front") { image_detailCMNDImage.Source = viewModel.singleContact.bsd_mattruoccmnd_source; }
                    else if (item.Val == "Behind") { image_detailCMNDImage.Source = viewModel.singleContact.bsd_matsaucmnd_source; }
                    this.showDetailCMNDImage();
                    break;
            }
        }

        private void showDetailCMNDImage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            popup_detailCMNDImage.IsVisible = true;
        }

        void BtnCloseModalImage_Clicked(object sender, System.EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            popup_detailCMNDImage.IsVisible = false;
        }

        private void CMND_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycardnumber)) return;
            if (!StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_identitycardnumber, 9))
            {
                ToastMessageHelper.ShortMessage(Language.so_cmnd_khong_hop_le_gioi_han_9_ky_tu);
            }
        }
        private void Phone_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            var num = sender as PhoneEntryControl;
            if (!string.IsNullOrWhiteSpace(num.Text))
            {
                string phone = num.Text;
                phone = phone.Contains("-") ? phone.Split('-')[1] : phone;

                if (phone.Length != 10)
                {
                    ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
                }
            }
        }
        private void CCCD_Unfocused(object sender, FocusEventArgs e)
        {
            if (!StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_identitycard, 12))
            {
                ToastMessageHelper.ShortMessage(Language.so_cccd_khong_hop_le_gioi_han_12_ky_tu);
            }
        }
        private void PassPort_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_passport)) return;
            if (!StringFormatHelper.CheckValueID(viewModel.singleContact.bsd_passport, 8))
            {
                ToastMessageHelper.ShortMessage(Language.so_ho_chieu_khong_hop_le_gioi_han_8_ky_tu);
            }
        }
    }
}