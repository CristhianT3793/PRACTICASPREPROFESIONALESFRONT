using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PracticasPreProfesionales
{
    /// <summary>
    /// Descripción breve de WSFunciones
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WSFunciones : System.Web.Services.WebService
    {

        [WebMethod]
        public string BorrarEmpresa(string id)
        {
            string resultado = Conexion.DeleteUMAS("PPP_Empresas", "id='" + id + "'") ? "EXITO" : "No Existe";
            return resultado;
        }
        [WebMethod]
        public string ValidarEmpresa(string id)
        {
            string resultado = Conexion.BuscarUMAS_ds("PPP_Empresas", "*", "where Cod_Empresa='" + id + "'").Tables[0].Rows.Count == 0 ? "EXITO" : "No Existe";
            return resultado;
        }

        [WebMethod]
        public string[] BuscaEstudiante(string cedula)
        {
            string[] resultado = new string[9];
            DataSet ds = Conexion.BuscarUMAS_ds("SEK_inscripcion_asignaturas t1 inner join [NAV_UISEK_ECUADOR].dbo.Customer t2 on t1.Cedula = t2.[VAT Registration No_] or '0'+t1.Cedula=t2.[VAT Registration No_]", "top 1 t1.*,t2.Nombre,t2.[Apellido 1],t2.[Apellido 2]", " where ('0'+t1.cedula='" + cedula + "' or t1.cedula='" + cedula + "')  and baja is  null and CODCARR<>'QINS' and Centro<>'MAESTRIAS' order by t1.[año matricula] desc");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSet ds2 = Conexion.BuscarUMAS_ds("SEK_Facultad_Carrera", "distinct codfac_ant,facultad_ant,CODFAC_NEW", " where CODCARR='" + ds.Tables[0].Rows[0]["codcarr"].ToString() + "'");
                resultado[0] = ds.Tables[0].Rows[0]["ALUMNO"].ToString();
                resultado[1] = ds.Tables[0].Rows[0]["Facultad"].ToString();
                resultado[2] = ds2.Tables[0].Rows[0]["codfac_ant"].ToString() + " - " + ds2.Tables[0].Rows[0]["facultad_ant"].ToString();
                resultado[3] = ds.Tables[0].Rows[0]["codcarr"].ToString() + " - " + ds.Tables[0].Rows[0]["carrera"].ToString();
                resultado[4] = ds.Tables[0].Rows[0]["Nombre"].ToString();
                resultado[5] = ds.Tables[0].Rows[0]["Apellido 1"].ToString()+" "+ ds.Tables[0].Rows[0]["Apellido 2"].ToString();
                resultado[6] = ds.Tables[0].Rows[0]["codcarr"].ToString();
                resultado[7] = ds.Tables[0].Rows[0]["carrera"].ToString();
                resultado[8] = ds2.Tables[0].Rows[0]["CODFAC_NEW"].ToString();
            }
            else
                resultado[0] = "No Existe";

            return resultado;
        }

        [WebMethod]
        public void DesactivarNotificacion(int id)
        {
            
            bool notificacion = Conexion.ActualizarPracticas("NOTIFICACION_PRACTICAS", "ACTIVONOTIFICACION=0", "where IDNOTIFICACION="+id);
        }
        [WebMethod]
        public string[] BuscaDocente(string cedula)
        {
            DataSet coodirector = Conexion.BuscarUMAS_ds("matricula.RA_PROFES", "top 1 *", "where CodProf = '" + cedula + "' or CodProf = '0" + cedula + "'");
            string[] datosProfesor = new string[3];
            try
            {
                
                datosProfesor[0] = coodirector.Tables[0].Rows[0]["NOMBRES"].ToString();
                datosProfesor[1] = coodirector.Tables[0].Rows[0]["AP_PATER"].ToString() +" "+ coodirector.Tables[0].Rows[0]["AP_MATER"].ToString();
                datosProfesor[2] = coodirector.Tables[0].Rows[0]["EMAIL"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return datosProfesor;

        }
        [WebMethod]
        public string[] BuscarCoordinador(string cedula)
        {
            DataSet coordinador = Conexion.BuscarPracticas_ds("coordinador t1 inner join [MatriculaUmasEC].matricula.RA_PROFES t2 on t1.IDENTIFICACIONCOORDINADOR=t2.RUT collate Modern_Spanish_CI_AS or '0'+t1.IDENTIFICACIONCOORDINADOR=t2.RUT collate Modern_Spanish_CI_AS or t1.IDENTIFICACIONCOORDINADOR='0'+t2.RUT collate Modern_Spanish_CI_AS ", " t1.*,t2.EMAIL ", "where t1.IDENTIFICACIONCOORDINADOR='" + cedula + "' or t1.IDENTIFICACIONCOORDINADOR='0"+cedula+ "' or '0'+t1.IDENTIFICACIONCOORDINADOR='"+cedula+ "' and EMAIL<>'' ");
            //DataSet coordinador = Conexion.BuscarPracticas_ds("[COORDINADOR]", "*", "where IDENTIFICACIONCOORDINADOR='"+cedula+"'");//busqueda normal
            string[] datoscoordinador = new string[4];
            try
            {
                datoscoordinador[0] = coordinador.Tables[0].Rows[0]["NOMBRECOORDINADOR"].ToString();
                datoscoordinador[1] = coordinador.Tables[0].Rows[0]["APELLIDOCOORDINADOR"].ToString();
                datoscoordinador[2] = coordinador.Tables[0].Rows[0]["CARRERACOORDINADOR"].ToString();
                datoscoordinador[3] = coordinador.Tables[0].Rows[0]["EMAIL"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return datoscoordinador;
        }
        [WebMethod]
        public string EliminarSolicitud(string tracking)
        {
            string resultado;
            DataSet ds_solicitud = Conexion.BuscarUMAS_ds("PPP_Solicitud", "top 1 *", "where id_tracking = '" + tracking + "'");

            if (ds_solicitud.Tables[0].Rows.Count == 0)
                resultado = "No Existe";
            else
                resultado = Conexion.DeleteUMAS("PPP_Solicitud", "id_tracking = '" + tracking + "'") ? "Eliminado" : "No Eliminado";

            return resultado;
        }
    }
}
