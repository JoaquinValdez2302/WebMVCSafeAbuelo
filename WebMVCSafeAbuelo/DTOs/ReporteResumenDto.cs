namespace WebMVCSafeAbuelo.DTOs
{
    public class ReporteResumenDto
    {
        public int Id { get; set; }
        public string Author { get; set; } // Aquí enviaremos el Nombre Completo, no el UUID
        public DateTime DateTime { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Estado { get; set; }
    }
}