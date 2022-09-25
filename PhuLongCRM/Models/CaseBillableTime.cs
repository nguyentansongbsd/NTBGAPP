using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CaseBillableTime
    {
        public static List<OptionSet> CaseBillableTimeData()
        {
            return new List<OptionSet>()
            {
               new OptionSet("1", "1 "+ Language.case_phut_sts),
                new OptionSet("15", "15 "+ Language.case_phut_sts),
                new OptionSet("30", "30 "+ Language.case_phut_sts),
                new OptionSet("45", "45 "+ Language.case_phut_sts),
                new OptionSet("60", "1 "+ Language.case_gio_sts),
                new OptionSet("90", "1.5 "+ Language.case_gio_sts),
                new OptionSet("120", "2 "+ Language.case_gio_sts),
                new OptionSet("150", "2.5 "+ Language.case_gio_sts),
                 new OptionSet("180", "3 "+ Language.case_gio_sts),
                new OptionSet("210", "3.5 "+ Language.case_gio_sts),
                 new OptionSet("240", "4 "+ Language.case_gio_sts),
                new OptionSet("270", "4.5 "+ Language.case_gio_sts),
                 new OptionSet("300", "5 "+ Language.case_gio_sts),
                new OptionSet("330", "5.5 "+ Language.case_gio_sts),
                 new OptionSet("360", "6 "+ Language.case_gio_sts),
                new OptionSet("390", "6.5 "+ Language.case_gio_sts),
                 new OptionSet("420", "7 "+ Language.case_gio_sts),
                new OptionSet("450", "7.5 "+ Language.case_gio_sts),
                new OptionSet("480", "8 "+ Language.case_gio_sts),
                new OptionSet("1440", "1 "+ Language.case_ngay_sts),
                new OptionSet("2880", "2 "+ Language.case_ngay_sts),
                new OptionSet("4320", "3 "+ Language.case_ngay_sts),
            };
        }
    }
}
