using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            
            
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            
            try 
            {
                
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                
                
                if (!context.ProductBrands.Any())
                {
                    //context.AddRange(brands);

                    foreach(var item in brands)
                    {
                        context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }

                if (!context.ProductTypes.Any())
                {
                    //context.AddRange(types);

                    foreach(var item in types)
                    {
                        context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }

                if (!context.Products.Any())
                {
                    //context.AddRange(products);

                    foreach(var item in products)
                    {
                        context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }

                await context.SaveChangesAsync();

                var productsInDb = await context.Products.ToListAsync();
                var productTypesInDb = await context.ProductTypes.ToListAsync();
                var productBrandsInDb = await context.ProductBrands.ToListAsync();

                if (productsInDb.Count != products.Count) throw new Exception(
                    "The number of products in the DB after the attempted insertion doesn't match the number of products in the seed file");
                if (productTypesInDb.Count != types.Count) throw new Exception(
                    "The number of product types in the DB after the attempted insertion doesn't match the number of product types in the seed file");
                if (productBrandsInDb.Count != brands.Count) throw new Exception(
                    "The number of product brands in the DB after the attempted insertion doesn't match the number of product brands in the seed file");

                logger.LogInformation("The seeding process completed all right");

            } 
            catch(Exception ex) 
            {
                logger.LogError(ex.Message);
            }
        }
    }
}