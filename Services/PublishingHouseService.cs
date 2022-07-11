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
    public class PublishingHouseService
    {
        ApplicationContext _context;
        private readonly ILogger<PublishingHouseController> _logger;

        DbSet<PublishingHouse> publishingHouseSet
        {
            get
            {
                return _context?.PublishingHouse;
            }
        }
        public PublishingHouseService(ILogger<PublishingHouseController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addPublishingHouse(PublishingHouse newPublishingHouse)
        {
            EntityEntry<PublishingHouse> entry = await publishingHouseSet.AddAsync(newPublishingHouse);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<PublishingHouse>> getPublishingHouse()
        {
            return publishingHouseSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PublishingHouse> getPublishingHouse(int id)
        {
            PublishingHouse result = await publishingHouseSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("PublishingHouse", id);
            }

            return result;
        }
        public async Task updatePublishingHouse(PublishingHouse publishingHouse)
        {
            if (publishingHouse.Name == null || publishingHouse.Code <= 0 || publishingHouse.Location == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            PublishingHouse pubHouse = publishingHouseSet.Find(publishingHouse.Id);
            pubHouse.Code = publishingHouse.Code;
            pubHouse.Name = publishingHouse.Name;
            pubHouse.Location = publishingHouse.Location;

            await _context.SaveChangesAsync();
        }
        public async Task removePublishingHouse(int? id)
        {
            PublishingHouse publishingHouse = publishingHouseSet.Where(o => o.Id == id).FirstOrDefault();
            if (publishingHouse == null)
            {
                throw new NotFoundException("PublishingHouse", id);
            }
            EntityEntry<PublishingHouse> entry = publishingHouseSet.Remove(publishingHouse);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
