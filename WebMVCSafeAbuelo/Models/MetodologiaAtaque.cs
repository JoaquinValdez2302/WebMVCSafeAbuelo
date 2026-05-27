using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{
    public class MetodologiaAtaque
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } // Ej: Cuento del Tío 2.0, Soporte Técnico Falso

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        // --- Vectores de Ingeniería Social ---
        [StringLength(100)]
        public string PrincipalMotorPsicologico { get; set; } = string.Empty; // Ej: Sentido de urgencia, Miedo, Falsa Autoridad

        [StringLength(200)]
        public string SeñalesDeAlarma { get; set; } = string.Empty; // Ej: "Te piden un código de 6 dígitos", "Te amenazan con bloquear la cuenta"

        [StringLength(300)]
        public string AccionPreventiva { get; set; } = string.Empty; // Ej: "Cortar la llamada y contactar al canal oficial"

        public bool EstaActivo { get; set; } = true; // Para ocultar métodos obsoletos sin borrarlos
    }
}