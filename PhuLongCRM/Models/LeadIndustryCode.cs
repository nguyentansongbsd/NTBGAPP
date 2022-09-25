using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhuLongCRM.Models
{
    public class LeadIndustryCode
    {
        public static List<OptionSet> LeadIndustryCodeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet() { Val = ("1"), Label = Language.lead_1_industry, },
            //Accounting
             new OptionSet() { Val = ("2"), Label = Language.lead_2_industry, },
            //Agriculture and Non-petrol natural resource extraction
             new OptionSet() { Val = ("3"), Label = Language.lead_3_industry, },
            //Broadcasting printing and Publishing
             new OptionSet() { Val = ("4"), Label = Language.lead_4_industry, },
            //Brokers
             new OptionSet() { Val = ("5"), Label = Language.lead_5_industry, },
            //Building supply retail
             new OptionSet() { Val = ("6"), Label = Language.lead_6_industry, },
            //Business services
             new OptionSet() { Val = ("7"), Label = Language.lead_7_industry, },
            //Consulting
             new OptionSet() { Val = ("8"), Label = Language.lead_8_industry, },
            //Consumer services
             new OptionSet() { Val = ("9"), Label = Language.lead_9_industry, },
            //Design, direction and creative management
             new OptionSet() { Val = ("10"), Label = Language.lead_10_industry, },
            //Distributors, dispatchers and processors
             new OptionSet() { Val = ("11"), Label = Language.lead_11_industry, },
            //Doctor's offices and clinics
             new OptionSet() { Val = ("12"), Label = Language.lead_12_industry, },
            //Durable manufacturing
             new OptionSet() { Val = ("13"), Label = Language.lead_13_industry, },
            //Eating and drinking places
             new OptionSet() { Val = ("14"), Label = Language.lead_14_industry, },
            //Entertainment retail
             new OptionSet() { Val = ("15"), Label = Language.lead_15_industry, },
            //Equipment rental and leasing
             new OptionSet() { Val = ("16"), Label = Language.lead_16_industry, },
            //Financial
             new OptionSet() { Val = ("17"), Label = Language.lead_17_industry, },
            //Food and tobacco processing
             new OptionSet() { Val = ("18"), Label = Language.lead_18_industry, },
            //Inbound capital intensive processing
             new OptionSet() { Val = ("19"), Label = Language.lead_19_industry, },
            //Inbound repair and services
             new OptionSet() { Val = ("20"), Label = Language.lead_20_industry, },
            //Insurance
             new OptionSet() { Val = ("21"), Label = Language.lead_21_industry, },
            //Legal services
             new OptionSet() { Val = ("22"), Label = Language.lead_22_industry, },
            //Non-Durable merchandise retail
             new OptionSet() { Val = ("23"), Label = Language.lead_23_industry, },
            //Outbound consumer service
             new OptionSet() { Val = ("24"), Label = Language.lead_24_industry, },
            //Petrochemical extraction and distribution
             new OptionSet() { Val = ("25"), Label = Language.lead_25_industry, },
            //Service retail
             new OptionSet() { Val = ("26"), Label = Language.lead_26_industry, },
            //SIG affiliations
             new OptionSet() { Val = ("27"), Label = Language.lead_27_industry, },
            //Social services
             new OptionSet() { Val = ("28"), Label = Language.lead_28_industry, },
            //Special outbound trade contractors
             new OptionSet() { Val = ("29"), Label = Language.lead_29_industry, },
            //Specialty realty
             new OptionSet() { Val = ("30"), Label = Language.lead_30_industry, },
            //Transportation
             new OptionSet() { Val = ("31"), Label = Language.lead_31_industry, },
            //Utility creation and distribution
             new OptionSet() { Val = ("32"), Label = Language.lead_32_industry, },
            //Vehicle retail
             new OptionSet() { Val = ("33"), Label = Language.lead_33_industry, },
            };
        }

        public static OptionSet GetIndustryCodeById(string Id)
        {
            return LeadIndustryCodeData().SingleOrDefault(x => x.Val == Id);
        }
    }
}
