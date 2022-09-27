using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AcceptanceTypeResult
    {
        public static List<OptionSet> AcceptanceTypeResultData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.acceptance_pass_typeresult),
                new OptionSet("100000001",Language.acceptance_error_typeresult),
                new OptionSet("100000002",Language.acceptance_pass_agree_to_request_typeresult),
                new OptionSet("100000003",Language.acceptance_pass_reject_request_typeresult),
            };
        }

        public static OptionSet GetAcceptanceTypeResultById(string Id)
        {
            return AcceptanceTypeResultData().SingleOrDefault(x => x.Val == Id);
        }
    }
}
