using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryDB.DB;
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
    /// Контроллер для работы со связями книг с авторами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Book_AuthorController : ControllerBase
    {
        private readonly ILogger<Book_AuthorController> logger;
        private readonly Book_AuthorService book_AuthorService;

        public Book_AuthorController(
            ILogger<Book_AuthorController> logger,
            Book_AuthorService book_AuthorService
        )
        {
            this.logger = logger;
            this.book_AuthorService = book_AuthorService;

            logger.LogDebug(1, "construct Book_Author");
        }

        /// <summary>
        /// Получить список связей авторов с книгами
        /// </summary>
        [HttpGet]
        public async Task<List<Book_Author>> Book_AuthorRead()
        {
            List<Book_Author> ba = await book_AuthorService.getBook_Author();

            return ba;
        }

        /// <summary>
        /// Получить связь по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> Book_AuthorRead(int id)
        {
            Book_Author ba = await book_AuthorService.getBook_Author(id);
            return Ok(ba);
        }

        /// <summary>
        /// Добавить связь
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Book_AuthorCreate(Book_Author ba)
        {
            Book_Author newBook_Author = new Book_Author
            {
                AuthorId = ba.AuthorId,
                BookId = ba.BookId,
            };

            int newId = await book_AuthorService.addBook_Author(newBook_Author);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить связь
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> Book_AuthorUpdate(Book_Author ba)
        {
            await book_AuthorService.updateBook_Author(ba);
            return Ok();
        }

        /// <summary>
        /// Удалить связь
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Book_AuthorDelete(int? id)
        {
            await book_AuthorService.removeBook_Author(id);
            return Ok();
        }
    }
}
