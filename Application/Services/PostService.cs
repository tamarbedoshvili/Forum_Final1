using Application.Interfaces;
using AutoMapper;
using Final.Domain.Dto;
using Final.Dto;
using Final.Entities;
using Final.Enum;
using Final.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Final.Services
{
    public class PostService : IPostService
    {
        public IPostRepository PostRepository;
        public ICommentRepository CommentRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;



        public PostService(UserManager<User> userManager, IPostRepository postRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            PostRepository = postRepository;
            CommentRepository = commentRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task AddComment(AddCommentDto commentDto)
        {
            var user = await _userManager.FindByIdAsync(commentDto.UserId);
            var post = await PostRepository.GetSinglePost(commentDto.PostID);

            var comment = _mapper.Map<Comment>(commentDto);
            comment.User = user;
            comment.Post = post;
            comment.CreateDate= DateTime.Now;
            

            await CommentRepository.AddComment(comment);
        }

        public async Task AddPost(AddPostDto postDto)
        {
            var user = await _userManager.FindByIdAsync(postDto.CreatorId);
            var post = _mapper.Map<Post>(postDto);
            post.Creator = user;
            post.State = EState.Pending;
            post.Status = EStatus.Active;
            post.CreateDate=DateTime.Now;
            await PostRepository.AddPost(post);

        }

        public async Task DeleteComment(int id)
        {
            await CommentRepository.DeleteComment(id);
        }

        public async Task DeletePost(int id)
        {
            await PostRepository.DeletePost(id);
        }

        public async Task<List<PostDto>> GetPosts()
        {
            return (await PostRepository.GetPosts()).Select(x => new PostDto
            {
                Id = x.Id,
                Name = x.Name,
                Content = x.Content,
                CreatorId = x.CreatorId,
                UserName = x.Creator.UserName,
                Comments = x.Comments.Select(a => new CommentDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    UserId = a.UserId,
                    PostID = a.PostID,
                    UserName = a.User.UserName,

                }).ToList(),
                CommentAmount = x.Comments.Count
            }).Where(x => x.State != EState.Hide).ToList();

        }

        public async Task UpdateComment(UpdateCommentDto commentDto)
        {
            var post = (await PostRepository.GetSinglePost(commentDto.PostID));
            if (post.Status == EStatus.Inactive)
            {
                throw new Exception("You cant update inactive post");
            }
            var user = await _userManager.FindByIdAsync(commentDto.UserId);

            if (post != null)
            {
                var comment = _mapper.Map<Comment>(commentDto);
                comment.Post = post;
                comment.User = user;

                await CommentRepository.UpdateComment(comment);
            }


        }
        public async Task UpdatePost(UpdatePostDto postDto)
        {
            var user = await _userManager.FindByIdAsync(postDto.CreatorId);
            var post = (await PostRepository.GetSinglePost(postDto.Id));
            if (post.Status == EStatus.Inactive)
            {
                throw new Exception("You cant update inactive post");
            }
            if (user.Id != post.Creator.Id)
            {
                throw new Exception("You can only update your post");

            }
            post.Name = postDto.Name;
            post.Content = postDto.Content;

            await PostRepository.UpdatePost(post);
        }



        public async Task ChangPostStatus(ChangePostStatusDto post)
        {
            var updatedPost = (await PostRepository.GetSinglePost(post.PostID));
            updatedPost.Status = post.Status;


            await PostRepository.UpdatePost(updatedPost);

        }

        public async Task ChangPostState(ChangePostStateDto post)
        {
            var updatedPost = (await PostRepository.GetSinglePost(post.PostID));
            updatedPost.State = post.State;


            await PostRepository.UpdatePost(updatedPost);
        }
    }
}
