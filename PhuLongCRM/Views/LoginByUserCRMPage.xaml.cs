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
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class LoginByUserCRMPage : ContentPage
    {
        private List<OptionSet> Roles = new List<OptionSet>();
        private string loginUrl = "https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize?client_id={1}&response_type=code&redirect_uri={2}&response_mode=query&scope={3}&state=12345";
        private JwtModel model;

        public LoginByUserCRMPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            await Task.Delay(1);
            loginWeb.Source = string.Format(loginUrl,Config.OrgConfig.TeantId,Config.OrgConfig.ClientId_ForUserCRM,Config.OrgConfig.Redirect_Uri,Config.OrgConfig.Scope);
            LoadingHelper.Hide();
        }

        private async void loginWeb_Navigating(System.Object sender, Xamarin.Forms.WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith(Config.OrgConfig.Redirect_Uri))
            {
                LoadingHelper.Show();
                string code = e.Url.Split('?', '&')[1].Split('=')[1];
                var response = await LoginHelper.LoginByUserCRM(code);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                    UserLogged.AccessToken = tokenData.access_token;
                    UserLogged.RefreshToken = tokenData.refresh_token;

                    await LoadUserCRM();
                }
                else
                {
                    LoadingHelper.Hide();
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage("Lỗi đăng nhập");
                }
            }
        }

        private async Task LoadUserCRM()
        {
            LoadingHelper.Show();
            var decodedToken = Decoder.DecodeToken(UserLogged.AccessToken);
            model = JsonConvert.DeserializeObject<JwtModel>(decodedToken.Payload);

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='systemuser'>
                                    <attribute name='systemuserid' />
                                    <order attribute='fullname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='domainname' operator='eq' value='{model.unique_name}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UserModel>>("systemusers", fetchXml);
            if (result != null || result.value.Count > 0)
            {
                UserModel user = result.value.SingleOrDefault();

                bool IsSalesRole = await CheckUserRole(user.systemuserid);
                if (IsSalesRole)
                {
                    UserLogged.UserCRM = model.name;
                    UserLogged.ContactName = model.unique_name;
                    UserLogged.Id = user.systemuserid;
                    UserLogged.IsLoginByUserCRM = true;
                    UserLogged.IsLogged = true;
                    UserLogged.UserAttribute = "ownerid";

                    Application.Current.MainPage = new AppShell(true);
                    await Task.Delay(1);
                }
                else
                {
                    DependencyService.Get<IClearCookies>().ClearAllCookies();
                    ToastMessageHelper.LongMessage("Vui lòng đăng nhập tài khoản kinh doanh.");
                    await Navigation.PopAsync();
                }
                
                LoadingHelper.Hide();
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_user);
                LoadingHelper.Hide();
            }
        }

        private async Task<bool> CheckUserRole(Guid id)
        {
            string fetchXml = $@"<fetch>
                                  <entity name='role'>
                                    <attribute name='name' alias='Label'/>
                                    <link-entity name='systemuserroles' from='roleid' to='roleid' intersect='true'>
                                      <filter>
                                        <condition attribute='systemuserid' operator='eq' value='{id}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("roles", fetchXml);
            if (result == null || result.value.Count == 0) return false;

            if (result.value.Any(x => x.Label.StartsWith("PL_S&M_")))
                return true;
            else
                return false;
        }
    }
}
