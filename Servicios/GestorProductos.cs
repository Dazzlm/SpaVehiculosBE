using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class GestorProductos
    {

        public class ProductoConStockDTO
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
            public string Descripción { get; set; }
            public Nullable<decimal> Precio { get; set; }
            public int IdProveedor { get; set; }
            public string Imagen { get; set; }
            public int Stock { get; set; }
        }

        private readonly SpaVehicularDBEntities _dbContext = new SpaVehicularDBEntities();
        public List<Producto> ObtenerTodos()
        {
            List<Producto> productos = _dbContext.Productoes.ToList();
            return productos;
        }

        public Producto ObtenerPorId(int id)
        {
            return _dbContext.Productoes.FirstOrDefault(p => p.IdProducto == id);
        }

        public List<ProductoConStockDTO> ObtenerConStockPorSede(int idSede)
        {


            List<SedeProducto> stockPorSede = _dbContext.SedeProductoes
            .Where(s => s.IdSede == idSede && s.StockDisponible != 0)
            .ToList();

            List<int> productoIds = stockPorSede.Select(s => s.IdProducto).Distinct().ToList();

            List<Producto> productos = _dbContext.Productoes
                .Where(p => productoIds.Contains(p.IdProducto))
                .ToList();

            List<ProductoConStockDTO> resultado = productos.Select(p => new ProductoConStockDTO
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Descripción = p.Descripción,
                IdProveedor = p.IdProveedor,
                Imagen = p.Imagen,
                Stock = stockPorSede.FirstOrDefault(s => s.IdProducto == p.IdProducto)?.StockDisponible ?? 0
            }).ToList();

            return resultado;

        }

        public List<Producto> ObtenerPorSede(int idSede)
        {
            return _dbContext.SedeProductoes
                .Include(sp => sp.Producto)
                .Where(sp => sp.IdSede == idSede)
                .Select(sp => sp.Producto)
                .ToList();
        }

        public string Crear(Producto producto)
        {
            try
            {
                _dbContext.Productoes.Add(producto);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            return "Producto creado con éxito";
        }

        public string Actualizar(Producto producto)
        {
            try
            {
                Producto actual = _dbContext.Productoes.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
                if (actual == null)
                {
                    return "Error404: Producto no encontrado";
                }


                actual.IdProducto = producto.IdProducto;
                actual.Nombre = producto.Nombre;
                actual.Descripción = producto.Descripción;
                actual.Precio = producto.Precio;
                actual.IdProveedor = producto.IdProveedor;
                actual.Imagen = producto.Imagen;

                _dbContext.Productoes.AddOrUpdate(actual);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            return "Producto editado con éxito";

        }

        public string Eliminar(int id)
        {
            try
            {
                Producto producto = _dbContext.Productoes.FirstOrDefault(p => p.IdProducto == id);
                if (producto == null)
                {
                    return "Error404: Producto no encontrado";
                }

                List<SedeProducto> sedeProductos = _dbContext.SedeProductoes.Where(sp => sp.IdProducto == id).ToList();
                foreach (SedeProducto sp in sedeProductos)
                {
                    _dbContext.SedeProductoes.Remove(sp);
                }

                _dbContext.Productoes.Remove(producto);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            return "Producto eliminado con éxito";
        }

    }
}