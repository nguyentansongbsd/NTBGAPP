using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class FormLabelValue : Label
    {
        public FormLabelValue()
        {
            this.FontSize = 15;
            Grid.SetColumn(this, 1);
            this.VerticalTextAlignment = TextAlignment.Center;
            //this.FontAttributes = FontAttributes.Bold;
            TextColor = Color.Black;
            Margin = new Thickness(4, 0, 0, 0);
        }
    }
}
