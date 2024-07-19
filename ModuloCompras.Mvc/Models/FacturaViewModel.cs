namespace ModuloCompras.Entidades
{
    public class FacturaViewModel
    {
        public int Id { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoPago { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Impresa { get; set; }

        public DetalleFactura DetalleFactura { get; set; }

        public FacturaViewModel()
        {
            FechaHora = DateTime.Now;
            DetalleFactura = new DetalleFactura();
        }
    }
}
