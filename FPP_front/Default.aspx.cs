using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FPP_front
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Session["cp"] = Request.QueryString["cp"].ToString();//descomentar en prod
                    //Session["cp"] = "1500761067";//usuario aprobador
                    //Session["cp"] = "1713919163";//usuario tutor de arquitectura
                    if (Session["cp"] == null)
                    {
                        Response.Redirect("https://portaldocentes.uisek.edu.ec/");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}