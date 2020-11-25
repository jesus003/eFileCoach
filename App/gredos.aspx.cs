using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class gredos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*Calendario calendario = new Calendario(1);
        literalEventos.Text = calendario.GetSalida();*/
        //calendario.NuevoEvento("Titulo de pruebas", "Notas de pruebas", DateTime.Now, DateTime.Now.AddDays(1));
    }

    protected void cmdBorrar_Click(object sender, EventArgs e)
    {
        Calendario calendario = new Calendario(1, true);
        Response.Write(calendario.EliminarEvento(txtIDEvento.Text.Trim()).ToString());
    }

    protected void cmdEditar_Click(object sender, EventArgs e)
    {
        Calendario calendario = new Calendario(1, true);
        Response.Write(calendario.ActualizarEvento(txtTituloEvento.Text,"",DateTime.Now,
            DateTime.Now, txtIDEvento.Text.Trim()).ToString());
    }

    protected void cmdActualizar_Click(object sender, EventArgs e)
    {
        Calendario calendario = new Calendario(1, true);
        calendario.LeerEventos(DateTime.Now.Date,DateTime.Now.AddDays(1).Date);
        literalEventos.Text = calendario.GetSalida();
    }

    protected void cmdNuevo_Click(object sender, EventArgs e)
    {
        Calendario calendario = new Calendario(1,true);
        literalEventos.Text = calendario.NuevoEvento(txtTituloEvento.Text, "", DateTime.Now, DateTime.Now).ToString();
    }

    protected void cmdCargarEvento_Click(object sender, EventArgs e)
    {
        Calendario calendario = new Calendario(1,true);
        Event evento = calendario.GetEvento(txtIDEvento.Text);
        if (evento==null)
        {
            literalEventos.Text = "Evento no encontrado";
        }
        else
        {
            literalEventos.Text = "Evento encontrado: " + evento.Summary;
        }
    }
}