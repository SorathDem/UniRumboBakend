using Microsoft.EntityFrameworkCore;
using UniRumbo.Repositories;

namespace UniRumbo.Repositories
{
    public class AlojamientoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlojamientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alojamiento>> GetAllAsync()
        {
            return await _context.Alojamiento
                .Include(a => a.IdUsuarioNavigation)
                .ToListAsync();
        }

        public async Task<Alojamiento?> GetByIdAsync(int id)
        {
            return await _context.Alojamiento
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefaultAsync(a => a.IdAlojamiento == id);
        }

        public async Task AddAsync(Alojamiento alojamiento)
        {
            _context.Alojamiento.Add(alojamiento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Alojamiento alojamiento)
        {
            _context.Alojamiento.Update(alojamiento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Alojamiento alojamiento)
        {
            _context.Alojamiento.Remove(alojamiento);
            await _context.SaveChangesAsync();
        }
    }
}
