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
    /// Контроллер для работы с работниками
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly EmployeeService employeeService;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            EmployeeService employeeService
        )
        {
            this.logger = logger;
            this.employeeService = employeeService;

            logger.LogDebug(1, "construct EmployeeController");
        }

        /// <summary>
        /// Получить список работников
        /// </summary>
        [HttpGet]
        public async Task<List<Employee>> AuthorRead()
        {
            List<Employee> employees = await employeeService.getEmployee();

            return employees;
        }

        /// <summary>
        /// Получить работника по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> AuthorRead(int id)
        {
            Employee employee = await employeeService.getEmployee(id);
            return Ok(employee);
        }

        /// <summary>
        /// Добавить работника
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> EmployeeCreate(EmployeeRequest employee)
        {
            Employee newEmployee = new Employee
            {
                PersonId = employee.PersonId,
                PostId = employee.PostId,
                Seniority = employee.Seniority,
                SeriesNumberPassport = employee.SeriesNumberPassport,
            };

            int newId = await employeeService.addEmployee(newEmployee);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить работника
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> EmployeeUpdate(EmployeeRequestForUpdate employee)
        {
            await employeeService.updateEmployee(employee);
            return Ok();
        }

        /// <summary>
        /// Удалить работника
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> EmployeeDelete(int? id)
        {
            await employeeService.removeEmployee(id);
            return Ok();
        }
        }
}
