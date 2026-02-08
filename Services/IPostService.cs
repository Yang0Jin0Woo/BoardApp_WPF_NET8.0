using BoardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
