using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class dowloadconvenios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string archivo = string.Empty;
                archivo = onServerPath() + "/" + Request.QueryString["id"].ToString();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + (Request.QueryString["id"].ToString()));
                Response.TransmitFile(archivo);
            }
            catch (Exception ex)
            {


            }
        }
        private string onServerPath()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path = string.Empty;

            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/convenios/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/convenios/");
            }


            return path;
        }
    }
}