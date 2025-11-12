using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniRumbo.Repositories;

namespace UniRumbo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rol
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Rol.ToListAsync();
            return Ok(roles);
        }
    }
}
