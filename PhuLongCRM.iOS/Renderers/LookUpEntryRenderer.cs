using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(LookUpEntry), typeof(LookUpEntryRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class LookUpEntryRenderer : EntryRenderer
    {
        private LookUpEntry _lookUpEntry;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null && e.NewElement != null)
            {
                UITextField textField = Control;
                textField.BackgroundColor = UIColor.White;
                textField.BorderStyle = UITextBorderStyle.None;
                textField.Layer.CornerRadius = 5f;
                textField.Layer.BorderColor = UIColor.FromRGB(201, 201, 201).CGColor;
                textField.Layer.BorderWidth = 1f;
                textField.LeftViewMode = UITextFieldViewMode.Always;
                textField.LeftView = new UIView(frame: new CGRect(0, 0, 10, 0f));
                textField.RightViewMode = UITextFieldViewMode.Always;

                textField.RightView = new UIView(frame: new CGRect(0, 0, 20, 0f));
                textField.ClearButtonMode = UITextFieldViewMode.Never;

                //textField.UserInteractionEnabled = false;
                textField.AddSubview(new UIView());
            }
        }
    }
}
