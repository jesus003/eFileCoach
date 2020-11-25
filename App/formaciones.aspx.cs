using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class formaciones : System.Web.UI.Page
{
    CSesion csesion;
    protected void Page_Load(object sender, EventArgs e)
    {
        csesion = (CSesion)Session["csesion"];
        if (!IsPostBack)
        {
            //cmbFormadores.SelectedValue = null;
            panelNuevo.Visible = csesion.EsCuenta();
            this.Enlazar();
        }
    }

    private void Enlazar()
    {
        if (this.GetIDCliente() != null)
        {
            hfIDCliente.Value = this.GetIDCliente().ToString();
            grdFormaciones.DataSourceID = "odsFormacionesCliente";
            hlVolver.NavigateUrl = "clientes.aspx";
        }
        else
        {
            if (csesion.EsUsuario())
            {
                grdFormaciones.DataSourceID = "odsFormacionesUsuario";
            }
        }
        if (this.GetIDUsuario() != null)
        {
            hlVolver.NavigateUrl = "usuarios.aspx";
        }
    }

    /// <summary>
    /// Método que filtra por usuario. Venimos de la página usuarios.aspx
    /// </summary>
    /// <returns></returns>
    public Int64? GetIDUsuario()
    {
        if (Request.QueryString["iu"] == null)
        {
            return null;
        }
        else
        {
            return Int64.Parse(Cifrado.descifrar(Request.QueryString["iu"],true));
        }
    }

    /// <summary>
    /// Método que filtra por cliente. Venimos de la página clientes.aspx
    /// </summary>
    /// <returns></returns>
    public Int64? GetIDCliente()
    {
        if (Request.QueryString["ic"] == null)
        {
            return null;
        }
        else
        {
            return Int64.Parse(Cifrado.descifrar(Request.QueryString["ic"],true));
        }
    }

    protected void cmdNuevaFormacion_Click(object sender, EventArgs e)
    {
        if (this.ValidarCampos())
        {
            Formaciones formacion = new Formaciones();
            Int64 idformacion = formacion.Insertar(false,this.csesion.GetCuenta().GetID(),
                Int64.Parse(cmbFormadores.SelectedValue), null, DateTime.Now, null,
                txtTituloFormacion.Text, String.Empty,String.Empty);
            if (idformacion!=-1)
            {
                Calendario calendario = new Calendario(this.csesion.GetCuenta().GetID(), true);
                String idgooglecalendar = calendario.NuevoEvento(txtTituloFormacion.Text, "", 
                    DateTime.Now, DateTime.Now.AddMinutes(60));
                if (idgooglecalendar!="")
                {
                    formacion = new Formaciones(idformacion);
                    formacion.ActualizarGoogleCalendar(idgooglecalendar);
                }
                CryptoAES c = new CryptoAES();
                Response.Redirect("formacion.aspx?id=" + c.cifrar(idformacion.ToString(),true));
            }
            else
            {
                panelErroresExterior.Visible = true;
                panelErrores.CssClass = "alert alert-danger";
                blErrores.Items.Add("Se ha producido un error al crear la formación");
            }
        }
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErroresExterior.Visible = false;
        panelErrores.CssClass = String.Empty;
        if (txtTituloFormacion.Text.Trim()==String.Empty)
        {
            blErrores.Items.Add("Debe rellenar el título de la acción formativa");
        }

        if (cmbFormadores.SelectedValue==String.Empty)
        {
            blErrores.Items.Add("Debe seleccionar el formador");
        }

        if (blErrores.Items.Count>0)
        {
            panelErroresExterior.Visible = true;
            panelErrores.CssClass = "alert alert-danger";
            blErrores.Visible = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void grdFormaciones_DataBound(object sender, EventArgs e)
    {
        if (this.GetIDUsuario()!=null)
        {
            //Aplicamos filtro por idformador
            odsFormaciones.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
            odsFormacionesUsuario.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
        }
        else
        {
            if (this.GetIDCliente()!=null)
            {
                //Aplicamos filtro por que participe en la formación. Este es más dificil
            }
        }
    }

    protected void grdFormaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex!=-1)
        {
            CryptoAES c = new CryptoAES();
            HyperLink hlDetalles = (HyperLink)e.Row.FindControl("hlDetalles");
            Int64 idformacion = Convert.ToInt64(hlDetalles.NavigateUrl);
            hlDetalles.NavigateUrl = "/formacion.aspx?id=" + c.cifrar(idformacion.ToString(), true);
        }
    }

    protected void cmdBuscadorFormaciones_Click(object sender, EventArgs e)
    {
        if (txtBuscador.Text.Trim()==String.Empty)
        {
            if (this.GetIDUsuario() != null)
            {
                //Aplicamos filtro por idformador
                odsFormaciones.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
                odsFormacionesUsuario.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
            }
            else
            {
                odsFormaciones.FilterExpression = String.Empty;
                odsFormacionesUsuario.FilterExpression = String.Empty;
            }
            odsFormacionesCliente.FilterExpression = String.Empty;

        }
        else
        {
            if (this.GetIDUsuario() != null)
            {
                odsFormaciones.FilterExpression = String.Format("idformador={0} AND (titulo LIKE '%{1}%' OR descripcion LIKE '%{1}%')",
                this.GetIDUsuario().ToString(), txtBuscador.Text.Trim());
                odsFormacionesUsuario.FilterExpression = String.Format("idformador={0} AND (titulo LIKE '%{1}%' OR descripcion LIKE '%{1}%')",
                this.GetIDUsuario().ToString(), txtBuscador.Text.Trim());
            }
            else
            {
                odsFormaciones.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
                odsFormacionesUsuario.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                    txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
            }

            odsFormacionesCliente.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
        }
        grdFormaciones.DataBind();
    }
}