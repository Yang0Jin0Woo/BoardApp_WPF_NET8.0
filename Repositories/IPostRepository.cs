using BoardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> AddAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
