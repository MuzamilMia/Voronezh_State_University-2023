using System.ComponentModel.DataAnnotations;

namespace BookStore.BL.Books.Entities
{
    public class BookBL
    {
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }
}
