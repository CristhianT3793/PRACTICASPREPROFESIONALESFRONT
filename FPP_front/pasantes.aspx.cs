using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FPP_front.DTOs;
using Newtonsoft.Json;
using FPP_front.ConexionServicios;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Data;
using PracticasPreProfesionales.LoginDb;
using System.Net.Mail;

namespace FPP_front
{
    public partial class pasantes : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static long idpasante = -1;
        static string identificacion = "";
        static string nombres = "";

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
                        cargargridPasante(1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }
        public async void cargargridPasante(int pagina)//carga todos
        {
            string uri = "ConvenioEmpresaPasante/joinAllActivos/page/" + pagina;
            List<DTOConvenioEmpresaPasante> pasante_ = new List<DTOConvenioEmpresaPasante>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                pasante_ = JsonConvert.DeserializeObject<List<DTOConvenioEmpresaPasante>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = pasante_;
                    dgvPasante.DataBind();
                }
            }
        }
        protected  void dgvPasante_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string identificacionTutorEmpresa = "", nombreempresa = "", codcarrera = "",carrera="", facultad = "", carpetaExpediente="",periodo="",emailcoordinador="";
            int idcampoespecifico = -1, horas = -1;
            string fInicio = "",fFinal="";
            if (e.CommandName == "fppAlumno")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idpasante = Convert.ToInt64(dgvPasante.DataKeys[fila].Values["IdPasante"].ToString());
                identificacion = dgvPasante.DataKeys[fila].Values["IdentificacionPasante"].ToString();
                nombres = dgvPasante.DataKeys[fila].Values["NombrePasante"].ToString().Trim()+" "+dgvPasante.DataKeys[fila].Values["ApellidoPasante"].ToString().Trim();
                identificacionTutorEmpresa= dgvPasante.DataKeys[fila].Values["IdentificacionTutorEmpresa"].ToString();
                nombreempresa = dgvPasante.DataKeys[fila].Values["NombreEmpresa"].ToString();
                codcarrera = dgvPasante.DataKeys[fila].Values["CodCarreraPasante"].ToString();
                carrera = dgvPasante.DataKeys[fila].Values["CarreraPasante"].ToString();
                facultad = dgvPasante.DataKeys[fila].Values["FacultadPasante"].ToString();
                idcampoespecifico = Convert.ToInt32(dgvPasante.DataKeys[fila].Values["IdCampoEspecifico"].ToString());
                horas= Convert.ToInt32(dgvPasante.DataKeys[fila].Values["NumeroHorasPasante"].ToString());
                fInicio=dgvPasante.DataKeys[fila].Values["FechaInicioPasante"].ToString();
                fFinal= dgvPasante.DataKeys[fila].Values["FechaFinPasante"].ToString();
                periodo = dgvPasante.DataKeys[fila].Values["PeriodoPasante"].ToString();
                emailcoordinador= dgvPasante.DataKeys[fila].Values["EmailTutor"].ToString();
                carpetaExpediente = dgvPasante.DataKeys[fila].Values["CarpetaPasanteExpediente"].ToString();
                ifrm.Src = "fppPasantes.aspx?id=" + idpasante+"&nom="+nombres.Trim()+"&cedula="+identificacion.Trim()+"&cedulatutor="+identificacionTutorEmpresa.Trim()+"&nomempresa="+ HttpUtility.HtmlDecode(nombreempresa.Trim().Replace("\"", "")) + "&carrera="+carrera.Trim()+"&facultad=" + facultad.Trim()+ "&idcampoespecifico="+ idcampoespecifico+"&horas="+horas+"&finicio="+fInicio+"&ffinal="+fFinal + "&folder=" + carpetaExpediente.Trim()+"&emailCoordinador="+ emailcoordinador.Trim();
                
                btnPopUp_ModalPopupExtender.Show();
            }
        }
        public async void cargargridPasantexparametros(string parametro,int pagina)//carga todos
        {
            string uri = "ConvenioEmpresaPasante/joinAllActivosParametro/page/" + pagina+"/parametro="+parametro;
            List<DTOConvenioEmpresaPasante> pasante_ = new List<DTOConvenioEmpresaPasante>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                pasante_ = JsonConvert.DeserializeObject<List<DTOConvenioEmpresaPasante>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = pasante_;
                    dgvPasante.DataBind();
                }else
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = pasante_;
                    dgvPasante.DataBind();
                }
            }
        }
        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(txtIdentificacion.Text.Trim()))
            {
                //si tiene algo para buscar
                cargargridPasantexparametros(txtIdentificacion.Text.Trim(), 1);
            }
            else
            {
                //si no tiene nada
                cargargridPasante(1);
            }
        }

        protected void dgvPasante_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPasante.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            cargargridPasante(page);
        }

        protected void dgvPasante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                int estadoAbrobado = Convert.ToInt32(dgvPasante.DataKeys[e.Row.RowIndex]["EstadoAprobado"]);
                if (estadoAbrobado == 2)
                {

                    e.Row.Cells[4].BackColor = System.Drawing.Color.FromArgb(68, 215, 84);
                }

            }
        }
    }
}