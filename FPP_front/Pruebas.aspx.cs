using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class Pruebas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            llenarempresas();
        }
        public void llenarempresas()
        {
            DataSet ds_empresas = Conexion.BuscarPracticas_ds("EMPRESA", "*", "where ACTIVO_EMPRESA=1");
            ddlempresa_.DataSource = ds_empresas.Tables[0];
            ddlempresa_.DataValueField = "ID_EMPRESA";
            ddlempresa_.DataTextField = "NOMBRE_EMPRESA";
            ddlempresa_.DataBind();
        }
    }
}