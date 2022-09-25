using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhuLongCRM.Config;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;

namespace PhuLongCRM.Helper
{
    public class LoginHelper
    {
        public static async Task<HttpResponseMessage> Login()
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/87bbdb08-48ba-4dbf-9c53-92ceae16c353/oauth2/token");//OrgConfig.LinkLogin
            var formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("resource", OrgConfig.Resource),
                        new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", OrgConfig.ClientId),

                    });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> LoginByUserCRM(string code)
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{OrgConfig.TeantId}/oauth2/v2.0/token");
            var formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", OrgConfig.ClientId_ForUserCRM),
                        new KeyValuePair<string, string>("scope", OrgConfig.Scope),
                        new KeyValuePair<string, string>("redirect_uri", OrgConfig.Redirect_Uri),
                        new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret_ForUserCRM),
                    });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> RefreshToken_UserCRM()
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{OrgConfig.TeantId}/oauth2/v2.0/token");
            var formContent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("client_id", OrgConfig.ClientId_ForUserCRM),
                        new KeyValuePair<string, string>("scope", OrgConfig.Scope),
                        new KeyValuePair<string, string>("refresh_token", UserLogged.RefreshToken),
                        new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret_ForUserCRM),
            });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            return response;
        }
        public static async Task<GetTokenResponse> getSharePointToken()
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{OrgConfig.TeantId}/oauth2/token");
            var formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("client_id", OrgConfig.ClientId_ForUserCRM),
                        new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret_ForUserCRM),
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("resource", OrgConfig.GraphReSource)
                    });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
            return tokenData;
        }
    }
}
