using System;
using System.Collections.Generic;
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
    public partial class modalareasespecificas : System.Web.UI.Page
    {
        static List<DTOCampoEspecifico> camposEspecificos = new List<DTOCampoEspecifico>();
        static readonly Servicios con = new Servicios();
        static int idCampoAmplio = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCampoAmplio.Text= Request.QueryString["ca"];
                idCampoAmplio = Convert.ToInt32(Request.QueryString["id"]);
                ServicioExtraerEmpresa(1,idCampoAmplio);
            }
        }

        protected void rptCampoespecifco_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptCampoespecifco_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        /// <summary>
        ///Extrae el detalle de los campos especificos
        ///recibe el id del campo amplio
        /// </summary>
        /// <param name="id"></param>


        public async void ServicioExtraerEmpresa(int pagina,int id)
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
                camposEspecificos = JsonConvert.DeserializeObject<List<DTOCampoEspecifico>>(items.ToString());
                if (Convert.ToBoolean(hasitems))
                {
                    camp_esp = camposEspecificos.Where(x => x.IdCampoAmplio == id).ToList();
                    if (camp_esp.Count()>0)
                    {
                        rptCampoespecifco.DataSource = camposEspecificos.ToList();
                        rptCampoespecifco.DataBind();
                    }
                }
            }

        }

    }
}