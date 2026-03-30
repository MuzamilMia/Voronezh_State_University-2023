namespace BookStore.DataAccess.Entities
{
    public class Book
    {
        public int BookId { get; set; }   
        public string Title { get; set; } = "";  
        public string Author { get; set; } = "";  
        public decimal Price { get; set; }       
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public int TypeId { get; set; }
        public BookType? BookType { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Recipt>? Recipts { get; set; }
    }
}
