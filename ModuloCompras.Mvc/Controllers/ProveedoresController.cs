using Microsoft.AspNetCore.Mvc;
using ModuloCompras.ConsumeApi;
using ModuloCompras.Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;


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
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.PageEvent = new FooterEventHandler(); // Añadir el evento para el pie de página
            writer.CloseStream = false;
            document.Open();

            // Encabezado

            PdfPTable headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 1f, 2f });



            Font titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.WHITE);
            PdfPCell titleCell = new PdfPCell(new Phrase("Reporte de Proveedores", titleFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = new BaseColor(0, 70, 122),
                Padding = 10
            };
            headerTable.AddCell(titleCell);

            Font infoFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.WHITE);
            PdfPCell addressCell = new PdfPCell(new Phrase("Ibarra, Ecuador", infoFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = new BaseColor(0, 70, 122),
                Padding = 40
            };
            headerTable.AddCell(addressCell);



            document.Add(headerTable);

            // Línea separadora
            PdfPCell lineCell = new PdfPCell(new Phrase("\n"))
            {
                Border = Rectangle.BOTTOM_BORDER,
                Colspan = 2,
                BorderColor = new BaseColor(0, 70, 122),
                BorderWidthBottom = 2f,
                PaddingBottom = 40
            };
            headerTable.AddCell(lineCell);

            // Descripción del proyecto
            Font descriptionFont = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
            Paragraph description = new Paragraph("DESCRIPCIÓN:\n\n", descriptionFont)
            {
                Alignment = Element.ALIGN_LEFT,
                SpacingAfter = 10
            };
            document.Add(description);

            // Tabla
            PdfPTable table = new PdfPTable(10)
            {
                WidthPercentage = 100,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };
            table.SetWidths(new float[] { 5f, 15f, 15f, 15f, 10f, 15f, 20f, 15f, 20f, 10f });

            // Estilos de la tabla
            Font tableHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
            Font tableCellFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);

            // Encabezados de la tabla
            string[] headers = { "ID", "Cédula/RUC", "Apellidos", "Nombres", "Ciudad", "Tipo de Proveedor", "Dirección", "Teléfono", "Email", "Estado" };
            foreach (string headerText in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(headerText, tableHeaderFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = new BaseColor(0, 70, 122), // Color azul
                    Padding = 5
                };
                table.AddCell(cell);
            }

            // Datos de los proveedores
            foreach (var proveedor in data)
            {
                string estado = proveedor.Estado ? "Activo" : "Inactivo";

                table.AddCell(new PdfPCell(new Phrase(proveedor.Id.ToString(), tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.CedulaRuc ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Apellidos ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Nombres ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Ciudad ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.TipoProveedor ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Direccion ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Telefono ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(proveedor.Email ?? "", tableCellFont)) { Padding = 5 });
                table.AddCell(new PdfPCell(new Phrase(estado, tableCellFont)) { Padding = 5 });
            }

            document.Add(table);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", "ReporteProveedores.pdf");
        }

        // Clase para manejar el pie de página
        public class FooterEventHandler : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footerTable = new PdfPTable(1);
                footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                Font footerFont = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);

                // Línea azul
                PdfPCell lineCell = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.BOTTOM_BORDER,
                    BorderColor = new BaseColor(0, 70, 122),
                    BorderWidthBottom = 2f,
                    PaddingBottom = 10
                };
                footerTable.AddCell(lineCell);

                // Fecha y hora
                string fechaHoraActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                PdfPCell dateTimeCell = new PdfPCell(new Phrase($"Módulo de Compras - Fecha y hora: {fechaHoraActual}", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 5
                };
                footerTable.AddCell(dateTimeCell);

                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
            }
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

