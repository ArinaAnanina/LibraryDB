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
    /// Контроллер для работы с отделами
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> logger;
        private readonly DepartmentService departmentService;

        public DepartmentController(
            ILogger<DepartmentController> logger,
            DepartmentService departmentService
        )
        {
            this.logger = logger;
            this.departmentService = departmentService;

            logger.LogDebug(1, "construct DepartmentController");
        }

        /// <summary>
        /// Получить список отделов
        /// </summary>
        [HttpGet]
        public async Task<List<Department>> DepartmentRead()
        {
            List<Department> departments = await departmentService.getDepartment();

            return departments;
        }

        /// <summary>
        /// Получить отдел по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> DepartmentRead(int id)
        {
            Department department = await departmentService.getDepartment(id);
            return Ok(department);
        }

        /// <summary>
        /// Добавить отдел 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> DepartmentCreate(Department department)
        {
            Department newDepartment = new Department
            {
                Name = department.Name,
            };

            int newId = await departmentService.addDepartment(newDepartment);
            return Ok(newId);
        }

        /// <summary>
        /// Изменить отдел
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> DepartmentUpdate(Department department)
        {
            await departmentService.updateDepartment(department);
            return Ok();
        }

        /// <summary>
        /// Удалить отдела
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DepartmentDelete(int? id)
        {
            await departmentService.removeDepartment(id);
            return Ok();
        }
    }
}
