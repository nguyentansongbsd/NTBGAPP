using System;
using System.Threading.Tasks;
using Android.Content;
using provider = Android.Provider;
using app = Android.App;
using PhuLongCRM.IServices;
using PhuLongCRM.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NumImeiService))]
namespace PhuLongCRM.Droid.Services
{
    public class NumImeiService : INumImeiService
    {
        public async Task<string> GetImei()
        {
            var id = provider.Settings.Secure.GetString(app.Application.Context.ContentResolver, provider.Settings.Secure.AndroidId);
            //TelephonyManager telephonyManager = (TelephonyManager)app.Application.Context.GetSystemService(Context.TelephonyService);
            //string ImeiNum = telephonyManager.Imei;
            return id;
        }
    }
}
