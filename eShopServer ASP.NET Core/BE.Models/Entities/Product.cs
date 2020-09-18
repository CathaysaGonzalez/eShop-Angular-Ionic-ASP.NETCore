using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using BE.Models.Entities.Base;


namespace BE.Models.Entities
{
  [Table("Products")]
  public class Product : ProductBase
  {
        [JsonIgnore]
        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Rating.ProductNavigation))]
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        [JsonIgnore]
        [InverseProperty(nameof(CartLine.ProductNavigation))]
        public List<CartLine> CartLines { get; set; } = new List<CartLine>();
        [JsonIgnore]
        [ForeignKey(nameof(CategoryId))]
        public Category CategoryNavigation { get; set; }
    }
}