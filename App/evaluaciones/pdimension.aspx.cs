using Controlador.DiagnosticosPlantillas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class evaluaciones_pdimension : System.Web.UI.Page
{
    Int64 iddimension = -1;
    DiagnosticosPlantillasDimensiones dimension;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.InicializaDatos();
        if (!IsPostBack)
        {
            this.CargarDatos();
        }
    }

    private void CargarDatos()
    {
        hlVolver.NavigateUrl = String.Format("plantilla.aspx?id={0}", 
            Cifrado.cifrarParaUrl(this.dimension.GetIDPlantilla().ToString()));
        this.txtNombre.Text = this.dimension.GetTitulo();
        DiagnosticosPlantillas plantilla = new DiagnosticosPlantillas(this.dimension.GetIDPlantilla());
        if (plantilla.GetIDTipo() == 1)
        {
            cmbTipoPregunta.Enabled = false;
            cmbTipoPregunta.SelectedValue = "1";
        }
        else
        {
            cmbTipoPregunta.Enabled = true;
        }
    }

    private void InicializaDatos()
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.iddimension = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"],true));
            this.hfIDDimension.Value = iddimension.ToString();
            this.dimension = new DiagnosticosPlantillasDimensiones(this.iddimension);
        }
        else
        {
            Response.Redirect("/default.aspx");
        }
    }

    private void ColumnasEdicion(Boolean visible)
    {
        grdPreguntas.Columns[0].Visible = visible;
        grdPreguntas.Columns[2].Visible = visible;
    }

    protected void grdPreguntas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 idpregunta;
        DiagnosticosPlantillasDimensionesPreguntas pregunta;
        GridViewRow fila;

        switch (e.CommandName)
        {
            case "cancelar":
                grdPreguntas.EditIndex = -1;
                grdPreguntas.DataBind();
                break;
            case "editar":
                if (grdPreguntas.EditIndex == -1)
                {
                    Button cmdEditar = (Button)e.CommandSource;
                    fila = (GridViewRow)cmdEditar.NamingContainer;
                    grdPreguntas.EditIndex = fila.RowIndex;
                    grdPreguntas.DataBind();
                }
                break;
            case "guardar":
                idpregunta = Convert.ToInt64(e.CommandArgument);
                pregunta = new DiagnosticosPlantillasDimensionesPreguntas(idpregunta);
                String texto = ((TextBox)grdPreguntas.Rows[grdPreguntas.EditIndex].FindControl("txtPregunta")).Text;
                if (pregunta.ActualizarPregunta(texto.Trim()))
                {
                    grdPreguntas.EditIndex = -1;
                    grdPreguntas.DataBind();
                }
                else
                {

                }

                break;
            case "eliminar":
                idpregunta = Convert.ToInt64(e.CommandArgument);
                pregunta = new DiagnosticosPlantillasDimensionesPreguntas(idpregunta);
                if (pregunta.Eliminar())
                {
                    grdPreguntas.DataBind();
                }
                else
                {

                }
                break;
            case "Subir":
                //Obtenemos el orden actual
                LinkButton cmdSubir = (LinkButton)e.CommandSource;
                fila = (GridViewRow)cmdSubir.NamingContainer;
                GridViewRow fila_anterior = grdPreguntas.Rows[fila.RowIndex - 1];

                HiddenField hfOrden = (HiddenField)fila.FindControl("hfOrden");
                HiddenField hfID = (HiddenField)fila.FindControl("hfID");
                Int64 idfila = Convert.ToInt64(hfID.Value);

                HiddenField hfOrdenAnterior = (HiddenField)fila_anterior.FindControl("hfOrden");
                HiddenField hfIDAnterior = (HiddenField)fila_anterior.FindControl("hfID");
                Int64 idfila_anterior = Convert.ToInt64(hfIDAnterior.Value);

                int orden, orden_anterior;
                orden = Convert.ToInt32(hfOrden.Value);
                orden_anterior = Convert.ToInt32(hfOrdenAnterior.Value);

                //Ahora tenemos que hacer el comando de actualización para las dos filas
                DiagnosticosPlantillasDimensionesPreguntas pregunta_fila =
                    new DiagnosticosPlantillasDimensionesPreguntas(idfila);

                DiagnosticosPlantillasDimensionesPreguntas pregunta_fila_anterior =
                    new DiagnosticosPlantillasDimensionesPreguntas(idfila_anterior);

                pregunta_fila.Actualizar(pregunta_fila.GetPregunta(), orden_anterior);
                pregunta_fila_anterior.Actualizar(pregunta_fila_anterior.GetPregunta(), orden);

                grdPreguntas.DataBind();

                break;
            case "Bajar":
                LinkButton cmdSubir2 = (LinkButton)e.CommandSource;
                GridViewRow fila2 = (GridViewRow)cmdSubir2.NamingContainer;
                GridViewRow fila_siguiente = grdPreguntas.Rows[fila2.RowIndex + 1];

                HiddenField hfOrden2 = (HiddenField)fila2.FindControl("hfOrden");
                HiddenField hfID2 = (HiddenField)fila2.FindControl("hfID");
                Int64 idfila2 = Convert.ToInt64(hfID2.Value);

                HiddenField hfOrdenSiguiente = (HiddenField)fila_siguiente.FindControl("hfOrden");
                HiddenField hfIDSiguiente = (HiddenField)fila_siguiente.FindControl("hfID");
                Int64 idfila_siguiente = Convert.ToInt64(hfIDSiguiente.Value);

                int orden2, orden_siguiente;
                orden2 = Convert.ToInt32(hfOrden2.Value);
                orden_siguiente = Convert.ToInt32(hfOrdenSiguiente.Value);

                //Ahora tenemos que hacer el comando de actualización para las dos filas

                DiagnosticosPlantillasDimensionesPreguntas pregunta_fila2 =
                    new DiagnosticosPlantillasDimensionesPreguntas(idfila2);

                DiagnosticosPlantillasDimensionesPreguntas pregunta_fila_anterior2 =
                    new DiagnosticosPlantillasDimensionesPreguntas(idfila_siguiente);


                pregunta_fila2.Actualizar(pregunta_fila2.GetPregunta(), orden_siguiente);
                pregunta_fila_anterior2.Actualizar(pregunta_fila_anterior2.GetPregunta(), orden2);
                grdPreguntas.DataBind();
                break;
            default:
                break;
        }

        this.ColumnasEdicion(grdPreguntas.EditIndex == -1);
    }

    protected void cmdGuardarPregunta_Click(object sender, EventArgs e)
    {
        if (this.ValidarNuevaPregunta())
        {
            DiagnosticosPlantillasDimensionesPreguntas pregunta = new DiagnosticosPlantillasDimensionesPreguntas();
            Int64 last_id = pregunta.Insertar(this.iddimension, txtPregunta.Text, dimension.GetOrdenSiguientePregunta(),
                Convert.ToInt64(cmbTipoPregunta.SelectedValue), Convert.ToInt32(rblNumRespuestas.SelectedValue));
            if (last_id != -1)
            {
                //Todo bien
                txtPregunta.Text = String.Empty;
                if (cmbTipoPregunta.Enabled)
                {
                    cmbTipoPregunta.SelectedIndex = 0;
                }
                grdPreguntas.DataBind();
            }
            else
            {
                //Fallos
                panelValidacionNuevaPregunta.Visible = true;
                literalResultado.Text = "Se ha producido un error al dar de alta la pregunta.";
            }
        }

    }

    private Boolean ValidarNuevaPregunta()
    {
        panelValidacionNuevaPregunta.Visible = false;
        literalResultado.Text = String.Empty;

        if (cmbTipoPregunta.SelectedIndex==0)
        {
            literalResultado.Text = "Debe Seleccionar el tipo de pregunta (numérica o libre)";
            return false;
        }

        if (txtPregunta.Text.Trim() == String.Empty)
        {
            literalResultado.Text = "Debe introducir el texto de la pregunta";
            return false;
        }

        return true;

    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        if (this.ValidarDatosCabecera())
        {
            if (!this.dimension.Actualizar(txtNombre.Text.Trim()))
            {
                panelValidacionCabecera.Visible = true;
                lResultadoCabecera.Text = "Se ha producido un error al guardar la dimensión";
            }
        }
    }

    private Boolean ValidarDatosCabecera()
    {
        panelValidacionCabecera.Visible = false;
        lResultadoCabecera.Text = String.Empty;
        if (txtNombre.Text.Trim() == String.Empty)
        {
            panelValidacionCabecera.Visible = true;
            lResultadoCabecera.Text = "Debe introducir el nombre de la dimensión";
            return true;
        }
        else
        {
            return true;
        }
    }

    protected void grdPreguntas_DataBound(object sender, EventArgs e)
    {
        //Vamos a organizar los botones del orden
        if (grdPreguntas.Rows.Count > 0)
        {
            //Desactivamos el botón de la primera fila
            LinkButton cmdSubir = (LinkButton)grdPreguntas.Rows[0].FindControl("cmdSubir");
            if (cmdSubir != null)
            {
                cmdSubir.Visible = false;
            }

            int contador = grdPreguntas.Rows.Count;
            LinkButton cmdBajar =
                (LinkButton)grdPreguntas.Rows[contador - 1].FindControl("cmdBajar");
            if (cmdBajar != null)
            {
                cmdBajar.Visible = false;
            }
        }
    }
}