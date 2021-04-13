using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public int TipoCuenta { get; set; }
        public enum eTipoCuenta
        {
            Empleado = 1,
            Administrador = 2,
        }
        public string TipoCuentaNombre => TipoCuenta > 0 ? ((eTipoCuenta)TipoCuenta).ToString() : "";
        public int Estado { get; set; }
    }
}
