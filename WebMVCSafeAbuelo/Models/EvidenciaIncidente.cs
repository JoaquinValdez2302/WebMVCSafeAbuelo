using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebMVCSafeAbuelo.Models.Enums; 

namespace WebMVCSafeAbuelo.Models
{
    public class EvidenciaIncidente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReporteIncidenteId { get; set; }
        public virtual ReporteIncidente? ReporteIncidente { get; set; }

        [Required(ErrorMessage = "Debe especificar el tipo de evidencia.")]
        public TipoEvidencia Tipo { get; set; } // Usa el Enum centralizado

        [Required(ErrorMessage = "El contenido de la evidencia no puede estar vacío.")]
        [StringLength(500, ErrorMessage = "El contenido no puede superar los 500 caracteres.")]
        [ValidarContenidoEvidencia] // <-- Nuestro escudo dinámico actualizado
        public string Valor { get; set; } = string.Empty;

        // Nuevas propiedades exigidas por el DTO
        public string Notas { get; set; } = string.Empty;
        public string? LinkEvidencia { get; set; }
    }

    // --- VALIDADOR INTELIGENTE EN CASCADA ---
    public class ValidarContenidoEvidenciaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var evidencia = (EvidenciaIncidente)validationContext.ObjectInstance;
            var contenido = value as string;

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return new ValidationResult("El contenido de la evidencia es requerido.");
            }

            // CASO 1: Es un enlace web
            if (evidencia.Tipo == TipoEvidencia.EnlaceWeb)
            {
                if (!Uri.TryCreate(contenido, UriKind.Absolute, out var uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    return new ValidationResult("Debe ingresar un enlace web válido que comience con http:// o https://");
                }
            }
            // CASO 2: Capturas de pantalla, comprobantes o texto descriptivo
            else if (evidencia.Tipo == TipoEvidencia.Texto ||
                     evidencia.Tipo == TipoEvidencia.CapturaPantalla ||
                     evidencia.Tipo == TipoEvidencia.ComprobanteBancario)
            {
                // Sanitización básica: Permitimos alfanuméricos y puntuación común, bloqueando scripts
                var regexTextoSeguro = new Regex(@"^[a-zA-Z0-9\s\.,:;\?¿!¡íóáéúñÍÓÁÉÚÑüÜ\(\)\-\/_\/]+$");
                if (!regexTextoSeguro.IsMatch(contenido))
                {
                    return new ValidationResult("El texto contiene caracteres especiales no permitidos por razones de seguridad.");
                }
            }

            return ValidationResult.Success;
        }
    }
}