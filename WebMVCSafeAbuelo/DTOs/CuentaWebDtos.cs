namespace WebMVCSafeAbuelo.DTOs
{
    public class RegistroWebDto
    {
        public string Token { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }

    public class LoginWebDto
    {
        public string Token { get; set; }
    }
}