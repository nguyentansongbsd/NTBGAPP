using PhuLongCRM.Resources;
using System;

using Xamarin.Forms;

namespace PhuLongCRM.Models
{
    public class ListCaseAcc : ContentView
    {
        public string customerid { get; set; }
        public string title { get; set; }
        public string ticketnumber { get; set; }
        public int prioritycode { get; set; }
        public string prioritycodevalue
        {
            get
            {
                switch (prioritycode)
                {
                    case 1:
                        return Language.case_high_priority; //case_high_priority
                    case 2:
                        return Language.case_normal_priority; //case_normal_priority
                    case 3:
                        return Language.case_low_priority; //case_low_priority
                    default:
                        return "";
                }
            }
        }
        public int caseorigincode { get; set; }
        public string caseorigincodevalue
        {
            get
            {
                switch (caseorigincode)
                {
                    case 1:
                        return "Phone";
                    case 2:
                        return "Email";
                    case 3:
                        return "Web";
                    case 2483:
                        return "Facebook";
                    case 3986:
                        return "Twitter";
                    default:
                        return "";
                }
            }
        }
        public int statuscode { get; set; }
        public string statuscodevalue
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return Language.dang_xu_ly; //"In Progress";
                    case 2:
                        return Language.dang_cho; //"On Hold";
                    case 3:
                        return Language.dang_cho_thong_tin_chi_tiet; //"Waiting for Details";
                    case 4:
                        return Language.nghien_cuu; //"Researching";
                    case 5:
                        return Language.van_de_da_duoc_giai_quyet; //"Problem Solved";
                    case 1000:
                        return Language.cung_cap_thong_tin; //"Information Provided";
                    case 6:
                        return Language.da_huy; //"Canceled";
                    case 2000:
                        return Language.hop_nhat; //"Merged"; 
                    default:
                        return "";
                }
            }
        }

        public string case_nameaccount { get; set; }
        public string case_nameaccontact { get; set; }
    }
}

