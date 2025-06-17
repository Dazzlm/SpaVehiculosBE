using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaVehiculosBE.Models
{
    public class RespuestaServicio<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static RespuestaServicio<T> ConError(string mensaje) =>
           new RespuestaServicio<T> { 
               Success = false,
               Message = mensaje,
               Data = default
           };

        public static RespuestaServicio<T> ConExito(T datos = default, string mensaje="") =>
            new RespuestaServicio<T> {
                Success = true,
                Message = mensaje,
                Data = datos, };
    }

}