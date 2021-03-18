using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GOVAPI.Data;
using System;
using System.Linq;
namespace GOVAPI.Models
{
    public static class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var context = new GOVAPIContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<GOVAPIContext>>()))
            {
                if (!context.Category.Any())
                {
                    context.Category.AddRange(
                        new Category { Name = "Electronics"},
                        new Category { Name = "Shoes"},
                        new Category { Name = "Shirt"},
                        new Category { Name = "Kitchen"},
                        new Category { Name = "Homeware"},
                        new Category { Name = "Exercise"}, 
                        new Category { Name = "Food"},
                        new Category { Name = "Outdoors"},
                        new Category { Name = "Stationary"},
                        new Category { Name = "Sanitary"},
                        new Category { Name = "Pet-related"},
                        new Category { Name = "Confectionary"},
                        new Category { Name = "Beverages"}
                    );
                    context.SaveChanges();
                }

                Product product1 = new Product
                {
                    Name = "Iphone 5",
                    ReleaseYear = 1984,
                    Description = "The iPhone 5 is a smartphone that was designed and marketed by Apple Inc. It is the sixth generation of the iPhone succeeding the iPhone 4S",
                    Score = 30,
                    Category = context.Category.Where(x => x.Name == "Electronics").SingleOrDefault()
                };

                if (!context.Product.Any()) { context.Product.Add(product1); }
                
                else { product1 = context.Product.Where(x => x.Name == "Iphone 5").SingleOrDefault(); }

                User user1 = context.User.Where(x => x.Username == "user").SingleOrDefault();

                if (user1 == null)
                {
                    user1 = new User
                    {
                        Username = "user",
                        Email = "user@test.com",
                        Password = "$2a$04$EmGblUehqJ7P.MViswB39.SPtN/yjQAc6g9dcQVVMRQZWotGftdnO",
                        ScoreTotal = 0,
                        Admin = true
                    };
                    context.User.Add(user1);
                }

                if (!context.Review.Any())
                {
                    context.Review.AddRange(
                    new Review
                     {
                         User = user1,
                         Product = product1,
                         Description = "This product is really good!",
                         Visible = true
                     });
                }
                context.SaveChanges();
            }
        }
    }
}