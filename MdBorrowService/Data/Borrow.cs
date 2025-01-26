using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MdBorrowService.Data
{
    public class Borrow
    {
        [Key]
        public long Id { get; set; }
        public long BookId { get; set; } 
        public int UserId { get; set; } 
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public BorrowStatus Status { get; set; } 
        //public Book Book { get; set; }
        //public User User { get; set; }
    }
    public enum BorrowStatus
    {
        Pending,  // Book is currently borrowed
        Returned, // Book has been returned
        Overdue   // Book is overdue
    }

}