using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(MainEntry), typeof(MainEntryRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class MainEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null && e.NewElement != null)
            {
                UITextField textField = Control;
                textField.BackgroundColor = UIColor.White;
                textField.BorderStyle = UITextBorderStyle.None;

                textField.Layer.CornerRadius = 5;
                textField.Layer.BorderColor = UIColor.FromRGB(201, 201, 201).CGColor;
                textField.Layer.BorderWidth = 1f;

                textField.LeftViewMode = UITextFieldViewMode.Always;
                textField.LeftView = new UIView(frame: new CGRect(0, 0, 10, 0f));
                textField.ClearButtonMode = (this.Element as MainEntry).ShowClearButton ? UITextFieldViewMode.Always : UITextFieldViewMode.Never;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "ShowClearButton")
            {
                if ((this.Element as MainEntry).ShowClearButton)
                {
                    Control.RightViewMode = UITextFieldViewMode.Always;
                }
                else
                {
                    Control.RightViewMode = UITextFieldViewMode.Never;
                }
            }
        }
    }
}
