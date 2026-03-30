using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.BL.Books.Entities;

namespace BookStore.BL.Books
{
    public interface IBooksManager
    {
        Task<IEnumerable<BookBL>> GetAllAsync();
        Task<BookBL?> GetByIdAsync(int id);
        Task<int> CreateAsync(BookBL book);     // Returns new BookId
        Task UpdateAsync(int id, BookBL book);
        Task DeleteAsync(int id);
    }
}
