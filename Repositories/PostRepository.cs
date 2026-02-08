using BoardApp.Data;
using BoardApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly Func<AppDbContext> _dbFactory;

        public PostRepository(Func<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            using var db = _dbFactory();
            return await db.Posts.AsNoTracking()
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            using var db = _dbFactory();
            return await db.Posts.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Post post)
        {
            using var db = _dbFactory();

            post.CreatedAtUtc = DateTime.UtcNow;
            post.UpdatedAtUtc = post.CreatedAtUtc;

            db.Posts.Add(post);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            using var db = _dbFactory();

            var entity = await db.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (entity == null)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 수정할 수 없습니다.");
            }

            entity.Title = post.Title;
            entity.Content = post.Content;
            entity.Author = post.Author;
            entity.UpdatedAtUtc = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var db = _dbFactory();

            db.Posts.Remove(new Post { Id = id });
            await db.SaveChangesAsync();
        }

    }
}
