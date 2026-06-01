using Webbanhang.Models;

namespace Webbanhang.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}
