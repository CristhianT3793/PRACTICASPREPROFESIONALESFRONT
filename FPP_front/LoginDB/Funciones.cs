using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticasPreProfesionales.LoginDb
{
    public class Funciones
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string conectarMail()
        {
            string cadena = "WJaaDRaFj6S5BDnMnm9ie+BlSBvQTFBj";
            string cadena1 = Desencriptar(cadena);
            return cadena1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static string Encriptar(string Input)
        {

            byte[] IV = ASCIIEncoding.ASCII.GetBytes("qualityi"); //La clave debe ser de 8 caracteres
            byte[] EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5"); //No se puede alterar la cantidad de caracteres pero si la clave
            byte[] buffer = Encoding.UTF8.GetBytes(Input);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;

            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length)); ;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static string Desencriptar(string Input)
        {

            byte[] IV = ASCIIEncoding.ASCII.GetBytes("qualityi"); //La clave debe ser de 8 caracteres
            byte[] EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5"); //No se puede alterar la cantidad de caracteres pero si la clave
            byte[] buffer = Convert.FromBase64String(Input);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;
            return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string EncriptarURL(string Input)
        {
            string query = Encriptar(Input);
            query = query.Replace("+", "-").Replace("/", "_").Replace("=", ".");
            return HttpUtility.UrlEncode(query);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string DesencriptarURL(string Input)
        {
            string query = HttpUtility.HtmlDecode(Input);
            query = query.Replace("-", "+").Replace("_", "/").Replace(".", "=");
            return Desencriptar(query);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Dc_sender(string Input)
        {
            return Desencriptar(Input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Ec_sender(string Input)
        {
            return Encriptar(Input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NombreUsuario"></param>
        /// <param name="cedula"></param>
        /// <param name="tipo"></param>
        /// <param name="idtracking"></param>
        /// <param name="anoperiodo"></param>
        /// <param name="pasoInicial"></param>
        /// <param name="pasoFinal"></param>
        /// <param name="observacion"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public static int EnviarCorreo(string NombreUsuario, string cedula, string tipo, string idtracking, string anoperiodo, string pasoInicial, string pasoFinal, string observacion, string archivo)
        {

            int valorRetorna = 1;
            return valorRetorna;
        }


        public static int EnviarCorreo(string tracking, string paso,string nombre,string tipo, string anoperiodo,string accion,string estado,string correo_docente)
        {
            int valorRetorna = 0;
            string desc_accion = string.Empty;
            string desc_tipo = string.Empty;
            if (estado != "3")
            {
                switch (accion) {
                    case "0":
                        desc_accion = "Enviado";
                        break;
                    case "1":
                            desc_accion = "Aprobado"; break;
                    case "2":
                            desc_accion = "Se envió a revisión";break;
                    case "3":
                            desc_accion = "Solicitud rechazada";
                        break;
                }
            }
            else
            {
                desc_accion = "Se asignó nueva fecha";
            }

            if (tipo == "1")
                desc_tipo = "(A) Pasantias";
            else if (tipo == "2")
                desc_tipo = "(B) Contrato";

            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("no.reply@uisek.edu.ec", "Sistema de seguimiento de pasantias Universidad Internacional SEK", System.Text.Encoding.UTF8);
                correo.Subject = "Correo autogenerado de Solicitud de Practicas Pre profesionales";
                correo.SubjectEncoding = System.Text.Encoding.UTF8;
                correo.Body = "SSistema de seguimiento de pasantias Universidad Internacional SEK.\n NO RESPONDA A ESTE EMAIL.\n En caso de dudas contacte al departamento técnico.";
                correo.BodyEncoding = System.Text.Encoding.UTF8;
                string encabezado = "<html><head> " +
                                              "<style type=\"text/css\">.style3 { width:30%;  } .style2 {color:red;}.style4 {border:0;} .titulo{text-align:center;}</style>" +
                                              "</head>" +
                                              "<body class=\"style4\">" +
                                              "<form id=\"form1\" runat=\"server\">" +
                                              "<div><img src='http://sgi.uisek.edu.ec/becas/media/logo_Sek3.png' width='100px' align='center'></img></div>" +
                                              "<div class=\"titulo\"><h3>SEGUIMIENTO DE PRACTICAS PRE PROFESIONALES</h3></div>";
                string finalmail = "<hr/><br/>" +
                                               "<div><table class=\"style4\" style=\"width:100%;\">" +
                                               "<tr><td  class=\"style2\" style=\"font-weight:bold\">FPP#" + paso + "</td></tr>" +
                                               "<tr></tr>" +
                                               "<tr><td style=\"font-weight:bold\">Nombre:</td><td>" + tracking + "</td> </tr>" +
                                               //"<tr><td style=\"font-weight:bold\">C&eacute;dula estudiante:</td><td>" + cedula + "</td> </tr>" +
                                               "<tr><td style=\"font-weight:bold\">Nombre Estudiante:</td><td>" + nombre + "</td> </tr>" +
                                               "<tr><td style=\"font-weight:bold\">Tipo de solicitud:</td><td>" + desc_tipo + "</td> </tr>" +
                                               "<tr><td style=\"font-weight:bold\">Semestre:</td><td>" + anoperiodo + "</td> </tr>" +
                                               "<tr><td style=\"font-weight:bold\">Accion:</td><td>" + desc_accion + "</td> </tr>" +
                                               "</table>" +
                                               "<hr/><br/>" +
                                               "<div><label>Saludos cordiales,</label><br/>" +
                                               "<br/><br/>" +
                                               "<div class=\"style2\"><label >NO RESPONDA A ESTE CORREO. </label></div>" +
                                               "En caso de dudas contacte al departamento t&eacute;cnico de la Universidad Internacional SEK sistemas@uisek.edu.ec ." +
                                               "</div></form></body></html>";
                string body = string.Empty;
                correo.To.Add("direccion.relint@uisek.edu.ec");
                if(correo_docente!="")
                    correo.To.Add(correo_docente);


                body = encabezado + finalmail;

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                // Add the alternate body to the message.
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                correo.AlternateViews.Add(alternate);
                correo.IsBodyHtml = false;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("no.reply@uisek.edu.ec", Funciones.conectarMail());
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail 
                client.Send(correo);
                valorRetorna = 1;
            }
            catch
            {
                valorRetorna = 0;
            }

            //int valorRetorna = 1;
            return valorRetorna;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string obtenerAnioAcademicoActual()
        {
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;
            if (mes > 9 && mes < 13)
                anio++;
            
            string aux = anio.ToString();

            return aux;
        }

    }

}
