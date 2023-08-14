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
    public partial class Empresas : System.Web.UI.Page
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
                    limpiarContenido();
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
        public async  Task<bool> ExisteEmpresa(string ruc)
        {
            string uri = "Empresa/GetEmpresaRuc/"+ruc;
            string empresa_ = string.Empty;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            empresa_ = JsonConvert.DeserializeObject<string>(micro_getdatos);
            if (empresa_!= null)
                return true;
            else
                return false;

        }

        protected async void btnGuardar_Click(object sender, EventArgs e)
        {
            DTOEmpresa empresa = new DTOEmpresa();
            empresa.ActivoEmpresa = chkActivoEmp.Checked;
            empresa.CodEmpresa = "EMP";
            empresa.RucEmpresa = txtRucEmpresa.Text;
            empresa.NombreEmpresa = txtNombreEmpresa.Text.ToUpper();
            empresa.TipoEmpresa = txtTipoInsctitucion.Text.Trim().ToUpper();
            empresa.DireccionEmpresa = txtDireccion.Text.Trim().ToUpper();
            empresa.Telefono1Empresa = txtTelefono1.Text.Trim();
            empresa.Telefono2Empresa = txtTelefono2.Text.Trim();
            empresa.EmailEmpresa = txtEmail.Text.Trim();
            empresa.FechafirmaEmpresa =Convert.ToDateTime(txtFechaFirma.Text).Date;
            empresa.ObjetivoEmpresa = txtObjetivo.Text.Trim().ToUpper();
            empresa.ObservacionEmpresa = txtObservacion.Text.Trim().ToUpper();
            empresa.FechaRegistroEmpresa = DateTime.Now;
            empresa.HomologadaEmpresa = chkHomologada.Checked;
            //if(!await ExisteEmpresa(empresa.RucEmpresa) || string.IsNullOrEmpty(empresa.RucEmpresa))
            //{
            if(!string.IsNullOrEmpty(empresa.RucEmpresa) && !string.IsNullOrEmpty(empresa.NombreEmpresa) && !string.IsNullOrEmpty(empresa.TipoEmpresa))
            {
                insertarEmpresa(empresa);
            }
            else
            {
               ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('warning','Debe completar los campos nombre de empresa, ruc, tipo de institución'); ", true);

            }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Ya existe una empresa con el mismo RUC, no se puede guardar'); ", true);
            //}

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
                ifrm.Src = "modalAutoridadEmpresa.aspx?idEm=" + idEmpresa+ "&NomEm=" + nombreEmpresa_;
                btnPopUp_ModalPopupExtender.Show();
            }
            if (e.CommandName == "ModificarEmpresa")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idEmpresa = Convert.ToInt64(dgvEmpresas.DataKeys[fila].Values["idEmpresa"].ToString());
                //btnCancelarEdicion.Visible = true;
                //btntModificarTutor.Visible = true;
                pnlAcciones1.Visible = true;
                pnlAcciones2.Visible = false;
                //btnGuardar.Visible = false;
                cargardatosEmpresa(idEmpresa);
            }
        }
        public async void cargardatosEmpresa(long idempresa)
        {
            string uri = "Empresa/" + idempresa;
            DTOEmpresa empresa_ = new DTOEmpresa();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {

                empresa_ = JsonConvert.DeserializeObject<DTOEmpresa>(micro_getdatos);
                if (empresa_!=null)
                {
                    DateTime fechafirma_ = new DateTime();
                    txtNombreEmpresa.Text = string.IsNullOrEmpty(empresa_.NombreEmpresa)?"":empresa_.NombreEmpresa.Trim();
                    txtTipoInsctitucion.Text= string.IsNullOrEmpty(empresa_.TipoEmpresa) ? "" : empresa_.TipoEmpresa.Trim();
                    txtRucEmpresa.Text = string.IsNullOrEmpty(empresa_.RucEmpresa)? "" : empresa_.RucEmpresa.Trim();
                    txtDireccion.Text = string.IsNullOrEmpty(empresa_.DireccionEmpresa) ? "" : empresa_.DireccionEmpresa.Trim();
                    txtTelefono1.Text = string.IsNullOrEmpty(empresa_.Telefono1Empresa) ? "" : empresa_.Telefono1Empresa.Trim();
                    txtTelefono2.Text = string.IsNullOrEmpty(empresa_.Telefono2Empresa) ? "" : empresa_.Telefono2Empresa.Trim();
                    txtEmail.Text = string.IsNullOrEmpty(empresa_.EmailEmpresa) ? "" : empresa_.EmailEmpresa.Trim();
                    fechafirma_ = Convert.ToDateTime(empresa_.FechafirmaEmpresa).Date;
                    txtFechaFirma.Text = Convert.ToDateTime(empresa_.FechafirmaEmpresa).Date.ToString("yyyy-MM-dd");
                    txtObjetivo.Text = string.IsNullOrEmpty(empresa_.ObjetivoEmpresa) ? "" : empresa_.ObjetivoEmpresa.Trim();
                    txtObservacion.Text= string.IsNullOrEmpty(empresa_.ObservacionEmpresa) ? "" : empresa_.ObservacionEmpresa.Trim();
                    chkActivoEmp.Checked = Convert.ToBoolean(empresa_.ActivoEmpresa);
                    chkHomologada.Checked = Convert.ToBoolean(empresa_.HomologadaEmpresa);
                    hddFechaRegistro.Value = Convert.ToDateTime(empresa_.FechaRegistroEmpresa).Date.ToString("yyyy-MM-dd");
                }
            }


        }
        protected  void dgvEmpresas_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

        protected void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            //btnCancelarEdicion.Visible = false;
            //btntModificarTutor.Visible = false;
            pnlAcciones1.Visible = false;
            pnlAcciones2.Visible = true;

            //btnGuardar.Visible = true;
            limpiarContenido();
        }
        public void limpiarContenido()
        {
            txtNombreEmpresa.Text = "";
            txtTipoInsctitucion.Text = "";
            txtRucEmpresa.Text = "";
            txtDireccion.Text = "";
            txtTelefono1.Text = "";
            txtTelefono2.Text = "";
            txtEmail.Text = "";
            txtFechaFirma.Text = "";
            txtObjetivo.Text = "";
            txtObservacion.Text = "";
            chkActivoEmp.Checked = true;
            chkHomologada.Checked = false;
        }
        protected async void btntModificarEmpresa_Click(object sender, EventArgs e)
        {
            DTOEmpresa dtoEmpresa = new DTOEmpresa();
            string url = "Empresa/" + idEmpresa;
            dtoEmpresa.NombreEmpresa = txtNombreEmpresa.Text;
            dtoEmpresa.TipoEmpresa = txtTipoInsctitucion.Text;
            dtoEmpresa.RucEmpresa = txtRucEmpresa.Text;
            dtoEmpresa.DireccionEmpresa = txtDireccion.Text;
            dtoEmpresa.Telefono1Empresa = txtTelefono1.Text;
            dtoEmpresa.Telefono2Empresa = txtTelefono2.Text;
            dtoEmpresa.EmailEmpresa = txtEmail.Text;
            dtoEmpresa.FechafirmaEmpresa = Convert.ToDateTime(txtFechaFirma.Text).Date;
            dtoEmpresa.ObjetivoEmpresa = txtObjetivo.Text;
            dtoEmpresa.ObservacionEmpresa = txtObservacion.Text;
            dtoEmpresa.ActivoEmpresa = chkActivoEmp.Checked;
            dtoEmpresa.HomologadaEmpresa = chkHomologada.Checked;
            dtoEmpresa.FechaRegistroEmpresa = Convert.ToDateTime(hddFechaRegistro.Value).Date;

            if (!string.IsNullOrEmpty(txtRucEmpresa.Text.Trim()) &&  !string.IsNullOrEmpty(txtNombreEmpresa.Text.Trim()) && !string.IsNullOrEmpty(txtTipoInsctitucion.Text.Trim()))
            {
                bool correcto = await updateDatosEmpresa(dtoEmpresa, url);
                if (correcto)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaActualizado(); ", true);
                    cancelarEdicion();
                    ServicioExtraerEmpresa(1);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','Registro no se pudo actualizar'); ", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('warning','Debe completar los campos nombre de empresa, ruc, tipo de institución'); ", true);

            }


        }
        public void cancelarEdicion()
        {

            pnlAcciones1.Visible = false;
            pnlAcciones2.Visible = true;
            limpiarContenido();
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