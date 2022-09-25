using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class CustomerLookUp : LookUp
    {
        /// <summary>
        /// 1: contact
        /// 2: account
        /// </summary>
        public int Type { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
