using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(MainSearchBar), typeof(MainSearchBarRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class MainSearchBarRenderer : SearchBarRenderer
    {
        private UITextField searchField;
        private UISearchTextField searchTextField;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && e.NewElement != null && Control != null)
            {
                this.Element.HeightRequest = 40;
                Control.BackgroundColor = UIColor.Clear;
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    searchTextField = Control.SearchTextField;
                    searchTextField.BorderStyle = UITextBorderStyle.None;
                    searchTextField.ClipsToBounds = true;
                    searchTextField.Layer.BorderWidth = 1f;
                    searchTextField.Layer.BorderColor = UIColor.FromWhiteAlpha(0.90f, 1).CGColor;
                    searchTextField.Layer.CornerRadius = 8;
                    searchTextField.BackgroundColor = (e.NewElement as MainSearchBar).BackgroundColor.ToUIColor();

                    searchTextField.TranslatesAutoresizingMaskIntoConstraints = false;
                    searchTextField.HeightAnchor.ConstraintEqualTo(40).Active = true;
                    searchTextField.LeadingAnchor.ConstraintEqualTo(Control.LeadingAnchor, 0).Active = true;
                    searchTextField.TrailingAnchor.ConstraintEqualTo(Control.TrailingAnchor, 0).Active = true;
                    searchTextField.TopAnchor.ConstraintEqualTo(Control.TopAnchor, 0).Active = true;
                }
                else
                {
                    bool flag = false;

                    foreach (var subView in Control.Subviews)
                    {
                        foreach (var subSubView in subView.Subviews)
                        {
                            if (subSubView.GetType() == typeof(UITextField))
                            {
                                UITextField field = subSubView as UITextField;
                                searchField = field;
                                field.BackgroundColor = (e.NewElement as MainSearchBar).BackgroundColor.ToUIColor();

                                field.Layer.BorderWidth = 1f;
                                field.Layer.BorderColor = UIColor.FromWhiteAlpha(0.90f, 1).CGColor;
                                field.Layer.CornerRadius = 8;

                                field.TranslatesAutoresizingMaskIntoConstraints = false;
                                field.HeightAnchor.ConstraintEqualTo(40).Active = true;
                                field.LeadingAnchor.ConstraintEqualTo(Control.LeadingAnchor, 0).Active = true;
                                field.TrailingAnchor.ConstraintEqualTo(Control.TrailingAnchor, 0).Active = true;
                                field.TopAnchor.ConstraintEqualTo(Control.TopAnchor, 0).Active = true;
                                Control.LayoutIfNeeded();
                                flag = true;
                            }
                            if (flag) break;
                        }
                        if (flag) break;
                    }
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "BackgroundColor")
            {
                if (searchField != null)
                {
                    searchField.BackgroundColor = (this.Element as MainSearchBar).BackgroundColor.ToUIColor();
                }
                else if (searchTextField != null)
                {
                    searchTextField.BackgroundColor = (this.Element as MainSearchBar).BackgroundColor.ToUIColor();
                }
            }
            else if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
        }
    }
}
