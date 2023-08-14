using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOEstructuraAcademicaParametro
    {
        public int IdModalidad { get; set; }
        public string Facultad { get; set; }
        public string Carrera { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
        //public string Modalidad { get; set; }
        //public string PeriodoPasante { get; set; }
        //public int? IdProfundidad { get; set; }
        //Parametro
        public int IdParametro { get; set; }
        public DateTime? FechaInicioParametro { get; set; }
        public DateTime? FechaFinParametro { get; set; }
        public bool? ActivoParametro { get; set; }
        public int? MaxHorasParametro { get; set; }
        //public int? Profundidad { get; set; }
        public string NombreFacultad { get; set; }
        public string NombreCarrera { get; set; }

    }
}