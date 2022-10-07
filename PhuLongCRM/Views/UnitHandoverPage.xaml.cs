using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class UnitHandoverPage : ContentPage
    {
        public static bool? NeedRefresh { get; set; } = null;
        public Action<bool> OnCompleted { get; set; }
        public UnitHandoverPageViewModel viewModel;

        public UnitHandoverPage(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitHandoverPageViewModel();
            viewModel.UnitHandoverId = id;
            NeedRefresh = false;
            Init();
        }

        public async void Init()
        {
            try
            {
                await viewModel.LoadUnitHandover();
                if (viewModel.UnitHandover != null)
                {
                    SetButtonFloatingButton();
                    OnCompleted?.Invoke(true);
                }
                else
                {
                    OnCompleted?.Invoke(false);
                }
            }
            catch(Exception ex)
            {
                OnCompleted?.Invoke(false);
            }
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedRefresh.HasValue && NeedRefresh.Value == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadUnitHandover();
                LoadingHelper.Hide();
            }
        }

        private void SetButtonFloatingButton()
        {
            if (viewModel.UnitHandover.statuscode == "1") // sts = active
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy, "FontAwesomeSolid", "\uf00d", null, CancelUnitHandover));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.chinh_sua, "FontAwesomeRegular", "\uf044", null, UpdateUnitHandover));
                if (viewModel.UnitHandover.bsd_handoverformprintdate.HasValue) // co ngay in
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_ban_giao, "FontAwesomeRegular", "\uf044", null, ConfirmHandover));
                }
            }

            if (viewModel.UnitHandover.statuscode != "1")
            {
                floatingButtonGroup.IsVisible = false;
            }
        }

        private void CancelUnitHandover(object sender, EventArgs e)
        {
            centerCancel.ShowCenterPopup();
        }

        private async void ConfirmCancel_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.UnitHandover.bsd_cancelledreason))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ly_do_huy);
                return;
            }

            LoadingHelper.Show();
            CrmApiResponse result = await viewModel.CancelHandover();
            if (result.IsSuccess)
            {
                await viewModel.LoadUnitHandover();
                if (UnitHandovers.NeedRefresh.HasValue) UnitHandovers.NeedRefresh = true;
                ToastMessageHelper.ShortMessage(Language.thanh_cong);
                centerCancel.CloseContent_Tapped(this,EventArgs.Empty);
                SetButtonFloatingButton();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(result.ErrorResponse.error.message);
            }
        }

        private void UpdateUnitHandover(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitHandoverForm handoverForm = new UnitHandoverForm(viewModel.UnitHandoverId);
            handoverForm.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
                {
                    await Navigation.PushAsync(handoverForm);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void ConfirmHandover(object sender, EventArgs e)
        {
            centerConfirm.ShowCenterPopup();
        }

        private async void ConfirmHandover_Clicked(object sender, EventArgs e)
        {
            if (!viewModel.ConfirmDate.HasValue)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_xac_nhan);
                return;
            }

            LoadingHelper.Show();
            CrmApiResponse result = await viewModel.ConfirmHandover();
            if (result.IsSuccess)
            {
                await viewModel.LoadUnitHandover();
                if (UnitHandovers.NeedRefresh.HasValue) UnitHandovers.NeedRefresh = true;
                ToastMessageHelper.ShortMessage(Language.thanh_cong);
                centerConfirm.CloseContent_Tapped(this, EventArgs.Empty);
                SetButtonFloatingButton();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(result.ErrorResponse.error.message);
            }
        }

        private void OptionEntry_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Guid id = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(id);
            contractDetail.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
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

        private void Project_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Guid id = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ProjectInfo projectInfo = new ProjectInfo(id);
            projectInfo.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
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

        private void Unit_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Guid id = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            UnitInfo unitInfo = new UnitInfo(id);
            unitInfo.OnCompleted = async (isSuccessed) => {
                if (isSuccessed)
                {
                    await Navigation.PushAsync(unitInfo);
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
