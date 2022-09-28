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

                if(viewModel.Acceptance.statuscode == "1")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_thong_tin, "FontAwesomeSolid", "\uf46c", null, ConfirmInformation));

                if (viewModel.Acceptance.statuscode == "1" || viewModel.Acceptance.statuscode == "100000001" || viewModel.Acceptance.statuscode == "100000000")
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_nghiem_thu, "FontAwesomeSolid", "\uf05e", null, Cancel));

                if (viewModel.Acceptance.statuscode == "100000001")
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
            if (result)
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
    }
}