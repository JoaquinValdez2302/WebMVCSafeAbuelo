using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{
    public enum EstadoIncidente
    {
        Pendiente,          // Reporte nuevo que aún no fue revisado
        EnInvestigacion,    // Un analista está validando las evidencias
        Resuelto,           // El caso fue mitigado o cerrado con éxito
        Desestimado         // Falso positivo o reporte duplicado
    }
    public enum Provincia
    {
        Corrientes,
        Chaco
    }

    public enum Localidad
    {
        Corrientes, // Corrientes Capital
        Resistencia
    }

    public enum PlataformaContacto
    {
        WhatsApp,
        LlamadaTelefonica,
        FacebookMessenger,
        Instagram,
        CorreoElectronico,
        SitioWebFalso,
        Otro
    }
    public class ReporteIncidente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe seleccionar una provincia.")]
        public Provincia Provincia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una localidad.")]
        public Localidad Localidad { get; set; }

        [Required(ErrorMessage = "La fecha del reporte es obligatoria.")]
        [DataType(DataType.Date)]
        [FechaNoFutura(ErrorMessage = "La fecha del reporte no puede ser posterior al día de hoy.")]
        public DateTime FechaReporte { get; set; }


        [Display(Name = "Plataforma de contacto")]
        [Required(ErrorMessage = "Debe seleccionar una plataforma de contacto.")]
        public PlataformaContacto PlataformaDeContacto { get; set; } // WhatsApp, Llamada, Facebook, etc.
        [MaxLength(50, ErrorMessage = "El nombre de la plataforma es demasiado largo.")]
        [RegularExpression(@"^[a-zA-Z0-9\síóáéúñÍÓÁÉÚÑüÜ]+$", ErrorMessage = "La plataforma solo puede contener letras, números y espacios.")]
        public string? PlataformaOtra { get; set; }

        // --- Modus Operandi (Ingeniería Social) ---
        // Estos booleanos son vitales para generar estadísticas sobre las tácticas de los atacantes
        public bool EjercePresionPsicologica { get; set; }
        public bool GeneraSentidoDeUrgencia { get; set; }
        [Required(ErrorMessage = "La descripción del engaño es obligatoria.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "La descripción debe detallar el incidente (mínimo 20 caracteres, máximo 1000).")]
        public string DescripcionDelEngaño { get; set; } = string.Empty;


        // --- Estado de la gestión ---
        [Required(ErrorMessage = "Debe especificar el estado del reporte.")]
        public EstadoIncidente Estado { get; set; }

        public ICollection<EvidenciaIncidente> Evidencias { get; set; } = new List<EvidenciaIncidente>();
    }

    public class FechaNoFuturaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime fecha)
            {
                if (fecha.Date > DateTime.UtcNow.Date)
                {
                    return new ValidationResult(ErrorMessage ?? "La fecha no puede ser futura.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

