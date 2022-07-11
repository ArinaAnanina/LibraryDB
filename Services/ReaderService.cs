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
    public class ReaderService
    {
        ApplicationContext _context;
        private readonly ILogger<ReaderController> _logger;

        DbSet<Reader> readerSet
        {
            get
            {
                return _context?.Reader;
            }
        }
        public ReaderService(ILogger<ReaderController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addReader(Reader newReader)
        {
            EntityEntry<Reader> entry = await readerSet.AddAsync(newReader);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public Task<List<Reader>> getReader()
        {
            return readerSet
                .Include(o => o.Person)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Reader> getReader(int id)
        {
            Reader result = await readerSet
                .Where(o => o.Id == id)
                .Include(o => o.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Reader", id);
            }

            return result;
        }
        public async Task updateReader(ReaderRequestForUpdate reader)
        {
            if (reader.Id <= 0 || reader.PersonId <= 0 || reader.SeriesNumberPassport == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Reader tmp = readerSet.Find(reader.Id);
            tmp.PersonId = reader.PersonId;
            tmp.SeriesNumberPassport = reader.SeriesNumberPassport;

            await _context.SaveChangesAsync();
        }
        public async Task removeReader(int? id)
        {
            Reader reader = readerSet.Where(o => o.Id == id).FirstOrDefault();
            if (reader == null)
            {
                throw new NotFoundException("Reader", id);
            }
            EntityEntry<Reader> entry = readerSet.Remove(reader);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
