using System;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class LookUpEntry : Entry
    {
        public LookUpEntry()
        {
            TextColor = Color.Black;
            this.FontSize = 15;
            this.PlaceholderColor = Color.Gray;
            this.HeightRequest = 40;
        }
    }
}
