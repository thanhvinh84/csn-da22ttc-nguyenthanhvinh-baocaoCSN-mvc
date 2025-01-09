using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;

namespace Shopping_Tutorial.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {

                CategoryModel vot = new CategoryModel { Name = "Vot", Slug = "vot", Description = "Vot Yonex is Large Brand in the world", Status = 1 };
                CategoryModel giay = new CategoryModel { Name = "Giay", Slug = "giay", Description = "Giay Yonex is Large Brand in the world", Status = 1 };
                CategoryModel balo = new CategoryModel { Name = "Balo", Slug = "balo", Description = "Balo Victor is Large Brand in the world", Status = 1 };
                BrandModel yonex = new BrandModel { Name = "Yonex", Slug = "yonex", Description = "Yonex is Large Brand in the world", Status = 1 };
                BrandModel victor = new BrandModel { Name = "Victor", Slug = "victor", Description = "Victor is Large Brand in the world", Status = 1 };

                _context.Products.AddRange(

                    new ProductModel { Name = "Vot", Slug = "vot", Description = "Vot Yonex is best", Image = "1.jpg", Category = vot,Brand= yonex, Price = "100" },
                    new ProductModel { Name = "Giay", Slug = "giay", Description = "Giay Yonex is best", Image = "2.jpg", Category = giay, Brand = yonex, Price = "75"},
                    new ProductModel { Name = "Balo", Slug = "balo", Description = "Balo Victor is best", Image = "3.jpg", Category = balo, Brand = victor, Price = "50"}
                );
                _context.SaveChanges();
            }

        }
    }
}
