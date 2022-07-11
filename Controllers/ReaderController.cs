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
    /// Контроллер для работы с читателями
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger<ReaderController> logger;
        private readonly ReaderService readerService;
        public ReaderController(
            ILogger<ReaderController> logger,
            ReaderService readerService
        )
        {
            this.logger = logger;
            this.readerService = readerService;

            logger.LogDebug(1, "construct ReaderController");
        }

        /// <summary>
        /// Получить список читателей
        /// </summary>
        [HttpGet]
        public async Task<List<Reader>> ReaderRead()
        {
            List<Reader> readers = await readerService.getReader();

            return readers;
        }

        /// <summary>
        /// Получить читателя по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> ReaderRead(int id)
        {
            Reader reader = await readerService.getReader(id);
            /*
            AuthorResponse result = new AuthorResponse
            {
                Id = author.Id,
                Description = author.Description,
                Person = author.Person,
                Books = bookService.getAuthorBook(author.Id).Result
            };
            */
            return Ok(reader);
        }

        /// <summary>
        /// Добавить читателя
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ReaderCreate(ReaderRequest reader)
        {
            Reader newReader = new Reader
            {
                PersonId = reader.PersonId,
                SeriesNumberPassport = reader.SeriesNumberPassport
            };

            int newId = await readerService.addReader(newReader);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить читателя
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> ReaderUpdate(ReaderRequestForUpdate reader)
        {
            await readerService.updateReader(reader);
            return Ok();
        }

        /// <summary>
        /// Удалить читателя
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> ReaderDelete(int? id)
        {
            await readerService.removeReader(id);
            return Ok();
        }
    }
}
