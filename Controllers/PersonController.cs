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
    /// Контроллер для работы с людьми
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> logger;
        private readonly PersonService personService;

        public PersonController(
            ILogger<PersonController> logger,
           PersonService personService
        )
        {
            this.logger = logger;
            this.personService = personService;

            logger.LogDebug(1, "construct PersonController");
        }

        /// <summary>
        /// Получить список людей
        /// </summary>
        [HttpGet]
        public async Task<List<Person>> PersonRead()
        {
            List<Person> persons = await personService.getPerson();

            return persons;
        }

        /// <summary>
        /// Получить челвоека по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> PersonRead(int id)
        {
            Person person = await personService.getPerson(id);
            return Ok(person);
        }

        /// <summary>
        /// Добавить человека
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PersonCreate(Person person)
        {
            Person newPerson = new Person
            {
                Name = person.Name,
                Surname = person.Surname,
                MiddleName = person.MiddleName,
                BirthDate = person.BirthDate,
                Domicile = person.Domicile,
                PhoneNumber = person.PhoneNumber
            };

            int newId = await personService.addPerson(newPerson);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить человека
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> PersonUpdate(Person person)
        {
            await personService.updatePerson(person);
            return Ok();
        }

        /// <summary>
        /// Удалить человека
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> PersonDelete(int? id)
        {
            await personService.removePerson(id);
            return Ok();
        }
    }
}
