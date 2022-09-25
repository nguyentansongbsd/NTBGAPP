using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class QueuesDetialPage : ContentPage
    {
        public static string CodeQueue = "5";
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshBTG = null;
        public static bool? NeedToRefreshDC = null;
        public static bool? NeedToRefreshActivity = null;
        public static bool? NeedToRefresh = null;
        public static OptionSet FromQueue = null;
        public static OptionSet CustomerFromQueue = null;
        public QueuesDetialPageViewModel viewModel;
        public QueuesDetialPage(Guid queueId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new QueuesDetialPageViewModel();
            viewModel.QueueId = queueId;
            NeedToRefreshBTG = false;
            NeedToRefreshActivity = false;
            NeedToRefreshDC = false;
            NeedToRefresh = false;
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadQueue();
            if (viewModel.Queue != null)
            {
                FromQueue = new OptionSet { Val = viewModel.Queue.opportunityid.ToString(), Label = viewModel.Queue.name, Title = CodeQueue };
                CustomerFromQueue = viewModel.Customer;
                SetButtons();
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadQueue();
                SetButtons();
                NeedToRefreshBTG = false;
                LoadingHelper.Hide();
            }    
            if (viewModel.BangTinhGiaList != null && NeedToRefreshBTG == true)
            {
                LoadingHelper.Show();
                viewModel.BangTinhGiaList.Clear();
                viewModel.PageBangTinhGia = 1;
                await viewModel.LoadDanhSachBangTinhGia();
                NeedToRefreshBTG = false;
                LoadingHelper.Hide();
            }
            if (viewModel.DatCocList != null && NeedToRefreshDC == true)
            {
                LoadingHelper.Show();
                viewModel.DatCocList.Clear();
                viewModel.PageDatCoc = 1;
                await viewModel.LoadDanhSachDatCoc();
                viewModel.ShowBtnBangTinhGia = await viewModel.CheckReserve();
                SetButtons();
                NeedToRefreshDC = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshActivity == true)
            {
                LoadingHelper.Show();
                viewModel.PageCase = 1;
                viewModel.list_thongtincase.Clear();
                await viewModel.LoadCaseForQueue();
                ActivityPopup.Refresh();
                NeedToRefreshActivity = false;
                LoadingHelper.Hide();
            }
        }

        private async void SetButtons()
        {
            if (viewModel.Queue != null)
            {
                if (viewModel.ButtonCommandList.Count > 0)
                    viewModel.ButtonCommandList.Clear();
                if (viewModel.ShowCare)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_hop, "FontAwesomeRegular", "\uf274", null, NewMeet));
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cuoc_goi, "FontAwesomeSolid", "\uf095", null, NewPhoneCall));
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_cong_viec, "FontAwesomeSolid", "\uf073", null, NewTask));
                }

                viewModel.ShowBtnBangTinhGia = await viewModel.CheckReserve();
                if (viewModel.QueueProject == Language.co)
                    viewModel.ShowBtnBangTinhGia = false;

                if (viewModel.ShowButtons)
                {
                    if(viewModel.ShowBtnBangTinhGia)
                        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_bang_tinh_gia, "FontAwesomeRegular", "\uf0fe", null, TaoBTG));
                    if(viewModel.ShowBtnHuyGiuCho)
                        viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_giu_cho, "FontAwesomeSolid", "\uf05e", null, HuyGC));
                }
                if (viewModel.ShowBtnBangTinhGia == false && viewModel.ShowBtnHuyGiuCho == false && viewModel.ShowCare == false)
                    floatingButtonGroup.IsVisible = false;
                if (viewModel.Queue.statuscode == 100000000 || viewModel.Queue.statuscode == 100000002)
                    floatingButtonGroup.IsVisible = true;
                else
                    floatingButtonGroup.IsVisible = false;
            }
        }
        private void GoToProject_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ProjectInfo projectInfo = new ProjectInfo(viewModel.Queue._bsd_project_value);
            projectInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(projectInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private void GoToPhaseLaunch_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();

            LoadingHelper.Hide();
        }
        private void GoToUnit_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Queue._bsd_units_value);
            unitInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(unitInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }
        private void GoToAcount_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            AccountDetailPage accountDetail = new AccountDetailPage(viewModel.Queue._bsd_salesagentcompany_value);
            accountDetail.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
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
        private void GoToCollaborator_Tapped(System.Object sender, System.EventArgs e)
        {
            if (viewModel.Queue != null && viewModel.Queue.collaborator_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ContactDetailPage newpage = new ContactDetailPage(viewModel.Queue.collaborator_id);
                newpage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newpage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
        }
        private void GoToCustomerReferral_Tapped(System.Object sender, System.EventArgs e)
        {
            if (viewModel.Queue != null && viewModel.Queue.customerreferral_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ContactDetailPage newpage = new ContactDetailPage(viewModel.Queue.customerreferral_id);
                newpage.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess)
                    {
                        await Navigation.PushAsync(newpage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
        }
        private async void NhanTin_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.NumPhone))
                {
                    string phone = viewModel.NumPhone.Replace(" ", "").Replace("+84-", "").Replace("84", "");
                    LoadingHelper.Hide();
                    SmsMessage sms = new SmsMessage(null, phone);
                    await Sms.ComposeAsync(sms);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_co_so_dien_thoai);
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(ex.Message);
            }
        }
        private async void GoiDien_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.NumPhone))
                {
                    string phone = viewModel.NumPhone.Replace(" ", "").Replace("+84-", "").Replace("84", "");
                    await Launcher.OpenAsync($"tel:{phone}");
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.khong_co_so_dien_thoai);
                }
                LoadingHelper.Hide();
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(ex.Message);
            }
        }
        private void CreateQuotation_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            OptionSet Queue = new OptionSet(viewModel.Queue.opportunityid.ToString(), viewModel.Queue.name);
            OptionSet SaleAgentCompany = new OptionSet(viewModel.Queue._bsd_salesagentcompany_value.ToString(), viewModel.Queue.salesagentcompany_name);
            string NameOfStaffAgent = viewModel.Queue.bsd_nameofstaffagent;

            ReservationForm reservationForm = new ReservationForm(viewModel.Queue._bsd_units_value, Queue, SaleAgentCompany, NameOfStaffAgent, viewModel.Customer);
            reservationForm.CheckReservation = async (isSuccess) =>
            {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservationForm);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }
        
        private async void ShowMoreBangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageBangTinhGia++;
            await viewModel.LoadDanhSachBangTinhGia();
            LoadingHelper.Hide();
        }
        private async void ShowMoreDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDatCoc++;
            await viewModel.LoadDanhSachDatCoc();
            LoadingHelper.Hide();
        }
        private async void ShowMoreHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageHopDong++;
            await viewModel.LoadDanhSachHopDong();
            LoadingHelper.Hide();
        }
        private void ItemQuatation_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(itemId) { Title = Language.bang_tinh_gia_title };
            bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(bangTinhGiaDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private void ItemReservation_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(itemId, true) { Title = Language.dat_coc_title };
            bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(bangTinhGiaDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private void ItemHopDong_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(itemId);
            contractDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contractDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }
        private void Customer_Tapped(object sender, EventArgs e)
        {
            if(viewModel.Customer!= null)
            {
                LoadingHelper.Show();
                if (viewModel.Customer.Title == viewModel.CodeContact)
                {
                    ContactDetailPage newPage = new ContactDetailPage(Guid.Parse(viewModel.Customer.Val));
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
                else if (viewModel.Customer.Title == viewModel.CodeAccount)
                {
                    AccountDetailPage newPage = new AccountDetailPage(Guid.Parse(viewModel.Customer.Val));
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
        }

        //private async void btnHuyGiuChoThuKy_Clicked(object sender, EventArgs e)
        //{
        //    bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn hủy giữ chỗ này không ?", "Đồng ý", "Hủy");
        //    if (confirm == false) return;

        //    LoadingHelper.Show();
        //    // string url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
        //    string url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Opportunity_HuyGiuChoCoTien";
        //    var content = new { };
        //    CrmApiResponse res = await CrmHelper.PostData(url_action, content);
        //    Message(res.IsSuccess);            
        //}
        private async void Message(bool IsSuccess)
        {
            if (IsSuccess)
            {
                await Task.WhenAll(
                    viewModel.updateStatusUnit(),
                    viewModel.LoadQueue()
               );
                //await viewModel.updateStatusUnit();
                //await viewModel.LoadQueue();
                SetButtons();
                if (QueueList.NeedToRefresh.HasValue) QueueList.NeedToRefresh = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                if (ProjectInfo.NeedToRefreshQueue.HasValue) ProjectInfo.NeedToRefreshQueue = true;
                if (ProjectInfo.NeedToRefreshNumQueue.HasValue) ProjectInfo.NeedToRefreshNumQueue = true;
                if (UnitInfo.NeedToRefreshQueue.HasValue) UnitInfo.NeedToRefreshQueue = true;
                if (AccountDetailPage.NeedToRefreshQueues.HasValue) AccountDetailPage.NeedToRefreshQueues = true;
                if (ContactDetailPage.NeedToRefreshQueues.HasValue) ContactDetailPage.NeedToRefreshQueues = true;
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
        }
        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadCaseForQueue();
            LoadingHelper.Hide();
        }
        private async void NewMeet(object sender, EventArgs e)
        {
            if (viewModel.Queue != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new MeetingForm());
                LoadingHelper.Hide();
            }
        }
        private async void NewPhoneCall(object sender, EventArgs e)
        {
            if (viewModel.Queue != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new PhoneCallForm());
                LoadingHelper.Hide();
            }
        }
        private async void NewTask(object sender, EventArgs e)
        {
            if (viewModel.Queue != null)
            {
                LoadingHelper.Show();
                await Navigation.PushAsync(new TaskForm());
                LoadingHelper.Hide();
            }
        }
        private void CaseItem_Tapped(object sender, EventArgs e)
        {
            var item = (HoatDongListModel)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null && item.activityid != Guid.Empty)
            {
                ActivityPopup.ShowActivityPopup(item.activityid, item.activitytypecode);
            }
        }
        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }
        private async void HuyGC(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_huy_giu_cho_nay_khong, Language.dong_y, Language.huy);
            if (confirm == false) return;

            string url_action = "";
            if (viewModel.Queue != null)
            {
                if (viewModel.Queue.statuscode == 100000002)
                {
                    url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
                }
                else if (viewModel.Queue.statuscode == 100000000)
                {
                    if (await viewModel.CheckQuote())
                    {
                        Message(false);
                        return;
                    }
                    else
                        url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
                }
                else if (viewModel.Queue.statuscode == 100000008)
                {
                    url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Opportunity_HuyGiuChoCoTien";
                }
            }

            LoadingHelper.Show();
            var data = new
            {
                input = "Yes"
            };
            CrmApiResponse res = await CrmHelper.PostData(url_action, data);
            if (res.IsSuccess == true)
            {
                Message(res.IsSuccess);
                if (res.IsSuccess)
                {
                    NeedToRefreshBTG = true;
                    OnAppearing();
                }
            }
            else
                ToastMessageHelper.ShortMessage(res.ErrorResponse?.error.message);
        }
        private void TaoBTG(object sender, EventArgs e)
        {
            if (viewModel.Queue.unit_status != "100000000" && viewModel.Queue.unit_status != "100000004") //queueing, available
            {
                ToastMessageHelper.ShortMessage(Language.can_ho_phai_o_tinh_trang_san_sang_hoac_giu_cho);
                return;
            }
            LoadingHelper.Show();
            OptionSet Queue = new OptionSet(viewModel.Queue.opportunityid.ToString(), viewModel.Queue.name);
            OptionSet SaleAgentCompany = new OptionSet(viewModel.Queue._bsd_salesagentcompany_value.ToString(), viewModel.Queue.salesagentcompany_name);
            string NameOfStaffAgent = viewModel.Queue.bsd_nameofstaffagent;

            ReservationForm reservationForm = new ReservationForm(viewModel.Queue._bsd_units_value, Queue, SaleAgentCompany, NameOfStaffAgent, viewModel.Customer);
            reservationForm.CheckReservation = async (isSuccess) =>
            {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservationForm);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if(e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    stThongTin.IsVisible = true;
                    stGiaoDich.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    stThongTin.IsVisible = false;
                    stGiaoDich.IsVisible = true;
                    LoadingHelper.Show();
                    if (viewModel.BangTinhGiaList == null && viewModel.DatCocList == null && viewModel.HopDongList == null)
                    {
                        viewModel.BangTinhGiaList = new ObservableCollection<ReservationListModel>();
                        viewModel.DatCocList = new ObservableCollection<ReservationListModel>();
                        viewModel.HopDongList = new ObservableCollection<ContractModel>();
                        await Task.WhenAll(
                            viewModel.LoadDanhSachBangTinhGia(),
                            viewModel.LoadDanhSachDatCoc(),
                            viewModel.LoadDanhSachHopDong(),
                            viewModel.LoadCaseForQueue()
                            );
                    }
                    LoadingHelper.Hide();
                }
            }    
        }
    }
}
