using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class desactivarnotificacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                int idnotificacion = Convert.ToInt32(Request.QueryString["id"]);
                desactivarNotificacion(idnotificacion);
            //}
        }
        public void desactivarNotificacion(int idNotificacion)
        {

        }
    }
}