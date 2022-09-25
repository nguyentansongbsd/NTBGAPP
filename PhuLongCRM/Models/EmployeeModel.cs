using System;
namespace PhuLongCRM.Models
{
    public class EmployeeModel
    {
        public Guid bsd_employeeid { get; set; }
        public string bsd_name { get; set; }
        public string bsd_avatar { get; set; }
        public string bsd_password { get; set; }
        public string bsd_imeinumber { get; set; }
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public Guid manager_id { get; set; }
        public string manager_name { get; set; }
        public DateTime createdon { get; set; }
    }
}
