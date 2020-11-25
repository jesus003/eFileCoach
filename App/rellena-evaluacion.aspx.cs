using Controlador;
using Controlador.Diagnosticos;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class rellena_evaluacion : System.Web.UI.Page
{
    private Personas GetPersona()
    {
        Personas persona = new Personas(Int64.Parse(hfIDPersona.Value));
        return persona;
    }

    public Diagnosticos GetDiagnostico()
    {
        Diagnosticos diagnostico = new Diagnosticos(this.GetPersona().GetIDDiagnostico());
        return diagnostico;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.QueryString["k"] != null)
            {
                String k = Request.QueryString["k"];
                String descifrado = Cifrado.descifrar(k,true);
               
                
                if (descifrado.Contains(":"))
                {
                    String[] descifrados = descifrado.Split(':');
                    if (descifrados.Length == 2)
                    {
                        if (Validador.EsInt64(descifrados[0]) && Validador.EsInt64(descifrados[1]))
                        {
                            Int64 idpersona = Convert.ToInt64(descifrados[0]);
                            Int64 contadorid = Convert.ToInt64(descifrados[1]);

                            this.hfIDPersona.Value = idpersona.ToString();
                            //Empezariamos a cargar las preguntas de la dimensión
                            //Creamos a la persona para obtener el Id del diagnóstico
                            Personas persona = new Personas(idpersona);
                            if (persona.GetEstado() == 2)
                            {
                                Response.Redirect("/alerta-evaluacion.aspx?alerta=" + Cifrado.cifrarParaUrl("Completo"));
                            }

                            Diagnosticos diagnostico = new Diagnosticos(persona.GetIDDiagnostico());
                            if (diagnostico.GetIDTipo()==1)
                            {
                                dlPreguntas.Visible = true;
                                dlPreguntasAbiertas.Visible = false;
                                //panelObserbaciones.Visible = true;
                            }
                            else
                            {
                                dlPreguntas.Visible = false;
                                dlPreguntasAbiertas.Visible = true;
                                //panelObserbaciones.Visible = false;
                            }
                            //Si el estado es 3 el formulario esta cerrado
                            if (diagnostico.GetIDEstado() == 1)
                            {
                                diagnostico.ActualizarEstado(2);
                            }
                            if (diagnostico.GetIDEstado() == 3)
                            {
                                Response.Redirect("/alerta-evaluacion.aspx?alerta=" + Cifrado.cifrarParaUrl("Cerrado"));
                            }
                            //Sacamos el id de la dimension a traves del diagnóstico
                            Int64 iddimension = diagnostico.GetIDDimension(Convert.ToInt32(contadorid));
                            //Asigno el valor de la dimensión a un hidden field
                            hfDimension.Value = iddimension.ToString();
                            Dimensiones dimension = new Dimensiones(iddimension);
                            literalTituloDimension.Text = "<h3>" + dimension.GetTitulo() + "</h3>";

                            Resultados resultados = new Resultados();
                            if (!resultados.Comprobar(idpersona, Convert.ToInt32(iddimension)))
                            {
                                Int64[] idspreguntas = dimension.DevolverPreguntas();
                                if (idspreguntas.Length > 0)
                                {
                                    for (int i = 0; i < idspreguntas.Length; i++)
                                    {
                                        resultados.Insertar(idpersona, idspreguntas[i], DateTime.Now,
                                            -1, String.Empty,Request.UserHostAddress);
                                    }
                                }
                                odsPreguntas.DataBind();
                            }

                            this.hfPaginacion.Value = contadorid.ToString();
                            if (contadorid == diagnostico.GetContadorDimensiones() - 1)
                            {
                                //panelObserbaciones.Visible = true;
                                //txtObservacionesFinales.Text = persona.GetObservaciones();
                                cmdSiguiente.Text = "Guardar y Salir";
                                cmdSiguiente.CommandArgument = "Salir";
                            }

                            if (contadorid == 0)
                            {
                                cmdAtras.Visible = false;
                            }

                        }


                    }
                    else
                    {
                        Response.Redirect("/default.aspx");
                    }
                }
                else
                {
                    //Podríamos estar en la primera página
                    if (Validador.EsInt64(descifrado))
                    {
                        Int64 idpersona = Convert.ToInt64(descifrado);
                        Personas persona = new Personas(idpersona);
                        if (persona.GetEstado() == 2)
                        {
                            Response.Redirect("/alerta-evaluacion.aspx?alerta=" + Cifrado.cifrarParaUrl("Completo"));

                        }
                        Diagnosticos diagnostico = new Diagnosticos(persona.GetIDDiagnostico());
                        if (diagnostico.GetIDTipo() == 1)
                        {
                            dlPreguntas.Visible = true;
                            dlPreguntasAbiertas.Visible = false;
                        }
                        else
                        {
                            dlPreguntas.Visible = false;
                            dlPreguntasAbiertas.Visible = true;
                        }
                        //Si el estado es 1 (Editable) lo pongo a 2(en espera para que no se pueda modificar)
                        if (diagnostico.GetIDEstado() == 1)
                        {
                            diagnostico.ActualizarEstado(2);
                        }
                        //Si el estado es 3 el formulario esta cerrado
                        if (diagnostico.GetIDEstado() == 3)
                        {
                            Response.Redirect("/alerta-evaluacion.aspx?alerta=" + Cifrado.cifrarParaUrl("Cerrado"));
                        }
                        if (diagnostico.GetContadorDimensiones() > 0)
                        {
                            Int64 idprimeradimension = diagnostico.GetIDDimension(0);
                            this.hfPaginacion.Value = 0.ToString();
                            this.hfIDPersona.Value = idpersona.ToString();
                            hfDimension.Value = idprimeradimension.ToString();
                            //Empezariamos a cargar las preguntas de la dimensión
                            cmdAtras.Visible = false;

                            Resultados resultados = new Resultados();

                            Dimensiones dimension = new Dimensiones(idprimeradimension);

                            literalTituloDimension.Text = "<h3>" + dimension.GetTitulo() + "</h3>";

                            if (0 == diagnostico.GetContadorDimensiones() - 1)
                            {
                                //panelObserbaciones.Visible = true;
                                //txtObservacionesFinales.Text = persona.GetObservaciones();
                                cmdSiguiente.Text = "Guardar y Salir";
                                cmdSiguiente.CommandArgument = "Salir";
                            }

                            if (!resultados.Comprobar(idpersona, Convert.ToInt32(idprimeradimension)))
                            {


                                Int64[] idspreguntas = dimension.DevolverPreguntas();
                                if (idspreguntas.Length > 0)
                                {
                                    for (int i = 0; i < idspreguntas.Length; i++)
                                    {
                                        resultados.Insertar(idpersona, idspreguntas[i], DateTime.Now,
                                            -1,String.Empty, Request.UserHostAddress);
                                    }

                                }
                            }
                            odsPreguntas.DataBind();
                        }
                        else
                        {
                            Response.Redirect("/default.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/default.aspx");
                    }
                }
            }
        }
    }
    /// <summary>
    /// Método para recorrer todas las preguntas del data list 
    /// </summary>
    private Boolean ActualizarDatos()
    {
        Boolean preguntavacia = false;
        if (this.GetDiagnostico().GetIDTipo()==1)
        {
            for (int i = 0; i < dlPreguntas.Items.Count && !preguntavacia; i++)
            {
                //Instacio los radio buttons
                RadioButtonList rblRespuestas = (RadioButtonList)dlPreguntas.Items[i].FindControl("rblRespuesta");
                if (rblRespuestas.SelectedIndex != -1)
                {
                    //Recojo el valor del hidden field que tiene el id del resultado
                    HiddenField idresultado = (HiddenField)dlPreguntas.Items[i].FindControl("hfIdResultado");
                    //Creo el resultado y lo actualizo
                    Resultados resultado = new Resultados(Convert.ToInt64(idresultado.Value));
                    resultado.ActualizarNumero(rblRespuestas.SelectedIndex, Request.UserHostAddress);
                }
                else
                {
                    preguntavacia = true;
                }
            }
            DataBind();
            return preguntavacia;
        }
        else
        {
            for (int i = 0; i < dlPreguntasAbiertas.Items.Count && !preguntavacia; i++)
            {
                HiddenField hfIDTipo = (HiddenField)dlPreguntasAbiertas.Items[i].FindControl("hfIDTipo");
                TextBox txtRespuesta = (TextBox)dlPreguntasAbiertas.Items[i].FindControl("txtRespuesta");
                RadioButtonList rblRespuesta = (RadioButtonList)dlPreguntasAbiertas.Items[i].FindControl("rblRespuesta");
                HiddenField idresultado = (HiddenField)dlPreguntasAbiertas.Items[i].FindControl("hfIdResultado");
                if (hfIDTipo.Value == "1")
                {
                    //Numérico
                    if (rblRespuesta.SelectedIndex != -1)
                    {
                        Resultados resultado = new Resultados(Convert.ToInt64(idresultado.Value));
                        resultado.ActualizarNumero(rblRespuesta.SelectedIndex, Request.UserHostAddress);
                    }
                    else
                    {
                        preguntavacia = true;
                    }
                }
                if(hfIDTipo.Value=="2")
                {
                    //Abierta
                    if (txtRespuesta.Text.Trim() != String.Empty)
                    {
                        Resultados resultado = new Resultados(Convert.ToInt64(idresultado.Value));
                        resultado.ActualizarTexto(txtRespuesta.Text, Request.UserHostAddress);
                    }
                    else
                    {
                        preguntavacia = true;
                    }
                }
                if (hfIDTipo.Value == "3")
                {
                    //Abierta
                    if (txtRespuesta.Text.Trim() != String.Empty)
                    {
                        Resultados resultado = new Resultados(Convert.ToInt64(idresultado.Value));
                        resultado.ActualizarTexto(txtRespuesta.Text, Request.UserHostAddress);
                    }
                    else
                    {
                        preguntavacia = true;
                    }
                }
                if (hfIDTipo.Value == "4")
                {
                    //Abierta
                    
                   
                        Resultados resultado = new Resultados(Convert.ToInt64(idresultado.Value));
                        resultado.ActualizarTexto("pregunta mixta contestada", Request.UserHostAddress);
                        ResultadosMixtos mixtos = new ResultadosMixtos();
                         Preguntas preguntas = new Preguntas(resultado.GetIDPregunta());
                   
                 
                    if (preguntas.GetNumRespuestas() == -2)
                    {
                        CheckBoxList listaValores = (CheckBoxList)dlPreguntasAbiertas.Items[i].FindControl("ckbRespuesta");

                        foreach (ListItem item in listaValores.Items)
                        {
                            if (item.Selected)
                            {
                                mixtos.Insertar(Convert.ToInt64(resultado.GetIDPregunta()), Convert.ToInt64(item.Value), Convert.ToInt64(hfIDPersona.Value));

                            }
                        }
                    }
                    else
                    {
                       
                        mixtos.Insertar(Convert.ToInt64(resultado.GetIDPregunta()), Convert.ToInt64(rblRespuesta.SelectedValue), Convert.ToInt64(hfIDPersona.Value));

                            
                        
                    }
                  
                }
               
            }
            DataBind();
            return preguntavacia;
        }
    }

    protected void cmdAtras_Click(object sender, EventArgs e)
    {
        ActualizarDatos();
        Personas persona = new Personas(Convert.ToInt64(hfIDPersona.Value));
        persona.ActualizarObservaciones("", 1);
        String k = Cifrado.cifrarParaUrl(hfIDPersona.Value.ToString() + ":" + (Convert.ToInt32(hfPaginacion.Value) - 1));
        Response.Redirect("rellena-evaluacion.aspx?k=" + k);
    }

    protected void cmdSiguiente_Click(object sender, EventArgs e)
    {
        Boolean preguntavacia = ActualizarDatos();
        if (preguntavacia)
        {
            pErroresPreguntas.Visible = true;
            blErroresPreguntas.Items.Clear();
            blErroresPreguntas.Items.Add("No se puede completar la evaluación sin responder todas las preguntas");
        }
        else
        {
            String k = Cifrado.cifrarParaUrl(hfIDPersona.Value.ToString() + ":" + (Convert.ToInt32(hfPaginacion.Value) + 1));
            Resultados resultados = new Resultados();
            Personas persona = new Personas(Convert.ToInt64(hfIDPersona.Value));
            Diagnosticos diagnostico = new Diagnosticos(persona.GetIDDiagnostico());
            if (((Button)sender).CommandArgument == "Salir")
            {

                persona.ActualizarObservaciones("", 2);

                //Si no hay nadie con la evaluación pendiente y no hay nadie con el correo sin enviar cerramos la evaluacion.

                if (resultados.ComprobarResultados(Convert.ToInt64(hfIDPersona.Value)))
                {
                    //En este punto hay que mandar un email diciendo que hay una nueva respuesta
                    Usuarios usuario = new Usuarios(diagnostico.GetIDUsuario());
                    String body = Interfaz.GetValor("evaluacion_notificacion_finalizada_body");
                    Cuentas cuenta = new Cuentas(usuario.GetIDCuenta());
                    if (cuenta.GetLogotipo()!=null)
                    {
                        body = body.Replace("[logo]", "https://my.efilecoach.com/logo.aspx?" 
                            + Cifrado.cifrarParaUrl(cuenta.GetID().ToString()));
                    }
                    else
                    {
                        body = body.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
                    }

                    if (cuenta.GetIDTipo()==1)
                    {
                        body = body.Replace("[logo_pie]", "https://my.efilecoach.com/Images/logo_pie.png");
                    }

                    body = body.Replace("[nombre]", diagnostico.GetNombre());
                    body = body.Replace("[email]", persona.GetEmail());
                    body = body.Replace("[link]", "https://my.efilecoach.com/evaluaciones/evaluacion.aspx?id=" +
                            Cifrado.cifrarParaUrl(diagnostico.GetID().ToString()));
                    Email.sendEmail(Interfaz.GetValor("email_altas"), usuario.GetEmail(),
                            Interfaz.GetValor("evaluacion_notificacion_finalizada_asunto").Replace("[nombre]" , persona.GetEmail()),
                            body, Request.UserHostAddress);

                    if (diagnostico.GetContadorEspera() == 0 && diagnostico.GetContadorSinEnviar() == 0)
                    {
                        //En este punto hay que mandar un email diciendo que la evaluación ha finalizado
                        String body_todos = Interfaz.GetValor("evaluacion_notificacion_finalizada_todos_body");
                        if (cuenta.GetLogotipo() != null)
                        {
                            body_todos = body_todos.Replace("[logo]", "https://my.efilecoach.com/logo.aspx?"
                                + Cifrado.cifrarParaUrl(cuenta.GetID().ToString()));
                        }
                        else
                        {
                            body_todos = body_todos.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
                        }

                        if (cuenta.GetIDTipo() == 1)
                        {
                            body_todos = body_todos.Replace("[logo_pie]", "https://my.efilecoach.com/Images/logo_pie.png");
                        }

                        body_todos =body_todos.Replace("[nombre]", diagnostico.GetNombre());
                        body_todos = body_todos.Replace("[link]", "https://my.efilecoach.com/evaluaciones/evaluacion.aspx?id=" + 
                            Cifrado.cifrarParaUrl(diagnostico.GetID().ToString()));
                        Email.sendEmail(Interfaz.GetValor("email_altas"), usuario.GetEmail(), 
                            Interfaz.GetValor("evaluacion_notificacion_finalizada_todos_asunto").Replace("[nombre]", diagnostico.GetNombre()), 
                            body_todos, Request.UserHostAddress);
                        diagnostico.ActualizarEstado(3);
                    }

                    Response.Redirect("/alerta-evaluacion.aspx?alerta=" + Cifrado.cifrarParaUrl("Correcto"));
                }
                else
                {
                    pErroresPreguntas.Visible = true;
                    blErroresPreguntas.Items.Clear();
                    blErroresPreguntas.Items.Add("No se puede completar la evaluación sin responder todas las preguntas");
                }
            }
            else
            {
                Response.Redirect("rellena-evaluacion.aspx?k=" + k);
            }
        }

    }

    protected void dlPreguntas_ItemCommand(object source, DataListCommandEventArgs e)
    {
        
    }

    protected void dlPreguntasAbiertas_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item != null)
        {
            HiddenField hfIDTipo = (HiddenField)e.Item.FindControl("hfIDTipo"); 
            HiddenField hfIdResultado = (HiddenField)e.Item.FindControl("hfIdResultado");
            PlaceHolder phRespuestaAbierta = (PlaceHolder)e.Item.FindControl("phRespuestaAbierta");
            PlaceHolder phRespuestaNumerica = (PlaceHolder)e.Item.FindControl("phRespuestaNumerica");
            TextBox txtRespuesta = (TextBox)e.Item.FindControl("txtRespuesta");
            if (hfIDTipo.Value == "1")
            {
                phRespuestaAbierta.Visible = false;
                phRespuestaNumerica.Visible = true;

                Resultados resultados = new Resultados(Convert.ToInt64(hfIdResultado.Value));

                Int64 idp = resultados.GetIDPregunta();
                Preguntas preguntas = new Preguntas(idp);
                HtmlTableRow lista = (HtmlTableRow)e.Item.FindControl("respuestas_variables");
                int valid1 = 0;
                for (int i = 1; i <= preguntas.GetNumRespuestas(); i++)
                {

                  
                    HtmlTableCell cell = new HtmlTableCell();
                  
                        cell.Controls.Add(new LiteralControl("" + i));
                      
                     
                    
                    lista.Cells.Add(cell);

                }

                RadioButtonList listaValores = (RadioButtonList)e.Item.FindControl("rblRespuesta");
                for (int i = 1; i <= preguntas.GetNumRespuestas(); i++)
                {
                    ListItem item = new ListItem();

                  
                        item.Value = i+"";
                       
                    listaValores.Items.Add(item);
                }

            }
            if(hfIDTipo.Value=="2")
            {
                phRespuestaAbierta.Visible = true;
                phRespuestaNumerica.Visible = false;
                txtRespuesta.MaxLength = 255;
            }
            if (hfIDTipo.Value == "3")
            {
                phRespuestaAbierta.Visible = true;
                phRespuestaNumerica.Visible = false;
                txtRespuesta.MaxLength = 1000;
            }
            if (hfIDTipo.Value == "4")
            {
                phRespuestaAbierta.Visible = false;
                phRespuestaNumerica.Visible = true;

                Resultados resultados = new Resultados(Convert.ToInt64(hfIdResultado.Value));
               
                Int64 idp = resultados.GetIDPregunta();
                Preguntas preguntas = new Preguntas(idp);
                Respuestas respuestas = new Respuestas(idp);
                DataTable table = respuestas.datos.Table;
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    var respuesta = table.Rows[i]["respuesta"];
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.Controls.Add(new LiteralControl("" + respuesta));
                    HtmlTableRow lista = (HtmlTableRow)e.Item.FindControl("respuestas_variables");
                    lista.Cells.Add(cell);
                }
                if (preguntas.GetNumRespuestas() == -2)
                {
                    CheckBoxList listaValores = (CheckBoxList)e.Item.FindControl("ckbRespuesta");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var id = table.Rows[i]["id"];
                        ListItem item = new ListItem();
                        item.Value = "" + id;
                        listaValores.Items.Add(item);
                    }
                }else
                {
                    RadioButtonList listaValores = (RadioButtonList)e.Item.FindControl("rblRespuesta");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var id = table.Rows[i]["id"];
                        ListItem item = new ListItem();
                        item.Value = "" + id;
                        listaValores.Items.Add(item);
                    }
                }
            }
        }
    }
}