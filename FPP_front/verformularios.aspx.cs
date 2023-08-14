using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace FPP_front
{
    public partial class verformularios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //recibe tipo de fpp si es fpp1 o fpp7
            string tipofpp = Request.QueryString["tipofpp"].ToString().Trim();//OBTIENE EL TIPO DE FPP EJ: FPP1
            string carpeta = Request.QueryString["carpeta"].ToString().Trim();//OBTIENE EL NOMBRE DE LA CARPETA EN LA QUE SE GUARDAN LOS FPP1 DEL ALUMNO
            string path = Request.QueryString["id"].ToString().Trim();//OBTIENE EL PATH DE EL ARCHIVO

            string archivo = string.Empty;
            if(tipofpp=="FPP1" || tipofpp == "FPP7" || tipofpp=="FPP4")//ESTOS TRES FPP SE ENCEUNTRAN GUARDADOS EN LA CARPETA fppCarrera
            {
                try
                {
                    archivo = onServerPathCoordinador() + "/" + path;
                    if (ExisteArchivo(archivo))
                    {
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "inline; filename=" + path);
                        Response.TransmitFile(archivo);
                    }
                    else
                    {
                        archivo = onServerPathEstudiante(carpeta) + "/" + path;
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "inline; filename=" + path);
                        Response.TransmitFile(archivo);
                    }
                }
                catch(IOException ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo abrir el documento.'); ", true);

                }


            }
            else
            {
                try
                {
                    archivo = onServerPathEstudiante(carpeta) + "/" + path;
                    if (ExisteArchivo(archivo))
                    {
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "inline; filename=" + path);
                        Response.TransmitFile(archivo);
                    }
                    else
                    {
                        archivo = onServerPathCoordinador() + "/" + path;
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "inline; filename=" + path);
                        Response.TransmitFile(archivo);
                    }
                }
                catch (IOException ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo abrir el documento.'); ", true);

                }




            }
        }
        public bool ExisteArchivo(string url)
        {
            bool result = true;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }



        //ver formularios cordinador
        private string onServerPathCoordinador()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path = string.Empty;
            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/fppCarrera/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/fppCarrera/");
            }
            return path;
        }
        //ver formularios alumno
        private string onServerPathEstudiante(string carpeta)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path = string.Empty;
            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/fppEstudiante/" + carpeta + "/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/fppEstudiante/" + carpeta + "/");
            }
            return path;
        }
    }
}