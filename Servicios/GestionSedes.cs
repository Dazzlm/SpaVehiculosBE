using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ServicesClass.Clases
{
    public class GestionSedes
    {
        private SpaVehicularDBEntities dbSuper = new SpaVehicularDBEntities();
        public Sede sede { get; set; }

        public RespuestaServicio<string> Insertar()
        {
            try
            {
                dbSuper.Sedes.Add(sede);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Sede insertada correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError( "Error al insertar la sede: " + ex.Message);
            }
        }

        public RespuestaServicio<string> Actualizar()
        {
            try
            {
                RespuestaServicio<Sede> s = Consultar(sede.IdSede);
                if (s.Data == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: La sede con el ID ingresado no existe, por lo tanto no se puede actualizar");
                }
                dbSuper.Sedes.AddOrUpdate(sede);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default, "Se actualizó la sede correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError( "No se pudo actualizar la sede: " + ex.Message);
            }
        }

        public RespuestaServicio<List<Sede>> ConsultarTodos()
        {

            try {
                List <Sede> sedes =dbSuper.Sedes.OrderBy(s => s.Nombre).ToList();
                return RespuestaServicio<List<Sede>>.ConExito(sedes, "Sedes consultadas correctamente");
            }
            catch (Exception ex) { 
                return RespuestaServicio<List<Sede>>.ConError("Error al consultar las sedes: " + ex.Message);
            }

        }

        public RespuestaServicio<Sede> Consultar(int IdSede)
        {
            try {
                Sede sede = dbSuper.Sedes.FirstOrDefault(s => s.IdSede == IdSede);

                if (sede == null) {
                    return RespuestaServicio<Sede>.ConError("Error404: No se encontro la sede");
                }

                return RespuestaServicio<Sede>.ConExito(sede);
            }
            catch (Exception ex)
            {
                return RespuestaServicio<Sede>.ConError("Error al consultar la sede: " + ex.Message);
            }
        }
        public RespuestaServicio<string> EliminarXId(int IdSede)
        {
            try
            {
                RespuestaServicio<Sede> s = Consultar(IdSede);
                if (s.Data == null)
                {
                    return RespuestaServicio<string>.ConError("Error404: La sede con el ID ingresado no existe, por lo tanto no se puede eliminar");
                }
                dbSuper.Sedes.Remove(s.Data);
                dbSuper.SaveChanges();
                return RespuestaServicio<string>.ConExito(default,"Se eliminó la sede correctamente");
            }
            catch (Exception ex)
            {
                return RespuestaServicio<string>.ConError("No se pudo eliminar la sede: " + ex.Message);
            }
        }
    }
}
