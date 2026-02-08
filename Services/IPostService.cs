using BoardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
