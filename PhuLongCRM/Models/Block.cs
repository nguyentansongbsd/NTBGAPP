using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PhuLongCRM.Models
{
    public class Block : BaseViewModel
    {
        public Guid bsd_blockid { get; set; }
        public string bsd_name { get; set; }

        private int _numChuanBiInBlock;
        public int NumChuanBiInBlock { get=>_numChuanBiInBlock; set { _numChuanBiInBlock = value;OnPropertyChanged(nameof(NumChuanBiInBlock)); } }

        private int _numSanSangInBlock;
        public int NumSanSangInBlock { get=>_numSanSangInBlock; set { _numSanSangInBlock = value;OnPropertyChanged(nameof(NumSanSangInBlock)); } }

        private int _numBookingInBlock;
        public int NumBookingInBlock { get => _numBookingInBlock; set { _numBookingInBlock = value; OnPropertyChanged(nameof(NumBookingInBlock)); } }

        private int _numGiuChoInBlock;
        public int NumGiuChoInBlock { get=>_numGiuChoInBlock; set { _numGiuChoInBlock = value; OnPropertyChanged(nameof(NumGiuChoInBlock)); } }

        private int _numDatCocInBlock;
        public int NumDatCocInBlock { get=>_numDatCocInBlock; set { _numDatCocInBlock = value; OnPropertyChanged(nameof(NumDatCocInBlock)); } }

        private int _numDongYChuyenCoInBlock;
        public int NumDongYChuyenCoInBlock { get=>_numDongYChuyenCoInBlock; set { _numDongYChuyenCoInBlock = value;OnPropertyChanged(nameof(NumDongYChuyenCoInBlock)); } }

        private int _numDaDuTienCocInBlock;
        public int NumDaDuTienCocInBlock { get=>_numDaDuTienCocInBlock; set { _numDaDuTienCocInBlock = value;OnPropertyChanged(nameof(NumDaDuTienCocInBlock)); } }

        private int _numOptionInBlock;
        public int NumOptionInBlock { get => _numOptionInBlock; set { _numOptionInBlock = value; OnPropertyChanged(nameof(NumOptionInBlock)); } }

        private int _numThanhToanDot1InBlock;
        public int NumThanhToanDot1InBlock { get=> _numThanhToanDot1InBlock; set { _numThanhToanDot1InBlock = value;OnPropertyChanged(nameof(NumThanhToanDot1InBlock)); } }

        private int _numSignedDAInBlock;
        public int NumSignedDAInBlock { get => _numSignedDAInBlock; set { _numSignedDAInBlock = value; OnPropertyChanged(nameof(NumSignedDAInBlock)); } }

        private int _numQualifiedInBlock;
        public int NumQualifiedInBlock { get => _numQualifiedInBlock; set { _numQualifiedInBlock = value; OnPropertyChanged(nameof(NumQualifiedInBlock)); } }

        private int _numDaBanInBlock;
        public int NumDaBanInBlock { get=>_numDaBanInBlock; set { _numDaBanInBlock = value;OnPropertyChanged(nameof(NumDaBanInBlock)); } }

        private int _totalUnitInBlock;
        public int TotalUnitInBlock { get => _totalUnitInBlock; set { _totalUnitInBlock = value;OnPropertyChanged(nameof(TotalUnitInBlock)); } }

        public ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();
        public int page { get; set; }// số page cho loadmore
    }
}
