using System;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class ClickableView : StackLayout
    {
        public event EventHandler Clicked;

        //CommandParameter
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LabelButton), null, BindingMode.TwoWay);
        public object CommandParameter { get => (object)GetValue(CommandParameterProperty); set { SetValue(CommandParameterProperty, value); } }

        public ClickableView()
        {
            TapGestureRecognizer tap = new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1
            };
            tap.Tapped += Tap_Tapped;
            this.GestureRecognizers.Add(tap);
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            Clicked(this, EventArgs.Empty);
        }
    }
}
