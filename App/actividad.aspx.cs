using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class actividad_form : System.Web.UI.Page
{
    Int64 id;
    FormacionesActividades actividad;
    CSesion csesion;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("inicio");
        }
        else
        {
            csesion = (CSesion)Session["csesion"];
            CryptoAES c = new CryptoAES();
            String descifrado = c.descifrar(Request.QueryString["id"], true);
            this.id = Convert.ToInt64(descifrado);
            this.hfIDActividad.Value = id.ToString();
            this.actividad = new FormacionesActividades(this.id);
            this.hfIDFormacion.Value = this.actividad.GetIDFormacion().ToString();
            Formaciones formacion = new Formaciones(actividad.GetIDFormacion());
            this.hfIDCuenta.Value = formacion.GetIDCuenta().ToString();

            if (UsuarioLogueado.EsCuenta())
            {
                if (UsuarioLogueado.GetCuenta().GetID() != formacion.GetIDCuenta())
                {
                    Response.Redirect("/Default.aspx");
                }
                lblNotasPrivadas.Visible = false;
                txtNotasPrivadas.Visible = false;
            }
            else
            {
                if (UsuarioLogueado.EsUsuario())
                {
                    cmbFormador.Enabled = false;
                    if (UsuarioLogueado.GetUsuario().GetID() != actividad.GetIDProfesor()
                        && UsuarioLogueado.GetUsuario().GetID() != formacion.GetIDFormador())
                    {
                        Response.Redirect("/formaciones.aspx");
                    }

                    if (UsuarioLogueado.GetUsuario().GetID() != actividad.GetIDProfesor())
                    {
                        lblNotasPrivadas.Visible = false;
                        txtNotasPrivadas.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/Default.aspx");
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
        CryptoAES c = new CryptoAES();
        txtTitulo.Text = this.actividad.GetTitulo();
        literalTitulo.Text = this.actividad.GetTitulo();
        this.Title = this.actividad.GetTitulo();
        cmbFormador.SelectedValue = this.actividad.GetIDProfesor().ToString();
        cmbTipoActividad.SelectedValue = actividad.GetIDTipo().ToString();
        txtNumHoras.Value = actividad.GetHoras();
        dtFechaInicio.SelectedDate = actividad.GetFechaInicio();
        dtFechaFin.SelectedDate = actividad.GetFechaFin();
        txtNotasPrivadas.Text = actividad.GetNotasPrivadas();
        txtInforme.Text = actividad.GetInforme();
        txtLugar.Text = actividad.GetLugar();
        txtDireccion.Text = actividad.GetDireccion();
        if (actividad.GetNotificable())
        {
            rblNotificaciones.SelectedValue = "1";
        }
        else
        {
            rblNotificaciones.SelectedValue = "0";
        }
        txtCP.Text = actividad.GetCodPos();
        txtPoblacion.Text = actividad.GetPoblacion();
        txtProvincia.Text = actividad.GetProvincia();
        txtPais.Text = actividad.GetPais();
        Formaciones formacion = new Formaciones(actividad.GetIDFormacion());
        hlVolver.NavigateUrl = "formacion.aspx?id=" + c.cifrar(actividad.GetIDFormacion().ToString(), true);
        hlVolver2.NavigateUrl = "formacion.aspx?id=" + c.cifrar(actividad.GetIDFormacion().ToString(), true);
        hlVolver.Text += formacion.GetTitulo();
        hlVolver2.Text += formacion.GetTitulo();
        if (actividad.GetIndividual()!=null)
        {
            rblIndividual.SelectedValue = Convert.ToInt16(actividad.GetIndividual()).ToString();
        }
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErrores.Visible = false;

        if (cmbTipoActividad.SelectedIndex==0)
        {
            blErrores.Items.Add("Debe seleccionar el tipo");
        }

        if (cmbFormador.SelectedValue==String.Empty)
        {
            blErrores.Items.Add("Debe seleccionar el profesor responsable");
        }

        if (dtFechaInicio.SelectedDate != null && dtFechaFin.SelectedDate != null)
        {
            if (dtFechaInicio.SelectedDate > dtFechaFin.SelectedDate)
            {
                blErrores.Items.Add("La fecha de inicio de la actividad no puede ser posterior a la fecha de finalización");
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
            Nullable<Int16> individual;
            if (rblIndividual.SelectedIndex==-1)
            {
                individual = null;
            }
            else
            {
                individual = Convert.ToInt16(rblIndividual.SelectedValue.ToString());
            }
            if (actividad.Actualizar(Int64.Parse(cmbFormador.SelectedValue),Int64.Parse(cmbTipoActividad.SelectedValue),
                individual,txtNumHoras.Value,txtTitulo.Text,
                txtNotasPrivadas.Text,dtFechaInicio.SelectedDate,dtFechaFin.SelectedDate,txtLugar.Text,
                txtDireccion.Text,txtCP.Text,txtPoblacion.Text,txtProvincia.Text,
                txtPais.Text,txtInforme.Text,
                Convert.ToBoolean(Convert.ToInt32(rblNotificaciones.SelectedValue)),null,
                actividad.GetIDGoogleCalendar()))
            {
                radVentanaGuardar.RadAlert("Actividad guardada con éxito", null, null, 
                    "eFilecoach", "", Interfaz.ICO_ok);
            }
            else
            {
                panelErrores.Visible = true;
                panelErroresInterior.CssClass = "alert alert-danger";
                blErrores.Items.Add("Se ha producido un error al guardar la actividad");
            }
        }
    }

    protected void cmdEliminar_Click(object sender, EventArgs e)
    {
        if (listaAlumnos.Items.Count>0)
        {
            radVentana.RadAlert("No se puede eliminar una actividad con alumnos asignados",
                null, null, "eFilecoach", "");
        }
        else
        {
            radVentana.RadConfirm("¿Desea eliminar la actividad?", "eliminarActividad",
            null, null, null, "eFilecoach", "/ico/borrar.png");
        }
        
    }


    protected void cmdAsignar_Click(object sender, EventArgs e)
    {
        if (cmbAlumnos.CheckedItems.Count>0)
        {
            FormacionesActividadesAlumnos actividades_alumnos = new FormacionesActividadesAlumnos();
            for (int i=0;i<cmbAlumnos.CheckedItems.Count;i++)
            {
                actividades_alumnos.Insertar(this.actividad.GetID(), Int64.Parse(cmbAlumnos.CheckedItems[i].Value));
            }
            listaAlumnos.DataBind();
            cmbAlumnos.DataBind();
            rlAlumnosFormacionAsistencia.DataBind();
        }
        else
        {
            radVentana.RadAlert("Debe seleccionar algún alumno", null, null, "eFilecoach", "",Interfaz.ICO_alert);
        }
    }

    private void DesAsignarAlumnos()
    {
        if (listaAlumnos.CheckedItems.Count>0)
        {
            FormacionesActividadesAlumnos formacion_alumnos = new FormacionesActividadesAlumnos();
            for (int i=0;i<listaAlumnos.CheckedItems.Count;i++)
            {
                formacion_alumnos.Eliminar(this.id, Int64.Parse(listaAlumnos.CheckedItems[i].Value));
            }
            listaAlumnos.DataBind();
            cmbAlumnos.DataBind();
            rlAlumnosFormacionAsistencia.DataBind();
        }
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        String funcion;
        if (e.Argument.Contains(","))
        {
            String[] x = e.Argument.Split(',') ;
            funcion = x[0];
        }
        else
        {
            funcion = e.Argument;
        }

        switch (funcion)
        {
            case "okDesasignar":
                this.DesAsignarAlumnos();
                break;
            case "eliminarFecha":

                break;
            case "eliminarValoracion":
                FormacionesTiposValoraciones valoracion = 
                    new FormacionesTiposValoraciones(Int64.Parse(listaValoraciones.SelectedValue));
                if (valoracion.Eliminar())
                {
                    listaValoraciones.DataBind();
                }
                break;
            case "eliminarActividad":
                if (actividad.Eliminar())
                {
                    CryptoAES c = new CryptoAES();
                    Formaciones formacion = new Formaciones(actividad.GetIDFormacion());
                    Response.Redirect("formacion.aspx?id=" + c.cifrar(actividad.GetIDFormacion().ToString(), true));
                }
                else
                {
                    panelErrores.Visible = true;
                    blErrores.Items.Clear();
                    blErrores.Items.Add("Se ha producido un error al eliminar la formación");
                }
                break;
        }
    }

    protected void cmdDesAsignar_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea desasignar los alumnos seleccionados?", "desasignarAlumnos",
            null, null, null, "eFilecoach","/ico/borrar.png");
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
            CryptoAES c = new CryptoAES();
            hlDetalles.NavigateUrl = "/actividad.aspx?id=" + c.cifrar(idactividad.ToString(), true);
        }
    }

    protected void cmdGuardarHorario_Click(object sender, EventArgs e)
    {
        if (dtFechaHoraFin.SelectedDate!=null && dtFechaHoraInicio.SelectedDate!=null)
        {
            FormacionesActividadesFechas fechas = new FormacionesActividadesFechas();
            if (fechas.Insertar(this.actividad.GetID(),
                (DateTime)dtFechaHoraInicio.SelectedDate, (DateTime)dtFechaHoraFin.SelectedDate)!=-1)
            {
                dtFechaHoraInicio.SelectedDate = null;
                dtFechaHoraFin.SelectedDate = null;
                grdFechas.DataBind();
            }
            else
            {
                radVentana.RadAlert("No se han podido guardar las fechas", null, null, "eFileCoach", "", Interfaz.ICO_alert);
            }
        }
        else
        {
            radVentana.RadAlert("Debe seleccionar la fecha inicio y fin", null, null, "eFileCoach", "", Interfaz.ICO_alert);
        }
    }

    protected void cmdQuitarValoracion_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea eliminar la valoracion?", "eliminarValoracion",
            null, null, null, "eFilecoach", "/ico/borrar.png");
    }

    private Boolean ValidarNuevaValoracion()
    {
        Boolean errores = false;
        if (txtNuevaValoracion.Text.Trim()==String.Empty)
        {
            lblNuevaValoracion.CssClass = "rojo";
            errores = true;
        }
        else
        {
            lblNuevaValoracion.CssClass = "";
        }

        if (cmbTipoValoracion.SelectedValue=="")
        {
            lblTipoValoracion.CssClass = "rojo";
            errores = true;
        }
        else
        {
            lblTipoValoracion.CssClass = "";
        }

        if (errores)
        {
            radVentana.RadAlert("Hay valores requeridos en la valoración", null, null, "eFileCoach", "", Interfaz.ICO_alert);
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmdNuevaValoracion_Click(object sender, EventArgs e)
    {
        if (this.ValidarNuevaValoracion())
        {
            FormacionesTiposValoraciones valoracion = new FormacionesTiposValoraciones();
            if (valoracion.Insertar(this.id,txtNuevaValoracion.Text.Trim(),
                (Int16)(listaValoraciones.Items.Count+1),Int64.Parse(cmbTipoValoracion.SelectedValue))!=-1)
            {
                txtNuevaValoracion.Text = "";
                cmbTipoValoracion.ClearSelection();
                listaValoraciones.DataBind();
            }
        }
    }

    protected void grdFechas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "eliminarFecha":
                Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
                FormacionesActividadesFechas fecha = new FormacionesActividadesFechas(id);
                if (fecha.Eliminar())
                {
                    this.grdFechas.DataBind();
                }
                break;
        }
    }

    protected void rlAlumnosFormacionAsistencia_DataBound(object sender, EventArgs e)
    {
        //Recorremos para activar o no los checks
        for (int i=0;i<rlAlumnosFormacionAsistencia.Items.Count;i++)
        {
            Int64 idasignacion = Int64.Parse(rlAlumnosFormacionAsistencia.Items[i].Value);
            FormacionesActividadesAlumnos asignacion = new FormacionesActividadesAlumnos(idasignacion);
            rlAlumnosFormacionAsistencia.Items[i].Checked = asignacion.GetAsistencia();
        }
    }

    protected void rlAlumnosFormacionAsistencia_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
    {
        //
        int i = 0;
        Int64 idasignacion = Int64.Parse(e.Item.Value);
        FormacionesActividadesAlumnos asignacion = new FormacionesActividadesAlumnos(idasignacion);
        asignacion.ActualizarAsistencia(e.Item.Checked);
    }

    protected void rlAlumnosFormacionAsistencia_CheckAllCheck(object sender, Telerik.Web.UI.RadListBoxCheckAllCheckEventArgs e)
    {
        for (int i = 0; i < rlAlumnosFormacionAsistencia.Items.Count; i++)
        {
            Int64 idasignacion = Int64.Parse(rlAlumnosFormacionAsistencia.Items[i].Value);
            FormacionesActividadesAlumnos asignacion = new FormacionesActividadesAlumnos(idasignacion);
            asignacion.ActualizarAsistencia(rlAlumnosFormacionAsistencia.Items[i].Checked);
        }
    }

    protected void rlAlumnosFormacionAsistencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i=0;i<rlAlumnosFormacionAsistencia.Items.Count;i++)
        {
            rlAlumnosFormacionAsistencia.Items[i].Selected = false;
        }
    }
}