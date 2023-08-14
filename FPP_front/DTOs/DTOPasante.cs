using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOPasante
    {
        public long IdPasante { get; set; }
        public int? IdCampoEspecifico { get; set; }
        public string IdentificacionPasante { get; set; }
        public string NombrePasante { get; set; }
        public string ApellidoPasante { get; set; }
        public DateTime? FechaRegistroPasante { get; set; }
        public int? NumeroHorasPasante { get; set; }
        public DateTime? FechaInicioPasante { get; set; }
        public DateTime? FechaFinPasante { get; set; }
        public string CarreraPasante { get; set; }
        public bool? ActivoPasante { get; set; }
        public string FacultadPasante { get; set; }
        public string PeriodoPasante { get; set; }
        public string CarpetaPasanteExpediente { get; set; }
        public string CodCarreraPasante { get; set; }
        public string CodPeriodoPasante { get; set; }
        public string CodFacultadPasante { get; set; }
        public int EstadoAprobado { get; set; }
    }
}