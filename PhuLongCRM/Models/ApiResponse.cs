using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ApiResponse<T> where T : class
    {
        public List<T> value { get; set; }
    }
}
