using BE.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BE.Models.Entities
{
    [Table("Orders")]
    public class Order: OrderBase
    {
        [InverseProperty(nameof(Payment.OrderNavigation))]
        public Payment PaymentNavigation { get; set; }
        [InverseProperty(nameof(CartLine.OrderNavigation))]
        public IEnumerable<CartLine> CartLines { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser UserNavigation { get; set; }
    }
}
