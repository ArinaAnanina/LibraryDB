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
    public class Book_PublicationTypeService
    {
        ApplicationContext _context;
        private readonly ILogger<Book_PublicationTypeController> _logger;

        DbSet<Book_PublicationType> book_PublicationTypeSet
        {
            get
            {
                return _context?.Book_PublicationType;
            }
        }
        public Book_PublicationTypeService(ILogger<Book_PublicationTypeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task updateBook_PublicationType(Book_PublicationType book_PublicationType)
        {
            if (book_PublicationType.BookId <= 0 || book_PublicationType.PublicationTypeId <= 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Book_PublicationType tmp = book_PublicationTypeSet.Find(book_PublicationType.Id);
            tmp.BookId = book_PublicationType.BookId;
            tmp.PublicationTypeId = book_PublicationType.PublicationTypeId;

            await _context.SaveChangesAsync();
        }
        public async Task<int> addBook_PublicationType(Book_PublicationType newBook_PublicationType)
        {
            EntityEntry<Book_PublicationType> entry = await book_PublicationTypeSet.AddAsync(newBook_PublicationType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public async Task removeBook_PublicationType(int? id)
        {
            Book_PublicationType book_PublicationType = book_PublicationTypeSet.Where(o => o.Id == id).FirstOrDefault();
            if (book_PublicationType == null)
            {
                throw new NotFoundException("Book_PublicationType", id);
            }
            EntityEntry<Book_PublicationType> entry = book_PublicationTypeSet.Remove(book_PublicationType);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
        public Task<List<Book_PublicationType>> getBook_PublicationType()
        {
            return book_PublicationTypeSet
                .Include(loc => loc.PublicationType)
                .Include(loc => loc.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book_PublicationType> getBook_PublicationType(int id)
        {
            Book_PublicationType result = await book_PublicationTypeSet
                .Where(o => o.Id == id)
                .Include(loc => loc.PublicationType)
                .Include(loc => loc.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Book_PublicationType", id);
            }

            return result;
        }
    }
}
