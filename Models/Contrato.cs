using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSpartanoInmobiliaria.Models
{
    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Inmueble")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int InmuebleId { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cierre")]
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
        //ESTADOS (0 Eliminado - 1 Activo - 2 Terminado)
        public Inmueble Inmueble { get; set; }
        public Inquilino Inquilino { get; set; }
        public Propietario Propietario { get; set; }
        public int MesesTotales => ((FechaFin.Year - FechaInicio.Year) * 12) + FechaFin.Month - FechaInicio.Month;
        public int EstadoPagos { get; set; }
        //ESTADOS DE PAGOS (1 Al dia, 2 Vencido)
        public DateTime ProximoPago;
    }
}
