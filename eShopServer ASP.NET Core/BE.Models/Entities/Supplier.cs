using BE.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace BE.Models.Entities
{
    [Table("Suppliers")]
    public class Supplier : SupplierBase
    {
        [JsonIgnore]
        [InverseProperty(nameof(Product.Supplier))]
        public IEnumerable<Product> Products { get; set; }
    }
}
