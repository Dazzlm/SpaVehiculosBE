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
        public RespuestaServicio<List<Factura>> GetFacturas()
        {
            try
            {
                List<Factura> facturas = db.Facturas.ToList();
                if (facturas == null || !facturas.Any())
                {
                    return RespuestaServicio<List<Factura>>.ConError("No hay facturas para mostrar");
                }
                return RespuestaServicio<List<Factura>>.ConExito(facturas);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<List<Factura>>.ConError("Error al obtener las facturas: " + ex.Message);
            }
        }

        public int ContarFacturasDeHoy()
        {
            DateTime hoy = DateTime.Today;
            DateTime mañana = hoy.AddDays(1);
            int cantidad = db.Facturas
                .Count(f => f.Fecha >= hoy && f.Fecha < mañana);
            return cantidad;
        }

        public RespuestaServicio<FacturaCompletedDTO> GetFactura(int id)
        {
            try
            {
                Factura factura = db.Facturas.FirstOrDefault(f => f.IdFactura == id);

                if (factura == null)
                {
                    return RespuestaServicio<FacturaCompletedDTO>.ConError("Error:404 Factura no encontrada");
                }      
                DetalleFactura detalleFactura = new DetalleFactura();
                DetallesFacturaDTO detallesFacturaDTO = detalleFactura.GetDetallesFactura(id);
                FacturaCompletedDTO facturaCompletedDTO = new FacturaCompletedDTO
                {
                    Factura = factura,
                    Productos = detallesFacturaDTO.Productos,
                    Servicios = detallesFacturaDTO.Servicios
                };
                return RespuestaServicio<FacturaCompletedDTO>.ConExito(facturaCompletedDTO);
            }
            catch(Exception e ) {
                return RespuestaServicio<FacturaCompletedDTO>.ConError("Error al obtener la factura: " + e.Message);
            }
            
        }

        public RespuestaServicio<int> AddFactura(Factura factura, List<DetalleFacturaProducto> detalleProds, List<DetalleFacturaServicio> detalleServs )
        {
            try
            {
                db.Facturas.Add(factura);
                db.SaveChanges();
                DetalleFactura detalleFactura = new DetalleFactura();
                if (detalleProds != null && detalleProds.Any())
                {
                    foreach (DetalleFacturaProducto detalleProd in detalleProds)
                    {
                        detalleProd.IdFactura = factura.IdFactura;
                    }
                }
                if (detalleServs != null && detalleServs.Any())
                {
                    foreach (DetalleFacturaServicio detalleServ in detalleServs)
                    {
                        detalleServ.IdFactura = factura.IdFactura;
                    }
                }

                Notificacion notificacion = new Notificacion();
                string result = detalleFactura.AddDetalle(detalleProds, detalleServs);
                notificacion.EnviarFactura(factura.IdFactura);

                return RespuestaServicio<int>.ConExito(factura.IdFactura,"Factura generada con exito");
            }
            catch (Exception)
            {
                return RespuestaServicio<int>.ConError("Error al agregar la factura");
            }
        }

        public RespuestaServicio<string> DeleteFactura(int id)
        {
            Factura factura = db.Facturas.FirstOrDefault(f => f.IdFactura == id);
            DetalleFactura detalleFactura = new DetalleFactura();
            if (factura == null)
            {
                return RespuestaServicio<string>.ConError("Error:404 Factura no encontrada");
            }
            string result = detalleFactura.DeleteDetalles(id);
            try {
                db.Facturas.Remove(factura);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                 return RespuestaServicio<string>.ConError("Error al eliminar la factura: " + ex.Message);
            }

            return RespuestaServicio<string>.ConExito(default,"Factura eliminada correctamente: " + result);

        }

    }
}