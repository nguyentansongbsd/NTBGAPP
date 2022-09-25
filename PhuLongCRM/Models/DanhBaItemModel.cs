using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class DanhBaItemModel : Plugin.ContactService.Shared.Contact, INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private string _numberFormted;
        public string numberFormated
        {
            get => _numberFormted; set
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    _numberFormted = value;
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    _numberFormted = value.Replace("stringValue=", "!").Split('!')[1].Split(',')[0];

                }
                OnPropertyChanged(nameof(numberFormated));
            }
        }

        private bool _isConvertToLead;
        public bool IsConvertToLead { get => _isConvertToLead; set { _isConvertToLead = value; OnPropertyChanged(nameof(IsConvertToLead)); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
