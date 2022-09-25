using System;
using System.Collections.Generic;
using System.Windows.Input;
using PhuLongCRM.Helper;
using PhuLongCRM.Resources;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public partial class DatePickerBoderControl : Grid
    {
        public event EventHandler Date_Selected;
        public event EventHandler Clear_Clicked;

        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(DatePickerBoderControl), null, BindingMode.TwoWay);
        public DateTime? Date { get { return (DateTime?)GetValue(DateProperty); } set { SetValue(DateProperty, value); } }

        public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan?), typeof(DatePickerBoderControl), null, BindingMode.TwoWay);
        public TimeSpan? Time { get { return (TimeSpan?)GetValue(TimeProperty); } set { SetValue(TimeProperty, value); } }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(DatePickerBoderControl), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty ShowEntryProperty = BindableProperty.Create(nameof(ShowEntry), typeof(bool), typeof(DatePickerBoderControl), true, BindingMode.TwoWay);
        public bool ShowEntry { get => (bool)GetValue(ShowEntryProperty); set => SetValue(ShowEntryProperty, value); }

        public static readonly BindableProperty ShowEntryTimeProperty = BindableProperty.Create(nameof(ShowEntryTime), typeof(bool), typeof(DatePickerBoderControl), true, BindingMode.TwoWay);
        public bool ShowEntryTime { get => (bool)GetValue(ShowEntryTimeProperty); set => SetValue(ShowEntryTimeProperty, value); }

        public static readonly BindableProperty FormatDateProperty = BindableProperty.Create(nameof(FormatDate), typeof(string), typeof(DatePickerBoderControl), "dd/MM/yyyy", BindingMode.TwoWay);
        public string FormatDate { get => (string)GetValue(FormatDateProperty); set => SetValue(FormatDateProperty, value); }

        public static readonly BindableProperty FormatTimeProperty = BindableProperty.Create(nameof(FormatTime), typeof(string), typeof(DatePickerBoderControl), "HH:mm", BindingMode.TwoWay);
        public string FormatTime { get => (string)GetValue(FormatTimeProperty); set => SetValue(FormatTimeProperty, value); }

        public static readonly BindableProperty ShowTimeProperty = BindableProperty.Create(nameof(ShowTime), typeof(bool?), typeof(DatePickerBoderControl), null, BindingMode.TwoWay, propertyChanged: ShowTimeChanged);
        public bool? ShowTime { get => (bool?)GetValue(ShowTimeProperty); set => SetValue(ShowTimeProperty, value); }

        public static readonly BindableProperty IsVisibleButtonClearProperty = BindableProperty.Create(nameof(IsVisibleButtonClear), typeof(bool?), typeof(DatePickerBoderControl), null, BindingMode.TwoWay);
        public bool? IsVisibleButtonClear { get => (bool?)GetValue(IsVisibleButtonClearProperty); set => SetValue(IsVisibleButtonClearProperty, value); }

        public bool IsTimeNull { get; set; } = true;

        public DatePickerBoderControl()
        {
            InitializeComponent();
            datePicker.SetBinding(DatePickerBorder.DateProperty, new Binding("Date") { Source = this });
            datePicker.SetBinding(DatePickerBorder.FormatProperty, new Binding("FormatDate") { Source = this });
            entry.SetBinding(MainEntry.PlaceholderProperty, new Binding("Placeholder") { Source = this });
            entry.SetBinding(MainEntry.IsVisibleProperty, new Binding("ShowEntry") { Source = this });
            ShowEntry = Date.HasValue ? false : true;
            btnClear.IsVisible = !ShowEntry;

            timePicker.SetBinding(TimePickerBorder.TimeProperty, new Binding("Time") { Source = this });
            timePicker.SetBinding(TimePickerBorder.FormatProperty, new Binding("FormatTime") { Source = this });
            timePicker.SetBinding(TimePickerBorder.IsVisibleProperty, new Binding("ShowTime") { Source = this });
            lblDash.SetBinding(Label.IsVisibleProperty, new Binding("ShowTime") { Source = this });
            entryTime.SetBinding(MainEntry.IsVisibleProperty, new Binding("ShowEntryTime") { Source = this });
        }

        private static void ShowTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            DatePickerBoderControl control = (DatePickerBoderControl)bindable;
            if (newValue == null) return;

            if (oldValue == null && newValue != null)
            {
                if (control.ShowTime == false)
                {
                    control.btnClearTime.IsVisible = control.entryTime.IsVisible = false;
                    control.ResetGrid(3);
                }
                else if (control.ShowTime == true)
                {
                    control.ResetGrid(1);
                }
            }
        }

        private void datePicker_OnChangeState(object sender, EventArgs e)
        {
            ShowEntry = false;
            if (!Date.HasValue)
            {
                this.Date = DateTime.Now;
                if (!this.Time.HasValue && ShowTime == true)// Khi Date co gia tri thi cap nhat lai Time
                {
                    this.Time = new TimeSpan(this.Date.Value.Hour, this.Date.Value.Minute, this.Date.Value.Second);
                    this.ShowEntryTime = this.Time.HasValue ? false : true;
                    this.IsTimeNull = false;
                    this.btnClearTime.IsVisible = !this.ShowEntryTime;
                    if (this.IsVisibleButtonClear.HasValue && this.IsVisibleButtonClear.Value == false)
                    {
                        this.btnClearTime.IsVisible = this.btnClear.IsVisible = false;
                    }
                    Date = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds);
                }
                else if (this.Time.HasValue && ShowTime == true)// Cap nhat lai time cua Date. Khi Date duoc chon lai
                {
                    Date = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds);
                }
            }
            btnClear.IsVisible = IsVisibleButtonClear.HasValue ? IsVisibleButtonClear.Value : !ShowEntry;
        }

        private void datePicker_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            if (Date.HasValue)
            {
                this.ShowEntry = false;
                btnClear.IsVisible = !ShowEntry;
                if (!this.Time.HasValue && ShowTime == true)// Khi Date co gia tri thi cap nhat lai Time
                {
                    this.Time = new TimeSpan(this.Date.Value.Hour, this.Date.Value.Minute, this.Date.Value.Second);
                    this.ShowEntryTime = this.Time.HasValue ? false : true;
                    this.IsTimeNull = false;
                    this.btnClearTime.IsVisible = !this.ShowEntryTime;
                    if (this.IsVisibleButtonClear.HasValue && this.IsVisibleButtonClear.Value == false)
                    {
                        this.btnClearTime.IsVisible = this.btnClear.IsVisible = false;
                    }
                    Date = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds);
                }
                else if (this.Time.HasValue && ShowTime == true)// Cap nhat lai time cua Date. Khi Date duoc chon lai
                {
                    Date = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, Time.Value.Hours, Time.Value.Minutes, Time.Value.Seconds);
                }
                this.Date_Selected?.Invoke(sender, EventArgs.Empty);
            }
        }

        private void timePicker_OnChangeState(object sender, EventArgs e)
        {
            this.ShowEntryTime = false;
            this.IsTimeNull = false;
            if (!Time.HasValue)
            {
                this.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else if (this.Date.HasValue && this.Time.HasValue)
            {
                this.Date = new DateTime(this.Date.Value.Year, this.Date.Value.Month, this.Date.Value.Day, this.Time.Value.Hours, this.Time.Value.Minutes, this.Time.Value.Seconds);
            }
            else if (!this.Date.HasValue && this.Time.HasValue)
            {
                this.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.Time.Value.Hours, this.Time.Value.Minutes, this.Time.Value.Seconds);
                ShowEntry = false;
                btnClear.IsVisible = !ShowEntry;
            }

            btnClearTime.IsVisible = IsVisibleButtonClear.HasValue ? IsVisibleButtonClear.Value : !ShowEntryTime;

            this.Date_Selected?.Invoke(sender, EventArgs.Empty);
        }

        private void ClearDate_Tapped(object sender, EventArgs e)
        {
            this.Date = null;
            this.ShowEntry = true;
            btnClear.IsVisible = !ShowEntry;
            ClearTime_Tapped(null, EventArgs.Empty);
        }

        private void ClearTime_Tapped(object sender, EventArgs e)
        {
            this.Time = null;
            this.ShowEntryTime = true;
            btnClearTime.IsVisible = !ShowEntryTime;
            this.IsTimeNull = true;
            Clear_Clicked?.Invoke(sender, EventArgs.Empty);
        }

        private void ResetGrid(int numSpan)
        {
            Grid.SetColumnSpan(datePicker, numSpan);
            Grid.SetColumnSpan(entry, numSpan);
            Grid.SetColumnSpan(btnClear, numSpan);
        }

        public void ReSetTime()
        {
            if (this.Date.HasValue)
            {
                this.Time = new TimeSpan(this.Date.Value.Hour, this.Date.Value.Minute, this.Date.Value.Second);
                this.Date = new DateTime(this.Date.Value.Year, this.Date.Value.Month, this.Date.Value.Day, this.Time.Value.Hours, this.Time.Value.Minutes, this.Time.Value.Seconds);
                this.IsTimeNull = false;
                this.ShowEntry = this.ShowEntryTime = false;
                btnClear.IsVisible = btnClearTime.IsVisible = !ShowEntryTime;
                if (this.IsVisibleButtonClear.HasValue && this.IsVisibleButtonClear.Value == false)
                {
                    this.btnClearTime.IsVisible = this.btnClear.IsVisible = false;
                }
            }
        }

        private void datePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (this.ShowTime== true)
            {
                this.Time = new TimeSpan(this.Date.Value.Hour, this.Date.Value.Minute, this.Date.Value.Second);
                this.Date = new DateTime(this.Date.Value.Year, this.Date.Value.Month, this.Date.Value.Day, this.Time.Value.Hours, this.Time.Value.Minutes, this.Time.Value.Seconds);
            }
            this.Date_Selected?.Invoke(sender, EventArgs.Empty);
        }
    }
}
