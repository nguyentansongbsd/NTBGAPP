using System;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class EntryNoneBorder : Entry
    {
        public EntryNoneBorder()
        {
            TextColor = Color.Black;
            this.FontSize = 16;
            this.HeightRequest = 40;
            this.PlaceholderColor = Color.Gray;
            this.FontFamily = "Segoe";
        }
    }
}
