using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class AccountLocalization
    {
        public static List<OptionSet> LocalizationOptions;

        public static void Localizations()
        {
            LocalizationOptions = new List<OptionSet>()
            {
                new OptionSet("100000000", Language.account_trong_nuoc_sts), //account_trong_nuoc_sts Foreigner
                new OptionSet("100000001", Language.account_nuoc_ngoai_sts) //account_nuoc_ngoai_sts Local
            };
        }
        public static OptionSet GetLocalizationById(string Id)
        {
            Localizations();
            if (Id != string.Empty)
            {
                OptionSet optionSet = LocalizationOptions.Single(x => x.Val == Id);
                return optionSet;
            }
            return null;
        }
    }
}
