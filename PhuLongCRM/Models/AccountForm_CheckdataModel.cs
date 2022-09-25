using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AccountForm_CheckdataModel : BaseViewModel
    {
      
        public string bsd_vatregistrationnumber { get; set ;} 

        public string bsd_registrationcode { get; set; }

        public string bsd_account_name { get; set; }
    }
}
