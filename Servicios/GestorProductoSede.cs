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
        public string CrearProductoSede(SedeProducto sedeProducto)
        {
            try
            {
                Producto producto = db.Productoes.FirstOrDefault(sp => sp.IdProducto == sedeProducto.IdProducto);
                if (producto == null)
                {
                    return "Error: Debes crear primero el producto antes de ingresar stock";
                }

                Sede sede = db.Sedes.FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede);
                if (sede == null)
                {
                    return "Error: Debes crear primero la sede antes de ingresar stock";
                }

                SedeProducto productoExistente = db.SedeProductoes.FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede && sp.IdProducto == sedeProducto.IdProducto);
                if (productoExistente != null)
                {
                    return "Error: El producto ya existe en la sede";
                }

                db.SedeProductoes.Add(sedeProducto);
                db.SaveChanges();
                return "Producto creado con éxito";
            }
            catch (Exception ex)
            {
                return "Error al crear el producto: " + ex.Message;
            }
        }
        public List<SedeProducto> BuscarProductosSedeID(int idProductoSede)
        {
            List<SedeProducto> sedeProductos = db.SedeProductoes.Where(sp => sp.IdSede == idProductoSede).ToList();
            return sedeProductos;
        }

        public SedeProducto BuscarPorID(int Id)
        {
            SedeProducto sedeProducto = db.SedeProductoes
                .FirstOrDefault(sp => sp.Id == Id);
            return sedeProducto;
        }

        public List<SedeProducto> BuscarProductoSedeTodos()
        {
            List<SedeProducto> productos = db.SedeProductoes.ToList();
            return productos;
        }
        public string ActualizarStock(SedeProducto sedeProducto)
        {
            try
            {
                var productoEnSede = db.SedeProductoes
                    .FirstOrDefault(sp => sp.IdSede == sedeProducto.IdSede && sp.IdProducto == sedeProducto.IdProducto);

                if (productoEnSede == null)
                {
                    return "Error404: El producto no se encuentra registrado en la sede.";
                }

                productoEnSede.StockDisponible = sedeProducto.StockDisponible;
                db.SaveChanges();

                return "Stock actualizado correctamente.";
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return "Error al actualizar el stock: " + inner;
            }
        }

        public string EliminarProductoSede(int idProducto)
        {
            try
            {
                List<SedeProducto> productoes = db.SedeProductoes.Where(a => a.IdProducto == idProducto).ToList();


                if (productoes == null)
                {
                    return "Error404: Producto en la sede no encontrado";
                }

                if (productoes.Count > 0)
                {
                    foreach (SedeProducto item in productoes)
                    {
                        db.SedeProductoes.Remove(item);
                    }
                    db.SaveChanges();
                    return "El producto ha sido eliminado correctamente";
                }
                else
                {
                    return "Error: El producto no se encuentra registrado en la sede";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el producto: " + ex.Message;
            }
        }

        public string EliminarProductoSedeId(int id)
        {

            try
            {
                SedeProducto producto = db.SedeProductoes.FirstOrDefault(sp => sp.Id == id);
                if (producto == null)
                {
                    return "Error404: Producto en la sede no encontrado";
                }
                db.SedeProductoes.Remove(producto);
                db.SaveChanges();
                return "El producto ha sido eliminado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al eliminar el producto: " + ex.Message;
            }

        }


    }
}