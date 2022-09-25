using System;
using System.Collections.Generic;
using System.Text;
using PhuLongCRM.ViewModels;

namespace PhuLongCRM.Models
{
    public class EventModel : BaseViewModel
    {
        public Guid bsd_eventid { get; set; }
        public string bsd_name { get; set; }
        public string bsd_eventcode { get; set; }
        public Guid bsd_phaselaunch { get; set; }
        public string bsd_phaselaunch_name { get; set; }
        private DateTime? _bsd_startdate;
        public DateTime? bsd_startdate { get=>_bsd_startdate; set { _bsd_startdate = value;OnPropertyChanged(nameof(bsd_startdate)); } }
        private DateTime? _bsd_enddate;
        public DateTime? bsd_enddate { get=>_bsd_enddate; set { _bsd_enddate = value; OnPropertyChanged(nameof(bsd_enddate)); } }
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }
    }
}
