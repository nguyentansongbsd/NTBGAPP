using System;
using Android.Content;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePickerBorder),typeof(DatePickerBorderRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class DatePickerBorderRenderer : DatePickerRenderer
    {
        public DatePickerBorderRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
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
