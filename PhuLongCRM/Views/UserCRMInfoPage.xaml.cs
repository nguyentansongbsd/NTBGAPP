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
    public partial class UserCRMInfoPage : ContentPage
    {
        public UserCRMInfoPageViewModel viewModel;
        public Action<bool> OnCompleted;
        public UserCRMInfoPage()
        {
            InitializeComponent();
            Init();
        }
        private async void Init()
        {
            this.BindingContext = viewModel = new UserCRMInfoPageViewModel();
            await viewModel.LoadUserCRM();
            if (viewModel.UserCRM != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }
    }
}