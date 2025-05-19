using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Configuration;
using static SpaVehiculosBE.Servicios.GestorFacturaPDF;


namespace SpaVehiculosBE.Servicios
{
	public class Notificacion
	{

		private readonly  SpaVehicularDBEntities db = new SpaVehicularDBEntities();

		public string EnviarFactura( int id ) {
            try
            {
                Factura factura = db.Facturas.FirstOrDefault(f => f.IdFactura == id);

                if (factura == null)
                {
                    return "Error404: Factura no encontrada.";
                }


                string correoRemitente = ConfigurationManager.AppSettings["CorreoRemitente"];
                string contrasena = ConfigurationManager.AppSettings["ContrasenaCorreo"];
                string smtpServidor = ConfigurationManager.AppSettings["SMTPServidor"];
                int smtpPuerto = int.Parse(ConfigurationManager.AppSettings["SMTPPuerto"]);

                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress(correoRemitente);
                
                string emailTo = db.Clientes.Where(c => c.IdCliente == factura.IdCliente)
                    .Select(c => c.Email)
                    .FirstOrDefault();
                mensaje.To.Add(emailTo);
                mensaje.Subject = "Factura correspondiente a su servicio en Spa Vehículos";

                // Mensaje en texto plano
                string nombreCliente = db.Clientes
                .Where(c => c.IdCliente == factura.IdCliente)
                .Select(c => c.Nombre + " " + c.Apellidos)
                .FirstOrDefault();

                mensaje.Body =
                    "🧾 FACTURA ELECTRÓNICA - SPA VEHÍCULOS 🧽🚗\n\n" +
                    $"Estimado(a) {nombreCliente},\n\n" +
                    "Adjunto a este correo encontrará la factura correspondiente al servicio que recibió en Spa Vehículos. 📎\n\n" +
                    "🛠️ Detalles del servicio:\n" +
                    "✔ Servicios y productos adquiridos\n" +
                    "✔ Fecha de atención\n" +
                    "✔ Total facturado\n\n" +
                    "📬 Si tiene alguna duda o necesita más información, no dude en contactarnos.\n\n" +
                    "📞 Teléfono: (123) 456-7890\n" +
                    "📧 Correo: SpaVehiculos7@gmail.com\n" +
                    "📍 Dirección: Calle 123, Ciudad Spa Vehicular\n\n" +
                    "Gracias por confiar en nosotros. ¡Esperamos volver a verlo pronto! ✨\n\n" +
                    "Atentamente,\n" +
                    "Equipo de Spa Vehículos 🚙💦";


                mensaje.IsBodyHtml = false;

                GestorFacturaPDF gestorFacturaPDF = new GestorFacturaPDF();
                string rutaPDF = gestorFacturaPDF.ObtenerRutaPDF(factura.IdFactura);
                PDFResult pdfResult = gestorFacturaPDF.ObtenerPDF(factura.IdFactura);
               
                Attachment adjunto = new Attachment(rutaPDF);
                adjunto.Name = pdfResult.NombreArchivo;
                mensaje.Attachments.Add(adjunto);

                SmtpClient smtp = new SmtpClient(smtpServidor, smtpPuerto);
                smtp.Credentials = new NetworkCredential(correoRemitente, contrasena);
                smtp.EnableSsl = true;

                smtp.Send(mensaje);
                EmailEnviado emailEnviado = new EmailEnviado
                {
                    IdFactura = factura.IdFactura,
                    FechaEnvio = DateTime.Now,
                    CorreoDestino = emailTo
                };
                try
                {
                    db.EmailEnviadoes.Add(emailEnviado);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return "Error al guardar el registro de envío: " + ex.Message;
                }
                
                return "Correo enviado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al enviar el correo: " + ex.Message;
            }
        } 

    }
}