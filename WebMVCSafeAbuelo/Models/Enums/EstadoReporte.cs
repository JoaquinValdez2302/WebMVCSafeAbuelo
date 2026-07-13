using System.Runtime.Serialization;

namespace WebMVCSafeAbuelo.Models.Enums
{
    public enum EstadoReporte
    {
        [EnumMember(Value = "Pendiente")] Pendiente,
        [EnumMember(Value = "Aceptado")] Aceptado,
        [EnumMember(Value = "Rechazado")] Rechazado
    }
}