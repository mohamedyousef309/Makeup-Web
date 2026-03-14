using Domain_Layer.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.Seeders
{
    public static class DbSeeder
    {

        public static async Task SeedDataAsync(AppDbContext DBContext)
        {
            await DBContext.Database.MigrateAsync();

            await seedAttributesAsync(DBContext);
            await seedCategoriesAsync(DBContext);
            await seedProductsAndVariantsAsync(DBContext);
        }

        private static async Task seedAttributesAsync(AppDbContext context)
        {

            if (await context.Attributes.AnyAsync()) return;

            var attributes = new List<Domain_Layer.Entites.Attribute>
            {
                new Domain_Layer.Entites.Attribute { Id = 1, Name = "Color" },
                new Domain_Layer.Entites.Attribute { Id = 2, Name = "Size" }
            };


            await context.Database.OpenConnectionAsync();
            try
            {
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Attributes] ON");
                await context.Attributes.AddRangeAsync(attributes);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Attributes] OFF");

                // نكرر للجدول التاني
                var attributeValues = new List<AttributeValue>
                {
                // Colors
                new AttributeValue { Id = 1, AttributeId = 1, Value = "Black" },
                new AttributeValue { Id = 2, AttributeId = 1, Value = "White" },
                new AttributeValue { Id = 3, AttributeId = 1, Value = "Red" },
                new AttributeValue { Id = 4, AttributeId = 1, Value = "Wine" },
                new AttributeValue { Id = 5, AttributeId = 1, Value = "Blue" },
                new AttributeValue { Id = 6, AttributeId = 1, Value = "Green" },
                new AttributeValue { Id = 7, AttributeId = 1, Value = "Yellow" },
                new AttributeValue { Id = 8, AttributeId = 1, Value = "Gray" },
                new AttributeValue { Id = 9, AttributeId = 1, Value = "Brown" },
                // Sizes
                new AttributeValue { Id = 10, AttributeId = 2, Value = "S" },
                new AttributeValue { Id = 11, AttributeId = 2, Value = "M" },
                new AttributeValue { Id = 12, AttributeId = 2, Value = "L" },
                new AttributeValue { Id = 13, AttributeId = 2, Value = "XL" },
                new AttributeValue { Id = 14, AttributeId = 2, Value = "XXL" }
                };

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [AttributeValues] ON");
                await context.AttributeValues.AddRangeAsync(attributeValues);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [AttributeValues] OFF");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }

        private static async Task seedCategoriesAsync(AppDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
              new Category { Id = 1, Name = "Tops", Description = "Shirts, t-shirts, hoodies, and all upper body clothing" },
              new Category { Id = 2, Name = "Bottoms", Description = "Pants, jeans, shorts, and all lower body clothing" },
             new Category { Id = 3, Name = "Accessories", Description = "Bags, hats, belts, and other fashion accessories" }
            };

            await context.Database.OpenConnectionAsync();
            try
            {
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Categories] ON");

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Categories] OFF");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }

        private static async Task seedProductsAndVariantsAsync(AppDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            await context.Database.OpenConnectionAsync();
            try
            {
                // 1. Products
                var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Oversized Zip-up Hoodie", Description = "Premium oversized hoodie with a full-length silver zipper.", CategoryId = 1, IsActive = true },
                    new Product { Id = 2, Name = "Batman Graphic Zip-up Hoodie", Description = "Urban streetwear with Batman logo.", CategoryId = 1, IsActive = true, ImageUrl = "batman-hoodie-main.jpg" },
                    new Product { Id = 3, Name = "Burberry Embroidered Sweatshirt", Description = "Premium embroidery on front and back.", CategoryId = 1, IsActive = true, ImageUrl = "burberry-white-back.jpg" },
                    new Product { Id = 4, Name = "Basic Oversized Zip-Up Hoodie", Description = "Plain high-quality hoodie for daily wear.", CategoryId = 1, IsActive = true, ImageUrl = "basic-black.jpg" },
                    new Product { Id = 5, Name = "Graphic 'X' Oversized Sweatshirt", Description = "Streetwear sweatshirt with a large pink 'X' on back.", CategoryId = 1, IsActive = true, ImageUrl = "graphic-x-back.jpg" },
                    new Product
                    {
                        Id = 7,
                        Name = "Off-White 'Tour 1993' Vintage Sweatshirt",
                        Description = "A perfect blend of retro and modern. Featuring a 1993 tour graphic in neon pink, finished with signature industrial patches at the hem. Made for those who appreciate high-end vintage aesthetics.",
                        CategoryId = 1,
                        IsActive = true,
                        ImageUrl = "tour-1993-main.jpg"
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "Diesel 'Dragon Eye' Industry Sweatshirt",
                        Description = "Unleash your wild side with this Diesel Industry piece. Featuring a high-definition dragon eye graphic with a distressed spray-paint effect. A bold statement sweatshirt for any modern urban wardrobe.",
                        CategoryId = 1,
                        IsActive = true,
                        ImageUrl = "diesel-dragon-eye.jpg"
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "Rhude 'Cambridge' Varsity Sweatshirt",
                        Description = "Classic collegiate aesthetic with a Rhude twist. This sweatshirt features 'Cambridge State Champions' embroidery and signature star graphics along the sleeves. A premium piece for a relaxed yet sharp varsity look.",
                        CategoryId = 1,
                        IsActive = true,
                        ImageUrl = "rhude-varsity-stars.jpg"
                    }

                };

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Products] ON");
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Products] OFF");

                // 2. Product Variants (Unique IDs 1 to 22)
                var variants = new List<ProductVariant>
                {
                    // P1 Variants
                    new ProductVariant { Id = 1, ProductId = 1, VariantName = "Black - L", Price = 850, Stock = 10 },
                    new ProductVariant { Id = 2, ProductId = 1, VariantName = "Black - M", Price = 850, Stock = 8 },
                    new ProductVariant { Id = 3, ProductId = 1, VariantName = "Wine - M", Price = 850, Stock = 8 },
                    new ProductVariant { Id = 4, ProductId = 1, VariantName = "Wine - L", Price = 850, Stock = 8 },

                    // P2 Variants
                    new ProductVariant { Id = 5, ProductId = 2, VariantName = "Batman Wine - M", Price = 950, Stock = 15 },
                    new ProductVariant { Id = 6, ProductId = 2, VariantName = "Batman Wine - L", Price = 950, Stock = 12 },
                    new ProductVariant { Id = 7, ProductId = 2, VariantName = "Batman White - M", Price = 950, Stock = 10 },
                    new ProductVariant { Id = 8, ProductId = 2, VariantName = "Batman White - L", Price = 950, Stock = 10 },
                    new ProductVariant { Id = 9, ProductId = 2, VariantName = "Batman Black - M", Price = 950, Stock = 20 },
                    new ProductVariant { Id = 10, ProductId = 2, VariantName = "Batman Black - L", Price = 950, Stock = 18 },

                    // P3 Variants
                    new ProductVariant { Id = 11, ProductId = 3, VariantName = "Burberry White - M", Price = 1200, Stock = 10 },
                    new ProductVariant { Id = 12, ProductId = 3, VariantName = "Burberry White - L", Price = 1200, Stock = 10 },
                    new ProductVariant { Id = 13, ProductId = 3, VariantName = "Burberry Black - M", Price = 1200, Stock = 10 },
                    new ProductVariant { Id = 14, ProductId = 3, VariantName = "Burberry Black - L", Price = 1200, Stock = 10 },

                    // P4 Variants
                    new ProductVariant { Id = 15, ProductId = 4, VariantName = "Plain White - M", Price = 800, Stock = 10 },
                    new ProductVariant { Id = 16, ProductId = 4, VariantName = "Plain White - L", Price = 800, Stock = 10 },
                    new ProductVariant { Id = 17, ProductId = 4, VariantName = "Plain Black - M", Price = 800, Stock = 10 },
                    new ProductVariant { Id = 18, ProductId = 4, VariantName = "Plain Black - L", Price = 800, Stock = 10 },

                    // P5 Variants
                    new ProductVariant { Id = 19, ProductId = 5, VariantName = "Graphic X Black - M", Price = 900, Stock = 10 },
                    new ProductVariant { Id = 20, ProductId = 5, VariantName = "Graphic X Black - L", Price = 900, Stock = 10 },
                    new ProductVariant { Id = 21, ProductId = 5, VariantName = "Graphic X White - M", Price = 900, Stock = 10 },
                    new ProductVariant { Id = 22, ProductId = 5, VariantName = "Graphic X White - L", Price = 900, Stock = 10 },

                    // P7 Variants (IDs 27 to 30)
                    new ProductVariant { Id = 27, ProductId = 7, VariantName = "Tour Black - M", Price = 1250, Stock = 8 },
                    new ProductVariant { Id = 28, ProductId = 7, VariantName = "Tour Black - L", Price = 1250, Stock = 10 },
                    new ProductVariant { Id = 29, ProductId = 7, VariantName = "Tour White - M", Price = 1250, Stock = 7 },
                    new ProductVariant { Id = 30, ProductId = 7, VariantName = "Tour White - L", Price = 1250, Stock = 9 },

                    // P8 Variants (IDs 31 to 34)
                    new ProductVariant { Id = 31, ProductId = 8, VariantName = "Diesel Black - M", Price = 1150, Stock = 15 },
                    new ProductVariant { Id = 32, ProductId = 8, VariantName = "Diesel Black - L", Price = 1150, Stock = 20 },
                    new ProductVariant { Id = 33, ProductId = 8, VariantName = "Diesel White - M", Price = 1150, Stock = 10 },
                    new ProductVariant { Id = 34, ProductId = 8, VariantName = "Diesel White - L", Price = 1150, Stock = 12 },

                    // P9 Variants (IDs 35 to 38)
                    new ProductVariant { Id = 35, ProductId = 9, VariantName = "Rhude Black - M", Price = 1300, Stock = 10 },
                    new ProductVariant { Id = 36, ProductId = 9, VariantName = "Rhude Black - L", Price = 1300, Stock = 12 },
                    new ProductVariant { Id = 37, ProductId = 9, VariantName = "Rhude White - M", Price = 1300, Stock = 8 },
                    new ProductVariant { Id = 38, ProductId = 9, VariantName = "Rhude White - L", Price = 1300, Stock = 10 }
                };

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ProductVariants] ON");
                await context.ProductVariants.AddRangeAsync(variants);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ProductVariants] OFF");

                // 3. Mapping (Variant + Attributes) - Explicit IDs
                // Black=1, White=2, Wine=4 | M=11, L=12
                var mapping = new List<VariantAttributeValue>
                {
                    // Variant 1 (P1 Black L)
                    new VariantAttributeValue { Id = 1, ProductVariantId = 1, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 2, ProductVariantId = 1, AttributeValueId = 12 },
                    // Variant 2 (P1 Black M)
                    new VariantAttributeValue { Id = 3, ProductVariantId = 2, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 4, ProductVariantId = 2, AttributeValueId = 11 },
                    // Variant 3 (P1 Wine M)
                    new VariantAttributeValue { Id = 5, ProductVariantId = 3, AttributeValueId = 4 },
                    new VariantAttributeValue { Id = 6, ProductVariantId = 3, AttributeValueId = 11 },
                    // Variant 4 (P1 Wine L)
                    new VariantAttributeValue { Id = 7, ProductVariantId = 4, AttributeValueId = 4 },
                    new VariantAttributeValue { Id = 8, ProductVariantId = 4, AttributeValueId = 12 },
                    
                    // Variant 5 (P2 Wine M)
                    new VariantAttributeValue { Id = 9, ProductVariantId = 5, AttributeValueId = 4 },
                    new VariantAttributeValue { Id = 10, ProductVariantId = 5, AttributeValueId = 11 },
                    // Variant 6 (P2 Wine L)
                    new VariantAttributeValue { Id = 11, ProductVariantId = 6, AttributeValueId = 4 },
                    new VariantAttributeValue { Id = 12, ProductVariantId = 6, AttributeValueId = 12 },
                    // Variant 7 (P2 White M)
                    new VariantAttributeValue { Id = 13, ProductVariantId = 7, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 14, ProductVariantId = 7, AttributeValueId = 11 },
                    // Variant 8 (P2 White L)
                    new VariantAttributeValue { Id = 15, ProductVariantId = 8, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 16, ProductVariantId = 8, AttributeValueId = 12 },
                    // Variant 9 (P2 Black M)
                    new VariantAttributeValue { Id = 17, ProductVariantId = 9, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 18, ProductVariantId = 9, AttributeValueId = 11 },
                    // Variant 10 (P2 Black L)
                    new VariantAttributeValue { Id = 19, ProductVariantId = 10, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 20, ProductVariantId = 10, AttributeValueId = 12 },

                    // Variant 11 (P3 White M)
                    new VariantAttributeValue { Id = 21, ProductVariantId = 11, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 22, ProductVariantId = 11, AttributeValueId = 11 },
                    // Variant 12 (P3 White L)
                    new VariantAttributeValue { Id = 23, ProductVariantId = 12, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 24, ProductVariantId = 12, AttributeValueId = 12 },
                    // Variant 13 (P3 Black M)
                    new VariantAttributeValue { Id = 25, ProductVariantId = 13, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 26, ProductVariantId = 13, AttributeValueId = 11 },
                    // Variant 14 (P3 Black L)
                    new VariantAttributeValue { Id = 27, ProductVariantId = 14, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 28, ProductVariantId = 14, AttributeValueId = 12 },

                    // Variant 15 (P4 White M)
                    new VariantAttributeValue { Id = 29, ProductVariantId = 15, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 30, ProductVariantId = 15, AttributeValueId = 11 },
                    // Variant 16 (P4 White L)
                    new VariantAttributeValue { Id = 31, ProductVariantId = 16, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 32, ProductVariantId = 16, AttributeValueId = 12 },
                    // Variant 17 (P4 Black M)
                    new VariantAttributeValue { Id = 33, ProductVariantId = 17, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 34, ProductVariantId = 17, AttributeValueId = 11 },
                    // Variant 18 (P4 Black L)
                    new VariantAttributeValue { Id = 35, ProductVariantId = 18, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 36, ProductVariantId = 18, AttributeValueId = 12 },

                    // Variant 19 (P5 Black M)
                    new VariantAttributeValue { Id = 37, ProductVariantId = 19, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 38, ProductVariantId = 19, AttributeValueId = 11 },
                    // Variant 20 (P5 Black L)
                    new VariantAttributeValue { Id = 39, ProductVariantId = 20, AttributeValueId = 1 },
                    new VariantAttributeValue { Id = 40, ProductVariantId = 20, AttributeValueId = 12 },
                    // Variant 21 (P5 White M)
                    new VariantAttributeValue { Id = 41, ProductVariantId = 21, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 42, ProductVariantId = 21, AttributeValueId = 11 },
                    // Variant 22 (P5 White L)
                    new VariantAttributeValue { Id = 43, ProductVariantId = 22, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 44, ProductVariantId = 22, AttributeValueId = 12 },

                     // Variant 27 (Black M)
                     new VariantAttributeValue { Id = 53, ProductVariantId = 27, AttributeValueId = 1 },
                     new VariantAttributeValue { Id = 54, ProductVariantId = 27, AttributeValueId = 11 },

                     // Variant 28 (Black L)
                     new VariantAttributeValue { Id = 55, ProductVariantId = 28, AttributeValueId = 1 },
                     new VariantAttributeValue { Id = 56, ProductVariantId = 28, AttributeValueId = 12 },

                    // Variant 29 (White M)
                    new VariantAttributeValue { Id = 57, ProductVariantId = 29, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 58, ProductVariantId = 29, AttributeValueId = 11 },

                    // Variant 30 (White L)
                    new VariantAttributeValue { Id = 59, ProductVariantId = 30, AttributeValueId = 2 },
                    new VariantAttributeValue { Id = 60, ProductVariantId = 30, AttributeValueId = 12 },

                    // Variant 31 (Black M)
                   new VariantAttributeValue { Id = 61, ProductVariantId = 31, AttributeValueId = 1 },
                   new VariantAttributeValue { Id = 62, ProductVariantId = 31, AttributeValueId = 11 },

                   // Variant 32 (Black L)
                   new VariantAttributeValue { Id = 63, ProductVariantId = 32, AttributeValueId = 1 },
                   new VariantAttributeValue { Id = 64, ProductVariantId = 32, AttributeValueId = 12 },

                  // Variant 33 (White M)
                  new VariantAttributeValue { Id = 65, ProductVariantId = 33, AttributeValueId = 2 },
                  new VariantAttributeValue { Id = 66, ProductVariantId = 33, AttributeValueId = 11 },

                  // Variant 34 (White L)
                  new VariantAttributeValue { Id = 67, ProductVariantId = 34, AttributeValueId = 2 },
                  new VariantAttributeValue { Id = 68, ProductVariantId = 34, AttributeValueId = 12 },

                  // Variant 35 (Black M)
new VariantAttributeValue { Id = 69, ProductVariantId = 35, AttributeValueId = 1 },
new VariantAttributeValue { Id = 70, ProductVariantId = 35, AttributeValueId = 11 },

// Variant 36 (Black L)
new VariantAttributeValue { Id = 71, ProductVariantId = 36, AttributeValueId = 1 },
new VariantAttributeValue { Id = 72, ProductVariantId = 36, AttributeValueId = 12 },

// Variant 37 (White M)
new VariantAttributeValue { Id = 73, ProductVariantId = 37, AttributeValueId = 2 },
new VariantAttributeValue { Id = 74, ProductVariantId = 37, AttributeValueId = 11 },

// Variant 38 (White L)
new VariantAttributeValue { Id = 75, ProductVariantId = 38, AttributeValueId = 2 },
new VariantAttributeValue { Id = 76, ProductVariantId = 38, AttributeValueId = 12 }

                };

                //await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [VariantAttributeValue] ON");
                await context.VariantAttributeValue.AddRangeAsync(mapping);
                await context.SaveChangesAsync();
                //await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [VariantAttributeValue] OFF");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }
    }
}

