using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class LeadSourcesData
    {
        public static List<OptionSet> GetListSources()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.bao),//Newspapers
                new OptionSet("100000001",Language.staff_phu_long),
                new OptionSet("100000002",Language.thuong_hieu),//Trademark
                new OptionSet("100000003",Language.hot_line),
                new OptionSet("100000004",Language.vi_tri_hien_tai),//The current of the position
                new OptionSet("100000005",Language.khach_hang_cu_da_mua_gioi_thieu),//Old customers have bought referrals
                new OptionSet("1",Language.quang_cao),
                new OptionSet("2",Language.nhan_vien_gioi_thieu),
                new OptionSet("3",Language.gioi_thieu_ben_ngoai),
                new OptionSet("4",Language.doi_tac),
                new OptionSet("5",Language.quan_he_cong_chung),
                new OptionSet("6",Language.hoi_thao),
                new OptionSet("7",Language.trien_lam_thuong_mai),
                new OptionSet("8",Language.trang_web),
                new OptionSet("9",Language.truyen_mieng),
                new OptionSet("10",Language.khac),
            };
        }

        public static OptionSet GetLeadSourceById(string Id)
        {
            return GetListSources().SingleOrDefault(x => x.Val == Id);
        }
    }
}
