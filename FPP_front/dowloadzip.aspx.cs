using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
using FPP_front.DTOs;

namespace FPP_front
{
    public partial class dowloadzip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    string carpeta = Request.QueryString["folder"];
                    descargarArchivo(carpeta.Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            
        }
        private string onServerPath(string carpeta)
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
        public List<ArchivoZip> llenarlistaArchivos(string carpeta)
        {
            List<ArchivoZip> arc_ = new List<ArchivoZip>();
            DirectoryInfo di = new DirectoryInfo(onServerPath(carpeta));
            FileInfo[] files = di.GetFiles("*.pdf");
            foreach (var file in files)
            {
                byte[] archivo = System.IO.File.ReadAllBytes(file.FullName);
                var tamaño = archivo.Length;
                arc_.Add(new ArchivoZip
                {
                    nombre = file.Name,
                    tamaño = archivo
                }
            );
            }
            return arc_;
        }
        public void descargarArchivo(string carpeta)
        {
            List<ArchivoZip> files = llenarlistaArchivos(carpeta);
            byte[] fileBytes = null;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var f in files)
                    {
                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry(carpeta+"/"+f.nombre);
                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(f.tamaño))
                        {
                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
                fileBytes = memoryStream.ToArray();
            }
            Response.AddHeader("Content-Disposition", "attachment; filename="+carpeta+".zip");
            Response.ContentType = "application/zip";
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
        }
    }
}