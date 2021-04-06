using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Contrato //v
    {
        public int Id { get; set; }
        public int InmuebleId { get; set; }
        public int InquilinoId { get; set; }
        public int FechaInicio { get; set; }
        public int FechaFin { get; set; }
        public int Estado { get; set; }
    }
}
