using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PremioExcelencia.Login
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    Session["usuario"] = "";
            //    Session["codUsuario"] = "";
            //    Session["Password"] = "";
            //    Session["TipoUser"] = "";
            //}
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                lbluser_v.Visible = false;
                lblpassword_v.Visible = false;
                string user = txtUsuario.Text.Trim().ToString();
                string password = txtPassword.Text.Trim().ToString();
                login_user(user, password);
                //login(user, password);
            }
            else
            {
                if (txtUsuario.Text.Trim() == "")
                {
                    lbluser_v.Visible = true;
                    lbluser_v.Text = "El campo no puede estar vacio";
                }
                if (txtPassword.Text.Trim() == "")
                {
                    lblpassword_v.Visible = true;
                    lblpassword_v.Text = "El campo no puede estar vacio";
                }

            }
        }
        public  void login_user(string usuario, string password)
        {
            string user, pass, nombres, apellidos;
            long codUser;
            int tipoUser = 0;
                user = "ctupiza";
                pass = "123"; 
                if (user.Trim() != null && user.Trim() != "" && pass.Trim() != "")
                {
                    if (pass.Trim().ToString() == password.Trim().ToString())
                    {

                        Session["usuario"] = user;
                        Session["Password"] = pass;
                        Response.Redirect("../Default.aspx");
                    }
                    else
                    {
                        string script = @"Swal.fire({
                            icon: 'error',
                            title: 'error',
                            text: 'contraseña o usuario incorrecto',
                            footer: '<a href></a>'
                        })";
                        ClientScript.RegisterStartupScript(GetType(), "script", script, true);
                        limpiarcampos();
                    }

                }
                else
                {
                    string script = @"Swal.fire({
                            icon: 'error',
                            title: 'error',
                            text: 'No existe usuario registrado en el sistema',
                            footer: '<a href></a>'
                        })";
                    ClientScript.RegisterStartupScript(GetType(), "script", script, true);
                    limpiarcampos();
                }


        }
        public void limpiarcampos()
        {
            txtUsuario.Text = "";
            txtPassword.Text = "";
        }
    }
}