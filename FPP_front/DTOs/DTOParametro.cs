using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOParametro
    {
        public int IdParametro { get; set; }
        public int? IdModalidad { get; set; }
        public DateTime? FechaInicioParametro { get; set; }
        public DateTime? FechaFinParametro { get; set; }
        public bool? ActivoParametro { get; set; }
        public int? MaxHorasParametro { get; set; }
        public int? Profundidad { get; set; }
    }
}