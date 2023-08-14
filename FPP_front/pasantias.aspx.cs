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
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace FPP_front
{
    public partial class pasantias : System.Web.UI.Page
    {
        //static string nombres = "";
        //static string apellidos = "";
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
                        cargarpasantes();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            string parametro = txtIdentificacion.Text;
            parametro = parametro.Replace(" ", "%");
            if (!string.IsNullOrEmpty(txtIdentificacion.Text.Trim()))
            {
                //si tiene algo para buscar
                cargarasantexparametros(parametro, 1);
            }
            else
            {
                //si no tiene nada
                cargarpasantes();
            }
        }



        protected void dgvPasante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "pasantiasalumno")
            {
                int fila = Convert.ToInt32(e.CommandArgument);
                LinkButton linkcedula = dgvPasante.Rows[fila].FindControl("lblCedula") as LinkButton;
                string cedula = linkcedula.Text;

                string identificacion= cedula;
                string nombres = dgvPasante.DataKeys[fila].Values["NOMBRE_PASANTE"].ToString();
                string apellidos = dgvPasante.DataKeys[fila].Values["APELLIDO_PASANTE"].ToString();
                string nombresCompletos = dgvPasante.DataKeys[fila].Values["NOMBRE_PASANTE"].ToString() + " " + dgvPasante.DataKeys[fila].Values["APELLIDO_PASANTE"].ToString();
                string carrera= dgvPasante.DataKeys[fila].Values["CARRERA_PASANTE"].ToString();
                string facultad= dgvPasante.DataKeys[fila].Values["FACULTAD_PASANTE"].ToString();
                string codcarrera= dgvPasante.DataKeys[fila].Values["COD_CARRERA_PASANTE"].ToString();
                //string codfacultad = dgvPasante.DataKeys[fila].Values["COD_FACULTAD_PASANTE"].ToString();
                cargarEncabezado(identificacion, nombresCompletos, carrera, facultad);
                cargarPasantiasEstudiante(identificacion);
                btnPopUp_ModalPopupExtender.Show();
            }
            if(e.CommandName == "Enviar")
            {
                

                string empresas = "";
                int totalHoras = 0;
                int fila = Convert.ToInt32(e.CommandArgument);
                string identificacion = dgvPasante.DataKeys[fila].Values["IDENTIFICACION_PASANTE"].ToString();
                string codcarrera = dgvPasante.DataKeys[fila].Values["COD_CARRERA_PASANTE"].ToString();
                string codfacultad = dgvPasante.DataKeys[fila].Values["COD_FACULTAD_PASANTE"].ToString();
                string carrera = dgvPasante.DataKeys[fila].Values["CARRERA_PASANTE"].ToString();
                string facultad = dgvPasante.DataKeys[fila].Values["FACULTAD_PASANTE"].ToString();
                string nombres = dgvPasante.DataKeys[fila].Values["NOMBRE_PASANTE"].ToString();
                string apellidos = dgvPasante.DataKeys[fila].Values["APELLIDO_PASANTE"].ToString();
                string codcli = "";
                string codAlumno = "", codcentro = "";
                string codcarr = "", codfac = "", plan = "";
                DateTime fechaInicio = new DateTime();
                DateTime fechaFin = new DateTime();
                //nuevo proceso
                //construirPDF(identificacion, carrera, facultad, nombres, apellidos);
                //valida si cumple el minimo de horas
                if (validarnumerohoras(identificacion, codfacultad, codcarrera))//si es verdad significa que cumple con el minimo de horas
                {
                    DataSet datos_pasantia = empresaspasante(identificacion);
                    DataSet ds_fechas = fechasPasantias(identificacion);
                    if (datos_pasantia.Tables[0].Rows.Count > 0 && ds_fechas.Tables[0].Rows.Count > 0)
                    {
                        fechaInicio = Convert.ToDateTime(ds_fechas.Tables[0].Rows[0]["FECHA_INICIO"]);
                        fechaFin = Convert.ToDateTime(ds_fechas.Tables[0].Rows[0]["FECHA_FIN"]);
                        empresas = datos_pasantia.Tables[0].Rows[0]["EMPRESAS"].ToString();
                        totalHoras = Convert.ToInt32(datos_pasantia.Tables[0].Rows[0]["TOTAL_HORAS"]);
                        DataSet ds_datosCursoEscolar = Conexion.BuscarNAV_ds("[NAV_UISEK_ECUADOR].[dbo].[Curso escolar alumno]", " top 1 [CODCLIU+],[Cód_ Curso Escolar],[Cód_ Alumno]", " where (cedula = '" + identificacion + "' or '0'+cedula='" + identificacion + "' or Cedula='0'+'"+identificacion+"') and CodCentro not in (65,125)  order by [Cód_ Curso Escolar] desc");
                        if (ds_datosCursoEscolar.Tables[0].Rows.Count > 0)
                        {
                            codcli = ds_datosCursoEscolar.Tables[0].Rows[0]["CODCLIU+"].ToString();
                            codAlumno = ds_datosCursoEscolar.Tables[0].Rows[0]["Cód_ Alumno"].ToString();
                            //busca con el codigo cliente umas
                            if (!string.IsNullOrEmpty(codcli) || !string.IsNullOrEmpty(codAlumno))
                            {
                                DataSet ds_egresado = Conexion.BuscarNAV_ds("Egresados", "top 1 *", "where [CodCliU+]='" + codcli + "'");
                                if (ds_egresado.Tables[0].Rows.Count > 0)
                                {
                                    //actualiza
                                    bool correcto = Conexion.ActualizarNAV("Egresados", "[Pasantia Horas]=" + totalHoras + ",[Pasantia Empresas]='" + empresas + "',[Pasantia Inicio]='" + fechaInicio + "',[Pasantia Final]='" + fechaFin + "'", " where [CodCliU+]='" + codcli + "'");
                                    if (correcto)
                                    {
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('success','Datos enviados correctamente a Registro Académico'); ", true);
                                        Conexion.ActualizarPracticas("PASANTE", "ENVIADO_REGISTRO=1", "WHERE IDENTIFICACION_PASANTE='" + identificacion + "'");
                                        construirPDF(identificacion, carrera, facultad, nombres, apellidos);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo enviar los datos a Registro Académico'); ", true);
                                    }
                                }
                                else
                                {
                                    //busca con el codigo de alumno
                                    if (!string.IsNullOrEmpty(codAlumno))
                                    {
                                        ds_egresado = Conexion.BuscarNAV_ds("Egresados", "top 1 *", "where [CodAlumno]='" + codAlumno + "'");
                                        if (ds_egresado.Tables[0].Rows.Count > 0)
                                        {
                                            //actualiza
                                            bool correcto = Conexion.ActualizarNAV("Egresados", "[Pasantia Horas]=" + totalHoras + ",[Pasantia Empresas]='" + empresas + "',[Pasantia Inicio]='" + fechaInicio + "',[Pasantia Final]='" + fechaFin + "'", " where [CodAlumno]='" + codAlumno + "'");
                                            if (correcto)
                                            {
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('success','Datos enviados correctamente a Registro Académico'); ", true);
                                                Conexion.ActualizarPracticas("PASANTE", "ENVIADO_REGISTRO=1", "WHERE IDENTIFICACION_PASANTE='" + identificacion + "'");
                                                construirPDF(identificacion, carrera, facultad, nombres, apellidos);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No se pudo enviar los datos a Registro Académico'); ", true);
                                            }
                                        }
                                        else
                                        {
                                            //inserta ya que no encuentra ni con el cod cli ni con el codalumno
                                            //validamos que existe el codcli+ o el codalumno
                                            DataSet ds_carrera = Conexion.BuscarNAV_ds("[Curso escolar alumno] t1 inner join Curso t2 on t1.[Cód_ Curso]=t2.[Cód_ curso]", " top 1 t2.[CarreraU+] CODCARR,t2.[MallaU+] PLANESTUDIO,t2.CodCentro CODCENTRO", " where ( t1.Cedula = '" + identificacion + "' or t1.Cedula = '0" + identificacion + "' or '0'+t1.Cedula = '" + identificacion + "') and t1.CodCentro not in (65,125) order by [Cód_ Curso Escolar] desc");
                                            if (ds_carrera.Tables[0].Rows.Count > 0)
                                            {
                                                codcarr = ds_carrera.Tables[0].Rows[0]["CODCARR"].ToString();
                                                plan = ds_carrera.Tables[0].Rows[0]["PLANESTUDIO"].ToString();
                                                codcentro = ds_carrera.Tables[0].Rows[0]["CODCENTRO"].ToString();
                                                DataSet ds_facultad = Conexion.BuscarUMAS_ds("SEK_Facultad_Carrera", "CODFAC_NEW", "where CODCARR='" + codcarr + "'");
                                                codfac = ds_facultad.Tables[0].Rows[0]["CODFAC_NEW"].ToString();
                                                bool correcto = insertarEgresados(codAlumno, codcli, fechaInicio, fechaFin, totalHoras, empresas, codcarrera, plan, codcentro, codfac);
                                                if (correcto)
                                                {
                                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('success','Datos enviados correctamente a Registro Académico'); ", true);
                                                    Conexion.ActualizarPracticas("PASANTE", "ENVIADO_REGISTRO=1", "WHERE IDENTIFICACION_PASANTE='" + identificacion + "'");

                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudieron enviar los datos a registro académico'); ", true);
                                                }

                                            }
                                            else
                                            {
                                                //termina
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudieron enviar los datos a registro académico.'); ", true);


                                            }
                                        }
                                    }
                                    else
                                    {
                                        //termina
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudieron enviar los datos a registro académico.'); ", true);

                                    }
                                }

                            }
                            else
                            {
                                //termina
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudieron enviar los datos a registro académico.'); ", true);

                            }
                        }
                        else
                        {
                            //termina
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No existe datos en curso escolar alumno'); ", true);

                        }
                    }
                    else
                    {
                        //termina
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('error','No se pudo completar la solicitud'); ", true);

                    }
                }
                else
                {
                    //termina
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", @"alertaParametro('info','No cumple con el mínimo de horas de pasantías para que los datos sean enviados a Registro Académico '); ", true);
                }
            }
        }
        public bool insertarEgresados(string codAlumno,string codcli,DateTime fechaInicio,DateTime fechaFin,int totalHoras,string empresas,string codcarrera,string plan,string codcentro,string codfac)
        {
            
           bool correcto = Conexion.InsertarNAV("[Egresados]", "CodAlumno," +
                            "[CodCliU+]," +
                            "Malla," +
                            "[Pasantia Inicio]," +
                            "[Pasantia Final]," +
                            "[Pasantia Horas]," +
                            "[Pasantia Empresas]," +
                            "Ingles," +
                            "[Comunidad Inicio]," +
                            "[Comunidad Final]," +
                            "[Comunidad Horas]," +
                            "[Comunidad Proyectos]," +
                            "Semestre," +
                            "Observaciones," +
                            "[Fecha Cumplimiento]," +
                            "[Proyecto Tesis]," +
                            "Tutor," +
                            "[Fecha Aprobacion Plan Tesis]," +
                            "[Nota Grado]," +
                            "[Fecha Grado]," +
                            "Graduado," +
                            "_Carrera," +
                            "_PlanEstudios," +
                            "_Centro," +
                            "_Facultad," +
                            "[Inserted Date]," +
                            "[Modified date]," +
                            "[Inserted User]," +
                            "[Modified User]," +
                            "[Codigo Referenciacion]," +
                            "[Nota Carrera]",
                            "'" + codAlumno + "'," +
                            "'" + codcli + "'," +
                            0 +
                            ",'" + fechaInicio + "'," +
                            "'" + fechaFin + "'," +
                            totalHoras +
                            ",'" + empresas + "'," +
                            "'1753-01-01'," +
                            "'1753-01-01'," +
                            "'1753-01-01'," +
                            0 +
                            ",''," +
                            "''," +
                            "''," +
                            "'1753-01-01'," +
                            "''," +
                            "''," +
                            "'1753-01-01'," +
                            0 +
                            ",'1753-01-01'," +
                            0 +
                            ",'" + codcarrera + "'," +
                            "'" + plan + "'," +
                            "'" + codcentro + "'," +
                            "'" + codfac + "'," +
                            "'" + DateTime.Now.Date + "'," +
                            "'" + DateTime.Now.Date + "'," +
                            "''," +
                            "''," +
                            "''," +
                            0
                            );
            return correcto;
        }
        public DataSet fechasPasantias(string identificacion)
        {
            DataSet ds_datos = Conexion.BuscarPracticas_ds(" PASANTE ", " min(FECHA_INICIO_PASANTE) FECHA_INICIO,MAX(FECHA_FIN_PASANTE) FECHA_FIN ", " where  IDENTIFICACION_PASANTE LIKE '%"+identificacion+"' and ESTADO_APROBADO=2");
            return ds_datos;
        }

        public DataSet empresaspasante(string identificacion)
        {
            DataSet ds_datos = Conexion.BuscarPracticas_ds(" PASANTE ", " IDENTIFICACION_PASANTE,STUFF((SELECT ',' + rtrim(ltrim(t3.NOMBRE_EMPRESA)) FROM PASANTE t1 inner join EMPRESA_CONVENIO_TUTOR t2 on t2.ID_PASANTE = t1.ID_PASANTE inner join EMPRESA t3 on t3.ID_EMPRESA = t2.ID_EMPRESA where t1.IDENTIFICACION_PASANTE LIKE'%" + identificacion + "' and t1.ACTIVO_PASANTE = 1 and t1.ESTADO_APROBADO = 2 FOR XML PATH('')),1,1, '') EMPRESAS, sum(NUMERO_HORAS_PASANTE) TOTAL_HORAS ", " where  IDENTIFICACION_PASANTE LIKE '%" + identificacion + "' and ESTADO_APROBADO=2 group by IDENTIFICACION_PASANTE");
            return ds_datos;
        }


        public int totalhorasparametro(string codfacultad,string codcarrera)
        {
            //busca todos
            int totalhorasParametro = 0;
            DataSet ds_facultadCarrera = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD=t2.ID_MODALIDAD", " t2.CARRERA,t2.FACULTAD,t1.MAX_HORAS_PARAMETRO,t1.ACTIVO_PARAMETRO", " where CARRERA='" + codcarrera + "' and FACULTAD='" + codfacultad + "'");
            if (ds_facultadCarrera.Tables[0].Rows.Count > 0)//busca con facultad y carrera
            {
                totalhorasParametro = Convert.ToInt32(ds_facultadCarrera.Tables[0].Rows[0]["MAX_HORAS_PARAMETRO"]);
            }
            else
            {   //busca con facultad
                DataSet ds_facultad = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD=t2.ID_MODALIDAD", " t2.CARRERA,t2.FACULTAD,t1.MAX_HORAS_PARAMETRO,t1.ACTIVO_PARAMETRO", " where FACULTAD='" + codfacultad + "'");
                if (ds_facultad.Tables[0].Rows.Count > 0)//busca con facultad y carrera
                {
                    totalhorasParametro = Convert.ToInt32(ds_facultad.Tables[0].Rows[0]["MAX_HORAS_PARAMETRO"]);
                }
                else//busca con carrera
                {
                    DataSet ds_carrera = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD=t2.ID_MODALIDAD", " t2.CARRERA,t2.FACULTAD,t1.MAX_HORAS_PARAMETRO,t1.ACTIVO_PARAMETRO", " where CARRERA='" + codcarrera + "'");
                    if (ds_carrera.Tables[0].Rows.Count > 0)
                    {
                        totalhorasParametro = Convert.ToInt32(ds_carrera.Tables[0].Rows[0]["MAX_HORAS_PARAMETRO"]);
                    }
                    else
                    {
                        DataSet ds_todos = Conexion.BuscarPracticas_ds("PARAMETRO t1 inner join ESTRUCTURA_ACADEMICA t2 on t1.ID_MODALIDAD=t2.ID_MODALIDAD", " t2.CARRERA,t2.FACULTAD,t1.MAX_HORAS_PARAMETRO,t1.ACTIVO_PARAMETRO", " where CARRERA='TODOS' AND FACULTAD='TODOS'");
                        if(ds_todos.Tables[0].Rows.Count > 0)
                        {
                            totalhorasParametro = Convert.ToInt32(ds_todos.Tables[0].Rows[0]["MAX_HORAS_PARAMETRO"]);
                        }
                    }

                }

            }
            return totalhorasParametro;
        }

        public bool validarnumerohoras(string identificacionpasante, string codfacultad,string codcarrera)
        {
            DataSet ds_totalHoras = Conexion.BuscarPracticas_ds("PASANTE", " isnull(sum(NUMERO_HORAS_PASANTE),0) TotalHoras ", " where (IDENTIFICACION_PASANTE='"+ identificacionpasante + "' or IDENTIFICACION_PASANTE='0"+ identificacionpasante + "' or '0'+IDENTIFICACION_PASANTE='"+ identificacionpasante + "')  and ESTADO_APROBADO=2 and COD_CARRERA_PASANTE='"+codcarrera+"' ");
            int totalhoraspasante = 0;
            int totalhorasparametro_ = 0;
            if (ds_totalHoras.Tables[0].Rows.Count > 0)
            {
                totalhoraspasante = Convert.ToInt32(ds_totalHoras.Tables[0].Rows[0]["TotalHoras"]);
            }
            totalhorasparametro_ = totalhorasparametro(codfacultad, codcarrera);
            if(totalhoraspasante>= totalhorasparametro_)
            {
                return true;
            }
            else
            {
                return false;
            }
            //Extraer horas minimas para el paso a egresados

        }
        //public void cargarpasantes()
        //{
        //    DataSet ds_pasantes = Conexion.BuscarPracticas_ds(" PASANTE ", " distinct(IDENTIFICACION_PASANTE),NOMBRE_PASANTE,APELLIDO_PASANTE,CARRERA_PASANTE,FACULTAD_PASANTE,COD_CARRERA_PASANTE,COD_FACULTAD_PASANTE,ENVIADO_REGISTRO", " order by APELLIDO_PASANTE asc");
        //    dgvPasante.DataSource = ds_pasantes.Tables[0];
        //    dgvPasante.DataBind();
        //}
        public void cargarpasantes()
        {
            DataSet ds_pasantes = Conexion.BuscarPracticas_ds("(select IDENTIFICACION_PASANTE,NOMBRE_PASANTE,CARRERA_PASANTE,FACULTAD_PASANTE,COD_CARRERA_PASANTE,APELLIDO_PASANTE,sum(NUMERO_HORAS_PASANTE) TOTAL_HORAS,"+ 
            "EMPRESAS = STUFF((SELECT ',' + LTRIM(RTRIM(NOMBRE_EMPRESA)) "+
            "FROM  PASANTE t1 inner join[EMPRESA_CONVENIO_TUTOR] t2 on t1.ID_PASANTE = t2.ID_PASANTE "+
            "inner join EMPRESA t3 on t2.ID_EMPRESA = t3.ID_EMPRESA where t1.IDENTIFICACION_PASANTE = A.IDENTIFICACION_PASANTE "+
            "FOR XML PATH('')), 1, 1, ''), A.ENVIADO_REGISTRO,COD_FACULTAD_PASANTE " +
            "from[dbo].[PASANTE] A where A.ESTADO_APROBADO = 2 "+
            "group by A.IDENTIFICACION_PASANTE,A.NOMBRE_PASANTE,A.APELLIDO_PASANTE,CARRERA_PASANTE,COD_FACULTAD_PASANTE,COD_CARRERA_PASANTE,FACULTAD_PASANTE,ENVIADO_REGISTRO)B", "*", "where B.TOTAL_HORAS >=(select top 1 min(MAX_HORAS_PARAMETRO) from PARAMETRO) or B.TOTAL_HORAS >=(select top 1 max(MAX_HORAS_PARAMETRO) from PARAMETRO)");

            dgvPasante.DataSource = ds_pasantes.Tables[0];
            dgvPasante.DataBind();
        }
        public void cargarasantexparametros(string parametro,int pagina)
        {
            //IDENTIFICACION_PASANTE like '%" + parametro+ "%' or NOMBRE_PASANTE like '%" + parametro + "%' or APELLIDO_PASANTE like '%" + parametro + "%' or (APELLIDO_PASANTE+' '+NOMBRE_PASANTE) like '%" + parametro + "%' or (NOMBRE_PASANTE+' '+APELLIDO_PASANTE) like '%" + parametro + "%'
            DataSet ds_pasantes = Conexion.BuscarPracticas_ds("(select IDENTIFICACION_PASANTE,NOMBRE_PASANTE,CARRERA_PASANTE,FACULTAD_PASANTE,COD_CARRERA_PASANTE,APELLIDO_PASANTE,sum(NUMERO_HORAS_PASANTE) TOTAL_HORAS," +
            "EMPRESAS = STUFF((SELECT ',' + LTRIM(RTRIM(NOMBRE_EMPRESA)) " +
            "FROM  PASANTE t1 inner join[EMPRESA_CONVENIO_TUTOR] t2 on t1.ID_PASANTE = t2.ID_PASANTE " +
            "inner join EMPRESA t3 on t2.ID_EMPRESA = t3.ID_EMPRESA where t1.IDENTIFICACION_PASANTE = A.IDENTIFICACION_PASANTE " +
            "FOR XML PATH('')), 1, 1, ''), A.ENVIADO_REGISTRO,COD_FACULTAD_PASANTE " +
            "from[dbo].[PASANTE] A where A.ESTADO_APROBADO = 2 " +
            "group by A.IDENTIFICACION_PASANTE,A.NOMBRE_PASANTE,A.APELLIDO_PASANTE,CARRERA_PASANTE,COD_FACULTAD_PASANTE,COD_CARRERA_PASANTE,FACULTAD_PASANTE,ENVIADO_REGISTRO)B", "*", "where IDENTIFICACION_PASANTE like '%" + parametro + "%' or NOMBRE_PASANTE like '%" + parametro + "%' or APELLIDO_PASANTE like '%" + parametro + "%' or (APELLIDO_PASANTE+' '+NOMBRE_PASANTE) like '%" + parametro + "%' or (NOMBRE_PASANTE+' '+APELLIDO_PASANTE) like '%" + parametro + "%' and  (B.TOTAL_HORAS >=(select top 1 min(MAX_HORAS_PARAMETRO) from PARAMETRO) or B.TOTAL_HORAS >=(select top 1 max(MAX_HORAS_PARAMETRO) from PARAMETRO))");

            dgvPasante.DataSource = ds_pasantes.Tables[0];
            dgvPasante.DataBind();
        }
        public void cargarPasantiasEstudiante(string identificacion)
        {

            int totalHoras = 0;
            DataSet ds_pasantias = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join EMPRESA_CONVENIO_TUTOR t2 on t1.ID_PASANTE = t2.ID_PASANTE inner join EMPRESA t3 on t2.ID_EMPRESA=t3.ID_EMPRESA ", " t3.NOMBRE_EMPRESA,t1.IDENTIFICACION_PASANTE,rtrim(t1.APELLIDO_PASANTE)+' '+t1.NOMBRE_PASANTE NOMBRES,t1.FECHA_INICIO_PASANTE,t1.FECHA_FIN_PASANTE,t1.NUMERO_HORAS_PASANTE,CASE t1.ESTADO_APROBADO WHEN 1 THEN 'EN PROCESO DE APROBACIÓN' WHEN 2 THEN 'APROBADA' END AS ESTADO", " where t1.IDENTIFICACION_PASANTE='" + identificacion + "'");
            TableHeaderRow RowTableHead = new TableHeaderRow();
            RowTableHead.TableSection = TableRowSection.TableHeader;
            string[] encabezados = new string[] { "#", "Nombre Empresa", "Fecha Inicio", "Fecha Fin", "N° Horas","Estado" };
            if (ds_pasantias.Tables[0].Rows.Count > 0)
            {
                //crear tabla 
                System.Web.UI.WebControls.Table tblpasantias = new System.Web.UI.WebControls.Table();
                foreach (var y in encabezados)
                {

                    TableHeaderCell CellHead = new TableHeaderCell
                    {
                        ForeColor = System.Drawing.Color.White,
                        BackColor = System.Drawing.Color.FromArgb(8, 83, 148),

                        Text = "<b><center>" + y + "</center></b>"

                    };
                    RowTableHead.Cells.Add(CellHead);
                }
                grd_Valores.Rows.Add(RowTableHead);
                for (int i = 0; i < ds_pasantias.Tables[0].Rows.Count; i++)
                {
                    TableRow RowTable = new TableRow();
                    TableCell Cell_Num = new TableCell
                    {

                        HorizontalAlign = HorizontalAlign.Center,
                        Text = (i + 1).ToString()
                    };
                    TableCell Cell_nombreEmpresa = new TableCell
                    {

                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["NOMBRE_EMPRESA"].ToString()
                    };
                    TableCell Cell_FechaInicio = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["FECHA_INICIO_PASANTE"].ToString()
                    };
                    TableCell Cell_FechaFin = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["FECHA_FIN_PASANTE"].ToString()
                    };
                    TableCell Cell_NumeroHoras = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["NUMERO_HORAS_PASANTE"].ToString()
                    };
                    TableCell Cell_Estado = new TableCell
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        Text = ds_pasantias.Tables[0].Rows[i]["ESTADO"].ToString()
                    };

                    RowTable.Cells.Add(Cell_Num);
                    RowTable.Cells.Add(Cell_nombreEmpresa);
                    RowTable.Cells.Add(Cell_FechaInicio);
                    RowTable.Cells.Add(Cell_FechaFin);
                    RowTable.Cells.Add(Cell_NumeroHoras);
                    RowTable.Cells.Add(Cell_Estado);
                    grd_Valores.Rows.Add(RowTable);
                    totalHoras += Convert.ToInt32(ds_pasantias.Tables[0].Rows[i]["NUMERO_HORAS_PASANTE"]);
                }




                //TableRow RowTable_TotalHoras = new TableRow();
                TableFooterRow tblfooter = new TableFooterRow();
                TableCell Cell_TotalH1 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalH2 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalH3 = new TableCell
                {
                    Text = " ",
                    HorizontalAlign = HorizontalAlign.Right
                };
                TableCell Cell_TotalHoras = new TableCell
                {
                    ForeColor = System.Drawing.Color.White,
                    Text = "<b>Total de Horas</b>",
                    BackColor = System.Drawing.Color.FromArgb(235, 162, 25),
                    HorizontalAlign = HorizontalAlign.Center
                };
                TableCell Cell_TotalHorasP = new TableCell
                {

                    ForeColor = System.Drawing.Color.White,
                    Text = "<b>" + totalHoras.ToString() + "</b>",
                    BackColor = System.Drawing.Color.FromArgb(235, 162, 25),
                    HorizontalAlign = HorizontalAlign.Center
                };

                tblfooter.Cells.Add(Cell_TotalH1);
                tblfooter.Cells.Add(Cell_TotalH2);
                tblfooter.Cells.Add(Cell_TotalH3);
                tblfooter.Cells.Add(Cell_TotalHoras);
                tblfooter.Cells.Add(Cell_TotalHorasP);
                grd_Valores.Rows.Add(tblfooter);

            }
        }
        public void cargarEncabezado(string identificacion,string nombres,string carrera,string facultad)
        {
            lblNombresAlumno.Text = nombres;
            lblIdentificacion.Text = identificacion;
            lblcarrera.Text = carrera;
            lblFacultad.Text = facultad;
        }

        protected void btnGenerarInforme_Click(object sender, EventArgs e)
        {
           // construirPDF(lblIdentificacion.Text);
        }
        /// <summary>
        /// obtiene el revisor de prácticas 
        /// </summary>
        /// <returns></returns>
        public string obtenerRevisor() {
            string nombresrevisor = "";
            DataSet ds_revisor = Conexion.BuscarPracticas_ds("APROBADOR", "top 1 * ", "where ACTIVO_APROBADOR=1");
            if (ds_revisor.Tables[0].Rows.Count > 0)
            {
                nombresrevisor = ds_revisor.Tables[0].Rows[0]["NOMBRE_APROBADOR"].ToString().Trim() + " " + ds_revisor.Tables[0].Rows[0]["APELLIDO_APROBADOR"].ToString().Trim();
            }
            return nombresrevisor;
        }

        public void construirPDF(string identificacion,string carrera_,string facultad_,string nombres,string apellidos)
        {
            string nombrerevisor = obtenerRevisor();
            Document doc = new iTextSharp.text.Document(PageSize.A4, 20f, 20f, 10f, 0f);
            PdfWriter pdfw;
            PdfContentByte cb;
            
            string facultad = facultad_;
            string carrera = carrera_;
            string nombres_ = nombres;
            string apellidos_ = apellidos;
            string nombreDocumento = identificacion.Trim() + "_INFORME_PASANTIAS.pdf";
            try
            {
                string titulo = "INFORME DE PASANTÍAS";
                string detalleHoras = "DETALLE DE HORAS";
                string filename = OnServerPath() + ("/" + nombreDocumento);
                pdfw = PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None));
                doc.Open();
                //tamaño y tipo de fuente tablas
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font fontH1 = new Font(bfTimes, 10, Font.NORMAL);
                //tamaño y tipo de fuente titulos
                BaseFont bfTimes2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font fontH2 = new Font(bfTimes, 10, Font.BOLD);

                BaseFont bfTimes4 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font fontH4 = new Font(bfTimes, 10, Font.UNDERLINE);
                //tamaño y tipo de fuente subtitulos
                BaseFont bfTimes3 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font fontH3 = new Font(bfTimes, 12, Font.NORMAL);
                //imagen
                string host = HttpContext.Current.Request.Url.Host.ToLower();
                string path = "";
                if (host == "localhost")
                    path = Server.MapPath("/images/");
                else
                    path = Server.MapPath("/images/");
                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(path + "logo.png");
                image1.ScaleAbsoluteWidth(110);
                image1.ScaleAbsoluteHeight(50);
                image1.GetRight(100);
                image1.Alignment = Element.ALIGN_RIGHT;
                doc.Add(image1);
                //titulo
                Paragraph title = new Paragraph();
                title.Alignment = Element.ALIGN_LEFT;
                title.Font = FontFactory.GetFont("Arial",BaseFont.WINANSI,BaseFont.EMBEDDED,13, Font.BOLD);
                title.Add(titulo);
                doc.Add(title);
                //fin titulo
                doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("FECHA: ", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                doc.Add(new Phrase(DateTime.Now.Date.ToString("dd-MM-yyyy") + "\n", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                doc.Add(new Phrase("\n"));
                Paragraph titleFacultad = new Paragraph();
                titleFacultad.Alignment = Element.ALIGN_CENTER;
                titleFacultad.Font = FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 12, Font.BOLD);
                titleFacultad.Add(facultad);
                doc.Add(titleFacultad);

                PdfPTable tablecarrera = new PdfPTable(4);
                Rectangle rect_0 = new Rectangle(80f, 80f);
                tablecarrera.SetWidthPercentage(new float[] { 15, 25,15,25 }, rect_0);
                PdfPCell cell0 = new PdfPCell(new Phrase("CARRERA:", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                PdfPCell cell1 = new PdfPCell(new Phrase(carrera, FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                PdfPCell cell2 = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                PdfPCell cell3 = new PdfPCell(new Phrase("", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));

               
                PdfPTable tablenombres = new PdfPTable(4);
                Rectangle rect_ = new Rectangle(80f, 80f);
                tablenombres.SetWidthPercentage(new float[] { 15, 25,15,25}, rect_);

                PdfPCell cell4 = new PdfPCell(new Phrase("APELLIDOS:", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                PdfPCell cell5 = new PdfPCell(new Phrase(apellidos_, FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));

                PdfPCell cell6 = new PdfPCell(new Phrase("NOMBRES:", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                PdfPCell cell7 = new PdfPCell(new Phrase(nombres_, FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                
                cell0.Border = 0;
                cell1.Border = 0;
                cell2.Border = 0;
                cell3.Border = 0;
                cell4.Border = 0;
                cell5.Border = 0;
                cell6.Border = 0;
                cell7.Border = 0;

                tablecarrera.AddCell(cell0);
                tablecarrera.AddCell(cell1);
                tablecarrera.AddCell(cell2);
                tablecarrera.AddCell(cell3);
                tablenombres.AddCell(cell4);
                tablenombres.AddCell(cell5);
                tablenombres.AddCell(cell6);
                tablenombres.AddCell(cell7);
                doc.Add(tablecarrera);           
                doc.Add(tablenombres);
                doc.Add(new Phrase("\n"));
                Paragraph detalleHoras_ = new Paragraph();
                detalleHoras_.Alignment = Element.ALIGN_LEFT;
                detalleHoras_.Font = FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 12, Font.BOLD);
                detalleHoras_.Add(detalleHoras);
                doc.Add(detalleHoras_);
                doc.Add(new Phrase("\n"));
                //tabla detalle de horas
                DataSet ds_detalleHoras = datosdetallehoras(identificacion);
                PdfPTable tabletemas = new PdfPTable(5);
                Rectangle rect = new Rectangle(115f, 115f);
                tabletemas.SetWidthPercentage(new float[] { 15, 15, 20,32,32 }, rect);
                tabletemas.HorizontalAlignment = Element.ALIGN_CENTER;
                tabletemas.AddCell(new PdfPCell(new Phrase("FECHA DE INICIO DE LA PASANTÍA", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                tabletemas.AddCell(new PdfPCell(new Phrase("FECHA FIN DE LA PASANTÍA", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                tabletemas.AddCell(new PdfPCell(new Phrase("NÚMERO DE HORAS", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                tabletemas.AddCell(new PdfPCell(new Phrase("EMPRESA", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                tabletemas.AddCell(new PdfPCell(new Phrase("RESPONSABLE DEL MONITOREO DE LAS PASANTÍAS", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                int sumatotalhoras = 0;
                int contadorregistros = 1;
                if (ds_detalleHoras.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_detalleHoras.Tables[0].Rows)
                    {
                        tabletemas.AddCell(new PdfPCell(new Phrase(Convert.ToDateTime(dr["FECHA_INICIO_PASANTE"]).Date.ToString("dd-MM-yyyy"), FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        
                        
                        tabletemas.AddCell(new PdfPCell(new Phrase(Convert.ToDateTime(dr["FECHA_FIN_PASANTE"]).Date.ToString("dd-MM-yyyy").ToString(), FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tabletemas.AddCell(new PdfPCell(new Phrase(dr["NUMERO_HORAS_PASANTE"].ToString(), FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tabletemas.AddCell(new PdfPCell(new Phrase(dr["NOMBRE_EMPRESA"].ToString().ToUpper(), FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tabletemas.AddCell(new PdfPCell(new Phrase(dr["RESPONSABLE"].ToString(), FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        sumatotalhoras += Convert.ToInt32(dr["NUMERO_HORAS_PASANTE"]);
                        contadorregistros++;
                    }

                }
                doc.Add(tabletemas);
                int saltolinea = 15 - contadorregistros;
                for(int i=0;i<saltolinea;i++)
                    doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("Total horas pasantías: ", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                doc.Add(new Phrase(sumatotalhoras + "\n", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                doc.Add(new Phrase("Nota: ", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.BOLD)));
                doc.Add(new Phrase("La Unidad Académica deberá adjuntar la siguiente documentación de respaldo." + "\n", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                doc.Add(new Phrase("- Solicitud hecha por la UISEK para la o las pasantías." + "\n", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                doc.Add(new Phrase("- Certificado de evaluación de la o las empresas." + "\n", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("\n"));
                PdfPTable tblfooter_ = new PdfPTable(3);
                Rectangle rect_f = new Rectangle(115f, 115f);
                tblfooter_.SetWidthPercentage(new float[] { 35,35,35 }, rect_f);
                PdfPCell cellf3 = new PdfPCell(new Phrase(nombrerevisor + "\n" + "________________________\n\n" + "REVISOR DE PRÁCTICAS\n"+"Aceptado Electrónicamente:"+DateTime.Now, FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL))) { 
                HorizontalAlignment =Element.ALIGN_CENTER
                };
                PdfPCell cellf4 = new PdfPCell(new Phrase(" "+ "\n"+" ", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL)));
                PdfPCell cellf5 = new PdfPCell(new Phrase("\n" + "________________________\n\n" + "SECRETARÍA ACADÉMICA", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, Font.NORMAL))) {
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                cellf3.Border = 0;
                cellf4.Border = 0;
                cellf5.Border = 0;
                tblfooter_.AddCell(cellf3);
                tblfooter_.AddCell(cellf4);
                tblfooter_.AddCell(cellf5);
                doc.Add(tblfooter_);
                doc.Close();
                if (doc.IsOpen())
                    doc.Close();
                //verificar si se creo el archivo
                //if (File.Exists(filename))
                //{
                //    //descargar
                //    string archivo = string.Empty;
                //    archivo = filename;
                //    Response.ContentType = "application/pdf";
                //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreDocumento);
                //    Response.TransmitFile(archivo);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet datosdetallehoras(string identificacion)
        {
            DataSet ds_pasantias = Conexion.BuscarPracticas_ds(" PASANTE t1 inner join EMPRESA_CONVENIO_TUTOR t2 on t1.ID_PASANTE = t2.ID_PASANTE inner join EMPRESA t3 on t2.ID_EMPRESA=t3.ID_EMPRESA ", " t3.NOMBRE_EMPRESA,t1.IDENTIFICACION_PASANTE,rtrim(t1.APELLIDO_PASANTE)+' '+t1.NOMBRE_PASANTE NOMBRES,t1.FECHA_INICIO_PASANTE,t1.FECHA_FIN_PASANTE,t1.NUMERO_HORAS_PASANTE,rtrim(t2.NOMBRE_TUTOR)+' '+t2.APELLIDO_TUTOR RESPONSABLE", " where t1.IDENTIFICACION_PASANTE='" + identificacion + "' and ESTADO_APROBADO=2");
            return ds_pasantias;
        }
        private string OnServerPath()
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string path;
            if (host == "localhost")
            {
                path = Server.MapPath("/documentosPPP/InformeFinalPPP/");
            }
            else
            {
                path = Server.MapPath("/documentosPPP/InformeFinalPPP/");
            }
            return path;
        }

        protected void dgvPasante_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvPasante.PageIndex = e.NewPageIndex;
            cargarpasantes();
        }

        protected void dgvPasante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string estadoEnviado = dgvPasante.DataKeys[e.Row.RowIndex]["ENVIADO_REGISTRO"].ToString();
                if (estadoEnviado == "1")
                {

                    e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(68, 215, 84);
                }

            }
        }
    }
}