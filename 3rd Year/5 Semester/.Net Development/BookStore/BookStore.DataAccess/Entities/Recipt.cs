using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DataAccess.Entities
{
    public class Recipt
    {
        public int ReciptId { get; set; }
        public string BillNumber { get; set; } = "";
        public int UserId { get; set; }
        public User? User { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public DateTime BillDate { get; set; } = DateTime.UtcNow;
        public string PaymentType { get; set; } = "";
    }
}
