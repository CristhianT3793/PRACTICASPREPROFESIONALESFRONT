using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPP_front.DTOs;
using Newtonsoft.Json;
using FPP_front.ConexionServicios;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace FPP_front
{
    public partial class pasantesCopia : System.Web.UI.Page
    {
        static conexionServicios cs = new conexionServicios();
        string url = cs.url.ToString();
        static long idpasante = -1;
        static long idestadofpp = -1;
        static long idfppAlumno = -1;
        static string identificacion = "";
        static string nameFile = "";
        static string nameFileDowload = "";
        static DropDownList ddlEstadosFpp;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                cargargridPasante();
            }
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(upModal);
        }
        public async void cargargridPasante()
        {
            List<DTOPasante> pasante_ = new List<DTOPasante>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await getpasante(1);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                pasante_ = JsonConvert.DeserializeObject<List<DTOPasante>>(items.ToString());

                if (Convert.ToBoolean(hasitems))
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = pasante_;
                    dgvPasante.DataBind();
                }
            }
        }

        private string OnServerPath()
        {
            //VERSION DE PRUEBA
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
            if (host == "localhost")
                path = Server.MapPath("/documentos/");
            else
            {
                path = Server.MapPath("/");
                path = path.Replace("PPP_UISEK\\", "\\PPP_UISEK\\documentos\\");
                //verificar linea 37 cuando vaya a produccion es la carga de documentos
            }
            return path;
        }
        private string subirFPP(string tipoFpp, string identificacion,FileUpload file_)
        {

            string exito = "-1";
            List<string> documentos = new List<string>();
            FileUpload file = file_;
            try
            {
                string strExtensionName = string.Empty;
                if (file.HasFile && file.Enabled)
                {

                    if (file.PostedFile.ContentType == "application/pdf" || file.PostedFile.ContentType == "image /jpg" || file.PostedFile.ContentType == "image/png")
                    {

                        if (file.FileBytes.Length <= 2048 * 1024)
                        {

                            string archivo = string.Empty;
                            try
                            {
                                string strFileNameWithPath = file.PostedFile.FileName;
                                nameFile = tipoFpp + "_" + identificacion + System.IO.Path.GetExtension(strFileNameWithPath);
                                // get the extension name of the file
                                strExtensionName = nameFile;
                                string path = OnServerPath() + "\\";
                                //aqui se arma el path del archivo                                
                                archivo = path + strExtensionName;
                                documentos.Add(archivo);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                if (File.Exists(archivo))
                                    System.IO.File.Delete(archivo);
                                file.PostedFile.SaveAs(archivo);
                                //revisar aqui captura excepcion cuando pasa a pruebas
                                exito = "1";
                            }
                            catch (Exception ex)
                            {
                                string script3 = "<script type='text/javascript'>console.log('" + ex.Message + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "script", script3, false);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alert('Solo se aceptan archivos tipo PDF, jpg, png y jpeg de hasta 2 mb.'); ", true);
                            return "-1";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alert('Solo se aceptan archivos tipo PDF, jpg, png y jpeg de hasta 2 mb.'); ", true);
                        return "-1";
                    }
                }
                else
                {
                    exito = "1";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alert('No se pudo ingresar la información enviada. Inténtelo más tarde o comuníquese con el Administrador'); console.log('" + ex.Message.Replace("'", "**") + "');", true);
                exito = "-1";
            }
            return exito;
        }
        public async Task<string> getpasante(int page)
        {
            string error = "error";
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("Pasante/page/" + page);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    return empResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return error;
        }

        public async Task<string> getFppsbyAlumno(long id, int page)
        {
            string error = "error";
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("FppAlumno/joinFppAlumnoFppTipoFpp/" + id + "/" + page);
                if (res.IsSuccessStatusCode)
                {
                    
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    return empResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return error;
        }
        public async void cargargridfppsAlumno(long idPasante)
        {
            List<DTOFppAlumno_Fpp_Pasante> fppsAlumno_ = new List<DTOFppAlumno_Fpp_Pasante>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await getFppsbyAlumno(idPasante, 1);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                fppsAlumno_ = JsonConvert.DeserializeObject<List<DTOFppAlumno_Fpp_Pasante>>(items.ToString());

                if (Convert.ToBoolean(hasitems))
                {

                    dgvFppsAlumno.VirtualItemCount = Convert.ToInt32(total);
                    dgvFppsAlumno.DataSource = fppsAlumno_;
                    dgvFppsAlumno.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script language='javascript'>");
                    sb.Append(@"$('#myModal').modal('show');");
                    sb.Append(@"</script>");

                    ClientScript.RegisterStartupScript(this.GetType(), "JSScript", sb.ToString());
                    //btnPopUp_ModalPopupExtender.Show();
                }
            }
        }
        protected  void dgvPasante_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "fppAlumno")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idpasante = Convert.ToInt64(dgvPasante.DataKeys[fila].Values["Idestudiante"].ToString());
                identificacion = dgvPasante.DataKeys[fila].Values["IdentificacionPasante"].ToString();

                cargargridfppsAlumno(idpasante);
         
                //actualizar url de fpp servicio que reciba id_fppAlumno,idestudiante
                UpdateFppAlumno(idpasante, nameFile);
            }
        }
        public void abrirModal(string identificaion, long idPasante)
        {
            
        }
        protected void btnFpp1_Click(object sender, EventArgs e)
        {

        }
        protected void btnDowloadFpp1_Click(object sender, EventArgs e)
        {
            //devuelde el nombre del archivo con parametros 
            //enviar parametro nombre del FPP1
            //id del estudiante
        }
        public async Task<string> ServicioExtraerEstadosFpp()
        {
            string error = "error";
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("EstadoFpp");
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    return empResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return error;
        }
        public async Task<string> ServicioExtraerFppAlumnoById(long idfpp)
        {
            string error = "error";
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("FppAlumno/"+idfpp);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    return empResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return error;
        }
        public async void UpdateFppAlumno(long idfpp, string nombreArchvo)
        {

            DTOFppPasantes fppAluno = new DTOFppPasantes();
            //fppAluno.FpparchivourlAlumno = nombreArchvo;
            //fppAluno.Fechasubidaarchivo = DateTime.Now.Date;
            try
            {
                string uri = "FppAlumno/" + idfpp;
                var myContent = JsonConvert.SerializeObject(fppAluno);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
                HttpResponseMessage res = await client.PutAsync(uri, stringContent);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async void UpdateEstadoFppAlumno(long idfpp, long estado)
        {

            DTOFppPasantes fppAluno = new DTOFppPasantes();
            //fppAluno.Idestadofpp = estado;
            try
            {
                string uri = "FppAlumno/" + idfpp;
                var myContent = JsonConvert.SerializeObject(fppAluno);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
                HttpResponseMessage res = await client.PutAsync(uri, stringContent);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void dgvFppsAlumno_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected async void dgvFppsAlumno_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<DTOEstadosFpp> estadosfpp = new List<DTOEstadosFpp>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await ServicioExtraerEstadosFpp();
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                //estadosfpp = JsonConvert.DeserializeObject<List<DTOEstadosFpp>>(items.ToString()).Where(x=>x.Idestadofpp!=3).ToList();
                estadosfpp = JsonConvert.DeserializeObject<List<DTOEstadosFpp>>(items.ToString());
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ddlEstadosFpp = (e.Row.FindControl("ddlEstadosFpp") as DropDownList);
                ddlEstadosFpp.Items.Insert(0, new ListItem("--selecione una opción--"));
                ddlEstadosFpp.AppendDataBoundItems = true;
                ddlEstadosFpp.DataSource = estadosfpp;
                ddlEstadosFpp.DataTextField = "Descestadofpp";
                ddlEstadosFpp.DataValueField = "Idestadofpp";
                ddlEstadosFpp.DataBind();
            }
        }

        protected async void dgvFppsAlumno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int fila = Convert.ToInt32(e.CommandArgument);
            
            List<DTOFppPasantes> fppAlumno = new List<DTOFppPasantes>();
            if (e.CommandName.Equals("UploadFpp"))
            {
                string tipofpp = dgvFppsAlumno.DataKeys[fila].Values["Desctipofpp"].ToString();
                idfppAlumno = Convert.ToInt64(dgvFppsAlumno.DataKeys[fila].Values["IdfppAlumno"].ToString());
                string strExtensionName = string.Empty;
                FileUpload file = dgvFppsAlumno.Rows[fila].FindControl("archivo") as FileUpload;
                string strPath = Path.GetFileName(file.PostedFile.FileName);
                string exito = subirFPP(tipofpp.Trim(), identificacion.Trim(), file);
                if (exito == "1")
                {
                    UpdateFppAlumno(idfppAlumno, nameFile);
                }
            }
            else if (e.CommandName.Equals("dowloadfppAlumno"))
            {
                idfppAlumno = Convert.ToInt64(dgvFppsAlumno.DataKeys[fila].Values["IdfppAlumno"].ToString());
                //servicio para extraer id de fpp
                string items = await ServicioExtraerFppAlumnoById(idfppAlumno);
                dynamic jsonObj = JsonConvert.DeserializeObject(items);
                nameFileDowload = jsonObj["fpparchivourlAlumno"].ToString().Trim();
                dowloadFpp(nameFileDowload);
            }
            else if (e.CommandName== "Enviar") {

                DropDownList ddlestado=dgvFppsAlumno.Rows[fila].FindControl("ddlEstadosFpp") as DropDownList;
                 
                if (Convert.ToInt32(ddlestado.SelectedValue) == 1)
                {
                    //ClientScript.RegisterStartupScript(typeof(Page), "script", "alert('entro en 1');", true);

                }else if(Convert.ToInt32(ddlestado.SelectedValue) == 2)
                {

                    idfppAlumno = Convert.ToInt64(dgvFppsAlumno.DataKeys[fila].Values["IdfppAlumno"].ToString());
                    idestadofpp = Convert.ToInt64(ddlestado.SelectedValue);
                    ModalPopupExtender3.Show();
                    //ClientScript.RegisterStartupScript(typeof(Page), "script", "alert('entro en 2');", true);
                }            
            }
        }
        public void dowloadFpp(string nombreArchivo)
        {
            string archivo = string.Empty;
            archivo = onServerPath("documentos");

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
            Response.TransmitFile(archivo+"/"+nombreArchivo);

        }
        private string onServerPath(string folder)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path = string.Empty;
            if (host == "localhost")
            {
                path = Server.MapPath("/" + folder + "/");
            }
            else
            {
                path = Server.MapPath("../documentos/" + folder + "/");
            }
            return path;
        }
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            
            
        }
        protected void dgvFppsAlumno_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        protected void dgvFppsAlumno_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvFppsAlumno.EditIndex = e.NewEditIndex;
        }
        protected void btnSubirArchivo_Click(object sender, EventArgs e)
        {

        }

        protected void btnEnviarRechazo_Click(object sender, EventArgs e)
        {
            //servicioactualizarestado a rechazado
            UpdateEstadoFppAlumno(idfppAlumno, idestadofpp);
        }
    }
}