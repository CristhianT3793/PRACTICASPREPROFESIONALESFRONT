using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPP_front.DTOs;
using FPP_front.ConexionServicios;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FPP_front
{
    public partial class horascumplimiento : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
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
                        cargarFacultad();
                        cargarEstructura(1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }



            }
        }
        /// <summary>
        /// Valida que no se repita el mismo parametro al momento de guardar
        /// </summary>
        /// <param name="codFacultad"></param>
        /// <param name="codCarrera"></param>
        /// <returns>true si existe</returns>
        public bool validarParametro(string codFacultad, string codCarrera) {

            DataSet ds_parametro=Conexion.BuscarPracticas_ds("ESTRUCTURA_ACADEMICA", "*", " where FACULTAD='"+ codFacultad + "' and CARRERA='"+ codCarrera + "'");
            if (ds_parametro.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //validar que no se repita al momento de guardar con mismo parametros facultad-carrera
            if(!validarParametro(ddlFacultad.SelectedValue, ddlCarrera.SelectedValue)){
                DTOEstructuraAcademica estructura_ = new DTOEstructuraAcademica();

                estructura_.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text).Date;
                estructura_.FechaFin = Convert.ToDateTime(txtFechaFin.Text).Date;
                estructura_.Activo = chkActivo.Checked;
                estructura_.Modalidad = null;
                estructura_.PeriodoPasante = null;
                estructura_.IdProfundidad = null;
                if (ddlFacultad.SelectedValue != "0")
                {
                    estructura_.NombreFacultad = ddlFacultad.SelectedItem.Text;
                    estructura_.Facultad = ddlFacultad.SelectedValue;
                }
                else
                {
                    estructura_.NombreFacultad = "-";
                    estructura_.Facultad = "-";
                }

                if (ddlCarrera.SelectedValue != "0")
                {
                    estructura_.NombreCarrera = ddlCarrera.SelectedItem.Text;
                    estructura_.Carrera = ddlCarrera.SelectedValue;
                }
                else
                {
                    estructura_.NombreCarrera = "-";
                    estructura_.Carrera = "-";
                }

                insertarEstructuraAcademica(estructura_);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','La regla no se pudo guardar, ya exsiste un registro con el mismo parametro'); ", true);
            }
        }
        public async void insertarEstructuraAcademica(DTOEstructuraAcademica estructura_)
        {
            string uri = "EstructuraAcademica";
            int correcto = -1;
            if (ddlFacultad.SelectedValue != "0" && txtHorasCumplimiento.Text!="" && txtHorasCumplimiento.Text!="0")
            {
                try
                {
                    correcto = await con.GenericPostId(estructura_, uri);
                    if (correcto != -1)
                    {
                        DTOParametro parametro_ = new DTOParametro();
                        parametro_.IdModalidad = correcto;
                        parametro_.FechaInicioParametro = Convert.ToDateTime(txtFechaInicio.Text).Date;
                        parametro_.FechaFinParametro = Convert.ToDateTime(txtFechaFin.Text).Date;
                        parametro_.ActivoParametro = chkActivo.Checked;
                        parametro_.MaxHorasParametro = Convert.ToInt32(txtHorasCumplimiento.Text);
                        insertarParametro(parametro_);
                        //guardar en tabla parametros
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
            }else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
            }

        }
        public async void cargarEstructura(int pagina)
        {
            string uri = "Parametro/EstructuraAcademicaParametro/page/" + pagina;
            List<DTOEstructuraAcademicaParametro> estructura_ = new List<DTOEstructuraAcademicaParametro>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                estructura_ = JsonConvert.DeserializeObject<List<DTOEstructuraAcademicaParametro>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvProfundidad.VirtualItemCount = Convert.ToInt32(total);
                    dgvProfundidad.DataSource = estructura_.ToList();
                    dgvProfundidad.DataBind();
                }
            }
        }
        public async void cargarEstructuraxParametros(string parametro,int pagina)
        {
            string uri = "Parametro/EstructuraAcademicaParametro/page/" + pagina+"/"+parametro;
            List<DTOEstructuraAcademicaParametro> estructura_ = new List<DTOEstructuraAcademicaParametro>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                estructura_ = JsonConvert.DeserializeObject<List<DTOEstructuraAcademicaParametro>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvProfundidad.VirtualItemCount = Convert.ToInt32(total);
                    dgvProfundidad.DataSource = estructura_.ToList();
                    dgvProfundidad.DataBind();
                }
            }
        }
        public async void insertarParametro(DTOParametro parametro_)
        {
            string uri = "Parametro";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(parametro_, uri);
                if (correcto)
                {
                    cargarEstructura(1);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
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
        public void cargarFacultad()
        {
            DataSet facultad = Conexion.BuscarUMAS_ds("SEK_Facultad_Carrera", " distinct codfac_new,facultad_new ", "");
            ddlFacultad.DataSource = facultad;
            ddlFacultad.DataTextField = "facultad_new";
            ddlFacultad.DataValueField = "codfac_new";
            ddlFacultad.DataBind();
        }

        public void cargarCarrera(string codcarrera)
        {
            limpiarcarrera();
            DataSet carrera = Conexion.BuscarUMAS_ds("SEK_Facultad_Carrera", "codcarr,carrera", "where codfac_ant='" + codcarrera + "'");
            ddlCarrera.DataSource = carrera;
            ddlCarrera.DataTextField = "carrera";
            ddlCarrera.DataValueField = "codcarr";
            ddlCarrera.DataBind();
        }
        public void cargarArea(string codfac)
        {
            limpiarArea();
			
            DataSet area = Conexion.BuscarUMAS_ds("SEK_Facultad_Carrera", " distinct codfac_ant,facultad_ant", "where codfac_new='" + codfac + "'");
            ddlArea.DataSource = area;
            ddlArea.DataTextField = "facultad_ant";
            ddlArea.DataValueField = "codfac_ant";
            ddlArea.DataBind();


        }

        protected void ddlFacultad_SelectedIndexChanged(object sender, EventArgs e)
        {

            cargarArea(ddlFacultad.SelectedValue);
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCarrera(ddlArea.SelectedValue);
        }
        public void limpiarArea()
        {
            ddlArea.Items.Clear();
            ListItem opcion = new ListItem("--Seleccione una opción--", "0");
            ddlArea.Items.Insert(0, opcion);
        }
        public void limpiarcarrera()
        {
            ddlCarrera.Items.Clear();
            ListItem opcion = new ListItem("--Seleccione una opción--", "0");
            ddlCarrera.Items.Insert(0, opcion);


        }

        protected void dgvProfundidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvProfundidad.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            cargarEstructura(page);
        }

        protected void dgvProfundidad_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvProfundidad.EditIndex = -1;
            cargarEstructura(1);
        }

        protected void dgvProfundidad_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) {
                dgvProfundidad.EditIndex = e.NewEditIndex;
                cargarEstructura(1);
            }
            else
            {
                dgvProfundidad.EditIndex = e.NewEditIndex;
                cargarEstructuraxParametros(txtSearch.Text.Trim(), 1);
            }

        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                //si tiene algo para buscar
                cargarEstructuraxParametros(txtSearch.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                cargarEstructura(1);
            }
        }

        protected void dgvProfundidad_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idParametro = Convert.ToInt32(dgvProfundidad.DataKeys[e.RowIndex]["IdParametro"].ToString());
            TextBox numHoras = (dgvProfundidad.Rows[e.RowIndex].Cells[0].FindControl("EditHorasCumplimiento") as TextBox);
            if (!string.IsNullOrEmpty(numHoras.Text))
            {
                bool correcto = actualizarHoraCumplimiento(idParametro, Convert.ToInt32(numHoras.Text));
                if (correcto)
                {


                    dgvProfundidad.EditIndex = -1;
                    cargarEstructura(1);
                }
                else
                {
                    dgvProfundidad.EditIndex = -1;
                    cargarEstructura(1);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
            }
        }
        public bool actualizarHoraCumplimiento(int idParametro,int numhoras)
        {
            bool correcto = Conexion.ActualizarPracticas(" PARAMETRO ", "MAX_HORAS_PARAMETRO=" + numhoras, " WHERE ID_PARAMETRO=" + idParametro);
            return correcto;
        }



    }
}