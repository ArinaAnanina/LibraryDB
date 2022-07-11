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
    public class BookService
    {
        ApplicationContext _context;
        private readonly ILogger<BookController> _logger;

        DbSet<Book> bookSet
        {
            get
            {
                return _context?.Book;
            }
        }
        public BookService(ILogger<BookController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<int> addBook(Book newBook)
        {
            EntityEntry<Book> entry = await bookSet.AddAsync(newBook);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public async Task<List<BookResponse>> getBook()
        {
            List<Book> books = await _context?.Book
               .Include(loc => loc.PublishingHouse)
               .AsNoTracking()
               .ToListAsync();
            var t = await _context.Book_Author
                .Include(x => x.Author)
                .ThenInclude(o => o.Person)
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
            var tmp = await _context.Book_PublicationType
                .Include(x => x.PublicationType)
                .ThenInclude(o => o.Category)
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
            return books.ConvertAll(o => new BookResponse
            {
                Id = o.Id,
                Name = o.Name,
                PublishingHouse = o.PublishingHouse,
                YearOfIssue = o.YearOfIssue,
                Authors = t.Where(f => f.BookId == o.Id).Select(b => b.Author),
                PublicationTypes = tmp.Where(f => f.BookId == o.Id).Select(b => b.PublicationType)
            });
        }

        public async Task<BookResponse> getBook(int id)
        {
            Book b = await bookSet
                .Where(o => o.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            var t = await _context.Book_Author
                .Include(x => x.Author)
                .ThenInclude(o => o.Person)
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
            var tmp = await _context.Book_PublicationType
                .Include(x => x.PublicationType)
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
            BookResponse result = new BookResponse
            {
                Id = b.Id,
                Name = b.Name,
                PublishingHouse = b.PublishingHouse,
                YearOfIssue = b.YearOfIssue,
                Authors = t.Where(f => f.BookId == b.Id).Select(f => f.Author),
                PublicationTypes = tmp.Where(f => f.BookId == b.Id).Select(f => f.PublicationType)
            };
           
            if (result == null)
            {
                throw new NotFoundException("Book", id);
            }

            return result;
        }
        public async Task<List<Book>> getAuthorBook(int authorId)
        {
            List<int> t = await _context.Book_Author
                .Where(o => o.AuthorId == authorId)
                .Include(x => x.Author)
                .Include(x => x.Book)
                .AsNoTracking()
                .Select(o => o.BookId)
                .ToListAsync();
            List<Book> books = await bookSet
               .Where(o => t.Contains(o.Id))
               .Include(loc => loc.PublishingHouse)
               .AsNoTracking()
               .ToListAsync();

            return books;
        }
        public async Task<List<Book>> getPublicationTypeBook(int publicationTypeId)
        {
            List<int> t = await _context.Book_PublicationType
                .Where(o => o.PublicationTypeId == publicationTypeId)
                .Include(x => x.PublicationType)
                .Include(x => x.Book)
                .AsNoTracking()
                .Select(o => o.BookId)
                .ToListAsync();
            List<Book> books = await bookSet
               .Where(o => t.Contains(o.Id))
               .Include(loc => loc.PublishingHouse)
               .AsNoTracking()
               .ToListAsync();

            return books;
        }
        public async Task updateBook(BookRequestForUpdate book)
        {
            if (book.Id <= 0 || book.Name == null || book.PublishingHouseId <= 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Book tmp = bookSet.Find(book.Id);
            tmp.Name = book.Name;
            tmp.PublishingHouseId = book.PublishingHouseId;
            tmp.YearOfIssue = book.YearOfIssue;

            await _context.SaveChangesAsync();
        }
        public async Task removeBook(int? id)
        {
            Book book = bookSet.Where(o => o.Id == id).FirstOrDefault();
            if (book == null)
            {
                throw new NotFoundException("Book", id);
            }
            EntityEntry<Book> entry = bookSet.Remove(book);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
    }
}
