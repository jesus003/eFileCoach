using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class evaluaciones_plantillas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblFecha = (Label)e.Row.FindControl("lblFecha");
        if (lblFecha != null)
        {
            if (lblFecha.Text == "01/01/1900")
            {
                lblFecha.Text = String.Empty;
            }
            else
            {
                DateTime fecha = Convert.ToDateTime(lblFecha.Text);
                lblFecha.Text = fecha.ToString("dd/MM/yyyy");
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
        Int32 cuantos = Convert.ToInt32(tabla.Compute("count(id)", odsDatos.FilterExpression).ToString());
        if (cuantos > 0)
        {
            lResultados.Text = "Se han encontrado " + cuantos.ToString() + " plantillas.";
        }
        else
        {
            lResultados.Text = "No se han encontrado plantillas con los criterios de búsqueda seleccionados.";
        }
    }

    protected void btnNuevaPlantilla_Click(object sender, EventArgs e)
    {
        Response.Redirect("plantilla.aspx");
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

        odsDatos.FilterExpression = filtro;
        grdResultados.DataBind();
    }

    protected void btnDetalles_Click(object sender, CommandEventArgs e)
    {
        String id = String.Empty;
        id = e.CommandArgument.ToString();
        Response.Redirect("plantilla.aspx?id=" + id);
    }



    protected void cmdBuscadorFormaciones_Click(object sender, EventArgs e)
    {
        this.ActualizarFiltro();
    }
}