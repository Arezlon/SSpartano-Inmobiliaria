using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Inmueble //v
    {
        public int Id { get; set; }
        public int PropietarioId { get; set; }
        public string Uso { get; set; }
        public string Tipo { get; set; }
        public int Ambientes { get; set; }
        public int Precio { get; set; }
        public int Estado { get; set; }

    }
}
