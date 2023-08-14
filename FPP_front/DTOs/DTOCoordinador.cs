using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOCoordinador
    {
        public int Idcoordinador { get; set; }
        public string Identificacioncoordinador { get; set; }
        public string Nombrecoordinador { get; set; }
        public string Apellidocoordinador { get; set; }
        public string Facultadcoordinador { get; set; }
        public string Carreracoordinador { get; set; }
        public bool? Activocoordinador { get; set; }
        public string Periodocoordinador { get; set; }

    }
}