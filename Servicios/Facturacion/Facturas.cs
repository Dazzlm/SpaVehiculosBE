using SpaVehiculosBE.Models;
using SpaVehiculosBE.Servicios.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static SpaVehiculosBE.Servicios.Facturacion.DetalleFactura;

namespace SpaVehiculosBE.Servicios
{
    
    public class Facturas
	{
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public class FacturaCompletedDTO
        {
            public Factura Factura { get; set; }
            public List<DetalleFacturaProducto> Productos { get; set; }
            public List<DetalleFacturaServicio> Servicios { get; set; }
        }
        public List<Factura> GetFacturas()
        {
            List<Factura> facturas = db.Facturas.ToList();
            return facturas;
        }

        public int ContarFacturasDeHoy()
        {
            DateTime hoy = DateTime.Today;
            DateTime mañana = hoy.AddDays(1);
            int cantidad = db.Facturas
                .Count(f => f.Fecha >= hoy && f.Fecha < mañana);
            return cantidad;
        }


        public class Response { 
            public bool Success { get; set; }
            public string Message { get; set; }
            public int IdFactura { get; set; }

        }

        public FacturaCompletedDTO GetFactura(int id)
        {
            Factura factura = db.Facturas.FirstOrDefault(f => f.IdFactura == id);

            if (factura == null)
            {
                return null;
            }

            DetalleFactura detalleFactura = new DetalleFactura();
            DetallesFacturaDTO detallesFacturaDTO = detalleFactura.GetDetallesFactura(id);
            FacturaCompletedDTO facturaCompletedDTO = new FacturaCompletedDTO
            {
                Factura = factura,
                Productos = detallesFacturaDTO.Productos,
                Servicios = detallesFacturaDTO.Servicios
            };
            return facturaCompletedDTO;
        }

        public Response AddFactura(Factura factura, List<DetalleFacturaProducto> detalleProds, List<DetalleFacturaServicio> detalleServs )
        {

            db.Facturas.Add(factura);
            db.SaveChanges();
            DetalleFactura detalleFactura = new DetalleFactura();
            if (detalleProds != null && detalleProds.Any()) {
                foreach (DetalleFacturaProducto detalleProd in detalleProds)
                {
                    detalleProd.IdFactura = factura.IdFactura;
                }
            }
            if(detalleServs != null && detalleServs.Any()) { 
                foreach (DetalleFacturaServicio detalleServ in detalleServs)
                {
                    detalleServ.IdFactura = factura.IdFactura;
                }
            }

            Notificacion notificacion = new Notificacion();
            string result = detalleFactura.AddDetalle(detalleProds, detalleServs);
            notificacion.EnviarFactura(factura.IdFactura);
            Response response = new Response
            {
                Success = true,
                Message = result,
                IdFactura = factura.IdFactura
            };

            return response;

        }

        public string DeleteFactura(int id)
        {
            Factura factura = db.Facturas.FirstOrDefault(f => f.IdFactura == id);
            DetalleFactura detalleFactura = new DetalleFactura();
            if (factura == null)
            {
                return "Error404: Factura no encontrada";
            }
            string result = detalleFactura.DeleteDetalles(id);
            try {
                db.Facturas.Remove(factura);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
               
                return "Error al eliminar la factura: " + ex.Message;
            }
            
            return "Factura eliminada correctamente: "+result;


        }

    }
}