using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class ViewData
    {
        public static OptionSetFilter GetViewById(string viewId)
        {
            var view = Views().SingleOrDefault(x => x.Val == viewId);
            return view;
        }

        public static string GetViewByIds(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return null;
            List<string> list = new List<string>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                var i = GetViewById(item);
                if (i != null)
                    list.Add(i.Label);
            }
            return string.Join(", ", list);
        }

        public static List<OptionSetFilter> Views()
        {
            return new List<OptionSetFilter>() {
                new OptionSetFilter(){ Val="100000000",Label=Language.thanh_pho},
                new OptionSetFilter(){ Val="100000001",Label=Language.be_boi },  // hồ bơi
                new OptionSetFilter(){ Val="100000002",Label=Language.cong_vien},
                new OptionSetFilter(){ Val="100000003",Label=Language.mat_tien},
                new OptionSetFilter(){ Val="100000004",Label=Language.san_vuon},
                new OptionSetFilter(){ Val="100000006",Label=Language.xa_lo},
                new OptionSetFilter(){ Val="100000007",Label=Language.ho}, //lake
                new OptionSetFilter(){ Val="100000008",Label=Language.song},
                new OptionSetFilter(){ Val="100000009",Label=Language.bien},
                new OptionSetFilter(){ Val="100000010",Label=Language.mot_mat_thoang}, 
                new OptionSetFilter(){ Val="100000011",Label=Language.hai_mat_thoang},
                new OptionSetFilter(){ Val="100000012",Label=Language.ho_boi}, // pool
            };
        }
    }
}
