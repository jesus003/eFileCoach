using Controlador;
using Controlador.Diagnosticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class evaluaciones_edimension : System.Web.UI.Page
{
    Int64 iddimension = -1;
    Dimensiones dimension;
    Diagnosticos diagnostico = new Diagnosticos();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.iddimension = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"], true));
        hfIDDimension.Value = this.iddimension.ToString();
        this.dimension = new Dimensiones(this.iddimension);
        diagnostico = new Diagnosticos(dimension.GetIDDiagnostico());
        this.InicializaDatos();

        }
        else
        {
            Response.Redirect("/default.aspx");
        }
        if (!IsPostBack)
        {


            for (int i = 1; i <= 100; i++)
            {
                ListItem item = new ListItem();
                item.Value = ""+i;
                cmbxRespuestas.Items.Add(item);
            }

                if (diagnostico.GetIDTipo() == 1)
                {
                    cmbTipoPregunta.Items.Add(new ListItem("Numérica", "1"));
                    cmbTipoPregunta.Items.Add(new ListItem("Libre", "2"));

                }
                else
                {
                    cmbTipoPregunta.Items.Add(new ListItem("Numérica", "1"));
                    cmbTipoPregunta.Items.Add(new ListItem("Varias respuestas posibles", "4"));
                    cmbTipoPregunta.Items.Add(new ListItem("Libre (Texto Corto - 255 Caracteres)", "2"));
                    cmbTipoPregunta.Items.Add(new ListItem("Libre (Parrafo - Hasta 1000 Caracteres)", "3"));
                }
           
            this.CargarDatos();
        }


        
    }

    private void CargarDatos()
    {
        hlVolver.NavigateUrl = String.Format("evaluacion.aspx?id={0}", Cifrado.cifrarParaUrl(this.dimension.GetIDDiagnostico().ToString()));
        this.txtNombre.Text = this.dimension.GetTitulo();
    }
    protected void cmbTipoPregunta_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (cmbTipoPregunta.SelectedValue)
        {
            case "1":
                divRespuesas.Visible = false;
                divPregunta.Visible = true;
                txtPregunta.Text = "";
                txtRespuestas.Text = "";
                div_num_respuestas.Visible = true;
                divTipoRespuestas.Visible = false;
                break;
            case "2":
                divPregunta.Visible = true;
                divRespuesas.Visible = false;
                txtPregunta.Text = "";
                txtRespuestas.Text = "";
                div_num_respuestas.Visible = false;
                divTipoRespuestas.Visible = false;
                break;
            case "3":
                divPregunta.Visible = true;
                divRespuesas.Visible = false;
                txtPregunta.Text = "";
                txtRespuestas.Text = "";
                div_num_respuestas.Visible = false;
                divTipoRespuestas.Visible = false;
                break;
            case "4":
                divRespuesas.Visible = true;
                divPregunta.Visible = true;
                txtPregunta.Text = "";
                txtRespuestas.Text = "";
                div_num_respuestas.Visible = false;
                divTipoRespuestas.Visible = true;
                break;
            default:
                break;
        }
    }
    private void InicializaDatos()
    {
        if (this.Request.QueryString["id"] != null)
        {
            if (Validador.EsInt64(Cifrado.descifrar(Request.QueryString["id"],true)))
            {
              
                if (diagnostico.GetIDTipo()==1)
                {
                    cmbTipoPregunta.Enabled = false;
                    cmbTipoPregunta.SelectedValue = "1";
                    divPregunta.Visible = true;
                    divRespuesas.Visible = true;
                }
                else
                {
                    cmbTipoPregunta.Enabled = true;
                }
                if (diagnostico.GetIDEstado() != 1)
                {
                    for (int i = 0; i < grdPreguntas.Rows.Count; i++)
                    {
                        Button cmdEditar = (Button)grdPreguntas.Rows[i].FindControl("cmdEditar");
                        Button cmdEliminar = (Button)grdPreguntas.Rows[i].FindControl("cmdEliminar");
                        cmdEditar.Enabled = false;
                        cmdEliminar.Enabled = false;

                        LinkButton cmdBajar = (LinkButton)grdPreguntas.Rows[i].FindControl("cmdBajar");
                        cmdBajar.Enabled = false;
                        LinkButton cmdSubir = (LinkButton)grdPreguntas.Rows[i].FindControl("cmdSubir");
                        cmdSubir.Enabled = false;
                    }

                    panelNuevaPregunta.Enabled = false;
                    txtNombre.Enabled = false;
                    cmdGuardar.Enabled = false;
                }
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

    private void ColumnasEdicion(Boolean visible)
    {
        grdPreguntas.Columns[0].Visible = visible;
        grdPreguntas.Columns[2].Visible = visible;
        grdPreguntas.Columns[3].Visible = visible;
    }

    protected void grdPreguntas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 idpregunta;
        Preguntas pregunta;
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
                pregunta = new Preguntas(idpregunta);
                String texto = ((TextBox)grdPreguntas.Rows[grdPreguntas.EditIndex].FindControl("txtPregunta")).Text;
                if (pregunta.Actualizar(texto.Trim(), pregunta.GetOrden()))
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
                pregunta = new Preguntas(idpregunta);
                if (pregunta.Eliminar())
                {
                    grdPreguntas.DataBind();
                }
                else
                {
                    ventanita.RadAlert("No se ha podido eliminar la pregunta", null, null, "eFilecoach", "", Interfaz.ICO_alert);
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
                Preguntas pregunta_fila =
                    new Preguntas(idfila);

                Preguntas pregunta_fila_anterior =
                    new Preguntas(idfila_anterior);

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

                Preguntas pregunta_fila2 =
                    new Preguntas(idfila2);

                Preguntas pregunta_fila_anterior2 =
                    new Preguntas(idfila_siguiente);


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

            Preguntas pregunta = new Preguntas();
            Int64 last_id = -1;
            if (cmbTipoPregunta.SelectedValue != "4")
            {
                 last_id = pregunta.Insertar(this.iddimension, txtPregunta.Text, dimension.GetOrdenSiguiente(),
              Int64.Parse(cmbTipoPregunta.SelectedValue), Convert.ToInt32(cmbxRespuestas.SelectedValue));
            }else
            {
                 last_id = pregunta.Insertar(this.iddimension, txtPregunta.Text, dimension.GetOrdenSiguiente(),
              Int64.Parse(cmbTipoPregunta.SelectedValue), Convert.ToInt32(rblTipoRespuestas.SelectedValue));
            }
          
            if (last_id != -1)
            {

                if (cmbTipoPregunta.SelectedValue == "4")
                {
                    Respuestas respuestas = new Respuestas();
                    string respuestas_text = txtRespuestas.Text;
                    var respuestas_lista = new List<string>(respuestas_text.Split('\n'));
                    int i = 0;
                    List<Int64> respuestas_ids = new List<Int64>();
                    foreach (var resp in respuestas_lista)
                    {
                        i++;
                        respuestas_ids.Add(respuestas.Insertar(last_id, resp, i));
                    }
                    
                    if(respuestas_lista.Count == respuestas_ids.Count)
                    {
                        //Todo bien
                        txtPregunta.Text = String.Empty;
                        if (cmbTipoPregunta.Enabled)
                        {
                            cmbTipoPregunta.SelectedIndex = 0;
                        }
                        grdPreguntas.DataBind();
                    } else
                    {
                        //Fallos
                        respuestas.EliminarByIDPregunta(last_id);
                        pregunta.Eliminar();
                        panelValidacionNuevaPregunta.Visible = true;
                        ventanita.RadAlert("Se ha producido un error al dar de alta una o mas respuestas.", null, null, "eFilecoach", "", Interfaz.ICO_alert);
                    }
                  
                }
                else
                {
                    //Todo bien
                    txtPregunta.Text = String.Empty;
                    if (cmbTipoPregunta.Enabled)
                    {
                        cmbTipoPregunta.SelectedIndex = 0;
                    }
                    grdPreguntas.DataBind();
                }

               
            }
            else
            {
                //Fallos
                panelValidacionNuevaPregunta.Visible = true;
                ventanita.RadAlert("Se ha producido un error al dar de alta la pregunta.", null, null, "eFilecoach", "", Interfaz.ICO_alert);
            }
        }

    }

    private Boolean ValidarNuevaPregunta()
    {
        panelValidacionNuevaPregunta.Visible = false;
        literalResultado.Text = String.Empty;

        if (cmbTipoPregunta.SelectedIndex==0)
        {
            ventanita.RadAlert("Seleccione el tipo de pregunta", 
                null, null, "eFilecoach", "", Interfaz.ICO_alert);
            return false;
        }
        
        if (txtPregunta.Text.Trim() == String.Empty)
        {
            //literalResultado.Text = "Debe introducir el texto de la pregunta";
            ventanita.RadAlert("Debe introducir el texto de la pregunta", null, 
                null, "eFilecoach", "", Interfaz.ICO_alert);
            return false;
        }
        //if (cmbTipoPregunta.SelectedValue == "1")
        //{
        //    string respuestas_text = txtRespuestas.Text;
        //    var respuestas_lista = new List<string>(respuestas_text.Split('\n'));
        //    if (Convert.ToInt32(cmbxRespuestas.SelectedValue) != respuestas_lista.Count)
        //    {
        //        ventanita.RadAlert("Debe introducir el numero de respuestas seleccionadas", null,
        //        null, "eFilecoach", "", Interfaz.ICO_alert);
        //        return false;
        //    }
        //    //literalResultado.Text = "Debe introducir el texto de la pregunta";
            
        //}

        return true;
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        if (this.ValidarDatosCabecera())
        {
            if (!this.dimension.Actualizar(txtNombre.Text.Trim(), this.dimension.GetOrden()))
            {
                panelValidacionCabecera.Visible = true;
                ventanita.RadAlert("Se ha producido un error al guardar la categoría", null, null, "eFilecoach", "", Interfaz.ICO_alert);
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
            lResultadoCabecera.Text = "Debe introducir el nombre de la categoría";
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