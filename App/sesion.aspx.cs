using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class sesion_form : System.Web.UI.Page
{
    Int64 id;
    FormacionesActividades sesion;
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
            this.hfIDSesion.Value = id.ToString();
            this.sesion = new FormacionesActividades(this.id);
            this.hfIDProceso.Value = this.sesion.GetIDFormacion().ToString();
            Formaciones proceso = new Formaciones(sesion.GetIDFormacion());
            this.hfIDCuenta.Value = proceso.GetIDCuenta().ToString();
            if (usuarioLogueado.EsCuenta())
            {
                if (usuarioLogueado.GetCuenta().GetID() != proceso.GetIDCuenta())
                {
                    Response.Redirect("/Default.aspx");
                }
                else
                {
                    if (usuarioLogueado.GetCuenta().GetID()!=sesion.GetIDProfesor())
                    {
                        //Si soy una cuenta pero no soy el coach de la sesión
                        //oculto el informe
                        txtNotasPrivadas.Visible = false;
                        lblNotasPrivadas.Visible = false;
                    }
                }
            }
            else
            {
                if (usuarioLogueado.EsUsuario())
                {
                    if (usuarioLogueado.GetUsuario().GetID() != sesion.GetIDProfesor()
                        && proceso.GetIDFormador()!=usuarioLogueado.GetUsuario().GetID())
                    {
                        Response.Redirect("/procesos.aspx");
                    }

                    if (usuarioLogueado.GetUsuario().GetID()!=sesion.GetIDProfesor())
                    {
                        txtNotasPrivadas.Visible = false;
                        lblNotasPrivadas.Visible = false;
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
        txtTitulo.Text = this.sesion.GetNumeroSesionTXT().ToString();
        literalTitulo.Text = this.sesion.GetTitulo();
        this.Title = this.sesion.GetTitulo();
        cmbCoach.SelectedValue = this.sesion.GetIDProfesor().ToString();
        cmbTipoSesion.SelectedValue = sesion.GetIDTipo().ToString();
        txtNumHoras.Value = sesion.GetHoras();
        dtFechaInicio.SelectedDate = sesion.GetFechaInicio();
        dtFechaFin.SelectedDate = sesion.GetFechaFin();
        txtLugar.Text = sesion.GetLugar();
        txtDireccion.Text = sesion.GetDireccion();
        txtCP.Text = sesion.GetCodPos();
        txtPoblacion.Text = sesion.GetPoblacion();
        if (sesion.GetNotificable())
        {
            rblNotificaciones.SelectedValue = "1";
        }
        else
        {
            rblNotificaciones.SelectedValue = "0";
        }
        txtProvincia.Text = sesion.GetProvincia();
        txtPais.Text = sesion.GetPais();
        txtNotasPrivadas.Text = sesion.GetNotasPrivadas();
        txtInforme.Text = sesion.GetInforme();
        Formaciones proceso = new Formaciones(sesion.GetIDFormacion());
        hlVolver.NavigateUrl = "proceso.aspx?id=" + c.cifrar(sesion.GetIDFormacion().ToString(), true);
        hlVolver2.NavigateUrl = "proceso.aspx?id=" + c.cifrar(sesion.GetIDFormacion().ToString(), true);
        hlVolver.Text += proceso.GetTitulo();
        hlVolver2.Text += proceso.GetTitulo();
        if (sesion.GetIndividual()!=null)
        {
            rblIndividual.SelectedValue = Convert.ToInt16(sesion.GetIndividual()).ToString();
        }
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErrores.Visible = false;

        if (!Interfaz.EsInt64(txtTitulo.Text.Trim()))
        {
            blErrores.Items.Add("El numero de la sesión debe estar bien escrito (formato numérico)");
        }
        else
        {
            Formaciones proceso = new Formaciones(this.sesion.GetIDFormacion());
            if (proceso.NumeroSesionEnUso(Int32.Parse(txtTitulo.Text.Trim()),this.sesion.GetID()))
            {
                blErrores.Items.Add("El numero de la sesión ya está siendo utilizado por otra sesión");
            }
        }

        if (cmbTipoSesion.SelectedIndex==0)
        {
            blErrores.Items.Add("Debe seleccionar el tipo");
        }

        if (cmbCoach.SelectedValue==String.Empty)
        {
            blErrores.Items.Add("Debe seleccionar el coach");
        }

        if (dtFechaInicio.SelectedDate != null && dtFechaFin.SelectedDate != null)
        {
            if (dtFechaInicio.SelectedDate > dtFechaFin.SelectedDate)
            {
                blErrores.Items.Add("La fecha de inicio de la sesión no puede ser posterior a la fecha de finalización");
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

            if (sesion.Actualizar(Int64.Parse(cmbCoach.SelectedValue),Int64.Parse(cmbTipoSesion.SelectedValue),
                individual,txtNumHoras.Value,String.Empty,
                txtNotasPrivadas.Text,dtFechaInicio.SelectedDate,
                dtFechaFin.SelectedDate,
                txtLugar.Text,
                txtDireccion.Text, txtCP.Text, txtPoblacion.Text,
                txtProvincia.Text, txtPais.Text,
                txtInforme.Text,Convert.ToBoolean(Convert.ToInt32(rblNotificaciones.SelectedValue)),
                Int32.Parse(txtTitulo.Text.Trim()),sesion.GetIDGoogleCalendar()))
            {
                radVentanaGuardar.RadAlert("Datos guardados con éxito", null, null, 
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
        if (listaPersonas.Items.Count>0)
        {
            radVentana.RadAlert("No se puede eliminar una sesión con coachees asignados",
                null, null, "eFilecoach", "");
        }
        else
        {
            //radVentana.RadConfirm("¿Desea eliminar la sesión?", "eliminarActividad",
            //null, null, null, "eFilecoach", "/ico/borrar.png");
            if (sesion.Eliminar())
            {
                CryptoAES c = new CryptoAES();
                Formaciones proceso = new Formaciones(sesion.GetIDFormacion());
                Response.Redirect("proceso.aspx?id=" + c.cifrar(sesion.GetIDFormacion().ToString(), true));
            }
            else
            {
                panelErrores.Visible = true;
                blErrores.Items.Clear();
                blErrores.Items.Add("Se ha producido un error al eliminar la sesión");
            }
        }
        
    }

    protected void cmdAsignar_Click(object sender, EventArgs e)
    {
        if (cmbPersonas.CheckedItems.Count>0)
        {
            FormacionesActividadesAlumnos actividades_personas = new FormacionesActividadesAlumnos();
            for (int i=0;i<cmbPersonas.CheckedItems.Count;i++)
            {
                actividades_personas.Insertar(this.sesion.GetID(), Int64.Parse(cmbPersonas.CheckedItems[i].Value));
            }
            listaPersonas.DataBind();
            cmbPersonas.DataBind();
            rlAlumnosFormacionAsistencia.DataBind();

        }
        else
        {
            radVentana.RadAlert("Debe seleccionar algún coachee", null, null, "eFilecoach", "", Interfaz.ICO_alert);
        }
    }

    private void DesAsignarAlumnos()
    {
        if (listaPersonas.CheckedItems.Count>0)
        {
            FormacionesActividadesAlumnos proceso_personas = new FormacionesActividadesAlumnos();
            for (int i=0;i<listaPersonas.CheckedItems.Count;i++)
            {
                proceso_personas.Eliminar(this.id, Int64.Parse(listaPersonas.CheckedItems[i].Value));
            }
            listaPersonas.DataBind();
            cmbPersonas.DataBind();
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
                rlAlumnosFormacionAsistencia.DataBind();
                break;
            case "eliminarSesion":
                if (sesion.Eliminar())
                {
                    CryptoAES c = new CryptoAES();
                    Formaciones proceso = new Formaciones(sesion.GetIDFormacion());
                    Response.Redirect("proceso.aspx?id=" + c.cifrar(sesion.GetIDFormacion().ToString(), true));
                }
                else
                {
                    panelErrores.Visible = true;
                    blErrores.Items.Clear();
                    blErrores.Items.Add("Se ha producido un error al eliminar la sesión");
                }
                break;
        }
    }

    protected void cmdDesAsignar_Click(object sender, EventArgs e)
    {
        radVentana.RadConfirm("¿Desea desasignar los coachees seleccionados?", "desasignarAlumnos",
            350, 150, null, "eFilecoach","/ico/borrar.png");
    }

    protected void cmbTipoSesion_DataBound(object sender, EventArgs e)
    {
        cmbTipoSesion.Items.Insert(0, "Seleccionar tipo...");
    }

    protected void rlAlumnosFormacionAsistencia_DataBound(object sender, EventArgs e)
    {
        //Recorremos para activar o no los checks
        for (int i = 0; i < rlAlumnosFormacionAsistencia.Items.Count; i++)
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
        for (int i = 0; i < rlAlumnosFormacionAsistencia.Items.Count; i++)
        {
            rlAlumnosFormacionAsistencia.Items[i].Selected = false;
        }
    }

}