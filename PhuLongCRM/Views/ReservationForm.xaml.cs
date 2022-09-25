using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;
using System;
using System.Linq;
using PhuLongCRM.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhuLongCRM.Resources;
using Xamarin.Forms.Internals;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationForm : ContentPage
    {
        public Action<int> CheckReservation;
        public ReservationFormViewModel viewModel;
        private List<string> newSelectedPromotionIds;
        private bool _isEnableCheck { get; set; }

        public ReservationForm(Guid quoteId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.QuoteId = quoteId;
            InitUpdate();
        }

        public ReservationForm(Guid productId, OptionSet queue, OptionSet saleAgentCompany, string nameOfStaffAgent, OptionSet customer)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.ProductId = productId;
            viewModel.Queue = queue;

            if (viewModel.Queue == null)
            {
                lblGiuCho.IsVisible = false;
                lookupGiuCho.IsVisible = false;
            }

            if (saleAgentCompany != null && Guid.Parse(saleAgentCompany.Val) != Guid.Empty)
            {
                viewModel.SalesAgent = saleAgentCompany;
                lookupDaiLySanGiaoDich.IsEnabled = false;
            }
            if (!string.IsNullOrWhiteSpace(nameOfStaffAgent))
            {
                viewModel.Quote.bsd_nameofstaffagent = nameOfStaffAgent;
                entryNhanVienDaiLy.IsEnabled = false;
            }
            if (customer != null)
            {
                viewModel.Buyer = customer;
                lookupNguoiMua.IsEnabled = false;
                lookupNguoiMua.HideClearButton();
            }

            Init();
        }

        public async void Init()
        {
            await viewModel.LoadUnitInfor();
            if (viewModel.UnitInfor != null)
            {
                if (viewModel.UnitInfor.statuscode == "100000006") // 100000006 :  Reserve
                {
                    CheckReservation?.Invoke(1);
                    return;
                }
                if (viewModel.Queue != null)
                {
                    lookupGiuCho.IsEnabled = false;
                    lookupGiuCho.HideClearButton();
                }
                viewModel.PaymentSchemeType = PaymentSchemeTypeData.GetPaymentSchemeTypeById("100000000");

                await Task.WhenAll(viewModel.LoadTaxCode(),viewModel.LoadPhasesLaunch());
                viewModel.IsLocked = false;
                SetPreOpen();
                // set giá trị trong tính tiền
                InitTotal();
                CheckReservation?.Invoke(0);
            }
            else
            {
                CheckReservation?.Invoke(2);
            }
        }

        public async void InitUpdate()
        {
            await viewModel.LoadQuote();
            if (viewModel.Quote != null)
            {
                this.Title = buttonSave.Text = Language.cap_nhat_bang_tinh_gia_title;
                lookupNguoiMua.IsEnabled = false;
                lookupGiuCho.IsEnabled = false;
                lookupDaiLySanGiaoDich.IsEnabled = false;
                lookUpCollaborator.IsEnabled = false;
                lookUpCustomerReferral.IsEnabled = false;
                entryNhanVienDaiLy.IsEnabled = false;
                _isEnableCheck = true;

                if (viewModel.Quote.bsd_paymentschemestype == "100000001") // Type = Gop dau
                {
                    datePickerNgayBatDauTinhLTT.IsVisible = true;
                }

                if (viewModel.Queue == null)
                { 
                    lblGiuCho.IsVisible = false;
                    lookupGiuCho.IsVisible = false;
                }

                await viewModel.CheckTaoLichThanhToan();
                Guid id = await viewModel.GetDiscountPamentSchemeListId(viewModel.PaymentScheme.bsd_paymentschemeid.ToString());
                await Task.WhenAll(
                    viewModel.LoadDiscountChilds(),
                    viewModel.LoadDiscountChildsPaymentSchemes(id.ToString()),
                    //viewModel.LoadDiscountSpecialPaymentSchemes(),
                    viewModel.LoadDiscountChildsInternel(),
                    viewModel.LoadDiscountChildsExchange(),
                    viewModel.LoadHandoverCondition(),
                    viewModel.LoadPromotionsSelected(),
                    viewModel.LoadPromotions(),
                    viewModel.LoadCoOwners()
                    ) ;

                viewModel.IsLocked = false;
                SetPreOpen();

                viewModel.PaymentSchemeType = PaymentSchemeTypeData.GetPaymentSchemeTypeById(viewModel.Quote.bsd_paymentschemestype);

                if (viewModel.IsHadLichThanhToan == true)
                {
                    lookupDieuKienBanGiao.IsEnabled = true;
                    lookupPhuongThucThanhToan.IsEnabled = true;
                    lookupChieuKhau.IsEnabled = true;
                    lookupChieuKhauNoiBo.HideClearButton();
                    lookupChieuKhauQuyDoi.HideClearButton();
                }
                if (!string.IsNullOrWhiteSpace(viewModel.Quote.bsd_discounts))
                {
                    List<string> arrDiscounts = viewModel.Quote.bsd_discounts.Split(',').ToList();
                    for (int i = 0; i < viewModel.DiscountChilds.Count; i++)
                    {
                        for (int j = 0; j < arrDiscounts.Count; j++)
                        {
                            if (viewModel.DiscountChilds[i].Val == arrDiscounts[j])
                            {
                                viewModel.DiscountChilds[i].Selected = true;
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(viewModel.Quote.bsd_selectedchietkhaupttt))
                {
                    List<string> arrCKPTTT = viewModel.Quote.bsd_selectedchietkhaupttt.Split(',').ToList();
                    for (int i = 0; i < viewModel.DiscountChildsPaymentSchemes.Count; i++)
                    {
                        for (int j = 0; j < arrCKPTTT.Count; j++)
                        {
                            if (viewModel.DiscountChildsPaymentSchemes[i].Val == arrCKPTTT[j])
                            {
                                viewModel.DiscountChildsPaymentSchemes[i].Selected = true;
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(viewModel.Quote.bsd_interneldiscount))
                {
                    List<string> arrCKNoiBo = viewModel.Quote.bsd_interneldiscount.Split(',').ToList();
                    for (int i = 0; i < viewModel.DiscountChildsInternel.Count; i++)
                    {
                        for (int j = 0; j < arrCKNoiBo.Count; j++)
                        {
                            if (viewModel.DiscountChildsInternel[i].Val == arrCKNoiBo[j])
                            {
                                viewModel.DiscountChildsInternel[i].Selected = true;
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(viewModel.Quote.bsd_exchangediscount))
                {
                    List<string> arrCKQuyDoi = viewModel.Quote.bsd_exchangediscount.Split(',').ToList();
                    for (int i = 0; i < viewModel.DiscountChildsExchanges.Count; i++)
                    {
                        for (int j = 0; j < arrCKQuyDoi.Count; j++)
                        {
                            if (viewModel.DiscountChildsExchanges[i].Val == arrCKQuyDoi[j])
                            {
                                viewModel.DiscountChildsExchanges[i].Selected = true;
                            }
                        }
                    }
                }
                this.CheckReservation?.Invoke(0);
                _isEnableCheck = false;
            }
            else
            {
                this.CheckReservation?.Invoke(2);
            }
        }

        private void SetPreOpen()
        {
            lookupDieuKienBanGiao.HideClearButton();
            lookupPhuongThucThanhToan.HideClearButton();
            lookupChieuKhau.PreOpenOneTime = false;

            if (viewModel.IsHadLichThanhToan)
            {
                lookupDieuKienBanGiao.PreOpenOneTime = false;
                lookupPhuongThucThanhToan.PreOpenOneTime = false;
                lookupChieuKhauNoiBo.PreOpenOneTime = false;
                lookupChieuKhauQuyDoi.PreOpenOneTime = false;
                lookupChieuKhau.HideClearButton();

                lookupDieuKienBanGiao.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupPhuongThucThanhToan.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupChieuKhau.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupChieuKhauNoiBo.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupChieuKhauQuyDoi.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };
            }
            else
            {
                lookupDieuKienBanGiao.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadHandoverConditions();
                    LoadingHelper.Hide();
                };

                lookupPhuongThucThanhToan.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadPaymentSchemes();
                    LoadingHelper.Hide();
                };

                lookupChieuKhau.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    if (viewModel.DiscountLists == null)
                    {
                        await viewModel.LoadDiscountList();
                    }

                    if (viewModel.DiscountLists == null) // dot mo ban khong co chieu khau
                    {
                        ToastMessageHelper.ShortMessage(Language.khong_co_chiet_khau);
                    }
                    LoadingHelper.Hide();
                };

                lookupChieuKhauQuyDoi.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadDiscountExchangeList();
                    LoadingHelper.Hide();
                };

                lookupChieuKhauNoiBo.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadDiscountInternelList();
                    LoadingHelper.Hide();
                };
            }

            lookupLoaiGopDot.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.PaymentSchemeTypes = PaymentSchemeTypeData.PaymentSchemeTypes();
                LoadingHelper.Hide();
            };

            lookupQuanHe.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.Relationships = RelationshipCoOwnerData.RelationshipData();
                LoadingHelper.Hide();
            };

            lookupGiuCho.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadQueues();
                LoadingHelper.Hide();
            };

            lookupDaiLySanGiaoDich.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgents();
                LoadingHelper.Hide();
            };

            lookUpCollaborator.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCollaboratorLookUp();
                LoadingHelper.Hide();
            };
            lookUpCustomerReferral.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCustomerReferralLookUp();
                LoadingHelper.Hide();
            };
        }

        private void ChinhSach_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "Active");
            VisualStateManager.GoToState(radBorderTongHop, "InActive");
            VisualStateManager.GoToState(radBorderChiTiet, "InActive");
            VisualStateManager.GoToState(lblChinhSach, "Active");
            VisualStateManager.GoToState(lblTongHop, "InActive");
            VisualStateManager.GoToState(lblChiTiet, "InActive");
            contentChinhSach.IsVisible = true;
            contentTongHop.IsVisible = false;
            contentChiTiet.IsVisible = false;
        }

        private void TongHop_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "InActive");
            VisualStateManager.GoToState(radBorderTongHop, "Active");
            VisualStateManager.GoToState(radBorderChiTiet, "InActive");
            VisualStateManager.GoToState(lblChinhSach, "InActive");
            VisualStateManager.GoToState(lblTongHop, "Active");
            VisualStateManager.GoToState(lblChiTiet, "InActive");
            contentChinhSach.IsVisible = false;
            contentTongHop.IsVisible = true;
            contentChiTiet.IsVisible = false;
        }

        private void ChiTiet_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "InActive");
            VisualStateManager.GoToState(radBorderTongHop, "InActive");
            VisualStateManager.GoToState(radBorderChiTiet, "Active");
            VisualStateManager.GoToState(lblChinhSach, "InActive");
            VisualStateManager.GoToState(lblTongHop, "InActive");
            VisualStateManager.GoToState(lblChiTiet, "Active");
            contentChinhSach.IsVisible = false;
            contentTongHop.IsVisible = false;
            contentChiTiet.IsVisible = true;
        }

        #region Handover Condition // Dieu kien ban giao
        private void HandoverCondition_SelectedItemChange(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            if (viewModel.HandoverCondition.bsd_byunittype == true && (viewModel.HandoverCondition._bsd_unittype_value != viewModel.UnitType))
            {
                ToastMessageHelper.ShortMessage(Language.dieu_kien_ban_giao_khong_phu_hop_voi_unit_type);
                //Điều kiện bàn giao đã chọn không phù hợp với Loại sản phẩm đang thực hiện giao dịch. Vui lòng kiểm tra lại thông tin hoặc chọn điều kiện bàn giao khác.
                viewModel.HandoverCondition = null;
                return;
            }
        }
        #endregion

        #region PaymentScheme
        private async void PTTT_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.PaymentScheme.bsd_paymentschemeid != viewModel.Quote.paymentscheme_id)
            {
                if (viewModel.Quote.paymentscheme_id != Guid.Empty)
                {
                    var ans = await DisplayAlert("", Language.ban_co_chan_chan_muon_thay_doi_pttt, Language.dong_y, Language.huy);
                    if (ans == false)
                    {
                        LoadingHelper.Hide();
                        viewModel.PaymentScheme = viewModel.paymentSheme_Temp;
                        return;
                    }
                    if (viewModel.DiscountChildsPaymentSchemes.Any(x => x.Selected))
                    {
                        var answer = await DisplayAlert("", Language.ban_dang_tich_chon_chieu_khau_theo_pttt_ban_co_chac_chan_muon_thay_doi_pttt_nay, Language.dong_y, Language.huy);
                        if (answer == false)
                        {
                            LoadingHelper.Hide();
                            viewModel.PaymentScheme = viewModel.paymentSheme_Temp;
                            return;
                        }
                        foreach (var item in viewModel.DiscountChildsPaymentSchemes)
                        {
                            if (item.Selected == true)
                            {
                                item.Selected = false;
                            }
                        }
                    }
                    CrmApiResponse apiResponse = await viewModel.UpdatePaymentShemes();
                    if (apiResponse.IsSuccess)
                    {
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        viewModel.paymentSheme_Temp = viewModel.PaymentScheme;
                        viewModel.Quote.paymentscheme_id = viewModel.PaymentScheme.bsd_paymentschemeid;
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(apiResponse.ErrorResponse.error.message);
                    }
                }
                
                viewModel.DiscountChildsPaymentSchemes.Clear();
                var id = await viewModel.GetDiscountPamentSchemeListId(viewModel.PaymentScheme.bsd_paymentschemeid.ToString());
                await viewModel.LoadDiscountChildsPaymentSchemes(id.ToString());
            }
            LoadingHelper.Hide();
        }

        private async void DiscountChildPaymentSchemeItem_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (_isEnableCheck) return;
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            if (viewModel.Quote.quoteid != Guid.Empty)
            {
                LoadingHelper.Show();
                CrmApiResponse response = await viewModel.UpdateDiscountChildsPaymentShemes();
                if (!response.IsSuccess)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(response.ErrorResponse.error.message);
                    return;
                }
                if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                LoadingHelper.Hide();
            }
        }

        private void DiscountChildPaymentSchemeItem_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.IsExpired == true || item.IsNotApplied == true) return;

            item.Selected = !item.Selected;
        }

        private void LoaiGopDot_SelectedItemChange(System.Object sender, PhuLongCRM.Models.LookUpChangeEvent e)
        {
            if (viewModel.PaymentSchemeType?.Val == "100000001") // Type = Gop dau
            {
                datePickerNgayBatDauTinhLTT.IsVisible = true;
            }
            else {
                datePickerNgayBatDauTinhLTT.IsVisible = false;
            }
        }
        #endregion

        #region Discount list // Chiet Khau
        private async void DiscountListItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();

            if (viewModel.DiscountList == null)
            {
                viewModel.DiscountChilds.Clear();
            }
            if (viewModel.DiscountChilds.Count == 0)
            {
                viewModel.DiscountChilds.Clear();
                await viewModel.LoadDiscountChilds();
            }
            LoadingHelper.Hide();
        }

        private void DiscountChildItem_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (_isEnableCheck) return;
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
        }

        private void DiscountChildItem_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.IsExpired == true || item.IsNotApplied == true) return;

            item.Selected = !item.Selected;
        }
        #endregion

        #region Discount Internel List / Chieu khau noi bo
        private async void DiscountInternelListItem_Changed(object sende, EventArgs e)
        {
            LoadingHelper.Show();

            if (viewModel.DiscountInternelList == null)
            {
                viewModel.DiscountChildsInternel.Clear();
            }
            if (viewModel.DiscountChildsInternel.Count == 0)
            {
                viewModel.DiscountChildsInternel.Clear();
                await viewModel.LoadDiscountChildsInternel();
            }
            LoadingHelper.Hide();
        }

        private void DiscountChildInternelItem_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (_isEnableCheck) return;
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
        }

        private void DiscountChildInternelItem_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.IsExpired == true || item.IsNotApplied == true) return;

            item.Selected = !item.Selected;
        }
        #endregion

        #region Promotion // Khuyen mai
        private async void Promotion_Tapped(object sender, EventArgs e)
        {
            //if (viewModel.IsHadLichThanhToan == true)
            //{
            //    ToastMessageHelper.ShortMessage("Đã có lịch thanh toán, không được chỉnh sửa");
            //    return;
            //}
            LoadingHelper.Show();
            this.newSelectedPromotionIds = new List<string>();
            if (viewModel.Promotions.Count == 0)
            {
                await viewModel.LoadPromotions();
            }
            else
            {
                foreach (var itemPromotion in viewModel.Promotions)
                {
                    if (viewModel.SelectedPromotionIds.Count != 0 && viewModel.SelectedPromotionIds.Any(x => x == itemPromotion.Val))
                    {
                        itemPromotion.Selected = true;
                    }
                    else
                    {
                        itemPromotion.Selected = false;
                    }
                }
            }
            await centerModalPromotions.Show();
            LoadingHelper.Hide();
        }

        private void PromotionItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemPromotion = (OptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            itemPromotion.Selected = !itemPromotion.Selected;
            LoadingHelper.Hide();
        }

        private async void SaveSelectedPromotion_CLicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.QuoteId == Guid.Empty)
            {
                viewModel.PromotionsSelected.Clear();
                viewModel.SelectedPromotionIds.Clear();

                foreach (var itemPromotion in viewModel.Promotions)
                {
                    if (itemPromotion.Selected)
                    {
                        viewModel.PromotionsSelected.Add(itemPromotion);
                        viewModel.SelectedPromotionIds.Add(itemPromotion.Val);
                    }
                }
            }
            else
            {
                foreach (var item in viewModel.Promotions)
                {
                    if (item.Selected == true)
                    {
                        if (viewModel.SelectedPromotionIds.Count != 0 && viewModel.SelectedPromotionIds.Any(x => x != item.Val))
                        {
                            this.newSelectedPromotionIds.Add(item.Val);
                        }
                        else
                        {
                            this.newSelectedPromotionIds.Add(item.Val);
                        }
                    }
                }

                if (this.newSelectedPromotionIds.Count != 0)
                {
                    bool IsSuccess = await viewModel.AddPromotion(this.newSelectedPromotionIds);
                    if (IsSuccess)
                    {
                        this.newSelectedPromotionIds.Clear();
                        viewModel.PromotionsSelected.Clear();

                        foreach (var itemPromotion in viewModel.Promotions)
                        {
                            if (itemPromotion.Selected)
                            {
                                viewModel.PromotionsSelected.Add(itemPromotion);
                            }
                        }
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        ToastMessageHelper.ShortMessage(Language.them_khuyen_mai_thanh_cong);
                    }
                    else
                    {
                        ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                    }
                }
            }
            await centerModalPromotions.Hide();
            LoadingHelper.Hide();
        }

        private async void UnSelect_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var conform = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_xoa_khuyen_mai_khong, Language.dong_y, Language.huy);
            if (conform == false)
            {
                LoadingHelper.Hide();
                return;
            }
            var item = (OptionSet)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (viewModel.QuoteId != Guid.Empty)
            {
                CrmApiResponse apiResponse = await viewModel.DeletePromotion(item.Val);
                if (apiResponse.IsSuccess)
                {
                    if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                    viewModel.PromotionsSelected.Remove(item);
                    viewModel.SelectedPromotionIds.Remove(item.Val);
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.LongMessage(apiResponse.ErrorResponse.error.message);
                }
            }
            else
            {
                viewModel.PromotionsSelected.Remove(item);
                viewModel.SelectedPromotionIds.Remove(item.Val);
            }
            LoadingHelper.Hide();
        }

        private void SearchPromotion_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var data = viewModel.Promotions.Where(x => x.Label.ToLower().Contains(viewModel.KeywordPromotion.ToLower())).ToList();
            viewModel.Promotions.Clear();
            foreach (var item in data)
            {
                viewModel.Promotions.Add(item);
            }
            LoadingHelper.Hide();
        }

        private async void SearchPromotion_TexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.KeywordPromotion))
            {
                LoadingHelper.Show();
                viewModel.Promotions.Clear();
                await viewModel.LoadPromotions();
                LoadingHelper.Hide();
            }
        }
        #endregion

        #region CK quy doi
        private async void DiscountListExchangeItem_Changed(object sende, EventArgs e)
        {
            LoadingHelper.Show();

            if (viewModel.DiscountExchangeList == null)
            {
                viewModel.DiscountChildsExchanges.Clear();
            }
            if (viewModel.DiscountChildsExchanges.Count == 0)
            {
                viewModel.DiscountChildsExchanges.Clear();
                await viewModel.LoadDiscountChildsExchange();
            }
            LoadingHelper.Hide();
        }

        private void DiscountChildExchangeItem_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (_isEnableCheck) return;
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
        }

        private void DiscountChildExchangeItem_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.IsExpired == true || item.IsNotApplied == true) return;

            item.Selected = !item.Selected;
        }
        #endregion

        #region Co Owner // Dong so huu
        private async void CoOwner_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.CoOwner = new CoOwnerFormModel();
            viewModel.TitleCoOwner = null;
            viewModel.CustomerCoOwner = null;
            viewModel.Relationship = null;
            await centerModalCoOwner.Show();
            LoadingHelper.Hide();
        }

        private async void UnSelectCoOwner_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CoOwnerFormModel)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;

            if (viewModel.QuoteId != Guid.Empty)
            {
                var conform = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_xoa_nguoi_dong_so_huu_nay_khong, Language.dong_y, Language.huy);
                if (conform == false)
                {
                    LoadingHelper.Hide();
                    return;
                }
                var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_coowners({item.bsd_coownerid})");
                if (deleteResponse.IsSuccess)
                {
                    viewModel.CoOwnerList.Remove(item);
                    ToastMessageHelper.ShortMessage(Language.xoa_nguou_dong_so_huu_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                    return;
                }
            }
            else
            {
                viewModel.CoOwnerList.Remove(item);
            }
            LoadingHelper.Hide();
        }

        private async void UpdateCoOwner_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            centerModalCoOwner.Title = Language.cap_nhat_dong_so_huu;
            var item = (CoOwnerFormModel)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.CoOwner = new CoOwnerFormModel();
            viewModel.TitleCoOwner = null;
            viewModel.CustomerCoOwner = null;
            viewModel.Relationship = null;
            viewModel.CoOwner = item;
            if (viewModel.CoOwner.contact_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.contact_id.ToString(), viewModel.CoOwner.contact_name) { Title = "2" };
            }

            if (viewModel.CoOwner.account_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.account_id.ToString(), viewModel.CoOwner.account_name) { Title = "3" };
            }

            viewModel.TitleCoOwner = viewModel.CoOwner.bsd_name;
            viewModel.Relationship = new OptionSet(viewModel.CoOwner.bsd_relationshipId.ToString(), viewModel.CoOwner.bsd_relationship);
            await centerModalCoOwner.Show();
            LoadingHelper.Hide();
        }

        private async void SaveCoOwner_CLicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.TitleCoOwner))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }

            if (viewModel.CustomerCoOwner == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                return;
            }

            if (viewModel.Relationship == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_quan_he);
                return;
            }

            if (viewModel.CustomerCoOwner?.Val == viewModel.Buyer?.Val)
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_duoc_trung_voi_nguoi_mua);
                viewModel.CustomerCoOwner = null;
                return;
            }

            if (viewModel.CoOwner.bsd_coownerid == Guid.Empty && viewModel.CoOwnerList.Any(x => x.contact_id == Guid.Parse(viewModel.CustomerCoOwner?.Val) || x.account_id == Guid.Parse(viewModel.CustomerCoOwner?.Val)))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_da_duoc_chon);
                return;
            }

            LoadingHelper.Show();
            if (viewModel.CustomerCoOwner.Title == "2")
            {
                viewModel.CoOwner.contact_id = Guid.Parse(viewModel.CustomerCoOwner.Val);
                viewModel.CoOwner.contact_name = viewModel.CustomerCoOwner.Label;
            }

            if (viewModel.CustomerCoOwner.Title == "3")
            {
                viewModel.CoOwner.account_id = Guid.Parse(viewModel.CustomerCoOwner.Val);
                viewModel.CoOwner.account_name = viewModel.CustomerCoOwner.Label;
            }
            viewModel.CoOwner.bsd_name = viewModel.TitleCoOwner;
            viewModel.CoOwner.bsd_relationshipId = viewModel.Relationship.Val;
            viewModel.CoOwner.bsd_relationship = viewModel.Relationship.Label;

            if (viewModel.CoOwner.bsd_coownerid == Guid.Empty)
            {
                viewModel.CoOwner.bsd_coownerid = Guid.NewGuid();

                viewModel.CoOwnerList.Add(viewModel.CoOwner);
                if (viewModel.QuoteId != Guid.Empty)
                {
                    bool IsSuccess = await viewModel.AddCoOwer();
                    if (IsSuccess)
                    {
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        ToastMessageHelper.ShortMessage(Language.them_dong_so_huu_thanh_cong);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                    }
                }
                await centerModalCoOwner.Hide();
                LoadingHelper.Hide();
            }
            else
            {
                if (viewModel.QuoteId == Guid.Empty)
                {
                    List<CoOwnerFormModel> coOwnerList = new List<CoOwnerFormModel>();
                    foreach (var item in viewModel.CoOwnerList)
                    {
                        if (viewModel.CoOwner.bsd_coownerid == item.bsd_coownerid)
                        {
                            item.bsd_name = viewModel.CoOwner.bsd_name;
                            item.contact_id = viewModel.CoOwner.contact_id;
                            item.contact_name = viewModel.CoOwner.contact_name;
                            item.account_id = viewModel.CoOwner.account_id;
                            item.account_name = viewModel.CoOwner.account_name;
                            item.bsd_relationshipId = viewModel.CoOwner.bsd_relationshipId;
                            item.bsd_relationship = viewModel.CoOwner.bsd_relationship;
                        }
                        coOwnerList.Add(item);
                    }
                    viewModel.CoOwnerList.Clear();
                    coOwnerList.ForEach(x => viewModel.CoOwnerList.Add(x));
                    await centerModalCoOwner.Hide();
                    LoadingHelper.Hide();
                }
                else
                {
                    CrmApiResponse response = await viewModel.UpdateCoOwner();
                    if (response.IsSuccess)
                    {
                        viewModel.CoOwnerList.Clear();
                        await viewModel.LoadCoOwners();
                        await centerModalCoOwner.Hide();
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        ToastMessageHelper.ShortMessage(Language.cap_nhat_dong_so_huu_thanh_cong);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage(response.ErrorResponse.error.message);
                    }
                }
            }
        }
        #endregion

        private void Buyer_SelectedItemChange(System.Object sender, PhuLongCRM.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent != null && (viewModel.SalesAgent.Val == viewModel.Buyer?.Val))
            {
                ToastMessageHelper.LongMessage(Language.nguoi_mua_khong_duoc_trung_voi_dai_ly_san_giao_dich_vui_long_chon_lai);
                viewModel.Buyer = null;
            }
            if (viewModel.CoOwnerList.Any(x => x.contact_id == Guid.Parse(viewModel.Buyer?.Val)) || viewModel.CoOwnerList.Any(x => x.account_id == Guid.Parse(viewModel.Buyer?.Val)))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_coower_va_khach_hang_khong_duoc_trung);
                viewModel.Buyer = null;
            }
            LoadingHelper.Hide();
        }

        private void SalesAgent_SelectedItemChange(System.Object sender, PhuLongCRM.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent != null && viewModel.Buyer != null && viewModel.SalesAgent == viewModel.Buyer)
            {
                ToastMessageHelper.LongMessage(Language.dai_ly_san_giao_dich_khong_duoc_trung_voi_nguoi_mua_vui_long_chon_lai);
                viewModel.SalesAgent = null;
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.SalesAgent != null && !string.IsNullOrWhiteSpace(viewModel.SalesAgent?.Val))
            {
                viewModel.Collaborator = null;
                viewModel.CustomerReferral = null;
            }
            LoadingHelper.Hide();
        }

        private void lookUpCollaborator_SelectedItemChange(System.Object sender, PhuLongCRM.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.Collaborator != null && viewModel.Buyer != null && viewModel.Collaborator?.Id.ToString() == viewModel.Buyer?.Val)
            {
                ToastMessageHelper.LongMessage(Language.cong_tac_vien_khong_duoc_trung_voi_nguoi_mua_vui_long_chon_lai);
                viewModel.Collaborator = null;
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Collaborator != null && viewModel.Collaborator?.Id != Guid.Empty)
            {
                viewModel.SalesAgent = null;
                viewModel.CustomerReferral = null;
            }
            LoadingHelper.Hide();
        }

        private void lookUpCustomerReferral_SelectedItemChange(System.Object sender, PhuLongCRM.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.CustomerReferral != null && viewModel.Buyer != null &&  viewModel.CustomerReferral?.Id.ToString() == viewModel.Buyer?.Val)
            {
                ToastMessageHelper.LongMessage(Language.khach_hang_gioi_thieu_khong_duoc_trung_voi_nguoi_mua_vui_long_chon_lai);
                viewModel.CustomerReferral = null;
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.CustomerReferral != null && viewModel.CustomerReferral?.Id != Guid.Empty)
            {
                viewModel.SalesAgent = null;
                viewModel.Collaborator = null;
            }
            LoadingHelper.Hide();
        }

        private async void SaveQuote_Clicked(object sender, EventArgs e)
        {
            if (viewModel.HandoverCondition == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dieu_kien_ban_giao);
                return;
            }
            if (viewModel.PaymentScheme == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_phuong_thuc_thanh_toan);
                return;
            }
            if (viewModel.PaymentSchemeType?.Val == "100000001" && viewModel.Quote.bsd_startingdatecalculateofps == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_bat_dau_tinh_ltt);
                return;
            }
            if (viewModel.Buyer == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Quote.name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }

            LoadingHelper.Show();

            if(this.Title == Language.tao_bang_tinh_gia_title) //(viewModel.QuoteId == Guid.Empty)
            {
                CrmApiResponse response = await viewModel.CreateQuote();
                if (response.IsSuccess)
                {
                    await Task.WhenAll(
                            viewModel.AddCoOwer(),
                            viewModel.AddPromotion(viewModel.SelectedPromotionIds),
                            viewModel.AddHandoverCondition()
                            );
                    viewModel.QuoteId = viewModel.Quote.quoteid;
                    CrmApiResponse responseQuoteProduct = await viewModel.CreateQuoteProduct();
                    if (responseQuoteProduct.IsSuccess)
                    {
                        CrmApiResponse responseGetTotal = await viewModel.GetTotal(viewModel.Quote.quoteid.ToString());
                        if (responseGetTotal.IsSuccess)
                        {
                            SetTotals(responseGetTotal);

                            CrmApiResponse apiResponse = await viewModel.UpdateQuote();
                            if (apiResponse.IsSuccess)
                            {
                                CrmApiResponse apiResponseQuoteProduct = await viewModel.UpdateQuoteProduct();
                                if (apiResponseQuoteProduct.IsSuccess == false)
                                {
                                    ToastMessageHelper.LongMessage(apiResponseQuoteProduct.ErrorResponse.error.message);
                                    LoadingHelper.Hide();
                                    return;
                                }
                                if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                                if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                                if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;

                                this.Title = buttonSave.Text = Language.cap_nhat_bang_tinh_gia_title;

                                viewModel.Quote.paymentscheme_id = viewModel.PaymentScheme.bsd_paymentschemeid;
                                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                ToastMessageHelper.LongMessage(apiResponse.ErrorResponse.error.message);
                                LoadingHelper.Hide();
                            }
                        }
                        else
                        {
                            ToastMessageHelper.LongMessage(responseGetTotal.ErrorResponse.error.message);
                            await viewModel.DeleteQuote();
                            // set lại id = null khi thất bại để chạy vào create
                            viewModel.quotedetailid = Guid.Empty;
                            LoadingHelper.Hide();
                        }

                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage(responseQuoteProduct.ErrorResponse.error.message);
                    }
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.LongMessage(response.ErrorResponse.error.message);
                }
            }
            else
            {
                if (viewModel.IsHadLichThanhToan)
                {
                    CrmApiResponse apiResponse = await viewModel.UpdateQuote_HasLichThanhToan();
                    if (apiResponse.IsSuccess)
                    {
                        if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                        if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                        this.Title = buttonSave.Text = Language.cap_nhat_bang_tinh_gia_title;
                        ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        ToastMessageHelper.LongMessage(apiResponse.ErrorResponse.error.message);
                        LoadingHelper.Hide();
                    }
                }
                else
                {
                    CrmApiResponse responseGetTotal = await viewModel.GetTotal(viewModel.Quote.quoteid.ToString());
                    if (responseGetTotal.IsSuccess)
                    {
                        SetTotals(responseGetTotal);

                        if (viewModel.HandoverCondition_Update != null && (viewModel.HandoverCondition_Update?.Val != viewModel.HandoverCondition.Val))
                        {
                            var response = await CrmHelper.DeleteRecord($"/quotes({viewModel.QuoteId})/bsd_quote_bsd_packageselling({viewModel.HandoverCondition_Update.Val})/$ref");
                            if (response.IsSuccess)
                            {
                                bool isSuccess_Update = await viewModel.AddHandoverCondition();
                                if (isSuccess_Update)
                                {
                                    viewModel.HandoverCondition_Update = viewModel.HandoverCondition;
                                }
                                else
                                {
                                    LoadingHelper.Hide();
                                    ToastMessageHelper.ShortMessage(Language.cap_nhat_that_bai);
                                    return;
                                }
                            }
                            else
                            {
                                viewModel.HandoverCondition = viewModel.HandoverCondition_Update;
                            }
                        }

                        CrmApiResponse apiResponse = await viewModel.UpdateQuote();
                        if (apiResponse.IsSuccess)
                        {
                            CrmApiResponse apiResponseQuoteProduct = await viewModel.UpdateQuoteProduct();
                            if (apiResponseQuoteProduct.IsSuccess == false)
                            {
                                ToastMessageHelper.LongMessage(apiResponseQuoteProduct.ErrorResponse.error.message);
                                LoadingHelper.Hide();
                                return;
                            }
                            if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                            if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                            if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                            if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                            this.Title = buttonSave.Text = Language.cap_nhat_bang_tinh_gia_title;
                            ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            ToastMessageHelper.LongMessage(apiResponse.ErrorResponse.error.message);
                            LoadingHelper.Hide();
                        }
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage(responseGetTotal.ErrorResponse.error.message);
                    }
                }
                
            }
        }

        private void SetTotals(CrmApiResponse data)
        {
            viewModel.TotalReservation = JsonConvert.DeserializeObject<TotalReservationModel>(data.Content);
            viewModel.TotalReservation.ListedPrice_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.ListedPrice));
            viewModel.TotalReservation.Discount_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.Discount));
            viewModel.TotalReservation.HandoverAmount_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.HandoverAmount));
            viewModel.TotalReservation.NetSellingPrice_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.NetSellingPrice));
            viewModel.TotalReservation.LandValue_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.LandValue));
            viewModel.TotalReservation.TotalTax_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.TotalTax));
            viewModel.TotalReservation.MaintenanceFee_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.MaintenanceFee));
            viewModel.TotalReservation.NetSellingPriceAfterVAT_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.NetSellingPriceAfterVAT));
            viewModel.TotalReservation.TotalAmount_format = StringFormatHelper.FormatCurrency(Math.Round(viewModel.TotalReservation.TotalAmount));
        }

        private void InitTotal()
        {
            if (viewModel.TotalReservation == null)
                viewModel.TotalReservation = new TotalReservationModel();

            viewModel.TotalReservation.ListedPrice_format = StringFormatHelper.FormatCurrency(viewModel.UnitInfor.price);
            viewModel.TotalReservation.Discount = 0;
            viewModel.TotalReservation.Discount_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.Discount);
            viewModel.TotalReservation.HandoverAmount = 0;
            viewModel.TotalReservation.HandoverAmount_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.HandoverAmount);
            viewModel.TotalReservation.NetSellingPrice = viewModel.UnitInfor.price + viewModel.TotalReservation.HandoverAmount - viewModel.TotalReservation.Discount;
            viewModel.TotalReservation.NetSellingPrice_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.NetSellingPrice);
            viewModel.TotalReservation.LandValue = viewModel.UnitInfor.bsd_landvalueofunit * viewModel.UnitInfor.bsd_netsaleablearea;
            viewModel.TotalReservation.LandValue_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.LandValue);
            viewModel.TotalReservation.TotalTax = Math.Round(((viewModel.TotalReservation.NetSellingPrice - viewModel.TotalReservation.LandValue) * 10) / 100);
            viewModel.TotalReservation.TotalTax_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.TotalTax);
            viewModel.TotalReservation.MaintenanceFee = Math.Round((viewModel.TotalReservation.NetSellingPrice * viewModel.Quote.maintenancefreespercent) / 100);
            viewModel.TotalReservation.MaintenanceFee_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.MaintenanceFee);
            viewModel.TotalReservation.NetSellingPriceAfterVAT = viewModel.TotalReservation.NetSellingPrice + viewModel.TotalReservation.TotalTax;
            viewModel.TotalReservation.NetSellingPriceAfterVAT_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.NetSellingPriceAfterVAT);
            viewModel.TotalReservation.TotalAmount = viewModel.TotalReservation.NetSellingPriceAfterVAT + viewModel.TotalReservation.MaintenanceFee;
            viewModel.TotalReservation.TotalAmount_format = StringFormatHelper.FormatCurrency(viewModel.TotalReservation.TotalAmount);

        }
    }
}