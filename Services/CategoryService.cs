using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDB.Controllers;
using LibraryDB.DB;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LibraryDB.Exceptions;

namespace LibraryDB.Services
{
    public class CategoryService
    {
        ApplicationContext _context;
        private readonly ILogger<CategoryController> _logger;

        DbSet<Category> categorySet
        {
            get
            {
                return _context?.Category;
            }
        }
        public CategoryService(ILogger<CategoryController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addCategory(Category newCategory)
        {
            EntityEntry<Category> entry = await categorySet.AddAsync(newCategory);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Category>> getCategory()
        {
            return categorySet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category> getCategory(int id)
        {
            Category result = await categorySet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Category", id);
            }

            return result;
        }
        public async Task updateCategory(Category category)
        {
            if (category.Name == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Category categ = categorySet.Find(category.Id);
            categ.Name = category.Name;

            await _context.SaveChangesAsync();
        }
        public async Task removeCategory(int? id)
        {
            Category category = categorySet.Where(o => o.Id == id).FirstOrDefault();
            if (category == null)
            {
                throw new NotFoundException("Category", id);
            }
            EntityEntry<Category> entry = categorySet.Remove(category);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
