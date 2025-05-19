using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Servicios.Facturacion
{
	public class DetalleFactura
	{
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public class DetallesFacturaDTO
        {
            public List<DetalleFacturaProducto> Productos { get; set; }
            public List<DetalleFacturaServicio> Servicios { get; set; }
        }

        public DetallesFacturaDTO GetDetallesFactura(int idFactura)
        {
            DetallesFacturaDTO detalles = new DetallesFacturaDTO();

            try
            {
                detalles.Productos = db.DetalleFacturaProductoes
                    .Where(p => p.IdFactura == idFactura)
                    .ToList();

                detalles.Servicios = db.DetalleFacturaServicios
                    .Where(s => s.IdFactura == idFactura)
                    .ToList();
            }
            catch (Exception ex)
            {
                detalles.Productos = new List<DetalleFacturaProducto>();
                detalles.Servicios = new List<DetalleFacturaServicio>();
            }

            return detalles;
        }


        public string AddDetallesFacturaProducto(List<DetalleFacturaProducto> detallesFacturaProducto)
        {
            try {

                if (detallesFacturaProducto == null) {
                    return "no hay detalles de Productos para agregar";
                }

                foreach (DetalleFacturaProducto detalle in detallesFacturaProducto)
                {
                    db.DetalleFacturaProductoes.Add(detalle);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Error al agregar el Detalle Producto: " + ex.Message;
            }
            
            return "Detalle de Producto agregado correctamente";
        }

        public string AddDetalleFacturaServicio(List<DetalleFacturaServicio> detallesFacturaProducto)
        {
            try {

                if ( detallesFacturaProducto== null)
                {
                    return "no hay detalles de Servicios para agregar";
                }

                foreach (DetalleFacturaServicio detalleFacturaServicio in detallesFacturaProducto)
                {
                    db.DetalleFacturaServicios.Add(detalleFacturaServicio);
                }
                db.SaveChanges();
                return "Detalle de Servicio agregado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al agregar el Detalle de Servicio: " + ex.Message;
            }
            
        }


        public string DeleteDetallesFacturaProducto(int id)
        {
            try
            {
                Factura factura = db.Facturas.FirstOrDefault(d => d.IdFactura == id);
                if (factura == null)
                {
                    return "Error404: Factura no encontrado";
                }
                List<DetalleFacturaProducto> detallesFacturaProducto = db.DetalleFacturaProductoes.Where(d => d.IdFactura == id).ToList();

                if (detallesFacturaProducto == null)
                {
                    return "no hay detalles de Productos para eliminar";
                }

                foreach (DetalleFacturaProducto detalle in detallesFacturaProducto)
                {
                    db.DetalleFacturaProductoes.Remove(detalle);
                }
                db.SaveChanges();
                return "Detalles de productos eliminados correctamente";
                
            }
            catch (Exception ex){ 
                return "Error al eliminar el Detalle de Productos: " + ex.Message;
            }
        }

        public string DeleteDetallesFacturaServicio(int id)
        {
            try {
                Factura factura = db.Facturas.FirstOrDefault(d => d.IdFactura == id);
                if (factura == null)
                {
                    return "Error404: Factura no encontrado";
                }
                List<DetalleFacturaServicio> detallesFacturaServicios = db.DetalleFacturaServicios.Where(d => d.IdFactura == id).ToList();

                if ( detallesFacturaServicios == null)
                {
                    return "no hay detalles de Servicios para eliminar";
                }

                foreach (DetalleFacturaServicio detalle in detallesFacturaServicios)
                {
                    db.DetalleFacturaServicios.Remove(detalle);
                }
                db.SaveChanges();
                return "Detalles de Servicios eliminado correctamente";
                
            }

            catch (Exception ex)
            {
                return "Error al eliminar el Detalle de Servicios: " + ex.Message;
            }
            
        }

        public string AddDetalle(List<DetalleFacturaProducto> detalleProds, List<DetalleFacturaServicio> detalleServs) {
            return AddDetallesFacturaProducto(detalleProds) +" y "+ AddDetalleFacturaServicio(detalleServs);
        }

        public string DeleteDetalles(int  id) {

            return DeleteDetallesFacturaProducto(id) + " y "+DeleteDetallesFacturaServicio(id); 
        }
    }
}