using System.Runtime.Serialization;

namespace WebMVCSafeAbuelo.Models.Enums
{
    public enum TipoEvidencia
    {
        [EnumMember(Value = "Captura de Pantalla")] CapturaPantalla,
        [EnumMember(Value = "Comprobante Bancario")] ComprobanteBancario,
        [EnumMember(Value = "Enlace Web")] EnlaceWeb,
        [EnumMember(Value = "Texto")] Texto
    }
}