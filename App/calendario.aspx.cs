using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class calendario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (usuario.EsCuenta())
            {
                radCalendario.DataSourceID = "odsCalendarioCuenta";
                if (usuario.GetCuenta().SyncGoogleCalendar())
                {
                    cmdSyncGoogle.Text = "Sincronizar calendario";
                }
                else
                {
                    cmdSyncGoogle.Text = "Conectar y sincronizar";
                }
                    
            }
            else
            {
                radCalendario.DataSourceID = "odsCalendarioUsuario";
                if (usuario.GetUsuario().SyncGoogleCalendar())
                {
                    cmdSyncGoogle.Text = "Sincronizar calendario";
                }
                else
                {
                    cmdSyncGoogle.Text = "Conectar y sincronizar";
                }
            }
        }
    }

    protected void radCalendario_AppointmentClick(object sender, Telerik.Web.UI.SchedulerEventArgs e)
    {
        int i = 0;
        String[] identificador = e.Appointment.ID.ToString().Split(':');
        Int64 id = Convert.ToInt64(identificador[1]);
        Controlador.Calendario c = new Controlador.Calendario(e.Appointment.ID.ToString());
        String redireccion = c.GetURL() + "?id=" + Cifrado.cifrarParaUrl(c.GetID().ToString());
        //Response.Write("<script>window.open ('" + redireccion + "','_blank');</script>");
        //Response.Redirect();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "show window",
            "window.open ('" + redireccion + "','_blank');", true);
    }

    protected void radCalendario_AppointmentDataBound(object sender, Telerik.Web.UI.SchedulerEventArgs e)
    {
        String id = e.Appointment.ID.ToString();
        if (id.Contains("sesion"))
        {
            e.Appointment.CssClass = "rsCategoryYellow";
        }
        else
        {
            if (id.Contains("formacion"))
            {
                e.Appointment.CssClass = "rsCategoryGreen";
            }
            else
            {
                if (id.Contains("coach"))
                {
                    e.Appointment.CssClass = "rsCategoryBlue";
                }
                else
                {
                    e.Appointment.CssClass = "rsCategoryOrange";
                }
            }
        }
    }

    protected void cmdSyncGoogle_Click(object sender, EventArgs e)
    {
        if (usuario.EsCuenta())
        {
            Calendario calendario = new Calendario(usuario.GetCuenta().GetID(),true);
            usuario.GetCuenta().SetGoogleCalendar(true);
        }
        else
        {
            Calendario calendario = new Calendario(usuario.GetUsuario().GetID(),false);
            usuario.GetUsuario().SetGoogleCalendar(true);
        }
    }
}