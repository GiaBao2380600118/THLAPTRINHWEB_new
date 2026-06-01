using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Webbanhang.Models;
using Webbanhang.Repositories;

namespace Webbanhang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string? searchString, int? categoryId, string? sortOrder)
        {
            var products = _productRepository.GetAll();
            var categories = _categoryRepository.GetAllCategories();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "name_asc" => products.OrderBy(p => p.Name),
                _ => products.OrderBy(p => p.Id)
            };

            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortOrder = sortOrder;

            return View(products);
        }

        public IActionResult Add()
        {
            LoadCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile? imageUrl, List<IFormFile>? imageUrls, IFormFile? videoFile)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveFile(imageUrl, "images");
                }

                if (imageUrls != null && imageUrls.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        product.ImageUrls.Add(await SaveFile(file, "images"));
                    }
                }

                if (videoFile != null)
                {
                    product.VideoUrl = await SaveFile(videoFile, "videos");
                }

                _productRepository.Add(product);
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            LoadCategories();
            return View(product);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = _categoryRepository.GetAllCategories()
                .FirstOrDefault(c => c.Id == product.CategoryId)?.Name;

            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            LoadCategories(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile? imageUrl, List<IFormFile>? imageUrls, IFormFile? videoFile)
        {
            if (ModelState.IsValid)
            {
                var oldProduct = _productRepository.GetById(product.Id);

                if (oldProduct != null)
                {
                    product.ImageUrl = oldProduct.ImageUrl;
                    product.ImageUrls = oldProduct.ImageUrls;
                    product.VideoUrl = oldProduct.VideoUrl;
                }

                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveFile(imageUrl, "images");
                }

                if (imageUrls != null && imageUrls.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        product.ImageUrls.Add(await SaveFile(file, "images"));
                    }
                }

                if (videoFile != null)
                {
                    product.VideoUrl = await SaveFile(videoFile, "videos");
                }

                _productRepository.Update(product);
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            LoadCategories(product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        private void LoadCategories(int? selectedId = null)
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }

        private async Task<string> SaveFile(IFormFile file, string folder)
        {
            var safeFileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            Directory.CreateDirectory(folderPath);

            var savePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/{folder}/{uniqueFileName}";
        }
    }
}
