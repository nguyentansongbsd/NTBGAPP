using Android.App;
using Android.Content;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TimePickerBorder), typeof(TimePickerBorderRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class TimePickerBorderRenderer : TimePickerRenderer
    {
        public TimePickerBorderRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.bg_main_entry);
                Control.SetPadding(15, 26, 15, 26);
            }
        }
    }
}