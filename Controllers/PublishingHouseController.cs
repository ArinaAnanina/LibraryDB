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
    /// Контроллер для работы с издательствами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublishingHouseController : ControllerBase
    {
        private readonly ILogger<AuthorController> logger;
        private readonly PublishingHouseService publishingHouseService;

        public PublishingHouseController(
            ILogger<AuthorController> logger,
            PublishingHouseService publishingHouseService
        )
        {
            this.logger = logger;
            this.publishingHouseService = publishingHouseService;

            logger.LogDebug(1, "construct PublishingHouseController");
        }

        /// <summary>
        /// Получить список издательств
        /// </summary>
        [HttpGet]
        public async Task<List<PublishingHouse>> PublishingHouseRead()
        {
            List<PublishingHouse> publishingHouses = await publishingHouseService.getPublishingHouse();

            return publishingHouses;
        }

        /// <summary>
        /// Получить издательство по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> PublishingHouseRead(int id)
        {
            PublishingHouse publishingHouse = await publishingHouseService.getPublishingHouse(id);
            return Ok(publishingHouse);
        }

        /// <summary>
        /// Добавить издательство
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PublishingHouseCreate(PublishingHouse publishingHouse)
        {
            PublishingHouse newPublishingHouse = new PublishingHouse
            {
                Name = publishingHouse.Name,
                Code = publishingHouse.Code,
                Location = publishingHouse.Location
            };

            int newId = await publishingHouseService.addPublishingHouse(newPublishingHouse);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить издательство
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> PublishingHouseUpdate(PublishingHouse publishingHouse)
        {
            await publishingHouseService.updatePublishingHouse(publishingHouse);
            return Ok();
        }

        /// <summary>
        /// Удалить издательство
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> PublishingHouseDelete(int? id)
        {
            await publishingHouseService.removePublishingHouse(id);
            return Ok();
        }
    }
}
