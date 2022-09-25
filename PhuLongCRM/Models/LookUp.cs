using System;
using System.Collections.Generic;
using System.Text;

namespace PhuLongCRM.Models
{
    public class LookUp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is LookUp)) return false;
            var lookup = (LookUp)obj;
            return this.Id == lookup.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

    }
}
