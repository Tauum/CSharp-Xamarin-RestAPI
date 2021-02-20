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
                Product product1 = new Product
                {
                    Name = "Iphone 5",
                    ReleaseYear = 1984,
                    Description = "The iPhone 5 is a smartphone that was designed and marketed by Apple Inc. It is the sixth generation of the iPhone succeeding the iPhone 4S",
                    Score = 30
                };

                if (!context.Product.Any())
                {
                    context.Product.Add(product1);
                }
                else
                {
                    product1 = context.Product.Where(x => x.Name == "Iphone 5").SingleOrDefault();
                }

                User user1 = context.User.Where(x => x.Username == "Tom").SingleOrDefault();
                if (user1 == null)
                {
                    user1 = new User
                    {
                        Username = "Tom",
                        Email = "Tjs1crt@Bolton.ac.uk",
                        Password = "b",
                        ScoreTotal = 1500,
                        Admin = 1
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
                         Description = "This product is really good!"
                     });
                }
                context.SaveChanges();
            }
        }
    }
}