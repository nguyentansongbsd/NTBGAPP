using System;
using Android.Content;
using Android.Content.Res;
using adnroidGraphics= Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PhuLongCRM.Droid.Renderers;

[assembly: ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        public ExtendedSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(adnroidGraphics.Color.Transparent);

                SearchView searchView = (base.Control as SearchView);
                var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var searchIcon = searchView.FindViewById(searchIconId);
                (searchIcon as ImageView).SetColorFilter(adnroidGraphics.Color.LightGray, adnroidGraphics.PorterDuff.Mode.SrcIn);

                int editTextId = Resources.GetIdentifier("android:id/search_src_text", null, null);
                EditText editText = (Control.FindViewById(editTextId) as EditText);
            }

        }
    }
}
