using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class GestorProductoSede
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public RespuestaServicio<string> CrearProductoSede(SedeProducto sedeProducto)
        {
            try
            {
                Producto producto = db.Productoes.FirstOrDefault(sp => sp.IdProducto == sedeProducto.IdProducto);
                if (producto == null)
                {
                    return RespuestaServicio<string>.ConError( "Error: Debes crear primero el producto antes de ingresar stock");
                }

                Sede sede = db.Sedes.FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede);
                if (sede == null)
                {
                    return RespuestaServicio<string>.ConError("Error: Debes crear primero la sede antes de ingresar stock");
                }

                SedeProducto productoExistente = db.SedeProductoes.FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede && sp.IdProducto == sedeProducto.IdProducto);
                if (productoExistente != null)
                {
                    return RespuestaServicio<string>.ConError("Error: El producto ya existe en la sede");
                }

                db.SedeProductoes.Add(sedeProducto);
                db.SaveChanges();
                return RespuestaServicio<string>.ConExito("Producto creado con éxito");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConExito("Error al crear el producto: " + ex.Message);
            }
        }
        public RespuestaServicio<List<SedeProducto>> BuscarProductosSedeID(int idProductoSede)
        {
            try
            {
                List<SedeProducto> sedeProductos = db.SedeProductoes.Where(sp => sp.IdSede == idProductoSede).ToList();
                if (sedeProductos == null)
                {
                    return RespuestaServicio<List<SedeProducto>>.ConError("Error404: No se encontraron productos en la sede especificada");
                }
                return RespuestaServicio<List<SedeProducto>>.ConExito(sedeProductos);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<List<SedeProducto>>.ConError("Error al buscar productos en la sede: " + ex.Message);
            }
        }
            
        public RespuestaServicio<SedeProducto> BuscarPorID(int Id)
        {
            try
            {
                SedeProducto sedeProducto = db.SedeProductoes
                .FirstOrDefault(sp => sp.Id == Id);
                return RespuestaServicio < SedeProducto >.ConExito( sedeProducto);
            }
            catch (Exception ex) {
                return RespuestaServicio<SedeProducto>.ConError("Error al buscar el producto en la sede: " + ex.Message);
            }
            
        }

        public RespuestaServicio< List<SedeProducto>> BuscarProductoSedeTodos()
        {
            try
            {
                List<SedeProducto> productos = db.SedeProductoes.ToList();
                return RespuestaServicio<List<SedeProducto>>.ConExito(productos);
            }
            catch (Exception ex) {
                return RespuestaServicio<List<SedeProducto>>.ConError("Error al obtener los productos en la sede:"+ ex.Message);
            }
            
        }
        public RespuestaServicio<string> ActualizarStock(SedeProducto sedeProducto)
        {
            try
            {
                var productoEnSede = db.SedeProductoes
                    .FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede && sp.IdProducto == sedeProducto.IdProducto);

                if (productoEnSede == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: El producto no se encuentra registrado en la sede.");
                }

                productoEnSede.StockDisponible = sedeProducto.StockDisponible;
                db.SaveChanges();

                return RespuestaServicio<string>.ConExito(default, "Stock actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error: " +ex.Message);  
            }
        }

        public RespuestaServicio<string> EliminarProductoSede(int idProducto)
        {
            try
            {
                List<SedeProducto> productoes = db.SedeProductoes.Where(a => a.IdProducto == idProducto).ToList();


                if (productoes == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: Producto en la sede no encontrado");
                }

                if (productoes.Count > 0)
                {
                    foreach (SedeProducto item in productoes)
                    {
                        db.SedeProductoes.Remove(item);
                    }
                    db.SaveChanges();
                    return RespuestaServicio<string>.ConExito(default,"El producto ha sido eliminado correctamente");
                }
                else
                {
                    return RespuestaServicio<string>.ConError("Error: El producto no se encuentra registrado en la sede");
                }
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al eliminar el producto: " + ex.Message);
            }
        }

        public RespuestaServicio<string> EliminarProductoSedeId(int id)
        {

            try
            {
                SedeProducto producto = db.SedeProductoes.FirstOrDefault(sp => sp.Id == id);
                if (producto == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: Producto en la sede no encontrado");
                }
                db.SedeProductoes.Remove(producto);
                db.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"El producto ha sido eliminado correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al eliminar el producto: " + ex.Message);
            }

        }


    }
}