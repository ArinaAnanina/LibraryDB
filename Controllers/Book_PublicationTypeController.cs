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
    /// Контроллер для работы со связями книг с типами издания
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Book_PublicationTypeController : ControllerBase
    {
        private readonly ILogger<Book_PublicationTypeController> logger;
        private readonly Book_PublicationTypeService book_PublicationTypeService;

        public Book_PublicationTypeController(
            ILogger<Book_PublicationTypeController> logger,
            Book_PublicationTypeService book_PublicationTypeService
        )
        {
            this.logger = logger;
            this.book_PublicationTypeService = book_PublicationTypeService;

            logger.LogDebug(1, "construct Book_PublicationType");
        }

        /// <summary>
        /// Получить список связей книг с типом издания
        /// </summary>
        [HttpGet]
        public async Task<List<Book_PublicationType>> Book_PublicationTypeRead()
        {
            List<Book_PublicationType> bp = await book_PublicationTypeService.getBook_PublicationType();

            return bp;
        }

        /// <summary>
        /// Получить связь по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> Book_PublicationTypeRead(int id)
        {
            Book_PublicationType bp = await book_PublicationTypeService.getBook_PublicationType(id);
            return Ok(bp);
        }

        /// <summary>
        /// Добавить связь
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Book_PublicationTypeCreate(Book_PublicationType bp)
        {
            Book_PublicationType newBook_PublicationType = new Book_PublicationType
            {
                PublicationTypeId = bp.PublicationTypeId,
                BookId = bp.BookId,
            };

            int newId = await book_PublicationTypeService.addBook_PublicationType(newBook_PublicationType);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить связь
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> Book_PublicationTypeUpdate(Book_PublicationType bp)
        {
            await book_PublicationTypeService.updateBook_PublicationType(bp);
            return Ok();
        }

        /// <summary>
        /// Удалить связь
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Book_PublicationTypeDelete(int? id)
        {
            await book_PublicationTypeService.removeBook_PublicationType(id);
            return Ok();
        }
    }
}
