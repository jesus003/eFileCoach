using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class recuperar_clave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["k"] == null || Request.QueryString["k2"] == null)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                CryptoAES c = new CryptoAES();
                DateTime fecha = DateTime.Parse(c.descifrar(Request.QueryString["k2"],true).ToString());
                Double horas = (DateTime.Now - fecha).TotalHours;
                if (horas>24)
                {
                    cmdConfirmar.Visible = false;
                    literalFeedback.Text = "El enlace ha caducado. Debe solicitar uno nuevo.";
                    panelFeedback.Visible = true;
                    panelFeedback.CssClass = "alert alert-danger";
                }
            }
        }
    }

    protected void cmdConfirmar_Click(object sender, EventArgs e)
    {
        if (ValidarCampos())
        {
            CryptoAES c = new CryptoAES();
            String email = c.descifrar(Request.QueryString["k"].ToString(), false);
            Cuentas cuenta = new Cuentas(email);
            if (!cuenta.TieneDatos())
            {
                Usuarios usuario = new Usuarios(email);
                if (!usuario.TieneDatos())
                {
                    blErrores.Items.Add("El usuario no existe");
                }
                else
                {
                    if (usuario.ActualizarClave(txtPassword.Text.Trim()))
                    {
                        blErrores.Items.Add("Se ha restablecido la clave con éxito.");
                        panelFeedback.Visible = true;
                        panelFeedback.CssClass = "alert alert-success";
                    }
                    else
                    {
                        blErrores.Items.Add("Se ha producido un errror al restablecer la clave.");
                        panelFeedback.Visible = true;
                        panelFeedback.CssClass = "alert alert-danger";
                    }
                }
            }
            else
            {
                if (cuenta.ActualizarClave(txtPassword.Text.Trim()))
                {
                    blErrores.Items.Add("Se ha restablecido la clave con éxito.");
                    panelFeedback.Visible = true;
                    panelFeedback.CssClass = "alert alert-success";
                }
                else
                {
                    blErrores.Items.Add("Se ha producido un errror al restablecer la clave.");
                    panelFeedback.Visible = true;
                    panelFeedback.CssClass = "alert alert-danger";
                }
            }
        }
        else
        {
            panelFeedback.Visible = true;
        }
    }

    private Boolean ValidarCampos()
    {
        panelFeedback.Visible = false;
        blErrores.Items.Clear();
        if (txtPassword.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El campo contraseña no puede estar vacío");
        }

        if (txtPasswordRepeat.Text.Trim() == String.Empty || txtPassword.Text.Trim() != txtPasswordRepeat.Text.Trim())
        {
            blErrores.Items.Add("Ambos campos deben tener la misma contraseña");
        }

        if (blErrores.Items.Count==0)
        {
            return true;
        }
        else
        {
            panelFeedback.Visible = true;
            panelFeedback.CssClass = "alert alert-danger";
            return false;
        }
    }
}