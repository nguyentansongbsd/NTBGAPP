using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(MainEditor), typeof(MainEditorRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class MainEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    UITextView textField = Control;
                    textField.BackgroundColor = UIColor.White;
                    textField.ContentInset = new UIEdgeInsets(0,5,0,5);
                    textField.Layer.CornerRadius = 5f;
                    textField.Layer.BorderColor = UIColor.FromRGB(201, 201, 201).CGColor;
                    textField.Layer.BorderWidth = 1f;
                }
            }
        }
    }
}
