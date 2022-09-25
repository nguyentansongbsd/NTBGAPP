using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class FormLabel : Label
    {
        public FormLabel()
        {
            this.FontSize = 15;
            this.VerticalTextAlignment = TextAlignment.Center;
            Grid.SetColumn(this, 0);
            TextColor = Color.FromHex("#444444");
        }
    }
}
