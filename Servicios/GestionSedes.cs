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

        public string Insertar()
        {
            try
            {
                dbSuper.Sedes.Add(sede);
                dbSuper.SaveChanges();
                return "Sede insertada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar la sede: " + ex.Message;
            }
        }

        public string Actualizar()
        {
            try
            {
                Sede s = Consultar(sede.IdSede);
                if (s == null)
                {
                    return "La sede con el ID ingresado no existe, por lo tanto no se puede actualizar";
                }
                dbSuper.Sedes.AddOrUpdate(sede);
                dbSuper.SaveChanges();
                return "Se actualizó la sede correctamente";
            }
            catch (Exception ex)
            {
                return "No se pudo actualizar la sede: " + ex.Message;
            }
        }

        public List<Sede> ConsultarTodos()
        {
            return dbSuper.Sedes
                .OrderBy(s => s.Nombre)
                .ToList();
        }

        public Sede Consultar(int IdSede)
        {
            return dbSuper.Sedes.FirstOrDefault(s => s.IdSede == IdSede);
        }
        public string EliminarXId(int IdSede)
        {
            try
            {
                Sede s = Consultar(IdSede);
                if (s == null)
                {
                    return "La sede con el ID ingresado no existe, por lo tanto no se puede eliminar";
                }
                dbSuper.Sedes.Remove(s);
                dbSuper.SaveChanges();
                return "Se eliminó la sede correctamente";
            }
            catch (Exception ex)
            {
                return "No se pudo eliminar la sede: " + ex.Message;
            }
        }
    }
}
