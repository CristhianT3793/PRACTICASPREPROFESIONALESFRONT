using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOCampoAmplio_Especifico
    {
        public int IdCampoAmplio { get; set; }
        public string DescripcionCampoAmplio { get; set; }
        public int IdCampoEspecifico { get; set; }
        public string DescripcionCampoEspecifico { get; set; }
    }
}