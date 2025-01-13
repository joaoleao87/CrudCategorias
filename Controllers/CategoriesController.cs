using Microsoft.AspNetCore.Mvc;
using CrudCategorias.Data;
using CrudCategorias.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (category.Id != 0)
        {
            return BadRequest(new { message = "O campo 'id' não deve ser enviado ou deve ser igual a 0." });
        }

        if (string.IsNullOrWhiteSpace(category.Title) || string.IsNullOrWhiteSpace(category.Description))
        {
            return BadRequest(new { message = "Os campos 'Title' e 'Description' são obrigatórios." });
        }

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest(new { message = "O ID no corpo deve corresponder ao ID no endpoint." });
        }

        if (string.IsNullOrWhiteSpace(category.Title) || string.IsNullOrWhiteSpace(category.Description))
        {
            return BadRequest(new { message = "Os campos 'Title' e 'Description' são obrigatórios." });
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(c => c.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound(new { message = "Categoria não encontrada." });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
