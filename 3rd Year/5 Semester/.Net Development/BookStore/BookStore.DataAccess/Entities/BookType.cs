using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DataAccess.Entities
{
    public class BookType
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = "";
        public string? Description { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
