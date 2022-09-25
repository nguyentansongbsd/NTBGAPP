using System;
using System.Threading.Tasks;
using PhuLongCRM.iOS.Services;
using PhuLongCRM.IServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NumImeiService))]
namespace PhuLongCRM.iOS.Services
{
    public class NumImeiService :INumImeiService
    {
        public async Task<string> GetImei()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}
