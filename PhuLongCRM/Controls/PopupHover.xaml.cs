using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhuLongCRM.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupHover : AbsoluteLayout
    {
        public PopupHover()
        {
            InitializeComponent();
        }

        private void Close_Tapped(object sender, EventArgs e)
        {
            try
            {
                this.IsVisible = false;
            }
            catch(Exception ex)
            {

            }
        }
        public void ShowHover(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && this.IsVisible == false)
            {
                label.Text = text;
                this.IsVisible = true;
                //await Task.Delay(2000);
                //this.IsVisible = false;
            }
        }
    }
}