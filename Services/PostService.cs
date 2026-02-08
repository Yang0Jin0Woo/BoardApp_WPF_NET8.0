using BoardApp.Models;
using BoardApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardApp.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repo;

        public PostService(IPostRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Post>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Post?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task CreateAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            Validate(post.Title, post.Content, post.Author);
            post.Title = post.Title.Trim();
            post.Content = post.Content.Trim();
            post.Author = post.Author.Trim();

            await _repo.AddAsync(post);
        }

        public async Task UpdateAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            Validate(post.Title, post.Content, post.Author);

            var existing = await _repo.GetByIdAsync(post.Id);
            if (existing == null)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 수정할 수 없습니다.");
            }

            existing.Title = post.Title.Trim();
            existing.Content = post.Content.Trim();
            existing.Author = post.Author.Trim();

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 삭제할 수 없습니다.");
            }

            await _repo.DeleteAsync(id);
        }

        private static void Validate(string title, string content, string author)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("제목은 필수입니다.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("작성자는 필수입니다.");
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("내용은 필수입니다.");
        }
    }
}
