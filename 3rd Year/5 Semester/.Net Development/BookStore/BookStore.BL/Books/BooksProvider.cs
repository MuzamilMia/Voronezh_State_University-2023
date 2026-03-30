using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.BL.Books.Entities;
using BookStore.DataAccess;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BL.Books
{
    public class BooksProvider : IBooksManager
    {
        private readonly ApplicationDBContext _context;

        public BooksProvider(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookBL>> GetAllAsync()
        {
            var books = await _context.Books
                .Include(b => b.BookType)
                .Include(b => b.User)
                .ToListAsync();

            return books.Select(b => new BookBL
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                Quantity = b.Quantity,
                CreateDate = b.CreateDate,
                TypeId = b.TypeId,
                TypeName = b.BookType?.TypeName,
                UserId = b.UserId,
                UserName = b.User?.UserName
            });
        }

        public async Task<BookBL?> GetByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookType)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookId == id);

            return book == null ? null : new BookBL
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Quantity = book.Quantity,
                CreateDate = book.CreateDate,
                TypeId = book.TypeId,
                TypeName = book.BookType?.TypeName,
                UserId = book.UserId,
                UserName = book.User?.UserName
            };
        }

        public async Task<int> CreateAsync(BookBL model)
        {
            var entity = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Price = model.Price,
                Quantity = model.Quantity,
                TypeId = model.TypeId,
                UserId = model.UserId,
                CreateDate = DateTime.UtcNow
            };

            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
            return entity.BookId; // Return new ID
        }

        public async Task UpdateAsync(int id, BookBL model)
        {
            var entity = await _context.Books.FindAsync(id);
            if (entity == null) return;

            entity.Title = model.Title;
            entity.Author = model.Author;
            entity.Price = model.Price;
            entity.Quantity = model.Quantity;
            entity.TypeId = model.TypeId;
            entity.UserId = model.UserId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Books.FindAsync(id);
            if (entity != null)
            {
                _context.Books.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
