using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Servicios
{
    public class GestionProveedores
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();
        public RespuestaServicio<string> CrearProvedor(Proveedor proveedor)

        {
            try
            {
                db.Proveedors.Add(proveedor);
                db.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Proveedor creado con exito");

            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al crear el proveedor: " + ex);
            }
        }
        public RespuestaServicio<Proveedor> BuscarProveedorID(int idProveedor)
        {
            try {
                Proveedor proveedor = db.Proveedors.FirstOrDefault(p => p.IdProveedor == idProveedor);
                if (proveedor == null)
                {
                    return RespuestaServicio<Proveedor>.ConError("Error404: El proveedor con el ID ingresado no existe");
                }

                return RespuestaServicio<Proveedor>.ConExito(proveedor);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<Proveedor>.ConError("Error al buscar el proveedor: " + ex.Message);
            }
            
        }
       
        public RespuestaServicio<List<Proveedor>> BuscarProveedorTodos()
        {
            try {
                List<Proveedor> proveedor = db.Proveedors.ToList();
                if (proveedor == null )
                {
                    return RespuestaServicio<List<Proveedor>>.ConError("Error404: No hay proveedores para mostrar");
                }
                return RespuestaServicio<List<Proveedor>>.ConExito(proveedor);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<List<Proveedor>>.ConError("Error al buscar los proveedores: " + ex.Message);
            }
            
        }
        public RespuestaServicio<string> EliminarProveedor(int idProveedor)
        {

            try
            {
                Proveedor proveedor = db.Proveedors.FirstOrDefault(p => p.IdProveedor== idProveedor);
                if (proveedor != null)
                {
                    db.Proveedors.Remove(proveedor);
                    db.SaveChanges();
                    return RespuestaServicio<string>.ConExito( default,"El proveedor ha sido eliminado correctamente");
                }
                else
                {
                    return RespuestaServicio<string>.ConError("El proveedor no se encuentra registrado");
                }
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError( "Error al eliminar el proveedor: " + ex.Message);
            }

        }
        public RespuestaServicio<string> ActualizarProveedor(Proveedor prov)
        {
            try
            {
                Proveedor proveedor = db.Proveedors.FirstOrDefault(p => p.IdProveedor== prov.IdProveedor);
                if (proveedor != null)
                {
                    proveedor.IdProveedor = prov.IdProveedor;
                    proveedor.NombreEmpresa = prov.NombreEmpresa;
                    proveedor.Contacto = prov.Contacto;
                    proveedor.Teléfono=prov.Teléfono;
                    proveedor.Email = prov.Email; 
                    db.SaveChanges();
                    return RespuestaServicio<string>.ConExito(default,"El proveedor ha sido actualizado exitosamente.");

                }
                else
                {
                    return RespuestaServicio<string>.ConError("Error404: El proveedor no existe.");
                }
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al actualizar el proveedor: " + ex.Message);

            }

        }

    }
}