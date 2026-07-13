using System.Runtime.Serialization;

namespace WebMVCSafeAbuelo.Models.Enums
{
    public enum PlataformaDeContacto
    {
        [EnumMember(Value = "WhatsApp")] WhatsApp,
        [EnumMember(Value = "Facebook")] Facebook,
        [EnumMember(Value = "Instagram")] Instagram,
        [EnumMember(Value = "Llamada Telefónica")] LlamadaTelefonica,
        [EnumMember(Value = "SMS")] SMS,
        [EnumMember(Value = "Correo Electrónico")] CorreoElectronico,
        [EnumMember(Value = "Otro")] Otro
    }
}