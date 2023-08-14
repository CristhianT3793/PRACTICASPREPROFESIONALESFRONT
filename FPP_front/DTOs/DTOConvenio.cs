using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOConvenio
    {
        public int IdConvenio { get; set; }
        public string NombreConvenio { get; set; }
        public bool? ActivoConvenio { get; set; }
        public string DescripcionConvenio { get; set; }
        public DateTime? FechaInicioConvenio { get; set; }
        public DateTime? FechaFinConvenio { get; set; }
        public string PathConvenio { get; set; }
        public string EmpresaConvenio { get; set; }
        public long? IdEmpresa { get; set; }
    }
}