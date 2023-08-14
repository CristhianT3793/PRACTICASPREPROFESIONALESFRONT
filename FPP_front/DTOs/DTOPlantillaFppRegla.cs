﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.DTOs
{
    public class DTOPlantillaFppRegla
    {
        //plantilla
        public int IdPlantilla { get; set; }
        public int? IdParametro { get; set; }
        public string NomrePlantilla { get; set; }
        public string DescripcionPlantilla { get; set; }
        public bool? ActivoPlantilla { get; set; }
        public string PathPlatilla { get; set; }
        //parametro
        public int? IdModalidad { get; set; }
        public int? MaxHorasParametro { get; set; }
        //estructura academica
        public string NombreFacultad { get; set; }
        public string NombreCarrera { get; set; }
    }
}