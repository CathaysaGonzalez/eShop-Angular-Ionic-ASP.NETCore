using System;
using System.Collections.Generic;
using System.Text;
using BE.Models.Entities;

namespace BE.Models.ViewModels
{
    public class ProductData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
        public long SupplierId { get; set; }
        public long CategoryId { get; set; }
        public string ModelNumber { get; set; }
        public string ModelName { get; set; }
        public string ProductImage { get; set; }
        public string ProductImageLarge { get; set; }
        public string ProductImageThumb { get; set; }
        public bool IsFeatured { get; set; }
        public decimal CurrentPrice { get; set; }
        public Product Product => new Product
        {
            Name = Name,
            Description = Description,
            Price = Price,
            UnitsInStock = UnitsInStock,
            SupplierId = SupplierId,
            CategoryId = CategoryId,
            ModelNumber = ModelNumber,
            ModelName = ModelName,
            ProductImage = ProductImage,
            ProductImageLarge = ProductImageLarge,
            ProductImageThumb = ProductImageThumb,
            IsFeatured = IsFeatured,
            CurrentPrice = CurrentPrice
        };

    }
}
