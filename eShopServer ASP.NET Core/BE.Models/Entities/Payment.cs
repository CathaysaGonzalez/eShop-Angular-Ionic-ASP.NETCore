using BE.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BE.Models.Entities
{
    [Table("Payments")]
    public class Payment: PaymentBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(OrderId))]
        public Order OrderNavigation { get; set; }
    }
}
