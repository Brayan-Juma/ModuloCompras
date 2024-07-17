using System.Collections.Generic;
using ModuloCompras.Entidades;

namespace ModuloCompras.Mvc.Models.ViewModels
{
    public class FacturaDetalleViewModel
    {
        public Factura Factura { get; set; }
        public List<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();
    }
}
