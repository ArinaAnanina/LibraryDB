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
    public class StatusTypeService
    {
        ApplicationContext _context;
        private readonly ILogger<StatusTypeController> _logger;

        DbSet<StatusType> statusTypeSet
        {
            get
            {
                return _context?.StatusType;
            }
        }
        public StatusTypeService(ILogger<StatusTypeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addStatusType(StatusType newStatusType)
        {
            EntityEntry<StatusType> entry = await statusTypeSet.AddAsync(newStatusType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<StatusType>> getStatusType()
        {
            return statusTypeSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StatusType> getStatusType(int id)
        {
            StatusType result = await statusTypeSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("StatusType", id);
            }

            return result;
        }
        public async Task updateStatusType(StatusType statusType)
        {
            if (statusType.Name == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            StatusType categ = statusTypeSet.Find(statusType.Id);
            categ.Name = statusType.Name;

            await _context.SaveChangesAsync();
        }
        public async Task removeStatusType(int? id)
        {
            StatusType statusType = statusTypeSet.Where(o => o.Id == id).FirstOrDefault();
            if (statusType == null)
            {
                throw new NotFoundException("StatusType", id);
            }
            EntityEntry<StatusType> entry = statusTypeSet.Remove(statusType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
