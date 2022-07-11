using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryDB.DB;
using LibraryDB.Services;

namespace LibraryDB.Controllers
{
    /// <summary>
    /// Контроллер для работы с должностями
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> logger;
        private readonly PostService postService;

        public PostController(
            ILogger<PostController> logger,
            PostService postService
        )
        {
            this.logger = logger;
            this.postService = postService;

            logger.LogDebug(1, "construct PostController");
        }

        /// <summary>
        /// Получить список должностей
        /// </summary>
        [HttpGet]
        public async Task<List<Post>> PostRead()
        {
            List<Post> posts = await postService.getPost();

            return posts;
        }

        /// <summary>
        /// Получить должность по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> PostRead(int id)
        {
            Post post = await postService.getPost(id);
            return Ok(post);
        }

        /// <summary>
        /// Добавить должность
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostCreate(Post post)
        {
            Post newPost = new Post
            {
                Name = post.Name
            };

            int newId = await postService.addPost(newPost);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить должность
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> PostUpdate(Post post)
        {
            await postService.updatePost(post);
            return Ok();
        }

        /// <summary>
        /// Удалить должность
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> PostDelete(int? id)
        {
            await postService.removePost(id);
            return Ok();
        }
    }
}
