using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class proceso_form : System.Web.UI.Page
{
    Int64 id;
    Formaciones proceso;
    //CSesion csesion;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] ==null)
        {
            Response.Redirect("inicio");
        }
        else
        {
            //csesion = (CSesion)Session["csesion"];
            CryptoAES c = new CryptoAES();
            String descifrado = c.descifrar(Request.QueryString["id"], true);
            this.id = Convert.ToInt64(descifrado);
            this.hfIDProceso.Value = id.ToString();
            this.proceso = new Formaciones(this.id);
            Int64 idcuenta = proceso.GetIDCuenta();
            hfIDCuenta.Value = idcuenta.ToString();
            if (usuarioLogueado.EsCuenta())
            {
                if (usuarioLogueado.GetCuenta().GetID() != proceso.GetIDCuenta())
                {
                    Response.Redirect("/Default.aspx");
                }
            }
            else
            {
                if (usuarioLogueado.EsUsuario())
                {
                    this.hfIDUsuario.Value = usuarioLogueado.GetUsuario().GetID().ToString();
                    if (usuarioLogueado.GetUsuario().GetID()==proceso.GetIDFormador())
                    {
                        //Soy el coordinador, lo veo todo aunque no pueda acceder
                        grdProcesos.DataSourceID = "odsProcesos";
                    }
                    else
                    {
                        //Soy un coach, sólo veo lo mio
                        grdProcesos.DataSourceID = "odsProcesosUsuario";
                    }
                    cmbCoach.Enabled = false;
                    if (usuarioLogueado.GetUsuario().GetID() != proceso.GetIDFormador())
                    {
                        //Tenemos que ver si tiene alguna sesión
                        if (Procesos.GetCoachAsignado(usuarioLogueado.GetUsuario().GetID(),
                            proceso.GetID()))
                        {
                            panelContenidoPrincipal.Enabled = false;
                            cmdGuardar.Visible = false;
                            cmdEliminar.Visible = false;
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

            if (Request.UrlReferrer!=null)
            {
                if (Request.UrlReferrer.AbsoluteUri.Contains("sesion.aspx"))
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
        if (this.usuarioLogueado.EsCuenta())
        {
            panelDisponiblesParaAsignar.Visible = true;
        }
        else
        {
            panelDisponiblesParaAsignar.Visible = false;
        }
        txtTitulo.Text = this.proceso.GetTitulo();
        this.Title = this.proceso.GetTitulo();
        this.literalTitulo.Text = this.proceso.GetTitulo();
        cmbCoach.SelectedValue = this.proceso.GetIDFormador().ToString();
        dtFechaInicio.SelectedDate = proceso.GetFechaInicio();
        txtObjetivos.Text = proceso.GetObjetivos();
        if (proceso.GetFechaFin()!=null)
        {
            dtFechaFin.SelectedDate = proceso.GetFechaFin();
        }
        txtDescripcion.Text = proceso.GetDescripcion();
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErrores.Visible = false;

        if (dtFechaInicio.SelectedDate==null)
        {
            blErrores.Items.Add("Debe indicar la fecha de inicio");
        }

        if (dtFechaInicio.SelectedDate != null && dtFechaFin.SelectedDate != null)
        {
            if (dtFechaInicio.SelectedDate > dtFechaFin.SelectedDate)
            {
                blErrores.Items.Add("La fecha de inicio del proceso no puede ser posterior a la fecha de finalización");
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
            if (proceso.Actualizar(Int64.Parse(cmbCoach.SelectedValue), null,
                (DateTime)dtFechaInicio.SelectedDate, dtFechaFin.SelectedDate, 
                txtTitulo.Text,txtDescripcion.Text,txtObjetivos.Text,proceso.GetIDGoogleCalendar()))
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
        if (proceso.Eliminar())
        {
            Response.Redirect("procesos.aspx");
        }
        else
        {
            radVentana.RadAlert("No se ha podido eliminar el proceso de coachig", null, null, "efileCoach", "", Interfaz.ICO_alert);
        }
    }


    protected void cmdAsignar_Click(object sender, EventArgs e)
    {
        if (cmbAlumnos.CheckedItems.Count>0)
        {
            FormacionesAlumnos formacion_alumnos = new FormacionesAlumnos();
            for (int i=0;i<cmbAlumnos.CheckedItems.Count;i++)
            {
                formacion_alumnos.Insertar(this.proceso.GetID(), Int64.Parse(cmbAlumnos.CheckedItems[i].Value));
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
            
        }
    }

    protected void cmdDesAsignar_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea desasignar los alumnos seleccionados?", "desasignarAlumnos",
            null, null, null, "eFilecoach","/ico/borrar.png");
    }

    protected void cmdGuardarNuevaSesion_Click(object sender, EventArgs e)
    {
        if (this.ValidarNuevaSesion())
        {
            FormacionesActividades sesion = new FormacionesActividades();
            Int64 idsesion = sesion.Insertar(this.proceso.GetID(), 
                Int64.Parse(cmbCoachResponsable.SelectedValue),
                Int64.Parse(cmbTipoSesion.SelectedValue), null, (Nullable<Decimal>)txtNumHoras.Value,
                "",String.Empty,
                dtFechaInicioSesion.SelectedDate,dtFechaFinSesion.SelectedDate,
                txtInforme.Text.Trim(), Int32.Parse(txtTituloSesion.Text.Trim()),"");
            FormacionesActividadesAlumnos proceso_sesion_coachees = new FormacionesActividadesAlumnos();
            if (idsesion != -1)
            {
                if (proceso_sesion_coachees.InsertarAlumnosAsignadosFormacion(this.id, idsesion))
                {
                    txtTituloSesion.Text = "";
                    txtInforme.Text = String.Empty;
                    dtFechaFinSesion.SelectedDate = null;
                    dtFechaInicioSesion.SelectedDate = null;
                    txtNumHoras.Value = null;
                    cmbTipoSesion.SelectedIndex = 0;
                    cmbCoachResponsable.ClearSelection();
                    grdProcesos.DataBind();
                    Response.Redirect("sesion.aspx?id=" + Cifrado.cifrarParaUrl(idsesion.ToString()));
                }
                else
                {
                    radVentana.RadAlert("Se ha producido algún error al guardar la sesión", null, null,
                        "eFilecoach", "", Interfaz.ICO_alert);
                }
            }
            else
            {
                radVentana.RadAlert("Se ha producido algún error al guardar la sesión", null, null,
                    "eFilecoach", "", Interfaz.ICO_alert);
            }
        }
    }

    private Boolean ValidarNuevaSesion()
    {
        Boolean errores=false;

        if (!Interfaz.EsInt64(txtTituloSesion.Text.Trim()))
        {
            errores = true;
            radVentana.RadAlert("El número de sesión debe tener un valor numérico",
                    null, null, "eFilecoach", "", Interfaz.ICO_alert);
            return false;
        }
        else
        {
            if (this.proceso.NumeroSesionEnUso(Int32.Parse(txtTituloSesion.Text.Trim())))
            {
                errores = true;
                radVentana.RadAlert("El número de sesión está en uso",
                        null, null, "eFilecoach", "", Interfaz.ICO_alert);
                return false;
            }
            else
            {
                lblNuevaActividad.CssClass = "";
            }
        }

        if (cmbTipoSesion.SelectedIndex==0)
        {
            errores = true;
            lblTipoSesion.CssClass= "rojo";
        }
        else
        {
            lblTipoSesion.CssClass = "";
        }

        if (cmbCoachResponsable.SelectedValue==String.Empty)
        {
            errores = true;
            lblCoachResponsable.CssClass = "rojo";
        }
        else
        {
            lblCoachResponsable.CssClass = "";
        }

        if (dtFechaInicioSesion.SelectedDate != null && dtFechaFinSesion.SelectedDate != null)
        { 
            if (dtFechaInicioSesion.SelectedDate > dtFechaFinSesion.SelectedDate)
            {
                radVentana.RadAlert("La fecha de inicio de la sesión no puede ser posterior a la fecha de finalización", 
                    null, null, "eFilecoach", "", Interfaz.ICO_alert);
                return false;
            }
        }

        if (errores)
        {
            radVentana.RadAlert("Algunos campos tienen valores obligatorios", null, null, "eFilecoach", "", Interfaz.ICO_alert);
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmbTipoSesion_DataBound(object sender, EventArgs e)
    {
        cmbTipoSesion.Items.Insert(0, "Seleccionar tipo...");
    }

    protected void grdProcesos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex!=-1)
        {
            HyperLink hlDetalles = (HyperLink)e.Row.FindControl("hlDetalles");
            Int64 idsesion = Int64.Parse(hlDetalles.NavigateUrl);
            FormacionesActividades sesion = new FormacionesActividades(idsesion);
            if (usuarioLogueado.EsUsuario())
            {
                if (sesion.GetIDProfesor() == usuarioLogueado.GetUsuario().GetID() ||
                    this.proceso.GetIDFormador() == usuarioLogueado.GetUsuario().GetID())
                {
                    hlDetalles.Visible = true;
                }
                else
                {
                    hlDetalles.Visible = false;
                }
            }
            CryptoAES c = new CryptoAES();
            hlDetalles.NavigateUrl = "/sesion.aspx?id=" + c.cifrar(idsesion.ToString(), true);
        }
    }

    protected void cmdNuevoIndicador_Click(object sender, EventArgs e)
    {
        if (txtNuevoIndicador.Text.Trim()==String.Empty)
        {
            radVentana.RadAlert("Debe introducir el indicador", 
                null, null, "eFilecoach", "", Interfaz.ICO_alert);
        }
        else
        {
            if (this.proceso.InsertarIndicador(txtNuevoIndicador.Text.Trim()))
            {
                txtNuevoIndicador.Text = String.Empty;
                chkIndicadores.DataBind();
                txtNuevoIndicador.Focus();
            }
            else
            {
                radVentana.RadAlert("Se ha producido un error al dar de alta el indicador", null, null, "eFilecoach", "", Interfaz.ICO_alert);
            }
        }
    }

    protected void chkIndicadores_Deleting(object sender, Telerik.Web.UI.RadListBoxDeletingEventArgs e)
    {
        if (e.Items.Count==1)
        {
            if (this.proceso.EliminarIndicador(Int64.Parse(e.Items[0].Value)))
            {
                txtNuevoIndicador.Text = String.Empty;
            }
            else
            {
                radVentana.RadAlert("Se ha producido un error al eliminar el indicador", null, null, "eFilecoach", "", Interfaz.ICO_alert);
            }
        }
    }

    protected void grdProcesos_DataBound(object sender, EventArgs e)
    {

    }

    protected void grdProcesos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "duplicar")
        {
            Int64 idactividad = Int64.Parse(e.CommandArgument.ToString());
            FormacionesActividades clase = new FormacionesActividades(idactividad);
            Int64 idnueva_actividad = clase.Duplicar(true);
            if (idnueva_actividad != -1)
            {
                Response.Redirect("sesion.aspx?id=" + Cifrado.cifrarParaUrl(idnueva_actividad.ToString()));
            }
        }
    }
}