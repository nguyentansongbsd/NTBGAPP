using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Foundation;
using PhuLongCRM.IServices;
using UIKit;
using Xamarin.Forms;

namespace PhuLongCRM.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            Stormlion.PhotoBrowser.iOS.Platform.Init();
            CachedImageRenderer.Init();
            
            LoadApplication(new App());
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            DependencyService.Get<ILoadingService>().Initilize();

            return base.FinishedLaunching(app, options);
        }
    }
}
