namespace ModuloCompras.Mvc.Models
{
    public class Producto
    {
        public int Id_Producto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool GravaIVA { get; set; }
        public double Costo { get; set; }
        public double Pvp { get; set; }
        public bool Estado { get; set; }
        public int StockProducto { get; set; }
    }
}
