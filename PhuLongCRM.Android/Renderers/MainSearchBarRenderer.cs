using System;
using Android.Content;
using views = Android.Views;
using graphics = Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PhuLongCRM.Controls;
using PhuLongCRM.Droid.Renderers;

[assembly: ExportRenderer(typeof(MainSearchBar), typeof(MainSearchBarRenderer))]
namespace PhuLongCRM.Droid.Renderers
{
    public class MainSearchBarRenderer : SearchBarRenderer
    {
        public MainSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null && e.NewElement != null)
            {
                Element.HeightRequest = 38;
                Element.BackgroundColor = Color.Transparent;
                Control.Background = Context.GetDrawable(Droid.Resource.Drawable.bg_search);

                var searchView = base.Control as SearchView;

                // set size cua search icon
                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                ImageView searchViewIcon = (ImageView)searchView.FindViewById<ImageView>(searchIconId);
                //searchViewIcon.SetImageDrawable(Context.GetDrawable(Resource.Drawable.bell));
                var layoutParameters = new SearchView.LayoutParams(50, 50);
                layoutParameters.Gravity = views.GravityFlags.CenterVertical;
                searchViewIcon.LayoutParameters = layoutParameters;

                // set size cua clear icon

                ImageView searchClose = (ImageView)searchView.FindViewById(Context.Resources.GetIdentifier("android:id/search_close_btn", null, null));

                searchClose.SetImageResource(Droid.Resource.Drawable.ic_clear);

                // bo underline
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(graphics.Color.Transparent);
            }
        }
    }
}
