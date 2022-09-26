using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountForm : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid AccountId;
        private AccountFormViewModel viewModel;

        public AccountForm()
        {
            InitializeComponent();
            AccountId = Guid.Empty;
            Init();
            Create();
        }

        public AccountForm(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            Init();
            Update();
        }

        public void Init()
        {
            this.BindingContext = viewModel = new AccountFormViewModel();
            //Lookup_BusinessType.BindingContext = viewModel;
            SetPreOpen();
        }

        private void Create()
        {
            this.Title = Language.tao_moi_khach_hang;
            btnSave.Text = Language.tao_moi_khach_hang_doanh_nghiep;
            btnSave.Clicked += CreateContact_Clicked;
            viewModel.BusinessType = viewModel.BusinessTypeOptionList.SingleOrDefault(x => x.Val == "100000000");
            viewModel.CustomerStatusReason = CustomerStatusReasonData.GetCustomerStatusReasonById("1");//mac dinh la KH tiem nang
        }

        private void CreateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(null);
        }

        private async void Update()
        {
            viewModel.singleAccount = new AccountFormModel();
            this.Title = Language.cap_nhat_khach_hang;
            btnSave.Text = Language.cap_nhat_khach_hang_doanh_nghiep;
            btnSave.Clicked += UpdateContact_Clicked;

            await viewModel.LoadOneAccount(this.AccountId);

            if (viewModel.singleAccount.accountid != Guid.Empty)
            {
                datePickerNgayCap.ReSetTime();
                customerCode.IsVisible = true;
                viewModel.CustomerStatusReason = CustomerStatusReasonData.GetCustomerStatusReasonById(viewModel.singleAccount.statuscode);
                viewModel.OperationScope = OperationScopeData.GetOperationScopeById(viewModel.singleAccount.bsd_operationscope);
                viewModel.BusinessType = viewModel.BusinessTypeOptionList.SingleOrDefault(x => x.Val == "100000000");

                if (viewModel.singleAccount != null && viewModel.singleAccount.bsd_localization != null)
                {
                    viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
                }

                if (viewModel.singleAccount != null && viewModel.singleAccount.primarycontactname != null)
                {
                    viewModel.GetPrimaryContactByID();
                }

                viewModel.singleAccount.bsd_address = await SetAddress();

                OnCompleted?.Invoke(true);
            }
                
            else
                OnCompleted?.Invoke(false);
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

        private void UpdateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(this.AccountId.ToString());
        }

        public void SetPreOpen()
        {
            //lookUpTinhTrang.PreOpenAsync = async () => {
            //    LoadingHelper.Show();
            //    viewModel.CustomerStatusReasons = CustomerStatusReasonData.CustomerStatusReasons();
            //    LoadingHelper.Hide();
            //};

            lookUpOperationScope.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.OperationScopes = OperationScopeData.OperationScopes();
                LoadingHelper.Hide();
            };

            Lookup_Localization.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                AccountLocalization.Localizations();
                foreach (var item in AccountLocalization.LocalizationOptions)
                {
                    viewModel.LocalizationOptionList.Add(item);
                }
                LoadingHelper.Hide();
            };
            //Lookup_BusinessType.PreShow = async () =>
            //{
            //    LoadingHelper.Show();
            //    viewModel.LoadBusinessTypeForLookup();
            //    LoadingHelper.Hide();
            //};

            
            Lookup_PrimaryContact.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadContactForLookup();
                LoadingHelper.Hide();
            };
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

        private async void SaveData(string id)
        {
            if (viewModel.Localization == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_khach_hang);
                return;
            }
            if (viewModel.singleAccount.bsd_name == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ten_cong_ty);
                return;
            }
            if (viewModel.OperationScope == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_pham_vi_hoat_dong);
                return;
            }

            if (viewModel.PrimaryContact == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_nguoi_dai_dien);
                return;
            }
            if (viewModel.singleAccount.bsd_registrationcode == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_giay_phep_kinh_doanh);
                return;
            }
            if (!StringFormatHelper.CheckValueID(viewModel.singleAccount.bsd_registrationcode, 10))
            {
                ToastMessageHelper.ShortMessage(Language.so_gpkd_khong_hop_le_gom_10_ky_tu);
                return;
            }
            if (!await viewModel.Check_form_keydata(null, viewModel.singleAccount.bsd_registrationcode, viewModel.singleAccount.accountid.ToString()))
            {
                ToastMessageHelper.ShortMessage(Language.so_giay_phep_kinh_doanh_da_tao_trong_du_lieu_doanh_nghiep);
                return;
            }
            if (viewModel.singleAccount.bsd_vatregistrationnumber != null)
            {
                if (!await viewModel.Check_form_keydata(viewModel.singleAccount.bsd_vatregistrationnumber, null, viewModel.singleAccount.accountid.ToString()))
                {
                    ToastMessageHelper.ShortMessage(Language.ma_so_thue_da_tao_trong_du_lieu_doanh_nghiep);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(viewModel.Address1?.address))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_lien_lac);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.Address2?.address))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_tru_so_chinh);
                return;
            }
            if (viewModel.singleAccount.telephone1 == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai_cong_ty);
                return;
            }
            string phone = viewModel.singleAccount.telephone1;
            phone = phone.Contains("-") ? phone.Split('-')[1] : phone;
            if (phone.Length != 10)
            {
                ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le_gom_10_ky_tu);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.singleAccount.emailaddress1))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_email);
                return;
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.emailaddress1))
            {
                Match match = regex.Match(viewModel.singleAccount.emailaddress1);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage(Language.email_sai_dinh_dang_vui_long_thu_lai);
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_email2))
            {
                Match match = regex.Match(viewModel.singleAccount.bsd_email2);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage(Language.email_2_sai_dinh_dang_vui_long_thu_lai);
                    return;
                }
            }

            LoadingHelper.Show();
            if (viewModel.Localization != null && viewModel.Localization.Val != null)
            {
                viewModel.singleAccount.bsd_localization = viewModel.Localization.Val;
            }
            if (viewModel.PrimaryContact != null && viewModel.PrimaryContact.Id != null)
            {
                viewModel.singleAccount._primarycontactid_value = viewModel.PrimaryContact.Id;
            }
            //if (viewModel.BusinessType != null && viewModel.BusinessType.Count > 0)
            //{
            //    viewModel.singleAccount.bsd_businesstypesys = string.Join(", ", viewModel.BusinessType);
            //}
            if (id == null)
            {
                var created = await viewModel.createAccount();
                if (created)
                {
                    //if (QueueForm.NeedToRefresh.HasValue) QueueForm.NeedToRefresh = true;
                    if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                    ToastMessageHelper.ShortMessage(Language.tao_khach_hang_doanh_nghiep_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.tao_khach_hang_doanh_nghiep_that_bai);
                }
            }
            else
            {
                var updated = await viewModel.updateAccount();
                if (updated)
                {
                    if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                    if (AccountDetailPage.NeedToRefreshAccount.HasValue) AccountDetailPage.NeedToRefreshAccount = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_khach_hang_doanh_nghiep_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_khach_hang_doanh_nghiep_that_bai);
                }
            }
        }

        private void so_gpkd_Unfocused(object sender, FocusEventArgs e)
        {
            if (!StringFormatHelper.CheckValueID(viewModel.singleAccount.bsd_registrationcode, 10))
            {
                ToastMessageHelper.ShortMessage(Language.so_gpkd_khong_hop_le_gom_10_ky_tu);
                return;
            }
        }

        private async void Lookup_PrimaryContact_SearchPress(System.Object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadContactForLookup(e.Item.ToString());
            Lookup_PrimaryContact.ResetItemSource();
            LoadingHelper.Hide();
        }
    }
}