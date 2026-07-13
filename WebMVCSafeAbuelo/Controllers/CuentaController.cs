using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebMVCSafeAbuelo.Data;
using WebMVCSafeAbuelo.Models;
using WebMVCSafeAbuelo.DTOs; 

namespace WebMVCSafeAbuelo.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CuentaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- SECCIÓN REGISTRO ---

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SincronizarRegistroWeb([FromBody] RegistroWebDto request)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Token);
                string uid = decodedToken.Uid;

                var perfil = await _context.PerfilesUsuarios.FindAsync(uid);
                if (perfil == null)
                {
                    perfil = new PerfilUsuario
                    {
                        FirebaseUid = uid,
                        NombreCompleto = request.NombreCompleto,
                        Telefono = request.Telefono,
                        EmailContacto = request.Email
                    };

                    _context.PerfilesUsuarios.Add(perfil);
                    await _context.SaveChangesAsync();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, uid),
                    new Claim(ClaimTypes.Name, perfil.NombreCompleto),
                    new Claim(ClaimTypes.Email, perfil.EmailContacto)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new { urlRedireccion = Url.Action("Index", "Home") });
            }
            catch (Exception)
            {
                return StatusCode(401, new { mensaje = "Error de seguridad o de base de datos." });
            }
        }

        // --- SECCIÓN LOGIN ---

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ¡Este es el método que recibe el token desde el JS de Login!
        [HttpPost]
        public async Task<IActionResult> SincronizarLoginWeb([FromBody] LoginWebDto request)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Token);
                string uid = decodedToken.Uid;

                var perfil = await _context.PerfilesUsuarios.FindAsync(uid);
                if (perfil == null)
                {
                    return StatusCode(404, new { mensaje = "El perfil no existe en la base de datos." });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, uid),
                    new Claim(ClaimTypes.Name, perfil.NombreCompleto),
                    new Claim(ClaimTypes.Email, perfil.EmailContacto)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new { urlRedireccion = Url.Action("Index", "Home") });
            }
            catch (Exception)
            {
                return StatusCode(401, new { mensaje = "Credenciales inválidas o token expirado." });
            }
        }

        // --- SECCIÓN LOGOUT ---

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}