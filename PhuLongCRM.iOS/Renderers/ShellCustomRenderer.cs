using System;
using PhuLongCRM;
using PhuLongCRM.iOS.Renderers;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppShell), typeof(ShellCustomRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class ShellCustomRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            var renderer = base.CreateShellSectionRenderer(shellSection);
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                OverrideUserInterfaceStyle = UIKit.UIUserInterfaceStyle.Light;
            if (DeviceInfo.Version.Major >= 15 && renderer != null)
            {
                if (renderer is ShellSectionRenderer shellRenderer)
                {
                    var appearance = new UINavigationBarAppearance();
                    appearance.ConfigureWithOpaqueBackground();
                    appearance.BackgroundColor = UIColor.FromRGB(19, 153, 213); 
                    appearance.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };

                    shellRenderer.NavigationBar.Translucent = false;
                    shellRenderer.NavigationBar.StandardAppearance = appearance;
                    shellRenderer.NavigationBar.ScrollEdgeAppearance = shellRenderer.NavigationBar.StandardAppearance;
                }
            }
            return renderer;
        }
    }
}
