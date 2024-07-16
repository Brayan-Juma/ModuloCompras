using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuloCompras.Entidades
{
    public class DetalleFactura
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public double Iva { get; set; }
        public double SubTotal { get; set; }

        // Relación muchos a uno con Factura
        public Factura? Factura { get; set; }
    


    }
}
