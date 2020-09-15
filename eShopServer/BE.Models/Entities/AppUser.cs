using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace BE.Models.Entities
{
    public class AppUser :IdentityUser
    {
        [JsonIgnore]
        [InverseProperty(nameof(Order.UserNavigation))]
        public IEnumerable<Order> Orders { get; set; }
        [JsonIgnore]
        public string Role { get; set; }
    }
}
