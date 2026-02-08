using BoardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
