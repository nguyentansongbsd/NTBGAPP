using System;
using Foundation;
using UIKit;
using PhuLongCRM.IServices;
using PhuLongCRM.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppSettings))]
namespace PhuLongCRM.iOS.Services
{
    public class OpenAppSettings : IOpenAppSettings
    {
        public void Open()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
        }
    }
}
