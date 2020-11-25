using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class usuario : System.Web.UI.Page
{
    Int64 id;
    Usuarios user;
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
            this.user = new Usuarios(this.id);
            Int64 idcuenta = user.GetIDCuenta();
            if (usuarioLogueado.GetCuenta().GetID()!= idcuenta)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                this.user = new Usuarios(this.id);
            }
        }

        if (!IsPostBack)
        {
            this.CargarDatos();
        }
    }

    private void CargarDatos()
    {
        this.txtEmail.Text = user.GetEmail();
        this.txtNombre.Text = user.GetNombre();
        this.txtApellidos.Text = user.GetApellidos();
        this.txtNIF.Text = user.GetNIF();
        this.txtTelefono.Text = user.GetTelefono();
        this.txtMovil.Text = user.GetMovil();
        this.txtDireccion.Text = user.GetDireccion();
        this.txtCodPos.Text = user.GetDireccion();
        this.txtPoblacion.Text = user.GetPoblacion();
        this.txtProvincia.Text = user.GetProvincia();
        this.cmbTiposUsuario.SelectedValue = user.GetIDTipo().ToString();
        this.chkActivo.Checked = user.IsActivo();
        txtPassword.ReadOnly = true;

    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErrores.Visible = false;

        if (user.EmailEnUso(txtEmail.Text.Trim().ToLower()))
        {
            blErrores.Items.Add("El correo electrónico introducido pertenece a otro usuario");
        }

        if (cmbTiposUsuario.SelectedIndex == -1)
        {
            blErrores.Items.Add("Debe seleccionar un tipo de usuario");
        }

        if (txtEmail.Text.Trim() == String.Empty)
        {
            this.blErrores.Items.Add("El campo email no puede estar vacío");
        }
        else
        {
            if (!Interfaz.EsEmail(txtEmail.Text.ToLower().Trim()))
            {
                blErrores.Items.Add("El email introducido no tiene el formato correcto");
            }
        }

        hfClave.Value = txtPassword.Text.Trim();
        if (hfClave.Value.Trim() != String.Empty)
        {
            if (hfClave.Value.Length<6)
            {
                blErrores.Items.Add("La contraseña debe tener al menos 6 caracteres");
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
            
            if (this.user.Actualizar(Convert.ToInt64(cmbTiposUsuario.SelectedValue),
                txtEmail.Text.ToLower().Trim(),
            hfClave.Value.ToString(), txtNombre.Text.Trim(), txtApellidos.Text.Trim(),
            txtTelefono.Text.Trim(), txtMovil.Text.Trim(), txtDireccion.Text.Trim(), txtCodPos.Text.Trim(),
            txtPoblacion.Text.ToLower().Trim(), txtProvincia.Text.ToLower().Trim(), txtNIF.Text.ToLower().Trim(), 
            Convert.ToInt16(chkActivo.Checked)))
            {
                this.blErrores.Items.Add("Usuario actualizado con éxito");
                panelErrores.Visible = true;
                panelErroresInterior.CssClass = "alert alert-success";
                user = new Usuarios(this.id);
                this.CargarDatos();
            }

        }
    }

    protected void grdUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void cmdNuevaCuenta_Click(object sender, EventArgs e)
    {

    }

    protected void cmdEliminar_Click(object sender, EventArgs e)
    {
        if (user.Eliminar())
        {
            Response.Redirect("usuarios.aspx");
        }
        else
        {
            panelErrores.Visible = true;
            blErrores.Items.Clear();
            blErrores.Items.Add("Se ha producido un error al eliminar el usuario");
        }
    }

    protected void cmdEditarClave_Click(object sender, EventArgs e)
    {
        if (txtPassword.ReadOnly)
        {
            txtPassword.ReadOnly = false;
            txtPassword.Text = hfClave.Value.ToString();
            txtPassword.Focus();
        }
        else
        {
            hfClave.Value = txtPassword.Text;
            txtPassword.ReadOnly = true;
        }
    }
}