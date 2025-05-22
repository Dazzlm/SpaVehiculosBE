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
        public string CrearProvedor(Proveedor proveedor)

        {
            try
            {
                db.Proveedors.Add(proveedor);
                db.SaveChanges();
                return "Proveedor creado con exito";

            }
            catch (Exception ex)
            {
                return "Error al crear el proveedor: " + ex;
            }
        }
        public Proveedor BuscarProveedorID(int idProveedor)
        {
            Proveedor proveedor = db.Proveedors.FirstOrDefault(p => p.IdProveedor == idProveedor);
            return proveedor;
        }
       
        public List<Proveedor> BuscarProveedorTodos()
        {
            List<Proveedor> proveedor = db.Proveedors.ToList();
            return proveedor;
        }
        public string EliminarProveedor(int idProveedor)
        {

            try
            {
                Proveedor proveedor = db.Proveedors.FirstOrDefault(p => p.IdProveedor== idProveedor);
                if (proveedor != null)
                {
                    db.Proveedors.Remove(proveedor);
                    db.SaveChanges();
                    return "El proveedor ha sido eliminado correctamente";
                }
                else
                {
                    return "El proveedor no se encuentra registrado";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el proveedor: " + ex.Message;
            }

        }
        public string ActualizarProveedor(Proveedor prov)
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
                    return "El proveedor ha sido actualizado exitosamente.";

                }
                else
                {
                    return "El proveedor no existe.";
                }
            }
            catch (Exception ex)
            {
                return "Error al actualizar el proveedor: " + ex.Message;

            }

        }

    }
}