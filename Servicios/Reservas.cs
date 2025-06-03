using SpaVehiculosBE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SpaVehiculosBE.Servicios
{
    public class Reservas
    {
        private readonly SpaVehicularDBEntities db = new SpaVehicularDBEntities();

        public string CrearReserva(Reserva reserva)
        {
            try
            {
                if (!reserva.Fecha.HasValue || !reserva.HoraInicio.HasValue)
                {
                    return "La fecha y hora de inicio de la reserva son obligatorias.";
                }

                var clienteExiste = db.Clientes.Any(c => c.IdCliente == reserva.IdCliente);
                if (!clienteExiste)
                {
                    return "Cliente no válido. No se puede crear la reserva.";
                }

                var servicio = db.Servicios.FirstOrDefault(s => s.IdServicio == reserva.IdServicio);
                if (servicio == null)
                {
                    return "Servicio no válido. No se puede crear la reserva.";
                }

                if (!servicio.DuraciónMinutos.HasValue || servicio.DuraciónMinutos.Value <= 0)
                {
                    return "El servicio seleccionado no tiene una duración válida definida.";
                }

                var sedeExiste = db.Sedes.Any(s => s.IdSede == reserva.IdSede);
                if (!sedeExiste)
                {
                    return "Sede no válida. No se puede crear la reserva.";
                }
                DateTime fechaHoraInicio = reserva.Fecha.Value.Date + reserva.HoraInicio.Value;

                if (fechaHoraInicio < DateTime.Now)
                {
                    return "La fecha y hora de la reserva no pueden ser anteriores al momento actual.";
                }

                TimeSpan duracionServicio = TimeSpan.FromMinutes(servicio.DuraciónMinutos.Value);
                TimeSpan horaFinCalculada = (fechaHoraInicio + duracionServicio).TimeOfDay;

                reserva.HoraFin = horaFinCalculada;

                DateTime fechaHoraFinCalculada = fechaHoraInicio + duracionServicio;

                var reservasSolapadas = db.Reservas
                    .Where(r => r.IdSede == reserva.IdSede && 
                                r.Fecha.HasValue && r.HoraInicio.HasValue && r.HoraFin.HasValue && 
                                DbFunctions.CreateDateTime(r.Fecha.Value.Year, r.Fecha.Value.Month, r.Fecha.Value.Day, r.HoraInicio.Value.Hours, r.HoraInicio.Value.Minutes, r.HoraInicio.Value.Seconds) < fechaHoraFinCalculada && // La reserva existente comienza antes de que la nueva termine
                                DbFunctions.CreateDateTime(r.Fecha.Value.Year, r.Fecha.Value.Month, r.Fecha.Value.Day, r.HoraFin.Value.Hours, r.HoraFin.Value.Minutes, r.HoraFin.Value.Seconds) > fechaHoraInicio) // La reserva existente termina después de que la nueva comienza
                    .ToList();

                if (reservasSolapadas.Any())
                {
                    return "Ya existe una reserva que se solapa en este horario y sede. Por favor, elige otro horario.";
                }


                db.Reservas.Add(reserva);
                db.SaveChanges();
                return "Reserva creada con éxito";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CrearReserva: {ex.Message} - StackTrace: {ex.StackTrace}");
                return "Error al crear la reserva: " + ex.Message;
            }
        }

        public Reserva ConsultarReservaID(int idReserva)
        {
            return db.Reservas
                     .Include(r => r.Cliente)
                     .Include(r => r.Sede)
                     .Include(r => r.Servicio)
                     .FirstOrDefault(a => a.IdReserva == idReserva);
        }

        public List<Reserva> ConsultarReservasPorCliente(int idCliente)
        {
            return db.Reservas
                     .Include(r => r.Cliente)
                     .Include(r => r.Sede)
                     .Include(r => r.Servicio)
                     .Where(a => a.IdCliente == idCliente)
                     .ToList();
        }
        public List<Reserva> ConsultarReservasPorFecha(DateTime fecha)
        {
            var inicioDia = fecha.Date;
            var finDia = inicioDia.AddDays(1); 

            return db.Reservas
                     .Include(r => r.Cliente)
                     .Include(r => r.Sede)
                     .Include(r => r.Servicio)
                     .Where(a => a.Fecha.HasValue && a.Fecha.Value >= inicioDia && a.Fecha.Value < finDia)
                     .ToList();
        }

        public List<Reserva> ConsultarReservasTodos()
        {
            return db.Reservas
                     .Include(r => r.Cliente)
                     .Include(r => r.Sede)
                     .Include(r => r.Servicio)
                     .ToList();
        }


        public string ActualizarReserva(Reserva reservaActualizada)
        {
            if (!reservaActualizada.Fecha.HasValue || !reservaActualizada.HoraInicio.HasValue)
            {
                return "La fecha y hora de inicio de la reserva son obligatorias.";
            }

            var reservaExistente = db.Reservas
                                     .Include(r => r.Servicio) 
                                     .FirstOrDefault(a => a.IdReserva == reservaActualizada.IdReserva);
            if (reservaExistente == null)
            {
                return "Reserva no encontrada.";
            }

            var clienteExiste = db.Clientes.Any(c => c.IdCliente == reservaActualizada.IdCliente);
            if (!clienteExiste)
            {
                return "Cliente no válido.";
            }

            var sedeExiste = db.Sedes.Any(s => s.IdSede == reservaActualizada.IdSede);
            if (!sedeExiste)
            {
                return "Sede no válida.";
            }

            var nuevoServicio = db.Servicios.FirstOrDefault(s => s.IdServicio == reservaActualizada.IdServicio);
            if (nuevoServicio == null)
            {
                return "Servicio no válido.";
            }
            if (!nuevoServicio.DuraciónMinutos.HasValue || nuevoServicio.DuraciónMinutos.Value <= 0)
            {
                return "El servicio seleccionado no tiene una duración válida definida.";
            }

 
            DateTime nuevaFechaHoraInicio = reservaActualizada.Fecha.Value.Date + reservaActualizada.HoraInicio.Value;

            if (nuevaFechaHoraInicio < DateTime.Now && reservaExistente.IdReserva != reservaActualizada.IdReserva)
            {

                return "La nueva fecha y hora de la reserva no pueden ser anteriores al momento actual.";
            }


            TimeSpan nuevaDuracionServicio = TimeSpan.FromMinutes(nuevoServicio.DuraciónMinutos.Value);
            TimeSpan nuevaHoraFinCalculada = (nuevaFechaHoraInicio + nuevaDuracionServicio).TimeOfDay;


            DateTime nuevaFechaHoraFinCalculada = nuevaFechaHoraInicio + nuevaDuracionServicio;

            var reservasSolapadas = db.Reservas
                .Where(r => r.IdReserva != reservaActualizada.IdReserva && 
                            r.IdSede == reservaActualizada.IdSede &&       
                            r.Fecha.HasValue && r.HoraInicio.HasValue && r.HoraFin.HasValue && 
                            DbFunctions.CreateDateTime(r.Fecha.Value.Year, r.Fecha.Value.Month, r.Fecha.Value.Day, r.HoraInicio.Value.Hours, r.HoraInicio.Value.Minutes, r.HoraInicio.Value.Seconds) < nuevaFechaHoraFinCalculada &&
                            DbFunctions.CreateDateTime(r.Fecha.Value.Year, r.Fecha.Value.Month, r.Fecha.Value.Day, r.HoraFin.Value.Hours, r.HoraFin.Value.Minutes, r.HoraFin.Value.Seconds) > nuevaFechaHoraInicio)
                .ToList();

            if (reservasSolapadas.Any())
            {
                return "Ya existe otra reserva que se solapa en este horario y sede. Por favor, elige otro horario para la actualización.";
            }

            reservaExistente.Fecha = reservaActualizada.Fecha;
            reservaExistente.HoraInicio = reservaActualizada.HoraInicio;
            reservaExistente.HoraFin = nuevaHoraFinCalculada;
            reservaExistente.IdServicio = reservaActualizada.IdServicio;
            reservaExistente.IdCliente = reservaActualizada.IdCliente;
            reservaExistente.IdSede = reservaActualizada.IdSede;


            try
            {
                db.SaveChanges();
                return "Reserva actualizada con éxito";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ActualizarReserva: {ex.Message} - StackTrace: {ex.StackTrace}");
                return "Error al actualizar la reserva: " + ex.Message;
            }
        }

        public string EliminarReserva(int idReserva)
        {
            var reserva = db.Reservas.FirstOrDefault(a => a.IdReserva == idReserva);
            if (reserva != null)
            {
                try
                {
                    db.Reservas.Remove(reserva);
                    db.SaveChanges();
                    return "Reserva eliminada con éxito";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en EliminarReserva: {ex.Message} - StackTrace: {ex.StackTrace}");
                    return "Error al eliminar la reserva: " + ex.Message;
                }
            }
            else
            {
                return "Reserva no encontrada.";
            }
        }


    }
}