using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class dowloadfilespasante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    string archivo = string.Empty;
                    string CarpetaExpediente = Request.QueryString["folder"].ToString();
                    archivo = onServerPath(CarpetaExpediente) + "/" + Request.QueryString["id"].ToString().Trim();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Request.QueryString["id"].ToString().Trim());
                    Response.TransmitFile(archivo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private string onServerPath(string carpeta)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path = string.Empty;

            if (host == "localhost")
            {
                path = Server.MapPath("/fppEstudiante/"+carpeta+"/");
            }
            else
            {

                path = Server.MapPath("/fppEstudiante/" + carpeta + "/");
            }


            return path;
        }
    }
}