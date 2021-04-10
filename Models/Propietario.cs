using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Propietario
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), StringLength(22, MinimumLength = 3, ErrorMessage = "3 caracteres mínimo, 22 máximo"), DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), StringLength(22, MinimumLength = 3, ErrorMessage = "3 caracteres mínimo, 22 máximo"), DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Display(Name = "DNI"), Required(ErrorMessage = "Campo obligatorio"), StringLength(8, MinimumLength = 8, ErrorMessage = "Ingrese un número de DNI válido (8 caracteres)"), DataType(DataType.Text)]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
        public string Email { get; set; }
        [Display(Name = "Teléfono"), Required(ErrorMessage = "Campo obligatorio"), DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required, DataType(DataType.Password), StringLength(16, MinimumLength = 8, ErrorMessage = "8 caracteres mínimo, 16 máximo")]
        public string Clave { get; set; }
        public override string ToString()
        {
            return Apellido + " " + Nombre + " (Cód: " + Id + ")";
        }
    }
}
