using System;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class LabelButton : Label
    {
        public event EventHandler Clicked;

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LabelButton), null, BindingMode.TwoWay);
        public object CommandParameter { get => (object)GetValue(CommandParameterProperty); set { SetValue(CommandParameterProperty, value); } }

        private TapGestureRecognizer tap;

        public LabelButton()
        {
            tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;
            this.GestureRecognizers.Add(tap);
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            if (this.CommandParameter != null)
            {
                tap.CommandParameter = this.CommandParameter;
            }
            Clicked?.Invoke(this, e);
        }
    }
}
