using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Text;
using iTextSharp.tool.xml;

namespace SpaVehiculosBE.Servicios
{
    public class GestorFacturaPDF
    {

        public class PDFResult
        {
            public bool Success { get; set; }
            public string Mensaje { get; set; }
            public string NombreArchivo { get; set; }
            public byte[] Archivo { get; set; }
        }


        private readonly string directorioPDF = HttpContext.Current.Server.MapPath("~/FacturasPDF/");

        public string ObtenerRutaPDF(int idFactura)
        {
            string carpeta = HttpContext.Current.Server.MapPath("~/FacturasPDF/");
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            return Path.Combine(carpeta, $"Factura_{idFactura}.pdf");
        }

        

        public PDFResult ObtenerPDF(int idFactura)
        {
            string nombreArchivo = $"Factura_{idFactura}.pdf";
            string rutaPDF = ObtenerRutaPDF(idFactura);

            try
            {
                if (!File.Exists(rutaPDF))
                {
                    using (SpaVehicularDBEntities db = new SpaVehicularDBEntities())
                    {
                        Factura factura = db.Facturas.Include("Cliente").Include("Sede").FirstOrDefault(f => f.IdFactura == idFactura);
                        List<DetalleFacturaProducto> productos = db.DetalleFacturaProductoes.Include("Producto").Where(p => p.IdFactura == idFactura).ToList();
                        List<DetalleFacturaServicio> servicios = db.DetalleFacturaServicios.Include("Servicio").Where(s => s.IdFactura == idFactura).ToList();

                        if (factura == null)
                        {
                            return new PDFResult
                            {
                                Success = false,
                                Mensaje = "Factura no encontrada"
                            };
                        }

                        string rutaPlantilla = HttpContext.Current.Server.MapPath("~/Plantillas/PlantillaFactura.html");
                        string html = CargarPlantillaYReemplazar(rutaPlantilla, factura, productos, servicios);
                        GenerarPDFDesdeHTML(html, rutaPDF);

                    }
                }

                return new PDFResult
                {
                    Success = true,
                    Archivo = File.ReadAllBytes(rutaPDF),
                    NombreArchivo = nombreArchivo
                };
            }
            catch (Exception ex)
            {
                return new PDFResult
                {
                    Success = false,
                    Mensaje = "Error generando PDF: " + ex.Message
                };
            }
        }

        public string CargarPlantillaYReemplazar(string rutaPlantilla, Factura factura, List<DetalleFacturaProducto> productos, List<DetalleFacturaServicio> servicios)
        {
            string plantilla = File.ReadAllText(rutaPlantilla);

            string tablaProductos = "";
            if (productos.Any())
            {
                tablaProductos = "<div class='section-title'>Productos</div>";
                tablaProductos += "<table><tr><th>Producto</th><th>Cantidad</th><th>Subtotal</th></tr>";
                foreach (DetalleFacturaProducto p in productos)
                {
                    tablaProductos += $"<tr><td>{p.Producto?.Nombre}</td><td>{p.Cantidad}</td><td>${p.Subtotal:N2}</td></tr>";
                }
                tablaProductos += "</table>";
            }

            string tablaServicios = "";
            if (servicios.Any())
            {
                tablaServicios = "<div class='section-title'>Servicios</div>";
                tablaServicios += "<table><tr><th>Servicio</th><th>Subtotal</th></tr>";
                foreach (var s in servicios)
                {
                    tablaServicios += $"<tr><td>{s.Servicio?.Nombre}</td><td>${s.Subtotal:N2}</td></tr>";
                }
                tablaServicios += "</table>";
            }

            plantilla = plantilla.Replace("{{IdFactura}}", factura.IdFactura.ToString())
                                 .Replace("{{Fecha}}", factura.Fecha.ToShortDateString())
                                 .Replace("{{Cliente}}", factura.Cliente?.Nombre ?? "N/A")
                                 .Replace("{{Sede}}", factura.Sede?.Nombre ?? "N/A")
                                 .Replace("{{TablaProductos}}", tablaProductos)
                                 .Replace("{{TablaServicios}}", tablaServicios)
                                 .Replace("{{Total}}", factura.Total.ToString("N2"));

            return plantilla;
        }


        public void GenerarPDFDesdeHTML(string htmlContent, string rutaPDF)
        {
            using (FileStream stream = new FileStream(rutaPDF, FileMode.Create))
            {
                using (Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30))
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    using (StringReader sr = new StringReader(htmlContent))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                }
            }
        }

        public string EliminarPDF(int idFactura)
        {
            string path = ObtenerRutaPDF(idFactura);
            if (File.Exists(path))
            {
                try {
                    File.Delete(path);
                    return "Archivo eliminado correctamente";
                }
                catch (Exception ex)
                {
                    return "Error al eliminar el archivo: " + ex.Message;
                }
                
            }
            return "Error404: Factura no encontrada";
        }
    }
}