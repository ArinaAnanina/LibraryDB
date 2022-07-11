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
    /// Контроллер для работы с книгами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> logger;
        private readonly AuthorService authorService;
        private readonly BookService bookService;

        public BookController(
            ILogger<BookController> logger,
            AuthorService authorService,
            BookService bookService
        )
        {
            this.logger = logger;
            this.authorService = authorService;
            this.bookService = bookService;

            logger.LogDebug(1, "construct BookController");
        }
        /// <summary>
        /// Получить список книг
        /// </summary>
        [HttpGet]
        public async Task<List<BookResponse>> BookRead()
        {
            return await bookService.getBook();
        }

        /// <summary>
        /// Получить книгу по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> BookRead(int id)
        {
            BookResponse book = await bookService.getBook(id);
            return Ok(book);
        }
        /// <summary>
        /// Добавить книгу
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> BookCreate(BookRequest book)
        {
            Book newBook = new Book
            {
                PublishingHouseId = book.PublishingHouseId,
                Name = book.Name,
                YearOfIssue = book.YearOfIssue,
            };

            int newId = await bookService.addBook(newBook);
            return Ok(newId);
        }
        /// <summary>
        /// Изменить книгу
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> BookUpdate(BookRequestForUpdate book)
        {
            await bookService.updateBook(book);
            return Ok();
        }

        /// <summary>
        /// Удалить книгу
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> BookDelete(int? id)
        {
            await bookService.removeBook(id);
            return Ok();
        }
       
    }
}
