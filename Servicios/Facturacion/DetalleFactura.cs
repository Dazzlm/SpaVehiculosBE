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

                if (detallesFacturaProducto == null ) {
                    return "no hay detalles de Productos para agregar";
                }

                if (detallesFacturaProducto.Count < 1)
                {
                    return "no hay detalles de Productos para agregar";
                }

                int idFactura = detallesFacturaProducto[0].IdFactura;
                // Verificar si la factura existe
                Factura factura = db.Facturas.FirstOrDefault(d => d.IdFactura == idFactura);
                if (factura == null)
                {
                    return "Error404: Factura no encontrado";
                }

                // Verificar si la sede existe
                Sede sede = db.Sedes.FirstOrDefault(d => d.IdSede == factura.IdSede);
                if (sede == null)
                {
                    return "Error404: Sede no encontrado";
                }


                foreach (DetalleFacturaProducto detalle in detallesFacturaProducto)
                {
                    SedeProducto sedeProducto = db.SedeProductoes.FirstOrDefault(d => d.IdSede == sede.IdSede &&  d.IdProducto == detalle.IdProducto );


                    if (sedeProducto == null)
                    {
                        return "Error404:  Producto no encontrado en la sede";
                    }

                    if(sedeProducto.StockDisponible  < detalle.Cantidad)
                    {
                        return "Error: Stock no disponible en la sede para el producto id: "+detalle.IdProducto;
                    }
                    sedeProducto.StockDisponible -= detalle.Cantidad;

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

                Sede sede = db.Sedes.FirstOrDefault(d => d.IdSede == factura.IdSede);
                if (sede == null)
                {
                    return "Error404: Sede no encontrado";
                }

                foreach (DetalleFacturaProducto detalle in detallesFacturaProducto)
                {

                    SedeProducto sedeProducto = db.SedeProductoes.FirstOrDefault(d => d.IdSede == sede.IdSede && d.IdProducto == detalle.IdProducto);

                    if (sedeProducto == null)
                    {
                        return "Error404:  Producto no encontrado en la sede";
                    }

                    sedeProducto.StockDisponible += detalle.Cantidad;

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