using BE.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Models.ViewModels
{
    public class OrderConfirmation : Order
    {
        public new long Id { get; set; }
        public string AuthCode { get; set; }
        public decimal Total { get; set; }
    }
}
