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
                // 1. إنشاء المنتج
                var products = new List<Product>
        {
            new Product{
                Id = 1,
                Name = "Oversized Zip-up Hoodie",
                Description = "Premium oversized hoodie with a full-length silver zipper and front pockets.",
                CategoryId = 1,
                IsActive = true
            },
            new Product{
                Id = 2,
                Name = "Batman Graphic Zip-up Hoodie",
                Description = "Urban streetwear oversized zip-up hoodie featuring a bold Batman logo on the chest. Double-lined hood and heavy fleece fabric for premium feel.",
                CategoryId = 1,
                IsActive = true,
                ImageUrl = "batman-hoodie-main.jpg"
            },
        };

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Products] ON");
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Products] OFF");

                // 2. إنشاء الـ Variants
                var variants = new List<ProductVariant>
        {
            new ProductVariant
            {
                Id = 3, ProductId = 1, VariantName = "Black - Large",
                Price = 850.00m, Stock = 10,
            },
            new ProductVariant
            {
                Id = 4, ProductId = 1, VariantName = "Black - X-Large",
                Price = 850.00m, Stock = 8,
            },
            // Batman Wine
            new ProductVariant { Id = 5, ProductId = 2, VariantName = "Batman - Wine - Medium", Price = 950.00m, Stock = 15}, //
            new ProductVariant { Id = 6, ProductId = 2, VariantName = "Batman - Wine - Large", Price = 950.00m, Stock = 12}, //

           // Batman White
           new ProductVariant { Id = 7, ProductId = 2, VariantName = "Batman - White - Medium", Price = 950.00m, Stock = 10 }, //
           new ProductVariant { Id = 8, ProductId = 2, VariantName = "Batman - White - Large", Price = 950.00m, Stock = 10 }, //
 
           // Batman Black
          new ProductVariant { Id = 9, ProductId = 2, VariantName = "Batman - Black - Medium", Price = 950.00m, Stock = 20,}, //
          new ProductVariant { Id = 10, ProductId = 2, VariantName = "Batman - Black - Large", Price = 950.00m, Stock = 18,} //
        };

                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ProductVariants] ON");
                await context.ProductVariants.AddRangeAsync(variants);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ProductVariants] OFF");

                // 3. إنشاء الـ Mapping (VariantAttributeValue)
                var mapping = new List<VariantAttributeValue>
        {
            new VariantAttributeValue { Id = 5, ProductVariantId = 3, AttributeValueId = 1 },
            new VariantAttributeValue { Id = 6, ProductVariantId = 3, AttributeValueId = 12 },
            new VariantAttributeValue { Id = 7, ProductVariantId = 4, AttributeValueId = 1 },
            new VariantAttributeValue { Id = 8, ProductVariantId = 4, AttributeValueId = 13 },

            // Wine Batman (M & L)
            new VariantAttributeValue { Id = 9, ProductVariantId = 5, AttributeValueId = 4 },  // Color: Wine
            new VariantAttributeValue { Id = 10, ProductVariantId = 5, AttributeValueId = 11 }, // Size: M
            new VariantAttributeValue { Id = 11, ProductVariantId = 6, AttributeValueId = 4 },  // Color: Wine
            new VariantAttributeValue { Id = 12, ProductVariantId = 6, AttributeValueId = 12 }, // Size: L

            // White Batman (M & L)
            new VariantAttributeValue { Id = 13, ProductVariantId = 7, AttributeValueId = 2 },  // Color: White
            new VariantAttributeValue { Id = 14, ProductVariantId = 7, AttributeValueId = 11 }, // Size: M
            new VariantAttributeValue { Id = 15, ProductVariantId = 8, AttributeValueId = 2 },  // Color: White
            new VariantAttributeValue { Id = 16, ProductVariantId = 8, AttributeValueId = 12 }, // Size: L

            // Black Batman (M & L)
            new VariantAttributeValue { Id = 17, ProductVariantId = 9, AttributeValueId = 1 },  // Color: Black
            new VariantAttributeValue { Id = 18, ProductVariantId = 9, AttributeValueId = 11 }, // Size: M
            new VariantAttributeValue { Id = 19, ProductVariantId = 10, AttributeValueId = 1 }, // Color: Black
            new VariantAttributeValue { Id = 20, ProductVariantId = 10, AttributeValueId = 12 } // Size: L
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

