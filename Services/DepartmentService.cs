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
    public class DepartmentService
    {
        ApplicationContext _context;
        private readonly ILogger<DepartmentController> _logger;

        DbSet<Department> departmentSet
        {
            get
            {
                return _context?.Department;
            }
        }
        public DepartmentService(ILogger<DepartmentController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addDepartment(Department newDepartment)
        {
            EntityEntry<Department> entry = await departmentSet.AddAsync(newDepartment);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Department>> getDepartment()
        {
            return departmentSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Department> getDepartment(int id)
        {
            Department result = await departmentSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Department", id);
            }

            return result;
        }
        public async Task updateDepartment(Department department)
        {
            if (department.Name == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Department dep = departmentSet.Find(department.Id);
            dep.Name = department.Name;

            await _context.SaveChangesAsync();
        }
        public async Task removeDepartment(int? id)
        {
            Department department = departmentSet.Where(o => o.Id == id).FirstOrDefault();
            if (department == null)
            {
                throw new NotFoundException("Category", id);
            }
            EntityEntry<Department> entry = departmentSet.Remove(department);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
