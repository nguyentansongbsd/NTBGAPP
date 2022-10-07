using System;
namespace PhuLongCRM.Models
{
    public class ConfirmDocumentForPinkBookDetailModel
    {
        public Guid bsd_confirmdocumentforpinbookdetailid { get; set; }
        public string bsd_name { get; set; }
        public bool bsd_hasidcardpassport { get; set; }
        public bool bsd_hascontract { get; set; }
        public bool bsd_hasvatinvoice { get; set; }
    }
}
