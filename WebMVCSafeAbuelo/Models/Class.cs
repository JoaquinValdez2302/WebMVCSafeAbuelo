using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVCSafeAbuelo.Models
{
    public class ReporteIncidente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaReporte { get; set; }

        [Required]
        [StringLength(100)]
        public string Localidad { get; set; } = string.Empty; // Ej: Corrientes Capital, Resistencia, etc.

        [Required]
        [StringLength(50)]
        public string PlataformaDeContacto { get; set; } = string.Empty; // WhatsApp, Llamada, Facebook, etc.

        // --- Modus Operandi (Ingeniería Social) ---
        // Estos booleanos son vitales para generar estadísticas sobre las tácticas de los atacantes
        public bool EjercePresionPsicologica { get; set; }
        public bool GeneraSentidoDeUrgencia { get; set; }

        public string DescripcionDelEngaño { get; set; } = string.Empty;

        // --- Estado de la gestión ---
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = string.Empty;// "Pendiente", "En Revisión", "Descartado"
    }   
}
