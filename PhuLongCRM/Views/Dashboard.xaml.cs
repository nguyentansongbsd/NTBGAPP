using System;
using System.Threading.Tasks;
using PhuLongCRM.Controls;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public static bool? NeedToRefreshPhoneCall = null;
        public static bool? NeedToRefreshMeet = null;
        public static bool? NeedToRefreshTask = null;
        public static bool? NeedToRefreshQueue = null;
        public static bool? NeedToRefreshLeads = null;
        public DashboardViewModel viewModel;

        public Dashboard()
        {
            InitializeComponent();
            LoadingHelper.Show();
            NeedToRefreshPhoneCall = false;
            NeedToRefreshMeet = false;
            NeedToRefreshTask = false;
            NeedToRefreshQueue = false;
            NeedToRefreshLeads = false;
            Init();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new DashboardViewModel();

            await Task.WhenAll(
                 viewModel.LoadAcceptances(),
                 viewModel.LoadUnitHandovers(),
                 viewModel.LoadPinkBookHandovers(),
                 viewModel.LoadTasks(),
                 viewModel.LoadMettings(),
                 viewModel.LoadPhoneCalls()

                );




            //viewModel.LoadQueueFourMonths(),
            //     viewModel.LoadQuoteFourMonths(),
            //     viewModel.LoadOptionEntryFourMonths(),
            //     viewModel.LoadUnitFourMonths(),
            //     viewModel.LoadLeads(),
            //viewModel.LoadCommissionTransactions()



            MessagingCenter.Subscribe<ScanQRPage, string>(this, "CallBack", async (sender, e) =>
            {
                try
                {
                    string[] data = e.Trim().Split(',');
                    if (data[1] == "lead")
                    {
                        LeadDetailPage leadDetail = new LeadDetailPage(Guid.Parse(data[2]), true);
                        leadDetail.OnCompleted = async (isSuccess) =>
                        {
                            if (isSuccess)
                            {
                                await Navigation.PushAsync(leadDetail);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                LoadingHelper.Hide();
                                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                            }
                        };
                    }
                    else if (data[1] == "account")
                    {
                        AccountDetailPage accountDetail = new AccountDetailPage(Guid.Parse(data[2]));
                        accountDetail.OnCompleted = async (isSuccess) =>
                        {
                            if (isSuccess)
                            {
                                await Navigation.PushAsync(accountDetail);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                LoadingHelper.Hide();
                                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                            }
                        };
                    }
                    else if (data[1] == "contact")
                    {
                        ContactDetailPage contactDetail = new ContactDetailPage(Guid.Parse(data[2]));
                        contactDetail.OnCompleted = async (isSuccess) =>
                        {
                            if (isSuccess)
                            {
                                await Navigation.PushAsync(contactDetail);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                LoadingHelper.Hide();
                                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                            }
                        };
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.ma_qr_khong_dung);
                    }
                }
                catch (Exception ex)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.LongMessage(ex.Message);
                }

            });
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (NeedToRefreshQueue == true)
            {
                LoadingHelper.Show();
                viewModel.DataMonthQueue.Clear();
                await viewModel.LoadQueueFourMonths();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshLeads == true)
            {
                LoadingHelper.Show();
                viewModel.LeadsChart.Clear();
                await viewModel.LoadLeads();
                NeedToRefreshLeads = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshTask == true || NeedToRefreshPhoneCall == true || NeedToRefreshMeet == true)
            {
                LoadingHelper.Show();
                viewModel.Activities.Clear();
                await Task.WhenAll(
                    viewModel.LoadMettings(),
                    viewModel.LoadTasks(),
                    viewModel.LoadPhoneCalls()
                    );

                NeedToRefreshPhoneCall = false;
                NeedToRefreshTask = false;
                NeedToRefreshMeet = false;
                LoadingHelper.Hide();
            }
        }

        private async void ShowMore_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Shell.Current.GoToAsync("//HoatDong");
            LoadingHelper.Hide();
        }

        private void ActivitiItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (ActivitiModel)((sender as ExtendedFrame).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
            LoadingHelper.Hide();
        }

        private async void ScanQRCode_Clicked(object sender, EventArgs e)
        {
            PermissionStatus camerastatus = await PermissionHelper.RequestCameraPermission();
            if (camerastatus == PermissionStatus.Granted)
            {
                LoadingHelper.Show();
                ScanQRPage scanQR = new ScanQRPage();
                await Navigation.PushAsync(scanQR);
                LoadingHelper.Hide();
            }
        }

        private void DatCoc_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.so_dat_coc_da_tao_trong_thang_nay);
            }
            catch(Exception ex)
            {

            }
        }

        private void GiaoDich_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.so_giu_cho_da_tao_trong_thang_nay);
            }
            catch (Exception ex)
            {

            }
        }
        private void HopDong_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.so_hop_dong_da_tao_trong_thang_nay);
            }
            catch (Exception ex)
            {

            }
        }
        private void DaBan_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.so_san_pham_da_ban_trong_thang_nay);
            }
            catch (Exception ex)
            {

            }
        }
        private void TongTien_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.tong_tien_co_the_nhan_trong_thang_nay);
            }
            catch (Exception ex)
            {

            }
        }
        private void DaNhan_Hover_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupHover.ShowHover(Language.tong_tien_da_nhan_trong_thang_nay);
            }
            catch (Exception ex)
            {

            }
        }

        private void BieuDo_Hover_Tapped(object sender, EventArgs e)
        {
            PopupHover.ShowHover(Language.bieu_do_hoa_hong_4_thang_gan_nhat);
        }
        private void BieuDoGiaoDich_Hover_Tapped(object sender, EventArgs e)
        {
            PopupHover.ShowHover(Language.bieu_do_giao_dich_4_thang_gan_nhat);
        }
    }
}