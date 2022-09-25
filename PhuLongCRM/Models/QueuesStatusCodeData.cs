using System;
using System.Collections.Generic;
using System.Linq;
using PhuLongCRM.Resources;

namespace PhuLongCRM.Models
{
    public class QueuesStatusCodeData
    {
        public static QueuesStatusCodeModel GetQueuesById(string id)
        {
            return GetQueuesData().SingleOrDefault(x => x.Id == id);
        }
        public static List<QueuesStatusCodeModel> GetQueuesByIds(string ids)
        {
            List<QueuesStatusCodeModel> listQueue = new List<QueuesStatusCodeModel>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                listQueue.Add(GetQueuesById(item));
            }
            return listQueue;
        }
        public static List<QueuesStatusCodeModel> GetQueuesData()
        {
            return new List<QueuesStatusCodeModel>()
            {
                new QueuesStatusCodeModel("1",Language.nhap,"#808080"), // Draft
                new QueuesStatusCodeModel("2",Language.dang_cho,"#808080"), // On hold
                new QueuesStatusCodeModel("3",Language.thanh_cong,"#808080"), // Won
                new QueuesStatusCodeModel("4",Language.da_huy,"#808080"),
                new QueuesStatusCodeModel("5",Language.het_hang,"#808080"), //Out-Sold
                new QueuesStatusCodeModel("100000000",Language.giu_cho,"#00CF79"),
                new QueuesStatusCodeModel("100000002",Language.dang_doi,"#FDC206"),
                new QueuesStatusCodeModel("100000003",Language.het_han,"#B3B3B3"),
                new QueuesStatusCodeModel("100000004",Language.hoan_thanh,"#C50147"),
                new QueuesStatusCodeModel("100000008",Language.xac_nhan_huy,"#808080"),
                new QueuesStatusCodeModel("100000009",Language.huy_gg_chua_hoan_tien,"#808080"), 
                new QueuesStatusCodeModel("100000010",Language.huy_gg_da_hoan_tien,"#808080"),
                new QueuesStatusCodeModel("0","","#808080")
            };
        }
    }

    public class QueuesStatusCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BackGroundColor { get; set; }
        public QueuesStatusCodeModel(string id,string name,string backGroundColor)
        {
            Id = id;
            Name = name;
            BackGroundColor = backGroundColor;
        }
    }
}
