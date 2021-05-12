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
                        new Category { Name = "Shoes"}, new Category { Name = "Shirt"}, new Category { Name = "Kitchen"},
                        new Category { Name = "Homeware"}, new Category { Name = "Exercise"}, new Category { Name = "Food"},
                        new Category { Name = "Outdoors"}, new Category { Name = "Stationary"}, new Category { Name = "Sanitary"},
                        new Category { Name = "Pet-related"}, new Category { Name = "Confectionary"}, new Category { Name = "Beverages"}
                    );
                    context.SaveChanges();
                }

                Product product1 = new Product
                {
                    Name = "Iphone 5", ReleaseYear = 2012,
                    Description = "The iPhone 5 is a smartphone that was designed and marketed by Apple Inc. It is the sixth generation of the iPhone succeeding the iPhone 4S",
                    Score = 30, Category = context.Category.Where(x => x.Name == "Electronics").SingleOrDefault()
                };

                Product product2 = new Product
                {
                    Name = "Samsung A51",
                    ReleaseYear = 2020,
                    Description = "Classic Samsung traits you do get in the Galaxy A51 include a bold AMOLED screen, and software that looks just like that of the Galaxy S21 series.",
                    Score = 45,
                    Category = context.Category.Where(x => x.Name == "Electronics").SingleOrDefault()
                };

                if (!context.Product.Any())
                { 
                    context.Product.Add(product1);
                    context.Product.Add(product2);
                }
                
                else 
                { 
                    product1 = context.Product.Where(x => x.Name == "Iphone 5").SingleOrDefault();
                    product2 = context.Product.Where(x => x.Name == "Samsung A51").SingleOrDefault();
                }

                User user1 = context.User.Where(x => x.Username == "admin").SingleOrDefault();
                User user2 = context.User.Where(x => x.Username == "user").SingleOrDefault();
                User user3 = context.User.Where(x => x.Username == "test").SingleOrDefault();

                if (user1 == null || user2 == null)
                {
                    user1 = new User
                    {
                        Username = "admin", Email = "admin@test.com",
                        Password = "$2a$04$EmGblUehqJ7P.MViswB39.SPtN/yjQAc6g9dcQVVMRQZWotGftdnO", 
                        ScoreTotal = 0, Admin = true
                    };
                    context.User.Add(user1);
                    user2 = new User
                    {
                        Username = "user",
                        Email = "user@test.com",
                        Password = "$2a$04$EmGblUehqJ7P.MViswB39.SPtN/yjQAc6g9dcQVVMRQZWotGftdnO",
                        ScoreTotal = 0,
                        Admin = false
                    };
                    context.User.Add(user2);
                    user3 = new User
                    {
                        Username = "test",
                        Email = "test@test.com",
                        Password = "$2a$04$EmGblUehqJ7P.MViswB39.SPtN/yjQAc6g9dcQVVMRQZWotGftdnO",
                        ScoreTotal = 0,
                        Admin = false
                    };
                    context.User.Add(user3);
                }

                if (!context.Review.Any())
                {
                    context.Review.AddRange(
                    new Review {  User = user1, Product = product1, Description = "This product is really good!", Visible = false },
                    new Review { User = user3, Product = product1, Description = "this is okay", Visible = true },
                    new Review { User = user2, Product = product1, Description = "IIII HATE ITTTT!", Visible = true });
                }
                context.SaveChanges();
            }
        }
    }
}