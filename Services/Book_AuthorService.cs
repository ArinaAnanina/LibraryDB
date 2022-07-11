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
    public class Book_AuthorService
    {
        ApplicationContext _context;
        private readonly ILogger<Book_AuthorController> _logger;

        DbSet<Book_Author> book_AuthorSet
        {
            get
            {
                return _context?.Book_Author;
            }
        }
        public Book_AuthorService(ILogger<Book_AuthorController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task updateBook_Author(Book_Author book_Author)
        {
            if (book_Author.AuthorId <= 0 || book_Author.BookId <= 0)
            {
                throw new BadRequestException("неверно задан параметр");
            }
            Book_Author tmp = book_AuthorSet.Find(book_Author.Id);
            tmp.AuthorId = book_Author.AuthorId;
            tmp.BookId = book_Author.BookId;

            await _context.SaveChangesAsync();
        }
        public async Task<int> addBook_Author(Book_Author newBook_Author)
        {
            EntityEntry<Book_Author> entry = await book_AuthorSet.AddAsync(newBook_Author);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Added)
            {
                throw new ServerErrorException();
            }

            return entry.Entity.Id;
        }
        public async Task removeBook_Author(int? id)
        {
            Book_Author book_Author = book_AuthorSet.Where(o => o.Id == id).FirstOrDefault();
            if (book_Author == null)
            {
                throw new NotFoundException("Book_Author", id);
            }
            EntityEntry<Book_Author> entry = book_AuthorSet.Remove(book_Author);
            EntityState responseState = entry.State;

            await _context?.SaveChangesAsync();

            if (responseState != EntityState.Deleted)
            {
                throw new ServerErrorException();
            }
        }
        public Task<List<Book_Author>> getBook_Author()
        {
            return book_AuthorSet
                .Include(o => o.Author)
                .ThenInclude(f => f.Person)
                .Include(o => o.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book_Author> getBook_Author(int id)
        {
            Book_Author result = await book_AuthorSet
                .Where(o => o.Id == id)
                .Include(o => o.Author)
                 .ThenInclude(f => f.Person)
                .Include(o => o.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException("Book_Author", id);
            }

            return result;
        }
    }
}
