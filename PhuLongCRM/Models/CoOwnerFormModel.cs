using PhuLongCRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CoOwnerFormModel 
    {
        public Guid bsd_coownerid { get; set; }
        public string bsd_name { get; set; }

        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public string bsd_relationshipId { get; set; }
        public string bsd_relationship { get; set; }

        public Guid reservation_id { get; set; }
        public string reservation_name { get; set; }
    }

}
