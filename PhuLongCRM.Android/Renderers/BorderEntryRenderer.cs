using System;
using Android.Content;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class BorderEntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
    {
        public BorderEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;
            //var gradientDrawable = new GradientDrawable();
            //gradientDrawable.SetCornerRadius(10f);
            //gradientDrawable.SetStroke(1, Android.Graphics.Color.Black);
            //gradientDrawable.SetColor(Android.Graphics.Color.LightGray);
            //Control.SetBackground(gradientDrawable);
            Control.Background = new ColorDrawable(Color.White.ToAndroid());
            Control.SetPadding(10,10,10,10);
        }
    }
}