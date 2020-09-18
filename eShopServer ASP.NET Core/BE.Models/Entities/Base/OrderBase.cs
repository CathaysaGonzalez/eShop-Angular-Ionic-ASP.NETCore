using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BE.Models.Entities.Base
{
    public class OrderBase : EntityBase
    {
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public bool Shipped { get; set; } = false;
        public decimal Total { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}


