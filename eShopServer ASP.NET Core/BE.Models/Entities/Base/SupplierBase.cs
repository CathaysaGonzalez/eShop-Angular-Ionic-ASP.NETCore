using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Models.Entities.Base
{
    public class SupplierBase : EntityBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
