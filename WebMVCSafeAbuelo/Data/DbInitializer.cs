using Microsoft.AspNetCore.Identity;
using WebMVCSafeAbuelo.Models;
using Microsoft.Extensions.Configuration;

namespace WebMVCSafeAbuelo.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<UsuarioAdministrador> userManager, IConfiguration config)
        {
            // 1. Extraemos las credenciales desde la configuración (la capa de secretos)
            var adminEmail = config["AdminConfig:Email"];
            var adminPassword = config["AdminConfig:Password"];

            // Control de seguridad por si las variables están vacías
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                return;
            }

            // 2. Comprobamos si el usuario administrador ya existe en Neon
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // 3. Si no existe, lo creamos por primera vez
            if (adminUser == null)
            {
                var newAdmin = new UsuarioAdministrador
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Evita la validación por token de correo
                    NombreCompleto = "Super Administrador",
                    RolEnElEquipo = "Analista SOC Maestro",
                    NivelAccesoTotal = true
                };

                // Identity se encarga de aplicar el hashing seguro a la contraseña antes de subirla
                await userManager.CreateAsync(newAdmin, adminPassword);
            }
        }
    }
}
