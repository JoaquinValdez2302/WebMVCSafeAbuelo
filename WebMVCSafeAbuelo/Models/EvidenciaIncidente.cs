using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace WebMVCSafeAbuelo.Models
{
    public enum TipoEvidencia
    {
        CapturaDePantalla,
        EnlaceMalicioso,
        NumeroDeTelefono,
        ComprobanteFinanciero,
        DocumentoFalso,
        CorreoElectronico
    }
    public class EvidenciaIncidente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReporteIncidenteId { get; set; }
        public ReporteIncidente? ReporteIncidente { get; set; }

        [Required(ErrorMessage = "Debe especificar el tipo de evidencia.")]
        public TipoEvidencia Tipo { get; set; }

        // Asumimos que tu propiedad de texto se llama DetalleEvidencia o ArchivoUrl.
        // Reemplaza "DetalleEvidencia" por el nombre exacto de tu propiedad de texto.
        [Required(ErrorMessage = "El contenido de la evidencia no puede estar vacío.")]
        [StringLength(500, ErrorMessage = "El contenido no puede superar los 500 caracteres.")]
        [ValidarContenidoEvidencia] // <-- Nuestro escudo dinámico
        public string Valor { get; set; } = string.Empty;
    }

    // --- VALIDADOR INTELIGENTE EN CASCADA ---
    public class ValidarContenidoEvidenciaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Obtenemos la instancia del modelo que se está validando en tiempo real
            var evidencia = (EvidenciaIncidente)validationContext.ObjectInstance;
            var contenido = value as string;

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return new ValidationResult("El contenido de la evidencia es requerido.");
            }

            // CASO 1: Es un número de teléfono
            if (evidencia.Tipo == TipoEvidencia.NumeroDeTelefono)
            {
                // Regex estricta: Permite código de país opcional (+), código de área y entre 6 y 15 dígitos numéricos.
                var regexTelefono = new Regex(@"^\+?[0-9\s\-]{6,15}$");
                if (!regexTelefono.IsMatch(contenido))
                {
                    return new ValidationResult("Formato de teléfono inválido. Use números, espacios o guiones (Ej: +54 9 379 4123456).");
                }
            }
            // CASO 2: Es un enlace malicioso (URL)
            else if (evidencia.Tipo == TipoEvidencia.EnlaceMalicioso)
            {
                // Intentamos parsear la URL para verificar que sea absoluta y use protocolos web seguros
                if (!Uri.TryCreate(contenido, UriKind.Absolute, out var uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    return new ValidationResult("Debe ingresar un enlace web válido que comience con http:// o https://");
                }
            }
            else if (evidencia.Tipo == TipoEvidencia.CorreoElectronico)
            {
                // Usamos el validador nativo de .NET
                var emailValidator = new EmailAddressAttribute();
                if (!emailValidator.IsValid(contenido))
                {
                    return new ValidationResult("Debe ingresar una dirección de correo electrónico válida (Ej: estafador@dominio.com).");
                }
            }
            // CASO 3: Capturas de pantalla, comprobantes o documentos (Texto descriptivo)
            else
            {
                // Sanitización básica: Permitimos caracteres alfanuméricos y puntuación común, bloqueando caracteres de scripts (<, >, ;, etc.)
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
