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
    public partial class ActaSocializacion : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static int idplatillaFpp2 = -1;
        static string pathfpp = "";
        static int idProfundidad_ = -1;
        static string identificacioncoordinador = "";
        static string[] carreras;
        static int idCoordinador_ = -1;
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
                        identificacioncoordinador = Session["CedCoordinar"].ToString();//la cedula remplazar por Session["CedCoordinar"]
                        //1716631286 QARQ,QCIV
                        //1707757348 QMAE,QMAED
                        carreras = extraerCarrerasCoordinador(identificacioncoordinador);
                        idCoordinador_ = getIdCoordinador(identificacioncoordinador);
                        //verifica y extrae platilla FPP1
                        //si no esta creado el tipo de plantilla no dejar guardar
                        //buscar si esta creado planificacion para el semestre actual caso FPP1 tabla  fppcoordinador
                        //si existe mostrar alerta y nodejar subir
                        //si no existe dejar subir
                        CargarPeriodoActivo();
                        CargarCarreras(identificacioncoordinador);
                        cargarActaSocializacion(1, identificacioncoordinador, carreras);//cedula de un coordinador
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public int getIdCoordinador(string identificacion)
        {
            int idCoordinador = -1;
            DataSet ds_carreras = Conexion.BuscarPracticas_ds("COORDINADOR", " IDCOORDINADOR ", "where IDENTIFICACIONCOORDINADOR='" + identificacion + "' or IDENTIFICACIONCOORDINADOR='0" + identificacion + "' or '0'+IDENTIFICACIONCOORDINADOR='" + identificacion + "'");

            if (ds_carreras.Tables[0].Rows.Count > 0)
            {
                idCoordinador = Convert.ToInt32(ds_carreras.Tables[0].Rows[0]["IDCOORDINADOR"]);

            }
            return idCoordinador;
        }
        public async void cargarActaSocializacion(int pagina, string identificacionCoordinador, string[] carreras_)
        {
            string carreras_separado = "";
            for (int i = 0; i < carreras_.Length; i++)
            {
                carreras_separado = carreras_separado + "," + carreras_[i].Trim();

            }
            carreras_separado = carreras_separado.Substring(1, carreras_separado.Length - 1);
            string uri = "FppCoordinador/GetFppCoordinadorCarrera/page/" + pagina + "/carrera=" + carreras_separado + "/tipoDocumento=FPP2";
            List<DTOFppCoordinador> plantilla_ = new List<DTOFppCoordinador>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantilla_ = JsonConvert.DeserializeObject<List<DTOFppCoordinador>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvFppActaSocializacion.VirtualItemCount = Convert.ToInt32(total);
                    dgvFppActaSocializacion.DataSource = plantilla_.ToList();
                    dgvFppActaSocializacion.DataBind();
                }
            }
        }
        public async void CargarCarreras(string identificacion)
        {
            string uri = "Coordinador";
            List<DTOCoordinador> coordinadores_ = new List<DTOCoordinador>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                coordinadores_ = JsonConvert.DeserializeObject<List<DTOCoordinador>>(items.ToString()).Where(x => x.Activocoordinador == true && (x.Identificacioncoordinador.Trim() == identificacion.Trim() || x.Identificacioncoordinador.Trim() == "0" + identificacion.Trim() || "0" + x.Identificacioncoordinador.Trim() == "0" + identificacion.Trim() || "0" + x.Identificacioncoordinador.Trim() == identificacion.Trim())).ToList();
                if (Convert.ToBoolean(hasitems))
                {
                    ddlcarrera.DataSource = coordinadores_;
                    ddlcarrera.DataTextField = "Carreracoordinador";
                    ddlcarrera.DataValueField = "Carreracoordinador";
                    ddlcarrera.DataBind();
                }
            }
        }
        public void CargarPeriodoActivo()
        {
            string semestreActivo = CargarPeriodoActivoSemestre();
            DataSet ds = Conexion.BuscarNAV_ds("[NAV_UISEK_ECUADOR].dbo.[UISEK_ECUADOR$Dimension Value] ", "top 25 code,code +' ('+[name]+')' as semestre", "where [Dimension Code]='CURSO' and Code not in ('S/C','N/A') order by Code desc");
            ddlsemestre.DataSource = ds.Tables[0];
            ddlsemestre.DataTextField = "semestre";
            ddlsemestre.DataValueField = "code";
            ddlsemestre.DataBind();
            ddlsemestre.SelectedValue = semestreActivo;
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
        public string[] extraerCarrerasCoordinador(string identificacion)
        {

            DataSet ds_carreras = Conexion.BuscarPracticas_ds("COORDINADOR", " CARRERACOORDINADOR ", "where IDENTIFICACIONCOORDINADOR='" + identificacion + "' or IDENTIFICACIONCOORDINADOR='0" + identificacion + "' or '0'+IDENTIFICACIONCOORDINADOR='" + identificacion + "'");
            string[] carreras = new string[ds_carreras.Tables[0].Rows.Count];
            for (int i = 0; i < ds_carreras.Tables[0].Rows.Count; i++)
            {
                carreras[i] = ds_carreras.Tables[0].Rows[i]["CARRERACOORDINADOR"].ToString().Trim();
            }
            return carreras;
        }
        public async void cargarActaSocializacionxParametro(int pagina, string identificacionCoordinador, string[] carreras_, string parametro)
        {
            //separar carreras
            string carreras_separado = "";
            for (int i = 0; i < carreras_.Length; i++)
            {
                carreras_separado = carreras_separado + "," + carreras_[i].Trim();

            }
            carreras_separado = carreras_separado.Substring(1, carreras_separado.Length - 1);
            string uri = "FppCoordinador/GetFppCoordinadorCarrera/page/" + pagina + "/carrera=" + carreras_separado + "/tipoDocumento=FPP2/parametro=" + parametro;
            List<DTOFppCoordinador> plantilla_ = new List<DTOFppCoordinador>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantilla_ = JsonConvert.DeserializeObject<List<DTOFppCoordinador>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvFppActaSocializacion.VirtualItemCount = Convert.ToInt32(total);
                    dgvFppActaSocializacion.DataSource = plantilla_.ToList();
                    dgvFppActaSocializacion.DataBind();
                }
            }
        }
        public bool validarParametro(string codcarrera, string semestre)
        {
            DataSet ds_parametro = Conexion.BuscarPracticas_ds("FPP_COORDINADOR", "*", "where NOM_PLATILLA_CORDINADOR='FPP2' AND CARRERA_FPP_COORDINADOR='" + codcarrera + "' AND SEMESTRE_FPP_COORDINADOR='" + semestre + "'");
            if (ds_parametro.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        protected void dgvFppActaSocializacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvFppActaSocializacion.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            cargarActaSocializacion(page, identificacioncoordinador, carreras);
        }

        protected void dgvFppActaSocializacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int fila = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "descargarInforme")
            {
                string path = dgvFppActaSocializacion.DataKeys[fila]["PathFppCoordinador"].ToString();
                Response.Redirect("dowloadfilescoordinador.aspx?id=" + path);
            }
        }
        public int validarProfundidad(string tipoFpp, string codcarrera)
        {
            //1  busca primero con facultad y carrera
            int existePlantilla = -1;
            DataSet ds_fac_carr = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and  t2.activo=1  and t2.CARRERA='" + codcarrera.Trim() + "'");
            if (ds_fac_carr.Tables[0].Rows.Count > 0)
            {
                //2  si encontro retorna id de parametro activo
                existePlantilla = Convert.ToInt32(ds_fac_carr.Tables[0].Rows[0]["ID_PLANTILLA"]);
            }
            else//3  si no encontro hacer lo que sigue
            {

                //6  si no encuentra facultad busca en carrera
                DataSet ds_carrera = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and t2.activo=1 and t2.CARRERA='" + codcarrera.Trim() + "'");
                if (ds_carrera.Tables[0].Rows.Count > 0)
                {
                    //7  si encuentra con carrera retorna id

                    existePlantilla = Convert.ToInt32(ds_carrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                }
                else//8  si no encuentra ninguno escoge todos
                {
                    //9  retorna el id de la profundidad
                    DataSet ds_todos = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD = t2.ID_MODALIDAD inner join PLANTILLA_FPP t3 on t1.ID_PARAMETRO = t3.ID_PARAMETRO", "t3.ID_PLANTILLA,t3.NOMRE_PLANTILLA,T1.ID_PARAMETRO,T1.ID_MODALIDAD,FECHA_INICIO_PARAMETRO,FECHA_FIN_PARAMETRO,ACTIVO_PARAMETRO, MAX_HORAS_PARAMETRO, FACULTAD, CARRERA, t2.FECHA_INICIO, T2.FECHA_FIN, ACTIVO", " where t3.NOMRE_PLANTILLA='" + tipoFpp + "' and t2.activo=1 and t2.CARRERA='TODOS' and t2.FACULTAD='TODOS'");
                    if (ds_todos.Tables[0].Rows.Count > 0)
                        existePlantilla = Convert.ToInt32(ds_todos.Tables[0].Rows[0]["ID_PLANTILLA"]);

                }

            }
            //si no existe nada debe de devolver un -1
            return existePlantilla;
            //10 una vez que se tiene el id de profundidad se busca en plantillas quien lleva el id y se crea linea de FPP
        }
        protected void ddlcarrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoFpp = "FPP2";
            if (ddlcarrera.SelectedValue != "0")
            {
                //mandar a buscar plantilla FPP1 con parametros
                idProfundidad_ = validarProfundidad(tipoFpp, ddlcarrera.SelectedValue.Trim());
                if (idProfundidad_ != -1)
                {
                    btnGuardar.Enabled = true;
                    cargarplantillaFPP2(idProfundidad_, tipoFpp);
                    //cargar plantilla
                }
                else
                {
                    btnGuardar.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto('error','No existe ninguna plantilla asociada para la carrera'); ", true);
                }

            }
            else
            {
                txtpathFPP.Text = "";
            }
        }
        public async void cargarplantillaFPP2(int idprofundidad, string tipofpp)
        {
            string path = await ServicioExtarerPathPlantilla(idprofundidad, tipofpp);
            if (path != "")
            {
                txtpathFPP.Text = path.Trim();
            }

        }
        public async Task<string> ServicioExtarerPathPlantilla(int idprofundidad, string TipoDocumento)
        {
            string uri = "PlantillaFpp";
            List<DTOPlantillaFpp> plantilla_ = new List<DTOPlantillaFpp>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantilla_ = JsonConvert.DeserializeObject<List<DTOPlantillaFpp>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    pathfpp = plantilla_.Where(x => x.IdPlantilla == idprofundidad && x.NomrePlantilla.Trim() == TipoDocumento).Select(x => (x.PathPlatilla)).FirstOrDefault();
                    idplatillaFpp2 = idprofundidad;
                    return pathfpp;
                }
            }
            return pathfpp;
        }
        public bool validarArchivo(string nombreArchivo)
        {
            FileUpload file_ = fileplanificacion;
            string archivo = "";
            bool existe = false;
            if (file_.HasFile && file_.Enabled)
            {
                if (fileplanificacion.PostedFile.ContentType == "application/pdf")
                {
                    if (fileplanificacion.FileBytes.Length <= 2048 * 1024)
                    {
                        archivo = rutaArchivos() + nombreArchivo;
                        if (File.Exists(archivo))
                            System.IO.File.Delete(archivo);

                        file_.PostedFile.SaveAs(archivo);
                        existe = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','Debe subir un documento en formato pdf'); ", true);
                }
            }
            else
            {
                existe = false;
            }
            return existe;
        }
        public string rutaArchivos()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
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
        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            string uri = "FppCoordinador";
            DTOFppCoordinador fppcoordinador = new DTOFppCoordinador();
            FileUpload file_ = fileplanificacion;
            string nombreArchivo = "";
            bool correcto = false;
            fppcoordinador.Idcoordinador = idCoordinador_;//cambiar esta quemado
            fppcoordinador.IdPlantilla = idplatillaFpp2;
            fppcoordinador.FechaRegistroFpp = DateTime.Now;
            fppcoordinador.NomPlatillaCordinador = "FPP2";
            fppcoordinador.CarreraFppCoordinador = ddlcarrera.SelectedValue;
            fppcoordinador.SemestreFppCoordinador = ddlsemestre.SelectedValue;
            fppcoordinador.SemestreFppCoordinador = ddlsemestre.SelectedValue;
            if (!validarParametro(ddlcarrera.SelectedValue, ddlsemestre.SelectedValue))
            {
                if (file_.HasFile && file_.Enabled)
                {
                    string strFileNameWithPath = file_.PostedFile.FileName;
                    //nombreArchivo = "FPP1_PLANIFICACION_QARQ_" + txtsemestre.Text + System.IO.Path.GetExtension(strFileNameWithPath);
                    nombreArchivo = "FPP2_INFORME_ACTA_SOCIALIZACION_" + ddlcarrera.SelectedValue.Trim() + "_" + ddlsemestre.SelectedValue + System.IO.Path.GetExtension(strFileNameWithPath);

                    if (validarArchivo(nombreArchivo))//valida que el documento se haya guardado correctamente
                    {
                        fppcoordinador.PathFppCoordinador = nombreArchivo;
                        correcto = await con.GenericPost(fppcoordinador, uri);
                        if (correcto)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                            cargarActaSocializacion(1, identificacioncoordinador, carreras);
                            //getPlantilla(1);
                            //limpiarCampos();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                        }

                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','Ya existe un registro guardado para el semestre y carrera'); ", true);
            }



        }
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            Response.Redirect("dowloadfiles.aspx?id=" + txtpathFPP.Text);
        }
        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                //si tiene algo para buscar
                cargarActaSocializacionxParametro(1, identificacioncoordinador, carreras, txtSearch.Text.Trim());
            }
            else
            {
                //si no tiene nada
                cargarActaSocializacion(1, identificacioncoordinador, carreras);
            }
        }
    }
}