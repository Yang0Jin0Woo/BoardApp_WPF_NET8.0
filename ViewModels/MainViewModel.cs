using BoardApp.Models;
using BoardApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BoardApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPostService _service;

        public ObservableCollection<Post> Posts { get; } = new();

        [ObservableProperty] private Post? selectedPost;

        [ObservableProperty] private string title = "";
        [ObservableProperty] private string content = "";
        [ObservableProperty] private string author = "";

        [ObservableProperty] private string statusMessage = "";

        public MainViewModel(IPostService service)
        {
            _service = service;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            try
            {
                StatusMessage = "불러오는 중...";
                var items = await _service.GetAllAsync();

                Posts.Clear();
                foreach (var item in items) Posts.Add(item);

                StatusMessage = $"총 {Posts.Count}건";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                StatusMessage = "불러오기 실패";
            }
        }

        partial void OnSelectedPostChanged(Post? value)
        {
            if (value == null)
            {
                ClearForm();
                return;
            }

            Title = value.Title;
            Content = value.Content;
            Author = value.Author;
        }

        [RelayCommand]
        public async Task CreateAsync()
        {
            try
            {
                var created = await _service.CreateAsync(new Post
                {
                    Title = Title,
                    Content = Content,
                    Author = Author
                });
                Posts.Insert(0, created);
                ClearForm();
                StatusMessage = "등록 완료";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                StatusMessage = "등록 실패";
            }
        }

        [RelayCommand]
        public async Task UpdateAsync()
        {
            if (SelectedPost == null)
            {
                StatusMessage = "수정할 게시글을 선택하세요.";
                return;
            }

            try
            {
                var updated = new Post
                {
                    Id = SelectedPost.Id,
                    Title = Title,
                    Content = Content,
                    Author = Author
                };

                var normalized = await _service.UpdateAsync(updated);
                ReplacePostInList(normalized);
                StatusMessage = "수정 완료";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                StatusMessage = "수정 실패";
            }
        }

        [RelayCommand]
        public async Task DeleteAsync()
        {
            if (SelectedPost == null)
            {
                StatusMessage = "삭제할 게시글을 선택하세요.";
                return;
            }

            try
            {
                var toDelete = SelectedPost;
                await _service.DeleteAsync(toDelete.Id);
                Posts.Remove(toDelete);
                SelectedPost = null;
                StatusMessage = "삭제 완료";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                StatusMessage = "삭제 실패";
            }
        }

        [RelayCommand]
        public void Clear()
        {
            ClearForm();
            SelectedPost = null;
            StatusMessage = "입력 초기화";
        }

        private void ClearForm()
        {
            Title = "";
            Content = "";
            Author = "";
        }

        private void ReplacePostInList(Post updated)
        {
            for (int i = 0; i < Posts.Count; i++)
            {
                if (Posts[i].Id == updated.Id)
                {
                    Posts[i] = updated;
                    SelectedPost = updated;
                    return;
                }
            }
        }
    }
}
