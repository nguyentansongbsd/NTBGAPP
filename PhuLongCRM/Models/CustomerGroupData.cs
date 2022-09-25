using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class CustomerGroupData
    {
        public static List<OptionSet> CustomerGroups()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000", Language.nha_o_can_ho),
                new OptionSet("100000001",Language.nghi_duong),
                new OptionSet("100000002",Language.chua_xac_dinh),
            };
        }

        public static OptionSet GetCustomerGroupById(string Id)
        {
            return CustomerGroups().SingleOrDefault(x => x.Val == Id);
        }
    }
}
