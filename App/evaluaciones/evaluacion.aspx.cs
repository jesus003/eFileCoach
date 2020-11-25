using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Controlador.Diagnosticos;
using Controlador.DiagnosticosPlantillas;
using Controlador;
using Newtonsoft.Json;
using Telerik.Web.UI;

public partial class evaluaciones_evaluacion : System.Web.UI.Page
{
    Int64 idevaluacion = -1;
    Int64 idusuario;
    Diagnosticos evaluacion;
    StreamReader Ficherocsv;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.idusuario = Convert.ToInt64(Session["idusuario"].ToString());
        if (!Page.IsPostBack)
        {
            literalMensajeAyudaCSV.Text = Interfaz.GetValor("evaluadores_ayuda_csv");
            if (Request.QueryString["id"] != null)
            {
                cmbTipoEncuesta.Enabled = false;
                this.idevaluacion = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"],true));
                hfIDEvaluacion.Value = this.idevaluacion.ToString();
                this.evaluacion = new Diagnosticos(this.idevaluacion);
                if (evaluacion.GetTieneDatos())
                {
                    if (Session["idusuario"].ToString()!=evaluacion.GetIDUsuario().ToString())
                    {
                        Session.Clear();
                        Response.Redirect("/default.aspx");
                    }
                    else
                    {
                        txtNombre.Text = evaluacion.GetNombre();
                        cmbTipoEncuesta.SelectedValue = evaluacion.GetIDTipo().ToString();
                        this.ActualizarVisibilidadClientes();
                        hfIDTipo.Value = evaluacion.GetIDTipo().ToString();
                        cmbPlantillas.DataBind();
                        this.Title = "Encuesta: " + evaluacion.GetNombre();
                        txtFechaAlta.Text = evaluacion.GetFecha().ToString("dd/MM/yyyy");
                        pvDimensiones.Selected = true;
                        if (evaluacion.GetIDCliente() != -1)
                        {
                            Personas persona = new Personas(idevaluacion, evaluacion.GetIDCliente());
                            cmbClientes.SelectedValue = evaluacion.GetIDCliente().ToString();
                            hfIDAuto.Value = persona.GetIdPersona().ToString();
                        }
                        else
                        {
                            hfIDAuto.Value = "0";
                        }

                        //Compruebo todos los usuarios
                        for (int i = 0; i < grdUsuarios.Rows.Count; i++)
                        {
                            HiddenField hfestado = (HiddenField)grdUsuarios.Rows[i].FindControl("hfEstadoEvaluador");
                            //Si el estado es distinto de 0 significa que se le ha enviado la evaluación
                            if (hfestado.Value != "0")
                            {
                                //Si se le ha enviado la evaluacion no permito editar ni eliminar al usuario del Grid view
                                Button cmdEliminargrdUsuarios = (Button)grdUsuarios.Rows[i].FindControl("cmdEliminar");
                                //cmdEliminargrdUsuarios.Enabled = false;
                                Button cmdEditar = (Button)grdUsuarios.Rows[i].FindControl("cmdEditar");
                                //cmdEditar.Enabled = false;
                            }
                        }

                        cmbClientes.Enabled = false;
                        //Si el estado de la evaluación es editable (1) permito todo menos el combo de clientes
                        if (evaluacion.GetIDEstado() == 1)
                        {
                            tabsCabeceraOpciones.Visible = true;
                            tabsOpciones.Visible = true;
                            hlEnviarEnlaces.Enabled = true;
                            hlEnviarEnlaces.NavigateUrl = String.Format("evaluacion-envios.aspx?id={0}",
                                Cifrado.cifrarParaUrl(this.idevaluacion.ToString()));
                        }
                        //Si el estado es 2 o 3 Ya no puedo permitir modificar las dimensiones ni las preguntas
                        else if (evaluacion.GetIDEstado() == 2 || evaluacion.GetIDEstado() == 3)
                        {
                            tabsCabeceraOpciones.Visible = true;
                            tabsOpciones.Visible = true;
                            //Recorro todas las dimensiones
                            for (int i = 0; i < grdDimensiones.Rows.Count; i++)
                            {
                                Button cmdEliminargrd = (Button)grdDimensiones.Rows[i].FindControl("cmdEliminar");
                                //Elimino el onclientclick porque aunque esté deshabilitado sigue saliendo el popup arriba
                                cmdEliminargrd.OnClientClick = "";
                                //Deshabilito el boton de eliminar el de bajar y el de subir
                                cmdEliminargrd.Enabled = false;
                                LinkButton cmdBajar = (LinkButton)grdDimensiones.Rows[i].FindControl("cmdBajar");
                                cmdBajar.Enabled = false;
                                LinkButton cmdSubir = (LinkButton)grdDimensiones.Rows[i].FindControl("cmdSubir");
                                cmdSubir.Enabled = false;

                            }

                            //Deshabilito las plantillas
                            cmbPlantillas.Enabled = false;
                            cmdImportarPlantilla.Enabled = false;

                            if (evaluacion.GetIDEstado() == 2)
                            {
                                cmdGuardar.CommandArgument = "Cerrar";
                                cmdGuardar.Text = "Cerrar encuesta";
                            }
                            else
                            {
                                cmdGuardar.CommandArgument = "Abrir";
                                cmdGuardar.Text = "Abrir la encuesta";
                            }

                            txtNombre.Enabled = false;

                            hlEnviarEnlaces.Enabled = true;
                            hlEnviarEnlaces.NavigateUrl = String.Format("evaluacion-envios.aspx?id={0}",
                                Cifrado.cifrarParaUrl(this.idevaluacion.ToString()));
                            //No permito guardar dimensiones
                            cmdGuardarDimension.Enabled = false;
                            //Si el estado es 3 Es que esta cerrada


                        }
                        if (evaluacion.GetIDTipo() == 1)
                        {
                            hlInforme.NavigateUrl = "informe-evaluacion.aspx?id=" +
                                Cifrado.cifrarParaUrl(this.idevaluacion.ToString());
                        }
                        else
                        {
                            hlInforme.NavigateUrl = "informe-evaluacion-texto.aspx?id=" +
                                Cifrado.cifrarParaUrl(this.idevaluacion.ToString());
                        }
                        if (evaluacion.GetCuantosGrupos() == evaluacion.GetCuantosGruposResponden())
                        {
                            hlInforme.ToolTip = "";
                        }
                        else
                        {
                            hlInforme.ToolTip = "Hay grupos que contienen usuarios sin responder. Los datos pueden no ser precisos.";
                        }

                        this.ActivarDesactivarEnviar();
                    }
                }
                else
                {
                    Session.Clear();
                    Response.Redirect("/default.aspx");
                }
                
            }
            else
            {
                //Este caso es para las nuevas dimensiones
                cmbTipoEncuesta.Enabled = true;
                cmdEliminar.Enabled = false;
                hlEnviarEnlaces.Enabled = false;
                cmdEliminar.CssClass = "btn btn-default btn-block";
                txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            }
        }
        else
        {

            if (Request.QueryString["id"] != null)
            {
                this.idevaluacion = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"],true));
                this.evaluacion = new Diagnosticos(this.idevaluacion);
                txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                cmbTipoEncuesta.Enabled = false;
            }
        }
    }

    protected void ActivarDesactivarEnviar()
    {
        //Compruebo que la evaluación contenga preguntas para 
        //habilitar o deshabilitar el botón de enviar
        if (!evaluacion.ContarPreguntasDiagnostico())
        {
            hlEnviarEnlaces.Enabled = false;
            hlEnviarEnlaces.ToolTip = "Hay evaluaciones sin preguntas";
            panelDimensionesSinPregunta.Visible = true;
        }
        else
        {
            hlEnviarEnlaces.Enabled = true;
            hlEnviarEnlaces.ToolTip = "";
            panelDimensionesSinPregunta.Visible = false;
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
    }

    protected void cmbPlantillas_DataBound(object sender, EventArgs e)
    {
        cmbPlantillas.Items.Insert(0, "Seleccione una plantilla...");
    }

    protected void cmbClientes_DataBound(object sender, EventArgs e)
    {
        cmbClientes.Items.Insert(0, "Listado de clientes...");
    }

    protected void odsResultados_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["idDiagnostico"] = this.idevaluacion;
    }

    /// <summary>
    /// Preparado para importar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportar_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Preparado para Enviar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        if (grdUsuarios.Rows.Count > 0)
        {
            Response.Redirect("evaluacion-envios.aspx?id=" + Cifrado.cifrarParaUrl(this.idevaluacion.ToString()));
        }
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        blResultados.Items.Clear();
        panelResultados.Visible = false;
        Button cmdGuardar = (Button)sender;

        //Si el estado de la dimensión 2 el botón tiene el parametro en cerrar
        //Para cerrar manualmente la dimensión
        if (cmdGuardar.CommandArgument == "Cerrar")
        {
            if (this.idevaluacion != -1)
            {
                //Creo el objeto dimensión
                Diagnosticos diagnostico = new Diagnosticos(idevaluacion);
                //Y llamo al método para poner el estado a 3 (Cerrado)
                if (diagnostico.ActualizarEstado(3))
                {
                    //Actualizo porque is no no se guardan los cambios hasta que refresques manualmente
                    Response.Redirect(Request.RawUrl.ToString());
                }
                else
                {
                    //En caso de que haya algún error en la actualización de los datos muestro un mensaje
                    ventanita.RadAlert("Ha ocurrido un error durante el cierre de la encuesta", null, null, "eFilecoach", "", Interfaz.ICO_alert);
                    //panelResultados.Visible = true;
                }
            }
        }
        else
        {
            if (cmdGuardar.CommandArgument == "Abrir")
            {
                if (this.idevaluacion != -1)
                {
                    //Creo el objeto dimensión
                    Diagnosticos diagnostico = new Diagnosticos(idevaluacion);
                    //Y llamo al método para poner el estado a 3 (Cerrado)
                    if (diagnostico.ActualizarEstado(2))
                    {
                        //Actualizo porque is no no se guardan los cambios hasta que refresques manualmente
                        Response.Redirect(Request.RawUrl.ToString());
                    }
                    else
                    {
                        //En caso de que haya algún error en la actualización de los datos muestro un mensaje
                        ventanita.RadAlert("Ha ocurrido un error durante la reapertura de la encuesta", null, null, "eFilecoach", "", Interfaz.ICO_alert);
                        //panelResultados.Visible = true;
                    }
                }
            }

            if (this.ValidarCampos())
            {
                Nullable<Int64> idcliente;
                if (cmbClientes.SelectedIndex == 0)
                {
                    idcliente = null;
                }
                else
                {
                    idcliente = Int64.Parse(cmbClientes.SelectedValue);
                }

                if (this.idevaluacion == -1)
                {
                    //Alta
                    this.evaluacion = new Diagnosticos();
                    idevaluacion = this.evaluacion.Insertar(this.idusuario, idcliente,
                        txtNombre.Text.Trim(),
                    Convert.ToDateTime(txtFechaAlta.Text), 1, String.Empty,Convert.ToInt64(cmbTipoEncuesta.SelectedValue));

                    //Si se ha seleccionado algún cliente
                    if (cmbClientes.SelectedIndex != 0)
                    {
                        //Creo el coachee (Cliente)
                        UsuariosFinales coachee = new UsuariosFinales(Convert.ToInt64(cmbClientes.SelectedValue));
                        //Creo el objeto persona
                        Personas persona = new Personas();
                        //Llamo al método para insertar al coachee en la tabla personas
                        //Y que así aparezca para realizar las autoevaluaciones consultando en la tabla diagnosticos_personas.
                        persona.Insertar(idevaluacion, Convert.ToInt64(cmbClientes.SelectedValue), 
                            coachee.GetNombre(), coachee.GetApellidos(), coachee.GetEmail(),
                            null);
                    }
                    if (idevaluacion != -1)
                    {
                        Response.Redirect("evaluacion.aspx?id=" +
                            Cifrado.cifrarParaUrl(idevaluacion.ToString()));
                    }
                }
                else
                {
                    //Actualización
                    if (evaluacion.Actualizar(txtNombre.Text, DateTime.Parse(txtFechaAlta.Text),
                        evaluacion.GetIDEstado()))
                    {
                        panelResultadosGuardar.Visible = true;
                        //Response.Redirect("evaluacion.aspx?id=" + idevaluacion.ToString());
                    }
                }

            }
        }

    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("/evaluaciones");
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

        if (!this.evaluacion.Eliminar())
        {
            panelResultados.Visible = true;
            blResultados.Visible = true;
            blResultados.Items.Clear();
            blResultados.Items.Add("");
            ventanita.RadAlert("Se ha producido un error al eliminar la encuesta", null, null, "eFilecoach", "", Interfaz.ICO_alert);
        }
        else
        {
            Response.Redirect("/evaluaciones");
        }
    }
    /// <summary>
    /// Rellenar datagridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>



    /// <summary>
    /// Método para limpiar el formulario, por ejemplo cuando se vaya a añadir nueva plantilla.
    /// </summary>
    private void limpiarFormulario()
    {
        txtNombre.Text = String.Empty;
        txtFechaAlta.Text = String.Empty;
        cmbPlantillas.SelectedIndex = 0;
        cmbClientes.SelectedIndex = 0;
    }
    /// <summary>
    /// Validamos campos
    /// </summary>

    private Boolean ValidarCampos()
    {
        blResultados.Items.Clear();
        if (txtNombre.Text.Equals(String.Empty))
        {
            blResultados.Items.Add("La campo nombre está vacío");
        }
        if (cmbTipoEncuesta.SelectedIndex==0)
        {
            blResultados.Items.Add("Debe seleccionar el tipo de encuesta");
        }
        if (txtFechaAlta.Text.Equals(String.Empty))
        {
            blResultados.Items.Add("La campo fecha de alta está vacío");
        }
        if (cmbPlantillas.SelectedValue.ToString().Equals("0"))
        {
            blResultados.Items.Add("Debe seleccionar una plantilla");
        }
        if (!Controlador.Validador.EsFecha(txtFechaAlta.Text))
        {
            panelResultados.Visible = true;
            blResultados.Items.Add("La fecha de la encuesta no tiene el formato de fecha correcto");

        }
        if (blResultados.Items.Count > 0)
        {
            panelResultados.Visible = true;
            blResultados.Visible = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmdImportarPlantilla_Click(object sender, EventArgs e)
    {
        if (cmbPlantillas.SelectedIndex != 0)
        {
            Int64 idplantilla = Convert.ToInt64(cmbPlantillas.SelectedValue);
            DiagnosticosPlantillas plantilla = new DiagnosticosPlantillas(idplantilla);
            DiagnosticosPlantillasDimensiones[] dimensiones_plantilla = plantilla.GetDimensiones();
            Boolean conerrores = false;
            for (int i = 0; i < dimensiones_plantilla.Length; i++)
            {
                Controlador.Diagnosticos.Dimensiones dimension_evaluacion = new Dimensiones();
                Int64 last_id_dimension = dimension_evaluacion.Insertar(this.evaluacion.GetID(),
                    dimensiones_plantilla[i].GetTitulo(),
                    plantilla.GetOrdenSiguiente());
                if (last_id_dimension != -1)
                {
                    DiagnosticosPlantillasDimensionesPreguntas[] preguntas_plantilla = dimensiones_plantilla[i].GetPreguntas();
                    for (int j = 0; j < preguntas_plantilla.Length; j++)
                    {
                        Controlador.Diagnosticos.Preguntas pregunta_evaluacion = new Preguntas();
                        Int64 last_ultima_pregunta = pregunta_evaluacion.Insertar(last_id_dimension, 
                            preguntas_plantilla[j].GetPregunta(),
                            dimensiones_plantilla[i].GetOrdenSiguientePregunta(),
                            preguntas_plantilla[j].GetIDTipo(),
                            preguntas_plantilla[j].GetNumRespuestas());
                        if (last_ultima_pregunta == -1)
                        {
                            conerrores = true;
                        }
                    }
                }
                else
                {
                    conerrores = true;
                }
            }
            if (!conerrores)
            {
                cmbPlantillas.SelectedIndex = 0;
                grdDimensiones.DataBind();
            }
            else
            {

            }
            pvDimensiones.Enabled = true;
        }
        this.ActivarDesactivarEnviar();
    }

    private Boolean ValidarPersona(String nombre, String email)
    {
        Personas persona = new Personas();
        if (nombre.Trim() == String.Empty)
        {
            ventanita.RadAlert("Debe introducir el nombre", null, null, "efielCoach", "", Interfaz.ICO_alert);
            return false;
        }
        else
        {
            if (!Validador.EsEmail(email.Trim()))
            {
                ventanita.RadAlert("Debe introducir el email correctamente", null, null, "efielCoach", "", Interfaz.ICO_alert);
                    return false;
            }
            else
            {
                if (persona.ExisteEmail(email.Trim().ToLower(),this.idevaluacion))
                {
                    ventanita.RadAlert("El correo electrónico está en uso por otra persona", null, null, "efielCoach", "", Interfaz.ICO_alert);
                    return false;
                }
                else
                {
                    if (cmbGrupo.SelectedIndex == 0)
                    {
                        ventanita.RadAlert("Debe seleccionar el grupo", null, null, "efielCoach", "", Interfaz.ICO_alert);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }

    protected void cmdGuardarPersona_Click(object sender, EventArgs e)
    {
        if (this.ValidarPersona(txtNombrePersona.Text, txtEmail.Text))
        {
            Personas persona = new Personas();
            if (persona.Insertar(this.idevaluacion, null, txtNombrePersona.Text.Trim(),
                txtApellidos.Text.Trim(), txtEmail.Text.Trim(),Int64.Parse(cmbGrupo.SelectedValue)) != -1)
            {
                txtNombrePersona.Text = String.Empty;
                txtApellidos.Text = String.Empty;
                txtEmail.Text = String.Empty;
                cmbGrupo.SelectedIndex = 0;
                grdUsuarios.DataBind();
                txtNombrePersona.Focus();
            }
            else
            {
                panelValidacionPersona.Visible = true;
                blValidacionPersona.Items.Add("Se ha producido un error al dar de alta la persona");
            }
        }
        tabsCabeceraOpciones.Tabs[1].Visible = true;
        pvEvaluadores.Selected = true;
        panelResultadosGuardar.Visible = false;

    }

    protected void grdUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 idpersona;
        Personas persona;
        GridViewRow fila = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        switch (e.CommandName)
        {
            case "eliminar":
                idpersona = Int64.Parse(e.CommandArgument.ToString());
                persona = new Personas(idpersona);
                persona.Eliminar();
                grdUsuarios.DataBind();
                break;
            case "Editar":
                grdUsuarios.EditIndex = fila.RowIndex;
                break;
            case "actualizar":
                //fila = grdUsuarios.Rows[grdUsuarios.EditIndex];
                idpersona = Int64.Parse(((HiddenField)fila.FindControl("hfID")).Value);
                String nombre = ((TextBox)fila.FindControl("txtNombreGrid")).Text;
                String apellidos = ((TextBox)fila.FindControl("txtApellidosGrid")).Text;
                DropDownList cmbGrupo = (DropDownList)fila.FindControl("cmbGrupo");
                Int64 idgrupo = Int64.Parse(((DropDownList)fila.FindControl("cmbGrupo")).SelectedValue);
                String email = ((TextBox)fila.FindControl("txtEmailGrid")).Text;
                persona = new Personas(idpersona);
                if (persona.ExisteEmailEdicion(email.Trim().ToLower(), this.idevaluacion))
                {
                    ventanita.RadAlert("El correo electrónico está en uso por otra persona", null, null, "efielCoach", "", Interfaz.ICO_alert);
                }
                else
                {
                    if (persona.Actualizar(nombre, apellidos, email, idgrupo))
                    {
                        grdUsuarios.EditIndex = -1;
                        grdUsuarios.DataBind();
                    }
                    else
                    {
                        panelValidacionPersona.Visible = true;
                        blValidacionPersona.Items.Add("Se ha producido un error al guardar los datos");
                    }
                }
                break;
        }
        tabsCabeceraOpciones.Tabs[1].Visible = true;
        pvEvaluadores.Selected = true;
        panelResultadosGuardar.Visible = false;

    }

    protected void grdDimensiones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Dimensiones dimension;
        switch (e.CommandName)
        {
            case "eliminar":
                Int64 iddimension = Int64.Parse(e.CommandArgument.ToString());
                dimension = new Dimensiones(iddimension);
                if (dimension.Eliminar())
                {
                    grdDimensiones.DataBind();
                }
                break;
            case "Subir":
                //Obtenemos el orden actual
                LinkButton cmdSubir = (LinkButton)e.CommandSource;
                GridViewRow fila = (GridViewRow)cmdSubir.NamingContainer;
                GridViewRow fila_anterior = grdDimensiones.Rows[fila.RowIndex - 1];

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
                dimension = new Dimensiones(idfila);

                Dimensiones dimension_anterior =
                    new Dimensiones(idfila_anterior);

                dimension.Actualizar(dimension.GetTitulo(), orden_anterior);
                dimension_anterior.Actualizar(dimension_anterior.GetTitulo(), orden);
                grdDimensiones.DataBind();

                break;
            case "Bajar":
                LinkButton cmdSubir2 = (LinkButton)e.CommandSource;
                GridViewRow fila2 = (GridViewRow)cmdSubir2.NamingContainer;
                GridViewRow fila_siguiente = grdDimensiones.Rows[fila2.RowIndex + 1];

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
                Dimensiones dimension2 =
                    new Dimensiones(idfila2);

                Dimensiones dimension_siguiente =
                    new Dimensiones(idfila_siguiente);

                dimension2.Actualizar(dimension2.GetTitulo(), orden_siguiente);
                dimension_siguiente.Actualizar(dimension_siguiente.GetTitulo(), orden2);
                grdDimensiones.DataBind();
                break;
        }
        pvDimensiones.Selected = true;
        panelResultadosGuardar.Visible = false;
    }

    protected void cmdGuardarDimension_Click(object sender, EventArgs e)
    {
        panelValidacionDimension.Visible = false;
        blValidacionDimension.Items.Clear();
        Dimensiones dimension = new Dimensiones();
        if (dimension.Insertar(this.idevaluacion, txtNuevaDimension.Text,
            this.evaluacion.GetOrdenSiguiente()) != -1)
        {
            txtNuevaDimension.Text = String.Empty;
            grdDimensiones.DataBind();
            txtNuevaDimension.Focus();
        }
        else
        {
            panelValidacionDimension.Visible = true;
            blValidacionDimension.Items.Add("Se ha producido un error al dar de alta la categoría");
        }
        pvDimensiones.Selected = true;
        panelResultadosGuardar.Visible = false;
    }

    protected void grdDimensiones_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grdDimensiones_DataBound(object sender, EventArgs e)
    {
        //Vamos a organizar los botones del orden
        if (grdDimensiones.Rows.Count > 0)
        {
            //Desactivamos el botón de la primera fila
            LinkButton cmdSubir = (LinkButton)grdDimensiones.Rows[0].FindControl("cmdSubir");
            cmdSubir.Visible = false;

            int contador = grdDimensiones.Rows.Count;
            LinkButton cmdBajar =
                (LinkButton)grdDimensiones.Rows[contador - 1].FindControl("cmdBajar");
            cmdBajar.Visible = false;

            literalTituloDimensiones.Text = "<h4>Listado de categorías</h4>";
        }
        else
        {
            literalTituloDimensiones.Text = "<h4>Listado de categorías vacío</h4>";
        }
    }

    private Boolean ValidarCamposImportacion()
    {
        String[] valores = ViewState["valores_asignados"].ToString().Split(';');
        Boolean fallo = false;

        for (int i = 0; i < valores.Length - 1 && fallo == false; i++)
        {
            for (int j = 0; j < valores.Length && fallo == false; j++)
            {
                if (valores[i] == valores[j] && valores[i].ToLower() != "sin asignar" && i != j)
                {
                    fallo = true;
                }
            }
        }

        return !fallo;
    }

    protected void cmdInsertarEvaluadoresArchivo_Click(object sender, EventArgs e)
    {
        LeerFichero();
    }

    private void TerminarEIntroducir()
    {
        blFicherosNotificaciones.Items.Clear();
        pnlNotificacionesFicheros.Visible = false;
        if (ViewState["introducir"] != null)
        {
            if (ValidarCamposImportacion() == true)
            {
                String fichero = ViewState["fichero"].ToString();

                String[] lineas = JsonConvert.DeserializeObject(ViewState["fichero"].ToString()).ToString().Split('\n');

                String[] valores = ViewState["valores_asignados"].ToString().Split(';');
                int email = 0;
                int nombre = 0;
                int apellidos = 0;

                for (int j = 0; j < valores.Length; j++)
                {
                    if (valores[j].ToLower() == "nombre")
                    {
                        nombre = j;
                    }

                    if (valores[j].ToLower() == "apellidos")
                    {
                        apellidos = j;
                    }

                    if (valores[j].ToLower() == "correo electrónico")
                    {
                        email = j;
                    }
                }
                Boolean errores = false;
                for (int i = 0; i < lineas.Length; i++)
                {
                    Personas p = new Personas();
                    String[] linea = lineas[i].Split(';');
                    if (linea[0] != String.Empty)
                    {
                        Int64 last_id = p.Insertar(this.idevaluacion, null, linea[nombre], linea[apellidos], linea[email],
                            Int64.Parse(cmbGrupo.SelectedValue));
                        if (last_id == -1)
                        {
                            errores = true;
                        }
                    }


                }

                if (!errores)
                {
                    blFicherosNotificaciones.Items.Add("Evaluadores Introducidos");
                    pnlNotificacionesFicheros.CssClass = "alert alert-success";
                    literalAyudaFichero.Text = "";
                    ViewState.Remove("introducir");
                    ViewState.Remove("valores_asignados");
                    ViewState.Remove("fichero");
                    ViewState.Remove("columnas");
                    phControlesDinamicos.Controls.Clear();
                    //phMostrarValores.Controls.Clear();
                }
                else
                {
                    blFicherosNotificaciones.Items.Add("Algo ha salido mal en la inserción de evaluadores");
                    pnlNotificacionesFicheros.CssClass = "alert alert-danger";
                }

            }
            else
            {
                blFicherosNotificaciones.Items.Add("No puede haber dos columnas asignadas a un campo. \n Cargue el fichero de nuevo y vuelva a intentarlo");
                pnlNotificacionesFicheros.CssClass = "alert alert-danger";
                literalAyudaFichero.Visible = false;
            }
            pnlNotificacionesFicheros.Visible = true;

        }
        grdUsuarios.DataBind();

    }

    private void LeerFichero()
    {
        if (cmbGrupo.SelectedIndex==0)
        {
            ventanita.RadAlert("No ha seleccionado el grupo del que serán miembros los usuarios", null, null, "efileCoach", "", Interfaz.ICO_alert);
        }
        else
        {
            if (Archivo.UploadedFiles.Count > 0)
            {
                int contador = 0;
                Boolean errores_columnas = false;
                Boolean errores_email = false;
                Personas persona = new Personas();
                String Extension = Path.GetExtension(Archivo.UploadedFiles[0].FileName);
                StreamReader fichero = new StreamReader(Archivo.UploadedFiles[0].InputStream);
                String linea;
                while ((linea = fichero.ReadLine()) != null)
                {
                    String[] columnas = linea.Replace(',', ';').Trim().Split(';');
                    if (columnas.Length==3)
                    {
                        if (Interfaz.EsEmail(columnas[2].Trim().ToLower()))
                        {
                            if (!persona.ExisteEmail(columnas[2].Trim().ToLower(), this.idevaluacion))
                            {
                                if (persona.Insertar(this.idevaluacion, null,
                                    columnas[0], columnas[1], columnas[2], Int64.Parse(cmbGrupo.SelectedValue)) != -1)
                                {
                                    contador++;
                                }
                            }
                        }
                        else
                        {
                            errores_email = true;
                        }
                        
                    }
                    else
                    {
                        errores_columnas = true;
                    }
                }
                fichero.Close();
                grdUsuarios.DataBind();
                cmbGrupo.SelectedIndex = 0;
                if (errores_email)
                {
                    ventanita.RadAlert("Alguna dirección de email no tenía el formato correcto",
                        null, null, "efileCoach", "", Interfaz.ICO_alert);
                }
                if (errores_columnas)
                {
                    ventanita.RadAlert("Alguna línea del fichero no contiene las tres columnas requeridas",
                        null, null, "efileCoach", "", Interfaz.ICO_alert);

                }
                ventanita.RadAlert(contador.ToString() + " contactos importados con éxito", null, null, "efileCoach", "", Interfaz.ICO_ok);
            }
            else
            {
                ventanita.RadAlert("No ha seleccionado ningún fichero", null, null, "efileCoach", "", Interfaz.ICO_alert);
            }
        }
    }

    private void MostrarAsignaciones()
    {
        //phMostrarValores.Controls.Clear();
        if (ViewState["valores_asignados"] != null)
        {
            String[] valores = ViewState["valores_asignados"].ToString().Split(';');
            int j = 0;
            for (int i = 0; i < phControlesDinamicos.Controls.Count; i++)
            {
                DropDownList cmbColumna =
                    (DropDownList)phControlesDinamicos.FindControl("cmbColumna" + i.ToString());
                if (cmbColumna != null)
                {
                    cmbColumna.Enabled = false;
                    Label lblColumna =
                        (Label)phControlesDinamicos.FindControl("lblColumna" + i.ToString());
                    cmbColumna.SelectedValue = valores[j];
                    j++;
                    if (cmbColumna != null && lblColumna != null)
                    {
                        Literal literalabrimosCapa = new Literal();
                        literalabrimosCapa.Text = "<div class='col-md-3'>";
                        Literal literalCerramosCapa = new Literal();
                        literalCerramosCapa.Text = "</div>";
                    }
                }

            }
            ViewState.Add("introducir", "si");
            pnlAsignaciones.Visible = false;
        }
    }

    protected void grdUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        HiddenField hfEstadoEvaluador = (HiddenField)e.Row.FindControl("hfEstadoEvaluador");
        Literal mensajeEstado = (Literal)e.Row.FindControl("ltEstadoEvaluador");
        if (hfEstadoEvaluador != null && mensajeEstado != null)
        {
            switch (Convert.ToInt64(hfEstadoEvaluador.Value))
            {
                case 0:
                    mensajeEstado.Text = "<h3><span class='label label-default'>Pendiente envío<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span></span></h3>";
                    break;

                case 1:
                    mensajeEstado.Text = "<h3><span class='label label-info'>En espera <span class='glyphicon glyphicon-envelope' aria-hidden='true'></span></span></h3>";
                    break;

                case 2:
                    mensajeEstado.Text = "<h3><span class='label label-success'>Completada <span class='glyphicon glyphicon-ok' aria-hidden='true'></span></span></h3>";
                    break;
            }
        }
    }

    protected void cmdMostrarAsignaciones_Click(object sender, EventArgs e)
    {
        String valores = String.Empty;
        Boolean nombre = false;
        Boolean apellidos = false;
        Boolean email = false;


        for (int i = 0; i < phControlesDinamicos.Controls.Count; i++)
        {
            DropDownList cmb = phControlesDinamicos.FindControl("cmbColumna" + i.ToString()) as DropDownList;
            if (cmb != null)
            {
                if (cmb is DropDownList)
                {
                    valores += cmb.SelectedValue + ";";
                    if (cmb.SelectedValue.ToLower() == "nombre")
                    {
                        nombre = true;
                    }
                    if (cmb.SelectedValue.ToLower() == "apellidos")
                    {
                        apellidos = true;
                    }
                    if (cmb.SelectedValue.ToLower() == "correo electrónico")
                    {
                        email = true;
                    }
                }
            }
        }
        if (nombre && apellidos && email)
        {
            ViewState.Add("valores_asignados", valores);
            this.MostrarAsignaciones();
            this.TerminarEIntroducir();
        }
        else
        {
            blFicherosNotificaciones.Items.Add("Los campos requeridos para importar el fichero son Nombre, Apellidos y Correo Electrónico");
            pnlNotificacionesFicheros.Visible = true;
            pnlNotificacionesFicheros.CssClass = "alert alert-danger";
        }


    }

    protected void cmdInsertarCampos_Click(object sender, EventArgs e)
    {

    }

    protected void grdUsuarios_DataBound(object sender, EventArgs e)
    {
        if (grdUsuarios.Rows.Count==0)
        {
            literalTituloEvaluadores.Text = "<h4>Listado de evaluadores vacío</h4>";
        }
        else
        {
            literalTituloEvaluadores.Text = "<h4>Listado de evaluadores</h4>";
        }
        panelListadoEvaluadores.Visible = !(grdGrupos.Rows.Count == 0);
        panelNuevaPersona.Visible = !(grdGrupos.Rows.Count == 0);
    }

    protected void cmdNuevoGrupo_Click(object sender, EventArgs e)
    {
        if (txtNuevoGrupo.Text.Trim()==String.Empty)
        {
            ventanita.RadAlert("Debe introducir el nombre del grupo",null, null, "efileCoach", "", Interfaz.ICO_alert);
        }
        else
        {
            if (txtPonderacion.Value == null && this.ActualizarVisibilidadClientes())
            {
                ventanita.RadAlert("El valor de la ponderación debe tener un valor comprendido entre 0 y 100", null, null,
                    "efileCoach", "", Interfaz.ICO_alert);
                txtPonderacion.Focus();
            }
            else
            {
                Decimal ponderacion;
                if (this.ActualizarVisibilidadClientes())
                {
                    ponderacion = (Decimal)txtPonderacion.Value;
                }
                else
                {
                    ponderacion = 0;
                }

                if ((ponderacion >100 || ponderacion <0) && this.ActualizarVisibilidadClientes())
                {
                    ventanita.RadAlert("El valor de la ponderación debe tener un valor comprendido entre 0 y 100",
                        null, null, "efileCoach", "", Interfaz.ICO_alert);
                    txtPonderacion.Focus();
                }
                else
                {
                    Grupos grupo = new Grupos();
                    if (grupo.Nuevo(this.idevaluacion, txtNuevoGrupo.Text, "",ponderacion))
                    {
                        grdGrupos.DataBind();
                        grdUsuarios.DataBind();
                        txtNuevoGrupo.Text = String.Empty;
                        txtPonderacion.Value = null;
                        cmbGrupo.DataBind();
                        txtNuevoGrupo.Focus();
                    }
                    else
                    {
                        ventanita.RadAlert("Se ha producido un error al dar de alta el grupo",
                            null, null, "efileCoach", "", Interfaz.ICO_alert);
                    }
                }
            }
        }
    }

    protected void grdGrupos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 idgrupo;
        Grupos grupo;
        GridViewRow fila = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        switch (e.CommandName)
        {
            case "Guardar":
                idgrupo = Convert.ToInt64(e.CommandArgument.ToString());
                grupo = new Grupos(idgrupo);
                TextBox txtNuevoNombre = (TextBox)fila.FindControl("txtNombre");
                RadNumericTextBox txtPonderacion = (RadNumericTextBox)fila.FindControl("txtPonderacion");
                if (txtPonderacion.Value==null && ActualizarVisibilidadClientes())
                {
                    ventanita.RadAlert("El valor de la ponderación debe tener un valor comprendido entre 0 y 100", null, null, "efileCoach", "", Interfaz.ICO_alert);
                }
                else
                {
                    Decimal ponderacion = (Decimal)txtPonderacion.Value;
                    if ((ponderacion > 100 || ponderacion < 0) && this.ActualizarVisibilidadClientes())
                    {
                        ventanita.RadAlert("El valor de la ponderación debe estar comprendido entre 0 y 100", null, null, "efileCoach", "", Interfaz.ICO_alert);
                    }
                    else
                    {
                        if (grupo.Actualizar(txtNuevoNombre.Text.Trim(), ponderacion))
                        {
                            grdGrupos.EditIndex = -1;
                            grdGrupos.DataBind();
                            cmbGrupo.DataBind();
                        }
                    }
                }
                break;
            case "Eliminar":
                idgrupo = Convert.ToInt64(e.CommandArgument.ToString());
                grupo = new Grupos(idgrupo);
                if (grupo.Eliminar())
                {
                    literalContadorPonderacion.Text = String.Empty;
                    grdGrupos.DataBind();
                    grdUsuarios.DataBind();
                }
                break;
        }
    }

    protected void cmbGrupo_DataBound(object sender, EventArgs e)
    {
        cmbGrupo.Items.Insert(0, "Seleccionar grupo...");
    }

    protected void grdGrupos_DataBound(object sender, EventArgs e)
    {
        Decimal ponderacion = evaluacion.GetSumaPonderacionGrupos();
        if (grdGrupos.Rows.Count>0)
        {
            if (this.ActualizarVisibilidadClientes())
            {
                if (ponderacion == 100)
                {
                    literalContadorPonderacion.Text = "<div class='alert alert-info'><p>La suma de todas las ponderaciones es del 100 %</p></div>";
                }
                else
                {
                    literalContadorPonderacion.Text = "<div class='alert alert-warning'><p>La suma de todas las ponderaciones es " + ponderacion.ToString("0.00")
                        + " % y su valor debe ser 100 %</p></div>";
                }
            }
        }
    }

    private Boolean ActualizarVisibilidadClientes()
    {
        if (cmbTipoEncuesta.SelectedValue == "2")
        {
            lblClientes.Visible = false;
            cmbClientes.Visible = false;
            grdGrupos.Columns[2].Visible = false;
            grdGrupos.Columns[2].Visible = false;
            lblPonderacion.Visible = false;
            lblPonderacion.Visible = false;
            txtPonderacion.Visible = false;
            return false;
        }
        else
        {
            lblClientes.Visible = true;
            cmbClientes.Visible = true;
            grdGrupos.Columns[2].Visible = true;
            grdGrupos.Columns[2].Visible = true;
            return true;
        }
    }

    protected void cmbTipoEncuesta_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ActualizarVisibilidadClientes();
    }
}