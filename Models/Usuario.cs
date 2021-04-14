using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Usuario
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio"), StringLength(22, MinimumLength = 3, ErrorMessage = "3 caracteres mínimo, 22 máximo"), DataType(DataType.Text)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo obligatorio"), StringLength(22, MinimumLength = 3, ErrorMessage = "3 caracteres mínimo, 22 máximo"), DataType(DataType.Text)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Campo obligatorio"), EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), StringLength(16, MinimumLength = 8, ErrorMessage = "8 caracteres mínimo, 16 máximo")]
        public string Clave { get; set; }
        [Display(Name = "Tipo de cuenta")]
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
