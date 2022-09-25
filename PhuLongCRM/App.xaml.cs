using System;
using System.Globalization;
using PhuLongCRM.Resources;
using PhuLongCRM.Settings;
using PhuLongCRM.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CultureInfo cultureInfo = new CultureInfo(UserLogged.Language);
            Language.Culture = cultureInfo;
            MainPage = new AppShell();
            Shell.Current.Navigation.PushAsync(new Login(), false);
            //MainPage = new BlankPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
