using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class RequiredMeetModel
    {
        public string required_account { get; set; }
        public string required_contact { get; set; }
        public string required_lead { get; set; }
        public string required
        {
            get
            {
                if (!string.IsNullOrEmpty(required_account))
                    return required_account;
                else if (!string.IsNullOrEmpty(required_contact))
                    return required_contact;
                else if (!string.IsNullOrEmpty(required_lead))
                    return required_lead;
                else
                    return null;
            }
        }
    }
}
