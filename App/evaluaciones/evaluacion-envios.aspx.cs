using Controlador;
using Controlador.Diagnosticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class evaluaciones_evaluacion_envios : System.Web.UI.Page
{
    Int64 idevaluacion = -1;
    Int64 idusuario;
    Diagnosticos evaluacion;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["id"] == null)
        {
            Response.Redirect("/default.aspx");
        }
        else
        {
            if (UsuarioLogueado.EsCuenta())
            {
                this.idusuario = UsuarioLogueado.GetCuenta().GetIDUsuarioPrincipal();
            }
            else
            {
                this.idusuario = UsuarioLogueado.GetUsuario().GetID();
            }
            this.idevaluacion = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"],true));
            hfIDDiagnostico.Value = this.idevaluacion.ToString();
            this.evaluacion = new Diagnosticos(this.idevaluacion);
            if (!IsPostBack)
            {

                this.lNombreEvaluacion.Text = evaluacion.GetNombre();
                //Creo el diagnostico y saco el id del cliente
                Diagnosticos diagnostico = new Diagnosticos(idevaluacion);
                //Si tiene id cliente definido
                if (diagnostico.GetIDCliente() != -1)
                {
                    UsuariosFinales coachee = new UsuariosFinales(diagnostico.GetIDCliente());

                    //Pongo visible el panel
                    pAutoevaluacion.Visible = true;

                    //asigno los valores alos hidden fields
                    hfEmailAutoeval.Value = coachee.GetEmail().ToString();
                    hfIDAutoeval.Value = coachee.GetID().ToString();
                    chkNombreAutoeval.Text = coachee.GetNombre().ToString() + " " + coachee.GetApellidos().ToString() 
                        + " - " + coachee.GetEmail().ToString() + " (Autoevaluación)";

                    //Creo a la persona (que es el coachee a través del id evaluación y el id del coachee)
                    Personas persona = new Personas(this.idevaluacion, Convert.ToInt64(hfIDAutoeval.Value));

                    //En caso de que la evaluación sea antigua y no exista en la tabla personas el coachee
                    //if (!persona.GetTieneDatos())
                    //{
                    //    //Lo inserto
                    //    persona.Insertar(idevaluacion, diagnostico.GetIDCliente(),
                    //        coachee.GetNombre(), coachee.GetApellidos(), coachee.GetEmail());
                    //}

                }

                RecargarDatosVisuales(diagnostico);
            }
        }

        if (!IsPostBack)
        {
            hlVolver.NavigateUrl = String.Format(String.Format("evaluacion.aspx?id={0}",
                Cifrado.cifrarParaUrl(idevaluacion.ToString())));
            if (grdHistorial.Rows.Count > 0)
            {
                panelHistorial.Visible = true;
            }
            else
            {
                panelHistorial.Visible = false;
            }
        }

    }
    /// <summary>
    /// Método de tipo cadena que recibe como parametro la cadena que almacena errores para mostrar errores en caso de que
    /// exista algún error en el envio de email
    /// </summary>
    /// <param name="errores">Cadena que almacena los errores</param>
    /// <returns>Devuelve la cadena de errores que recibe + si ha habido algún error </returns>
    private String enviaremailAutoeval(String errores)
    {
        if (chkNombreAutoeval.Checked)
        {
            //Es el id de la tabla diagnosticos_personas
            String email = hfEmailAutoeval.Value.Trim().ToLower();
            //Instancio la persona conociendo el id de la evaluacion y el IdCoachee
            Personas persona = new Personas(idevaluacion, Int64.Parse(hfIDAutoeval.Value));
            if (Validador.EsEmail(email))
            {
                //Hacemos el envío
                String contenido = Interfaz.GetValor("evaluacion_email_contenido");
                contenido = contenido.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
                contenido = contenido.Replace("[url]",
                    String.Format("https://my.efilecoach.com/rellena-evaluacion.aspx?k={0}",
                    Cifrado.cifrarParaUrl(persona.GetIdPersona().ToString())));
                contenido = contenido.Replace("[contenido]", txtEmail.Text.Replace("\n", "<br />"));
                String firma;
                Cuentas cuenta;
                if (UsuarioLogueado.EsCuenta())
                {
                    firma = UsuarioLogueado.GetCuenta().GetSMTPFirma();
                    cuenta = UsuarioLogueado.GetCuenta();
                }
                else
                {
                    cuenta = new Cuentas(UsuarioLogueado.GetUsuario().GetIDCuenta());
                    firma = cuenta.GetSMTPFirma();
                }
                contenido = contenido.Replace("[firma]", firma);
                if (!Email.sendEmailCuenta("efileCoach <noreply@efilecoach.com>",
                    email, "Autoevaluacion: " + evaluacion.GetNombre(), contenido,
                    Request.UserHostAddress,cuenta))
                {
                    errores += "; " + email;
                }
                else
                {
                    persona.ActualizarObservaciones("", 1);
                    persona.InsertarEmail("noreply@efilecoach.com", email,
                        "Autoevaluacion: " + evaluacion.GetNombre(), contenido, DateTime.Now,
                        Request.UserHostAddress);

                }

            }
        }
        return errores;
    }

    protected void cmdEnviar_Click(object sender, EventArgs e)
    {
        String errores = String.Empty;
        int contador = 0;
        for (int i = 0; i < listadoEvaluadores.Items.Count; i++)
        {
            CheckBox chkNombre = (CheckBox)listadoEvaluadores.Items[i].FindControl("chkNombre");
            if (chkNombre.Checked)
            {
                HiddenField hfEmail = (HiddenField)listadoEvaluadores.Items[i].FindControl("hfEmail");
                HiddenField hfIDEvaluador = (HiddenField)listadoEvaluadores.Items[i].FindControl("hfID");
                //Es el id de la tabla diagnosticos_personas
                Int64 idevaluador = Int64.Parse(hfIDEvaluador.Value);
                String email = hfEmail.Value.Trim().ToLower();
                if (Validador.EsEmail(email))
                {
                    //Hacemos el envío
                    String contenido = Interfaz.GetValor("evaluacion_email_contenido");
                    contenido = contenido.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
                    contenido = contenido.Replace("[url]",
                        String.Format("https://my.efilecoach.com/rellena-evaluacion.aspx?k={0}",
                        Cifrado.cifrarParaUrl(idevaluador.ToString())));
                    contenido = contenido.Replace("[contenido]",txtEmail.Text.Replace("\n", "<br />"));
                    String firma;
                    Cuentas cuenta;
                    if (UsuarioLogueado.EsCuenta())
                    {
                        firma = UsuarioLogueado.GetCuenta().GetSMTPFirma();
                        cuenta = UsuarioLogueado.GetCuenta();
                    }
                    else
                    {
                        cuenta = new Cuentas(UsuarioLogueado.GetUsuario().GetIDCuenta());
                        firma = cuenta.GetSMTPFirma();
                    }
                    contenido = contenido.Replace("[firma]", firma);
                    if (!Email.sendEmailCuenta("efileCoach <noreply@efilecoach.com>",
                        email, "Evaluacion: " + evaluacion.GetNombre(), contenido,
                        Request.UserHostAddress,cuenta))
                    {
                        errores += "; " + email;
                    }
                    else
                    {
                        contador++;
                        Personas persona = new Personas(idevaluador);
                        persona.ActualizarObservaciones("", 1);
                        persona.InsertarEmail("noreply@efilecoach.com", email,
                            "Evaluacion: " + evaluacion.GetNombre(), contenido, DateTime.Now,
                            Request.UserHostAddress);
                    }

                }
            }
        }
        //Si marca el checkbox de autoevaluacion
        if (chkNombreAutoeval.Checked)
        {
            errores = enviaremailAutoeval(errores);
            //Le sumo uno al contador
            contador++;
        }

        if (errores == String.Empty)
        {
            panelEnvios.Visible = true;
            lEnvios.Text = String.Format("<div class='alert alert-success'><p>Mensaje enviado con éxito a {0} destinos.</p></div>",
                contador);
        }
        else
        {

            panelEnvios.Visible = true;
            lEnvios.Text = String.Format("<div class='alert alert-danger'><p>Mensaje enviado con errores a las siguientes direcciones: {0}</p></div>",
                errores);
        }

        Diagnosticos diagnostico = new Diagnosticos(idevaluacion);

        RecargarDatosVisuales(diagnostico);

        listadoEvaluadores.DataBind();

        grdHistorial.DataBind();
        panelHistorial.Visible = true;
    }



    protected void cmdSeleccionarTodos_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < listadoEvaluadores.Items.Count; i++)
        {
            CheckBox chkNombre = (CheckBox)listadoEvaluadores.Items[i].FindControl("chkNombre");
            chkNombre.Checked = true;
        }
        if (pAutoevaluacion.Visible == true)
        {
            chkNombreAutoeval.Checked = true;
        }
    }

    protected void cmdDesSeleccionarTodos_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < listadoEvaluadores.Items.Count; i++)
        {
            CheckBox chkNombre = (CheckBox)listadoEvaluadores.Items[i].FindControl("chkNombre");
            chkNombre.Checked = false;
        }
        if (pAutoevaluacion.Visible == true)
        {
            chkNombreAutoeval.Checked = false;
        }
    }

    protected void listadoEvaluadores_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item != null)
        {
            Literal litEstado = (Literal)e.Item.FindControl("litEstado");
            if (litEstado != null)
            {
                HiddenField hfEstado = (HiddenField)e.Item.FindControl("hfEstado");
                CheckBox check = (CheckBox)e.Item.FindControl("chkNombre");
                switch (Convert.ToInt64(hfEstado.Value))
                {
                    case 0:
                        //Sin enviar
                        litEstado.Text = " <span class='label label-default'>No enviado <span class='glyphicon glyphicon-remove' aria-hidden='true'></span> </span>";
                        check.Checked = true;
                        check.Enabled = true;
                        break;
                    case 1:
                        //Enviado sin contestar
                        litEstado.Text = " <span class='label label-info'>En espera <span class='glyphicon glyphicon-envelope' aria-hidden='true'></span> </span>";
                        check.Enabled = true;
                        check.Checked = false;
                        break;
                    case 2:
                        //Contestado
                        litEstado.Text = " <span class='label label-success'>Terminada <span class='glyphicon glyphicon-ok' aria-hidden='true'></span> </span>";
                        check.Checked = false;
                        check.Enabled = false;
                        break;
                    default:
                        break;
                }
            }

        }
    }

    private void ActualizarProgreso(Int64? noenviada, Int64? espera, Int64? completa)
    {

        //Los totales es la suma de todos
        Int64? totales = noenviada + espera + completa;

        //Calculo los porcentajes para asignarselos a la barra de progreso
        Double? pnoenviada = (Double)noenviada / totales * 100;
        Double? pespera = (Double)espera / totales * 100;
        Double? pcompletas = (Double)completa / totales * 100;
        //Creo los divs en los literales (Porque no se podía modificar el estilo del div con código)
        litDanger.Text = "<div id = 'divdanger' runat='server' class='progress-bar progress-bar-danger progress-bar-striped active' role='progressbar' aria-valuenow='45' aria-valuemin='0' aria-valuemax='100' style='width: " + String.Format("{0:0.##}", pnoenviada).Replace(',', '.') + "%'></div>";
        litInfo.Text = "<div id = 'divprimary' runat='server' class='progress-bar progress-bar-info progress-bar-striped active' role='progressbar' aria-valuenow='45' aria-valuemin='0' aria-valuemax='100' style='width: " + String.Format("{0:0.##}", pespera).Replace(',', '.') + "%'></div>";
        litSuccess.Text = "<div id = 'divsuccess' runat = 'server' class='progress-bar progress-bar-success progress-bar-striped active' role='progressbar' aria-valuenow='45' aria-valuemin='0' aria-valuemax='100' style='width: " + String.Format("{0:0.##}", pcompletas).Replace(',', '.') + "%'></div>";

    }

    private void ActualizarEstados(Int64? noenviada, Int64? espera, Int64? completa)
    {
        //Si existe el id de autoevaluación modifico sus labels
        if (hfIDAutoeval.Value != String.Empty)
        {
            //Modifico el literal de la autoevaluacion que es individual
            Personas persona = new Personas(this.idevaluacion, Convert.ToInt64(hfIDAutoeval.Value));

            switch (persona.GetEstado())
            {
                case 0:
                    //Sin enviar
                    litEstadoAutoeval.Text = " <span class='label label-default'>No enviado <span class='glyphicon glyphicon-remove' aria-hidden='true'></span> </span>";
                    chkNombreAutoeval.Checked = true;
                    break;
                case 1:
                    //Enviado sin contestar
                    litEstadoAutoeval.Text = " <span class='label label-info'>En espera <span class='glyphicon glyphicon-envelope' aria-hidden='true'></span> </span>";
                    chkNombreAutoeval.Checked = false;
                    break;
                case 2:
                    litEstadoAutoeval.Text = " <span class='label label-success'>Terminada <span class='glyphicon glyphicon-ok' aria-hidden='true'></span> </span>";
                    chkNombreAutoeval.Checked = false;
                    chkNombreAutoeval.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        Diagnosticos diagnostico = new Diagnosticos(this.idevaluacion);

        //Compruebo el estado para actualizar los estados de los diagnosticos
        if (diagnostico.GetIDEstado() == 2 || diagnostico.GetIDEstado() == 3)
        {
            //Paneles que puse inicialmente para que se viera
            pInfo.Visible = true;
            if (diagnostico.GetIDEstado() == 2)
            {
                lblInformacion.Text = String.Format("La evaluacion está en estado de espera se ha mandado a {0} usuarios.<br />Han respondido {1} usuarios\n<br />Faltan {2} usuarios por contestar<br />Hay {3} usuarios a los que no se le ha enviado", diagnostico.GetContadorPersonas(), completa, espera, noenviada);

            }
            else if (diagnostico.GetIDEstado() == 3)
            {
                lblInicio.Visible = false;
                pInfo.CssClass = "alert alert-danger";
                lblInformacion.Text = String.Format("La evaluacion está cerrada se ha mandado a {0} usuarios.<br />Han respondido {1} usuarios\n<br />Faltan {2} usuarios por contestar<br />Hay {3} usuarios a los que no se le ha enviado", diagnostico.GetContadorPersonas(), completa, espera, noenviada);
            }


        }
        //Si el diagnostico esta cerrado
        if (diagnostico.GetIDEstado() == 3)
        {
            //Deshabilito la lista
            //listadoEvaluadores.Enabled = false;
            //Deshabilito el botón de enviar
            //cmdEnviar.Enabled = false;
            //Oculto los botones y el textfield que no son necesarios
            txtEmail.Visible = false;
            lblTexto.Visible = false;
            cmdDesSeleccionarTodos.Visible = false;
            cmdSeleccionarTodos.Visible = false;
            //Quito los check de la autoevaluación (Los normales los hago en el data bound)
            chkNombreAutoeval.Enabled = false;
            chkNombreAutoeval.Checked = false;
        }
    }

    private void RecargarDatosVisuales(Diagnosticos diagnostico)
    {


        //Obtengo los datos de los usuarios a los que se le ha enviado y cuales han respondido etc etc...
        Int64? noenviada = diagnostico.GetContadorSinEnviar();
        Int64? espera = diagnostico.GetContadorEspera();
        Int64? completa = diagnostico.GetContadorCerrados();

        //Llamo al método que me cambia los literales de los usuarios
        ActualizarEstados(noenviada, espera, completa);

        //llamo al método para que actualize la barra de progreso de arriba
        ActualizarProgreso(noenviada, espera, completa);
    }
}