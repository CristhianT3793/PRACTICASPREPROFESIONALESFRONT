using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOEmpresa
    {
        public long IdEmpresa { get; set; }
        public bool? ActivoEmpresa { get; set; }
        public string CodEmpresa { get; set; }
        public string RucEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string TipoEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public string Telefono1Empresa { get; set; }
        public string Telefono2Empresa { get; set; }
        public string EmailEmpresa { get; set; }
        public DateTime? FechafirmaEmpresa { get; set; }
        public string ObjetivoEmpresa { get; set; }
        public string ObservacionEmpresa { get; set; }
        public DateTime? FechaRegistroEmpresa { get; set; }
        public bool? HomologadaEmpresa { get; set; }
    }
}