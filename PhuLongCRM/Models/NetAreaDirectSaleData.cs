using PhuLongCRM.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhuLongCRM.Models
{
    public class NetAreaDirectSaleData
    {
        public static List<NetAreaDirectSaleModel> NetAreaData()
        {
            return new List<NetAreaDirectSaleModel>()
            {
                new NetAreaDirectSaleModel("1",Language.string_under + " 60 "+Language.sqm,"60"),
                new NetAreaDirectSaleModel("2","60 "+Language.sqm + " -> 80 "+Language.sqm,"60","80"),
                new NetAreaDirectSaleModel("3","81 "+Language.sqm + " -> 100 "+Language.sqm,"81","100"),
                new NetAreaDirectSaleModel("4","101 "+Language.sqm + " -> 120 "+Language.sqm,"101","120"),
                new NetAreaDirectSaleModel("5","121 "+Language.sqm + " -> 150 "+Language.sqm,"121","150"),
                new NetAreaDirectSaleModel("6","151 "+Language.sqm + " -> 180 "+Language.sqm,"151","180"),
                new NetAreaDirectSaleModel("7","211 "+Language.sqm + " -> 240 "+Language.sqm,"211","240"),
                new NetAreaDirectSaleModel("8","241 "+Language.sqm + " -> 270 "+Language.sqm,"241","270"),
                new NetAreaDirectSaleModel("9","271 "+Language.sqm + " -> 300 "+Language.sqm,"271","300"),
                new NetAreaDirectSaleModel("10","301 "+Language.sqm + " -> 350 "+Language.sqm,"301","350"),
                new NetAreaDirectSaleModel("11",Language.string_more_than + " 350 "+Language.sqm,null,"350"),
            };
        }
        public static NetAreaDirectSaleModel GetNetAreaById(string Id)
        {
            return NetAreaData().SingleOrDefault(x => x.Id == Id);
        }
    }
    public class NetAreaDirectSaleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public NetAreaDirectSaleModel(string id,string name,string from = null,string to = null)
        {
            Id = id;
            Name = name;
            From = from;
            To = to;
        }
    }
}
