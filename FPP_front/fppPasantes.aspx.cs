using FPP_front.ConexionServicios;
using FPP_front.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class fppPasantes : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static List<DTOPlantillaFpp> plantillas = new List<DTOPlantillaFpp>();
        static List<DTOFppPasantePlantilla> fppPasante = new List<DTOFppPasantePlantilla>();
        static List<DTOFppPasantes> fppPasante_ = new List<DTOFppPasantes>();
        static int idpasante = -1;
        static DropDownList ddlEstadosFpp;
        static int idfppAlumno = -1;
        static int idestadofpp=-1;
        static string cedula = "";
        static string carrera="";
        static string CarpetaExpediente = "";
        static string descripcionFpp = "";
        static string emailcoordinador = ""; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idpasante = Convert.ToInt32(Request.QueryString["id"]);
                cedula =Request.QueryString["cedula"];
                //codcarrera= Request.QueryString["codcarrera"];
                carrera= Request.QueryString["carrera"];
                CarpetaExpediente = Request.QueryString["folder"];


                lblNombresAlumno.Text = Request.QueryString["nom"];
                lblIdentificacion.Text = cedula;
                lblcarrera.Text = carrera;
                lblFacultad.Text = Request.QueryString["facultad"];

                lblNumHoras.Text = Request.QueryString["horas"];
                lblFechaInicio.Text= Request.QueryString["finicio"];
                lblFechaFin.Text= Request.QueryString["ffinal"];

                emailcoordinador= Request.QueryString["emailCoordinador"];

                cargarDatosTutorEmpresa(Request.QueryString["nomempresa"], Request.QueryString["cedulatutor"]);
                cargarDatosAreasEstudio(Convert.ToInt32(Request.QueryString["idcampoespecifico"]));              
                ////verificar plantilla
                //catalogoFpp();
                //verificar fpp pasante
                //FppPasante(idpasante);
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
        public async void cargarDatosTutorEmpresa(string nomempresa,string idtutor)
        {
            string uri = "AutoridadEmpresa/searchTutor/idtutor="+ idtutor;
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
        public void verificarFppFaltante(int idpasante)
        {
            List<int> fppFaltante = new List<int>();
            foreach (var plantilla in plantillas)
            {
                var check = Array.Exists(fppPasante.ToArray(), x => x.IdPlantilla == plantilla.IdPlantilla);
                if (!check)
                {
                    fppFaltante.Add(plantilla.IdPlantilla);
                }
            }
            crearlineafpp(fppFaltante);
        }
        public void crearlineafpp(List<int> platillaFaltante)
        {
            

            foreach (var idplantilla in platillaFaltante)
            {
                //llamar servicio de insercion
                DTOFppPasantes objectFppPasante = new DTOFppPasantes();
                objectFppPasante.IdPasante = idpasante;
                objectFppPasante.IdEstadoFpp = 1;//1 significa creado
                objectFppPasante.FppPasantePath = null;
                objectFppPasante.FppPasanteFechaSubida = null;
                objectFppPasante.FppPasanteObservacion = null;
                objectFppPasante.FppPasanteActivo = true;
                objectFppPasante.IdPlantilla = idplantilla;
                objectFppPasante.IdAprobador = 1;//cambiar
                insertarFpp(objectFppPasante);
            }

        }
        /// <summary>
        /// Extrae el path del archivo del fpp1,fpp2 y fpp7 del coordinador para actualizar los fpp1,fpp2 y fpp7 del estudiante
        /// </summary>
        /// <returns></returns>
        public async Task<List<DTOFppCoordinador>> fppCoordinador()
        {
            string[] fpps =  { "FPP1","FPP4" };
            List<DTOFppCoordinador> fppCoordinador_ = new List<DTOFppCoordinador>();
            string uri = "FppCoordinador";
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                fppCoordinador_ = JsonConvert.DeserializeObject<List<DTOFppCoordinador>>(items.ToString()).Where((n) => fpps.Contains(n.NomPlatillaCordinador.Trim().ToString())).ToList();
                if (Convert.ToBoolean(hasitems))
                {
                    return fppCoordinador_;
                }
                else
                    return fppCoordinador_;
            }
            return fppCoordinador_;
        }
        public async void insertarFpp(DTOFppPasantes fpp)
        {
            string uri = "FppPasante";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(fpp, uri);
                if (correcto)
                {
                    //actualiza paths de fpp1 y fpp7
                    //extrae id creados de fpp1 y fpp7
                    List<DTOFppPasantePlantilla> fpp1_7=await ActualizarFPP1_7(idpasante);
                    //extrae paths de fpp1 y fpp7
                    List<DTOFppCoordinador> fppcoordinador = await fppCoordinador();

                    //actualizar ffp1 y ff7 
                    if (fpp1_7.Count > 0)
                    {
                        //verifica que este vacio el path
                        foreach (var x in fppcoordinador)
                        {
                            foreach(var y in fpp1_7)
                            {
                                if (x.NomPlatillaCordinador.Trim() == y.NomrePlantilla.Trim())
                                {
                                    if (string.IsNullOrEmpty(y.FppPasantePath))
                                    {
                                        //si esta vacio actualiza estado,path y fecha
                                        DTOFppPasantes dtoFppPasante = new DTOFppPasantes
                                        {
                                            IdPasante = -1,
                                            IdAprobador = -1,
                                            IdPlantilla = -1,
                                            IdEstadoFpp = 2,
                                            FppPasanteFechaSubida = DateTime.Now,
                                            FppPasantePath = x.PathFppCoordinador,
                                        };
                                        string url = "FppPasante/" + y.IdFppPasante;
                                        bool correctoActualizacion = await updateEstadoFpp(dtoFppPasante, url);
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudo crear los FPP'); ", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCrearFpp_Click(object sender, EventArgs e)
        {
            //crear fppAlumno del 1 al 7 
            //traer lista de fpp del 1 al 7 para crear lineas
            //verificar que fpp le falta y luego crear
            //si ya tiene creado el fpp no crear nada

            verificarFppFaltante(idpasante);
        }        
        public async void cargarFpps(int idpasante,int pagina)
        {
            //string uri = "FppPasante/fppPasantes/" + idpasante;
            string uri = "FppPasantePlantilla/joinFppPasante/"+pagina+"/" + idpasante;
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
                    dgvPasante.DataSource = fppPasante.OrderBy(x => x.NomrePlantilla).ToList();
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
        public async Task<List<DTOFppPasantePlantilla>> ActualizarFPP1_7(int idpasante)
        {
            string[] fpps = { "FPP1", "FPP4" };
            string uri = "FppPasantePlantilla/joinFppPasante/" + 1 + "/" + idpasante;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            List<DTOFppPasantePlantilla> listaFpp1_7 = new List<DTOFppPasantePlantilla>();
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                listaFpp1_7 = JsonConvert.DeserializeObject<List<DTOFppPasantePlantilla>>(items.ToString()).Where((n)=>fpps.Contains(n.NomrePlantilla.Trim().ToString())).ToList();
                if (Convert.ToBoolean(hasitems))
                {
                    return listaFpp1_7;
                }
                else
                    return listaFpp1_7;
            }
            return listaFpp1_7;
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
            int fila = Convert.ToInt32(e.CommandArgument);            
            if (e.CommandName== "dowloadFile")
            {
                string path = dgvPasante.DataKeys[fila]["FppPasantePath"].ToString();
                if (dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim() == "FPP1" || dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim()=="FPP7" || dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim() == "FPP2")
                {
                    Response.Redirect("dowloadfilescoordinador.aspx?id=" + path);
                }else
                    Response.Redirect("dowloadfilespasante.aspx?id=" + path+"&folder="+CarpetaExpediente);


            }
            if (e.CommandName == "abrirModalHistorialFPP")
            {
                //int idfpp = Convert.ToInt32(dgvPasante.DataKeys[fila]["IdFppPasante"].ToString());
                string nombreFpp = dgvPasante.DataKeys[fila].Values["NomrePlantilla"].ToString();
                idfppAlumno = Convert.ToInt32(dgvPasante.DataKeys[fila]["IdFppPasante"].ToString());
                cargarHistorial(idfppAlumno, 1);
                lblHistorialFpp.Text = "Seguimiento: " + nombreFpp;
                ModalHistorialFPP.Show();
            }
            if (e.CommandName == "verFpp")
            {
                string path = dgvPasante.DataKeys[fila]["FppPasantePath"].ToString();
                if (dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim() == "FPP1" || dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim() == "FPP7" || dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim() == "FPP4")
                {
                    lbltipoFpp.Text = dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim();
                    ifrminformes.Src = "verformularios.aspx?id="+path+"&tipofpp="+dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim()+"&carpeta="+CarpetaExpediente;
                    ModalVerFPP.Show();
                }
                else
                {
                    lbltipoFpp.Text = dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim();
                    ifrminformes.Src = "verformularios.aspx?id="+path+"&tipofpp="+dgvPasante.DataKeys[fila]["NomrePlantilla"].ToString().Trim()+"&carpeta="+CarpetaExpediente;
                    ModalVerFPP.Show();
                }                  
            }

        }
        protected async void dgvPasante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int[] estados = { 3, 4 };
            string uri = "EstadoFpp";
            List<DTOEstadosFpp> estadosfpp = new List<DTOEstadosFpp>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await  con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                estadosfpp = JsonConvert.DeserializeObject<List<DTOEstadosFpp>>(items.ToString()).Where(x=>x.IdEstadoFpp==3 || x.IdEstadoFpp==4).ToList();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int estado = Convert.ToInt32(dgvPasante.DataKeys[e.Row.RowIndex]["IdEstadoFpp"]);
                ddlEstadosFpp = (e.Row.FindControl("ddlEstadosFpp") as DropDownList);
                ddlEstadosFpp.Items.Insert(0, new ListItem("--selecione una opción--"));
                ddlEstadosFpp.AppendDataBoundItems = true;
                ddlEstadosFpp.DataSource = estadosfpp;
                ddlEstadosFpp.DataTextField = "descripcionEstadoFpp";
                ddlEstadosFpp.DataValueField = "idEstadoFpp";
                ddlEstadosFpp.DataBind();
                LinkButton btnDescargarArchivo = (LinkButton)e.Row.FindControl("dowloadFile");
                LinkButton btnverinforme = (LinkButton)e.Row.FindControl("verFpp");
                if (dgvPasante.DataKeys[e.Row.RowIndex]["FppPasantePath"]!=null)
                {

                    btnDescargarArchivo.Visible = true;
                    btnverinforme.Visible = true;
                    ddlEstadosFpp.Enabled = true;
                }
                else
                {
                    btnDescargarArchivo.Visible = false;
                    btnverinforme.Visible = false;
                    ddlEstadosFpp.Enabled = false;
                }
                if (estado == 3)
                {

                    e.Row.BackColor = System.Drawing.Color.FromArgb(68, 215, 84);
                    ddlEstadosFpp.Enabled = false;
                }
                else if (estado == 4)
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(255, 164, 22);
                    
                    ddlEstadosFpp.Enabled = true;
                }
            }
        }

        protected void ddlEstadosFpp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            if (ddl.SelectedValue == "4")
            {
                Label1.Text = "Motivo de Devolución";
                idfppAlumno = Convert.ToInt32(dgvPasante.DataKeys[row.RowIndex].Values["IdFppPasante"].ToString());
                descripcionFpp = dgvPasante.DataKeys[row.RowIndex].Values["NomrePlantilla"].ToString();
                idestadofpp = Convert.ToInt32(ddl.SelectedValue);
                txtObservacion.Text = "";
                ModalPopupExtender3.Show();
            }
            if (ddl.SelectedValue == "3")
            {
                Label1.Text = "Observación";
                idfppAlumno = Convert.ToInt32(dgvPasante.DataKeys[row.RowIndex].Values["IdFppPasante"].ToString());
                idestadofpp = Convert.ToInt32(ddl.SelectedValue);
                txtObservacion.Text = "";
                ModalPopupExtender3.Show();
            }
        }
        protected void dgvHistorico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvHistorico.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex;
            cargarHistorial(idfppAlumno, page);
        }
        public async Task<bool> updateEstadoFpp(DTOFppPasantes dto, string url)
        {
            bool correcto = await con.GenericPut(dto, url);
            return correcto;
        }
        protected async void btnEnviarRechazo_Click(object sender, EventArgs e)
        {
            string url = "FppPasante/" + idfppAlumno;
            DTOFppPasantes dtoFppPasante = new DTOFppPasantes
            {
                IdPasante=-1,
                IdAprobador=-1,
                IdPlantilla=-1,
                IdEstadoFpp =idestadofpp,
                FppPasanteObservacion=txtObservacion.Text
            };
            bool correcto = await updateEstadoFpp(dtoFppPasante, url);
            if (correcto)
            {
                string uri = "HistoricoFpp";
                bool correctoH = false;
                DTOHistoricoFppEstado historicofpp_ = new DTOHistoricoFppEstado();
                historicofpp_.IdFppPasante = idfppAlumno;
                historicofpp_.FechaRegistroHisfpp = DateTime.Now;
                historicofpp_.ObservacionHisfpp = txtObservacion.Text;
                historicofpp_.IdEstadoHisfpp = idestadofpp;
                correctoH = await con.GenericPost(historicofpp_, uri);               
                cargarFpps(idpasante,1);
                if(idestadofpp==4)
                    EnviaCorreoRechazo_Coordinador(lblNombresAlumno.Text, descripcionFpp, txtObservacion.Text);
                txtObservacion.Text = "";
            }
            else
            {
                ClientScript.RegisterStartupScript(typeof(Page), "script", "alertaParametro('error','No se pudo actualizar el estado');", true);
            }
        }
        public async void cargarHistorial(int idFpp, int pagina)
        {


            string uri = "HistoricoFpp/Hitoricobyfpp/page/" + pagina + "/idfpp=" + idFpp;
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
        protected void btnDescargarZip_Click(object sender, EventArgs e)
        {
            try
            {
                
                Response.Redirect("dowloadzip.aspx?folder=" + CarpetaExpediente);
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }

        protected async void btnAprobarTodo_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvPasante.Rows)
            {
                string NombreFPP = dgvPasante.DataKeys[row.RowIndex].Values["NomrePlantilla"].ToString().Trim();
                int idFpp = Convert.ToInt32(dgvPasante.DataKeys[row.RowIndex].Values["IdFppPasante"]);
                int estadofpp= Convert.ToInt32(dgvPasante.DataKeys[row.RowIndex].Values["IdEstadoFpp"]);
                if (dgvPasante.DataKeys[row.RowIndex].Values["FppPasantePath"] != null)
                {

                    string url = "FppPasante/" + idFpp;
                    DTOFppPasantes dtoFppPasante = new DTOFppPasantes
                    {
                        IdPasante = -1,
                        IdAprobador = -1,
                        IdPlantilla = -1,
                        IdEstadoFpp = 3,//3 significa aprobado
                        FppPasanteObservacion = txtObservacion.Text
                    };
                    bool correcto = await updateEstadoFpp(dtoFppPasante, url);
                    if (correcto)
                    {
                        //insertar en historial
                        string uri = "HistoricoFpp";
                        bool correctoH = false;
                        DTOHistoricoFppEstado historicofpp_ = new DTOHistoricoFppEstado();
                        historicofpp_.IdFppPasante = idFpp;
                        historicofpp_.FechaRegistroHisfpp = DateTime.Now;
                        historicofpp_.ObservacionHisfpp = "APROBADO";
                        historicofpp_.IdEstadoHisfpp = 3;
                        if (estadofpp != 3)
                        {
                            correctoH = await con.GenericPost(historicofpp_, uri);

                        }
                        //una vez que ha cargado todos los documentos actualizar estado de pasantía a terminada 2
                        Conexion.ActualizarPracticas("PASANTE", "ESTADO_APROBADO=2","WHERE ID_PASANTE="+idpasante);
                        cargarFpps(idpasante, 1);
                        
                    }
                }

            }

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




                //TableRow RowTable_TotalHoras = new TableRow();
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
        //enviar email cuando se rechaza
        public void EnviaCorreoRechazo_Coordinador(string nombrealumno, string tipoDocumento, string observacion)
        {
            try
            {  
                //string email = "cris.alexis.th@gmail.com";
                string email = emailcoordinador.Trim(); //descomentar en produccion
                string NombreUsuario = nombrealumno;
                MailMessage correo = new MailMessage();
                correo.To.Add(email);

                correo.From = new MailAddress("no.reply@uisek.edu.ec", "Revisión Formulario Prácticas Preprofesionales", System.Text.Encoding.UTF8);
                correo.Subject = "Revisión Formulario Prácticas Preprofesionales";
                correo.SubjectEncoding = System.Text.Encoding.UTF8;
                correo.Body = "Estimad@ coordinador de prácticas preprofesionales."  + "\n El informe "+tipoDocumento+ " del estudiante "+ nombrealumno + " ha sido rechazado con la siguiente observación "+observacion+" por favor volver a realizar la respectiva corrección." + ",\n " +
                  "  NO RESPONDA A ESTE EMAIL.\n  \n En caso de dudas contacte al departamento de Tecnología.  \n helpdesk@uisek.edu.ec";
                correo.BodyEncoding = System.Text.Encoding.UTF8;
                LinkedResource logo = new LinkedResource(Server.MapPath("~/Images/") + "logo.png")
                {
                    ContentId = "logo"
                };
                string body = "<html><head> " +
                 "<style type=\"text/css\">.contenedor { width: 50%;box - shadow: 0 0 25px #5D5C5C;border-radius: 10px;border: solid 0.5px #CBCBD3;text-align: center;} " +
                 ".cabecera {background-color: #0b5a9c; font-size: 2em; color: white;}" +
                 ".footer {background-color: #0b5a9c; font-size: 15px; color: white;}" +
                 ".body {height: 150px;font - size: 20px;}" + "\n" +
                 "</style>" +
                "</head>" +
                "<body>" +
                "<form id=\"form1\" runat=\"server\">" +
                "<center>" +
                "<table style=\"text-align: center;\">" +
                    "<thead>" +
                        "<tr>" +
                            "<th colspan=\"2\" style=\"background-color: #0b5a9c; font-size: 1.5em; color: white;\" ><p>Revisión Formulario Prácticas Preprofesionales</p></th>" +
                             "<th></th>" +
                         "<tr>" +
                    "</thead>" +
                    "<tbody>" +
                        "<tr>" +
                            "<td  colspan=\"2\" style=\"height:150px; background-color:#EEEEEE; font-size: 15px;\">" +
                            "<p>Estimad@ coordinador de prácticas preprofesionales.\nEl informe <b>" + tipoDocumento + "</b> del estudiante <b>" + nombrealumno + "</b> ha sido rechazado con la siguiente observación <b>" + observacion.Trim().ToUpper() + "</b>.\nPor favor volver a realizar la respectiva corrección. <b>" + "</p>" +
                            "</td>" +
                        "</tr>" +
                    "</tbody>" +
                    "<tr>" +
                        "<td style=\"padding:5px; text-align: center; background-color: #0b5a9c; font-size: 12px; color: white;\"><p>Este correo fue generado de forma automática y no requiere respuesta. En caso de dudas contacte al departamento de Tecnología de la Universidad Internacional SEK.</p></td>" +
                        "<td style=\"Background-color:#FFFFFF;\">" +
                            "<img src='cid:logo' width='100px'>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
                "</center>" +
                "</form>" +
                "</body>" +
                "</html>";
                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                // Add the alternate body to the message.
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                // Lo incrustamos en la vista HTML...
                alternate.LinkedResources.Add(logo);
                correo.AlternateViews.Add(alternate);
                correo.IsBodyHtml = false;
                SmtpClient client = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential("no.reply@uisek.edu.ec", PracticasPreProfesionales.LoginDb.Funciones.conectarMail()),
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true //Esto es para que vaya a través de SSL que es obligatorio con GMail 
                };
                client.Send(correo);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudo enviar el correo electrónico al profesor'); console.log('" + ex.Message.Replace("'", "**") + "');", true);
            }
        }
    }
}