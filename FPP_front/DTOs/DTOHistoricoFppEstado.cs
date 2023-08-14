using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOHistoricoFppEstado
    {
        public int IdHistoricoFpp { get; set; }
        public int? IdFppPasante { get; set; }
        public DateTime? FechaRegistroHisfpp { get; set; }
        public string ObservacionHisfpp { get; set; }
        public int IdEstadoHisfpp { get; set; }
        public string DescripcionEstadoFpp { get; set; }
    }
}