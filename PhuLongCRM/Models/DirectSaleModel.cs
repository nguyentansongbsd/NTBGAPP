using System;
using System.Collections.Generic;

namespace PhuLongCRM.Models
{
    public class DirectSaleModel
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string sumQty { get; set; }
        public string stringQty { get; set; }
        public List<ListFloor> listFloor { get; set; }
    }
    public class ListFloor
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string sumQty { get; set; }
        public string stringQty { get; set; }
    }
}
