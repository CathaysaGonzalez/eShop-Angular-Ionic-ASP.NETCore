using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Models.ViewModels
{
    public class CheckoutState
    {
        public string name { get; set; }
        public string address { get; set; }
        public string cardNumber { get; set; }
        public string cardExpiry { get; set; }
        public string cardSecurityCode { get; set; }
    }
}
