using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class formacion_form : System.Web.UI.Page
{
    Int64 id;
    Formaciones formacion;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("inicio");
        }
        else
        {
            CryptoAES c = new CryptoAES();
            String descifrado = c.descifrar(Request.QueryString["id"], true);
            this.id = Convert.ToInt64(descifrado);
            this.hfIDFormacion.Value = id.ToString();
            this.formacion = new Formaciones(this.id);
            hfIDCuenta.Value = formacion.GetIDCuenta().ToString();
            if (usuarioLogueado.EsCuenta())
            {
                if (usuarioLogueado.GetCuenta().GetID() != formacion.GetIDCuenta())
                {

                    Response.Redirect("/Default.aspx");
                }
            }
            else
            {
                if (usuarioLogueado.EsUsuario())
                {
                    cmbFormador.Enabled = false;
                    if (usuarioLogueado.GetUsuario().GetID() != formacion.GetIDFormador())
                    {
                        if (Procesos.GetCoachAsignado(usuarioLogueado.GetUsuario().GetID(),
                            formacion.GetID()))
                        {
                            panelContenidoPrincipal.Enabled = false;
                            cmdEliminar.Visible = false;
                            cmdGuardar.Visible = false;
                            panelNuevaActividad.Visible = false;
                        }
                        else
                        {
                            Response.Redirect("/Default.aspx");
                        }
                    }
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }

            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.AbsoluteUri.Contains("actividad.aspx"))
                {
                    RadTabStrip1.SelectedIndex = 1;
                    RadMultiPage1.PageViews[1].Selected = true;
                }
            }

        }

        if (!IsPostBack)
        {
            this.CargarDatos();
        }
    }

    private void CargarDatos()
    {
        txtTitulo.Text = this.formacion.GetTitulo();
        this.Title = this.formacion.GetTitulo();
        literalTitulo.Text = this.formacion.GetTitulo();
        txtObjetivos.Text = this.formacion.GetObjetivos();
        cmbFormador.SelectedValue = this.formacion.GetIDFormador().ToString();
        dtFechaInicio.SelectedDate = formacion.GetFechaInicio();
        if (formacion.GetFechaFin()!=null)
        {
            dtFechaFin.SelectedDate = formacion.GetFechaFin();
        }
        txtDescripcion.Text = formacion.GetDescripcion();
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErrores.Visible = false;

        if (dtFechaInicio.SelectedDate==null)
        {
            blErrores.Items.Add("Debe indicar la fecha de inicio");
        }

        if (dtFechaInicio.SelectedDate!=null && dtFechaFin.SelectedDate!=null)
        {
            if (dtFechaInicio.SelectedDate>dtFechaFin.SelectedDate)
            {
                blErrores.Items.Add("La fecha de inicio no puede ser posterior a la fecha de finalización");
            }
        }

        if (blErrores.Items.Count>0)
        {
            panelErrores.Visible = true;
            panelErroresInterior.CssClass = "alert alert-danger";
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        this.GuardarDatos();
    }

    private void GuardarDatos()
    {
        this.panelErrores.Visible = false;
        this.blErrores.Items.Clear();
        if (this.ValidarCampos())
        {
            if (formacion.Actualizar(Int64.Parse(cmbFormador.SelectedValue),formacion.GetIDMateria(),
                (DateTime)dtFechaInicio.SelectedDate, dtFechaFin.SelectedDate,
                txtTitulo.Text,txtDescripcion.Text,txtObjetivos.Text,formacion.GetIDGoogleCalendar()))
            {
                radVentanaGuardar.RadAlert("Formación guardada con éxito", null, null, 
                    "eFilecoach", "", Interfaz.ICO_ok);
            }
            else
            {
                panelErrores.Visible = true;
                panelErroresInterior.CssClass = "alert alert-danger";
                blErrores.Items.Add("Se ha producido un error al guardar la formación");
            }
        }
    }

    protected void cmdEliminar_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea eliminar la formación? Se eliminará toda la información de la misma, así como los datos de las actividades.", "eliminarFormacion",
            null, null, null, "efileCoach", "/ico/borrar.png");
        
    }


    protected void cmdAsignar_Click(object sender, EventArgs e)
    {
        if (cmbAlumnos.CheckedItems.Count>0)
        {
            FormacionesAlumnos formacion_alumnos = new FormacionesAlumnos();
            for (int i=0;i<cmbAlumnos.CheckedItems.Count;i++)
            {
                formacion_alumnos.Insertar(this.formacion.GetID(), Int64.Parse(cmbAlumnos.CheckedItems[i].Value));
            }
            listaAlumnos.DataBind();
            cmbAlumnos.DataBind();
        }
        else
        {
            radVentana.RadAlert("Debe seleccionar algún alumno", null, null, "eFilecoach", "", Interfaz.ICO_alert);
        }
    }

    private void DesAsignarAlumnos()
    {
        if (listaAlumnos.CheckedItems.Count>0)
        {
            FormacionesAlumnos formacion_alumnos = new FormacionesAlumnos();
            for (int i=0;i<listaAlumnos.CheckedItems.Count;i++)
            {
                formacion_alumnos.Eliminar(this.id, Int64.Parse(listaAlumnos.CheckedItems[i].Value));
            }
            listaAlumnos.DataBind();
            cmbAlumnos.DataBind();
        }
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        switch (e.Argument)
        {
            case "okDesasignar":
                this.DesAsignarAlumnos();
                break;
            case "eliminarFormacion":
                if (formacion.Eliminar())
                {
                    Response.Redirect("formaciones.aspx");
                }
                else
                {
                    radVentana.RadAlert("Se ha producido un error al eliminar la formacion", null, null, "efilecoach", "", Interfaz.ICO_alert);
                }
                break;
        }
    }

    protected void cmdDesAsignar_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea desasignar los alumnos seleccionados?", "desasignarAlumnos",
            350, 150, null, "eFilecoach","/ico/borrar.png");
    }

    protected void cmdGuardarNuevaActividad_Click(object sender, EventArgs e)
    {
        if (this.ValidarNuevaActividad())
        {
            FormacionesActividades actividad = new FormacionesActividades();
            Int64 idactividad = actividad.Insertar(this.formacion.GetID(), 
                Int64.Parse(cmbProfesorResponsable.SelectedValue),
                Int64.Parse(cmbTipoActividad.SelectedValue), null, (Nullable<Decimal>)txtNumHoras.Value,
                txtTituloActividad.Text.Trim(),String.Empty,dtFechaInicioActividad.SelectedDate,
                dtFechaFinActividad.SelectedDate,
                txtInforme.Text,null,"");
            FormacionesActividadesAlumnos formacion_actividad_alumnos = new FormacionesActividadesAlumnos();
            if (idactividad != -1)
            {
                if (formacion_actividad_alumnos.InsertarAlumnosAsignadosFormacion(this.id,idactividad))
                {
                    txtTituloActividad.Text = "";
                    txtNumHoras.Value = null;
                    txtInforme.Text = String.Empty;
                    dtFechaFinActividad.SelectedDate = null;
                    dtFechaInicioActividad.SelectedDate = null;
                    cmbTipoActividad.SelectedIndex = 0;
                    cmbProfesorResponsable.ClearSelection();
                    grdActividades.DataBind();
                    Response.Redirect("actividad.aspx?id=" + Cifrado.cifrarParaUrl(idactividad.ToString()));
                }
                else
                {
                    radVentana.RadAlert("Se ha producido algún error al guardar la actividad", null, null,
                    "eFilecoach", "", Interfaz.ICO_alert);
                }
            }
            else
            {
                radVentana.RadAlert("Se ha producido algún error al guardar la actividad", null, null,
                    "eFilecoach", "", Interfaz.ICO_alert);
            }
        }
    }

    private Boolean ValidarNuevaActividad()
    {
        Boolean errores=false;

        if (txtTituloActividad.Text.Trim()==String.Empty)
        {
            errores = true;
            lblNuevaActividad.CssClass = "rojo";
        }
        else
        {
            lblNuevaActividad.CssClass = "";
        }

        if (cmbTipoActividad.SelectedIndex==0)
        {
            errores = true;
            lblTipoActividad.CssClass= "rojo";
        }
        else
        {
            lblTipoActividad.CssClass = "";
        }

        if (cmbProfesorResponsable.SelectedValue==String.Empty)
        {
            errores = true;
            lblProfesorResponsable.CssClass = "rojo";
        }
        else
        {
            lblProfesorResponsable.CssClass = "";
        }

        if (dtFechaInicioActividad.SelectedDate != null && dtFechaFinActividad.SelectedDate != null)
        {
            if (dtFechaInicioActividad.SelectedDate > dtFechaFinActividad.SelectedDate)
            {
                radVentana.RadAlert("La fecha de inicio de la actividad no puede ser posterior a la fecha de finalización", null, null, "eFilecoach", "", Interfaz.ICO_alert);
                return false;
            }
        }

        if (errores)
        {
            radVentana.RadAlert("Algunos campos tienen valores obligatorios", null,null, "eFilecoach", "", Interfaz.ICO_alert);
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmbTipoActividad_DataBound(object sender, EventArgs e)
    {
        cmbTipoActividad.Items.Insert(0, "Seleccionar tipo...");
    }

    protected void grdActividades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex!=-1)
        {
            HyperLink hlDetalles = (HyperLink)e.Row.FindControl("hlDetalles");
            Int64 idactividad = Int64.Parse(hlDetalles.NavigateUrl);
            FormacionesActividades actividad = new FormacionesActividades(idactividad);
            if (usuarioLogueado.EsUsuario())
            {
                if (actividad.GetIDProfesor()==usuarioLogueado.GetUsuario().GetID() ||
                    this.formacion.GetIDFormador()==usuarioLogueado.GetUsuario().GetID())
                {
                    hlDetalles.Visible = true;
                }
                else
                {
                    hlDetalles.Visible = false;
                }
            }
            CryptoAES c = new CryptoAES();
            hlDetalles.NavigateUrl = "/actividad.aspx?id=" + 
                c.cifrar(idactividad.ToString(), true);
        }
    }
    protected void chkIndicadores_Deleting(object sender, Telerik.Web.UI.RadListBoxDeletingEventArgs e)
    {
        if (e.Items.Count == 1)
        {
            if (this.formacion.EliminarIndicador(Int64.Parse(e.Items[0].Value)))
            {
                txtNuevoIndicador.Text = String.Empty;
            }
            else
            {
                radVentana.RadAlert("Se ha producido un error al eliminar el indicador", null, null, "eFilecoach", "", Interfaz.ICO_alert);
            }
        }
    }

    protected void cmdNuevoIndicador_Click(object sender, EventArgs e)
    {
        if (txtNuevoIndicador.Text.Trim() == String.Empty)
        {
            radVentana.RadAlert("Debe introducir el indicador", null, null, "efileCoach", "", Interfaz.ICO_alert);
        }
        else
        {
            if (this.formacion.InsertarIndicador(txtNuevoIndicador.Text.Trim()))
            {
                txtNuevoIndicador.Text = String.Empty;
                chkIndicadores.DataBind();
                txtNuevoIndicador.Focus();
            }
            else
            {
                radVentana.RadAlert("Se ha producido un error al dar de alta el indicador", null, null, "eFilecoach","", Interfaz.ICO_alert);
            }
        }
    }

    protected void grdActividades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName=="duplicar")
        {
            Int64 idactividad = Int64.Parse(e.CommandArgument.ToString());
            FormacionesActividades clase = new FormacionesActividades(idactividad);
            Int64 idnueva_actividad = clase.Duplicar(false);
            if (idnueva_actividad!=-1)
            {
                Response.Redirect("actividad.aspx?id=" + Cifrado.cifrarParaUrl(idnueva_actividad.ToString()));
            }
        }
    }
}