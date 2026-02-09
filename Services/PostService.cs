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

        public async Task<Post> CreateAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            NormalizeAndValidate(post);

            return await _repo.AddAsync(post);
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            NormalizeAndValidate(post);

            var updated = await _repo.UpdateAsync(post);
            if (updated == null)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 수정할 수 없습니다.");
            }

            return updated;
        }

        public async Task DeleteAsync(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 삭제할 수 없습니다.");
            }
        }

        // Trim(정규화) -> Validate(검증)
        private static void NormalizeAndValidate(Post post)
        {
            post.Title = post.Title?.Trim() ?? "";
            post.Author = post.Author?.Trim() ?? "";
            post.Content = post.Content?.Trim() ?? "";

            if (post.Title.Length == 0)
                throw new ArgumentException("제목은 필수입니다.", nameof(post.Title));
            if (post.Author.Length == 0)
                throw new ArgumentException("작성자는 필수입니다.", nameof(post.Author));
            if (post.Content.Length == 0)
                throw new ArgumentException("내용은 필수입니다.", nameof(post.Content));
        }

    }
}
