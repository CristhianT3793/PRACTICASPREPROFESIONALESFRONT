using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPP_front.ConexionServicios
{
    public class conexionServicios
    {
        public string url { get; set; }
        public conexionServicios()
        {
            this.url = "http://localhost:9002/";//local
        }
    }
}