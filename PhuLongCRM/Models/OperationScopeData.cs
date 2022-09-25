using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class OperationScopeData
    {
        public static List<OptionSet> OperationScopes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.account_real_estate_os),// account_real_estate_os //Real Estate
                new OptionSet("100000001",Language.account_finance_os),// account_finance_os //Finance
                new OptionSet("100000002",Language.account_education_os),// account_education_os //Education
            };
        }

        public static OptionSet GetOperationScopeById(string Id)
        {
            return OperationScopes().SingleOrDefault(x => x.Val == Id);
        }
    }
}
