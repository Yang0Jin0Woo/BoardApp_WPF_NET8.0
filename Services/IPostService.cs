using BoardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task CreateAsync(string title, string content, string author);
        Task UpdateAsync(int id, string title, string content, string author);
        Task DeleteAsync(int id);
    }
}
