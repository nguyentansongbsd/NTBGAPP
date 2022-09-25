using System;
using PhuLongCRM.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(LightModeRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
	public class LightModeRenderer : PageRenderer
	{
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                OverrideUserInterfaceStyle = UIKit.UIUserInterfaceStyle.Light;
        }
    }
}

