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
    /// Контроллер для работы с категориями
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> logger;
        private readonly CategoryService categoryService;

        public CategoryController(ILogger<CategoryController> logger, CategoryService categoryService)
        {
            this.logger = logger;
            this.categoryService = categoryService;

            logger.LogDebug(1, "construct CategoryController");
        }

        /// <summary>
        /// Получить список категорий
        /// </summary>
        [HttpGet]
        public async Task<List<Category>> CategoryRead()
        {
            return await categoryService.getCategory();
        }
       
        /// <summary>
        /// Получить категорию по ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult> CategoryRead(int id)
        {
            Category category = await categoryService.getCategory(id);
            return Ok(category);
        }
        /// <summary>
        /// Добавить категорию
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CategoryCreate(Category category)
        {
            Category newCategory = new Category
            {
                Name = category.Name,
            };

            int newId = await categoryService.addCategory(newCategory);
            return Ok(newId);
        }
        /// <summary>
        /// Изменить категорию
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> CategoryUpdate(Category category)
        {
            await categoryService.updateCategory(category);
            return Ok();
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> CategoryDelete(int? id)
        {
            await categoryService.removeCategory(id);
            return Ok();
        }
    }

}
