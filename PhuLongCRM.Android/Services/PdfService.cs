using System;
using System.Threading.Tasks;
using Android.Content;
using PhuLongCRM.Droid.Services;
using PhuLongCRM.IServices;

[assembly: Xamarin.Forms.Dependency(typeof(PdfService))]
namespace PhuLongCRM.Droid.Services
{
    public class PdfService : IPdfService
    {
        public async Task View(string url, string name)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse(url), "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetAction(Intent.ActionView);
            try
            {
                Xamarin.Essentials.Platform.CurrentActivity.StartActivity(intent);
            }
            catch (Exception ex)
            {
                await AppShell.Current.Navigation.PushAsync(new PhuLongCRM.Views.ViewPDFFilePage(url) { Title = name });
            }
        }
    }
}