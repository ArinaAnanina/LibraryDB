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
    /// Контроллер для работы с экземплярами книг
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InstanceController : ControllerBase
    {
        private readonly ILogger<InstanceController> logger;
        private readonly InstanceService instanceService;

        public InstanceController(
            ILogger<InstanceController> logger,
            InstanceService instanceService
        )
        {
            this.logger = logger;
            this.instanceService = instanceService;

            logger.LogDebug(1, "construct InstanceController");
        }

        /// <summary>
        /// Получить список экземпляров книг
        /// </summary>
        [HttpGet]
        public async Task<List<Instance>> InstanceRead()
        {
            List<Instance> instances = await instanceService.getInstance();

            return instances;
        }

        /// <summary>
        /// Получить экземпляр книги по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> InstanceRead(int id)
        {
            Instance instance = await instanceService.getInstance(id);
            /*
            AuthorResponse result = new AuthorResponse
            {
                Id = author.Id,
                Description = author.Description,
                Person = author.Person,
                Books = bookService.getAuthorBook(author.Id).Result
            };
            */
            return Ok(instance);
        }

        /// <summary>
        /// Добавить экземпляр книги
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> InstanceCreate(InstanceRequest instance)
        {
            Instance newInstance = new Instance
            {
                BookId = instance.BookId,
                DepartmentId = instance.DepartmentId,
                Code = instance.Code,
                Availabilities = instance.Availabilities,
                ReasonForLack = instance.ReasonForLack,
            };

            int newId = await instanceService.addInstance(newInstance);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить экземпляр книги
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> InstanceUpdate(InstanceRequestForUpdate instance)
        {
            await instanceService.updateInstance(instance);
            return Ok();
        }

        /// <summary>
        /// Удалить автора
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> InstanceDelete(int? id)
        {
            await instanceService.removeInstance(id);
            return Ok();
        }
    }
}
