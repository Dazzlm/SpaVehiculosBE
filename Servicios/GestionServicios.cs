using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaVehiculosBE.Servicios
{
    public class GestionServicios
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();

        public RespuestaServicio<string> CrearServicio(Servicio servicio)
        {
            try
            {
                db.Servicios.Add(servicio);
                db.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Servicio creado con éxito");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al crear el servicio: " + ex.Message);
            }
        }

        public RespuestaServicio<Servicio> BuscarServicioID(int idServicio)
        {
            try {
                Servicio servicio = db.Servicios.FirstOrDefault(a => a.IdServicio == idServicio);
                if (servicio == null)
                {
                    return RespuestaServicio<Servicio>.ConError("Error404: El servicio con el ID ingresado no existe");
                }
                return RespuestaServicio<Servicio>.ConExito(servicio);

            }
            catch(Exception ex)
            {
                return RespuestaServicio<Servicio>.ConError("Error al buscar el servicio: " + ex.Message);
            }
            

        }

        public RespuestaServicio<Servicio> BuscarServicioNombre(string nombre)
        {
            try {
                Servicio servicio = db.Servicios.FirstOrDefault(a => a.Nombre == nombre);
                if (servicio == null)
                {
                    return RespuestaServicio<Servicio>.ConError("Error404: El servicio con el nombre ingresado no existe");
                }
                return RespuestaServicio<Servicio>.ConExito(servicio);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<Servicio>.ConError("Error al buscar el servicio: " + ex.Message);
            }
            
        }

        public RespuestaServicio<List<Servicio>> BuscarServicioTodos()
        {
            try
            {
                List<Servicio> servicios = db.Servicios.ToList();
                if (servicios == null)
                {
                    return RespuestaServicio<List<Servicio>>.ConError("Error404: No hay servicios para mostrar");
                }

                return RespuestaServicio<List<Servicio>>.ConExito(servicios, "Servicios consultados correctamente");
            }
            catch (Exception ex) { 
                return RespuestaServicio<List<Servicio>>.ConError("Error al buscar los servicios: " + ex.Message);
            }
            
        }

        public RespuestaServicio<string> ActualizarServicio(Servicio servicioActualizado)
        {
            try
            {

                var servicio = db.Servicios.FirstOrDefault(a => a.IdServicio == servicioActualizado.IdServicio);
                if (servicio != null)
                {
                    servicio.Nombre = servicioActualizado.Nombre;
                    servicio.Descripción = servicioActualizado.Descripción;
                    servicio.Precio = servicioActualizado.Precio;
                    servicio.DuraciónMinutos = servicioActualizado.DuraciónMinutos;
                    db.SaveChanges();
                    return RespuestaServicio<string>.ConExito(default, "Servicio editado con éxito");
                }
                else
                {
                    return RespuestaServicio<string>.ConError("Error404: Servicio no encontrado");
                }

            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al actualizar el servicio: " + ex.Message);
            }
        }
        public RespuestaServicio<string> EliminarServicio(int idServicio)
        {
            try
            {
                var servicio = db.Servicios.FirstOrDefault(a => a.IdServicio == idServicio);
                if (servicio != null)
                {
                    db.Servicios.Remove(servicio);
                    db.SaveChanges();
                    return RespuestaServicio<string>.ConExito(default, "El servicio ha sido eliminado correctamente");
                }
                else
                {
                    return RespuestaServicio<string>.ConError("El servicio no se encuentra registrado");
                }
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al eliminar el servicio: " + ex.Message);
            }
        }
    }
}

