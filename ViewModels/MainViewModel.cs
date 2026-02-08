using BoardApp.Models;
using BoardApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
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
                Posts.Clear();

                var items = await _service.GetAllAsync();
                foreach (var item in items) Posts.Add(item);

                StatusMessage = $"총 {Posts.Count}건";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }

        partial void OnSelectedPostChanged(Post? value)
        {
            if (value == null) return;
            Title = value.Title;
            Content = value.Content;
            Author = value.Author;
        }

        [RelayCommand]
        public async Task CreateAsync()
        {
            try
            {
                await _service.CreateAsync(Title, Content, Author);
                ClearForm();
                await LoadAsync();
                StatusMessage = "등록 완료";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
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
                await _service.UpdateAsync(SelectedPost.Id, Title, Content, Author);
                await LoadAsync();
                StatusMessage = "수정 완료";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
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
                await _service.DeleteAsync(SelectedPost.Id);
                ClearForm();
                await LoadAsync();
                StatusMessage = "삭제 완료";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
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
    }
}
