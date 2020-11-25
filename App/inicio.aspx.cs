using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class inicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            CSesion csesion = (CSesion)Session["csesion"];
            if (csesion.EsUsuario())
            {
                Response.Redirect("inicio-cliente.aspx");
            }
        }
    }
}