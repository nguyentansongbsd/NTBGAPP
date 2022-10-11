using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptanceDetailPage : ContentPage
    {
        private AcceptanceDetailPageViewModel viewModel;
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh = null;

        public AcceptanceDetailPage(Guid id)
        {
            InitializeComponent();
            BindingContext = viewModel = new AcceptanceDetailPageViewModel();
            NeedToRefresh = false;
            Init(id);
        }
        public async void Init(Guid id)
        {
            await viewModel.LoadAcceptance(id);
            if (viewModel.Acceptance != null && viewModel.Acceptance.bsd_acceptanceid != Guid.Empty)
            {
                SetButtonFloatingButton();
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }
        protected async override void OnAppearing()
        {
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadAcceptance(viewModel.Acceptance.bsd_acceptanceid);
                SetButtonFloatingButton();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
            base.OnAppearing();
        }
        private void SetButtonFloatingButton()
        {
            if (viewModel.Acceptance.bsd_acceptanceid != Guid.Empty)
            {
                if (viewModel.ButtonCommandList.Count > 0)
                    viewModel.ButtonCommandList.Clear();

                if(viewModel.Acceptance.statuscode == "100000000")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_thong_tin, "FontAwesomeSolid", "\uf46c", null, ConfirmInformation));

                if (viewModel.Acceptance.statuscode == "1" || viewModel.Acceptance.statuscode == "100000001" || viewModel.Acceptance.statuscode == "100000000")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_nghiem_thu, "FontAwesomeSolid", "\uf05e", null, Cancel));

                if (viewModel.Acceptance.statuscode == "100000001" && viewModel.Acceptance.bsd_printedate != null && viewModel.Acceptance.printer_name != null)
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.dong_nghiem_thu, "FontAwesomeSolid", "\uf011", null, Close));

                if (viewModel.Acceptance.statuscode == "1")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, Update));

                if (viewModel.ButtonCommandList.Count == 0)
                    floatingButtonGroup.IsVisible = false;
                else
                    floatingButtonGroup.IsVisible = true;

            }
        }

        private async void Close(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var result = await viewModel.CloseInformation();
            if (result.IsSuccess)
            {
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                NeedToRefresh = true;
                OnAppearing();
                if (AcceptanceList.NeedToRefresh.HasValue) AcceptanceList.NeedToRefresh = true;
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(Language.thong_bao_that_bai);
            }
        }

        private void Cancel(object sender, EventArgs e)
        {
            PopupCancel.ShowCenterPopup();
        }

        private async void ConfirmInformation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var result = await viewModel.ConfirmInformation();
            if (result.IsSuccess)
            {
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                NeedToRefresh = true;
                OnAppearing();
                if (AcceptanceList.NeedToRefresh.HasValue) AcceptanceList.NeedToRefresh = true;
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(result.ErrorResponse.error.message);
            }
        }

        private void Update(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            AcceptanceForm newPage = new AcceptanceForm(viewModel.Acceptance.bsd_acceptanceid);
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

        private void ClosePopupCancel_Clicked(object sender, EventArgs e)
        {
            PopupCancel.CloseContent();
        }

        private async void ConfirmCancel_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(viewModel.Acceptance.bsd_cancelledreason))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ly_do_huy);
                return;
            }
            LoadingHelper.Show();
            var result = await viewModel.CancelAcceptance();
            if (result.IsSuccess)
            {
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                PopupCancel.CloseContent();
                NeedToRefresh = true;
                OnAppearing();
                if (AcceptanceList.NeedToRefresh.HasValue) AcceptanceList.NeedToRefresh = true;
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(result.ErrorResponse.error.message);
            }
        }

        private void Unit_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Acceptance.unit_id);
            unitInfo.OnCompleted = async (IsSuccess) => {
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

        private void Project_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ProjectInfo projectInfo = new ProjectInfo(viewModel.Acceptance.project_id);
            projectInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess == true)
                {
                    await Navigation.PushAsync(projectInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    LoadingHelper.Hide();
                }
            };
        }

        private void Contract_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ContractDetailPage newPage = new ContractDetailPage(viewModel.Acceptance.contract_id);
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

        private void Customer_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Acceptance.contact_id != Guid.Empty)
            {
                ContactDetailPage newPage = new ContactDetailPage(viewModel.Acceptance.contact_id);
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
            else if (viewModel.Acceptance.account_id != Guid.Empty)
            {
                AccountDetailPage newPage = new AccountDetailPage(viewModel.Acceptance.account_id);
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

        private async void TabControl_IndexTab(object sender, LookUpChangeEvent e)
        {
            if (e.Item != null)
            {
                if ((int)e.Item == 0)
                {
                    stackThongTin.IsVisible = true;
                    stackNghiemThu.IsVisible = false;
                }
                else if ((int)e.Item == 1)
                {
                    LoadingHelper.Show();
                    if (viewModel.Acceptance != null && viewModel.Acceptance.bsd_acceptanceid != Guid.Empty && viewModel.UnitSpecificationDetails == null)
                        await viewModel.LoadUnitSpecificationDetails();
                    stackThongTin.IsVisible = false;
                    stackNghiemThu.IsVisible = true;
                    LoadingHelper.Hide();
                }
            }
        }
        private async void ShowMore_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.Page++;
            await viewModel.LoadUnitSpecificationDetails();
            LoadingHelper.Hide();
        }

        private async void SaveUnitSpecificationDetail_Clicked(object sender, EventArgs e)
        {
            Guid id = (Guid)(sender as Button).CommandParameter;
            if (id != Guid.Empty)
            {
                foreach (var item in viewModel.UnitSpecificationDetails)
                {
                  var unitspec = item.Where(x => x.bsd_unitsspecificationdetailsid == id).FirstOrDefault();
                    if(unitspec != null)
                    {
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        LoadingHelper.Show();
                        var result = await viewModel.UpdateUnitSpecificationDetail(unitspec);
                        if (result.IsSuccess)
                            ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                        else
                            ToastMessageHelper.LongMessage(result.ErrorResponse.error.message);
                        LoadingHelper.Hide();
                    }    
                }    
            }    
        }

        private void ClosePopupUnitSpecDetail_Clicked(object sender, EventArgs e)
        {
            PopupUnitSpecDetail.CloseContent();
        }

        private async void SavePopupUnitSpecDetail_Clicked(object sender, EventArgs e)
        {
            if (viewModel.UnitSpecificationDetail != null)
            {
                LoadingHelper.Show();
                var result = await viewModel.UpdateUnitSpecificationDetail(viewModel.UnitSpecificationDetail);
                if (result.IsSuccess)
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                else
                    ToastMessageHelper.LongMessage(result.ErrorResponse.error.message);
                LoadingHelper.Hide();
            }
        }
        private async void UnitSpecificationDetail_Tapped(object sender, EventArgs e)
        {
            Guid id = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (id != Guid.Empty)
            {
                LoadingHelper.Show();
                await viewModel.LoadUnitSpecificationDetail(id);
                PopupUnitSpecDetail.ShowCenterPopup();
                LoadingHelper.Hide();
            }
        }
    }
}