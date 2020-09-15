using BE.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BE.Models.Entities
{
    [Table("CartLines")]
    public class CartLine: CartLineBase
    {
        [ForeignKey(nameof(ProductId))]
        public Product ProductNavigation { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order OrderNavigation { get; set; }
    }
}
