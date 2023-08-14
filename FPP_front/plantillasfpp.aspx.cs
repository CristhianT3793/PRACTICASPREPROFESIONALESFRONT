using FPP_front.ConexionServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPP_front.DTOs;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticasPreProfesionales.LoginDb;
using System.Data;

namespace FPP_front
{
    public partial class plantillasfpp : System.Web.UI.Page
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
                        cargarReglas();
                        getPlantilla(1);
                    } 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validarParametro(Convert.ToInt32(ddlRegla.SelectedValue), ddlNombrePlantilla.SelectedValue))
            {
                insertPlantilla();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Ya existe una plantilla con los mismos parámetros'); ", true);
            }

        }
        /// <summary>
        /// valida si ya esta creado un registro con mismos parametros
        /// </summary>
        /// <param name="idparametro"></param>
        /// <param name="tipoFpp"></param>
        /// <returns>true si ya existe un registro con los mismos parametros</returns>
        public bool validarParametro(int idparametro,string tipoFpp)
        {
            
           DataSet ds_parametro= Conexion.BuscarPracticas_ds("PLANTILLA_FPP", "*", "where ID_PARAMETRO="+idparametro+ " and NOMRE_PLANTILLA='"+tipoFpp.Trim()+"'");
            if (ds_parametro.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;

        }

        public void limpiarCampos()
        {
            txtdescPlantilla.Text = "";
            //txtNombrePlantilla.Text = "";
            file = null;
            
        }
        public async void insertPlantilla()
        {
            string uri = "PlantillaFpp";
            DTOPlantillaFpp plantilla_ = new DTOPlantillaFpp();
            bool correcto = false;
            string nombreArchivo = "";
            FileUpload file_ = file;
            plantilla_.IdParametro = Convert.ToInt32(ddlRegla.SelectedValue);
            plantilla_.NomrePlantilla = ddlNombrePlantilla.SelectedValue;
            plantilla_.DescripcionPlantilla = txtdescPlantilla.Text;
            plantilla_.ActivoPlantilla = chkActivo.Checked;    
            if (file_.HasFile && file_.Enabled)
            {
                string strFileNameWithPath = file_.PostedFile.FileName;
              
                nombreArchivo = ddlNombrePlantilla.SelectedValue + "_plantilla_"+ddlRegla.SelectedValue+ System.IO.Path.GetExtension(strFileNameWithPath); 
                if (validarArchivo(nombreArchivo))//valida que el documento se haya guardado correctamente
                {
                    plantilla_.PathPlatilla = nombreArchivo;
                    correcto = await con.GenericPost(plantilla_, uri);
                    if (correcto)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                        getPlantilla(1);
                        limpiarCampos();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
                    }
                }
            }

        }
        /// <summary>
        /// Guarda archivo en carpeta plantillas
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns>true si se guardo,false si no se guardo</returns>
        public bool validarArchivo(string nombreArchivo)
        {
            FileUpload file_ = file;
            string archivo = "";
            bool existe = false;
            if (file_.HasFile && file_.Enabled)
            {
                if (file.PostedFile.ContentType == "application/pdf" || file.PostedFile.ContentType == "application/msword" || file.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    if (file.FileBytes.Length <= 2048 * 1024)
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
                existe = false;
            }
            return existe;
        }
        public async void getPlantilla(int pagina)
        {
            string uri = "PlantillaFpp/GetPlantillaRegla/page/" + pagina;
            List<DTOPlantillaFppRegla> plantilla_ = new List<DTOPlantillaFppRegla>();
            string micro_getdatos = string.Empty;
            micro_getdatos=await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantilla_ = JsonConvert.DeserializeObject<List<DTOPlantillaFppRegla>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvPlantillas.VirtualItemCount= Convert.ToInt32(total);
                    dgvPlantillas.DataSource = plantilla_.ToList();
                    dgvPlantillas.DataBind();
                }
            }
        }
        public string rutaArchivos()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/plantillas/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/plantillas/");
            }
            return path;
        }

        protected void dgvPlantillas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int fila = Convert.ToInt32(e.CommandArgument);
            if(e.CommandName== "descargarInforme")
            {
                string path = dgvPlantillas.DataKeys[fila]["pathPlatilla"].ToString();
                Response.Redirect("dowloadfiles.aspx?id="+path);
            }
        }
        public  void cargarReglas()
        {
            DataSet ds_reglas = Conexion.BuscarPracticas_ds(" PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD=t2.ID_MODALIDAD ", " t1.ID_PARAMETRO,t2.NOMBRE_FACULTAD+' ('+RTRIM(LTRIM(t2.FACULTAD))+'); '+t2.NOMBRE_CARRERA+' ('+rtrim(ltrim(t2.CARRERA))+'); TOTAL HORAS: '+convert(varchar(10),t1.MAX_HORAS_PARAMETRO) as DESCREGLA  ", " where t1.ACTIVO_PARAMETRO=1 ");
            if (ds_reglas.Tables[0].Rows.Count > 0)
            {
                ddlRegla.DataSource = ds_reglas.Tables[0];
                ddlRegla.DataValueField = "ID_PARAMETRO";
                ddlRegla.DataTextField = "DESCREGLA";
                ddlRegla.DataBind();
            }


            //string uri = "Parametro/EstructuraAcademicaParametro/page/1";
            //List<DTOEstructuraAcademicaParametro> reglas_ = new List<DTOEstructuraAcademicaParametro>();
            //string micro_getdatos = string.Empty;
            //micro_getdatos = await con.GenericGet(uri);
            //if (micro_getdatos != "error")
            //{
            //    var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
            //    var total = JObject.Parse(micro_getdatos).SelectToken("total");
            //    var page = JObject.Parse(micro_getdatos).SelectToken("page");
            //    var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
            //    var items = JObject.Parse(micro_getdatos).SelectToken("items");
            //    reglas_ = JsonConvert.DeserializeObject<List<DTOEstructuraAcademicaParametro>>(items.ToString());
            //    var regla = from a in reglas_
            //                select new
            //                {
            //                    IdParametro=a.IdParametro,
            //                    IdModalidad = a.IdModalidad,
            //                    DescRegla = "Carrera: "+a.Carrera+"- Facultad: "+a.Facultad
            //                };
            //    if (Convert.ToBoolean(hasitems))
            //    {
            //        ddlRegla.DataSource = regla.ToList();
            //        ddlRegla.DataValueField = "IdParametro";
            //        ddlRegla.DataTextField = "DescRegla";
            //        ddlRegla.DataBind();
            //    }
            //}
        }

        protected void dgvPlantillas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPlantillas.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            getPlantilla(page);
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                //si tiene algo para buscar
                getPlantillaParametros(txtSearch.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                getPlantilla(1);
            }
        }
        public async void getPlantillaParametros(string parametro,int pagina)
        {
            string uri = "PlantillaFpp/GetPlantillaRegla/page/" + pagina+"/parametro="+parametro;
            List<DTOPlantillaFppRegla> plantilla_ = new List<DTOPlantillaFppRegla>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                plantilla_ = JsonConvert.DeserializeObject<List<DTOPlantillaFppRegla>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvPlantillas.VirtualItemCount = Convert.ToInt32(total);
                    dgvPlantillas.DataSource = plantilla_.ToList();
                    dgvPlantillas.DataBind();
                }
            }
        }

        protected void dgvPlantillas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnDescargarArchivo = (LinkButton)e.Row.FindControl("descargarInforme");

                if (dgvPlantillas.DataKeys[e.Row.RowIndex]["pathPlatilla"] != null && dgvPlantillas.DataKeys[e.Row.RowIndex]["pathPlatilla"] != "")
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