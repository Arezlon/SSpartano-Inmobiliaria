using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public DateTime Fecha { get; set; }
        public Contrato Contrato { get; set; }
    }
}
