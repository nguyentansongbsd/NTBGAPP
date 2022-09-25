using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class RatingData
    {
        public static List<OptionSet> Ratings()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1","Hot"),
                new OptionSet("2","Warm"),
                new OptionSet("3","Cold"),
            };
        }

        public static OptionSet GetRatingById(string id)
        {
            return Ratings().SingleOrDefault(x => x.Val == id);
        }
    }
}
