using System;
using content = Android.Content;
using app = Android.App;
using provider = Android.Provider;
using PhuLongCRM.Droid.Services;
using PhuLongCRM.IServices;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppSetting))]
namespace PhuLongCRM.Droid.Services
{
    public class OpenAppSetting : IOpenAppSettings
    {
        public void Open()
        {
            content.Intent intent = new content.Intent(provider.Settings.ActionLocationSourceSettings);
            intent.SetFlags(content.ActivityFlags.NewTask);
            app.Application.Context.StartActivity(intent);
        }
    }
}
