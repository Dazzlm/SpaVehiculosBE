//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpaVehiculosBE.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class SedeProducto
    {
        public int Id { get; set; }
        public int IdSede { get; set; }
        public int IdProducto { get; set; }
        public int StockDisponible { get; set; }
        [JsonIgnore]
        public virtual Producto Producto { get; set; }
        [JsonIgnore]
        public virtual Sede Sede { get; set; }
    }
}
