using BoardApp.Data;
using BoardApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public PostRepository(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            await using var db = _dbFactory.CreateDbContext();
            return await db.Posts.AsNoTracking()
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            await using var db = _dbFactory.CreateDbContext();
            return await db.Posts.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post> AddAsync(Post post)
        {
            await using var db = _dbFactory.CreateDbContext();

            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> UpdateAsync(Post post)
        {
            await using var db = _dbFactory.CreateDbContext();

            var existing = await db.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existing == null) return null;

            existing.Title = post.Title;
            existing.Content = post.Content;
            existing.Author = post.Author;

            await db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var db = _dbFactory.CreateDbContext();

            var existing = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return false;

            db.Posts.Remove(existing);
            await db.SaveChangesAsync();
            return true;
        }

    }
}
