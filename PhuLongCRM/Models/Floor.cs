using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace PhuLongCRM.Models
{
    public class Floor : BaseViewModel
    {
        public Guid bsd_floorid { get; set; }
        public string bsd_name { get; set; }
        //public List<Unit> Units { get; set; } = new List<Unit>();
        public ObservableCollection<Unit> Units { get; set; } = new ObservableCollection<Unit>();
        //private ObservableCollection<Unit> _units;
        //public ObservableCollection<Unit> Units { get => _units; set { _units = value; OnPropertyChanged(nameof(Units)); } }

        public int NumChuanBiInFloor { get; set; }
        public int NumSanSangInFloor { get; set; }
        public int NumBookingInFloor { get; set; }
        public int NumGiuChoInFloor { get; set; }
        public int NumDatCocInFloor { get; set; }
        public int NumDongYChuyenCoInFloor { get; set; }
        public int NumDaDuTienCocInFloor { get; set; }
        public int NumOptionInFloor { get; set; }
        public int NumThanhToanDot1InFloor { get; set; }
        public int NumSignedDAInFloor { get; set; }
        public int NumQualifiedInFloor { get; set; }
        public int NumDaBanInFloor { get; set; }
        public int TotalUnitInFloor { get; set; }

        private bool _iShow;
        public bool iShow { get => _iShow; set { _iShow = value; OnPropertyChanged(nameof(iShow)); } }
    }
}
