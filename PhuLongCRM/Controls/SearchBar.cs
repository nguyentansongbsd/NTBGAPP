using System;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class SearchBar : Xamarin.Forms.SearchBar
    {
        public SearchBar()
        {
            Placeholder = Language.tim_kiem;
            FontSize = 15;
            TextColor = Color.FromHex("#444444");
            FontFamily = "Segoe";
            BackgroundColor = Color.White;
        }
    }
}
