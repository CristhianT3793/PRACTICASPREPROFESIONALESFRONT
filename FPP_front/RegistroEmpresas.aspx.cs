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
using FPP_front.ConexionServicios;
using FPP_front.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FPP_front
{
    public partial class RegistroEmpresas : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();

        static long idEmpresa = -1;
        static string nombreEmpresa_ = "";
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
                        ServicioExtraerEmpresa(1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        public async void insertarEmpresa(DTOEmpresa empresa)
        {
            string uri = "Empresa";
            bool correcto = false;
            try
            {
                correcto = await con.GenericPost(empresa, uri);
                if (correcto)
                {
                    ServicioExtraerEmpresa(1);
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
        /// <summary>
        /// Verifica si ya existe una empresa
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns>devuelve true si ya existe y false si no existe</returns>
        public async Task<bool> ExisteEmpresa(string ruc)
        {
            string uri = "Empresa/GetEmpresaRuc/" + ruc;
            string empresa_ = string.Empty;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            empresa_ = JsonConvert.DeserializeObject<string>(micro_getdatos);
            if (empresa_ != null)
                return true;
            else
                return false;

        }

        public async void ServicioExtraerEmpresa(int pagina)
        {
            string uri = "Empresa/page/" + pagina;
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
                empresa_ = JsonConvert.DeserializeObject<List<DTOEmpresa>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvEmpresas.VirtualItemCount = Convert.ToInt32(total);
                    dgvEmpresas.DataSource = empresa_.ToList();
                    dgvEmpresas.DataBind();
                }
            }
        }

        protected void dgvEmpresas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddTutorEnterprise")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idEmpresa = Convert.ToInt64(dgvEmpresas.DataKeys[fila].Values["idEmpresa"].ToString());
                nombreEmpresa_ = dgvEmpresas.DataKeys[fila].Values["nombreEmpresa"].ToString();
                ifrm.Src = "modalAutoridadEmpresa.aspx?idEm=" + idEmpresa + "&NomEm=" + nombreEmpresa_;
                btnPopUp_ModalPopupExtender.Show();
            }
        }
        protected void dgvEmpresas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvEmpresas.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            ServicioExtraerEmpresa(page);

        }

        public async Task<bool> updateDatosEmpresa(DTOEmpresa dto, string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;

        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchEmpresa.Text.Trim()))
            {
                //si tiene algo para buscar
                cargargridEmpresaxparametros(txtSearchEmpresa.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                ServicioExtraerEmpresa(1);
            }

        }
        public async void cargargridEmpresaxparametros(string parametro, int pagina)//carga todos
        {

            string uri = "Empresa/GetEmpresasParametro/page/" + pagina + "/parametro=" + parametro;
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
                empresa_ = JsonConvert.DeserializeObject<List<DTOEmpresa>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvEmpresas.VirtualItemCount = Convert.ToInt32(total);
                    dgvEmpresas.DataSource = empresa_;
                    dgvEmpresas.DataBind();
                }
                else
                {
                    dgvEmpresas.VirtualItemCount = Convert.ToInt32(total);
                    dgvEmpresas.DataSource = empresa_;
                    dgvEmpresas.DataBind();
                }
            }
        }
    }
}