using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace ModuloCompras.Mvc.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly string urlApi;

        public ProveedoresController(IConfiguration configuration)
        {
            urlApi = configuration.GetValue<string>("ApiUrlBase") + "/Proveedores";
        }

        public ActionResult GenerarReportePDF()
        {
            var data = Crud<Proveedor>.Read(urlApi);

            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.Open();

            // Encabezado
            Font headerFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Paragraph header = new Paragraph("Módulo Compras\n", headerFont);
            header.Alignment = Element.ALIGN_CENTER;
            document.Add(header);

            Font subHeaderFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
            Paragraph subHeader = new Paragraph("Aplicaciones Distribuidas\n\n", subHeaderFont);
            subHeader.Alignment = Element.ALIGN_CENTER;
            document.Add(subHeader);

            // Fecha
            Font dateFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Paragraph date = new Paragraph($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}\n\n", dateFont);
            date.Alignment = Element.ALIGN_RIGHT;
            document.Add(date);

            // Título del reporte
            Font titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Paragraph title = new Paragraph("Reporte de Proveedores\n\n", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Tabla
            PdfPTable table = new PdfPTable(10); // 10 columnas
            table.WidthPercentage = 100;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            // Estilos de la tabla
            Font tableHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font tableCellFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);

            // Encabezados de la tabla
            string[] headers = { "ID", "Cédula/RUC", "Apellidos", "Nombres", "Ciudad", "Tipo de Proveedor", "Dirección", "Teléfono", "Email", "Estado" };
            foreach (string headerText in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(headerText, tableHeaderFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
            }

            // Datos de los proveedores
            foreach (var proveedor in data)
            {
                string estado = proveedor.Estado ? "Activo" : "Inactivo";

                table.AddCell(new PdfPCell(new Phrase(proveedor.Id.ToString(), tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.CedulaRuc ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Apellidos ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Nombres ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Ciudad ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.TipoProveedor ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Direccion ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Telefono ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(proveedor.Email ?? "", tableCellFont)));
                table.AddCell(new PdfPCell(new Phrase(estado, tableCellFont)));
            }

            document.Add(table);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", "ReporteProveedores.pdf");
        }
  




// GET: ProveedoresController
public ActionResult Index()
        {
            var data = Crud<Proveedor>.Read(urlApi);
            return View(data);
        }

        // GET: ProveedoresController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // GET: ProveedoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProveedoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proveedor data)
        {
            try
            {
                var newData = Crud<Proveedor>.Create(urlApi, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: ProveedoresController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: ProveedoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Proveedor data)
        {
            try
            {
                Crud<Proveedor>.Update(urlApi, id, data);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: ProveedoresController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Proveedor>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: ProveedoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Proveedor data)
        {
            try
            {
                Crud<Proveedor>.Delete(urlApi, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }
       
    }


}

