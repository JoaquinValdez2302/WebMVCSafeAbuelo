using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{
    // LA CLAVE ESTÁ AQUÍ: Al poner ": IdentityUser", el modelo absorbe Email, UserName, etc.
    public class UsuarioAdministrador : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Rol en el Equipo")]
        public string RolEnElEquipo { get; set; } = string.Empty;

        [Display(Name = "Acceso Total")]
        public bool NivelAccesoTotal { get; set; } = false;
    }
}