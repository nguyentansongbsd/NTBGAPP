
using Newtonsoft.Json;
using PhuLongCRM.Helper;
using PhuLongCRM.IServices;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class Login : ContentPage
    {
        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(nameof(UserName)); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        private bool _eyePass = false;
        public bool EyePass { get => _eyePass; set { _eyePass = value; OnPropertyChanged(nameof(EyePass)); } }
        private bool _issShowPass = true;
        public bool IsShowPass { get => _issShowPass; set { _issShowPass = value; OnPropertyChanged(nameof(IsShowPass)); } }

        private string _verApp;
        public string VerApp { get => _verApp; set { _verApp = value; OnPropertyChanged(nameof(VerApp)); } }

        public string ImeiNum { get; set; }
        public Login()
        {
            InitializeComponent();
            this.BindingContext = this;

            VerApp = Config.OrgConfig.VerApp;
            if (UserLogged.IsLogged && UserLogged.IsSaveInforUser)
            {
                checkboxRememberAcc.IsChecked = true;
                UserName = UserLogged.User;
                Password = UserLogged.Password;
                SetGridUserName();
                SetGridPassword();
            }
            else
            {
                checkboxRememberAcc.IsChecked = false;
            }

            if (UserLogged.Language == "vi")
            {
                flagVN.BorderColor = Color.FromHex("#2196F3");
                flagEN.BorderColor = Color.FromHex("#eeeeee");
            }

            else if (UserLogged.Language == "en")
            {
                flagVN.BorderColor = Color.FromHex("#eeeeee");
                flagEN.BorderColor = Color.FromHex("#2196F3");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void IsRemember_Tapped(object sender, EventArgs e)
        {
            checkboxRememberAcc.IsChecked = !checkboxRememberAcc.IsChecked;
        }

        private void UserName_Focused(object sender, EventArgs e)
        {
            entryUserName.Placeholder = "";
            SetGridUserName();
        }

        private void UserName_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                Grid.SetRow(lblUserName, 0);
                Grid.SetRow(entryUserName, 0);
                Grid.SetRowSpan(entryUserName, 2);             
                
                entryUserName.Placeholder = Language.ten_dang_nhap;               
            }
        }

        private void SetGridUserName()
        {
            Grid.SetRow(lblUserName, 0);
            Grid.SetRow(entryUserName, 1);
            Grid.SetRowSpan(entryUserName, 1);
        }

        private void Password_Focused(object sender, EventArgs e)
        {
            entryPassword.Placeholder = "";
            SetGridPassword();
            lblEyePass.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Password_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                Grid.SetRow(lblPassword, 1);
                Grid.SetRow(entryPassword, 1);
                Grid.SetRowSpan(entryPassword, 2);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    lblEyePass.Margin = new Thickness(0, 0, 0, -13);
                }
                else
                {
                    lblEyePass.Margin = new Thickness(0, 0, 0, -10);
                }

                EyePass = false;
                entryPassword.Placeholder = Language.mat_khau;
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (!EyePass)
            {
                EyePass = string.IsNullOrWhiteSpace(Password) ? false : true;
            }
        }

        private void SetGridPassword()
        {
            Grid.SetRow(lblPassword, 0);
            Grid.SetRow(entryPassword, 1);
            Grid.SetRowSpan(entryPassword, 1);
        }

        private void ShowHidePass_Tapped(object sender, EventArgs e)
        {
            IsShowPass = !IsShowPass;
            if(IsShowPass)
            {
                lblEyePass.Text = "\uf070";
            }
            else
            {
                lblEyePass.Text = "\uf06e";
            }    
        }

        private void Flag_Tapped(object sender, EventArgs e)
        {
            string code = (string)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (code == UserLogged.Language) return;
            LoadingHelper.Show();
            UserLogged.Language = code;
            CultureInfo cultureInfo = new CultureInfo(UserLogged.Language);
            Language.Culture = cultureInfo;
            if (code == "vi")
            {
                flagVN.BorderColor = Color.FromHex("#2196F3");
                flagEN.BorderColor = Color.FromHex("#eeeeee");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
            }
                
            else if (code == "en")
            {
                flagVN.BorderColor = Color.FromHex("#eeeeee");
                flagEN.BorderColor = Color.FromHex("#2196F3");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            }
            ChangedLanguage();
            LoadingHelper.Hide();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_khong_duoc_de_trong);
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duong_de_trong);
                return;
            }
            if (!string.IsNullOrWhiteSpace(UserLogged.DateLoginError) && DateTime.Parse(UserLogged.DateLoginError).Date == DateTime.Now.Date && UserLogged.CountLoginError == 5)
            {
                ToastMessageHelper.ShortMessage(Language.khong_the_dang_nhap_vi_ban_da_dang_nhap_qua_so_lan_gioi_han);
                return;
            }
            try
            {
                LoadingHelper.Show();
                var response = await LoginHelper.Login();
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                    UserLogged.AccessToken = tokenData.access_token;
                    UserLogged.UserAttribute = "bsd_employee";

                    EmployeeModel employeeModel = await LoginUser();
                    if (employeeModel != null)
                    {
                        if (employeeModel.bsd_name != UserName)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_hoac_mat_khau_khong_chinh_xac);
                            UserLogged.CountLoginError++;
                            UserLogged.DateLoginError = DateTime.Now.ToString();
                            return;
                        }

                        if (employeeModel.bsd_password != Password)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_hoac_mat_khau_khong_chinh_xac);
                            UserLogged.CountLoginError++;
                            UserLogged.DateLoginError = DateTime.Now.ToString();
                            return;
                        }

                        ImeiNum = await DependencyService.Get<INumImeiService>().GetImei();

                        if (string.IsNullOrWhiteSpace(employeeModel.bsd_imeinumber))
                        {
                            await UpdateImei(employeeModel.bsd_employeeid.ToString()) ;
                        }
                        //else if (employeeModel.bsd_imeinumber != ImeiNum)
                        //{
                        //    LoadingHelper.Hide();
                            //ToastMessageHelper.ShortMessage(Language.tai_khoan_khong_the_dang_nhap_tren_thiet_bi_nay);
                        //    return;
                        //}
                        
                        UserLogged.Id = employeeModel.bsd_employeeid;
                        UserLogged.User = employeeModel.bsd_name;
                        UserLogged.Avartar = employeeModel.bsd_avatar;
                        UserLogged.Password = employeeModel.bsd_password;
                        UserLogged.ContactId = employeeModel.contact_id;
                        UserLogged.ContactName = employeeModel.contact_name;
                        UserLogged.ManagerId = employeeModel.manager_id;
                        UserLogged.ManagerName = employeeModel.manager_name;
                        UserLogged.IsSaveInforUser = checkboxRememberAcc.IsChecked;
                        UserLogged.IsLogged = true;
                        UserLogged.IsLoginByUserCRM = false;

                        Application.Current.MainPage = new AppShell();
                        await Task.Delay(1);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_hoac_mat_khau_khong_chinh_xac);
                        UserLogged.CountLoginError++;
                        UserLogged.DateLoginError = DateTime.Now.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                await DisplayAlert(Language.thong_bao, $"{Language.loi_ket_noi_dern_server} \n" + ex.Message, Language.dong);
            }
        }

        public async Task<EmployeeModel> LoginUser()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='bsd_employee'>
                    <attribute name='bsd_employeeid' />
                    <attribute name='bsd_name' />
                    <attribute name='createdon' />
                    <attribute name='bsd_password' />
                    <attribute name='bsd_imeinumber' />
                    <attribute name='bsd_manager' />
                    <attribute name='bsd_avatar' />
                    <order attribute='bsd_name' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_name' operator='eq' value='{UserName}' />
                    </filter>
                    <link-entity name='systemuser' from='systemuserid' to='bsd_manager' visible='false' link-type='outer' alias='a_548d21d0fee9eb11bacb002248163181'>
                      <attribute name='fullname' alias='manager_name'/>
                      <attribute name='systemuserid' alias='manager_id' />
                    </link-entity>
                    <link-entity name='contact' from='contactid' to='bsd_contact' visible='false' link-type='outer' alias='a_5b790f4631f4eb1194ef000d3a801090'>
                      <attribute name='contactid' alias='contact_id'/>
                      <attribute name='bsd_fullname' alias='contact_name'/>
                    </link-entity>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EmployeeModel>>("bsd_employees", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return null;
            }
            else
            {
                return result.value.FirstOrDefault();
            }
        }

        public async Task UpdateImei(string employeeId)
        {
            string path = $"/bsd_employees({employeeId})";
            var content = await getContent();
            CrmApiResponse crmApiResponse = await CrmHelper.PatchData(path, content);
            if (!crmApiResponse.IsSuccess)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.khong_cap_nhat_duoc_thong_tin_imei);
                return;
            }
        }

        private async Task<object> getContent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_imeinumber"] = ImeiNum;
            return data;
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ForgotPassWordPage());
            LoadingHelper.Hide();
        }

        private async void LoginUserCRM_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new LoginByUserCRMPage());
        }

        private void ChangedLanguage()
        {
            lblUserName.Text = Language.ten_dang_nhap;
            entryUserName.Placeholder = Language.ten_dang_nhap;
            lblPassword.Text = Language.mat_khau;
            entryPassword.Placeholder = Language.mat_khau;
            lbRemember.Text = Language.ghi_nho_dang_nhap;
            lbfogotPassword.Text = Language.quen_mat_khau;
            btnLogin.Text = Language.dang_nhap;
            btnLoginUserCRM.Text = Language.dang_nhap_voi_user_crm;
        }
    }
}