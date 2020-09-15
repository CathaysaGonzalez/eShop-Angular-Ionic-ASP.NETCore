using BE.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Models.ViewModels
{
    public class ProductSelection
    {
        public new long id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
