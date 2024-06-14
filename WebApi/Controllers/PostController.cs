using Final.Domain.Dto;
using Final.Dto;
using Final.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [ApiController]
    [Route("Post")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetPosts();
            return Ok(posts);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddPost(AddPostDto addPostDto)
        {
            await _postService.AddPost(addPostDto);
            return Ok();
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdatePost(UpdatePostDto updatePostDto)
        {
            await _postService.UpdatePost(updatePostDto);
            return Ok();
        }

        [HttpPost("AddComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddComment(AddCommentDto addCommentDto)
        {
            await _postService.AddComment(addCommentDto);
            return Ok();
        }

        [HttpPut("UpdateComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> EditComment(UpdateCommentDto updateCommentDto)
        {
            await _postService.UpdateComment(updateCommentDto);
            return Ok();
        }

        [HttpDelete("DeleteComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteComment(int id)
        {
            await _postService.DeleteComment(id);
            return Ok();
        }

        [HttpPut("ChangeStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public async Task<IActionResult> ChangeStatus(ChangePostStatusDto changePostStatusDto)
        {

            await _postService.ChangPostStatus(changePostStatusDto);
            return Ok();
        }

        [HttpPut("ChangeState")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public async Task<IActionResult> ChangeState(ChangePostStateDto changePostStateDto)
        {
            await _postService.ChangPostState(changePostStateDto);
            return Ok();
        }
    }
}
