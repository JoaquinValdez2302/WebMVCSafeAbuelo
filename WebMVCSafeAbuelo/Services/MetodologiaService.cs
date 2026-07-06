using Microsoft.EntityFrameworkCore;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Services
{
    public class MetodologiaService : IMetodologiaService
    {
        private readonly ApplicationDbContext _context;

        public MetodologiaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MetodologiaAtaque>> ObtenerTodosAsync()
        {
            return await _context.MetodologiaAtaque.ToListAsync();
        }

        public async Task<(IEnumerable<MetodologiaAtaque> Metodologias, int TotalPaginas)> ObtenerPaginadosAsync(string buscar, int pagina, int cantidadPorPagina)
        {
            var query = _context.MetodologiaAtaque.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                buscar = buscar.ToLower();
                query = query.Where(m => m.Nombre.ToLower().Contains(buscar) || m.Descripcion.ToLower().Contains(buscar));
            }

            int totalRegistros = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling(totalRegistros / (double)cantidadPorPagina);
            if (totalPaginas == 0) totalPaginas = 1;

            var metodologias = await query
                .OrderBy(m => m.Nombre)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            return (metodologias, totalPaginas);
        }

        public async Task<MetodologiaAtaque?> ObtenerPorIdAsync(int? id) => await _context.MetodologiaAtaque.FindAsync(id);

        public async Task CrearAsync(MetodologiaAtaque metodologia)
        {
            _context.Add(metodologia);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(MetodologiaAtaque metodologia)
        {
            _context.Update(metodologia);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var m = await _context.MetodologiaAtaque.FindAsync(id);
            if (m != null) { _context.MetodologiaAtaque.Remove(m); await _context.SaveChangesAsync(); }
        }

        public bool Existe(int? id) => _context.MetodologiaAtaque.Any(e => e.Id == id);
    }
}