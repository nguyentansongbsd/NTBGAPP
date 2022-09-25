using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
	public class InterestChargeStatusData
	{
		public static List<OptionSet> GetInterstChargeData()
        {
			return new List<OptionSet>()
			{
				new OptionSet("1","Active"),//Active
				new OptionSet("100000000",Language.chua_thanh_toan),//Not Paid
				new OptionSet("100000001",Language.da_thanh_toan),//Paid
				new OptionSet("2","Inactive"),//Inactive
				new OptionSet("0",""),
			};
        }

		public static OptionSet GetInterestChargeStatusById(string id)
        {
			return GetInterstChargeData().SingleOrDefault(x => x.Val == id);
        }
	}
}

