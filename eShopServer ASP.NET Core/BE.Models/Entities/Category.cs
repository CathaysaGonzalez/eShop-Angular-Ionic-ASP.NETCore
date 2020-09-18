using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BE.Models.Entities.Base;
using Newtonsoft.Json;

namespace BE.Models.Entities
{
    [Table("Categories")]
    public class Category: CategoryBase
    {
        [JsonIgnore]
        [InverseProperty(nameof(Product.CategoryNavigation))]
        public IEnumerable<Product> Products { get; set; }
    }
}
