using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class TimePickerBorder : TimePicker
    {
        public event EventHandler OnChangeState;

        public TimePickerBorder()
        {
            this.FontSize = 15;
            this.TextColor = Color.FromHex("#333333");
        }

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            OnChangeState?.Invoke(this, EventArgs.Empty);
        }
    }
}
