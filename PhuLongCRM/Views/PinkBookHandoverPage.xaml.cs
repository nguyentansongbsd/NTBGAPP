using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class PinkBookHandoverPage : ContentPage
    {
        public Action<bool> OnCompleted { get; set; }
        public PinkBookHandoverPageViewModel viewModel;
        public PinkBookHandoverPage(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new PinkBookHandoverPageViewModel();
            viewModel.PinkBookHandoverId = id;
            Init();
        }

        private async void Init()
        {
            await viewModel.LoadPinkBookHandover();
            if (viewModel.PinkBookHandover != null)
            {
                SetButtonFloatingButton();
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void SetButtonFloatingButton()
        {
            if (viewModel.PinkBookHandover.statuscode == "1")
            {
                if (viewModel.PinkBookHandover.bsd_printdate.HasValue) // co ngay in
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_ban_giao, "FontAwesomeRegular", "\uf044", null, ConfirmHandover));
                }
                else
                {
                    floatingButtonGroup.IsVisible = false;
                }
            }
            else
            {
                floatingButtonGroup.IsVisible = false;
            }
        }

        private void ConfirmHandover(object sender, EventArgs e)
        {
            centerConfirm.ShowCenterPopup();
        }

        private async void ConfirmPinkbookHandover_Clicked(object sender, EventArgs e)
        {
            if (!viewModel.ConfirmDate.HasValue)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_xac_nhan);
                return;
            }

            LoadingHelper.Show();
            CrmApiResponse result = await viewModel.ConfirmPinkbookHandover();
            if (result.IsSuccess)
            {
                await viewModel.LoadPinkBookHandover();
                if (PinkBookHandovers.NeedRefresh.HasValue) PinkBookHandovers.NeedRefresh = true;
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
