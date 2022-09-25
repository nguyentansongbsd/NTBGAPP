using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ReservationFormStatus
    {
        public static List<OptionSet> ReservationFormStatusData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.rf_not_print_rf_sts),
                new OptionSet("100000001",Language.rf_printed_rf_sts), 
                new OptionSet("100000002",Language.rf_signed_rf_sts),
                new OptionSet("100000003",Language.rf_expired_rf_sts), 
            };
        }

        public static OptionSet GetRFStatusById(string Id)
        {
            return ReservationFormStatusData().SingleOrDefault(x => x.Val == Id);
        }
    }
}
