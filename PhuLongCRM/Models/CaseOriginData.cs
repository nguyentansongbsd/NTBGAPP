using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class CaseOriginData
    {
        public static List<OptionSet> Origins()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1","Phone"),
                new OptionSet("2","Email"),
                new OptionSet("3","Web"),
                new OptionSet("2483","Facebook"),
                new OptionSet("3986","Twitter"),
                new OptionSet("700610000","IoT"),
                new OptionSet("0","")
            };
        }

        public static OptionSet GetOriginById(string id)
        {
            return Origins().SingleOrDefault(x => x.Val == id);
        }
    }
}