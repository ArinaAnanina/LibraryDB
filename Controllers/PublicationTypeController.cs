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
    /// Контроллер для работы с типами издания
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublicationTypeController : ControllerBase
    {
        private readonly ILogger<PublicationTypeController> logger;
        private readonly PublicationTypeService publicationTypeService;
        private readonly BookService bookService;

        public PublicationTypeController(
            ILogger<PublicationTypeController> logger,
            PublicationTypeService publicationTypeService,
            BookService bookService
        )
        {
            this.logger = logger;
            this.publicationTypeService = publicationTypeService;
            this.bookService = bookService;

            logger.LogDebug(1, "construct  PublicationTypeController");
        }

        /// <summary>
        /// Получить список типов издания
        /// </summary>
        [HttpGet]
        public async Task<List<PublicationTypeResponse>> PublicationTypeRead()
        {
            List<PublicationType> publicationTypes = await publicationTypeService.getPublicationType();

            return publicationTypes.ConvertAll( o => new PublicationTypeResponse { 
                Id = o.Id,
                Name = o.Name,
                CategoryId = o.CategoryId,
                Category = o.Category,
                Books = bookService.getPublicationTypeBook(o.Id).Result,
            });
        }

        /// <summary>
        /// Получить тип издания по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> PublicationTypeRead(int id)
        {
            PublicationType publicationType = await publicationTypeService.getPublicationType(id);
            
            PublicationTypeResponse result = new PublicationTypeResponse
            {

            };
            
            return Ok(result);
        }

        /// <summary>
        /// Добавить тип издания
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PublicationTypeCreate(PublicationTypeRequest publicationType)
        {
            PublicationType newPublicationType = new PublicationType
            {
                Name = publicationType.Name,
                CategoryId = publicationType.CategoryId
            };

            int newId = await publicationTypeService.addPublicationType(newPublicationType);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить тип издания
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> PublicationTypeUpdate(PublicationTypeRequestForUpdate publicationType)
        {
            await publicationTypeService.updatePublicationType(publicationType);
            return Ok();
        }

        /// <summary>
        /// Удалить тип издания
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> PublicationTypeDelete(int? id)
        {
            await publicationTypeService.removePublicationType(id);
            return Ok();
        }
    }
}
