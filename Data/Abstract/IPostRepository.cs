using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Abstract

{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void CreatePost(Post post);

        void EditPost(PostEditViewModel post, int[] tagList);

        void DeletePost(Post post);
    }
}