using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOConvenioEmpresaPasante
    {
        //tabla empresa_convenio_tutor
        public int IdEmpresaConvenio { get; set; }
        public int? IdConvenio { get; set; }
        public long? IdEmpresa { get; set; }
        public long? IdPasante { get; set; }
        public string IdentificacionTutor { get; set; }
        public string NombreTutor { get; set; }
        public string ApellidoTutor { get; set; }
        public string FacultadTutor { get; set; }
        public string CarreraTutor { get; set; }
        public string IdentificacionTutorEmpresa { get; set; }
        //tabla Empresa
        public string RucEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string TipoEmpresa { get; set; }
        public bool? HomologadaEmpresa { get; set; }
        //tabla Pasante
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
        public string EmailTutor { get; set; }
        public int? EstadoAprobado { get; set; }
        public int? EnviadoRegistro { get; set; }

    }
}