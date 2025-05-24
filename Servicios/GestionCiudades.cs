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

        public string Insertar()
        {
            try
            {
                dbSuper.Ciudads.Add(ciudad);
                dbSuper.SaveChanges();
                return "Ciudad insertada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al insertar la ciudad: " + ex.Message;
            }
        }

        public string Actualizar()
        {
            try
            {
                Ciudad c = Consultar(ciudad.IdCiudad);
                if (c == null)
                {
                    return "La ciudad con el ID ingresado no existe, por lo tanto no se puede actualizar";
                }
                dbSuper.Ciudads.AddOrUpdate(ciudad);
                dbSuper.SaveChanges();
                return "Se actualizó la ciudad correctamente";
            }
            catch (Exception ex)
            {
                return "No se pudo actualizar la ciudad: " + ex.Message;
            }
        }

        public List<Ciudad> ConsultarTodos()
        {
            return dbSuper.Ciudads
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public Ciudad Consultar(int IdCiudad)
        {
            return dbSuper.Ciudads.FirstOrDefault(c => c.IdCiudad == IdCiudad);
        }
        public string EliminarXId(int IdCiudad)
        {
            try
            {
                Ciudad c = Consultar(IdCiudad);
                if (c == null)
                {
                    return "La ciudad con el ID ingresado no existe, por lo tanto no se puede eliminar";
                }
                dbSuper.Ciudads.Remove(c);
                dbSuper.SaveChanges();
                return "Se eliminó la ciudad correctamente";
            }
            catch (Exception ex)
            {
                return "No se pudo eliminar la ciudad: " + ex.Message;
            }
        }
    }
}
