using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOFppxAlumno
    {
        public long IdfppAlumno { get; set; }
        public long? Idfpp { get; set; }
        public long? Idestudiante { get; set; }
        public long? Idaprobador { get; set; }
        public long? Idestadofpp { get; set; }
        public short? FppestadoAlumno { get; set; }
        public string FpparchivourlAlumno { get; set; }
        public DateTime? Fechasubidaarchivo { get; set; }
        public string ObservacionfppAlumno { get; set; }
    }
}