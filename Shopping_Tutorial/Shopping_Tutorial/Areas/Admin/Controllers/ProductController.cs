using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;
namespace Shopping_Tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context; _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            product.Brand = (await _dataContext.Brands.FindAsync(product.BrandId) ?? new BrandModel());
            product.Category = (await _dataContext.Categories.FindAsync(product.CategoryId) ?? new CategoryModel());
            if(string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Price))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên sản phẩm hoặc giá sản phẩm");
                return View(product);
            }
            if(product.CategoryId == 0 || product.BrandId == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục và thương hiện sản phẩm");
                return View(product);
            }
            product.Slug = product.Name.Replace(" ", "-");
            var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã có trong database");
                return View(product);
            }
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                product.Image = imageName;
            }
            _dataContext.Add(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm sản phẩm mới thành công";
            return RedirectToAction("Index");
          
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            var existed_product = _dataContext.Products.Find(product.Id);
            if(string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Price))
            {
                ModelState.AddModelError("Error", "Vui lòng nhập tên sản phẩm và giá tiền sản phẩm");
                return View(product);
            }
            if(product.CategoryId == 0 || product.BrandId == 0)
            {
                ModelState.AddModelError("Error", "Vui lòng nhập danh mục và thương hiệu");
                return View(product);
            }
            product.Slug = product.Name.Replace(" ", "-");

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                string oldFilePath = Path.Combine(uploadsDir, existed_product.Image);
                try
                {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while deteting the product image");
                }
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                existed_product.Image = imageName;
            }
            existed_product.Name = product.Name;
            existed_product.Description = product.Description;
            existed_product.Price = product.Price;
            existed_product.CategoryId = product.CategoryId;
            existed_product.BrandId = product.BrandId;

            _dataContext.Update(existed_product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật sản phẩm mới thành công";
            return RedirectToAction("Index");            
        }
        public async Task<IActionResult> Delete(int Id) 
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
          if(!string.Equals(product.Image, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string oldFileImage = Path.Combine(uploadsDir, product.Image);
                if(System.IO.File.Exists(oldFileImage)) 
                {
                    System.IO.File.Delete(oldFileImage);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã bị xóa";
            return RedirectToAction("Index");
        }
    }
}