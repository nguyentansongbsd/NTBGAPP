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
        public AcceptanceDetailPage(Guid id)
        {
            InitializeComponent();
            BindingContext = viewModel = new AcceptanceDetailPageViewModel();
            Init(id);
        }
        public async void Init(Guid id)
        {
            await viewModel.LoadAcceptance(id);
            if (viewModel.Acceptance != null && viewModel.Acceptance.bsd_acceptanceid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }
    }
}