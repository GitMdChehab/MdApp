using MdBookService.Data;
using MdBookService.Interfaces;
using MdBookService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MdBookService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController(IBookService _bookService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllBooks(FilterBookRB request)
        {
            var books = await _bookService.GetAllBooksAsync(request);
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(long id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book request)
        {
            var book = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(long id, [FromBody] Book request)
        {
            if (id != request.Id) return BadRequest();
            var book = await _bookService.UpdateBookAsync(request);
            if (book == null) return NotFound();
            return Ok(book);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
