using FPP_front.ConexionServicios;
using FPP_front.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class modalAutoridadEmpresa : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static long idEmpresa = -1;
        static string nombreEmpresa_ = "";
        static int idtutor = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                idEmpresa = Convert.ToInt64(Request.QueryString["idEm"]);
                nombreEmpresa_ = Request.QueryString["NomEm"];
                lblEmpresa.Text = nombreEmpresa_;
                cargarTutorporIdEmpresa(1,idEmpresa);
            }

        }
        public async void insertarAutoridadEmpresa(DTOAutoridadEmpresa tutor)
        {
            string uri = "AutoridadEmpresa";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(tutor, uri);
                if (correcto)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
                    cargarTutorporIdEmpresa(1, idEmpresa);
                    lblExisteDatos.Visible = false;
                    limpiarContenido();
                    
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

        public async void cargarTutorporIdEmpresa(int pagina,long idempresa)
        {
            string uri = "AutoridadEmpresa/search/page/"+pagina+"/idEmpresa="+idempresa;
            List<DTOAutoridadEmpresa> tutores = new List<DTOAutoridadEmpresa>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                tutores = JsonConvert.DeserializeObject<List<DTOAutoridadEmpresa>>(items.ToString());

                if (Convert.ToBoolean(hasitems))
                {
                    dgvTutorEmpresa.VirtualItemCount = Convert.ToInt32(total);
                    dgvTutorEmpresa.DataSource = tutores;
                    dgvTutorEmpresa.DataBind();
                }
                else
                {
                    lblExisteDatos.Visible = true;
                }
            }
        }
        protected void btnSaveTutor_Click(object sender, EventArgs e)
        {
            DTOAutoridadEmpresa tutor = new DTOAutoridadEmpresa();

            if (txtCargoTEmpresa.Text.Trim()!="" 
                && txtNombresTEmpresa.Text.Trim()!="" && txtApellidos.Text.Trim()!=""        
                )
            {
                tutor.IdEmpresa = idEmpresa;
                tutor.IdentificacionAempresa = txtIdentificacionTEmpresa.Text;
                tutor.NombreAempresa = txtNombresTEmpresa.Text;
                tutor.ApellidoAempresa = txtApellidos.Text;
                tutor.EmailAempresa = txtEmailTEmpresa.Text;
                tutor.TelefonoAempresa = txtTelefonoTEmpresa.Text;
                tutor.CelularAempresa = txtCelularTEmpresa.Text;
                tutor.DireccionAempresa = txtDireccinTEmpresa.Text;
                tutor.CargoAempresa = txtCargoTEmpresa.Text;
                tutor.ActivoAempresa = true;
                tutor.FechaRegistroAempresa = DateTime.Now;

                insertarAutoridadEmpresa(tutor);
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Tutor no se pudo guardar, falta completar datos'); ", true);
            }

        }
        protected void dgvTutorEmpresa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvTutorEmpresa.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            cargarTutorporIdEmpresa(page, idEmpresa);
        }
        protected async void btntModificarTutor_Click(object sender, EventArgs e)
        {
            DTOAutoridadEmpresa tutor = new DTOAutoridadEmpresa();
            string url = "AutoridadEmpresa/" + idtutor;
            tutor.IdEmpresa = idEmpresa;
            tutor.IdentificacionAempresa = txtIdentificacionTEmpresa.Text.Trim();
            tutor.NombreAempresa = txtNombresTEmpresa.Text.Trim();
            tutor.ApellidoAempresa = txtApellidos.Text.Trim();
            tutor.EmailAempresa = txtEmailTEmpresa.Text.Trim();
            tutor.TelefonoAempresa = txtTelefonoTEmpresa.Text.Trim();
            tutor.CelularAempresa = txtCelularTEmpresa.Text.Trim();
            tutor.DireccionAempresa = txtDireccinTEmpresa.Text.Trim();
            tutor.CargoAempresa = txtCargoTEmpresa.Text.Trim();
            tutor.ActivoAempresa = chkActivoTEmp.Checked;
            bool correcto = await updateDatosTutor(tutor, url);
            if (correcto)
            {
                cargarTutorporIdEmpresa(1, idEmpresa);
                limpiarContenido();
                btnCancelarEdicion.Visible = false;
                btntModificarTutor.Visible = false;
                btnSaveTutor.Visible = true;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaincorrecto(); ", true);
            }
        }

        public async Task<bool> updateDatosTutor(DTOAutoridadEmpresa dto, string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;

        }
        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            btnCancelarEdicion.Visible = false;
            btntModificarTutor.Visible = false;
            btnSaveTutor.Visible = true;
            limpiarContenido();
        }
        public void limpiarContenido()
        {
            txtIdentificacionTEmpresa.Text = "";
            txtNombresTEmpresa.Text = "";
            txtApellidos.Text = "";
            txtEmailTEmpresa.Text = "";
            txtTelefonoTEmpresa.Text = "";
            txtCelularTEmpresa.Text = "";
            txtDireccinTEmpresa.Text = "";
            txtCargoTEmpresa.Text = "";
            chkActivoTEmp.Checked = false;
        }
        protected void dgvTutorEmpresa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ModificarTutor")
            {
                
                int fila = Convert.ToInt32(e.CommandArgument);
                idtutor = Convert.ToInt32(dgvTutorEmpresa.DataKeys[fila].Values["idAutoridadEmpresa"].ToString());
                btnCancelarEdicion.Visible = true;
                btntModificarTutor.Visible = true;
                btnSaveTutor.Visible = false;
                cargarDatosTutor(idtutor);
            }
        }
        public async void cargarDatosTutor(int idTutor)
        {
            string uri = "AutoridadEmpresa/" + idTutor;
            DTOAutoridadEmpresa tutor = new DTOAutoridadEmpresa();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {

                tutor = JsonConvert.DeserializeObject<DTOAutoridadEmpresa>(micro_getdatos);
                if (tutor != null)
                {
                    txtIdentificacionTEmpresa.Text= string.IsNullOrEmpty(tutor.IdentificacionAempresa) ? "" : tutor.IdentificacionAempresa.Trim();
                    txtNombresTEmpresa.Text= string.IsNullOrEmpty(tutor.NombreAempresa) ? "" : tutor.NombreAempresa.Trim();
                    txtApellidos.Text= string.IsNullOrEmpty(tutor.ApellidoAempresa) ? "" : tutor.ApellidoAempresa.Trim();
                    txtEmailTEmpresa.Text= string.IsNullOrEmpty(tutor.EmailAempresa) ? "" : tutor.EmailAempresa.Trim();
                    txtTelefonoTEmpresa.Text= string.IsNullOrEmpty(tutor.TelefonoAempresa) ? "" : tutor.TelefonoAempresa.Trim();
                    txtCelularTEmpresa.Text = string.IsNullOrEmpty(tutor.CelularAempresa) ? "" : tutor.CelularAempresa.Trim();
                    txtDireccinTEmpresa.Text= string.IsNullOrEmpty(tutor.DireccionAempresa) ? "" : tutor.DireccionAempresa.Trim();
                    txtCargoTEmpresa.Text= string.IsNullOrEmpty(tutor.CargoAempresa) ? "" : tutor.CargoAempresa.Trim();
                    chkActivoTEmp.Checked=Convert.ToBoolean(tutor.ActivoAempresa);
                }
            }
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdentificacion.Text.Trim()))
            {
                //si tiene algo para buscar
                cargargridAutoridadxparametros(txtIdentificacion.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                cargarTutorporIdEmpresa(1, idEmpresa);
            }
        }
        public async void cargargridAutoridadxparametros(string parametro, int pagina)//carga todos
        {

            string uri = "AutoridadEmpresa/GetAutoridadParametro/page/" + pagina + "/parametro=" + parametro+"/idempresa="+idEmpresa;
            List<DTOAutoridadEmpresa> tutor = new List<DTOAutoridadEmpresa>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                tutor = JsonConvert.DeserializeObject<List<DTOAutoridadEmpresa>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvTutorEmpresa.VirtualItemCount = Convert.ToInt32(total);
                    dgvTutorEmpresa.DataSource = tutor;
                    dgvTutorEmpresa.DataBind();
                }
                else
                {
                    dgvTutorEmpresa.VirtualItemCount = Convert.ToInt32(total);
                    dgvTutorEmpresa.DataSource = tutor;
                    dgvTutorEmpresa.DataBind();
                }
            }
        }
    }
}