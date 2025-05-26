
namespace SpaVehiculosBE.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class SpaVehicularDBEntities : DbContext
    {
        public SpaVehicularDBEntities()
            : base("name=SpaVehicularDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Administrador> Administradors { get; set; }
        public virtual DbSet<Ciudad> Ciudads { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<DetalleFacturaProducto> DetalleFacturaProductoes { get; set; }
        public virtual DbSet<DetalleFacturaServicio> DetalleFacturaServicios { get; set; }
        public virtual DbSet<EmailEnviado> EmailEnviadoes { get; set; }
        public virtual DbSet<EmpleadoServicio> EmpleadoServicios { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<HistorialClienteProducto> HistorialClienteProductoes { get; set; }
        public virtual DbSet<HistorialClienteServicio> HistorialClienteServicios { get; set; }
        public virtual DbSet<LogActividad> LogActividads { get; set; }
        public virtual DbSet<Producto> Productoes { get; set; }
        public virtual DbSet<Proveedor> Proveedors { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Sede> Sedes { get; set; }
        public virtual DbSet<SedeProducto> SedeProductoes { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<TurnoEmpleado> TurnoEmpleadoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

    }
}