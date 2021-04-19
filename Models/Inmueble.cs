using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Propietario")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int PropietarioId { get; set; }
        [Required(ErrorMessage = "Campo obligatorio"), StringLength(50, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 50 máximo")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Uso { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo obligatorio"), StringLength(2, MinimumLength = 1, ErrorMessage = "1 ambiente mínimo, 99 máximo")]
        public int Ambientes { get; set; }
        [Display(Name = "Precio"), StringLength(6, MinimumLength = 3, ErrorMessage = "3 dígitos mínimo, 6 máximo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Precio { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Estado")]
        public int Estado { get; set; }
        //ESTADOS (0 Eliminado - 1 Activo - 2 Oculto)
        public Propietario Propietario { get; set; }
        public override string ToString()
        {
            return Direccion + " (" + Uso + ", Cód: " + Id + ")";
        }
    }
}
