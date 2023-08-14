using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOFppCoordinador
    {
        public int IdFppCoordinador { get; set; }
        public int? Idcoordinador { get; set; }
        public int? IdPlantilla { get; set; }
        public DateTime? FechaRegistroFpp { get; set; }
        public string PathFppCoordinador { get; set; }
        public string NomPlatillaCordinador { get; set; }
        public string CarreraFppCoordinador { get; set; }
        public string SemestreFppCoordinador { get; set; }

    }
}