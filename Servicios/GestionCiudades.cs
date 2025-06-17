using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ServicesClass.Clases
{
    public class GestionCiudades
    {
        private SpaVehicularDBEntities dbSuper = new SpaVehicularDBEntities();
        public Ciudad ciudad { get; set; }

        public RespuestaServicio<string> Insertar()
        {
            try
            {
                dbSuper.Ciudads.Add(ciudad);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Ciudad insertada correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("Error al insertar la ciudad: " + ex.Message);
            }
        }

        public RespuestaServicio<string> Actualizar()
        {
            try
            {
                RespuestaServicio<Ciudad> c = Consultar(ciudad.IdCiudad);
                if (c.Data == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: La ciudad con el ID ingresado no existe, por lo tanto no se puede actualizar");
                }
                dbSuper.Ciudads.AddOrUpdate(ciudad);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Se actualizó la ciudad correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("No se pudo actualizar la ciudad: " + ex.Message);
            }
        }

        public RespuestaServicio<List<Ciudad>> ConsultarTodos()
        {

            try {
                List<Ciudad> ciudades = dbSuper.Ciudads
                .OrderBy(c => c.Nombre)
                .ToList();
                if (ciudades == null)
                {
                    return RespuestaServicio<List<Ciudad>>.ConError("Error404: No hay ciudades para mostrar");
                }
                return RespuestaServicio<List<Ciudad>>.ConExito(ciudades, "Ciudades consultadas correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<List<Ciudad>>.ConError("Error al consultar las ciudades: " + ex.Message);
            }

        }

        public RespuestaServicio<Ciudad> Consultar(int IdCiudad)
        {
            try {
                Ciudad ciudad = dbSuper.Ciudads.FirstOrDefault(c => c.IdCiudad == IdCiudad);
                if (ciudad == null)
                {
                    return RespuestaServicio<Ciudad>.ConError("Error404: Ciudad no encontrada");
                }
            }
            catch (Exception ex)
            {
                return RespuestaServicio<Ciudad>.ConError("Error al consultar la ciudad: " + ex.Message);
            }
            
            return RespuestaServicio<Ciudad>.ConExito(ciudad);
        }
        public RespuestaServicio<string> EliminarXId(int IdCiudad)
        {
            try
            {
                RespuestaServicio<Ciudad>  c = Consultar(IdCiudad);
                if (c.Data == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: La ciudad con el ID ingresado no existe, por lo tanto no se puede eliminar");
                }
                dbSuper.Ciudads.Remove(c.Data);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Se eliminó la ciudad correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("No se pudo eliminar la ciudad: " + ex.Message);
            }
        }
    }
}
