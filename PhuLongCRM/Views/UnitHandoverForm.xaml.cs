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
        public UnitHandoverForm(ContractModel contract)
        {
            InitializeComponent();
            LoadingHelper.Show();
            this.BindingContext = viewModel = new UnitHandoverFormViewModel();
            if (contract != null)
            {
                InitCreate(contract);
            }    
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
        public async void InitCreate(ContractModel contract)
        {
            await viewModel.LoadContract(contract);
            if (viewModel.UnitHandover != null)
            {
                label_maBanGiao.IsVisible = false;
                label_tenBanGiao.IsVisible = false;
                LoadingHelper.Hide();
                OnCompleted?.Invoke(true);
            }
            else
            {
                LoadingHelper.Hide();
                OnCompleted?.Invoke(false);
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.UnitHandoverId != Guid.Empty)
            {
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
            else
            {
                CrmApiResponse result = await viewModel.CreateUnitHandover();
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
}
