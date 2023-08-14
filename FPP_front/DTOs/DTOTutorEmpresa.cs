using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOTutorEmpresa
    {
        public long Idtutorempresa { get; set; }
        public long? IdEmpresa { get; set; }
        public string Identificacion { get; set; }
        public string NombreTempresa { get; set; }
        public string ApellidoTempresa { get; set; }
        public string EmailTempresa { get; set; }
        public int? TelefonoTempresa { get; set; }
        public int? CelularTempresa { get; set; }
        public string DireccionTempresa { get; set; }
        public string CargoTempresa { get; set; }
    }
}