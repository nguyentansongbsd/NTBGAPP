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
    public partial class AcceptanceForm : ContentPage
    {
        private AcceptanceFormViewModel viewModel;
        public Action<bool> OnCompleted;
        public AcceptanceForm(Guid id)
        {
            InitializeComponent();
            BindingContext = viewModel = new AcceptanceFormViewModel();
            Init(id);
        }
        public async void Init(Guid id)
        {
            await viewModel.LoadAcceptance(id);
            if (viewModel.Acceptance != null && viewModel.Acceptance.bsd_acceptanceid != Guid.Empty)
            {
                SetPreOpen();
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }
        public void SetPreOpen()
        {
            lookUpLoaiKQ.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.TypeResults = AcceptanceTypeResult.AcceptanceTypeResultData();
                LoadingHelper.Hide();
            };
            lookUpLich.PreOpenAsync = async () => {
                LoadingHelper.Show();
                await viewModel.LoadInstallment();
                LoadingHelper.Hide();
            };
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (viewModel.TypeResult == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_ket_qua);
                return;
            }
            bool IsSuccess = await viewModel.Update();
            if (IsSuccess)
            {
                if (AcceptanceDetailPage.NeedToRefresh.HasValue) AcceptanceDetailPage.NeedToRefresh = true;
                await Navigation.PopAsync();
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
        }
    }
}