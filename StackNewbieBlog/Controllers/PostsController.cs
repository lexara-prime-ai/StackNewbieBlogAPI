using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackNewbieBlog.Data;
using StackNewbieBlog.Models.DTO;
using StackNewbieBlog.Models.Entities;

namespace StackNewbieBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly StackNewbieBlogDbContext dbContext;

        public PostsController(StackNewbieBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get all Posts
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await dbContext.Posts.ToListAsync();
            return Ok(posts);
        }

        // Get single Post
        [HttpGet]
        [ActionName("GetPostById")]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post != null)
            {
                return Ok(post);
            }
            return NotFound();
        }

        // Add Post
        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            // Convert DTO(Data Transfer Object) to Entity
            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Summary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visible = addPostRequest.Visible
            };

            // Create and assign an Id
            post.Id = Guid.NewGuid();
            await dbContext.Posts.AddAsync(post);
            // Save changes to the SQL database
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        // Update Post
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatePostRequest)
        {
            // Convert DTO(Data Transfer Object) to Entity
            var post = new Post()
            {
                Title = updatePostRequest.Title,
                Content = updatePostRequest.Content,
                Author = updatePostRequest.Author,
                FeaturedImageUrl = updatePostRequest.FeaturedImageUrl,
                PublishDate = updatePostRequest.PublishDate,
                UpdatedDate = updatePostRequest.UpdatedDate,
                Summary = updatePostRequest.Summary,
                UrlHandle = updatePostRequest.UrlHandle,
                Visible = updatePostRequest.Visible
            };

            // Check if post exists
            var existingPost = await dbContext.Posts.FindAsync(id);

            if (existingPost != null)
            {
                existingPost.Title = updatePostRequest.Title;
                existingPost.Content = updatePostRequest.Content;
                existingPost.Author = updatePostRequest.Author;
                existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
                existingPost.PublishDate = updatePostRequest.PublishDate;
                existingPost.UpdatedDate = updatePostRequest.UpdatedDate;
                existingPost.Summary = updatePostRequest.Summary;
                existingPost.UrlHandle = updatePostRequest.UrlHandle;
                existingPost.Visible = updatePostRequest.Visible;

                await dbContext.SaveChangesAsync();

                return Ok(existingPost);
            }
            return NotFound();
        }  

        // Delete Post
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingPost = await dbContext.Posts.FindAsync(id);

            if (existingPost != null)
            {
                // Pass in the existingPost entity as an argument
                dbContext.Remove(existingPost);
                await dbContext.SaveChangesAsync();

                return Ok(existingPost);
            }

            return NotFound(); 
        }

    }
}