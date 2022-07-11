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
    /// Контроллер для работы со статусами книг
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StatusTypeController : ControllerBase
    {
        private readonly ILogger<StatusTypeController> logger;
        private readonly StatusTypeService statusTypeService;

        public StatusTypeController(
            ILogger<StatusTypeController> logger,
            StatusTypeService statusTypeService
        )
        {
            this.logger = logger;
            this.statusTypeService = statusTypeService;

            logger.LogDebug(1, "construct StatusTypeController");
        }

        /// <summary>
        /// Получить список статусов книг
        /// </summary>
        [HttpGet]
        public async Task<List<StatusType>> StatusTypeRead()
        {
            List<StatusType> statusTypes = await statusTypeService.getStatusType();

            return statusTypes;
        }

        /// <summary>
        /// Получить статус по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> StatusTypeRead(int id)
        {
            StatusType statusType = await statusTypeService.getStatusType(id);
            return Ok(statusType);
        }

        /// <summary>
        /// Добавить статус
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> StatusTypeCreate(StatusType statusType)
        {
            StatusType newStatusType = new StatusType
            {
                Name = statusType.Name
            };

            int newId = await statusTypeService.addStatusType(newStatusType);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить статус
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> StatusTypeUpdate(StatusType statusType)
        {
            await statusTypeService.updateStatusType(statusType);
            return Ok();
        }

        /// <summary>
        /// Удалить статус
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> StatusTypeDelete(int? id)
        {
            await statusTypeService.removeStatusType(id);
            return Ok();
        }
    }
}
