using System;
using PhuLongCRM.Helper;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class TotalReservationModel : BaseViewModel
    {
        private decimal _listedPrice;
        public decimal ListedPrice { get => _listedPrice; set { _listedPrice = value;OnPropertyChanged(nameof(ListedPrice)); } }

        private string _listedPrice_format;
        public string ListedPrice_format { get => _listedPrice_format; set { _listedPrice_format = value; OnPropertyChanged(nameof(ListedPrice_format)); OnPropertyChanged(nameof(ListedPrice)); } }

        private decimal _discount;
        public decimal Discount { get => _discount; set { _discount = value; OnPropertyChanged(nameof(Discount)); } }

        private string _discount_format;
        public string Discount_format { get => _discount_format; set { _discount_format = value; OnPropertyChanged(nameof(Discount_format)); } }

        private decimal _handoverAmount;
        public decimal HandoverAmount { get => _handoverAmount; set { _handoverAmount = value; OnPropertyChanged(nameof(HandoverAmount)); } }

        private string _handoverAmount_format;
        public string HandoverAmount_format { get => _handoverAmount_format; set { _handoverAmount_format = value; OnPropertyChanged(nameof(HandoverAmount_format)); } }

        private decimal _netSellingPrice;
        public decimal NetSellingPrice { get => _netSellingPrice; set { _netSellingPrice = value; OnPropertyChanged(nameof(NetSellingPrice)); } }

        private string _netSellingPrice_format;
        public string NetSellingPrice_format { get => _netSellingPrice_format; set { _netSellingPrice_format = value; OnPropertyChanged(nameof(NetSellingPrice_format)); } }

        private decimal _landValue;
        public decimal LandValue { get => _landValue; set { _landValue = value; OnPropertyChanged(nameof(LandValue)); } }

        private string _landValue_format;
        public string LandValue_format { get => _landValue_format; set { _landValue_format = value; OnPropertyChanged(nameof(LandValue_format)); } }

        private decimal _totalTax;
        public decimal TotalTax { get => _totalTax; set { _totalTax = value; OnPropertyChanged(nameof(TotalTax)); } }

        private string _totalTax_format;
        public string TotalTax_format { get => _totalTax_format; set { _totalTax_format = value; OnPropertyChanged(nameof(TotalTax_format)); } }

        private decimal _maintenanceFee;
        public decimal MaintenanceFee { get => _maintenanceFee; set { _maintenanceFee = value; OnPropertyChanged(nameof(MaintenanceFee)); } }

        private string _maintenanceFee_format;
        public string MaintenanceFee_format { get => _maintenanceFee_format; set { _maintenanceFee_format = value; OnPropertyChanged(nameof(MaintenanceFee_format)); } }

        private decimal _netSellingPriceAfterVAT;
        public decimal NetSellingPriceAfterVAT { get => _netSellingPriceAfterVAT; set { _netSellingPriceAfterVAT = value; OnPropertyChanged(nameof(NetSellingPriceAfterVAT)); } }

        private string _netSellingPriceAfterVAT_format;
        public string NetSellingPriceAfterVAT_format { get => _netSellingPriceAfterVAT_format; set { _netSellingPriceAfterVAT_format = value; OnPropertyChanged(nameof(NetSellingPriceAfterVAT_format)); } }

        private decimal _totalAmount;
        public decimal TotalAmount { get => _totalAmount; set { _totalAmount = value; OnPropertyChanged(nameof(TotalAmount)); } }

        private string _totalAmount_format;
        public string TotalAmount_format { get => _totalAmount_format; set { _totalAmount_format = value; OnPropertyChanged(nameof(TotalAmount_format)); } }
    }
}
