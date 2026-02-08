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

        public async Task CreateAsync(string title, string content, string author)
        {
            Validate(title, content, author);

            var post = new Post
            {
                Title = title.Trim(),
                Content = content.Trim(),
                Author = author.Trim()
            };

            await _repo.AddAsync(post);
        }

        public async Task UpdateAsync(int id, string title, string content, string author)
        {
            Validate(title, content, author);

            var post = await _repo.GetByIdAsync(id);
            if (post == null)
            {
                throw new InvalidOperationException("게시글이 존재하지 않아 수정할 수 없습니다.");
            }

            post.Title = title.Trim();
            post.Content = content.Trim();
            post.Author = author.Trim();

            await _repo.UpdateAsync(post);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        private static void Validate(string title, string content, string author)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("제목은 필수입니다.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("작성자는 필수입니다.");
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("내용은 필수입니다.");
        }
    }
}
