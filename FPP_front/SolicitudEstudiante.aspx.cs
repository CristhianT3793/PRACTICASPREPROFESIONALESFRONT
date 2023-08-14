using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPP_front.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using FPP_front.ConexionServicios;

namespace FPP_front
{
    public partial class SolicitudEstudiante : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static List<DTOAutoridadEmpresa> autoridadempresa = new List<DTOAutoridadEmpresa>();
        static List<DTOFppPasantePlantilla> fppPasante = new List<DTOFppPasantePlantilla>();
        static List<DTOPlantillaFpp> plantillas = new List<DTOPlantillaFpp>();
        static int idProfundidad_ = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Session["CedCoordinar"] = Session["cp"];
                    if (Session["CedCoordinar"] == null)
                    {
                        Response.Redirect("https://portaldocentes.uisek.edu.ec/");
                    }
                    else
                    {
                        CargarPeriodoActivo();
                        CargarConvenios();
                        CargarEmpresa();
                        cargarCampoAmplio();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }

        }
        public void CargarPeriodoActivo()
        {
            string semestreActivo = CargarPeriodoActivoSemestre();
            DataSet ds = Conexion.BuscarNAV_ds("[NAV_UISEK_ECUADOR].dbo.[UISEK_ECUADOR$Dimension Value] ", "top 25 code,code +' ('+[name]+')' as semestre", "where [Dimension Code]='CURSO' and Code not in ('S/C','N/A') order by Code desc");
            ddlPeriodo.DataSource = ds.Tables[0];
            ddlPeriodo.DataTextField = "semestre";
            ddlPeriodo.DataValueField = "code";         
            ddlPeriodo.DataBind();
            ddlPeriodo.SelectedValue = semestreActivo;
        }


        public string CargarPeriodoActivoSemestre()
        {
            string semestre = "";
            DataSet ds = Conexion.BuscarUMAS_ds("CAMBNOTA_Semestre_Activo", "top 1 * ", "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                semestre = ds.Tables[0].Rows[0]["code"].ToString();
            }
            return semestre;
        }
        public void limpiarCampos()
        {
            txtIdentificacionEstudiante.Text = "";
            txtIdentificacionProfesor.Text = "";
            
        }
        public async void CargarEmpresa()
        {

            string uri = "Empresa";
            List<DTOEmpresa> empresa_ = new List<DTOEmpresa>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                empresa_ = JsonConvert.DeserializeObject<List<DTOEmpresa>>(items.ToString()).Where(x=>x.ActivoEmpresa==true).ToList();
                if (Convert.ToBoolean(hasitems))
                {
                    ddlEmpresa.DataSource = empresa_;
                    ddlEmpresa.DataValueField = "IdEmpresa";
                    ddlEmpresa.DataTextField = "NombreEmpresa";
                    ddlEmpresa.DataBind();
                }
            }
        }


        public async void insertarPasante(DTOPasante pasante)
        {

            string uri = "Pasante";
            int correcto = -1;
            try
            {
                correcto = await con.GenericPostId(pasante, uri);
                if (correcto!=-1)
                {
                    string carpetaExpediente = "SEK_PPP_" + pasante.IdentificacionPasante.Trim() + "_" + pasante.CodCarreraPasante.Trim() + "_" + pasante.PeriodoPasante.Trim() + "_" + correcto;
                    bool actualizacarpetaExpediente = Conexion.ActualizarPracticas("PASANTE", "CARPETA_EXPEDIENTE_PASANTE='"+carpetaExpediente+"'", "where ID_PASANTE="+correcto);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);


                        guardarProfesor(correcto,pasante.CodFacultadPasante.Trim(),pasante.CodCarreraPasante.Trim());

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region creacion_lineas_FPP
        /// <summary>
        /// //extrae todos las plantillas de FPP disponibles en la tabla de plantillas
        /// </summary>
        /// <param name="idpasante"></param>
        public  void crearfppalumno(int idpasante,string facultadpasante,string carrerapasante)
        {
            string[] plantillas = {"FPP2","FPP3" };// se quita el FPP1,FPP2 y FPP7 ya que estos se crea una vez se suba el informe de planificacion y el informe semestral de ptacticas
           
                    foreach (string tipoplantilla in plantillas)
                    {

                            idProfundidad_ = validarProfundidad(tipoplantilla.Trim(),facultadpasante, carrerapasante);
                            DTOFppPasantes objectFppPasante = new DTOFppPasantes();
                            objectFppPasante.IdPasante = idpasante;
                            objectFppPasante.IdEstadoFpp = 1;//1 significa creado
                            objectFppPasante.FppPasantePath = null;
                            objectFppPasante.FppPasanteFechaSubida = null;
                            objectFppPasante.FppPasanteObservacion = null;
                            objectFppPasante.FppPasanteActivo = false;
                            objectFppPasante.IdPlantilla = idProfundidad_;
                            objectFppPasante.IdAprobador = 1;//cambiar
                            insertarFpp(objectFppPasante, idpasante);
                    }
        }

        public async void insertarFpp(DTOFppPasantes fpp, int idpasante)
        {
            string uri = "FppPasante";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(fpp, uri);
                if (correcto)
                {
                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> updateEstadoFpp(DTOFppPasantes dto, string url)
        {
            bool correcto = await con.GenericPut(dto, url);
            return correcto;
        }

        #endregion
        public async void insertarProfesor(DTOEmpresaConvenioTutor empcontuto,string facultadpasante,string carrerapasante)
        {
            string uri = "EmpresaConvenioTutor";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(empcontuto, uri);
                if (correcto)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                    //crear fpps
                    //crearfppalumno(Convert.ToInt32(empcontuto.IdPasante));
                    crearfppalumno(Convert.ToInt32(empcontuto.IdPasante), facultadpasante, carrerapasante);
                    limpiarSolicitud();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  void guardarProfesor(int idpasante,string facultadpasante,string carrerapasante)
        {
            DTOEmpresaConvenioTutor dtoempresacontutor = new DTOEmpresaConvenioTutor();

            dtoempresacontutor.IdConvenio = null;
            dtoempresacontutor.IdEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            dtoempresacontutor.IdPasante = idpasante;
            dtoempresacontutor.IdentificacionTutor = txtIdentificacionProfesor.Text;
            dtoempresacontutor.NombreTutor = Page.Request.Form[txtnombreDocente.UniqueID];
            dtoempresacontutor.ApellidoTutor = Page.Request.Form[txtapellidoDocente.UniqueID];
            dtoempresacontutor.FacultadTutor = txtFacultad.Text;
            dtoempresacontutor.CarreraTutor = Page.Request.Form[txtCarrera.UniqueID];
            dtoempresacontutor.IdentificacionTutorEmpresa = ddlEncargado.SelectedValue;
            if (!chkReconocimiento.Checked)
                dtoempresacontutor.IdConvenio = Convert.ToInt32(ddlconvenio.SelectedValue);

            dtoempresacontutor.EmailTutor = txtEmailP.Text;
            insertarProfesor(dtoempresacontutor, facultadpasante, carrerapasante);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool reconocimiento;
            if (chkReconocimiento.Checked)
            {
                reconocimiento = true;
            }
            else
            {
                reconocimiento = validarFechasconvenio(Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
            }

            if (reconocimiento)
            {
                if (txtIdentificacionEstudiante.Text.Trim() != "" && txtNumeroHoras.Text.Trim() != "" && ddlCampoAmplio.SelectedValue != "0"
                    && ddlCampoEspecifico.SelectedValue != "0" && txtIdentificacionProfesor.Text.Trim() != "" && ddlEmpresa.SelectedValue != "0"
                    && ddlEncargado.SelectedValue != "0"
                    )
                {
                    DTOPasante pasante = new DTOPasante();
                    pasante.IdentificacionPasante = txtIdentificacionEstudiante.Text.Trim();
                    pasante.NombrePasante = Page.Request.Form[txtNombresEstudiante.UniqueID];
                    pasante.ApellidoPasante = Page.Request.Form[txtApellidosEstudiante.UniqueID];
                    pasante.FechaRegistroPasante = DateTime.Now;
                    pasante.NumeroHorasPasante = Convert.ToInt32(txtNumeroHoras.Text);
                    pasante.FechaInicioPasante = Convert.ToDateTime(txtFechaInicio.Text);
                    pasante.FechaFinPasante = Convert.ToDateTime(txtFechaFin.Text);
                    pasante.FacultadPasante = Page.Request.Form[txtFacultad.UniqueID];
                    pasante.CarreraPasante = Page.Request.Form[txtCarrera.UniqueID];
                    pasante.CodCarreraPasante = Page.Request.Form[txtCodCarrera.UniqueID];
                    pasante.ActivoPasante = false;//se inicia en false una vez que el coordinador envia expediente se cambia a true y es visible para el revisor
                    pasante.PeriodoPasante = ddlPeriodo.SelectedValue;
                    pasante.IdCampoEspecifico = Convert.ToInt32(ddlCampoEspecifico.SelectedValue);
                    pasante.CodFacultadPasante = Page.Request.Form[txtCodFacultad.UniqueID];
                    pasante.EstadoAprobado = 1;//1 significa en proceso
                    insertarPasante(pasante);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo guardar, debe completar todos los datos'); ", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','La fecha de inicio y fin de pasantías debe estar dentro de las fechas del convenio escogido.'); ", true);
            }

        }
        public async void cargarCampoAmplio()
        {
            
            string uri = "CampoAmplio";
            List<DTOCampoAmplio> campoAmplio = new List<DTOCampoAmplio>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                campoAmplio = JsonConvert.DeserializeObject<List<DTOCampoAmplio>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    ddlCampoAmplio.DataSource = campoAmplio;
                    ddlCampoAmplio.DataValueField = "IdCampoAmplio";
                    ddlCampoAmplio.DataTextField = "DescripcionCampoAmplio";
                    ddlCampoAmplio.DataBind();
                }
            }
        }
        public async void cargarCampoEspecifico(int idCampoAmplio)
        {
            string uri = "CampoEspecifico";
            List<DTOCampoEspecifico> campoespecifico = new List<DTOCampoEspecifico>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                campoespecifico = JsonConvert.DeserializeObject<List<DTOCampoEspecifico>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    campoespecifico = campoespecifico.Where(x => x.IdCampoAmplio == idCampoAmplio).ToList();
                    if (campoespecifico.Count() > 0)
                    {
                        ddlCampoEspecifico.DataSource = campoespecifico;
                        ddlCampoEspecifico.DataValueField = "IdCampoEspecifico";
                        ddlCampoEspecifico.DataTextField = "DescripcionCampoEspecifico";
                        ddlCampoEspecifico.DataBind();
                    }

                }
            }
        }

        protected void ddlCampoAmplio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampoAmplio.SelectedValue != "0")
            {
                limpiarComboCampEsp();
                int idCampoAmplio = Convert.ToInt32(ddlCampoAmplio.SelectedValue);
                cargarCampoEspecifico(idCampoAmplio);
            }
            else
            {
                limpiarComboCampEsp();
            }
        }

        public void limpiarComboCampEsp()
        {
            ddlCampoEspecifico.Items.Clear();
            ddlCampoEspecifico.AppendDataBoundItems = true;
            ListItem lst = new ListItem("--Seleccionar--", "0");
            ddlCampoEspecifico.Items.Add(lst);
        }

        public async void cargarTutorEmpresa(int idEmpresa )
        {
            string uri = "AutoridadEmpresa";
            
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                autoridadempresa = JsonConvert.DeserializeObject<List<DTOAutoridadEmpresa>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    autoridadempresa = autoridadempresa.Where(x => x.IdEmpresa == idEmpresa && x.ActivoAempresa==true).Select(
                        x=>new DTOAutoridadEmpresa{
                            IdAutoridadEmpresa=x.IdAutoridadEmpresa,
                            IdentificacionAempresa=x.IdentificacionAempresa,
                            NombreAempresa=x.IdentificacionAempresa+"-"+x.NombreAempresa+" "+x.ApellidoAempresa+"("+x.CargoAempresa+")",
                            CargoAempresa=x.CargoAempresa
                        }
                        ).ToList();
                    if (autoridadempresa.Count() > 0)
                    {
                        ddlEncargado.DataSource = autoridadempresa;
                        ddlEncargado.DataValueField = "IdAutoridadEmpresa";
                        ddlEncargado.DataTextField = "NombreAempresa";
                        ddlEncargado.DataBind();
                    }
                }
            }
        }
        public void limpiarcomboAutoridadEmpresa()
        {
   
            ddlEncargado.Items.Clear();
            ddlEncargado.AppendDataBoundItems = true;
            ListItem lst = new ListItem("--Seleccionar--", "0");
            ddlEncargado.Items.Add(lst);
        }
        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpresa.SelectedValue != "0")
            {
                limpiarcomboAutoridadEmpresa();
                int idempresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                cargarTutorEmpresa(idempresa);
            }
            else
            {
                limpiarcomboAutoridadEmpresa();


            }
        }
        public async void CargarConvenios()
        {
            string uri = "Convenio";
            List<DTOConvenio> convenio_ = new List<DTOConvenio>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                convenio_ = JsonConvert.DeserializeObject<List<DTOConvenio>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    if (convenio_.Count() > 0)
                    {

                        ddlconvenio.DataSource = convenio_;
                        ddlconvenio.DataValueField = "IdConvenio";
                        ddlconvenio.DataTextField = "NombreConvenio";
                        ddlconvenio.DataBind();
                    }
                }
            }
        }
        public async void cargarDatosConvenio(string idconvenio)
        {
            string uri = "Convenio/"+idconvenio;
            DTOConvenio convenio_ = new DTOConvenio();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                convenio_ = JsonConvert.DeserializeObject<DTOConvenio>(micro_getdatos);
                if (convenio_!=null)
                {
                    txtDescConvenio.Text = convenio_.DescripcionConvenio;
                    txtFechaInicioConvenio.Text = Convert.ToDateTime(convenio_.FechaInicioConvenio).Date.ToString();
                    txtFechaFinConvenio.Text = Convert.ToDateTime(convenio_.FechaFinConvenio).Date.ToString();
                }
            }
        }
        protected void ddlconvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlconvenio.SelectedValue!="0")
            {
                cargarDatosConvenio(ddlconvenio.SelectedValue);
            }
            else
            {
                txtDescConvenio.Text = "";
                txtFechaInicioConvenio.Text = "";
                txtFechaFinConvenio.Text = "";
            }
        }
        public void limpiarSolicitud()
        {
            txtIdentificacionEstudiante.Text = "";
            txtNumeroHoras.Text = "";
            ddlCampoAmplio.SelectedValue = "0";
            ddlCampoEspecifico.SelectedValue = "0";
            txtIdentificacionProfesor.Text = "";
            ddlEmpresa.SelectedValue = "0";
            ddlEncargado.SelectedValue = "0";
            ddlconvenio.SelectedValue = "0";
            txtDescConvenio.Text = "";
            txtFechaInicioConvenio.Text = "";
            txtFechaFinConvenio.Text = "";
        }

        public int  validarProfundidad(string tipoFpp,string codFacultad,string codcarrera)
        {
            //1  busca primero con facultad y carrera
            DataSet ds_fac_carr = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='"+tipoFpp+"' and  t2.activo=1 and t2.FACULTAD='" + codFacultad.Trim()+"' and t2.CARRERA='"+codcarrera.Trim()+"'");
            if (ds_fac_carr.Tables[0].Rows.Count > 0)
            {
                //2  si encontro retorna id de parametro activo
                
                return Convert.ToInt32(ds_fac_carr.Tables[0].Rows[0]["ID_PLANTILLA"]);
            }
            else//3  si no encontro hacer lo que sigue
            { 
                //4  se busca primero en facultad
                DataSet ds_faacultad= Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and t2.activo=1 and t2.FACULTAD='" + codFacultad.Trim() + "'");
                if(ds_faacultad.Tables[0].Rows.Count > 0)
                {
                    //5  si encuentra con facultad retorna el id
                    return Convert.ToInt32(ds_faacultad.Tables[0].Rows[0]["ID_PLANTILLA"]);
                }
                else
                {
                    //6  si no encuentra facultad busca en carrera
                    DataSet ds_carrera = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and t2.activo=1 and t2.CARRERA='" + codcarrera.Trim() + "'");
                    if (ds_carrera.Tables[0].Rows.Count > 0)
                    {
                        //7  si encuentra con carrera retorna id
                        return Convert.ToInt32(ds_carrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    }
                    else//8  si no encuentra ninguno escoge todos
                    {
                        //9  retorna el id de la profundidad
                        DataSet ds_todos = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and t2.activo=1 and t2.CARRERA='TODOS' and t2.FACULTAD='TODOS'");
                        return Convert.ToInt32(ds_todos.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    }
                }
            }
            
            //10 una vez que se tiene el id de profundidad se busca en plantillas quien lleva el id y se crea linea de FPP
        }
        /// <summary>
        /// recibe la fecha inicion y fin de pasantias
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns>devuelve true si esta dentro de las fechas del convenio</returns>
        public bool validarFechasconvenio(DateTime fechaInicio,DateTime fechaFin)
        {
            if (fechaInicio >= Convert.ToDateTime(txtFechaInicioConvenio.Text).Date && fechaFin <= Convert.ToDateTime(txtFechaFinConvenio.Text).Date)
            {
                return true;
            }
            else
                return false;
        }
        protected void chkReconocimiento_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReconocimiento.Checked)
            {
                pnlConvenios.Visible = false;
            }
            else
            {
                pnlConvenios.Visible = true;
            }
        }

    }
}