using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOFppAlumno_Fpp_Pasante
    {
        public long IdfppAlumno { get; set; }
        public long? Idfpp { get; set; }
        public long? Idestudiante { get; set; }
        public string FpparchivourlAlumno { get; set; }
        public long? Idestadofpp { get; set; }
        public DateTime? Fechasubidaarchivo { get; set; }
        public string Descfpp { get; set; }
        public DateTime fechainiciofpp { get; set; }
        public DateTime fechafinfpp { get; set; }
        public string DescEstadofpp { get; set; }
        public string Desctipofpp { get; set; }
        
    }
}