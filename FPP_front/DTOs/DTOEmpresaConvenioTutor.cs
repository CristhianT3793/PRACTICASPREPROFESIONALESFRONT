using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOEmpresaConvenioTutor
    {
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
        public string EmailTutor { get; set; }
        
    }
}