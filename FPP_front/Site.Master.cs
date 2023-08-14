
using FPP_front.DTOs;
using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Web.UI;

namespace Proyecto_Ingles_V2.Interfaces
{
    public partial class Site : System.Web.UI.MasterPage
    {
        static List<DTONotificacion> lista = new List<DTONotificacion>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Nombres"] = "";
            Session["Apellidos"] = "";
            
            if (Session["cp"] == null)
            {
                Response.Redirect("https://portaldocentes.uisek.edu.ec/");
            }
            else
            {
                int tipoProfesor = EsCoordinador(Session["cp"].ToString());
                Session["TipoProfesor"] = tipoProfesor;
                cargarrNotificaciones();
            }
        }
        public int EsCoordinador(string identificacion)
        {

            int permiso = 0;
            DataSet ds_aprobador = Conexion.BuscarPracticas_ds(" APROBADOR ", " top 1 * ", " where IDENTIFICACION_APROBADOR like '%" + identificacion + "'");
            if (ds_aprobador.Tables[0].Rows.Count > 0)//es un aprobador
            {
                permiso = 1;//es aprobador
                Session["Nombres"] = "USUARIO";
                Session["Apellidos"] = "APROBADOR";
            }
            else//es un coordinador
            {
                DataSet ds_coordinador = Conexion.BuscarPracticas_ds(" COORDINADOR ", " top 1 * ", " where IDENTIFICACIONCOORDINADOR like '%" + identificacion + "' and ACTIVOCOORDINADOR=1");
                if (ds_coordinador.Tables[0].Rows.Count > 0)
                {
                    permiso = 2;//es coordinador
                    Session["Nombres"] = "USUARIO";
                    Session["Apellidos"] = "COORDINADOR";
                }                
            }
            return permiso;
        }

        protected void btnSendSuggestion_Click(object sender, EventArgs e)
        {
        
        }  
        public void cargarrNotificaciones()
        {
            DataSet ds = Conexion.BuscarPracticas_ds("NOTIFICACION_PRACTICAS", "*", "where ACTIVONOTIFICACION=1 order by FECHAREGISTRONOTIFICACION desc");
            lblNotificaciones.Text = ds.Tables[0].Rows.Count.ToString();
            rptNotificacion.DataSource = ds.Tables[0];
            rptNotificacion.DataBind();
        }

    }
}