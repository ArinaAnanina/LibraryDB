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
    public class InstanceService
    {
        ApplicationContext _context;
        private readonly ILogger<InstanceController> _logger;

        DbSet<Instance> instanceSet
        {
            get
            {
                return _context?.Instance;
            }
        }
        public InstanceService(ILogger<InstanceController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addInstance(Instance newInstance)
        {
            EntityEntry<Instance> entry = await instanceSet.AddAsync(newInstance);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Instance>> getInstance()
        {
            return instanceSet
                .Include(o => o.Book)
                .Include(o => o.Department)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Instance> getInstance(int id)
        {
            Instance result = await instanceSet
                .Where(o => o.Id == id)
                .Include(o => o.Book)
                .Include(o => o.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Instance", id);
            }

            return result;
        }
        public async Task updateInstance(InstanceRequestForUpdate instance)
        {
            if (instance.BookId <= 0 || instance.Code <= 0 || instance.DepartmentId <= 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Instance tmp = instanceSet.Find(instance.Id);
            tmp.Availabilities = instance.Availabilities;
            tmp.BookId = instance.BookId;
            tmp.Code = instance.Code;
            tmp.DepartmentId = instance.DepartmentId;
            tmp.ReasonForLack = instance.ReasonForLack;

            await _context.SaveChangesAsync();
        }
        public async Task removeInstance(int? id)
        {
            Instance instance = instanceSet.Where(o => o.Id == id).FirstOrDefault();
            if (instance == null)
            {
                throw new NotFoundException("Instance", id);
            }
            EntityEntry<Instance> entry = instanceSet.Remove(instance);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
