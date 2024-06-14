using Final.Domain.Dto;
using Final.Dto;

namespace Final.Interfaces
{
    public interface IPostService
    {
        public Task<List<PostDto>> GetPosts();

        public Task AddPost(AddPostDto post);
        public Task AddComment(AddCommentDto comment);
        public Task UpdateComment(UpdateCommentDto comment);

        public Task DeletePost(int id);
        public Task DeleteComment(int id);

        public Task UpdatePost(UpdatePostDto Post);
        public Task ChangPostStatus(ChangePostStatusDto Post);
        public Task ChangPostState(ChangePostStateDto Post);

       
    }
}
