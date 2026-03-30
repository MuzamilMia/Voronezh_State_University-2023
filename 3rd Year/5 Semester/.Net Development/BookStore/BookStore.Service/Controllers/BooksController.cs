using Microsoft.AspNetCore.Mvc;
using BookStore.BL.Books;
using BookStore.BL.Books.Entities;

namespace BookStore.Service.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksManager _booksManager;

        public BooksController(IBooksManager booksManager)
        {
            _booksManager = booksManager;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _booksManager.GetAllAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var book = await _booksManager.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookBL book)
        {
            if (ModelState.IsValid)
            {
                // Default values for demo
                book.TypeId = 1;
                book.UserId = 1;
                await _booksManager.CreateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var book = await _booksManager.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookBL book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                // FIX: Preserve foreign keys from DB (don't let form change them)
                var existingBook = await _booksManager.GetByIdAsync(id);
                if (existingBook == null) return NotFound();

                book.TypeId = existingBook.TypeId;  // Keep original TypeId
                book.UserId = existingBook.UserId;  // Keep original UserId

                await _booksManager.UpdateAsync(id, book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _booksManager.GetByIdAsync(id.Value);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _booksManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
