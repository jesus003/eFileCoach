using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class evaluaciones_evaluaciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Int64 idcuenta;
            if (UsuarioLogueado.EsCuenta())
            {
                idcuenta = UsuarioLogueado.GetCuenta().GetID();
            }
            else
            {
                idcuenta = UsuarioLogueado.GetUsuario().GetIDCuenta();
            }
            Int32 contador_encuestas = Cuentas.GetEncuestasContador(idcuenta);
            if (contador_encuestas>=5)
            {
                btnNuevaEvaluacion.Enabled = false;
                btnNuevaEvaluacion.Text = "Ha llegado al máximo de encuestas (5) en la cuenta efileCoach free";
            }
            else
            {
                btnNuevaEvaluacion.Enabled = true;
            }
        }
    }
    protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Literal lFecha = (Literal)e.Row.FindControl("lFecha");
        if (lFecha != null)
        {
            if (lFecha.Text == "01/01/1900")
            {
                lFecha.Text = String.Empty;
            }
            else
            {
                DateTime fecha = Convert.ToDateTime(lFecha.Text);
                lFecha.Text = fecha.ToString("dd/MM/yyyy");
            }
        }

        Literal litEstado = (Literal)e.Row.FindControl("litEstado");
        if (litEstado != null)
        {
            HiddenField hfEstado = (HiddenField)e.Row.FindControl("hfEstado");
            switch (Convert.ToInt64(hfEstado.Value))
            {
                case 1:
                    //Sin enviar
                    litEstado.Text = " <h3><span class='label label-default'>Editable <span class='glyphicon glyphicon-pencil' aria-hidden='true'></span> </span></h3>";
                    break;
                case 2:
                    //Enviado sin contestar
                    litEstado.Text = " <h3><span class='label label-info'>En espera <span class='glyphicon glyphicon-envelope' aria-hidden='true'></span> </span></h3>";
                    break;
                case 3:
                    litEstado.Text = " <h3><span class='label label-success'>Cerrada <span class='glyphicon glyphicon-ok' aria-hidden='true'></span> </span></h3>";
                    break;
                default:
                    break;
            }
        }

    }

    protected void grdResultados_PageIndexChanged(object sender, EventArgs e)
    {
        this.ActualizarFiltro();
    }
    protected void grdResultados_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.ActualizarFiltro();
    }

    protected void odsResultados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        System.Data.DataTable tabla = ((System.Data.DataTable)e.ReturnValue);
        Int32 cuantos = Convert.ToInt32(tabla.Compute("count(id)", odsResultados.FilterExpression).ToString());
        if (lResultados.Text!="")
        {
            if (cuantos > 0)
            {
                lResultados.Text = "Se han encontrado " + cuantos.ToString() + " evaluaciones.";
            }
            else
            {
                lResultados.Text = "No se han encontrado evaluaciones con los criterios de búsqueda seleccionados.";
            }
        }
    }

    protected void btnNuevaEvaluacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("evaluacion.aspx");
    }

    protected void btnDetalles_Click(object sender, CommandEventArgs e)
    {
        String id = String.Empty;
        id = e.CommandArgument.ToString();
        Response.Redirect("evaluacion.aspx?id=" + Cifrado.cifrarParaUrl(id));
    }

    private void ActualizarFiltro()
    {
        String filtro = "1=1";
        String subfiltro = String.Empty;
        if (txtBuscador.Text != String.Empty)
        {
            subfiltro = " AND ";
            subfiltro += String.Format("(nombre LIKE '%{0}%')", txtBuscador.Text.Trim());
        }

        filtro += subfiltro;

        odsResultados.FilterExpression = filtro;
        grdResultados.DataBind();
    }

    protected void txtBuscador_TextChanged(object sender, EventArgs e)
    {
        this.ActualizarFiltro();
    }

    protected void cmdBuscar_Click(object sender, EventArgs e)
    {
        this.ActualizarFiltro();
    }
}