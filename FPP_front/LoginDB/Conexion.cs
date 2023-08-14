using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace PracticasPreProfesionales.LoginDb
{
    public static class Conexion
    {
        /// <summary>
        /// Crea una cadena de conexión al NAV
        /// </summary>
        /// <returns></returns>
        public static SqlConnection CreateConnection()
        {
            string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PortalEstudiantesUISEK_NV"].ConnectionString;
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Crea una cadena de conexión a Practicas 
        /// </summary>
        /// <returns></returns>
        public static SqlConnection CreateConnectionPracticas()
        {
            string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UISEK_PRATICAS_PREPROFESIONALES"].ConnectionString;
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Crea una cadena de conexión al UMAS
        /// </summary>
        /// <returns></returns>
        private static SqlConnection CreateConnectionUMAS()
        {
            string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PortalEstudiantesUISEK_EC"].ConnectionString;
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        ///  Crea una cadena de conexión a la base de RRHH
        /// </summary>
        /// <returns></returns>
        private static SqlConnection CreateConnectionRRHH()
        {
            string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PortalEstudiantesUISEK_RRHH"].ConnectionString;
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Busca en el NAV y devuelve un adaptador SQL
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        public static SqlDataAdapter BuscarNAV(string tabla, string campos, string condicion)
        {
            SqlConnection myConnection = CreateConnection();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "SELECT " + campos + " FROM " + tabla + " " + condicion;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            SqlDataAdapter lector = default(SqlDataAdapter);
            comando.Connection = myConnection;
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;

            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;
        }
        public static DataSet BuscarPracticas_ds(string tabla, string campos, string condicion)
        {
            SqlDataAdapter da = BuscarPracticas(tabla, campos, condicion);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        public static bool ActualizarPracticas(string tabla, string campos, string condicion)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnectionPracticas();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "UPDATE " + tabla + " SET " + campos + " " + condicion + ";";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;
            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }
        public static bool InsertarPracticas(string tabla, string campos, string valores)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnectionPracticas();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "INSERT INTO  " + tabla + " (   " + campos + ") values ( " + valores + ");";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;

            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }

        /// <summary>
        /// Usa la función de buscar en NAV y devuelve una Tabla 
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        public static DataSet BuscarNAV_ds(string tabla, string campos, string condicion)
        {
            SqlDataAdapter da = BuscarNAV(tabla, campos, condicion);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        public static DataSet BuscarPracticas_ds_simple(string campos)
        {
            SqlDataAdapter da = BuscarPracticas_simple(campos);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        public static DataSet BuscarUMAS_ds_simple(string campos)
        {
            SqlDataAdapter da = BuscarUMAS_simple(campos);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        /// <summary>
        /// Funcion copia del original creada para realizar una consulta en UMAS sin la necesidad de campos ni condiciones
        /// fue creada para revisar las asistencias del estudiante
        /// </summary>
        /// <param name="campos"></param>
        /// <returns></returns>

        private static SqlDataAdapter BuscarUMAS_simple(string campos)
        {
            SqlConnection myConnection = CreateConnectionUMAS();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "SELECT " + campos;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;
            SqlDataAdapter lector = default(SqlDataAdapter);
            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;

        }
        private static SqlDataAdapter BuscarPracticas_simple(string campos)
        {
            SqlConnection myConnection = CreateConnectionPracticas();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "SELECT " + campos;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;
            SqlDataAdapter lector = default(SqlDataAdapter);
            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;

        }

        private static SqlDataAdapter BuscarPracticas(string tabla, string campos, string condicion)
        {
            SqlConnection myConnection = CreateConnectionPracticas();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "SELECT " + campos + " FROM " + tabla + " " + condicion;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;
            SqlDataAdapter lector = default(SqlDataAdapter);
            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;

        }

        /// <summary>
        /// Busca en el UMAS y devuelve un adaptador SQL
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        private static SqlDataAdapter BuscarUMAS(string tabla, string campos, string condicion)
        {
            SqlConnection myConnection = CreateConnectionUMAS();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "SELECT " + campos + " FROM " + tabla + " " + condicion;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;
            SqlDataAdapter lector = default(SqlDataAdapter);
            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;

        }

        /// <summary>
        /// Usa la función de buscar en UMAS y devuelve una Tabla 
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        /// 
        public static DataSet BuscarRRHH_ds(string tabla, string campos, string condicion)
        {
            SqlDataAdapter da = BuscarRRHH(tabla, campos, condicion);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        /// <summary>
        /// Busca en el UMAS y devuelve un adaptador SQL
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        private static SqlDataAdapter BuscarRRHH(string tabla, string campos, string condicion)
        {
            SqlConnection myConnection = CreateConnectionRRHH();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql;
            sql = "SELECT " + campos + " FROM " + tabla + " " + condicion;
            SqlCommand comando = new SqlCommand(sql, myConnection);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = 99999999;
            SqlDataAdapter lector = default(SqlDataAdapter);
            try
            {
                lector = new SqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                lector = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return lector;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabla">tabla en la que se va a buscar</param>
        /// <param name="campos">campos que se quiere en la busqueda</param>
        /// <param name="condicion">condiciones de busqueda</param>
        /// <returns></returns>
        public static DataSet BuscarUMAS_ds(string tabla, string campos, string condicion)
        {
            SqlDataAdapter da = BuscarUMAS(tabla, campos, condicion);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return (ds);
        }
        /// <summary>
        /// permite insertar una registro en una tabla del NAV
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public static bool InsertarNAV(string tabla, string campos, string valores)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnection();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "INSERT INTO  " + tabla + " (   " + campos + ") values ( " + valores + ");";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;

            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }

        /// <summary>
        /// Función que permite insertar un valor en una tabla del UMAS 
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public static bool InsertarUMAS(string tabla, string campos, string valores)
        {
            bool messages =false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnectionUMAS();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "INSERT INTO  " + tabla + " (   " + campos + ") values ( " + valores + ");";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                // comando.Transaction = oTrans
                //comando.CommandTimeout = 14000
                comando.ExecuteNonQuery();

                messages = true;


            }
            catch (SqlException ex)
            {
                messages = false;

            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }
        /// <summary>
        /// Función que permite actualizar un valor en una tabla del NAV 
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="condicion"></param>
        /// <returns></returns>
        public static bool ActualizarNAV(string tabla, string campos, string condicion)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnection();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "UPDATE " + tabla + " SET " + campos + " " + condicion + ";";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                // comando.Transaction = oTrans
                //comando.CommandTimeout = 14000
                comando.ExecuteNonQuery();

                messages = true;

            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }
        /// <summary>
        /// Función que permite actualizar un valor en una tabla del UMAS 
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="condicion"></param>
        /// <returns></returns>
        public static bool ActualizarUMAS(string tabla, string campos, string condicion)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnectionUMAS();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "UPDATE " + tabla + " SET " + campos + " " + condicion + ";";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;
            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;

        }
        /// <summary>
        /// Función que permite borrar un registro en una tabla del UMAS 
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="condicion"></param>
        /// <returns></returns>
        public static bool DeleteUMAS(string tabla, string condicion)
        {

            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnectionUMAS();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "DELETE " + tabla + " WHERE " + condicion + ";";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;

            }
            catch (SqlException ex)
            {

                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;
        }
        /// <summary>
        /// Función que permite borrar un registro en una tabla del NAV 
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="condicion"></param>
        /// <returns></returns>
        public static bool DeleteNAV(string tabla, string condicion)
        {
            bool messages = false;
            SqlCommand comando = new SqlCommand();
            SqlConnection myConnection = CreateConnection();
            if (myConnection.State == ConnectionState.Closed)
                myConnection.Open();

            string sql = "DELETE " + tabla + " WHERE " + condicion + ";";
            // crecion de excepcion de la consulta a la base de datos
            try
            {
                comando.Connection = myConnection;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                messages = true;

            }
            catch (SqlException ex)
            {
                messages = false;
            }
            finally
            {

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }
            return messages;
        }

    }
}