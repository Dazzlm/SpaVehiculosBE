using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaVehiculosBE.Servicios
{
    public class GestionServicios
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();

        public string CrearServicio(Servicio servicio)
        {
            try
            {
                db.Servicios.Add(servicio);
                db.SaveChanges();
                return "Servicio creado con éxito";
            }
            catch (Exception ex)
            {
                return "Error al crear el servicio: " + ex.Message;
            }
        }

        public Servicio BuscarServicioID(int idServicio)
        {
            return db.Servicios.FirstOrDefault(a => a.IdServicio == idServicio);
        }

        public Servicio BuscarServicioNombre(string nombre)
        {
            return db.Servicios.FirstOrDefault(a => a.Nombre == nombre);
        }

        public List<Servicio> BuscarServicioTodos()
        {
            return db.Servicios.ToList();
        }

        public string ActualizarServicio(Servicio servicioActualizado)
        {
            var servicio = db.Servicios.FirstOrDefault(a => a.IdServicio == servicioActualizado.IdServicio);
            if (servicio != null)
            {
                servicio.Nombre = servicioActualizado.Nombre;
                servicio.Descripción = servicioActualizado.Descripción;
                servicio.Precio = servicioActualizado.Precio;
                servicio.DuraciónMinutos = servicioActualizado.DuraciónMinutos;
                db.SaveChanges();
                return "Servicio editado con éxito";
            }
            else
            {
                return "Servicio no encontrado";
            }
        }

        public string EliminarServicio(int idServicio)
        {
            try
            {
                var servicio = db.Servicios.FirstOrDefault(a => a.IdServicio == idServicio);
                if (servicio != null)
                {
                    db.Servicios.Remove(servicio);
                    db.SaveChanges();
                    return "El servicio ha sido eliminado correctamente";
                }
                else
                {
                    return "El servicio no se encuentra registrado";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar el servicio: " + ex.Message;
            }
        }
    }
}

