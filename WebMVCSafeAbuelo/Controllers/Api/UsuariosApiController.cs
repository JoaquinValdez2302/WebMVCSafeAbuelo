using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth; // Requiere instalar el paquete FirebaseAdmin
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;

namespace WebMVCSafeAbuelo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Inyectamos nuestro contexto de Entity Framework para hablar con Neon
        public UsuariosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("sincronizar")]
        public async Task<IActionResult> SincronizarPerfil([FromBody] SincronizacionRequestDto request)
        {
            try
            {
                // 1. Auditoría del Token (La Barrera Criptográfica)
                // Esto se comunica con Google para verificar que el token no sea inventado.
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Token);

                // Si llegamos a esta línea, el token es 100% real. Extraemos el UUID.
                string uid = decodedToken.Uid;

                // 2. Verificación en PostgreSQL
                // Revisamos si este usuario ya existe por si la app móvil envió la petición dos veces por error de red.
                var perfilExiste = await _context.PerfilesUsuarios.FindAsync(uid);

                if (perfilExiste != null)
                {
                    return Ok(new { mensaje = "El perfil ya se encontraba sincronizado." });
                }

                // 3. Inserción en la Caja Fuerte (Neon)
                var nuevoPerfil = new PerfilUsuario
                {
                    FirebaseUid = uid,
                    NombreCompleto = request.NombreCompleto,
                    Telefono = request.Telefono,
                    EmailContacto = request.Email
                };

                _context.PerfilesUsuarios.Add(nuevoPerfil);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { mensaje = "Perfil sincronizado exitosamente en la base de datos." });
            }
            catch (FirebaseAuthException)
            {
                // El atacante intentó mandar un token falso o expirado
                return Unauthorized(new { mensaje = "Token de seguridad inválido o expirado." });
            }
            catch (Exception ex)
            {
                // Falló la base de datos o hubo un error interno del servidor
                return StatusCode(500, new { mensaje = "Error interno al procesar la sincronización." });
            }
        }
    }

    // Objeto DTO (Data Transfer Object) para mapear el JSON entrante
    public class SincronizacionRequestDto
    {
        public string Token { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}