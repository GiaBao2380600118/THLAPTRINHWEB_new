using Webbanhang.Models;

namespace Webbanhang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categoryList;

        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Điện thoại" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Máy tính bảng" },
                new Category { Id = 4, Name = "Phụ kiện" },
                new Category { Id = 5, Name = "Âm thanh" }
            };
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}
