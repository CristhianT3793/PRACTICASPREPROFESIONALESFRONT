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

namespace FPP_front
{
    public partial class pasantescoordinador : System.Web.UI.Page
    {
        static readonly Servicios con = new Servicios();
        static long idpasante = -1;
        static string identificacion = "";
        static string nombres = "";
        static string identificacioncoordinador = "";
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
                        identificacioncoordinador = Session["CedCoordinar"].ToString();//cedula remplazar con Session["CedCoordinar"]
                        cargargridPasante(1, identificacioncoordinador);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        public async void cargargridPasante(int pagina,string identificacioncoordinador)
        {
            string uri = "ConvenioEmpresaPasante/joinAll/page/" + pagina+"/"+identificacioncoordinador;
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
        protected void dgvPasante_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string identificacionTutorEmpresa = "", nombreempresa = "", codcarrera = "",carrera="", facultad = "", periodo="", emailcoordinador="";
            int idcampoespecifico = -1, horas = -1;
            string fInicio = "", fFinal = "";
            string carpetaExpediente = "";
            if (e.CommandName == "fppAlumno")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                idpasante = Convert.ToInt64(dgvPasante.DataKeys[fila].Values["IdPasante"].ToString());
                identificacion = dgvPasante.DataKeys[fila].Values["IdentificacionPasante"].ToString();
                nombres = dgvPasante.DataKeys[fila].Values["NombrePasante"].ToString().Trim() + " " + dgvPasante.DataKeys[fila].Values["ApellidoPasante"].ToString().Trim();
                identificacionTutorEmpresa = dgvPasante.DataKeys[fila].Values["IdentificacionTutorEmpresa"].ToString();
                nombreempresa = dgvPasante.DataKeys[fila].Values["NombreEmpresa"].ToString();
                codcarrera= dgvPasante.DataKeys[fila].Values["CodCarreraPasante"].ToString();
                carrera = dgvPasante.DataKeys[fila].Values["CarreraPasante"].ToString();
                facultad = dgvPasante.DataKeys[fila].Values["FacultadPasante"].ToString();
                idcampoespecifico = Convert.ToInt32(dgvPasante.DataKeys[fila].Values["IdCampoEspecifico"].ToString());
                horas = Convert.ToInt32(dgvPasante.DataKeys[fila].Values["NumeroHorasPasante"].ToString());
                fInicio = dgvPasante.DataKeys[fila].Values["FechaInicioPasante"].ToString();
                fFinal = dgvPasante.DataKeys[fila].Values["FechaFinPasante"].ToString();
                periodo = dgvPasante.DataKeys[fila].Values["PeriodoPasante"].ToString();
                emailcoordinador = dgvPasante.DataKeys[fila].Values["EmailTutor"].ToString();
                carpetaExpediente = dgvPasante.DataKeys[fila].Values["CarpetaPasanteExpediente"].ToString();
                crearlineafpp_1(periodo.Trim(), codcarrera.Trim(), identificacion.Trim(),idpasante);
                //crearlineafpp_2(periodo.Trim(), codcarrera.Trim(), identificacion.Trim(), idpasante);
                //crearlineafpp_7(periodo.Trim(), codcarrera.Trim(), identificacion.Trim(),idpasante);
                crearlineafpp_4(periodo.Trim(), codcarrera.Trim(), identificacion.Trim(), idpasante);

                ifrm.Src = "fppPasantesCoordinador.aspx?id=" + idpasante + "&nom=" + nombres.Trim() + "&cedula=" + identificacion.Trim() + "&cedulatutor=" + identificacionTutorEmpresa.Trim() + "&nomempresa=" + HttpUtility.HtmlDecode(nombreempresa.Trim().Replace("\"", "")) + "&carrera=" + carrera.Trim() + "&facultad=" + facultad.Trim() + "&idcampoespecifico=" + idcampoespecifico + "&horas=" + horas + "&finicio=" + HttpUtility.HtmlDecode(fInicio) + "&ffinal=" + HttpUtility.HtmlDecode(fFinal) + "&folder=" + carpetaExpediente.Trim();
                btnPopUp_ModalPopupExtender.Show();
            }
        }
        public void crearlineafpp_1(string periodo, string carrera, string identificaionEstudiante,long idpasante)
        {
            int idfpppasante = -1;
            string pathfpp1 = "";
            int idfppcarrera = -1;
            //actualizacion 
            //1 verificar si existe fpp en tabla de fpps del alumno
            //2 si no existe buscar en los fpp de carrera con la carrera y el periodo
            //3 si existe en fpp de carrera extraer path y actualizar la linea de path del fpp del alumno del punto 1
            DataSet ds_fpp = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join FPP_PASANTE t2 on t1.ID_PASANTE=t2.ID_PASANTE inner join PLANTILLA_FPP t3 on t2.ID_PLANTILLA=t3.ID_PLANTILLA", " t2.ID_FPP_PASANTE,t3.NOMRE_PLANTILLA ", " where t1.IDENTIFICACION_PASANTE='" + identificaionEstudiante + "' and t3.NOMRE_PLANTILLA='FPP1' and t2.ID_PASANTE="+idpasante);
            if (ds_fpp.Tables[0].Rows.Count > 0)
            {
                idfpppasante = Convert.ToInt32(ds_fpp.Tables[0].Rows[0]["ID_FPP_PASANTE"]);
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP1'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp1 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    Conexion.ActualizarPracticas("FPP_PASANTE", " FPP_PASANTE_PATH='" + pathfpp1.Trim() + "'", " where ID_FPP_PASANTE=" + idfpppasante);
                }
            }
            else
            {
                //creacion
                //4 si no existe en tabla de fpp alumno
                //5 buscar fpp de carrera extraer path 
                //6 insertar nuevo fpp 
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP1'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp1 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    idfppcarrera = Convert.ToInt32(ds_fppcarrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    Conexion.InsertarPracticas("[dbo].[FPP_PASANTE]", "ID_PASANTE,ID_ESTADO_FPP,ID_APROBADOR,ID_PLANTILLA,FPP_PASANTE_PATH,FPP_PASANTE_FECHA_SUBIDA", idpasante+","+2+","+1+","+idfppcarrera+",'"+pathfpp1.Trim()+"',"+"'"+DateTime.Now+"'");
                }
            }
        }
        public void crearlineafpp_4(string periodo, string carrera, string identificaionEstudiante, long idpasante)
        {
            int idfpppasante = -1;
            string pathfpp1 = "";
            int idfppcarrera = -1;
            //actualizacion 
            //1 verificar si existe fpp en tabla de fpps del alumno
            //2 si no existe buscar en los fpp de carrera con la carrera y el periodo
            //3 si existe en fpp de carrera extraer path y actualizar la linea de path del fpp del alumno del punto 1
            DataSet ds_fpp = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join FPP_PASANTE t2 on t1.ID_PASANTE=t2.ID_PASANTE inner join PLANTILLA_FPP t3 on t2.ID_PLANTILLA=t3.ID_PLANTILLA", " t2.ID_FPP_PASANTE,t3.NOMRE_PLANTILLA ", " where t1.IDENTIFICACION_PASANTE='" + identificaionEstudiante + "' and t3.NOMRE_PLANTILLA='FPP4' and t2.ID_PASANTE=" + idpasante);
            if (ds_fpp.Tables[0].Rows.Count > 0)
            {
                idfpppasante = Convert.ToInt32(ds_fpp.Tables[0].Rows[0]["ID_FPP_PASANTE"]);
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP4'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp1 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    Conexion.ActualizarPracticas("FPP_PASANTE", " FPP_PASANTE_PATH='" + pathfpp1.Trim() + "'", " where ID_FPP_PASANTE=" + idfpppasante);
                }
            }
            else
            {
                //creacion
                //4 si no existe en tabla de fpp alumno
                //5 buscar fpp de carrera extraer path 
                //6 insertar nuevo fpp 
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP4'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp1 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    idfppcarrera = Convert.ToInt32(ds_fppcarrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    Conexion.InsertarPracticas("[dbo].[FPP_PASANTE]", "ID_PASANTE,ID_ESTADO_FPP,ID_APROBADOR,ID_PLANTILLA,FPP_PASANTE_PATH,FPP_PASANTE_FECHA_SUBIDA", idpasante + "," + 2 + "," + 1 + "," + idfppcarrera + ",'" + pathfpp1.Trim() + "'," + "'" + DateTime.Now + "'");
                }
            }
        }
        public void crearlineafpp_7(string periodo, string carrera, string identificaionEstudiante,long idpasante)
        {
            int idfpppasante = -1;
            string pathfpp7 = "";
            int idfppcarrera = -1;
            //actualizacion 
            //1 verificar si existe fpp en tabla de fpps del alumno
            //2 si no existe buscar en los fpp de carrera con la carrera y el periodo
            //3 si existe en fpp de carrera extraer path y actualizar la linea de path del fpp del alumno del punto 1
            DataSet ds_fpp = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join FPP_PASANTE t2 on t1.ID_PASANTE=t2.ID_PASANTE inner join PLANTILLA_FPP t3 on t2.ID_PLANTILLA=t3.ID_PLANTILLA", " t2.ID_FPP_PASANTE,t3.NOMRE_PLANTILLA ", " where t1.IDENTIFICACION_PASANTE='" + identificaionEstudiante + "' and t3.NOMRE_PLANTILLA='FPP7' and t2.ID_PASANTE="+idpasante);
            if (ds_fpp.Tables[0].Rows.Count > 0)
            {
                idfpppasante = Convert.ToInt32(ds_fpp.Tables[0].Rows[0]["ID_FPP_PASANTE"]);
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP7'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp7 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    Conexion.ActualizarPracticas("FPP_PASANTE", " FPP_PASANTE_PATH='" + pathfpp7.Trim() + "'", " where ID_FPP_PASANTE=" + idfpppasante);
                }
            }
            else
            {
                //creacion
                //4 si no existe en tabla de fpp alumno 
                //5 buscar fpp de carrera extraer path 
                //6 insertar nuevo fpp 
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP7'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp7 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    idfppcarrera = Convert.ToInt32(ds_fppcarrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    Conexion.InsertarPracticas("[dbo].[FPP_PASANTE]", "ID_PASANTE,ID_ESTADO_FPP,ID_APROBADOR,ID_PLANTILLA,FPP_PASANTE_PATH,FPP_PASANTE_FECHA_SUBIDA", idpasante + "," + 2 + "," + 1 + "," + idfppcarrera + ",'" + pathfpp7.Trim() + "'," + "'" + DateTime.Now + "'");
                }
            }
        }
        public void crearlineafpp_2(string periodo, string carrera, string identificaionEstudiante, long idpasante)
        {
            int idfpppasante = -1;
            string pathfpp2 = "";
            int idfppcarrera = -1;
            //actualizacion 
            //1 verificar si existe fpp en tabla de fpps del alumno
            //2 si no existe buscar en los fpp de carrera con la carrera y el periodo
            //3 si existe en fpp de carrera extraer path y actualizar la linea de path del fpp del alumno del punto 1
            DataSet ds_fpp = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join FPP_PASANTE t2 on t1.ID_PASANTE=t2.ID_PASANTE inner join PLANTILLA_FPP t3 on t2.ID_PLANTILLA=t3.ID_PLANTILLA", " t2.ID_FPP_PASANTE,t3.NOMRE_PLANTILLA ", " where t1.IDENTIFICACION_PASANTE='" + identificaionEstudiante + "' and t3.NOMRE_PLANTILLA='FPP2' and t2.ID_PASANTE=" + idpasante);
            if (ds_fpp.Tables[0].Rows.Count > 0)
            {
                idfpppasante = Convert.ToInt32(ds_fpp.Tables[0].Rows[0]["ID_FPP_PASANTE"]);
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP2'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp2 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    Conexion.ActualizarPracticas("FPP_PASANTE", " FPP_PASANTE_PATH='" + pathfpp2.Trim() + "'", " where ID_FPP_PASANTE=" + idfpppasante);
                }
            }
            else
            {
                //creacion
                //4 si no existe en tabla de fpp alumno
                //5 buscar fpp de carrera extraer path 
                //6 insertar nuevo fpp 
                DataSet ds_fppcarrera = Conexion.BuscarPracticas_ds(" FPP_COORDINADOR ", " * ", " where CARRERA_FPP_COORDINADOR='" + carrera + "' and SEMESTRE_FPP_COORDINADOR='" + periodo + "' and NOM_PLATILLA_CORDINADOR='FPP2'");
                if (ds_fppcarrera.Tables[0].Rows.Count > 0)
                {
                    pathfpp2 = Convert.ToString(ds_fppcarrera.Tables[0].Rows[0]["PATH_FPP_COORDINADOR"]);
                    idfppcarrera = Convert.ToInt32(ds_fppcarrera.Tables[0].Rows[0]["ID_PLANTILLA"]);
                    Conexion.InsertarPracticas("[dbo].[FPP_PASANTE]", "ID_PASANTE,ID_ESTADO_FPP,ID_APROBADOR,ID_PLANTILLA,FPP_PASANTE_PATH,FPP_PASANTE_FECHA_SUBIDA", idpasante + "," + 2 + "," + 1 + "," + idfppcarrera + ",'" + pathfpp2.Trim() + "'," + "'" + DateTime.Now + "'");
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
                cargargridPasante(1,identificacioncoordinador);
            }
        }
        public async void cargargridPasantexparametros(string parametro, int pagina)//carga todos
        {
            string uri = "ConvenioEmpresaPasante/joinAllParametro/page/" + pagina + "/parametro=" + parametro;
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
                else
                {
                    dgvPasante.VirtualItemCount = Convert.ToInt32(total);
                    dgvPasante.DataSource = pasante_;
                    dgvPasante.DataBind();
                }
            }
        }

        protected void dgvPasante_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPasante.PageIndex = e.NewPageIndex;
            int page = e.NewPageIndex + 1;
            cargargridPasante(page,identificacioncoordinador);
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