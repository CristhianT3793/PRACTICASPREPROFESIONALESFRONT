using FPP_front.ConexionServicios;
using FPP_front.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PracticasPreProfesionales.LoginDb;
using System.Data;
using System.Threading.Tasks;

namespace FPP_front
{
    public partial class ConveniosEmpresas : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();

        static int idconvenio = -1;
        static string patharchivo = "";
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
                        ServicioExtraerConvenio(1);
                        CargarEmpresa();


                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                
            }
        }
        public async void ServicioExtraerConvenio(int pagina)
        {
            string uri = "Convenio/page/" + pagina;
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
                    dgvConvenios.VirtualItemCount = Convert.ToInt32(total);
                    dgvConvenios.DataSource = convenio_.ToList();
                    dgvConvenios.DataBind();
                }
            }
        }




        protected  void btnGuardar_Click(object sender, EventArgs e)
        {
            insertarconvenio_service();
        }

        public async void insertarconvenio_service()
        {
            DTOConvenio convenio_ = new DTOConvenio();
            string nombreArchivo = "";
            bool correcto = false;
            string uri = "Convenio";
            convenio_.NombreConvenio = txtNombreConvenio.Text;
            convenio_.ActivoConvenio = chkActivoConvenio.Checked;
            convenio_.DescripcionConvenio = txtDescripcionConvenio.Text;
            convenio_.FechaInicioConvenio = Convert.ToDateTime(txtFechaInicio.Text).Date;
            convenio_.FechaFinConvenio = Convert.ToDateTime(txtFechaFin.Text).Date;
            convenio_.IdEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            if (ddlEmpresa.SelectedValue != "0")
            {
                if (fileconvenio.HasFile && fileconvenio.Enabled)
                {
                    string strFileNameWithPath = fileconvenio.PostedFile.FileName;
                    nombreArchivo = "CONVENIO_" + obtenerIdConvenio() + System.IO.Path.GetExtension(strFileNameWithPath);
                    if (validarArchivo(nombreArchivo))//valida que el documento se haya guardado correctamente
                    {
                        convenio_.PathConvenio = nombreArchivo;
                        correcto = await con.GenericPost(convenio_, uri);
                        if (correcto)
                        {

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                            ServicioExtraerConvenio(1);
                            limpiarContenido();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                        }
                    }
                }
                else
                {
                    correcto = await con.GenericPost(convenio_, uri);
                    if (correcto)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                        ServicioExtraerConvenio(1);
                        limpiarContenido();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','Por favor seleccione una empresa'); ", true);
            }

        }


        public int obtenerIdConvenio()
        {

            int id = -1;
            DataSet ds_id = Conexion.BuscarPracticas_ds_simple("IDENT_CURRENT ('[dbo].[CONVENIO]') AS Current_Identity");
            if (ds_id.Tables[0].Rows.Count > 0)
            {
                id = Convert.ToInt32(ds_id.Tables[0].Rows[0]["Current_Identity"].ToString())+1;
            }
            return id;
        }

        public bool validarArchivo(string nombreArchivo)
        {

            FileUpload file_ = fileconvenio;
            string archivo = "";
            bool existe = false;
            if (file_.HasFile && file_.Enabled)
            {
                if (fileconvenio.PostedFile.ContentType == "application/pdf" )
                {
                    if (fileconvenio.FileBytes.Length <= 4096 * 1024)
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
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','Se debe subir un archivo en formato pdf'); ", true);
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
                path = Server.MapPath("/documentosPPP/convenios/");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"console.log('" + path + "');", true);
            }
            else
            {
                path = Server.MapPath("/documentosPPP/convenios/");
            }
            return path;
        }

        public void limpiarContenido()
        {
            txtNombreConvenio.Text = "";
            chkActivoConvenio.Checked = false;
            txtDescripcionConvenio.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            ddlEmpresa.SelectedValue = "0";
        }
        public async void ServicioExtraerConvenioParametro(string parametro,int pagina)
        {
            string uri = "Convenio/GetConvenioParametro/page/" + pagina+ "/parametro="+parametro;
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
                    dgvConvenios.VirtualItemCount = Convert.ToInt32(total);
                    dgvConvenios.DataSource = convenio_.ToList();
                    dgvConvenios.DataBind();
                }
            }
        }

        protected void dgvConvenios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int fila = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "descargarInforme")
            {
                string path = dgvConvenios.DataKeys[fila]["PathConvenio"].ToString();
                Response.Redirect("dowloadconvenios.aspx?id=" + path);
            }
            if (e.CommandName == "ModificarConvenio")
            {
                idconvenio = Convert.ToInt32(dgvConvenios.DataKeys[fila].Values["IdConvenio"].ToString());
                pnlAcciones1.Visible = true;
                pnlAcciones2.Visible = false;
                cargardatosconvenio_service(idconvenio);
            }
        }




        public async void cargardatosconvenio_service(int idConvenio) {
            string uri = "Convenio/" + idConvenio;
            DTOConvenio convenio_ = new DTOConvenio();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            
            if (micro_getdatos != "error")
            {

                convenio_ = JsonConvert.DeserializeObject<DTOConvenio>(micro_getdatos);
                if (convenio_ != null)
                {
                    ddlEmpresa.SelectedValue = "0";
                    txtNombreConvenio.Text = convenio_.NombreConvenio;
                    txtDescripcionConvenio.Text = convenio_.DescripcionConvenio;
                    txtFechaInicio.Text = Convert.ToDateTime(convenio_.FechaInicioConvenio).Date.ToString("yyyy-MM-dd");
                    txtFechaFin.Text = Convert.ToDateTime(convenio_.FechaFinConvenio).Date.ToString("yyyy-MM-dd");
                    chkActivoConvenio.Checked= Convert.ToBoolean(convenio_.ActivoConvenio);
                    if (!string.IsNullOrEmpty(convenio_.PathConvenio))
                    {
                        patharchivo = convenio_.PathConvenio.Trim();
                    }
                    if (!string.IsNullOrEmpty(convenio_.IdEmpresa.ToString()))
                    {
                        ddlEmpresa.SelectedValue = convenio_.IdEmpresa.ToString();
                    }
                    

                }
            }

        }

        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            pnlAcciones1.Visible = false;
            pnlAcciones2.Visible = true;
            limpiarContenido();
        }


        protected  void btntModificarConvenio_Click(object sender, EventArgs e)
        {
            modificarConvenio_service();
        }
        public async void modificarConvenio_service()
        {
            DTOConvenio dtoConvenio = new DTOConvenio();
            string url = "Convenio/" + idconvenio;
            dtoConvenio.NombreConvenio = txtNombreConvenio.Text;
            dtoConvenio.DescripcionConvenio = txtDescripcionConvenio.Text;
            dtoConvenio.ActivoConvenio = chkActivoConvenio.Checked;
            dtoConvenio.FechaInicioConvenio = Convert.ToDateTime(txtFechaInicio.Text).Date;
            dtoConvenio.FechaFinConvenio = Convert.ToDateTime(txtFechaFin.Text).Date;
            dtoConvenio.IdEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            string strFileNameWithPath = fileconvenio.PostedFile.FileName;
            if (fileconvenio.HasFile && fileconvenio.Enabled)
            {
                patharchivo = "CONVENIO_" + idconvenio + System.IO.Path.GetExtension(strFileNameWithPath);
                dtoConvenio.PathConvenio = patharchivo;
                if (validarArchivo(patharchivo))//valida que el documento se haya guardado correctamente
                {
                    bool correcto = await updateDatosConvenio(dtoConvenio, url);
                    if (correcto)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
                        cancelarEdicion();
                        ServicioExtraerConvenio(1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Registro no se pudo actualizar'); ", true);
                    }
                }
            }
            else
            {
                dtoConvenio.PathConvenio = patharchivo;
                bool correcto = await updateDatosConvenio(dtoConvenio, url);
                if (correcto)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
                    cancelarEdicion();
                    ServicioExtraerConvenio(1);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Registro no se pudo actualizar'); ", true);
                }
            }

        }

        public void cancelarEdicion()
        {
            pnlAcciones1.Visible = false;
            pnlAcciones2.Visible = true;
            limpiarContenido();
        }
        public async Task<bool> updateDatosConvenio(DTOConvenio dto, string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;

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
                empresa_ = JsonConvert.DeserializeObject<List<DTOEmpresa>>(items.ToString()).Where(x => x.ActivoEmpresa == true).ToList();
                if (Convert.ToBoolean(hasitems))
                {
                    ddlEmpresa.DataSource = empresa_;
                    ddlEmpresa.DataValueField = "IdEmpresa";
                    ddlEmpresa.DataTextField = "NombreEmpresa";
                    ddlEmpresa.DataBind();
                }
            }
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                //si tiene algo para buscar
                ServicioExtraerConvenioParametro(txtSearch.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                ServicioExtraerConvenio(1);
            }
        }





        protected void dgvConvenios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvConvenios.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            ServicioExtraerConvenio(page);
        }

        /// <summary>
        /// DATASETS
        /// </summary>

        public void cargardatosconvenio_ds(int idConvenio)
        {
            string idempres = "0";
            DataSet ds_convenio = Conexion.BuscarPracticas_ds("CONVENIO", "*", "where ID_CONVENIO=" + idConvenio);
            if (ds_convenio.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_convenio.Tables[0].Rows[0]["ID_EMPRESA"].ToString()))
                {
                    idempres = ds_convenio.Tables[0].Rows[0]["ID_EMPRESA"].ToString();
                }
                txtNombreConvenio.Text = ds_convenio.Tables[0].Rows[0]["NOMBRE_CONVENIO"].ToString();
                txtDescripcionConvenio.Text = ds_convenio.Tables[0].Rows[0]["DESCRIPCION_CONVENIO"].ToString();
                txtFechaInicio.Text = Convert.ToDateTime(ds_convenio.Tables[0].Rows[0]["FECHA_INICIO_CONVENIO"]).Date.ToString("yyyy-MM-dd");
                txtFechaFin.Text = Convert.ToDateTime(ds_convenio.Tables[0].Rows[0]["FECHA_FIN_CONVENIO"]).Date.ToString("yyyy-MM-dd");
                chkActivoConvenio.Checked = Convert.ToBoolean(ds_convenio.Tables[0].Rows[0]["ACTIVO_CONVENIO"]);
                ddlEmpresa.SelectedValue = idempres;
                if (!string.IsNullOrEmpty(ds_convenio.Tables[0].Rows[0]["PATH_CONVENIO"].ToString()))
                {
                    patharchivo = ds_convenio.Tables[0].Rows[0]["PATH_CONVENIO"].ToString().Trim();
                }
            }
        }
        public void cargarGridConvenio()
        {
            DataSet ds_convenio = DatasetExtraerConvenio();
            if (ds_convenio.Tables[0].Rows.Count > 0)
            {
                dgvConvenios.DataSource = ds_convenio.Tables[0];
                dgvConvenios.DataBind();
            }
        }
        public void cargarGridConvenioParametro(string parametro)
        {
            DataSet ds_convenio = DatasetExtraerConvenioParametro(parametro);
            if (ds_convenio.Tables[0].Rows.Count > 0)
            {
                dgvConvenios.DataSource = ds_convenio.Tables[0];
                dgvConvenios.DataBind();
            }
        }

        public DataSet DatasetExtraerConvenio()
        {
            DataSet ds_convenio = Conexion.BuscarPracticas_ds("CONVENIO t1 left join EMPRESA t2 on t1.ID_EMPRESA=t2.ID_EMPRESA", "t1.ID_CONVENIO IdConvenio,t1.NOMBRE_CONVENIO NombreConvenio,"+
            "t1.ACTIVO_CONVENIO ActivoConvenio,"+
            "t1.DESCRIPCION_CONVENIO DescripcionConvenio,"+
            "t1.FECHA_INICIO_CONVENIO FechaInicioConvenio,"+
            "t1.FECHA_FIN_CONVENIO FechaFinConvenio,"+
            "t1.PATH_CONVENIO PathConvenio,"+
            "t1.ID_EMPRESA IdEmpresa,"+
            "t2.NOMBRE_EMPRESA EmpresaConvenio ", "order by t2.NOMBRE_EMPRESA asc");
            return ds_convenio;
        }
        public DataSet DatasetExtraerConvenioParametro(string parametro)
        {
            parametro=parametro.Trim().Replace(" ","%");
            DataSet ds_convenio = Conexion.BuscarPracticas_ds("CONVENIO t1 left join EMPRESA t2 on t1.ID_EMPRESA=t2.ID_EMPRESA", "t1.ID_CONVENIO IdConvenio,t1.NOMBRE_CONVENIO NombreConvenio," +
            "t1.ACTIVO_CONVENIO ActivoConvenio," +
            "t1.DESCRIPCION_CONVENIO DescripcionConvenio," +
            "t1.FECHA_INICIO_CONVENIO FechaInicioConvenio," +
            "t1.FECHA_FIN_CONVENIO FechaFinConvenio," +
            "t1.PATH_CONVENIO PathConvenio," +
            "t1.ID_EMPRESA IdEmpresa," +
            "t2.NOMBRE_EMPRESA EmpresaConvenio ", "where t1.NOMBRE_CONVENIO like '%"+parametro+ "%' or t1.DESCRIPCION_CONVENIO  like '%" + parametro + "%' order by t2.NOMBRE_EMPRESA asc");
            return ds_convenio;
        }



        //cambiar a servicio
        public void insertarconevio_ds()
        {
            string nombreArchivo = "";
            bool correcto = false;
            if (ddlEmpresa.SelectedValue != "0")
            {
                if (fileconvenio.HasFile && fileconvenio.Enabled)
                {
                    string strFileNameWithPath = fileconvenio.PostedFile.FileName;
                    nombreArchivo = "CONVENIO_" + obtenerIdConvenio() + System.IO.Path.GetExtension(strFileNameWithPath);
                    if (validarArchivo(nombreArchivo))//valida que el documento se haya guardado correctamente
                    {
                        correcto = Conexion.InsertarPracticas("CONVENIO", "NOMBRE_CONVENIO,ACTIVO_CONVENIO,DESCRIPCION_CONVENIO,FECHA_INICIO_CONVENIO,FECHA_FIN_CONVENIO,PATH_CONVENIO,ID_EMPRESA",
                        "'" + txtNombreConvenio.Text + "'," + Convert.ToInt32(chkActivoConvenio.Checked) + ",'" + txtDescripcionConvenio.Text + "','" + Convert.ToDateTime(txtFechaInicio.Text).Date + "','" + Convert.ToDateTime(txtFechaFin.Text).Date + "','" + nombreArchivo + "'," + ddlEmpresa.SelectedValue);
                        if (correcto)
                        {

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                            //ServicioExtraerConvenio(1);
                            cargarGridConvenio();
                            limpiarContenido();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                        }
                    }
                }
                else
                {
                    correcto = Conexion.InsertarPracticas("CONVENIO", "NOMBRE_CONVENIO,ACTIVO_CONVENIO,DESCRIPCION_CONVENIO,FECHA_INICIO_CONVENIO,FECHA_FIN_CONVENIO,PATH_CONVENIO,ID_EMPRESA",
                    "'" + txtNombreConvenio.Text + "'," + Convert.ToInt32(chkActivoConvenio.Checked) + ",'" + txtDescripcionConvenio.Text + "','" + Convert.ToDateTime(txtFechaInicio.Text).Date + "','" + Convert.ToDateTime(txtFechaFin.Text).Date + "','" + nombreArchivo + "',"+ ddlEmpresa.SelectedValue);
                    if (correcto)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                        //ServicioExtraerConvenio(1);
                        cargarGridConvenio();
                        limpiarContenido();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','Por favor seleccione una empresa'); ", true);

            }


        }
        public void modificarConvenio_ds()
        {
            //DTOConvenio dtoConvenio = new DTOConvenio();
            //string url = "Convenio/" + idconvenio;
            //dtoConvenio.NombreConvenio = txtNombreConvenio.Text;
            //dtoConvenio.DescripcionConvenio = txtDescripcionConvenio.Text;
            //dtoConvenio.ActivoConvenio = chkActivoConvenio.Checked;
            //dtoConvenio.FechaInicioConvenio = Convert.ToDateTime(txtFechaInicio.Text).Date;
            //dtoConvenio.FechaFinConvenio = Convert.ToDateTime(txtFechaFin.Text).Date;
            string idempresa = null;
            if (ddlEmpresa.SelectedValue!="0")
                idempresa = ddlEmpresa.SelectedValue;
            string strFileNameWithPath = fileconvenio.PostedFile.FileName;
            if (fileconvenio.HasFile && fileconvenio.Enabled)
            {
                patharchivo = "CONVENIO_" + idconvenio + System.IO.Path.GetExtension(strFileNameWithPath);
                //dtoConvenio.PathConvenio = patharchivo;
                if (validarArchivo(patharchivo))//valida que el documento se haya guardado correctamente
                {
                    //bool correcto = await updateDatosConvenio(dtoConvenio, url);

                    bool correcto = Conexion.ActualizarPracticas("CONVENIO", "NOMBRE_CONVENIO='"+txtNombreConvenio.Text+"'," +
                        "ACTIVO_CONVENIO="+Convert.ToInt32(chkActivoConvenio.Checked)+"," +
                        "DESCRIPCION_CONVENIO='"+txtDescripcionConvenio.Text+"'," +
                        "FECHA_INICIO_CONVENIO='"+ Convert.ToDateTime(txtFechaInicio.Text).Date + "'," +
                        "FECHA_FIN_CONVENIO='"+ Convert.ToDateTime(txtFechaFin.Text).Date + "'," +
                        "PATH_CONVENIO='"+ patharchivo + "'," +
                        "ID_EMPRESA="+ idempresa, "where ID_CONVENIO="+idconvenio);

                    if (correcto)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
                        cancelarEdicion();
                        //ServicioExtraerConvenio(1);
                        cargarGridConvenio();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Registro no se pudo actualizar'); ", true);
                    }
                }
            }
            else
            {
                //dtoConvenio.PathConvenio = patharchivo;
                //bool correcto = await updateDatosConvenio(dtoConvenio, url);
                bool correcto = Conexion.ActualizarPracticas("CONVENIO", "NOMBRE_CONVENIO='" + txtNombreConvenio.Text + "'," +
                                "ACTIVO_CONVENIO=" + Convert.ToInt32(chkActivoConvenio.Checked) + "," +
                                "DESCRIPCION_CONVENIO='" + txtDescripcionConvenio.Text + "'," +
                                "FECHA_INICIO_CONVENIO='" + Convert.ToDateTime(txtFechaInicio.Text).Date + "'," +
                                "FECHA_FIN_CONVENIO='" + Convert.ToDateTime(txtFechaFin.Text).Date + "'," +
                                "PATH_CONVENIO='" + patharchivo + "'," +
                                "ID_EMPRESA=" + idempresa, "where ID_CONVENIO=" + idconvenio);
                if (correcto)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
                    cancelarEdicion();
                    //ServicioExtraerConvenio(1);
                    cargarGridConvenio();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Registro no se pudo actualizar'); ", true);
                }
            }

        }

        protected void dgvConvenios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnDescargarArchivo = (LinkButton)e.Row.FindControl("descargarInforme");

                if (dgvConvenios.DataKeys[e.Row.RowIndex]["PathConvenio"]!=null && dgvConvenios.DataKeys[e.Row.RowIndex]["PathConvenio"] != "")
                {

                    btnDescargarArchivo.Visible = true;
                }
                else
                {
                    btnDescargarArchivo.Visible = false;
                }
            }
        }
    }
}