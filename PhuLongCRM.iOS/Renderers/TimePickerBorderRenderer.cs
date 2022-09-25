using Foundation;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePickerBorder), typeof(TimePickerBorderRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class TimePickerBorderRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.RoundedRect;
                Control.Layer.BorderWidth = 1f;
                Control.Layer.CornerRadius = 6;
                Control.Layer.BorderColor = Color.FromHex("#c9c9c9").ToCGColor();
                Control.AdjustsFontSizeToFitWidth = true;
                Element.HeightRequest = 40f;
            }
        }
    }
}