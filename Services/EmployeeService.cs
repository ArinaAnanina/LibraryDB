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
    public class EmployeeService
    {
        ApplicationContext _context;
        private readonly ILogger<EmployeeController> _logger;

        DbSet<Employee> employeeSet
        {
            get
            {
                return _context?.Employee;
            }
        }
        public EmployeeService(ILogger<EmployeeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addEmployee(Employee newEmployee)
        {
            EntityEntry<Employee> entry = await employeeSet.AddAsync(newEmployee);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Employee>> getEmployee()
        {
            return employeeSet
                .Include(o => o.Person)
                .Include(o => o.Post)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> getEmployee(int id)
        {
            Employee result = await employeeSet
                .Where(o => o.Id == id)
                .Include(o => o.Person)
                .Include(o => o.Post)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Employee", id);
            }

            return result;
        }
        public async Task updateEmployee(EmployeeRequestForUpdate employee)
        {
            if (employee.Id <= 0 || employee.PersonId <= 0 || employee.PostId <= 0 || employee.SeriesNumberPassport == null || employee.Seniority < 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Employee tmp = employeeSet.Find(employee.Id);
            tmp.PersonId = employee.PersonId;
            tmp.PostId = employee.PostId;
            tmp.SeriesNumberPassport = employee.SeriesNumberPassport;
            tmp.Seniority = employee.Seniority;

            await _context.SaveChangesAsync();
        }
        public async Task removeEmployee(int? id)
        {
            Employee employee = employeeSet.Where(o => o.Id == id).FirstOrDefault();
            if (employee == null)
            {
                throw new NotFoundException("Employee", id);
            }
            EntityEntry<Employee> entry = employeeSet.Remove(employee);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
