using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BE.Models.Entities.Base
{
    public class ProductBase : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        public long SupplierId { get; set; }
        public int UnitsInStock { get; set; }
        public long CategoryId { get; set; }
        public string ModelNumber { get; set; }
        public string ModelName { get; set; }
        public string ProductImage { get; set; }
        public string ProductImageLarge { get; set; }
        public string ProductImageThumb { get; set; }
        public bool IsFeatured { get; set; }
        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; set; }
    }
}
