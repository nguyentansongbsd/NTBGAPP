using System;
using Android.Content;
using PhuLongCRM.Droid.Renderers;
using PhuLongCRM.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(EntryNoneBorderControl),typeof(EntryNoneBorderRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class EntryNoneBorderRenderer : EntryRenderer
    {
        public EntryNoneBorderRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.SetBackgroundColor(color.White);

                var lp = new MarginLayoutParams(Control.LayoutParameters);
                lp.SetMargins(0, 0, 0, 0);
                LayoutParameters = lp;
                Control.LayoutParameters = lp;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}
