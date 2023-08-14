using PracticasPreProfesionales.LoginDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace FPP_front
{
    /// <summary>
    /// Descripción breve de WSValidarPracticas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WSValidarPracticas : System.Web.Services.WebService
    {

        /// <summary>
        /// funcion que permite validar de que un usuario tiene practicas
        /// </summary>
        /// <returns>si aprobador o coordinador retornar 1,si no es ni coordinador ni aprobador retornar -1</returns>
        [WebMethod]
        public string validarPracticas(string cedula)
        {

            string resultado = "-1";   
            //primera validacion buscar si es aprobador
            DataSet ds_practicas = Conexion.BuscarPracticas_ds("[APROBADOR]", "*", "where (IDENTIFICACION_APROBADOR like '%" + cedula + "%' or IDENTIFICACION_APROBADOR like '0%" + cedula + "%' or '0'+IDENTIFICACION_APROBADOR like '%" + cedula + "%') and ACTIVO_APROBADOR=1");
            if (ds_practicas.Tables[0].Rows.Count > 0)
            {
                resultado = "1";
            }
            else
            {
                //segunda validacion buscar si es coordinador
                DataSet ds_coordinador = Conexion.BuscarPracticas_ds("[COORDINADOR]", "*", "where (IDENTIFICACIONCOORDINADOR like '%" + cedula + "%' or IDENTIFICACIONCOORDINADOR like '0%" + cedula + "%' or '0'+IDENTIFICACIONCOORDINADOR like '%" + cedula + "%') and ACTIVOCOORDINADOR=1");
                if (ds_coordinador.Tables[0].Rows.Count > 0)
                {
                    resultado = "1";
                }
                else
                {
                    resultado = "-1";
                }
            }
            return resultado;
        }
    }
}
