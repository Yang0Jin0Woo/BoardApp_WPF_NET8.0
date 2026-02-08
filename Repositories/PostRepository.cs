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
            post.UpdatedAtUtc = DateTime.UtcNow;

            db.Posts.Attach(post);
            var entry = db.Entry(post);
            entry.Property(p => p.Title).IsModified = true;
            entry.Property(p => p.Content).IsModified = true;
            entry.Property(p => p.Author).IsModified = true;
            entry.Property(p => p.UpdatedAtUtc).IsModified = true;

            var affected = await db.SaveChangesAsync();
            if (affected == 0)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 수정할 수 없습니다.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var db = _dbFactory();

            db.Posts.Remove(new Post { Id = id });
            var affected = await db.SaveChangesAsync();
            if (affected == 0)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 삭제할 수 없습니다.");
            }
        }

    }
}
