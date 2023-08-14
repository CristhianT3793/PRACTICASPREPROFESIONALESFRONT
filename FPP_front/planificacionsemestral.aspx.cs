﻿using FPP_front.ConexionServicios;
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
    public partial class planificacionsemestral : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static int idplatillaFpp1 = -1;//obtener el id de platilla
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
                        //cargar carreras del coordinador
                        CargarCarreras(identificacioncoordinador);
                        cargarplanificacion(1, identificacioncoordinador, carreras);//pagina,cedula de un coordinador
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
        public async void cargarplantillaFPP1(int idprofundidad, string tipofpp)
        {
            string path = await ServicioExtarerPathPlantilla(idprofundidad, tipofpp);
            if (path != "")
            {
                txtpathFPP.Text = path.Trim();
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
        public async void buscarCordinador(string identificacion)
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
                coordinadores_ = JsonConvert.DeserializeObject<List<DTOCoordinador>>(items.ToString()).Where(x => x.Activocoordinador == true).ToList();
                if (Convert.ToBoolean(hasitems))
                {

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


        public async void cargarplanificacion(int pagina, string identificacionCoordinador, string[] carreras_)
        {
            //separar carreras
            string carreras_separado = "";
            for (int i = 0; i < carreras_.Length; i++)
            {
                carreras_separado = carreras_separado + "," + carreras_[i].Trim();

            }
            carreras_separado = carreras_separado.Substring(1, carreras_separado.Length - 1);
            string uri = "FppCoordinador/GetFppCoordinadorCarrera/page/" + pagina + "/carrera=" + carreras_separado + "/tipoDocumento=FPP1";
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
                    dgvFppPlanificacion.VirtualItemCount = Convert.ToInt32(total);
                    dgvFppPlanificacion.DataSource = plantilla_.ToList();
                    dgvFppPlanificacion.DataBind();
                }
            }
        }
        public async void cargarplanificacionxParametro(int pagina, string identificacionCoordinador, string[] carreras_, string parametro)
        {
            //separar carreras
            string carreras_separado = "";
            for (int i = 0; i < carreras_.Length; i++)
            {
                carreras_separado = carreras_separado + "," + carreras_[i].Trim();

            }
            carreras_separado = carreras_separado.Substring(1, carreras_separado.Length - 1);
            string uri = "FppCoordinador/GetFppCoordinadorCarrera/page/" + pagina + "/carrera=" + carreras_separado + "/tipoDocumento=FPP1/parametro=" + parametro;
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
                    dgvFppPlanificacion.VirtualItemCount = Convert.ToInt32(total);
                    dgvFppPlanificacion.DataSource = plantilla_.ToList();
                    dgvFppPlanificacion.DataBind();
                }
            }
        }

        /// <summary>
        /// valida si ya esta creado un registro con mismos parametros
        /// </summary>
        /// <param name="tipoFpp"></param>
        /// <param name="codcarrera"></param>
        /// <param name="semestre"></param>
        /// <returns>true si ya existe un registro con los mismos parametros</returns>
        public bool validarParametro(string codcarrera, string semestre)
        {
            DataSet ds_parametro = Conexion.BuscarPracticas_ds("FPP_COORDINADOR", "*", "where NOM_PLATILLA_CORDINADOR='FPP1' AND CARRERA_FPP_COORDINADOR='" + codcarrera + "' AND SEMESTRE_FPP_COORDINADOR='" + semestre + "'");
            if (ds_parametro.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            //obtener el id del coordinador

            string uri = "FppCoordinador";
            DTOFppCoordinador fppcoordinador = new DTOFppCoordinador();
            FileUpload file_ = fileplanificacion;
            string nombreArchivo = "";
            bool correcto = false;
            fppcoordinador.Idcoordinador = idCoordinador_;//cambiar esta quemado
            fppcoordinador.IdPlantilla = idplatillaFpp1;
            fppcoordinador.FechaRegistroFpp = DateTime.Now;
            fppcoordinador.NomPlatillaCordinador = "FPP1";
            fppcoordinador.CarreraFppCoordinador = ddlcarrera.SelectedValue;
            fppcoordinador.SemestreFppCoordinador = ddlsemestre.SelectedValue;
            //valiadar registro con parametro
            if (!validarParametro(ddlcarrera.SelectedValue, ddlsemestre.SelectedValue))
            {
                if (file_.HasFile && file_.Enabled)
                {
                    string strFileNameWithPath = file_.PostedFile.FileName;
                    //nombreArchivo = "FPP1_PLANIFICACION_QARQ_" + txtsemestre.Text + System.IO.Path.GetExtension(strFileNameWithPath);
                    nombreArchivo = "FPP1_PLANIFICACION_SEMESTRAL_" + ddlcarrera.SelectedValue.Trim() + "_" + ddlsemestre.SelectedValue.Trim() + System.IO.Path.GetExtension(strFileNameWithPath);

                    if (validarArchivo(nombreArchivo,file_))//valida que el documento se haya guardado correctamente
                    {
                        fppcoordinador.PathFppCoordinador = nombreArchivo;
                        correcto = await con.GenericPost(fppcoordinador, uri);
                        if (correcto)
                        {

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                            cargarplanificacion(1, identificacioncoordinador, carreras);
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
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo guardar ya existe un registro guardado para el semestre y carrera'); ", true);
            }
        }
        public string rutaArchivos()//ruta para validar si guardo el ffp de planificación correctamente
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
        public bool validarArchivo(string nombreArchivo, FileUpload file_)
        {
            //FileUpload file_ = fileplanificacion;
            string archivo = "";
            bool existe = false;
            if (file_.HasFile && file_.Enabled)
            {
                if (file_.PostedFile.ContentType == "application/pdf")
                {
                    if (file_.FileBytes.Length <= 2048 * 1024)
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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','solo se acepta archivos en formato pdf'); ", true);
                }
            }
            else
            {
                existe = false;
            }
            return existe;
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
                    idplatillaFpp1 = idprofundidad;
                    return pathfpp;
                }
            }
            return pathfpp;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)//descarga la plantilla
        {
            Response.Redirect("dowloadfiles.aspx?id=" + txtpathFPP.Text);
        }

        protected void dgvFppPlanificacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int fila = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "descargarInforme")
            {
                string path = dgvFppPlanificacion.DataKeys[fila]["PathFppCoordinador"].ToString();
                Response.Redirect("dowloadfilescoordinador.aspx?id=" + path);
            }
            if (e.CommandName == "modificarDocumento")
            {
                string path = dgvFppPlanificacion.DataKeys[fila]["PathFppCoordinador"].ToString();
                lbldescarchivo.Text = path;
                mdlArchivo.Show();
            }
        }

        protected void ddlcarrera_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoFpp = "FPP1";
            if (ddlcarrera.SelectedValue != "0")
            {
                //mandar a buscar plantilla FPP1 con parametros
                idProfundidad_ = validarProfundidad(tipoFpp, ddlcarrera.SelectedValue.Trim());
                if (idProfundidad_ != -1)
                {
                    btnGuardar.Enabled = true;
                    cargarplantillaFPP1(idProfundidad_, tipoFpp);
                    //cargar plantilla
                }
                else
                {
                    btnGuardar.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No existe ninguna plantilla asociada para la carrera'); ", true);
                }

            }
            else
            {
                txtpathFPP.Text = "";
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

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                //si tiene algo para buscar
                cargarplanificacionxParametro(1, identificacioncoordinador, carreras, txtSearch.Text.Trim());
            }
            else
            {
                //si no tiene nada
                cargarplanificacion(1, identificacioncoordinador, carreras);
            }
        }

        protected void dgvFppPlanificacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvFppPlanificacion.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;

            cargarplanificacion(page, identificacioncoordinador, carreras);
        }

        protected void btnSubirFpp_Click(object sender, EventArgs e)
        {

            FileUpload file_ = fileFpp;
            //valiadar registro con parametro

            if (file_.HasFile && file_.Enabled)
            {
                if (validarArchivo(lbldescarchivo.Text,file_))//valida que el documento se haya guardado correctamente
                {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('success','Archivo actualizado correctamente.'); ", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudo actualizar el archivo.'); ", true);
                }
            }
        }
    }
}