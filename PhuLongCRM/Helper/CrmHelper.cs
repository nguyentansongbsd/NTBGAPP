using Newtonsoft.Json;
using PhuLongCRM.Config;
using PhuLongCRM.Models;
using PhuLongCRM.Settings;
using PhuLongCRM.Views;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhuLongCRM.Helper
{
    public class CrmHelper
    {
        /// <summary>
        /// Ham nay de fetch du lieu tu crm.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EntityName">ten entity muon lay du lieu, dang so nhieu (them s)</param>
        /// <param name="FetchXml">cau fetch lay du lieu</param>
        /// <returns></returns>
        public static async Task<T> RetrieveMultiple<T>(string EntityName, string FetchXml) where T : class
        {
            try
            {
                var client = BsdHttpClient.Instance();
                string Token = UserLogged.AccessToken;
                var request = new HttpRequestMessage(HttpMethod.Get, $"{OrgConfig.ApiUrl}/{EntityName}?fetchXml={FetchXml}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<T>(body);
                    return api_Response;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (UserLogged.IsLoginByUserCRM)
                    {
                        var refreshTokenResponse = await LoginHelper.RefreshToken_UserCRM();
                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            var body = await refreshTokenResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;
                            UserLogged.RefreshToken = tokenData.refresh_token;
                            var api_Response = await RetrieveMultiple<T>(EntityName, FetchXml);
                            return api_Response;
                        }
                    }
                    else
                    {
                        var reLoginResponse = await LoginHelper.Login();
                        if (reLoginResponse.IsSuccessStatusCode)
                        {
                            var body = await reLoginResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;

                            var api_Response = await RetrieveMultiple<T>(EntityName, FetchXml);
                            return api_Response;
                        }
                    }
                }
                else
                {
                    var a = response.RequestMessage;
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);

                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return null;
        }

        public static async Task<T> RetrieveImagesSharePoint<T>(string url) where T : class
        {
            try
            {
                var client = BsdHttpClient.Instance();
                string fileListUrl = $"{OrgConfig.GraphApi}{OrgConfig.SP_SiteId}/lists/{url}";
                var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserLogged.AccessTokenSharePoint);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_response = JsonConvert.DeserializeObject<T>(body);
                    return api_response;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var loginSharePonit = await LoginHelper.getSharePointToken();
                    if (loginSharePonit.access_token != null)
                    {
                        UserLogged.AccessTokenSharePoint = loginSharePonit.access_token;
                        var api_response = await RetrieveImagesSharePoint<T>(url);
                        return api_response;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// Set giá trị Null cho field lookup
        /// </summary>
        /// <param name="EntityName">Tên của entity muốn set null field.</param>
        /// <param name="Id">Id của record chính.</param>
        /// <param name="FieldName">Tên field/Schema name của field cần xóa.</param>
        /// <returns></returns>
        public static async Task<CrmApiResponse> SetNullLookupField(string EntityName, Guid Id, string FieldName)
        {
            try
            {
                var client = BsdHttpClient.Instance();
                string Token = UserLogged.AccessToken;
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{OrgConfig.ApiUrl}/{EntityName}({Id})/{FieldName}/$ref");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new CrmApiResponse()
                    {
                        IsSuccess = true
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (UserLogged.IsLoginByUserCRM)
                    {
                        var refreshTokenResponse = await LoginHelper.RefreshToken_UserCRM();
                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            var body = await refreshTokenResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;
                            UserLogged.RefreshToken = tokenData.refresh_token;
                            var api_Response = await SetNullLookupField(EntityName, Id, FieldName);
                            return api_Response;
                        }
                        else
                        {
                            return new CrmApiResponse()
                            {
                                IsSuccess = false,
                                ErrorResponse = new ErrorResponse() { error = new Error() { message = refreshTokenResponse.RequestMessage.ToString() } }
                            };
                        }
                    }
                    else
                    {
                        var reLoginResponse = await LoginHelper.Login();
                        if (reLoginResponse.IsSuccessStatusCode)
                        {
                            var body = await reLoginResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;

                            var api_response = await SetNullLookupField(EntityName, Id, FieldName);
                            return api_response;
                        }
                        else
                        {
                            return new CrmApiResponse()
                            {
                                IsSuccess = false,
                                ErrorResponse = new ErrorResponse() { error = new Error() { message = reLoginResponse.RequestMessage.ToString() } }
                            };
                        }
                    }
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    return new CrmApiResponse()
                    {
                        IsSuccess = false,
                        ErrorResponse = api_Response
                    };
                }
            }
            catch (Exception ex)
            {
                return new CrmApiResponse()
                {
                    IsSuccess = false,
                    ErrorResponse = new ErrorResponse()
                    {
                        error = new Error()
                        {
                            code = "500",
                            message = ex.Message
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Post dữ liệu lên crm.
        /// </summary>
        /// <param name="path">Đường dẫn cần post, sau dấu sẹc (/)</param>
        /// <param name="formContent">Object data cần gửi lên (data phải chưa serializeObject), nếu post ko cần data
        /// thì gọi hàm postData ko truyền tham số formContent ví dụ : postData('/accounts/')</param>
        /// <returns></returns>
        public static async Task<CrmApiResponse> PostData(string path, object formContent = null)
        {
            string Token = UserLogged.AccessToken;
            var client = BsdHttpClient.Instance();
            CrmApiResponse res = new CrmApiResponse();
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                if (formContent == null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, OrgConfig.ApiUrl + path);
                    response = await client.SendAsync(request);
                }
                else
                {
                    string objContent = JsonConvert.SerializeObject(formContent);
                    HttpContent content = new StringContent(objContent, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(OrgConfig.ApiUrl + path, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    res.Content = body;
                    res.IsSuccess = true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (UserLogged.IsLoginByUserCRM)
                    {
                        var refreshTokenResponse = await LoginHelper.RefreshToken_UserCRM();
                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            var body = await refreshTokenResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;
                            UserLogged.RefreshToken = tokenData.refresh_token;
                            var api_Response = await PostData(path, formContent);
                            return api_Response;
                        }
                    }
                    else
                    {
                        var reLoginResponse = await LoginHelper.Login();
                        if (reLoginResponse.IsSuccessStatusCode)
                        {
                            var body = await reLoginResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;

                            var api_Response = await PostData(path, formContent);
                            return api_Response;
                        }
                    }
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    res.ErrorResponse = api_Response;
                    res.IsSuccess = false;
                }
                return res;
            }
            catch (Exception ex)
            {
                return new CrmApiResponse()
                {
                    IsSuccess = false,
                    ErrorResponse = new ErrorResponse()
                    {
                        error = new Error()
                        {
                            code = "500",
                            message = ex.Message
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Patch dữ liệu lên crm. sử dụng trong trường hợp update dữ liệu của entity.
        /// </summary>
        /// <param name="path">Đường dẫn cần post, sau dấu sẹc (/)</param>
        /// <param name="formContent">Object data cần gửi lên (data phải chưa serializeObject), nếu post ko cần data
        /// thì gọi hàm postData ko truyền tham số formContent ví dụ : postData('/accounts/')</param>
        /// <returns></returns>
        public static async Task<CrmApiResponse> PatchData(string path, object formContent)
        {
            string Token = UserLogged.AccessToken;
            var client = BsdHttpClient.Instance();
            CrmApiResponse res = new CrmApiResponse();
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, OrgConfig.ApiUrl + path);
                if (formContent != null)
                {
                    string objContent = JsonConvert.SerializeObject(formContent);
                    HttpContent content = new StringContent(objContent, Encoding.UTF8, "application/json");
                    request.Content = content;
                }
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    res.IsSuccess = true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (UserLogged.IsLoginByUserCRM)
                    {
                        var refreshTokenResponse = await LoginHelper.RefreshToken_UserCRM();
                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            var body = await refreshTokenResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;
                            UserLogged.RefreshToken = tokenData.refresh_token;
                            var api_Response = await PatchData(path, formContent);
                            return api_Response;
                        }
                    }
                    else
                    {
                        var reLoginResponse = await LoginHelper.Login();
                        if (reLoginResponse.IsSuccessStatusCode)
                        {
                            var body = await reLoginResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;

                            var api_Response = await PatchData(path, formContent);
                            return api_Response;
                        }
                    }
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    res.IsSuccess = false;
                    res.ErrorResponse = api_Response;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorResponse = new ErrorResponse()
                {
                    error = new Error()
                    {
                        message = ex.Message
                    }
                };
            }
            return res;
        }

        public static async Task<CrmApiResponse> DeleteRecord(string path)
        {
            string Token = UserLogged.AccessToken;
            var client = BsdHttpClient.Instance();
            CrmApiResponse res = new CrmApiResponse();
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync(OrgConfig.ApiUrl + path);
                if (response.IsSuccessStatusCode)
                {
                    res.IsSuccess = true;
                    // xoa du lieu thi thanh cong ko co content tra ve.. xac dinh viec xoa thanh cong hay ko bang bien isSuccess.
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (UserLogged.IsLoginByUserCRM)
                    {
                        var refreshTokenResponse = await LoginHelper.RefreshToken_UserCRM();
                        if (refreshTokenResponse.IsSuccessStatusCode)
                        {
                            var body = await refreshTokenResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;
                            UserLogged.RefreshToken = tokenData.refresh_token;
                            var api_Response = await DeleteRecord(path);
                            return api_Response;
                        }
                    }
                    else
                    {
                        var reLoginResponse = await LoginHelper.Login();
                        if (reLoginResponse.IsSuccessStatusCode)
                        {
                            var body = await reLoginResponse.Content.ReadAsStringAsync();
                            GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                            UserLogged.AccessToken = tokenData.access_token;

                            var api_Response = await DeleteRecord(path);
                            return api_Response;
                        }
                    }
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    res.IsSuccess = false;
                    res.ErrorResponse = api_Response;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorResponse = new ErrorResponse()
                {
                    error = new Error()
                    {
                        message = ex.Message
                    }
                };
            }
            return res;
        }
    }
}
