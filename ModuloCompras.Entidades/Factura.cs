using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuloCompras.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ProveedorId { get; set; }
        public string? TipoPago { get; set; } // Crédito o Contado
        public DateTime? FechaVencimiento { get; set; }
        public bool Impresa { get; set; } = false;


        // Relación muchos a uno con Proveedor
        public Proveedor? Proveedor { get; set; }

        // Relación uno a muchos con DetalleFactura
        public List<DetalleFactura>? Detalles { get; set; }




    }
}
