using System;

namespace PhuLongCRM.Models
{
    public class ListPhanHoiModel
    {
        public string customerid { get; set; }
        public string _customerid_value { get; set; }
        public string subjecttitle { get; set; }

        public int caseorigincode { get; set; }
        public string caseorigincodevalue => CaseOriginData.GetOriginById(caseorigincode.ToString()).Label;

        public string _productid_value { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
            }
        }
        public int statuscode { get; set; }
        public string statuscodevalue => CaseStatusCodeData.GetCaseStatusCodeById(this.statuscode.ToString()).Label;

        public Guid incidentid { get; set; }
        public string case_nameaccount { get; set; }
        public string case_namecontact { get; set; }
        public string productname { get; set; }
        public string contactname
        {
            get
            {
                if (this.case_nameaccount != null)
                {
                    return this.case_nameaccount;
                }
                else if (this.case_namecontact != null)
                {
                    return this.case_namecontact;
                }
                else
                {
                    return null;
                }
            }
        }

        public int casetypecode { get; set; }
        public string casetypecodevalue => CaseTypeData.GetCaseById(this.casetypecode.ToString()).Label;

        public Guid parentcaseid { get; set; }
    }
}

