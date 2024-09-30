using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {
            var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands.Count > 0 && context.ProductBrands.Count() == 0)
            {
                brands = brands.Select(b => new ProductBrand()
                {
                    Name = b.Name,
                }).ToList();

                foreach (var brand in brands)
                {
                    context.Set<ProductBrand>().Add(brand);
                }

                await context.SaveChangesAsync();
            }

            var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
            var categories = JsonSerializer.Deserialize<List<ProductCategory>>(brandsData);
            if (categories.Count > 0 && context.ProductCategories.Count() == 0)
            {
                categories = categories.Select(c => new ProductCategory()
                {
                    Name = c.Name,
                }).ToList();

                foreach (var category in categories)
                {
                    context.Set<ProductCategory>().Add(category);
                }

                await context.SaveChangesAsync();
            }

            var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(brandsData);
            if (products.Count > 0 && context.Products.Count() == 0)
            {
                foreach (var product in products)
                {
                    context.Set<Product>().Add(product);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
