using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Inmueble //v
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }
        public string Direccion { get; set; }
        public string Uso { get; set; }
        public string Tipo { get; set; }
        public int Ambientes { get; set; }
        public int Precio { get; set; }
        public int Estado { get; set; }
        public Propietario Propietario { get; set; }

    }
}
