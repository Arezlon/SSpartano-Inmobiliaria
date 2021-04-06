﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "6 caracteres mínimo, 16 máximo")]
        [DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Ingrese un número de DNI válido")]
        [DataType(DataType.Text)]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Telefono { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "8 caracteres mínimo, 16 máximo")]
        public string Clave { get; set; }
    }
}