using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOAutoridadEmpresa
    {
        public int IdAutoridadEmpresa { get; set; }
        public long? IdEmpresa { get; set; }
        public string IdentificacionAempresa { get; set; }
        public string NombreAempresa { get; set; }
        public string ApellidoAempresa { get; set; }
        public string EmailAempresa { get; set; }
        public string TelefonoAempresa { get; set; }
        public string CelularAempresa { get; set; }
        public string DireccionAempresa { get; set; }
        public string CargoAempresa { get; set; }
        public bool? ActivoAempresa { get; set; }
        public DateTime? FechaRegistroAempresa { get; set; }
    }
}