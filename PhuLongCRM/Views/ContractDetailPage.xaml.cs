using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContractDetailPage : ContentPage
    {
        private Guid ContractId;
        public Action<bool> OnCompleted;
        private ContractDetailPageViewModel viewModel;
        public ContractDetailPage(Guid id)
        {
            InitializeComponent();
            ContractId = id;
            BindingContext = viewModel = new ContractDetailPageViewModel();
            Init();
        }

        public async void Init()
        {
            await Task.WhenAll(
                    viewModel.LoadContract(ContractId),
                    viewModel.LoadPromotions(this.ContractId),
                    viewModel.LoadSpecialDiscount(this.ContractId),
                    viewModel.LoadCoOwners(ContractId),
                    viewModel.LoadDiscountsPaymentScheme());
            await Task.WhenAll(
                     viewModel.LoadHandoverCondition(this.ContractId),
                     viewModel.LoadDiscounts(),
                     viewModel.LoadDiscountsInternel(),
                     viewModel.LoadDiscountsExChange());
            if (viewModel.Contract.salesorderid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
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

        //tab chính sách
        private async Task LoadChinhSach()
        {
            if (viewModel.Contract.handovercondition_id == Guid.Empty)
            {
                LoadingHelper.Show();
                await Task.WhenAll(
                    viewModel.LoadHandoverCondition(this.ContractId),
                    viewModel.LoadDiscounts(),
                    viewModel.LoadPromotions(this.ContractId),
                    viewModel.LoadSpecialDiscount(this.ContractId),
                    viewModel.LoadCoOwners(ContractId));
                SetUpDiscount(viewModel.Contract.bsd_discounts);
                SutUpPromotions();
                SutUpSpecialDiscount();
                LoadingHelper.Hide();
            }
        }

        private void SalesCompany_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Contract.salesagentcompany_id != Guid.Empty)
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(viewModel.Contract.salesagentcompany_id);
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

        private async void SetUpDiscount(string ids)
        {
            //if (!string.IsNullOrEmpty(ids))
            //{
            //    scrolltDiscount.IsVisible = true;
            //    if (viewModel.Contract.discountlist_id != Guid.Empty)
            //    {
            //        await viewModel.LoadDiscounts();

            //        var list_id = ids.Split(',');

            //        foreach (var id in list_id)
            //        {
            //            OptionSet item = viewModel.ListDiscount.Single(x => x.Val == id);
            //            if (item != null && !string.IsNullOrEmpty(item.Val))
            //            {
            //                stackLayoutDiscount.Children.Add(SetUpItemBorder(item.Label));
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    scrolltDiscount.IsVisible = false;
            //}
        }

        private async void SutUpSpecialDiscount()
        {
            //if (viewModel.Contract.salesorderid != Guid.Empty)
            //{
            //    await viewModel.LoadSpecialDiscount(this.ContractId);
            //    if (viewModel.ListSpecialDiscount != null && viewModel.ListSpecialDiscount.Count > 0)
            //    {
            //        stackLayoutSpecialDiscount.IsVisible = true;
            //        foreach (var item in viewModel.ListSpecialDiscount)
            //        {
            //            if (!string.IsNullOrEmpty(item.Label))
            //            {
            //                stackLayoutSpecialDiscount.Children.Add(SetUpItem(item.Label));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        stackLayoutSpecialDiscount.IsVisible = false;
            //    }
            //}
        }

        private async void SutUpPromotions()
        {
            //if (viewModel.Contract.salesorderid != Guid.Empty)
            //{
            //    await viewModel.LoadPromotions(this.ContractId);
            //    if (viewModel.ListPromotion != null && viewModel.ListPromotion.Count > 0)
            //    {
            //        stackLayoutPromotions.IsVisible = true;
            //        foreach (var item in viewModel.ListPromotion)
            //        {
            //            if (!string.IsNullOrEmpty(item.Label))
            //            {
            //                stackLayoutPromotions.Children.Add(SetUpItem(item.Label));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        stackLayoutPromotions.IsVisible = false;
            //    }
            //}
        }

        private RadBorder SetUpItemBorder(string content)
        {
            RadBorder rd = new RadBorder();
            rd.Padding = 5;
            rd.BorderColor = Color.FromHex("f1f1f1");
            rd.BorderThickness = 1;
            rd.CornerRadius = 5;
            Label lb = new Label();
            lb.Text = content;
            lb.FontSize = 15;
            lb.TextColor = Color.FromHex("1399D5");
            lb.VerticalOptions = LayoutOptions.Center;
            lb.HorizontalOptions = LayoutOptions.Center;
            lb.FontAttributes = FontAttributes.Bold;
            rd.Content = lb;
            return rd;
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

        private void UnitDetail_Tapped(object sender, EventArgs e)
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
                Promotion_CenterPopup.ShowCenterPopup();
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
            else if (item.bsd_discounttype == "100000004")
                Discount_CenterPopup.Title = Language.chiet_khau_noi_bo;
            else if (item.bsd_discounttype == "100000002")
                Discount_CenterPopup.Title = Language.phuong_thuc_thanh_toan;
            else if (item.bsd_discounttype == "100000006")
                Discount_CenterPopup.Title = Language.chiet_khau_quy_doi;

            await viewModel.LoadDiscountItem(item.bsd_discountid);
            if (viewModel.Discount != null)
            {
                Discount_CenterPopup.ShowCenterPopup();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_chiet_khau);
            }
        }

        private async void HandoverConditionItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.HandoverConditionItem == null && viewModel.Contract.handovercondition_id != Guid.Empty)
            {
                await viewModel.LoadHandoverConditionItem(viewModel.Contract.handovercondition_id);
            }
            if (viewModel.HandoverConditionItem != null)
                HandoverCondition_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }

        private void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    TabChinhSach.IsVisible = true;
                    TabChiTiet.IsVisible = false;
                    TabTongHop.IsVisible = false;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    TabChinhSach.IsVisible = false;
                    TabChiTiet.IsVisible = true;
                    TabTongHop.IsVisible = false;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 2)
                {
                    TabChinhSach.IsVisible = false;
                    TabChiTiet.IsVisible = false;
                    TabTongHop.IsVisible = true;
                    TabLich.IsVisible = false;
                }
                else if ((int)e.Item == 3)
                {
                    TabChinhSach.IsVisible = false;
                    TabChiTiet.IsVisible = false;
                    TabTongHop.IsVisible = false;
                    TabLich.IsVisible = true;
                    LoadInstallmentList(this.ContractId);
                }
            }
        }

        private void GoToProject_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ProjectInfo project = new ProjectInfo(viewModel.Contract.project_id);
            project.OnCompleted = async (isSuccess) => {
                if (isSuccess)
                {
                    await Navigation.PushAsync(project);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void GoToDatDoc_Tapped(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //BangTinhGiaDetailPage datcoc = new BangTinhGiaDetailPage(viewModel.Contract.queue_id,true);
            //datcoc.OnCompleted = async (isSuccess) => {
            //    if (isSuccess)
            //    {
            //        await Navigation.PushAsync(datcoc);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            //    }
            //};
        }

        private async void Interest_Tapped(object sender, EventArgs e)
        {
            var item = (ReservationInstallmentDetailPageModel)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.bsd_interestchargeamount == 0 && item.overdue != Language.phat_cham_tt) return;
            LoadingHelper.Show();
            await viewModel.LoadInstallmentById(item.bsd_paymentschemedetailid);
            Interest_CenterPopup.ShowCenterPopup();
            LoadingHelper.Hide();
        }
    }
}