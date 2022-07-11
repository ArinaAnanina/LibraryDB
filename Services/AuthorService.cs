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
    public class AuthorService
    {
        ApplicationContext _context;
        private readonly ILogger<AuthorController> _logger;

        DbSet<Author> authorSet {
            get
            {
                return _context?.Author;
            }
        }
        public AuthorService(ILogger<AuthorController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task updateAuthor(AuthorRequestForUpdate author)
        {
            if (author.Id <= 0 || author.PersonId <= 0 || author.Description == null)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Author auth = authorSet.Find(author.Id);
            auth.PersonId = author.PersonId;
            auth.Description = author.Description;

            await _context.SaveChangesAsync();
        }
        public async Task<int> addAuthor(Author newAuthor)
        {
            EntityEntry<Author> entry = await authorSet.AddAsync(newAuthor);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public async Task removeAuthor(int? id)
        {
            Author author = authorSet.Where(o => o.Id == id).FirstOrDefault();
            if (author == null)
            {
                throw new NotFoundException("Author", id);
            }
            EntityEntry<Author> entry = authorSet.Remove(author);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted) {
                throw new ServerErrorException();
            }
        }
        public Task<List<Author>> getAuthor()
        {
            return authorSet
                .Include(loc => loc.Person)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<Author> getAuthor(int id)
        {
            Author result = await authorSet
                .Where(o => o.Id == id)
                .Include(loc => loc.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Author", id);
            }

            return result;
        }
    }
}
