using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ReservationCoownerModel
    {
        public Guid bsd_coownerid { get; set; }
        public string bsd_name { get; set; }
        public string account_name { get; set; }
        public string contact_name { get; set; }
        public string customer { get; set; }
        public string bsd_relationship { get; set; }
        public string relationship_format { get { return bsd_relationship != string.Empty ? RelationshipCoOwnerData.GetRelationshipById(bsd_relationship)?.Label : null; } }
    }
}
