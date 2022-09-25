using System;
using System.Collections.Generic;
using System.IO;
using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class UserInfoPage : ContentPage
    {
        public UserInfoPageViewModel viewModel;
        public Action<bool> OnCompleted;

        public UserInfoPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            this.BindingContext = viewModel = new UserInfoPageViewModel();
            centerModelPassword.Body.BindingContext = viewModel;
            await viewModel.LoadContact();
            if (viewModel.ContactModel != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private async void ChangePassword_Tapped(object sender, EventArgs e)
        {
            viewModel.OldPassword = null;
            viewModel.NewPassword = null;
            viewModel.ConfirmNewPassword = null;
            await centerModelPassword.Show();
        }

        private void OldPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.OldPassword != null && viewModel.OldPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.OldPassword = viewModel.OldPassword.Trim();
            }
        }

        private void NewPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.NewPassword != null && viewModel.NewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.NewPassword = viewModel.NewPassword.Trim();
            }
        }

        private void ConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.ConfirmNewPassword != null && viewModel.ConfirmNewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.ConfirmNewPassword = viewModel.ConfirmNewPassword.Trim();
            }
        }

        private async void SaveChangedPassword_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.OldPassword))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mat_khau_cu);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.NewPassword))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mat_khau_moi);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.ConfirmNewPassword))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_xac_nhan_mat_khau_moi);
                return;
            }

            if (viewModel.OldPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.NewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.ConfirmNewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.NewPassword.Length < 6)
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_it_nhat_6_ky_tu);
                return;
            }

            if (UserLogged.Password != viewModel.OldPassword)
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_cu_khong_dung);
                return;
            }

            if (viewModel.NewPassword != viewModel.ConfirmNewPassword)
            {
                ToastMessageHelper.ShortMessage(Language.xac_nhan_mat_khai_khong_dung);
                return;
            }

            if (viewModel.OldPassword == viewModel.NewPassword)
            {
                ToastMessageHelper.LongMessage(Language.ban_dang_su_dung_mat_khau_cu_vui_long_nhap_lai);
                return;
            }

            LoadingHelper.Show();
            bool isSuccess = await viewModel.ChangePassword();
            if (isSuccess)
            {
                await centerModelPassword.Hide();
                UserLogged.Password = viewModel.ConfirmNewPassword;
                ToastMessageHelper.ShortMessage(Language.doi_mat_khau_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.doi_mat_khau_that_bai);
            }
        }

        private void PhoneNum_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.PhoneNumber)) return;
            string phone = string.Empty;
            phone = viewModel.PhoneNumber.Contains("-") ? viewModel.PhoneNumber.Split('-')[1] : viewModel.PhoneNumber;

            if (phone.Length != 10)
            {
                ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
            }
        }

        private async void SaveUserInfor_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.PhoneNumber))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai);
                return;
            }

            string phone = string.Empty;
            phone = viewModel.PhoneNumber.Contains("-") ? viewModel.PhoneNumber.Split('-')[1] : viewModel.PhoneNumber;

            if (phone.Length != 10)
            {
                ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
                return;
            }

            if (viewModel.ContactModel.birthdate == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_sinh);
                return;
            }

            if (DateTime.Now.Year - DateTime.Parse(viewModel.ContactModel.birthdate.ToString()).Year < 18)
            {
                ToastMessageHelper.ShortMessage(Language.nguoi_dung_phai_tu_18_tuoi);
                return;
            }

            if (viewModel.Gender == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_gioi_tinh);
                return;
            }

            LoadingHelper.Show();

            bool isSuccess = await viewModel.UpdateUserInfor();
            if (isSuccess)
            {
                if (viewModel.ContactModel.bsd_fullname != UserLogged.ContactName)
                {
                    UserLogged.ContactName = viewModel.ContactModel.bsd_fullname;
                }
                if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.cap_nhat_that_bai);
            }
        }

        private async void ChangeAvatar_Tapped(object sender, EventArgs e)
        {
            string[] options = new string[] { Language.thu_vien, Language.chup_hinh };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.thu_vien)
            {
                await CrossMedia.Current.Initialize();
                PermissionStatus photostatus = await PermissionHelper.RequestPhotosPermission();
                if (photostatus == PermissionStatus.Granted)
                {
                    try
                    {
                        LoadingHelper.Show();
                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { PhotoSize = PhotoSize.MaxWidthHeight,MaxWidthHeight=600});
                        if (file != null)
                        {
                            viewModel.AvatarArr = System.IO.File.ReadAllBytes(file.Path);
                            string imgBase64 = Convert.ToBase64String(viewModel.AvatarArr);
                            viewModel.Avatar = imgBase64;
                            if (viewModel.Avatar != UserLogged.Avartar)
                            {
                                bool isSuccess = await viewModel.ChangeAvatar();
                                if (isSuccess)
                                {
                                    UserLogged.Avartar = viewModel.Avatar;
                                    if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                                    ToastMessageHelper.ShortMessage(Language.doi_hinh_dai_dien_thanh_cong);
                                    LoadingHelper.Hide();
                                }
                                else
                                {
                                    LoadingHelper.Hide();
                                    ToastMessageHelper.ShortMessage(Language.doi_hinh_dai_dien_that_bai);
                                }
                            }
                            LoadingHelper.Hide();
                        }
                    }
                    catch(Exception ex)
                    {
                        ToastMessageHelper.LongMessage(ex.Message);
                        LoadingHelper.Hide();
                    }
                }
                LoadingHelper.Hide();
            }
            else if (asw == Language.chup_hinh)
            {
                await CrossMedia.Current.Initialize();
                PermissionStatus camerastatus = await PermissionHelper.RequestCameraPermission();
                if (camerastatus == PermissionStatus.Granted)
                {
                    LoadingHelper.Show();
                    string fileName = $"{Guid.NewGuid()}.jpg";
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = fileName,
                        SaveToAlbum = false,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 600
                    });
                    if (file != null)
                    {
                        viewModel.AvatarArr = System.IO.File.ReadAllBytes(file.Path);
                        viewModel.Avatar = Convert.ToBase64String(viewModel.AvatarArr);
                        if (viewModel.Avatar != UserLogged.Avartar)
                        {
                            bool isSuccess = await viewModel.ChangeAvatar();
                            if (isSuccess)
                            {
                                UserLogged.Avartar = viewModel.Avatar;
                                if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                                ToastMessageHelper.ShortMessage(Language.doi_hinh_dai_dien_thanh_cong);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                LoadingHelper.Hide();
                                ToastMessageHelper.ShortMessage(Language.doi_hinh_dai_dien_that_bai);
                            }
                        }
                    }
                }
                LoadingHelper.Hide();
            }
        }
    }
}
