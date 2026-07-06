using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{

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

        [Required(ErrorMessage = "La fecha es obligatoria")]
       
        public DateTime FechaReporte { get; set; }

        [Required(ErrorMessage = "Por favor ingrese la localidad.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 100 caracteres.")]
        [Display(Name = "Localidad")]
        public string Localidad { get; set; } = string.Empty; // Ej: Corrientes Capital, Resistencia, etc.

        [Display(Name = "Plataforma de contacto")]
        [Required(ErrorMessage = "Debe seleccionar una plataforma de contacto.")]
        public PlataformaContacto PlataformaDeContacto { get; set; } // WhatsApp, Llamada, Facebook, etc.
        [MaxLength(50, ErrorMessage = "El nombre de la plataforma es demasiado largo.")]
        public string? PlataformaOtra { get; set; }

        // --- Modus Operandi (Ingeniería Social) ---
        // Estos booleanos son vitales para generar estadísticas sobre las tácticas de los atacantes
        public bool EjercePresionPsicologica { get; set; }
        public bool GeneraSentidoDeUrgencia { get; set; }
        [Required(ErrorMessage = "Ingrese una descripcion del incidente.")]
        [Display(Name = "Descripción")]
        public string DescripcionDelEngaño { get; set; } = string.Empty;


        // --- Estado de la gestión ---
        [Required(ErrorMessage = "Ingrese el estado del reporte.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El estado debe tener entre 5 y 100 caracteres.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;// "Pendiente", "En Revisión", "Descartado"
        
        public ICollection<EvidenciaIncidente> Evidencias { get; set; } = new List<EvidenciaIncidente>();
    }   
}

