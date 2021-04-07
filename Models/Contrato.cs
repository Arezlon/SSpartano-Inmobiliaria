using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Contrato //v
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
        public Inmueble Inmueble { get; set; }
        public Inquilino Inquilino { get; set; }
    }
}
