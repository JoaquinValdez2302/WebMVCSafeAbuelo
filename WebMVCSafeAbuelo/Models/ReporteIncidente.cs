using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMVCSafeAbuelo.Models.Enums; 
namespace WebMVCSafeAbuelo.Models
{
    public class ReporteIncidente
    {
        [Key]
        public int Id { get; set; }

        // --- Identificación del Usuario ---
        [Required(ErrorMessage = "El ID del autor es obligatorio.")]
        public string AuthorId { get; set; } = string.Empty; // Crucial para validar con Firebase JWT

        // --- Ubicación ---
        [Required(ErrorMessage = "Debe seleccionar una provincia.")]
        public Provincia Provincia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una localidad.")]
        public Localidad Localidad { get; set; }

        // --- Datos del Reporte ---
        [Required(ErrorMessage = "La fecha del reporte es obligatoria.")]
        [DataType(DataType.Date)]
        [FechaNoFutura(ErrorMessage = "La fecha del reporte no puede ser posterior al día de hoy.")]
        public DateTime FechaReporte { get; set; }

        [Display(Name = "Plataforma de contacto")]
        [Required(ErrorMessage = "Debe seleccionar una plataforma de contacto.")]
        public PlataformaDeContacto PlataformaDeContacto { get; set; }

        [MaxLength(50, ErrorMessage = "El nombre de la plataforma es demasiado largo.")]
        [RegularExpression(@"^[a-zA-Z0-9\síóáéúñÍÓÁÉÚÑüÜ]+$", ErrorMessage = "La plataforma solo puede contener letras, números y espacios.")]
        public string? PlataformaOtra { get; set; }

        // --- Modus Operandi (Ingeniería Social) ---
        public bool EjercePresionPsicologica { get; set; }
        public bool GeneraSentidoDeUrgencia { get; set; }

        [Required(ErrorMessage = "La descripción del engaño es obligatoria.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "La descripción debe detallar el incidente (mínimo 20 caracteres, máximo 1000).")]
        public string DescripcionDelEngaño { get; set; } = string.Empty;

        // --- Estado de la gestión ---
        [Required(ErrorMessage = "Debe especificar el estado del reporte.")]
        public EstadoReporte Estado { get; set; }

        // Propiedad de navegación (El 'virtual' permite Lazy Loading si lo habilitas luego)
        public virtual ICollection<EvidenciaIncidente> Evidencias { get; set; } = new List<EvidenciaIncidente>();
        
        [ForeignKey("AuthorId")]
        public PerfilUsuario Autor { get; set; }
    }

    // Validador de fecha
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