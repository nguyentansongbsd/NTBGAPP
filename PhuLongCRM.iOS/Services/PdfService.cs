using System;
using System.Threading.Tasks;
using PhuLongCRM.iOS.Services;
using PhuLongCRM.IServices;
using PhuLongCRM.Views;

[assembly:Xamarin.Forms.Dependency(typeof(PdfService))]
namespace PhuLongCRM.iOS.Services
{
    public class PdfService : IPdfService
    {
        public async Task View(string Url, string Name)
        {
            await AppShell.Current.Navigation.PushAsync(new ViewPDFFilePage(Url) { Title = Name });
        }
    }
}
