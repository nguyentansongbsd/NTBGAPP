using PhuLongCRM.Helper;
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
    public partial class UnitSpecificationDetailPage : ContentPage
    {
        private UnitSpecificationDetailPageViewModel viewModel;
        public Action<bool> OnCompleted;
        public UnitSpecificationDetailPage(Guid id)
        {
            InitializeComponent();
            BindingContext = viewModel = new UnitSpecificationDetailPageViewModel();
            Init(id);
        }

        public async void Init(Guid id)
        {
            await viewModel.LoadUnitSpec(id);
            if (viewModel.UnitSpec != null && viewModel.UnitSpec.bsd_unitsspecificationid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        private async void ShowMore_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.Page++;
            await viewModel.LoadUnitSpecificationDetails();
            LoadingHelper.Hide();
        }

        private async void UnitSpecificationDetail_Tapped(object sender, EventArgs e)
        {
        }
    }
}