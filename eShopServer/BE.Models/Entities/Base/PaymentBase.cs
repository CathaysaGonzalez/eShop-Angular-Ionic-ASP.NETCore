using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BE.Models.Entities.Base
{
    public class PaymentBase: EntityBase
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardExpiry { get; set; }
        [Required]
        public string CardSecurityCode { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Total { get; set; }
        public string AuthCode { get; set; }
        public long OrderId { get; set; }
    }
}
