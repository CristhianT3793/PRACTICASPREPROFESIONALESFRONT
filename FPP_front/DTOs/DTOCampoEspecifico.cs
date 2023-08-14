using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOCampoEspecifico
    {
        public int IdCampoEspecifico { get; set; }
        public int? IdCampoAmplio { get; set; }
        public string DescripcionCampoEspecifico { get; set; }
    }
}