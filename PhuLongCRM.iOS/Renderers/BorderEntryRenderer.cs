using System;
using CoreGraphics;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderEntry),typeof(BorderEntryRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class BorderEntryRenderer :EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                this.Control.LeftView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.RightView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.LeftViewMode = UITextFieldViewMode.Always;
                this.Control.RightViewMode = UITextFieldViewMode.Always;
                this.Control.BorderStyle = UITextBorderStyle.None;
                this.Element.HeightRequest = 35;
            }
        }
    }
}
