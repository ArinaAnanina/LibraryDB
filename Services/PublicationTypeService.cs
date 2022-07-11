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
    public class PublicationTypeService
    {
        ApplicationContext _context;
        private readonly ILogger<PublicationTypeController> _logger;

        DbSet<PublicationType> publicationTypeSet
        {
            get
            {
                return _context?.PublicationType;
            }
        }
        public PublicationTypeService(ILogger<PublicationTypeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addPublicationType(PublicationType newPublicationType)
        {
            EntityEntry<PublicationType> entry = await publicationTypeSet.AddAsync(newPublicationType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<PublicationType>> getPublicationType()
        {
            return publicationTypeSet
                .Include(o => o.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PublicationType> getPublicationType(int id)
        {
            PublicationType result = await publicationTypeSet
                .Where(o => o.Id == id)
                .Include(o => o.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("PublicationType", id);
            }

            return result;
        }
        public async Task updatePublicationType(PublicationTypeRequestForUpdate publicationType)
        {
            if (publicationType.Id <= 0 || publicationType.Name == null || publicationType.CategoryId <= 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            PublicationType pubType = publicationTypeSet.Find(publicationType.Id);
            pubType.Name = publicationType.Name;
            pubType.CategoryId = publicationType.CategoryId;

            await _context.SaveChangesAsync();
        }
        public async Task removePublicationType(int? id)
        {
            PublicationType publicationType = publicationTypeSet.Where(o => o.Id == id).FirstOrDefault();
            if (publicationType == null)
            {
                throw new NotFoundException("PublicationType", id);
            }
            EntityEntry<PublicationType> entry = publicationTypeSet.Remove(publicationType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
