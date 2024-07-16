using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Entidades;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<ModuloCompras.Entidades.Proveedor> Proveedores { get; set; } = default!;

        public DbSet<ModuloCompras.Entidades.Factura> Facturas { get; set; } = default!;

        public DbSet<ModuloCompras.Entidades.DetalleFactura> DetalleFacturas { get; set; } = default!;
    }
