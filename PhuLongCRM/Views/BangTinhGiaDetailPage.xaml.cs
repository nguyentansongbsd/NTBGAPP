using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BangTinhGiaDetailPage : ContentPage
    {
        private BangTinhGiaDetailPageViewModel viewModel;
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh = null; 
        public static bool? NeedToRefreshInstallment = null;

        public BangTinhGiaDetailPage(Guid id, bool isContract = false)
        {
            InitializeComponent();
            BindingContext = viewModel = new BangTinhGiaDetailPageViewModel();
            viewModel.ReservationId = id;
            NeedToRefresh = false;
            NeedToRefreshInstallment = false;
            Init();
            InitContract(isContract);
        }

        public async void Init()
        {
            await Task.WhenAll(
                LoadDataChinhSach(viewModel.ReservationId),
                viewModel.LoadCoOwners(viewModel.ReservationId)
                );

            MessagingCenter.Subscribe<BangTinhGiaDetailPageViewModel>(this, "IsRefresh", (sender) => {
                SetUpButtonGroup();
            });

            SetUpButtonGroup();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();

                viewModel.CoownerList.Clear();
                viewModel.ListDiscountPaymentScheme.Clear();
                viewModel.ListDiscount.Clear();
                viewModel.ListPromotion.Clear();
                viewModel.ListDiscountInternel.Clear();
                viewModel.ListDiscountExchange.Clear();
                viewModel.InstallmentList.Clear();
                viewModel.ShowInstallmentList = false;
                viewModel.NumberInstallment = 0;
                
                await Task.WhenAll(
                    LoadDataChinhSach(viewModel.ReservationId),
                    viewModel.LoadCoOwners(viewModel.ReservationId)
                );
                if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshInstallment == true)
            {
                LoadingHelper.Show();

                viewModel.ShowInstallmentList = false;
                viewModel.NumberInstallment = 0;
                viewModel.InstallmentList.Clear();
                
                await viewModel.LoadInstallmentList(viewModel.ReservationId);
                LoadingHelper.Hide();
            }
            if (NeedToRefreshInstallment == true || NeedToRefresh == true)
            {
                viewModel.ButtonCommandList.Clear();
                SetUpButtonGroup();
                NeedToRefreshInstallment = false;
                NeedToRefresh = false;
            }
        }

        //tab chinh sach
        private async Task LoadDataChinhSach(Guid id)
        {
            if (id != Guid.Empty)
            {
                await Task.WhenAll(
                    viewModel.LoadReservation(id),
                    viewModel.LoadPromotions(viewModel.ReservationId),
                    viewModel.LoadSpecialDiscount(viewModel.ReservationId),
                    viewModel.LoadInstallmentList(viewModel.ReservationId)
                    );
                await Task.WhenAll(
                    viewModel.LoadDiscounts(),
                    viewModel.LoadDiscountsPaymentScheme(),
                    viewModel.LoadDiscountsInternel(),
                    viewModel.LoadDiscountsExChange()
                    ) ;
                await viewModel.LoadHandoverCondition(viewModel.ReservationId);
               // SutUpSpecialDiscount();
            }
        }

        private void GoToQueueDetail_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueuesDetialPage queuesDetial = new QueuesDetialPage(viewModel.Reservation.queue_id);
            queuesDetial.OnCompleted = async (isSuccess) => {
                if (isSuccess)
                {
                    await Navigation.PushAsync(queuesDetial);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        // tab lich
        private async void LoadInstallmentList(Guid id)
        {
            if (id != Guid.Empty && viewModel.InstallmentList != null && viewModel.InstallmentList.Count == 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadInstallmentList(id);
                LoadingHelper.Hide();
            }
        }

        public async void SetUpButtonGroup()
        {
            var checkFul = await viewModel.CheckFUL();
            if (viewModel.Reservation.statuscode == 100000000)// (hủy đặt cọc)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_dat_coc, "FontAwesomeSolid", "\uf05e", null, CancelDeposit));
            }
            if (viewModel.Reservation.statuscode == 3 && checkFul == true)// show khi statuscode == 3(Deposited) (tạo ful)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.de_nghi_thanh_ly, "FontAwesomeSolid", "\uf560", null, FULTerminate));
            }
            if (viewModel.Reservation.statuscode == 4)// show khi statuscode == 4(Won) (đi đến contract) 
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.di_den_hop_dong, "FontAwesomeSolid", "\uf56c", null, GoToContract));
            }

            if (viewModel.Reservation.statuscode == 100000007)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat_bang_tinh_gia, "FontAwesomeRegular", "\uf044", null, EditQuotes));
                if (viewModel.InstallmentList?.Count == 0)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_lich_thanh_toan, "FontAwesomeRegular", "\uf271", null, CreatePaymentScheme));
                }
                if (viewModel.InstallmentList?.Count > 0)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xoa_lich_thanh_toan, "FontAwesomeRegular", "\uf1c3", null, CancelInstallment));
                }
                if (!viewModel.Reservation.bsd_quotationprinteddate.HasValue)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_in, "FontAwesomeSolid", "\uf02f", null, ConfirmSigning));
                }
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_bang_tinh_gia, "FontAwesomeRegular", "\uf273", null, CancelQuotes));
                if (viewModel.InstallmentList.Count > 0 && viewModel.Reservation.bsd_quotationprinteddate != null)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.ky_bang_tinh_gia, "FontAwesomeRegular", "\uf274", null, SignQuotationClicked));
                }
            }

            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate == null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_tai_pdc, "FontAwesomeRegular", "\uf15c", null, ConfirmReservation));
            }
            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate != null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.ky_phieu_dat_coc, "FontAwesomeRegular", "\uf274", null, CompletedReservation));
            }

            if (viewModel.ButtonCommandList.Count > 0)
            {
                floatingButtonGroup.IsVisible = true;
            }
            else
            {
                floatingButtonGroup.IsVisible = false;
            }
        }

        private void InitContract(bool _isContract)
        {
            if (_isContract)
            {
                ma_dat_coc.IsVisible = true;
                ma_bang_tinh_gia.IsVisible = false;
                this.Title = Language.dat_coc_title;
            }
            else
            {
                ma_dat_coc.IsVisible = false;
                ma_bang_tinh_gia.IsVisible = true;
                this.Title = Language.bang_tinh_gia_title;
            }

        }
        private void GoToContract(object sender, EventArgs e)
        {
            if (viewModel.Reservation != null && viewModel.Reservation.salesorder_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ContractDetailPage contractDetailPage = new ContractDetailPage(viewModel.Reservation.salesorder_id);
                contractDetailPage.OnCompleted = async (OnCompleted) =>
                {
                    if (OnCompleted == true)
                    {
                        await Navigation.PushAsync(contractDetailPage);
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
                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            }
        }
        private void FULTerminate(object sender, EventArgs e)
        {
            if (viewModel.Reservation != null && viewModel.Reservation.quoteid != Guid.Empty)
            {
                LoadingHelper.Show();
                int ful_type = 0;
                if (viewModel.Reservation.statuscode == 3)
                    ful_type = 100000005;
                else if (viewModel.Reservation.statuscode == 100000006 || viewModel.Reservation.statuscode == 100000004)
                    ful_type = 100000001;
                else if (viewModel.Reservation.bsd_reservationformstatus == 100000001)
                    ful_type = 100000000;
                else if (viewModel.Reservation.statuscode == 100000000)
                    ful_type = 100000000;

                var ful = new FollowUpModel
                {
                    bsd_followuplistid = Guid.NewGuid(),
                    project_id = viewModel.Reservation.project_id,
                    project_name = viewModel.Reservation.project_name,
                    bsd_group = 100000000,
                    bsd_type = ful_type,
                    bsd_name = "Termination_" + viewModel.Reservation.quotenumber + "_CCR",
                    bsd_reservation_id = viewModel.Reservation.quoteid,
                    name_reservation = viewModel.Reservation.name,
                    bsd_depositfee = viewModel.Reservation.bsd_depositfee,
                    product_id = viewModel.Reservation.unit_id,
                    bsd_units = viewModel.Reservation.unit_name,
                    bsd_sellingprice = viewModel.Reservation.totalamount,
                    bsd_totalamount = viewModel.Reservation.totalamount,
                    bsd_totalamountpaid = viewModel.Reservation.bsd_totalamountpaid,
                    bsd_date = DateTime.Now

                };
                FollowUpListForm newPage = new FollowUpListForm(ful);
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

        private async void CancelInstallment(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                if (await viewModel.DeactiveInstallment())
                {
                    NeedToRefresh = true;
                    NeedToRefreshInstallment = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_xoa_lich_thanh_toan);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.xoa_lich_thanh_toan_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void ConfirmSigning(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.bsd_quotationprinteddate.HasValue)
            {
                ToastMessageHelper.ShortMessage(Language.da_xac_nhan_in);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.InstallmentList.Count == 0)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_tao_lich_thanh_toan);
                LoadingHelper.Hide();
                return;
            }
            bool isSuccess = await viewModel.ConfirmSinging();
            if (isSuccess)
            {
                NeedToRefresh = true;
                NeedToRefreshInstallment = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                OnAppearing();
                ToastMessageHelper.ShortMessage(Language.xac_nhan_in_thanh_cong);
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.xac_nhan_in_that_bai);
            }
            LoadingHelper.Hide();
        }

        private async void ConfirmReservation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                viewModel.Reservation.bsd_reservationuploadeddate = DateTime.Now;
                if (await viewModel.UpdateQuotes(viewModel.ConfirmReservation))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_xac_nhan_tai_phieu_dat_coc);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.xac_nhan_tao_phieu_dat_coc_tat_bai_vui_long_thu_lai);
                }
            }
        }

        private void EditQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservation = new ReservationForm(viewModel.ReservationId);
            reservation.CheckReservation = async (isSuccess) =>
            {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservation);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_dang_o_trang_thai_reserve_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_co_thong_tin_bang_tinh_gia);
                }
            };
        } 

        private async void CreatePaymentScheme(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            CrmApiResponse response = await viewModel.UpdatePaymentScheme();
            if (response.IsSuccess == true)
            {
                NeedToRefreshInstallment = true;
                OnAppearing();
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.tao_lich_thanh_toan_thanh_cong);
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(response.ErrorResponse.error.message);
                //if (IsSuccess == "Localization")
                //{
                //    string asw = await App.Current.MainPage.DisplayActionSheet("Khách hàng chưa chọn quốc tịch", "Hủy", "Thêm quốc tịch");
                //    if (asw == "Thêm quốc tịch")
                //    {
                //        if (!string.IsNullOrEmpty(viewModel.Reservation.purchaser_contact_name))
                //        {
                //            await App.Current.MainPage.Navigation.PushAsync(new ContactForm(Guid.Parse(viewModel.Customer.Val)));
                //        }
                //        else
                //        {
                //            await App.Current.MainPage.Navigation.PushAsync(new AccountForm(Guid.Parse(viewModel.Customer.Val)));
                //        }
                //    }
                //    LoadingHelper.Hide();
                //}
                //else
                //{
                //    LoadingHelper.Hide();
                //    ToastMessageHelper.ShortMessage("Tạo lịch thanh toán thất bại. Vui lòng thử lại");
                //}    
            }
        }

        private async void CompletedReservation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                viewModel.Reservation.bsd_reservationformstatus = 100000002;
                viewModel.Reservation.bsd_rfsigneddate = DateTime.Now;
                if (await viewModel.UpdateQuotes(viewModel.UpdateReservation))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.phieu_dat_coc_da_duoc_ky);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.ky_phieu_dat_coc_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void SignQuotationClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.InstallmentList.Count == 0)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_tao_lich_thanh_toan);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                var res = await viewModel.SignQuotation();
                if (res.IsSuccess)
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    if (QueuesDetialPage.NeedToRefreshDC.HasValue) QueuesDetialPage.NeedToRefreshDC = true;
                    if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                    if (UnitInfo.NeedToRefreshReservation.HasValue) UnitInfo.NeedToRefreshReservation = true;
                    if (DatCocList.NeedToRefresh.HasValue) DatCocList.NeedToRefresh = true;
                    this.Title = Language.dat_coc_title;
                    InitContract(true);
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.bang_tinh_gia_da_duoc_ky);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(res.ErrorResponse.error.message);
                }
            }
        }

        private async void CancelQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string options = await DisplayActionSheet(Language.huy_bang_tinh_gia, Language.dong, Language.xac_nhan);
            if (options == Language.xac_nhan)
            {
                viewModel.Reservation.statecode = 3;
                viewModel.Reservation.statuscode = 6;
                if (await viewModel.UpdateQuotes(viewModel.UpdateQuote))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_huy_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.huy_bang_tinh_gia_that_bai_vui_long_thu_lai);
                }
            }
            LoadingHelper.Hide();
        }

        private Grid SetUpItem(string content)
        {
            Grid grid = new Grid();
            Label lb = new Label();
            lb.Text = content;
            lb.FontSize = 15;
            lb.TextColor = Color.FromHex("1399D5");
            lb.VerticalOptions = LayoutOptions.Center;
            lb.HorizontalOptions = LayoutOptions.End;
            lb.FontAttributes = FontAttributes.Bold;
            grid.Children.Add(lb);
            return grid;
        }

        private void Project_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Reservation.project_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ProjectInfo projectInfo = new ProjectInfo(viewModel.Reservation.project_id);
                projectInfo.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess == true)
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
        }

        private void SalesCompany_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Reservation.salescompany_accountid != Guid.Empty)
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(viewModel.Reservation.salescompany_accountid);
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

        private void Collaborator_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ContactDetailPage contactDetailPage = new ContactDetailPage(viewModel.Reservation.collaborator_id);
            contactDetailPage.OnCompleted = async (isSuccess) => {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contactDetailPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
            LoadingHelper.Hide();
        }

        private void CustomerReferral_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.CustomerReferral?.Title == "3")
            {
                ContactDetailPage contactDetailPage = new ContactDetailPage(Guid.Parse(viewModel.CustomerReferral.Val));
                contactDetailPage.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(contactDetailPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            else if (viewModel.CustomerReferral?.Title == "2")
            {
                AccountDetailPage accountDetailPage = new AccountDetailPage(Guid.Parse(viewModel.CustomerReferral.Val));
                accountDetailPage.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(accountDetailPage);
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

        private void Customer_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if(viewModel.Customer != null)
            {
                if(viewModel.Customer.Title == viewModel.CodeAccount)
                {
                    AccountDetailPage newPage = new AccountDetailPage(viewModel.Reservation.purchaser_accountid);
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
                else if (viewModel.Customer.Title == viewModel.CodeContact)
                {
                    ContactDetailPage newPage = new ContactDetailPage(viewModel.Reservation.purchaser_contactid);
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

        private async void CancelDeposit(object sender, EventArgs e)
        {
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                LoadingHelper.Show();
                if (await viewModel.CancelDeposit())
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (DatCocList.NeedToRefresh.HasValue) DatCocList.NeedToRefresh = true;
                    if (QueuesDetialPage.NeedToRefreshDC.HasValue) QueuesDetialPage.NeedToRefreshDC = true;
                    if (QueuesDetialPage.NeedToRefresh.HasValue) QueuesDetialPage.NeedToRefresh = true;
                    if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                    if (UnitInfo.NeedToRefreshReservation.HasValue) UnitInfo.NeedToRefreshReservation = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_huy_dat_coc);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.huy_dat_coc_that_bai_vui_long_thu_lai);
                }
            }
        }

        private void SutUpSpecialDiscount()
        {
            //if (viewModel.ListSpecialDiscount != null && viewModel.ListSpecialDiscount.Count > 0)
            //{
            //    stackLayoutSpecialDiscount.IsVisible = true;
            //    foreach (var item in viewModel.ListSpecialDiscount)
            //    {
            //        if (!string.IsNullOrEmpty(item.Label))
            //        {
            //            stackLayoutSpecialDiscount.Children.Add(SetUpItem(item.Label));
            //        }
            //    }
            //}
            //else
            //{
            //    stackLayoutSpecialDiscount.IsVisible = false;
            //}
        }

        private async void stackLayoutPromotions_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((Label)sender).GestureRecognizers[0]).CommandParameter as OptionSet;
            if (item != null && item.Val != string.Empty)
            {
                if (viewModel.PromotionItem == null)
                {
                    await viewModel.LoadPromotionItem(item.Val);
                }
                else if (viewModel.PromotionItem.bsd_promotionid.ToString() != item.Val)
                {
                    await viewModel.LoadPromotionItem(item.Val);
                }
            }
            if (viewModel.PromotionItem != null)
                KhuyenMai_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }

        private async void HandoverConditionItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.HandoverConditionItem == null && viewModel.Reservation.handovercondition_id != Guid.Empty)
            {
                await viewModel.LoadHandoverConditionItem(viewModel.Reservation.handovercondition_id);
            }
            if (viewModel.HandoverConditionItem != null)
                HandoverCondition_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }

        private async void stackLayoutSpecialDiscount_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((Label)sender).GestureRecognizers[0]).CommandParameter as DiscountSpecialModel;
            if (item != null && item.bsd_discountspecialid != Guid.Empty)
            {
                if (viewModel.DiscountSpecialItem == null)
                {
                    await viewModel.LoadDiscountSpecialItem(item.bsd_discountspecialid.ToString());
                }
                else if (viewModel.DiscountSpecialItem.bsd_discountspecialid != item.bsd_discountspecialid)
                {
                    await viewModel.LoadDiscountSpecialItem(item.bsd_discountspecialid.ToString());
                }
            }
            if (viewModel.DiscountSpecialItem != null)
                SpecialDiscount_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }

        private async void Discount_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (DiscountModel)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.bsd_discounttype == "100000000")
                Discount_CenterPopup.Title = Language.chiet_khau_chung;
            else if(item.bsd_discounttype == "100000004")
                Discount_CenterPopup.Title = Language.chiet_khau_noi_bo;
            else if(item.bsd_discounttype == "100000002")
                Discount_CenterPopup.Title = Language.phuong_thuc_thanh_toan;
            else if (item.bsd_discounttype == "100000006")
                Discount_CenterPopup.Title = Language.chiet_khau_quy_doi;
            await viewModel.LoadDiscountItem(item.bsd_discountid);
            if (viewModel.Discount != null)
                Discount_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }

        private void Unit_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var unitId = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            UnitInfo unit = new UnitInfo(unitId);
            unit.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(unit);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    TabChinhSach.IsVisible = true;
                    TabTongHop.IsVisible = false;
                    TabChiTiet.IsVisible = false;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    TabChinhSach.IsVisible = false;
                    TabTongHop.IsVisible = true;
                    TabChiTiet.IsVisible = false;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 2)
                {
                    TabChinhSach.IsVisible = false;
                    TabTongHop.IsVisible = false;
                    TabChiTiet.IsVisible = true;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 3)
                {
                    if (viewModel.InstallmentList.Count == 0)
                        LoadInstallmentList(viewModel.ReservationId);
                    TabChinhSach.IsVisible = false;
                    TabTongHop.IsVisible = false;
                    TabChiTiet.IsVisible = false;
                    TabLich.IsVisible = true;
                }
            }
        }
    }
}