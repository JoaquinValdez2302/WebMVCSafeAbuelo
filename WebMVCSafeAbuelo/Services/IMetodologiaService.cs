using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Services
{
    public interface IMetodologiaService
    {
        Task<IEnumerable<MetodologiaAtaque>> ObtenerTodosAsync();
        Task<(IEnumerable<MetodologiaAtaque> Metodologias, int TotalPaginas)> ObtenerPaginadosAsync(string buscar, int pagina, int cantidadPorPagina);
        Task<MetodologiaAtaque?> ObtenerPorIdAsync(int? id);
        Task CrearAsync(MetodologiaAtaque metodologia);
        Task ActualizarAsync(MetodologiaAtaque metodologia);
        Task EliminarAsync(int id);
        bool Existe(int? id);
    }
}