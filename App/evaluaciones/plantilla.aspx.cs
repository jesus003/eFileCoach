using Controlador.DiagnosticosPlantillas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;
using System.Data;

public partial class evaluaciones_plantilla : System.Web.UI.Page
{
    Int64 idusuario;
    Int64 idplantilla = -1;
    String nuevoOrden = String.Empty;
    Int32 NewListOrderNumber;
    Boolean actualizar = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["id"] != null)
        {
            cmbTipoEncuesta.Enabled = false;
            this.idplantilla = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"],true));
            hfIDPlantilla.Value = this.idplantilla.ToString();
            Session["plantilla"] = this.idplantilla;
            chkPublico.Visible = false;
            if (this.UsuarioLogueado.EsCuenta())
            {
                if (this.UsuarioLogueado.GetCuenta().GetPublicaPlantillas())
                {
                    chkPublico.Visible = true;
                }
            }
        }

        this.idusuario = Convert.ToInt64(Session["idusuario"].ToString());
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                Controlador.DiagnosticosPlantillas.DiagnosticosPlantillas plantillas =
                    new Controlador.DiagnosticosPlantillas.DiagnosticosPlantillas(idplantilla);
                txtNombre.Text = plantillas.GetNombre();
                cmbTipoEncuesta.SelectedValue = plantillas.GetIDTipo().ToString();
                dtFecha.SelectedDate = plantillas.GetFecha();
                chkDisponible.Checked = plantillas.GetDisponible();
                chkPublico.Checked = plantillas.GetPublica();
                this.GestionaPaneles();
            }
            else
            {
                //Estamos dando de alta una nueva plantilla
                dtFecha.SelectedDate = DateTime.Now;
            }

        }
        else
        {
            if (Request.QueryString["id"] != null)
            {
                DiagnosticosPlantillas plantillas = 
                    new DiagnosticosPlantillas(idplantilla);
                Session["actualiza"] = 1;
                this.GestionaPaneles();
            }
        }
    }

    protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void grdResultados_PageIndexChanged(object sender, EventArgs e)
    {
        this.ActualizarFiltro();
    }

    protected void grdResultados_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.ActualizarFiltro();
    }

    protected void odsResultados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["idPlantilla"] = this.idplantilla;
    }
    protected void odsResultados_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        System.Data.DataTable tabla = ((System.Data.DataTable)e.ReturnValue);
        Int32 cuantos = Convert.ToInt32(tabla.Compute("count(id)", odsResultados.FilterExpression).ToString());
        if (cuantos > 0)
        {
            lResultados.Text = "Se han encontrado " + cuantos.ToString() + " dimensiones.";
        }
        else
        {
            lResultados.Text = "Todavía no hay preguntas en tu plantilla. Puedes añadir una pregunta mediante el botón inferior \"Añadir Dimensión\"";
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        bllList.Items.Clear();
        Int64 idplantilla;
        Boolean actualizado = false;

        if (this.ValidarCampos())
        {
            DiagnosticosPlantillas plantilla;

            if (Request.QueryString["id"] != null)
            {
                plantilla = new DiagnosticosPlantillas(this.idplantilla);
                actualizado = plantilla.Actualizar(txtNombre.Text,
                    (DateTime)dtFecha.SelectedDate,
                    chkDisponible.Checked, chkPublico.Checked);
                if (!actualizado)
                {
                    pnlMensajes.Visible = true;
                    lblAlertas.Text = "Hay errores al actualizar la plantilla, por favor, pongase en contacto con el administrador de la web";
                    bllListPlantillas.Visible = true;
                    panelGuardado.Visible = false;
                }
                else
                {
                    //Response.Redirect(String.Format("plantilla.aspx?id={0}", this.idplantilla));
                    panelGuardado.Visible = true;
                }
            }
            else
            {
                //Sustituir el 1 por el id de usuario que corresponda.
                plantilla = new DiagnosticosPlantillas();
                idplantilla = plantilla.Insertar(idusuario,
                    txtNombre.Text, (DateTime)dtFecha.SelectedDate,
                    chkDisponible.Checked, chkPublico.Checked,
                    Int64.Parse(cmbTipoEncuesta.SelectedValue));
                if (idplantilla != -1)
                {
                    this.idplantilla = idplantilla;
                    Response.Redirect(String.Format("plantilla.aspx?id={0}", Cifrado.cifrarParaUrl(this.idplantilla.ToString())));
                    //panelGuardado.Visible = true;
                }
                else
                {
                    pnlMensajes.Visible = true;
                    lblAlertas.Text = "Hay errores al insertar la plantilla, por favor, pongase en contacto con el administrador de la web";
                    panelGuardado.Visible = false;
                    bllList.Visible = true;
                }
            }
        }
    }

    protected void cmdAnadirDimension_Click(object sender, EventArgs e)
    {
        bllList.Items.Clear();
        Int64 iddimension;

        if (this.ValidarCamposDimensiones())
        {

            DiagnosticosPlantillas plantilla = new DiagnosticosPlantillas(this.idplantilla);
            DiagnosticosPlantillasDimensiones dimensiones =
                new DiagnosticosPlantillasDimensiones();

            iddimension = dimensiones.Insertar(this.idplantilla, txtTitulo.Text, plantilla.GetOrdenSiguiente());
            if (iddimension != -1)
            {
                this.GetListData();
                this.limpiarCamposDimensiones();
                grdResultados.DataBind();
            }
            else
            {
                pnlMensajes.Visible = true;
                lblAlertas.Text = "Hay errores al insertar la pregunta, por favor, pongase en contacto con el administrador de la web";
                bllList.Visible = true;
            }
        }

        panelGuardado.Visible = false;
    }

    protected void btnEliminarDimPlantilla_Click(object sender, CommandEventArgs e)
    {
        Int64 idDimension = Convert.ToInt64(e.CommandArgument.ToString());
        DiagnosticosPlantillasDimensiones dimensiones = new DiagnosticosPlantillasDimensiones(idDimension);
        dimensiones.Eliminar();
        this.ActualizarFiltro();
        panelGuardado.Visible = false;
    }

    protected void btnEditarDimPlantilla_Click(object sender, CommandEventArgs e)
    {
        this.ActualizarFiltro();
    }

    /// <summary>
    /// Método para limpiar el formulario, por ejemplo cuando se vaya a añadir nueva plantilla.
    /// </summary>
    private void limpiarFormulario()
    {
        txtNombre.Text = String.Empty;
        dtFecha.SelectedDate = DateTime.Now;
        chkDisponible.Checked = false;
    }

    private void limpiarCamposDimensiones()
    {
        txtTitulo.Text = String.Empty;

    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        DiagnosticosPlantillas plantilla = new DiagnosticosPlantillas(this.idplantilla);
        if (!plantilla.Eliminar())
        {
            pnlMensajes.Visible = true;
            bllList.Items.Clear();
            lblAlertas.Text = "Hay errores al eliminar la plantilla, por favor, pongase en contacto con el administrador de la web";
            bllList.Items.Add("No se ha eliminado correctamente la plantilla");
            bllList.Visible = true;
        }
        else
        {
            Response.Redirect("plantillas.aspx");
        }
    }

    protected void btnSinGuardar_Click(object sender, EventArgs e)
    {
        Response.Redirect("plantillas.aspx");
    }

    //Validamos campos del formulario
    private Boolean ValidarCampos()
    {
        bllList.Items.Clear();

        if (txtNombre.Text.Trim() ==String.Empty)
        { 
            bllListPlantillas.Items.Add("El campo nombre está vacío");
        }

        if (cmbTipoEncuesta.SelectedIndex == 0)
        {
            bllListPlantillas.Items.Add("Debe seleccionar el tipo de encuesta");
        }

        if (dtFecha.SelectedDate==null)
        {
            bllListPlantillas.Items.Add("La campo fecha está vacío");
        }

        if (bllListPlantillas.Items.Count > 0)
        {
            pnlMensajesPlantillas.Visible = true;
            lblAlertas.Text = "Hay errores al insertar la plantilla, por favor, revise si algún campo está vacío o hay algún formato erroneo";
            bllListPlantillas.Visible = true;
            return false;
        }

        return true;
    }

    private Boolean ValidarCamposDimensiones()
    {
        bllList.Items.Clear();
        if (txtTitulo.Text.Equals(String.Empty))
        {
            bllList.Items.Add("El campo titulo está vacío");
        }

        if (bllList.Items.Count > 0)
        {
            pnlMensajes.Visible = true;
            lblAlertas.Text = "Hay errores al insertar la pregunta, por favor, revise si algún campo está vacío o hay algún formato erroneo";
            bllList.Visible = true;
            return false;
        }
        return true;
    }

    private void ActualizarFiltro()
    {
        String filtro = "1=1";

        odsResultados.FilterExpression = filtro;
        grdResultados.DataBind();
    }

    protected void grdResultados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "actualizar":
                if (this.validarEdicion(e))
                {
                    int i = grdResultados.EditIndex;
                    String txtTituloEdicion = ((TextBox)grdResultados.Rows[i].FindControl("txtTitulo")).Text;
                    Int64 iddimension = Convert.ToInt64(e.CommandArgument.ToString());
                    Controlador.DiagnosticosPlantillas.DiagnosticosPlantillasDimensiones dimensiones =
                        new Controlador.DiagnosticosPlantillas.DiagnosticosPlantillasDimensiones(iddimension);
                    bllList.Items.Clear();
                    if (dimensiones.Actualizar(txtTituloEdicion))
                    {
                        grdResultados.EditIndex = -1;
                        pnlMensajes.Visible = false;
                        this.ActualizarFiltro();
                    }
                    else
                    {
                        pnlMensajes.Visible = true;
                        bllList.Items.Add("Hay errores al actualizar la pregunta, por favor, pongase en contacto con el administrador de la web");
                        bllList.Visible = true;
                    }
                }
                panelGuardado.Visible = false;
                break;
            case "eliminar":
                Int64 iddimension2 = Convert.ToInt64(e.CommandArgument.ToString());
                DiagnosticosPlantillasDimensiones dimensiones2 =
                    new DiagnosticosPlantillasDimensiones(iddimension2);
                bllList.Items.Clear();
                if (dimensiones2.Eliminar())
                {
                    grdResultados.SelectedIndex = -1;
                    pnlMensajes.Visible = false;
                    this.ActualizarFiltro();
                }
                else
                {
                    pnlMensajes.Visible = true;
                    bllList.Items.Add("Hay errores al eliminar la pregunta, por favor, pongase en contacto con el administrador de la web");
                    bllList.Visible = true;
                }
                panelGuardado.Visible = false;
                break;
            case "Subir":
                //Obtenemos el orden actual
                LinkButton cmdSubir = (LinkButton)e.CommandSource;
                GridViewRow fila = (GridViewRow)cmdSubir.NamingContainer;
                GridViewRow fila_anterior = grdResultados.Rows[fila.RowIndex - 1];

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
                DiagnosticosPlantillasDimensiones dimension =
                    new DiagnosticosPlantillasDimensiones(idfila);

                DiagnosticosPlantillasDimensiones dimension_anterior =
                    new DiagnosticosPlantillasDimensiones(idfila_anterior);

                dimension.SetOrden(orden_anterior);
                dimension_anterior.SetOrden(orden);
                grdResultados.DataBind();
                panelGuardado.Visible = false;
                break;
            case "Bajar":
                LinkButton cmdSubir2 = (LinkButton)e.CommandSource;
                GridViewRow fila2 = (GridViewRow)cmdSubir2.NamingContainer;
                GridViewRow fila_siguiente = grdResultados.Rows[fila2.RowIndex + 1];

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
                DiagnosticosPlantillasDimensiones dimension2 =
                    new DiagnosticosPlantillasDimensiones(idfila2);

                DiagnosticosPlantillasDimensiones dimension_siguiente =
                    new DiagnosticosPlantillasDimensiones(idfila_siguiente);

                dimension2.SetOrden(orden_siguiente);
                dimension_siguiente.SetOrden(orden2);
                grdResultados.DataBind();
                panelGuardado.Visible = false;
                break;
            default:
                break;
        }


    }

    private Boolean validarEdicion(GridViewCommandEventArgs e)
    {
        TextBox txtTituloEdicion = (TextBox)grdResultados.Rows[grdResultados.EditIndex].FindControl("txtTitulo");
        TextBox txtOrdenEdicion = (TextBox)grdResultados.Rows[grdResultados.EditIndex].FindControl("txtOrden");

        if (txtTituloEdicion.Text.Trim() == String.Empty
            || txtOrdenEdicion.Text.Trim() == String.Empty)
        {
            bllList.Items.Add("Debe insertar los campos Título y Orden");
        }
        return true;
    }

    private Boolean EsAlta()
    {
        if (Request.QueryString["id"] == null)
            return true;
        return false;
    }

    private void GestionaPaneles()
    {
        if (!this.EsAlta())
        {
            cmdEliminar.Visible = true;
            pnlDimensiones.Visible = true;
            cmdGuardar.Text = "Guardar Plantilla";
            if (Convert.ToInt32(Session["actualiza"]) != 1)
            {
                GetListData();
            }
        }
    }




    /// <summary>
    /// Método para rellenar el reorderlist
    /// </summary>
    protected void GetListData()
    {
        DiagnosticosPlantillasDimensiones dimensiones = new DiagnosticosPlantillasDimensiones();
        DataTable dt = dimensiones.GetDimensionesPorPlantilla(this.idplantilla);
        if (dt.Rows.Count > 0)
        {
            lResultados.Text = "Se han encontrado " + dt.Rows.Count + " resultados";
        }
        else
        {
            lResultados.Text = "Todavía no hay dimensiones en tu plantilla. Puedes añadir una pregunta mediante el botón inferior \"Añadir Pregunta\"";
        }

    }

    protected void grdResultados_DataBound(object sender, EventArgs e)
    {
        //Vamos a organizar los botones del orden
        if (grdResultados.Rows.Count > 0)
        {
            //Desactivamos el botón de la primera fila
            LinkButton cmdSubir = (LinkButton)grdResultados.Rows[0].FindControl("cmdSubir");
            cmdSubir.Visible = false;

            int contador = grdResultados.Rows.Count;
            LinkButton cmdBajar =
                (LinkButton)grdResultados.Rows[contador - 1].FindControl("cmdBajar");
            cmdBajar.Visible = false;
        }
    }
}