using System;
using System.Collections.Generic;
using BE.Models.Entities;
using BE.Models.Entities.Base;
using BE.Models.ViewModels;

namespace BE.Dal.Initialization
{
    public static class SampleData
    {

            public static IEnumerable<Category> GetAllCategories() => new List<Category>
            {
                new Category()
                {
                    //Id = 1,
                    Name = "Category1"
                },
                new Category()
                {
                    //Id = 2,
                    Name = "Category2"
                }
            };

            public static IEnumerable<Supplier> GetSuppliers(IList<Category> categories) => new List<Supplier>
            {
                new Supplier
                {
                    Name = "NameSupplier1",
                    Address = "AddressSupplier1",
                    City = "CitySupplier1",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Name = "NameProduct1",
                            Description = "Description1",
                            Price = 275,
                            Ratings = new List<Rating>
                            {
                                new Rating
                                {
                                    Stars = 4
                                },
                                new Rating
                                {
                                    Stars = 3
                                }
                            },
                            UnitsInStock = 7,
                            CategoryNavigation = categories[0]
                        },
                        new Product
                        {
                            Name = "NameProduct2",
                            Description = "Description2",
                            Price = 48.95m,
                            Ratings = new List<Rating>
                            {
                                new Rating
                                {
                                    Stars = 2
                                },
                                new Rating
                                {
                                    Stars = 5
                                }
                            },
                            UnitsInStock = 4,
                            CategoryNavigation = categories[1]
                        }
                    }
                },
                new Supplier
                {
                    Name = "NameSupplier2",
                    Address = "AddressSupplier2",
                    City = "CitySupplier2",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Name = "NameProduct3",
                            Description = "Description3",
                            //Category = "Category3",
                            Price = 19.50m,
                            Ratings = new List<Rating>
                            {
                                new Rating
                                {
                                    Stars = 1
                                },
                                new Rating
                                {
                                    Stars = 3
                                }
                            },
                            UnitsInStock = 5,
                            CategoryNavigation = categories[1]
                        }
                    }
                }
            };


            public static IEnumerable<Order> GetAllOrders(IList<Product> products) => new List<Order>
            {
                new Order
                {
                    Name = "NameCustomer1",
                    Address = "AddressCustomer1",
                    Shipped = false,
                    CartLines = new List<CartLine>
                    {
                        new CartLine
                        {
                            ProductNavigation = products[0],
                            Quantity = 2,
                        },
                        new CartLine
                        {
                            ProductNavigation = products[1],
                            Quantity = 3,
                        }
                    },
                    PaymentNavigation = new Payment()
                    {
                        CardNumber = "111",
                        CardExpiry = "222",
                        CardSecurityCode = "333"
                    }
                },
                new Order
                {
                    Name = "NameCustomer2",
                    Address = "AddressCustomer2",
                    Shipped = false,
                    CartLines = new List<CartLine>
                    {
                        new CartLine
                        {
                            ProductNavigation = products[0],
                            Quantity = 3,
                        },
                        new CartLine
                        {
                            ProductNavigation = products[1],
                            Quantity = 4,
                        }
                    },
                    PaymentNavigation = new Payment()
                    {
                        CardNumber = "444",
                        CardExpiry = "555",
                        CardSecurityCode = "666"
                    }
                }
            };
        }
}