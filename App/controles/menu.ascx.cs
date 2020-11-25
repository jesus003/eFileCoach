using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class controles_menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CSesion sesion = (CSesion)Session["csesion"];
            
            if (sesion.EsCuenta())
            {
                //Es administrador
                phCalendario.Visible = sesion.GetCuenta().MostrarCalendario();
                phCuenta.Visible = true;
                phFormaciones.Visible = true;
                phProcesos.Visible = true;
                phMiCuenta.Visible = true;
            }
            else
            {
                if (sesion.EsUsuario())
                {
                    phCalendario.Visible = sesion.GetUsuario().MostrarCalendario();
                    //Es usuario profesor o coach
                    phMiCuenta.Visible = false;
                    phCuenta.Visible = false;
                    if (sesion.GetUsuario().GetIDTipo() == 1 ||
                    sesion.GetUsuario().GetIDTipo() == 3)
                    {
                        phProcesos.Visible = true;
                    }
                    else
                    {
                        phProcesos.Visible = false;
                    }

                    if (sesion.GetUsuario().GetIDTipo() == 2 ||
                        sesion.GetUsuario().GetIDTipo() == 3)
                    {
                        phFormaciones.Visible = true;
                    }
                    else
                    {
                        phFormaciones.Visible = false;
                    }

                }
                else
                {
                    Session.Clear();
                    Response.Redirect("/default.aspx");
                }
            }
        }
        catch
        {
            Response.Redirect("/default.aspx");
        }
    }

    protected void cmdSalir_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("/default.aspx");
    }
}