using System;
using System.Windows.Input;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class FloatButtonItem : BaseViewModel
    {

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public string Key { get; set; }

        public string Icon { get; set; }

        private string _fontFamily;
        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }


        public ICommand OnClickCommand { get; set; }
        public EventHandler OnClickeEvent { get; set; }

        public FloatButtonItem(string text, string fontfamily, string icon, Command onClickCommand, EventHandler onClickeEvent)
        {
            this.Text = text;
            this.Icon = icon;
            this.FontFamily = fontfamily;
            this.OnClickCommand = onClickCommand;
            this.OnClickeEvent = onClickeEvent;
        }

        public FloatButtonItem(string text, string fontfamily, string icon, Action _onClickeEvent, string key = null)
        {
            this.Text = text;
            this.Icon = icon;
            this.FontFamily = fontfamily;
            this.OnClickCommand = new Command(_onClickeEvent);
            this.Key = key;
        }
    }
}
