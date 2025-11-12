using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniRumbo.Repositories;

namespace UniRumbo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SedeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SedeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Sede
        [HttpGet]
        public async Task<IActionResult> GetSedes()
        {
            var sedes = await _context.Sede.ToListAsync();
            return Ok(sedes);
        }
    }
}
