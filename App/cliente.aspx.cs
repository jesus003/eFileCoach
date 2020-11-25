using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class cliente : System.Web.UI.Page
{
    Int64 id;
    UsuariosFinales u;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["i"] == null)
        {
            Response.Redirect("inicio");
        }
        else
        {
            CryptoAES c = new CryptoAES();
            String descifrado = c.descifrar(Request.QueryString["i"], true);
            this.id = Convert.ToInt64(descifrado);
            this.u = new UsuariosFinales(this.id);
            Int64 idcuenta = u.GetIDCuenta();

            if (usuarioLogueado.EsCuenta())
            {
                if (idcuenta!=usuarioLogueado.GetCuenta().GetID())
                {
                    Response.Redirect("inicio.aspx");
                }
                else
                {
                    this.u = new UsuariosFinales(this.id);
                }
            }
            else
            {
                if (idcuenta != usuarioLogueado.GetUsuario().GetIDCuenta())
                {
                    Response.Redirect("inicio.aspx");
                }
                else
                {
                    this.u = new UsuariosFinales(this.id);
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
        this.txtEmail.Text = u.GetEmail();
        this.txtNombre.Text = u.GetNombre();
        this.txtApellidos.Text = u.GetApellidos();
        this.txtNIF.Text = u.GetNIF();
        this.txtTelefono.Text = u.GetTelefono();
        this.txtMovil.Text = u.GetMovil();
        this.txtDireccion.Text = u.GetDireccion();
        this.txtCodPos.Text = u.GetDireccion();
        this.txtProvincia.Text = u.GetProvincia();
        this.cmbTiposUsuario.SelectedValue = u.GetIDTipo().ToString();
        this.txtOtraEmpresa.Text = u.GetOtraEmpresa();
        this.txtObservaciones.Text = u.GetObservaciones();
        this.txtPoblacion.Text = u.GetLocalidad();
        if (u.GetFechaNacimiento()!=null)
        {
            dtFechaNac.SelectedDate = u.GetFechaNacimiento();
        }
    }

    private void GuardarDatos()
    {
        this.pnlErrores.Visible = false;
        this.blErrores.Items.Clear();
        if (this.ValidarCampos())
        {
            if (this.u.Actualizar(Convert.ToInt64(this.cmbTiposUsuario.SelectedValue),
                txtEmail.Text.ToLower().Trim(), String.Empty, txtNombre.Text.Trim(), 
                txtApellidos.Text.Trim(), txtOtraEmpresa.Text.Trim(),
                txtNIF.Text.Trim(), txtDireccion.Text.Trim(), txtCodPos.Text.Trim(), 
                txtPoblacion.Text.Trim(), txtProvincia.Text.Trim(),
                txtTelefono.Text.Trim(), txtMovil.Text.Trim(),
                dtFechaNac.SelectedDate, 
                txtObservaciones.Text.Trim(), u.GetFechaAlta()))
            {
                this.blErrores.Items.Add("Usuario actualizado con éxito");
                pnlErrores.Visible = true;
                panelErroresInterior.CssClass = "alert alert-success";
                u = new UsuariosFinales(this.id);
                this.CargarDatos();
            }
        }
    }

    private Boolean ValidarCampos()
    {
        Boolean errores = false;

        if (cmbTiposUsuario.SelectedIndex == -1)
        {
            errores = true;
            blErrores.Items.Add("Debe seleccionar un tipo de usuario");
        }

        if (txtEmail.Text.Trim() == String.Empty)
        {
            errores = true;
            this.blErrores.Items.Add("El campo email no puede estar vacío");
        }
        else
        {
            if (!Interfaz.EsEmail(txtEmail.Text.ToLower().Trim()))
            {
                errores = true;
                blErrores.Items.Add("El email introducido no tiene el formato correcto");
            }
        }

        if (errores)
        {
            pnlErrores.Visible = true;
            panelErroresInterior.CssClass = "alert alert-danger";
        }

        return !errores;
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        this.GuardarDatos();
    }

    protected void cmdEliminar_Click(object sender, EventArgs e)
    {
        if (u.Eliminar())
        {
            Response.Redirect("clientes.aspx");
        }
        else
        {
            pnlErrores.Visible = true;
            blErrores.Items.Clear();
            blErrores.Items.Add("Se ha producido un error al eliminar el usuario");
        }
    }
}