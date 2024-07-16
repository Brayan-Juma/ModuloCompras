using System.ComponentModel.DataAnnotations;

namespace ModuloCompras.Entidades
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }
        public string? CedulaRuc { get; set; }
        public string? Apellidos { get; set; }
        public string? Nombres { get; set; }
        public string? Ciudad { get; set; }
        public string? TipoProveedor { get; set; } // Crédito o Contado
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Estado { get; set; } // Activo o Inactivo

        // Relación uno a muchos con Factura
        public List<Factura>? Facturas { get; set; }
    }
}
