using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class areasestudio : System.Web.UI.Page
    {
        static List<CampoEspecifico> listaCampoEspecifico = new List<CampoEspecifico>();//lista de campos especifico ingreso
        static List<CampoEspecifico> listaCampoEspecificoU = new List<CampoEspecifico>(); //lista de campos especificos para actualizar 
        static readonly Servicios con = new Servicios();
        static int idCampoAmplio = -1;
        static string campoAmplio = "";
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
                        ServicioExtraerCampoAmplio(1);
                        cargarComboCampoAmplio();
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
            DTOCampoAmplio campAmplio = new DTOCampoAmplio
            {
                DescripcionCampoAmplio = txtCampoAmplio.Text
            };
            insertarCampoAmplio(campAmplio);
        }


        public void limpiarContenedor()
        {
            txtCampoAmplio.Text = "";
            txtCampoEspecifico.Text = "";
            rptCampoespecifco.DataSource = "";
            rptCampoespecifco.DataBind();
            listaCampoEspecifico.Clear();
            rptNewCampEsp.DataSource = "";
            rptNewCampEsp.DataBind();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertacorrecto(); ", true);
        }
        public async void insertarCampoAmplio(DTOCampoAmplio campoamplio_)
        {
            string uri = "CampoAmplio";
            int correcto = -1;
            try
            {
                correcto = await con.GenericPostId(campoamplio_, uri);
                if (correcto != -1)
                {

                    
                    //recargar tabla
                    insertarCampoEspecifico(correcto);
                    
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
        public async void insertarCampoEspecifico(int idcampAmplio)
        {
            validarUltimoElemento();
            string uri = "CampoEspecifico";
            //insertar Campo espécifico
            if (txtCampoEspecifico.Text.Trim() != "")
            {
                listaCampoEspecifico.Add(new CampoEspecifico { campo = txtCampoEspecifico.Text });
            }
            if (listaCampoEspecifico.Count > 0)
            {
                foreach (var items in listaCampoEspecifico)
                {
                    DTOCampoEspecifico campEspecifico = new DTOCampoEspecifico
                    {
                        IdCampoAmplio = idcampAmplio,
                        DescripcionCampoEspecifico = items.campo

                    };
                    await con.GenericPost(campEspecifico, uri);
                }
                limpiarContenedor();
                ServicioExtraerCampoAmplio(1);
            }
        }
        public async void ServicioExtraerCampoAmplio(int pagina)
        {
            string uri = "CampoAmplio/page/" + pagina;
            List<DTOCampoAmplio> campoamplio_ = new List<DTOCampoAmplio>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                campoamplio_ = JsonConvert.DeserializeObject<List<DTOCampoAmplio>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    dgvAreasEstudio.VirtualItemCount = Convert.ToInt32(total);
                    dgvAreasEstudio.DataSource = campoamplio_.ToList();
                    dgvAreasEstudio.DataBind();
                    
                }
            }
        }
        protected void rptCampoespecifco_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            TextBox txt = e.Item.FindControl("txtcampoespecifico") as TextBox;
            if (txt != null)
                txt.Text = (e.Item.ItemIndex + 1).ToString();
        }

        protected void rptCampoespecifco_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                validarUltimoElemento();
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                var datos = listaCampoEspecifico;
                listaCampoEspecifico.RemoveAt(id - 1);
                rptCampoespecifco.DataSource = listaCampoEspecifico;
                rptCampoespecifco.DataBind();
            }
        }
        public void validarUltimoElemento()
        {
            listaCampoEspecifico.Clear();
            foreach (RepeaterItem item in rptCampoespecifco.Items)
            {
                listaCampoEspecifico.Add(
                                new CampoEspecifico()
                                {
                                    campo = (item.FindControl("txtcampoespecifico") as TextBox).Text,

                                });
            }
        }
        public void validarUltimoElementoU()
        {
            listaCampoEspecificoU.Clear();
            foreach (RepeaterItem item in rptNewCampEsp.Items)
            {
                listaCampoEspecificoU.Add(
                                new CampoEspecifico()
                                {
                                    campo = (item.FindControl("txtcampoespecificoU") as TextBox).Text,

                                });
            }
        }
        protected void addcampoespecifico_Click(object sender, EventArgs e)
        {
            addcampoesp();
        }

        public void addcampoespU()
        {

            listaCampoEspecifico.Clear();
            foreach (RepeaterItem item in rptNewCampEsp.Items)
            {
                listaCampoEspecifico.Add(
                                new CampoEspecifico()
                                {
                                    campo = (item.FindControl("txtcampoespecificoU") as TextBox).Text,

                                });
            }
            listaCampoEspecifico.Add(new CampoEspecifico());
            rptNewCampEsp.DataSource = listaCampoEspecifico;
            rptNewCampEsp.DataBind();
        }

        public void addcampoesp()
        {

            listaCampoEspecifico.Clear();
            foreach (RepeaterItem item in rptCampoespecifco.Items)
            {
                listaCampoEspecifico.Add(
                                new CampoEspecifico()
                                {
                                    campo = (item.FindControl("txtcampoespecifico") as TextBox).Text,

                                });
            }
            listaCampoEspecifico.Add(new CampoEspecifico());
            rptCampoespecifco.DataSource = listaCampoEspecifico;
            rptCampoespecifco.DataBind();
        }
        protected void dgvAreasEstudio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            dgvAreasEstudio.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex;
            ServicioExtraerCampoAmplio(page+1);
        }

        protected void dgvAreasEstudio_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        public async void cargarComboCampoAmplio()
        {
            List<DTOCampoAmplio> campo_amp = new List<DTOCampoAmplio>();
            campo_amp=await ServicioExtraerCampoAmplios();
            if (campo_amp.Count > 0)
            {
                ddlCampoAmplio.DataSource = campo_amp;
                ddlCampoAmplio.DataTextField = "DescripcionCampoAmplio";
                ddlCampoAmplio.DataValueField = "IdCampoAmplio";
                ddlCampoAmplio.DataBind();
            }

        }
        public async Task<List<DTOCampoAmplio>> ServicioExtraerCampoAmplios()
        {
            string uri = "CampoAmplio";
            List<DTOCampoAmplio> campoesp_ = new List<DTOCampoAmplio>();
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                campoesp_ = JsonConvert.DeserializeObject<List<DTOCampoAmplio>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    return campoesp_;
                }else
                    return campoesp_;
            }else
                return campoesp_;
        }

        /// <summary>
        ///Extrae el detalle de los campos especificos
        ///recibe el id del campo amplio
        /// </summary>
        /// <param name="id"></param>


        public async void ServicioExtraerCamposEspecificos(int pagina, int id)
        {
            List<DTOCampoEspecifico> camp_esp = new List<DTOCampoEspecifico>();
            string uri = "CampoEspecifico/page/" + pagina;
            string micro_getdatos = string.Empty;
            micro_getdatos = await con.GenericGet(uri);
            if (micro_getdatos != "error")
            {
                var hasitems = JObject.Parse(micro_getdatos).SelectToken("hasItems");
                var total = JObject.Parse(micro_getdatos).SelectToken("total");
                var page = JObject.Parse(micro_getdatos).SelectToken("page");
                var pages = JObject.Parse(micro_getdatos).SelectToken("pages");
                var items = JObject.Parse(micro_getdatos).SelectToken("items");
                camp_esp = JsonConvert.DeserializeObject<List<DTOCampoEspecifico>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    camp_esp = camp_esp.Where(x => x.IdCampoAmplio == id).ToList();
                    if (camp_esp.Count() > 0)
                    {
                        dgvUpdateCampoEspecifico.DataSource = camp_esp.ToList();
                        dgvUpdateCampoEspecifico.DataBind();
                    }
                    else
                    {
                        dgvUpdateCampoEspecifico.DataSource = null;
                        dgvUpdateCampoEspecifico.DataBind();
                    }
                }
            }
        }
        protected void ddlCampoAmplio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServicioExtraerCamposEspecificos(1,Convert.ToInt32(ddlCampoAmplio.SelectedValue));
        }

        protected void rptNewCampEsp_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            TextBox txt = e.Item.FindControl("txtcampoespecificoU") as TextBox;
            if (txt != null)
                txt.Text = (e.Item.ItemIndex + 1).ToString();
        }

        protected void rptNewCampEsp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                validarUltimoElementoU();
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                var datos = listaCampoEspecificoU;
                listaCampoEspecificoU.RemoveAt(id - 1);
                rptNewCampEsp.DataSource = listaCampoEspecificoU;
                rptNewCampEsp.DataBind();
            }
        }
        protected void btnNewCampoEsp_Click(object sender, EventArgs e)
        {
            addcampoespU();
        }

        protected void btnUpdateCampoEsp_Click(object sender, EventArgs e)
        {
            if(ddlCampoAmplio.SelectedValue!="0")
                insertarCampoEspecificoU(Convert.ToInt32(ddlCampoAmplio.SelectedValue));
        }
        public async void insertarCampoEspecificoU(int idcampAmplio)
        {
            validarUltimoElementoU();
            string uri = "CampoEspecifico";
            if (listaCampoEspecificoU.Count > 0)
            {
                foreach (var items in listaCampoEspecificoU)
                {
                    DTOCampoEspecifico campEspecifico = new DTOCampoEspecifico
                    {
                        IdCampoAmplio = idcampAmplio,
                        DescripcionCampoEspecifico = items.campo

                    };
                    await con.GenericPost(campEspecifico, uri);
                }
                limpiarContenedor();
                ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
            }
        }

        protected void dgvUpdateCampoEspecifico_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvUpdateCampoEspecifico.EditIndex = -1;
            ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
        }

        protected void dgvUpdateCampoEspecifico_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvUpdateCampoEspecifico.EditIndex = e.NewEditIndex;
            ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
        }

        protected async void dgvUpdateCampoEspecifico_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idCampoEspecifico = Convert.ToInt32(dgvUpdateCampoEspecifico.DataKeys[e.RowIndex]["IdCampoEspecifico"].ToString());
            string url = "CampoEspecifico/" + idCampoEspecifico;
            TextBox txtdesccampoespecifico = (dgvUpdateCampoEspecifico.Rows[e.RowIndex].Cells[0].FindControl("EditCampoEspecifico") as TextBox);
            DTOCampoEspecifico dtocamEspecifico = new DTOCampoEspecifico
            {
                IdCampoAmplio= Convert.ToInt32(dgvUpdateCampoEspecifico.DataKeys[e.RowIndex]["IdCampoAmplio"].ToString()),
                DescripcionCampoEspecifico = txtdesccampoespecifico.Text.Trim()
            };
            bool correcto = await updateCampoEspecifico(dtocamEspecifico, url);
            if (correcto)
            {
                dgvUpdateCampoEspecifico.EditIndex = -1;
                ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
            }
            else
            {
                dgvUpdateCampoEspecifico.EditIndex = -1;
                ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
            }
            dgvUpdateCampoEspecifico.EditIndex = -1;
            ServicioExtraerCamposEspecificos(1, Convert.ToInt32(ddlCampoAmplio.SelectedValue));
        }

        protected void dgvAreasEstudio_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvAreasEstudio.EditIndex = e.NewEditIndex;
            ServicioExtraerCampoAmplio(1);
        }
        public async Task<bool>  updateCampoAmplio(DTOCampoAmplio dto,string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;
            
        }
        public async Task<bool> updateCampoEspecifico(DTOCampoEspecifico dto, string url)
        {

            bool correcto = await con.GenericPut(dto, url);
            return correcto;

        }
        protected async void dgvAreasEstudio_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
            int idCampoAmplio = Convert.ToInt32(dgvAreasEstudio.DataKeys[e.RowIndex]["IdCampoAmplio"].ToString());
            string url = "CampoAmplio/" + idCampoAmplio;
            TextBox txtdesccampoamplio = (dgvAreasEstudio.Rows[e.RowIndex].Cells[0].FindControl("EditCampoAmplio") as TextBox);
            DTOCampoAmplio dtocamAmplio = new DTOCampoAmplio
            {
                DescripcionCampoAmplio = txtdesccampoamplio.Text.Trim()
            };
            bool correcto=await updateCampoAmplio(dtocamAmplio,url);
            if (correcto)
            {
                dgvAreasEstudio.EditIndex = -1;
                ServicioExtraerCampoAmplio(1);
            }
            else
            {
                dgvAreasEstudio.EditIndex = -1;
                ServicioExtraerCampoAmplio(1);
            }

        }
        protected void dgvAreasEstudio_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvAreasEstudio.EditIndex = -1;
            ServicioExtraerCampoAmplio(1);
        }
    }
}