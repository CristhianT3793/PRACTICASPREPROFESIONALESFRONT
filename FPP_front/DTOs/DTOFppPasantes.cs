﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOFppPasantes //solo permite actualizar
    {
        public int IdFppPasante { get; set; }
        public long? IdPasante { get; set; }
        public long? IdEstadoFpp { get; set; }
        public long? IdAprobador { get; set; }
        public int? IdPlantilla { get; set; }
        public short? FppPasanteEstado { get; set; }
        public string FppPasantePath { get; set; }
        public DateTime? FppPasanteFechaSubida { get; set; }
        public string FppPasanteObservacion { get; set; }
        public bool? FppPasanteActivo { get; set; }

    }
}