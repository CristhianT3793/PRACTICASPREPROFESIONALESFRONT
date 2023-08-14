using FPP_front.ConexionServicios;
using FPP_front.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class fppPasantesCoordinador : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static List<DTOPlantillaFpp> plantillas = new List<DTOPlantillaFpp>();
        static List<DTOFppPasantePlantilla> fppPasante = new List<DTOFppPasantePlantilla>();

        static List<DTOFppPasantes> fppPasante_ = new List<DTOFppPasantes>();
        static int idpasante = -1;
        static int idfppAlumno = -1;
        static string tipoDocumento = "";
        static string codcarrera = "", carrera = "";
        static string cedula = "";     
        static string nombreFpp = "";
        static string CarpetaExpediente = "";
        static string pathfpp1_7 = "";
        //static string emailcoordinador = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idpasante = Convert.ToInt32(Request.QueryString["id"]);
                cedula = Request.QueryString["cedula"];
                //codcarrera = Request.QueryString["codcarrera"];
                carrera = Request.QueryString["carrera"];
                CarpetaExpediente = Request.QueryString["folder"];


                lblNombresAlumno.Text = Request.QueryString["nom"];
                lblIdentificacion.Text = cedula;
                lblcarrera.Text = carrera;
                lblFacultad.Text = Request.QueryString["facultad"];

                lblNumHoras.Text = Request.QueryString["horas"];
                lblFechaInicio.Text = Request.QueryString["finicio"];
                lblFechaFin.Text = Request.QueryString["ffinal"];

                //emailcoordinador = Request.QueryString["emailCoordinador"];

                cargarDatosTutorEmpresa(Request.QueryString["nomempresa"], Request.QueryString["cedulatutor"]);
                cargarDatosAreasEstudio(Convert.ToInt32(Request.QueryString["idcampoespecifico"]));
                //verificar plantilla
                catalogoFpp();
                //verificar fpp pasante
                FppPasante(idpasante);
                //crear fpp faltantes
                cargarFpps(idpasante,1);
                
            }
            cargarPasantiasEstudiante(cedula);

        }
        public async void cargarDatosAreasEstudio(int idcampoespecifico)
        {
            string uri = "CampoAmplio/searchCampoEspecifico/idcampoespecifico=" + idcampoespecifico;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            DTOCampoAmplio_Especifico campoamplio_especifico_ = new DTOCampoAmplio_Especifico();
            if (micro_getdatos != "error")
            {
                campoamplio_especifico_ = JsonConvert.DeserializeObject<DTOCampoAmplio_Especifico>(micro_getdatos);
                if (!string.IsNullOrEmpty(campoamplio_especifico_.DescripcionCampoEspecifico))
                {
                    lblCampoAmplio.Text = campoamplio_especifico_.DescripcionCampoAmplio;
                    lblCampoEspecifico.Text = campoamplio_especifico_.DescripcionCampoEspecifico;
                }
            }
        }
        public async void cargarDatosTutorEmpresa(string nomempresa, string idtutor)
        {
            string uri = "AutoridadEmpresa/searchTutor/idtutor=" + idtutor;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            DTOAutoridadEmpresa datosAutoridadEmpresa = new DTOAutoridadEmpresa();
            if (micro_getdatos != "error")
            {

                datosAutoridadEmpresa = JsonConvert.DeserializeObject<DTOAutoridadEmpresa>(micro_getdatos);
                if (!string.IsNullOrEmpty(datosAutoridadEmpresa.IdentificacionAempresa))
                {
                    lblnombreTutor.Text = datosAutoridadEmpresa.NombreAempresa + " " + datosAutoridadEmpresa.ApellidoAempresa;
                    lblnombreempresa.Text = nomempresa;
                    lbltutorempresa.Text = datosAutoridadEmpresa.IdentificacionAempresa;
                    lblcargotutor.Text = datosAutoridadEmpresa.CargoAempresa;
                }
            }

        }
        public async void cargarFpps(int idpasante,int pagina)
        {

            //string uri = "FppPasante/fppPasantes/" + idpasante;
            string uri = "FppPasantePlantilla/joinFppPasante/" + pagina + "/" + idpasante;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                fppPasante = JsonConvert.DeserializeObject<List<DTOFppPasantePlantilla>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = fppPasante.OrderBy(x=>x.NomrePlantilla).ToList();
                    dgvPasante.DataBind();

                }
            }
        }
        /*extrae todos los fpp creados al pasante */
        public async void FppPasante(int idpasante)
        {
            string uri = "FppPasante/fppPasantes/" + idpasante;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                fppPasante_ = JsonConvert.DeserializeObject<List<DTOFppPasantes>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {

                }
            }
        }
        public async void catalogoFpp()
        {
            string uri = "PlantillaFpp";

            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantillas = JsonConvert.DeserializeObject<List<DTOPlantillaFpp>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {

                }
            }
        }

        protected void dgvPasante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            if (e.CommandName == "subirFpp")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                nombreFpp= dgvPasante.DataKeys[fila].Values["NomrePlantilla"].ToString();
                tipoDocumento = dgvPasante.DataKeys[fila].Values["IdPlantilla"].ToString();
                idfppAlumno= Convert.ToInt32(dgvPasante.DataKeys[fila].Values["IdFppPasante"].ToString());
                if (dgvPasante.DataKeys[fila].Values["FppPasantePath"]!=null)
                {
                    pathfpp1_7 = dgvPasante.DataKeys[fila].Values["FppPasantePath"].ToString().Trim();
                }             
                ModalPopupExtender3.Show();
            }
            if (e.CommandName == "dowloadPlantilla")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                string path = dgvPasante.DataKeys[fila]["PathPlatilla"].ToString();
                Response.Redirect("dowloadfiles.aspx?id=" + path);
            }
            if (e.CommandName == "abrirModalHistorialFPP")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idfppAlumno = Convert.ToInt32(dgvPasante.DataKeys[fila]["IdFppPasante"].ToString());
                nombreFpp = dgvPasante.DataKeys[fila].Values["NomrePlantilla"].ToString();
                cargarHistorial(idfppAlumno, 1);
                lblHistorialFpp.Text = "Seguimiento: " + nombreFpp;
                ModalHistorialFPP.Show();
            }
        }
        public async void cargarHistorial(int idFpp,int pagina) {

            
            string uri = "HistoricoFpp/Hitoricobyfpp/page/"+pagina+ "/idfpp="+idFpp;
            List<DTOHistoricoFppEstado> historico_ = new List<DTOHistoricoFppEstado>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            historico_.Clear();
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                historico_ = JsonConvert.DeserializeObject<List<DTOHistoricoFppEstado>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                   
                    dgvHistorico.VirtualItemCount = Convert.ToInt32(total);
                    dgvHistorico.DataSource = historico_.ToList();
                    dgvHistorico.DataBind();
                }
                else
                {                  
                    dgvHistorico.DataSource = historico_.ToList();
                    dgvHistorico.DataBind();
                }
            }
        }
        protected void dgvPasante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //1 creado
            //2 subido
            //3 aprobado
            //4 devuelto
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int estado = Convert.ToInt32(dgvPasante.DataKeys[e.Row.RowIndex]["IdEstadoFpp"].ToString());
                LinkButton btnSubirArchivo = (LinkButton)e.Row.FindControl("subirFpp");
                if (estado == 1 || estado==2)
                {
                    btnSubirArchivo.Visible = true;
                }else if (estado == 4)
                {
                    btnSubirArchivo.Visible = true;
                    e.Row.BackColor = System.Drawing.Color.FromArgb(255, 164, 22);
                }
                else if(estado==3)
                {
                    btnSubirArchivo.Visible = false;
                    e.Row.BackColor = System.Drawing.Color.FromArgb(68, 215, 84);
                }
            }
        }
        public async Task<bool> updatePathFpp(DTOFppPasantes dto, string url)
        {
            bool correcto = await con.GenericPut(dto, url);
            return correcto;
        }
        public string rutaArchivos()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
            //crear carpeta si no existe
            string folderPath = Server.MapPath("/documentosPPP/fppEstudiante/" + CarpetaExpediente + "/");
            if (host == "localhost")
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine(folderPath);

                }
            }
            else
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine(folderPath);

                }
            }

            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/fppEstudiante/" + CarpetaExpediente + "/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/fppEstudiante/" + CarpetaExpediente + "/");
            }
            return path;
        }

        public string rutaArchivosFPP1_7()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
            //crear carpeta si no existe
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

        public bool validarArchivo(string nombreArchivo)//recibe el path del archivo subido
        {
            FileUpload file_ = fileFpp;
            string archivo = "";
            bool existe = false;
            if (file_.HasFile && file_.Enabled)
            {
                if (fileFpp.PostedFile.ContentType == "application/pdf")
                {
                    if (fileFpp.FileBytes.Length <= 2048 * 1024)
                    {
                        if (nombreFpp.Trim() == "FPP1" || nombreFpp.Trim() == "FPP4" )//guarda el FPP1 2 y 7 en la carpeta de FPP por carrera
                        {
                            archivo = rutaArchivosFPP1_7() + nombreArchivo;
                            if (File.Exists(archivo))
                                System.IO.File.Delete(archivo);
                            file_.PostedFile.SaveAs(archivo);
                            existe = true;
                        }
                        else
                        {
                            archivo = rutaArchivos() + nombreArchivo;
                            if (File.Exists(archivo))
                                System.IO.File.Delete(archivo);

                            file_.PostedFile.SaveAs(archivo);
                            existe = true;
                        }

                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "script", "alertaParametro('info','El documento debe ser en formato pdf');", true);
                }
            }
            else
            {
                existe = false;
            }
            return existe;
        }



        protected async void btnSubirFpp_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = Conexion.BuscarPracticas_ds("PASANTE", "ACTIVO_PASANTE", " where ID_PASANTE=" + idpasante);
            bool enviado = Convert.ToBoolean(ds.Tables[0].Rows[0]["ACTIVO_PASANTE"]);
            string url = "FppPasante/" + idfppAlumno;

            FileUpload file_ = fileFpp;
            string nombreArchivo;
            if (file_.HasFile && file_.Enabled)
            {
                string strFileNameWithPath = file_.PostedFile.FileName;
                nombreArchivo = "SEK_PPP_"+lblcarrera.Text.Trim()+"_"+lblIdentificacion.Text.Trim()+"_"+nombreFpp.Trim()+"_"+ idpasante + System.IO.Path.GetExtension(strFileNameWithPath);//tipodocumento es idplantilla se debe cambiar
                if (nombreFpp.Trim() == "FPP1" || nombreFpp.Trim() == "FPP4" )//validar que sea FPP1 o FPP7
                {
                    //si es FPP1 o FPP7
                    //se queda con el nombre del archivo que tiene
                    nombreArchivo = pathfpp1_7;
                }
                if (validarArchivo(nombreArchivo))//valida que el documento se haya guardado correctamente
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "script", "console.log('entro en 1');", true);
                    DTOFppPasantes dtoFppPasante = new DTOFppPasantes
                    {
                        IdPasante = -1,
                        IdAprobador = -1,
                        IdPlantilla = -1,
                        IdEstadoFpp = 2,
                        FppPasanteFechaSubida=DateTime.Now,
                        FppPasantePath = nombreArchivo,
                    };
                    bool correcto = await updatePathFpp(dtoFppPasante, url);
                    if (correcto)
                    {

                        if (enviado)
                        {
                            string descripcionNotificacion = nombreFpp.Trim() + ", SUBIDO ALUMNO " + lblNombresAlumno.Text;
                            bool insertarNotificacion = Conexion.InsertarPracticas("notificacion_practicas", "DESCRIPCIONNOTIFICACION,FECHAREGISTRONOTIFICACION,ACTIVONOTIFICACION", "'" + descripcionNotificacion + "','" + DateTime.Now + "'," + 1);
                        }
                        //cargarFpps(idpasante, 1);
                        Server.TransferRequest(Request.Url.AbsolutePath, false);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "script", "alertaParametro('error','No se pudo subir el documento');", true);
                    }
                }
            }
            
            //cargarFpps(idpasante, 1);
        }
        protected async void btnEnviarExpediente_Click(object sender, EventArgs e)
        {
            if (verificarTodosDocumentos())
            {
                DTOPasante dtopasante = new DTOPasante();
                string url = "Pasante/" + idpasante;
                dtopasante.ActivoPasante = true;
                bool correcto = await updateEstadoEnviado(dtopasante, url);
                if (correcto)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('success','Expediente enviado correctamente'); ", true);
                    //insertar notificacion
                    string descripcionNotificacion = "EXPEDIENTE ENVIADO DEL ESTUDIANTE " + lblNombresAlumno.Text.Trim();
                    string fechanotificacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    bool insertarNotificacion = Conexion.InsertarPracticas("notificacion_practicas", "DESCRIPCIONNOTIFICACION,FECHAREGISTRONOTIFICACION,ACTIVONOTIFICACION", "'" + descripcionNotificacion + "','" + fechanotificacion + "'," + 1);
                    //insertar en historial
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Expediente no se pudo enviar'); ", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se puede enviar Expediente falta subir documentos'); ", true);
            }

        }
        /// <summary>
        /// revisa que el path del fpp no este vacio y que exista el FPP1 al FPP7 
        /// </summary>
        /// <returns>retorna true si tiene todo y false si le falta algo</returns>
        public bool verificarTodosDocumentos()
        {
            int contador = 0;
            foreach (GridViewRow row in dgvPasante.Rows)
            {              
                if (dgvPasante.DataKeys[row.RowIndex].Values["FppPasantePath"] != null)
                {
                    contador++;
                }
            }
            if (contador == 7 || contador == 4 || contador == 5)
            {
                return true;
            }
            else
                return false;

                
        }



        protected void dgvHistorico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvHistorico.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex;
            cargarHistorial(idfppAlumno,page);
        }

        public async Task<bool> updateEstadoEnviado(DTOPasante dto, string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;

        }
        public void cargarPasantiasEstudiante(string identificacion)
        {

            int totalHoras = 0;
            DataSet ds_pasantias = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join EMPRESA_CONVENIO_TUTOR t2 on t1.ID_PASANTE = t2.ID_PASANTE inner join EMPRESA t3 on t2.ID_EMPRESA=t3.ID_EMPRESA ", " t3.NOMBRE_EMPRESA,t1.IDENTIFICACION_PASANTE,rtrim(t1.APELLIDO_PASANTE)+' '+t1.NOMBRE_PASANTE NOMBRES,t1.FECHA_INICIO_PASANTE,t1.FECHA_FIN_PASANTE,t1.NUMERO_HORAS_PASANTE,CASE t1.ESTADO_APROBADO WHEN 1 THEN 'EN PROCESO DE APROBACIÓN' WHEN 2 THEN 'APROBADA' END AS ESTADO", " where t1.IDENTIFICACION_PASANTE='" + identificacion + "'");
            TableHeaderRow RowTableHead = new TableHeaderRow();
            RowTableHead.TableSection = TableRowSection.TableHeader;
            string[] encabezados = new string[] { "#", "Nombre Empresa", "Fecha Inicio", "Fecha Fin", "N° Horas", "Estado" };
            if (ds_pasantias.Tables[0].Rows.Count > 0)
            {
                //crear tabla 
                System.Web.UI.WebControls.Table tblpasantias = new System.Web.UI.WebControls.Table();
                foreach (var y in encabezados)
                {

                    TableHeaderCell CellHead = new TableHeaderCell
                    {
                        ForeColor = System.Drawing.Color.White,
                        BackColor = System.Drawing.Color.FromArgb(8, 83, 148),

                        Text = "<b><center>" + y + "</center></b>"

                    };
                    RowTableHead.Cells.Add(CellHead);
                }
                grd_Valores.Rows.Add(RowTableHead);
                for (int i = 0; i < ds_pasantias.Tables[0].Rows.Count; i++)
                {
                    TableRow RowTable = new TableRow();
                    TableCell Cell_Num = new TableCell
                    {

                        HorizontalAlign = HorizontalAlign.Center,
                        Text = (i + 1).ToString()
                    };
                    TableCell Cell_nombreEmpresa = new TableCell
                    {

                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["NOMBRE_EMPRESA"].ToString()
                    };
                    TableCell Cell_FechaInicio = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["FECHA_INICIO_PASANTE"].ToString()
                    };
                    TableCell Cell_FechaFin = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["FECHA_FIN_PASANTE"].ToString()
                    };
                    TableCell Cell_NumeroHoras = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["NUMERO_HORAS_PASANTE"].ToString()
                    };
                    TableCell Cell_Estado = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["ESTADO"].ToString()
                    };

                    RowTable.Cells.Add(Cell_Num);
                    RowTable.Cells.Add(Cell_nombreEmpresa);
                    RowTable.Cells.Add(Cell_FechaInicio);
                    RowTable.Cells.Add(Cell_FechaFin);
                    RowTable.Cells.Add(Cell_NumeroHoras);
                    RowTable.Cells.Add(Cell_Estado);
                    grd_Valores.Rows.Add(RowTable);
                    totalHoras += Convert.ToInt32(ds_pasantias.Tables[0].Rows[i]["NUMERO_HORAS_PASANTE"]);
                }
                TableFooterRow tblfooter = new TableFooterRow();
                TableCell Cell_TotalH1 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalH2 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalH3 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalHoras = new TableCell
                {
                    ForeColor = System.Drawing.Color.White,
                    Text = "<b>Total de Horas</b>",
                    BackColor = System.Drawing.Color.FromArgb(235, 162, 25),
                    HorizontalAlign = HorizontalAlign.Center
                };
                TableCell Cell_TotalHorasP = new TableCell
                {

                    ForeColor = System.Drawing.Color.White,
                    Text = "<b>" + totalHoras.ToString() + "</b>",
                    BackColor = System.Drawing.Color.FromArgb(235, 162, 25),
                    HorizontalAlign = HorizontalAlign.Center
                };

                tblfooter.Cells.Add(Cell_TotalH1);
                tblfooter.Cells.Add(Cell_TotalH2);
                tblfooter.Cells.Add(Cell_TotalH3);
                tblfooter.Cells.Add(Cell_TotalHoras);
                tblfooter.Cells.Add(Cell_TotalHorasP);
                grd_Valores.Rows.Add(tblfooter);

            }
        }


    }
}