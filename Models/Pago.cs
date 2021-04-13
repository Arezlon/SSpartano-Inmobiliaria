using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Pago
    {
        [Display(Name = "Código del pago")]
        public int Id { get; set; }
        [Display(Name = "Código del contrato")]
        public int ContratoId { get; set; }
        public DateTime Fecha { get; set; }
        public Contrato Contrato { get; set; }
    }
}
