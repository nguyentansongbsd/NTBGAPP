using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class ErrorResponse
    {
        public Error error { get; set; }
    }
    public class Innererror
    {
        public string message { get; set; }
        public string type { get; set; }
        public string stacktrace { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public Innererror innererror { get; set; }
    }

    
}
