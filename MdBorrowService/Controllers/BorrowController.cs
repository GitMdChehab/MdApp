using MdBorrowService.Interfaces;
using MdBorrowService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using MdBorrowService.Data;

namespace MdBorrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        // Get the token and UserID from the request
        private string Token => Request.Headers["Authorization"].ToString();
        private int UserID => int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

        // GET: api/borrow/available-books
        [HttpGet("available-books")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var books = await _borrowService.AvailableBooksAsync(Token);
            if (books == null)
                return NotFound("No books are available for borrowing.");

            return Ok(books);
        }

        // GET: api/borrow/{userId}/borrows
        [HttpGet("borrows")]
        public async Task<IActionResult> GetBorrows()
        {
            var borrows = await _borrowService.BorrowsAsync(UserID, Token); // `0` is just a placeholder for bookId
            if (borrows == null || borrows.Count == 0)
                return NotFound("No borrow records found.");

            return Ok(borrows);
        }

        // POST: api/borrow/reserve-book
        [HttpPost("reserve-book")]
        public async Task<IActionResult> ReserveBook([FromBody] long bookID)
        {
            var result = await _borrowService.ReserveBookAsync(bookID, UserID, Token);
            if (result)
                return Ok("Book reserved successfully.");
            return BadRequest("Unable to reserve the book.");
        }

        // POST: api/borrow/borrow-book
        [HttpPost("borrow-book")]
        public async Task<IActionResult> BorrowBook([FromBody] long bookID)
        {

            var result = await _borrowService.BorrowBookAsync(bookID, UserID, Token);
            if (result)
                return Ok("Book borrowed successfully.");
            return BadRequest("Unable to borrow the book.");
        }

        // POST: api/borrow/return-book
        [HttpPost("return-book")]

        public async Task<IActionResult> ReturnBook([FromBody] long bookID)
        {
            var result = await _borrowService.ReturnBookAsync(bookID, Token);
            if (result)
                return Ok("Book returned successfully.");
            return BadRequest("Unable to return the book.");
        }        

    }
}