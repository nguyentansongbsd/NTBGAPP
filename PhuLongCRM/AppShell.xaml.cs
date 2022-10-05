using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhuLongCRM.Helper;
using PhuLongCRM.IServices;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.Views;
using Xamarin.Forms;

namespace PhuLongCRM
{
    public partial class AppShell : Shell
    {
        public static bool? NeedToRefeshUserInfo = null;
        private string _avartar;
        public string Avartar { get => _avartar; set { _avartar = value; OnPropertyChanged(nameof(Avartar)); } }

        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(nameof(UserName)); } }

        private string _contactName;
        public string ContactName { get => _contactName; set { _contactName = value; OnPropertyChanged(nameof(ContactName)); } }

        private string _verApp;
        public string VerApp { get => _verApp; set { _verApp = value; OnPropertyChanged(nameof(VerApp)); } }

        private UserCRMInfoPage userCRMInfo;

        public AppShell()
        {
            InitializeComponent();
            UserName = UserLogged.User;
            ContactName = string.IsNullOrWhiteSpace(UserLogged.ContactName) ? UserLogged.User : UserLogged.ContactName;
            Avartar = UserLogged.Avartar;
            NeedToRefeshUserInfo = false;
            VerApp = Config.OrgConfig.VerApp;
            this.BindingContext = this;
        }

        public AppShell(bool isLoginByUserCrm)
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            VerApp = Config.OrgConfig.VerApp;
            this.ContactName = UserLogged.ContactName;
            this.UserName = UserLogged.UserCRM;
            this.Avartar = $"https://ui-avatars.com/api/?background=2196F3&rounded=false&color=ffffff&size=150&length=2&name={UserLogged.UserCRM}";
            this.BindingContext = this;
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            if (NeedToRefeshUserInfo == true)
            {
                LoadingHelper.Show();
                if (Avartar != UserLogged.Avartar)
                {
                    Avartar = UserLogged.Avartar;
                }
                ContactName = UserLogged.ContactName;
                NeedToRefeshUserInfo = false;
                LoadingHelper.Hide();
            }
        }

        private void UserInfor_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (!UserLogged.IsLoginByUserCRM && UserLogged.ContactId == Guid.Empty)
            {
                ToastMessageHelper.ShortMessage(Language.chua_co_contact_khong_the_chinh_sua_thong_tin);
                LoadingHelper.Hide();
                return;
            }
            if (UserLogged.IsLoginByUserCRM)
            {
                UserCRMInfoPage userCRMInfo = new UserCRMInfoPage();
                userCRMInfo.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(userCRMInfo);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_user);
                    }
                };
            }
            else
            {
                UserInfoPage userInfo = new UserInfoPage();
                userInfo.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(userInfo);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_user);
                    }
                };
            }
            //await Shell.Current.Navigation.PushAsync(new UserInfoPage());
            this.FlyoutIsPresented = false;
            //LoadingHelper.Hide();
        }

        private async void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            UserLogged.CountLoginError = 0;
            if (UserLogged.IsLoginByUserCRM)
                DependencyService.Get<IClearCookies>().ClearAllCookies(); ;
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
