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
    /// Контроллер для работы с авторами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> logger;
        private readonly AuthorService authorService;
        private readonly BookService bookService;

        public AuthorController(
            ILogger<AuthorController> logger,
            AuthorService authorService,
            BookService bookService
        ) 
        {
            this.logger = logger;
            this.authorService = authorService;
            this.bookService = bookService;

            logger.LogDebug(1, "construct AuthorController");
        }

        /// <summary>
        /// Получить список авторов
        /// </summary>
        [HttpGet]
        public async Task<List<AuthorResponse>> AuthorRead()
        {
            List<Author> authors = await authorService.getAuthor();

            return authors.ConvertAll(o => new AuthorResponse
            {
                Id = o.Id,
                Description = o.Description,
                Person = o.Person,
                Books = bookService.getAuthorBook(o.Id).Result
            });
        }

        /// <summary>
        /// Получить автора по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> AuthorRead(int id)
        {
            Author author = await authorService.getAuthor(id);
            AuthorResponse result = new AuthorResponse
            {
                Id = author.Id,
                Description = author.Description,
                Person = author.Person,
                Books = bookService.getAuthorBook(author.Id).Result
            };
            return Ok(result);
        }

        /// <summary>
        /// Добавить автора
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AuthorCreate(AuthorRequest author)
        {
            Author newAuthor = new Author
            {
                PersonId = author.PersonId,
                Description = author.Description,
            };

            int newId = await authorService.addAuthor(newAuthor);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить автора
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> AuthorUpdate(AuthorRequestForUpdate author)
        {
            await authorService.updateAuthor(author);
            return Ok();
        }

        /// <summary>
        /// Удалить автора
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> AuthorDelete(int? id)
        {
            await authorService.removeAuthor(id);
            return Ok();
        }
    }
}
