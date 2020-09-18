using BE.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Initialization
{
    public class Data
    {
        public static IEnumerable<Category> GetAllCategories() => new List<Category>
        {
            new Category()
            {
                Name = "Free time"
            },
            new Category()
            {
                Name = "Home"
            },
            new Category()
            {
                Name = "DIY"
            },
            new Category()
            {
                Name = "Stationary"
            },
            new Category()
            {
                Name = "Electronics"
            }
        };

        public static IEnumerable<Supplier> GetSuppliers(IList<Category> categories) => new List<Supplier>
        {
            new Supplier
            {
                Name = "Supplier 1",
                Address = "Address 1",
                City = "City 1",
                Products = new List<Product>
                {
                    //1
                    new Product
                    {
                        Name = "Batteries",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 5M,
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
                        UnitsInStock = 200,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/batteries_md.jpg",
                        ProductImageLarge = "assets/batteries_lg.jpg",
                        ProductImageThumb = "assets/batteries_sm.jpg",
                        CategoryNavigation = categories[2],
                        IsFeatured = true,
                        CurrentPrice = 5M
                    },
                    //2
                    new Product
                    {
                        Name = "Sled",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 100M,
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
                        UnitsInStock = 100,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/sled_md.jpg",
                        ProductImageLarge = "assets/sled_lg.jpg",
                        ProductImageThumb = "assets/sled_sm.jpg",
                        CategoryNavigation = categories[0],
                        IsFeatured = false,
                        CurrentPrice = 200M
                    }
                }
            },

           
            new Supplier
            {
                Name = "Supplier 2",
                Address = "Address 2",
                City = "City 2",
                Products = new List<Product>
                {
                    //3
                    //new Product
                    //{
                    //    Name = "Case",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/case_md.jpg",
                    //    ProductImageLarge = "assets/case_lg.jpg",
                    //    ProductImageThumb = "assets/case_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.50M
                    //},
                    //4
                    //new Product
                    //{
                    //    Name = "Color pencils",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 12.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/colorpencils_md.jpg",
                    //    ProductImageLarge = "assets/colorpencils_lg.jpg",
                    //    ProductImageThumb = "assets/colorpencils_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 12.50M
                    //},
                    //5
                    //new Product
                    //{
                    //    Name = "Container",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 4.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/container_md.jpg",
                    //    ProductImageLarge = "assets/container_lg.jpg",
                    //    ProductImageThumb = "assets/container_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 4.50M
                    //},
                    //6
                    new Product
                    {
                        Name = "Cushion",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 14.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/cushion_md.jpg",
                        ProductImageLarge = "assets/cushion_lg.jpg",
                        ProductImageThumb = "assets/cushion_sm.jpg",
                        CategoryNavigation = categories[1],
                        IsFeatured = true,
                        CurrentPrice = 14.50M
                    },
                    //7
                    new Product
                    {
                        Name = "Cushion",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 18.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/cushion2_md.jpg",
                        ProductImageLarge = "assets/cushion2_lg.jpg",
                        ProductImageThumb = "assets/cushion2_sm.jpg",
                        CategoryNavigation = categories[1],
                        IsFeatured = true,
                        CurrentPrice = 18.50M
                    },
                    //8
                    new Product
                    {
                        Name = "Flashlight",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 15.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/flashlight_md.jpg",
                        ProductImageLarge = "assets/flashlight_lg.jpg",
                        ProductImageThumb = "assets/flashlight_sm.jpg",
                        CategoryNavigation = categories[2],
                        IsFeatured = true,
                        CurrentPrice = 15.50M
                    },
                    //9
                    new Product
                    {
                        Name = "Gloves",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 30.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/gloves_md.jpg",
                        ProductImageLarge = "assets/gloves_lg.jpg",
                        ProductImageThumb = "assets/gloves_sm.jpg",
                        CategoryNavigation = categories[0],
                        IsFeatured = true,
                        CurrentPrice = 30.50M
                    },
                    //10
                    new Product
                    {
                        Name = "Gloves",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 20.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/gloves2_md.jpg",
                        ProductImageLarge = "assets/gloves2_lg.jpg",
                        ProductImageThumb = "assets/gloves2_sm.jpg",
                        CategoryNavigation = categories[0],
                        IsFeatured = true,
                        CurrentPrice = 20.50M
                    },
                    //11
                    new Product
                    {
                        Name = "Glue",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 5.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/glue_md.jpg",
                        ProductImageLarge = "assets/glue_lg.jpg",
                        ProductImageThumb = "assets/glue_sm.jpg",
                        CategoryNavigation = categories[3],
                        IsFeatured = true,
                        CurrentPrice = 5.50M
                    },
                    //12
                    //new Product
                    //{
                    //    Name = "Glue",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 6.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/glue2_md.jpg",
                    //    ProductImageLarge = "assets/glue2_lg.jpg",
                    //    ProductImageThumb = "assets/glue2_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 6.50M
                    //},
                    //13
                    //new Product
                    //{
                    //    Name = "Headphones",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/headphones_md.jpg",
                    //    ProductImageLarge = "assets/headphones_lg.jpg",
                    //    ProductImageThumb = "assets/headphones_sm.jpg",
                    //    CategoryNavigation = categories[4],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.50M
                    //},
                    //14
                    new Product
                    {
                        Name = "Headphones",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 12.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/headphones2_md.jpg",
                        ProductImageLarge = "assets/headphones2_lg.jpg",
                        ProductImageThumb = "assets/headphones2_sm.jpg",
                        CategoryNavigation = categories[4],
                        IsFeatured = true,
                        CurrentPrice = 12.50M
                    },
                    //15
                    //new Product
                    //{
                    //    Name = "Headphones",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 15.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/headphones3_md.jpg",
                    //    ProductImageLarge = "assets/headphones3_lg.jpg",
                    //    ProductImageThumb = "assets/headphones3_sm.jpg",
                    //    CategoryNavigation = categories[4],
                    //    IsFeatured = true,
                    //    CurrentPrice = 15.50M
                    //},
                    //16
                    //new Product
                    //{
                    //    Name = "Jacket",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 50.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/jacket_md.jpg",
                    //    ProductImageLarge = "assets/jacket_lg.jpg",
                    //    ProductImageThumb = "assets/jacket_sm.jpg",
                    //    CategoryNavigation = categories[0],
                    //    IsFeatured = true,
                    //    CurrentPrice = 50.50M
                    //},
                    //17
                    new Product
                    {
                        Name = "Markers",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 20.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/markers_md.jpg",
                        ProductImageLarge = "assets/markers_lg.jpg",
                        ProductImageThumb = "assets/markers_sm.jpg",
                        CategoryNavigation = categories[3],
                        IsFeatured = true,
                        CurrentPrice = 20.50M
                    },
                    //18
                    //new Product
                    //{
                    //    Name = "Tappers",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/markers2_md.jpg",
                    //    ProductImageLarge = "assets/markers2_lg.jpg",
                    //    ProductImageThumb = "assets/markers2_sm.jpg",
                    //    CategoryNavigation = categories[1],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.50M
                    //},
                    //19
                    new Product
                    {
                        Name = "Mat",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 40.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/mat_md.jpg",
                        ProductImageLarge = "assets/mat_lg.jpg",
                        ProductImageThumb = "assets/mat_sm.jpg",
                        CategoryNavigation = categories[0],
                        IsFeatured = true,
                        CurrentPrice = 40.50M
                    },
                    //20
                    new Product
                    {
                        Name = "Notebook",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 8.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/notebook_md.jpg",
                        ProductImageLarge = "assets/notebook_lg.jpg",
                        ProductImageThumb = "assets/notebook_sm.jpg",
                        CategoryNavigation = categories[3],
                        IsFeatured = true,
                        CurrentPrice = 8.50M
                    },
                    //21
                    new Product
                    {
                        Name = "Notebook",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 10.50m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/notebook2_md.jpg",
                        ProductImageLarge = "assets/notebook2_lg.jpg",
                        ProductImageThumb = "assets/notebook2_sm.jpg",
                        CategoryNavigation = categories[3],
                        IsFeatured = true,
                        CurrentPrice = 10.50M
                    },
                    //22
                    //new Product
                    //{
                    //    Name = "Pen",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/pen_md.jpg",
                    //    ProductImageLarge = "assets/pen_lg.jpg",
                    //    ProductImageThumb = "assets/pen_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.50M
                    //},
                    //23
                    //new Product
                    //{
                    //    Name = "Pencils",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 6.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/pencils_md.jpg",
                    //    ProductImageLarge = "assets/pencils_lg.jpg",
                    //    ProductImageThumb = "assets/pencils_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 6.50M
                    //},
                    //24
                    //new Product
                    //{
                    //    Name = "Pencils",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 5.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/pencils2_md.jpg",
                    //    ProductImageLarge = "assets/pencils2_lg.jpg",
                    //    ProductImageThumb = "assets/pencils2_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 5.50M
                    //},
                    //25
                    //new Product
                    //{
                    //    Name = "Picture frame",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 11.50m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Modelo",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/pictureframe_md.jpg",
                    //    ProductImageLarge = "assets/pictureframe_lg.jpg",
                    //    ProductImageThumb = "assets/pictureframe_sm.jpg",
                    //    CategoryNavigation = categories[1],
                    //    IsFeatured = true,
                    //    CurrentPrice = 11.50M
                    //},
                    //26
                    new Product
                    {
                        Name = "Pot",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 30m,
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
                        UnitsInStock = 20,
                        ModelName = "Modelo",
                        ModelNumber = "123456",
                        ProductImage = "assets/pot_md.jpg",
                        ProductImageLarge = "assets/pot_lg.jpg",
                        ProductImageThumb = "assets/pot_sm.jpg",
                        CategoryNavigation = categories[1],
                        IsFeatured = true,
                        CurrentPrice = 30M
                    },
                    //27
                    new Product
                    {
                        Name = "Screwdrivers",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 20m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/screwdrivers_md.jpg",
                        ProductImageLarge = "assets/screwdrivers_lg.jpg",
                        ProductImageThumb = "assets/screwdrivers_sm.jpg",
                        CategoryNavigation = categories[2],
                        IsFeatured = true,
                        CurrentPrice = 20M
                    },
                    //28
                    new Product
                    {
                        Name = "Skate",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 80.95m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/skate_md.jpg",
                        ProductImageLarge = "assets/skate_lg.jpg",
                        ProductImageThumb = "assets/skate_sm.jpg",
                        CategoryNavigation = categories[0],
                        IsFeatured = true,
                        CurrentPrice = 80.95M
                    },
                    //29
                    new Product
                    {
                        Name = "Smoke detector",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 80.95m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/smokedetector_md.jpg",
                        ProductImageLarge = "assets/smokedetector_lg.jpg",
                        ProductImageThumb = "assets/smokedetector_sm.jpg",
                        CategoryNavigation = categories[2],
                        IsFeatured = true,
                        CurrentPrice = 80.95M
                    },
                    //30
                    //new Product
                    //{
                    //    Name = "Stapler",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 6.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Modelo",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/stapler_md.jpg",
                    //    ProductImageLarge = "assets/stapler_lg.jpg",
                    //    ProductImageThumb = "assets/stapler_sm.jpg",
                    //    CategoryNavigation = categories[3],
                    //    IsFeatured = true,
                    //    CurrentPrice = 6.95M
                    //},
                    //31
                    //new Product
                    //{
                    //    Name = "Suitcase",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 40.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Modelo",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/suitcase_md.jpg",
                    //    ProductImageLarge = "assets/suitcase_lg.jpg",
                    //    ProductImageThumb = "assets/suitcase_sm.jpg",
                    //    CategoryNavigation = categories[0],
                    //    IsFeatured = true,
                    //    CurrentPrice = 40.95M
                    //},
                    //32
                    //new Product
                    //{
                    //    Name = "Tappers",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/suitcase_md.jpg",
                    //    ProductImageLarge = "assets/suitcase_lg.jpg",
                    //    ProductImageThumb = "assets/suitcase_sm.jpg",
                    //    CategoryNavigation = categories[1],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.95M
                    //},
                    //33
                    //new Product
                    //{
                    //    Name = "Tappers",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 10.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/tapper2_md.jpg",
                    //    ProductImageLarge = "assets/tapper2_lg.jpg",
                    //    ProductImageThumb = "assets/tapper2_sm.jpg",
                    //    CategoryNavigation = categories[1],
                    //    IsFeatured = true,
                    //    CurrentPrice = 10.95M
                    //},
                    //34
                    //new Product
                    //{
                    //    Name = "Tent",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 150.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/tent_md.jpg",
                    //    ProductImageLarge = "assets/tent_lg.jpg",
                    //    ProductImageThumb = "assets/tent_sm.jpg",
                    //    CategoryNavigation = categories[0],
                    //    IsFeatured = true,
                    //    CurrentPrice = 150.95M
                    //},
                    //35
                    //new Product
                    //{
                    //    Name = "Tools",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 40.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/tools_md.jpg",
                    //    ProductImageLarge = "assets/tools_lg.jpg",
                    //    ProductImageThumb = "assets/tools_sm.jpg",
                    //    CategoryNavigation = categories[2],
                    //    IsFeatured = true,
                    //    CurrentPrice = 40.95M
                    //},
                    //36
                    //new Product
                    //{
                    //    Name = "Usb charger",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 12.95m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/usbcharger_md.jpg",
                    //    ProductImageLarge = "assets/usbcharger_lg.jpg",
                    //    ProductImageThumb = "assets/usbcharger_sm.jpg",
                    //    CategoryNavigation = categories[4],
                    //    IsFeatured = true,
                    //    CurrentPrice = 12.95M
                    //},
                    //37
                    //new Product
                    //{
                    //    Name = "Usb cable",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 8.5m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/usbwire_md.jpg",
                    //    ProductImageLarge = "assets/usbwire_lg.jpg",
                    //    ProductImageThumb = "assets/usbwire_sm.jpg",
                    //    CategoryNavigation = categories[4],
                    //    IsFeatured = true,
                    //    CurrentPrice = 8.5M
                    //},
                    //38
                    new Product
                    {
                        Name = "Watering can",
                        Description =
                            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        Price = 5.5m,
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
                        UnitsInStock = 20,
                        ModelName = "Model",
                        ModelNumber = "123456",
                        ProductImage = "assets/wateringcan_md.jpg",
                        ProductImageLarge = "assets/wateringcan_lg.jpg",
                        ProductImageThumb = "assets/wateringcan_sm.jpg",
                        CategoryNavigation = categories[1],
                        IsFeatured = true,
                        CurrentPrice = 5.5M
                    },
                    //39
                    //new Product
                    //{
                    //    Name = "Cables",
                    //    Description =
                    //        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    //    Price = 8m,
                    //    Ratings = new List<Rating>
                    //    {
                    //        new Rating
                    //        {
                    //            Stars = 1
                    //        },
                    //        new Rating
                    //        {
                    //            Stars = 3
                    //        }
                    //    },
                    //    UnitsInStock = 20,
                    //    ModelName = "Model",
                    //    ModelNumber = "123456",
                    //    ProductImage = "assets/wires_md.jpg",
                    //    ProductImageLarge = "assets/wires_lg.jpg",
                    //    ProductImageThumb = "assets/wires_sm.jpg",
                    //    CategoryNavigation = categories[2],
                    //    IsFeatured = true,
                    //    CurrentPrice = 8M
                    //},

                }
                }
        };


        public static IEnumerable<Order> GetAllOrders(IList<Product> products) => new List<Order>
        {
            new Order
            {
                Name = "NameCustomer2",
                Address = "AddressCustomer2",
                Shipped = false,
                Total = 815.00m,
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
                       CardNumber = "111",
                       CardExpiry = "222",
                       CardSecurityCode = "333"
                },
                UserName = "User"
            },
            new Order
            {
                Name = "NameCustomer1",
                Address = "AddressCustomer1",
                Shipped = false,
                Total = 610.00m,
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
                       CardNumber = "444",
                       CardExpiry = "555",
                       CardSecurityCode = "666"
                },
                UserName = "User"
            }
        };
    }
}
