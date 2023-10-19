namespace lab6.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]

public class ElectroScooterController : ControllerBase
{
    private readonly AppDbContext _context;

    public ElectroScooterController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ElectroScooter>>> GetScooters()
    {
        var scooters = await _context.Scooters.ToListAsync();
        return Ok(scooters);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ElectroScooter>> GetScooter(int id)
    {
        var scooter = await _context.Scooters.FirstOrDefaultAsync(s => s.Id == id);
        if (scooter == null)
        {
            return NotFound();
        }
        return Ok(scooter);
    }

    [HttpPost]
    public async Task<ActionResult<ElectroScooter>> CreateScooter([FromBody] ElectroScooter scooter)
    {
        if (scooter == null)
        {
            return BadRequest();
        }
        var createdScooter = _context.Scooters.Add(scooter).Entity;
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetScooter), new { id = scooter.Id }, createdScooter);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ElectroScooter>> UpdateScooter(int id, [FromBody] ElectroScooter scooter)
    {
        if (scooter == null)
        {
            return BadRequest();
        }
        var scooterToUpdate = await _context.Scooters.FirstOrDefaultAsync(s => s.Id == id);
        if (scooterToUpdate == null)
        {
            return NotFound();
        }
        scooterToUpdate.Name = scooter.Name;
        scooterToUpdate.BatteryLevel = scooter.BatteryLevel;
        _context.Scooters.Update(scooterToUpdate);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ElectroScooter>> DeleteScooter(int id, [FromHeader] string password)
    {
        var scooter = await _context.Scooters.FirstOrDefaultAsync(s => s.Id == id);

        if (scooter == null)
        {
            return NotFound();
        }

        string hardcodedPassword = "admin";
        if (password != hardcodedPassword)
        {
            return Unauthorized();
        }

        _context.Scooters.Remove(scooter);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}