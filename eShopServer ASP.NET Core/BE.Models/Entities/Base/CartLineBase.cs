using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BE.Models.Entities.Base
{
    public class CartLineBase: EntityBase
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long OrderId { get; set; }
        public decimal LineItemTotal { get; set; }
    }
}
