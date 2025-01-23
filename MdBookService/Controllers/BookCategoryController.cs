using MdBookService.Data;
using MdBookService.Interfaces;
using MdBookService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MdBookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCategoryController(BookCategoryService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCategory>>> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCategory>> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();

            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<BookCategory>> Create(BookCategory category)
        {
            var createdCategory = await _service.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookCategory category)
        {
            if (id != category.Id) return BadRequest();

            var updatedCategory = await _service.UpdateAsync(category);
            return Ok(updatedCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }

}
