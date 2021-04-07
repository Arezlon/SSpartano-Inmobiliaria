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
        [Required(ErrorMessage = "Campo obligatorio")]
        public int PropietarioId { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Uso { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Ambientes { get; set; }
        [Display(Name = "Precio por mes")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Precio { get; set; }
        public int Estado { get; set; }
        public Propietario Propietario { get; set; }
        public override string ToString()
        {
            return Direccion + " (" + Uso + ", Cód: " + Id + ")";
        }
    }
}
