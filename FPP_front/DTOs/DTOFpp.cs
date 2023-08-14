using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOFpp
    {
        public long Idfpp { get; set; }
        public long? Idfacultad { get; set; }
        public long? Idtipofpp { get; set; }
        public string Descfpp { get; set; }
        public string Codgrupo { get; set; }
        public DateTime fechainiciofpp { get; set; }
        public DateTime fechafinfpp { get; set; }
    }
}