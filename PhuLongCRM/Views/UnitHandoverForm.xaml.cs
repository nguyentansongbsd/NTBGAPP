using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class UnitHandoverForm : ContentPage
    {
        public Action<bool> OnCompleted { get; set; }
        public UnitHandoverFormViewModel viewModel;
        public UnitHandoverForm(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitHandoverFormViewModel();
            viewModel.UnitHandoverId = id;
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadUnitHandover();
            if (viewModel.UnitHandover != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            CrmApiResponse result = await viewModel.UpdateUnitHandover();
            if (result.IsSuccess)
            {
                if (UnitHandoverPage.NeedRefresh.HasValue) UnitHandoverPage.NeedRefresh = true;
                if (UnitHandovers.NeedRefresh.HasValue) UnitHandovers.NeedRefresh = true;
                await Navigation.PopAsync();
                ToastMessageHelper.ShortMessage(Language.thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(result.ErrorResponse.error.message);
            }
        }
    }
}
