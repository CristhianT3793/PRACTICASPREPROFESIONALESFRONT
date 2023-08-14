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

namespace FPP_front
{
    public partial class IngresoFechas : System.Web.UI.Page
    {

        static conexionServicios cs = new conexionServicios();
        string url = cs.url.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargargridFpp();
                cargarCombo();
            }
              
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DTOFechas dtFechas = new DTOFechas();
            dtFechas.Codgrupo="";
            dtFechas.Idfacultad = null;
            dtFechas.Idtipofpp = Convert.ToInt64(ddlTipoFPP.SelectedValue);
            dtFechas.Descfpp = txtPeriodo.Text;
            dtFechas.fechainiciofpp = Convert.ToDateTime(txtFechaInicio.Text).Date;
            dtFechas.fechafinfpp = Convert.ToDateTime(txtFechaFin.Text).Date;

            ServicioInsertaFPP(dtFechas);
        }


        public async void ServicioInsertaFPP(DTOFechas per)
        {
            string uri = "Fpp";
            try
            {
                var myContent = JsonConvert.SerializeObject(per);
                var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.PostAsync(uri, stringContent);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    cargargridFpp();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async void cargarCombo()
        {
            JArray data = await getTipoFpp();
            ddlTipoFPP.DataSource = data;
            ddlTipoFPP.DataValueField = "Idtipofpp";
            ddlTipoFPP.DataTextField = "desctipofpp";
            ddlTipoFPP.DataBind();
        }
        public async Task<JArray> getTipoFpp()
        {
            JArray data=null;
            try
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("TipoFpp");
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                     data = (JArray)JObject.Parse(empResponse)["items"];
                    return data;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public async void cargargridFpp()
        {
            JArray data = await getFpp();
            dgvFechas.DataSource = data;
            dgvFechas.DataBind();
        }
        public async Task<JArray> getFpp()
        {
            JArray data = null;
            try
            {

                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("Fpp/fppTipofppjoin/1");
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    data = (JArray)JObject.Parse(empResponse)["items"];
                    return data;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return data;

        }


    }
}