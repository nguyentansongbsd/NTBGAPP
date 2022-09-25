using System;
namespace PhuLongCRM.Models
{
	public class UserModel
	{
        public Guid systemuserid { get; set; }
        public string fullname { get; set; }
        public string domainname { get; set; }
        public string internalemailaddress { get; set; }
        public string mobilephone { get; set; }
    }
}

