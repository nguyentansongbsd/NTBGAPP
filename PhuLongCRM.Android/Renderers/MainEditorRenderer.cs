using System;
using Android.Content;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MainEditor), typeof(MainEditorRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class MainEditorRenderer : EditorRenderer
    {
        public MainEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.SetBackgroundResource(PhuLongCRM.Droid.Resource.Drawable.bg_main_entry);
                    Control.SetPadding(15, 20, 15, 20);
                }
            }
        }
    }
}
