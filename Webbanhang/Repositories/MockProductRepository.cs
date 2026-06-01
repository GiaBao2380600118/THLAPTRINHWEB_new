using Webbanhang.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Webbanhang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "iPhone 15 Pro Max",
                    Price = 29990000,
                    Description = "Điện thoại cao cấp, chip mạnh, camera sắc nét, phù hợp chụp ảnh và quay video.",
                    CategoryId = 1,
                    ImageUrl = "/images/products/iphone-15-pro-max-1.jpg",
                    ImageUrls = new List<string> { "/images/products/iphone-15-pro-max-1.jpg", "/images/products/iphone-15-pro-max-2.jpg", "/images/products/iphone-15-pro-max-3.jpg" },
                    VideoUrl = "/videos/iphone-15-pro-max-demo.mp4"
                },
                new Product 
                {
                    Id = 2,
                    Name = "Samsung Galaxy S24 Ultra",
                    Price = 26990000,
                    Description = "Màn hình lớn, bút S Pen tiện lợi, camera zoom xa, pin dùng lâu.",
                    CategoryId = 1,
                    ImageUrl = "/images/products/samsungs24untra-1.jpg",
                    ImageUrls = new List<string> { "/images/products/samsungs24untra-1.jpg", "/images/products/samsungs24untra-2.jpg", "/images/products/samsungs24untra-3.jpg" },
                    VideoUrl = "/videos/samsung-s24-ultra-demo.mp4"
                },
                new Product
                {
                    Id = 3,
                    Name = "MacBook Air M2",
                    Price = 21990000,
                    Description = "Laptop mỏng nhẹ, pin tốt, phù hợp học tập, văn phòng và thiết kế cơ bản.",
                    CategoryId = 2,
                    ImageUrl = "/images/products/macbook-1.jpg",
                    ImageUrls = new List<string> { "/images/products/macbook-1.jpg", "/images/products/macbook-2.jpg", "/images/products/macbook-3.jpg" },
                    VideoUrl = "/videos/macbook-air-m2-demo.mp4"
                },
                new Product
                {
                    Id = 4,
                    Name = "Dell XPS 13",
                    Price = 24990000,
                    Description = "Laptop Windows cao cấp, thiết kế sang trọng, màn hình đẹp, hiệu năng ổn định.",
                    CategoryId = 2,
                    ImageUrl = "/images/products/dell-1.jpg",
                    ImageUrls = new List<string> { "/images/products/dell-1.jpg", "/images/products/dell-2.jpg", "/images/products/dell-3.jpg" },
                    VideoUrl = "/videos/dell-xps-13-demo.mp4"
                },
                new Product
                {
                    Id = 5,
                    Name = "ASUS ROG Strix G16",
                    Price = 32990000,
                    Description = "Laptop gaming mạnh mẽ, tản nhiệt tốt, màn hình tần số quét cao.",
                    CategoryId = 2,
                    ImageUrl = "/images/products/asus-1.jpg",
                    ImageUrls = new List<string> { "/images/products/asus-1.jpg", "/images/products/asus-2.jpg", "/images/products/asus-3.jpg" },
                    VideoUrl = "/videos/asus-rog-g16-demo.mp4"
                },
                
            };
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id)!;
        }

        public void Add(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
