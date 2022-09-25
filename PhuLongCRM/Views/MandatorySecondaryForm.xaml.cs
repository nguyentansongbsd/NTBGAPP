using PhuLongCRM.Helper;
using PhuLongCRM.Helpers;
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
    public partial class MandatorySecondaryForm : ContentPage
    {
        private MandatorySecondaryFormViewModel viewModel;
        public MandatorySecondaryForm(OptionSet account)
        {
            InitializeComponent();
            Init(account);
        }

        private async void Init(OptionSet account)
        {
            this.BindingContext = viewModel = new MandatorySecondaryFormViewModel();
            SetPreOpen();
            if (account != null)
            {
                viewModel.mandatorySecondary.bsd_developeraccount = account.Label;
                viewModel.mandatorySecondary._bsd_developeraccount_value = Guid.Parse(account.Val);
            }
        }

        public void SetPreOpen()
        {
            Lookup_Account.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadContactsLookup();
                LoadingHelper.Hide();
            };
        }

        private async void AddMandatorySecondary_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }
            if (viewModel.Contact == null || viewModel.Contact.Id == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_nguoi_uy_quyen);
                return;
            }
            if (viewModel.mandatorySecondary.bsd_effectivedatefrom == null || viewModel.mandatorySecondary.bsd_effectivedateto == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_hieu_luc);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_descriptionsvn))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mo_ta_vn);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_descriptionsen))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mo_ta_en);
                return;
            }
            LoadingHelper.Show();
            viewModel.mandatorySecondary.bsd_contactid = viewModel.Contact.Id;
            if(await viewModel.Save())
            {
                LoadingHelper.Hide();
                if (AccountDetailPage.NeedToRefreshMandatory.HasValue) AccountDetailPage.NeedToRefreshMandatory = true;
                await Navigation.PopAsync();
                ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
            }
        }

        private void Effectivedateto_DateSelected(object sender, EventArgs e)
        {
            if (viewModel.mandatorySecondary.bsd_effectivedatefrom == null)
                viewModel.mandatorySecondary.bsd_effectivedatefrom = DateTime.Now;
            if (this.compareDateTime(viewModel.mandatorySecondary.bsd_effectivedatefrom, viewModel.mandatorySecondary.bsd_effectivedateto) == -1)
            {
                viewModel.mandatorySecondary.bsd_effectivedateto = viewModel.mandatorySecondary.bsd_effectivedatefrom;
                ToastMessageHelper.ShortMessage(Language.ngay_het_hieu_luc_phai_lon_hon_ngay_bat_dau);
            }
        }

        private void Effectivedatefrom_DateSelected(object sender, EventArgs e)
        {
            if (viewModel.mandatorySecondary.bsd_effectivedateto == null)
                viewModel.mandatorySecondary.bsd_effectivedateto = DateTime.Now;
            if (this.compareDateTime(viewModel.mandatorySecondary.bsd_effectivedatefrom,viewModel.mandatorySecondary.bsd_effectivedateto) == -1)
            {
                viewModel.mandatorySecondary.bsd_effectivedatefrom = viewModel.mandatorySecondary.bsd_effectivedateto;
                ToastMessageHelper.ShortMessage(Language.ngay_het_hieu_luc_phai_lon_hon_ngay_bat_dau);
            }    
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date != null)
            {
                int result = DateTime.Compare(date.Value, date1.Value);
                if (result < 0)
                    return -1;
                else if (result == 0)
                    return 0;
                else
                    return 1;
            }
            if (date == null && date1 != null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }
    }
}