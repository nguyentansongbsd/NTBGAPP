using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class ExtendedSearchBarRenderer: SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            //UISearchBar bar = (UISearchBar)this.Control;
            //bar.TintColor = UIColor.Black;

            //!!! Works, but only for the first search bar (we have 3) **
            UITextField.AppearanceWhenContainedIn(typeof(UISearchBar)).BackgroundColor =
            UIColor.White;

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && Control != null)
            {
                Control.SearchTextField.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            }

            //Match text field within SearchBar to its background color
            //using (var searchKey = new NSString("_searchField"))
            //{
            //    if (e.NewElement == null) return;

            //    //!!! Throws an iOS error on iOS 13 ***
            //    //var textField = (UITextField)Control.ValueForKey(searchKey);
            //    //textField.BackgroundColor = e.NewElement.BackgroundColor.ToUIColor();
            //    //textField.TintColor = UIColor.White;
            //}

            //var searchBar = ((UISearchBar)Control);
            //searchBar.EnablesReturnKeyAutomatically = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            // Hide Cancel Button
            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
            
            //Control.ShowsCancelButton = true;
        }
    }
}
