﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Entidades;

namespace ModuloCompras.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly DbContext _context;

        public FacturasController(DbContext context)
        {
            _context = context;
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFactura()
        {
            return await _context.Facturas.ToListAsync();
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            if (id != factura.Id)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Facturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFactura", new { id = factura.Id }, factura);
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(int id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }
    }
}
