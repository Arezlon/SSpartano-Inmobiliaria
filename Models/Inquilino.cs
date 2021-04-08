using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Inquilino
    {
		[Display(Name = "Código")]
		public int Id { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo"), DataType(DataType.Text)]
		public string Nombre { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo"), DataType(DataType.Text)]
		public string Apellido { get; set; }
		[Display(Name = "DNI"), Required(ErrorMessage = "Campo obligatorio"), StringLength(8, MinimumLength = 8, ErrorMessage = "Ingrese un número de DNI válido (8 caracteres)"), DataType(DataType.Text)]
		public string Dni { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
		public string Email { get; set; }
		[Display(Name = "Teléfono"), Required(ErrorMessage = "Campo obligatorio"), DataType(DataType.PhoneNumber)]
		public string Telefono { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "Lugar de trabajo"), StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo"), DataType(DataType.Text)]
		public string LugarTrabajo { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "Nombre del garante"), StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo"), DataType(DataType.Text)]
		public string GaranteNombre { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "Apellido del garante"),StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo"), DataType(DataType.Text)]
		public string GaranteApellido { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "DNI del garante"), StringLength(8, MinimumLength = 8, ErrorMessage = "Ingrese un número de DNI válido (8 caracteres)"), DataType(DataType.Text)]
		public string GaranteDni { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "Teléfono del garante"), DataType(DataType.PhoneNumber)]
		public string GaranteTelefono { get; set; }
		[Required(ErrorMessage = "Campo obligatorio"), Display(Name = "E-mail del garante"), EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
		public string GaranteEmail { get; set; }
		public override string ToString()
		{
			return Apellido + " " + Nombre + " (Cód: " + Id + ")";
		}
	}
}
