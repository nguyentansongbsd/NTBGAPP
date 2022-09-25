using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class ConformOTPPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });

        private List<OTPModel> OTPList { get; set; } 
        public Action<bool> OnCompeleted;
        private string Phone { get; set; }
        private string fireBaseDb = "PhuLongOTPDb";

        public ConformOTPPage(string phone)
        {
            InitializeComponent();
            Phone = phone;
            Init();
        }

        public async void Init()
        {
            bool SendSuccess = await SendOTP();
            if (SendSuccess)
                OnCompeleted?.Invoke(true);
            else
                OnCompeleted?.Invoke(false);
        }

        public async Task<bool> SendOTP()
        {
            try
            {
                string otpCode = OTPCode();
                OTPModel data = new OTPModel()
                {
                    Id = Guid.NewGuid(),
                    Content = $"{otpCode} is your PhuLongCRM verification code.",
                    Phone = $"{Phone}",
                    OTPCode = otpCode,
                    IsSend = false,
                    Date= DateTime.Now
                };

                var result = await firebaseClient.Child(fireBaseDb).PostAsync<OTPModel>(data);
                if (result != null)
                {
                    OTPList = new List<OTPModel>();
                    OTPList = (await firebaseClient.Child(fireBaseDb).OnceAsync<OTPModel>()).Select(item => new OTPModel()
                    {
                        key = item.Key,
                        Id = item.Object.Id,
                        Content = item.Object.Content,
                        Phone = item.Object.Phone,
                        OTPCode = item.Object.OTPCode,
                        Date = item.Object.Date
                    }).ToList();
                    return true;
                }
                else
                {
                    ToastMessageHelper.LongMessage("Lỗi. Không thể lưu dữ liệu vào Firebase.");
                    return false;
                }
            }
            catch(FirebaseException ex)
            {
                ToastMessageHelper.LongMessage(ex.Message);
                return false;
            }
        }

        private string OTPCode()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        private void MainEntry_Unfocused(object sender, FocusEventArgs e)
        {
            mainEntry.Focus();
        }

        private async void ConfirmOTP_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string ConformCode = Code1.Text + Code2.Text + Code3.Text + Code4.Text;
            ConformCode = ConformCode.Replace(" ", "");
            if (OTPList.Any(x=>x.OTPCode == ConformCode && x.Phone == Phone))
            {
                if (ForgotPassWordPage.NeedRefreshForm.HasValue) ForgotPassWordPage.NeedRefreshForm = true;
                await DeleteRecordFirebase(); 
                await Navigation.PopAsync();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.ma_xac_thuc_khong_dung);
                mainEntry.Focus();
            }
        }

        private async Task DeleteRecordFirebase()
        {
            var key = OTPList.OrderByDescending(x=>x.Date).FirstOrDefault(x => x.Phone == Phone).key;
            await firebaseClient.Child(fireBaseDb).Child(key).DeleteAsync();
        }

        public void SetEnableButtonConform()
        {
            if (string.IsNullOrWhiteSpace(Code1.Text)
                || string.IsNullOrWhiteSpace(Code2.Text)
                || string.IsNullOrWhiteSpace(Code3.Text)
                || string.IsNullOrWhiteSpace(Code4.Text)
                )
            {
                BtnConform.IsEnabled = false;
            }
            else
            {
                BtnConform.IsEnabled = true;
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Resend_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            mainEntry.Text = "";
            await this.SendOTP();
            LoadingHelper.Hide();
        }

        public void mainEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            string text = e.NewTextValue?.Trim() ?? "";
            try
            {
                Code1.Text = text[0].ToString();
                Code1.IsVisible = true;
                ShowHideBoxView(Code1, false);
            }
            catch
            {
                Code1.Text = "";
                Code1.IsVisible = false;
                ShowHideBoxView(Code1, true);
            }

            try
            {
                Code2.Text = text[1].ToString();
                Code2.IsVisible = true;
                ShowHideBoxView(Code2, false);
            }
            catch
            {
                Code2.Text = "";
                Code2.IsVisible = false;
                ShowHideBoxView(Code2, true);
            }


            try
            {
                Code3.Text = text[2].ToString();
                Code3.IsVisible = true;
                ShowHideBoxView(Code3, false);
            }
            catch
            {
                Code3.Text = "";
                Code3.IsVisible = false;
                ShowHideBoxView(Code3, true);
            }

            try
            {
                Code4.Text = text[3].ToString();
                Code4.IsVisible = true;
                ShowHideBoxView(Code4, false);
            }
            catch
            {
                Code4.Text = "";
                Code4.IsVisible = false;
                ShowHideBoxView(Code4, true);
            }

            SetEnableButtonConform();

            if (BtnConform.IsEnabled)
            {
                ConfirmOTP_Clicked(BtnConform, EventArgs.Empty);
            }
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            mainEntry.Focus();
        }

        public void ShowHideBoxView(Label label, bool show)
        {
            Grid grid = label.Parent as Grid;
            BoxView boxView = grid.Children[0] as BoxView;
            boxView.IsVisible = show;

        }
    }
}
