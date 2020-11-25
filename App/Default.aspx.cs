using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    private Boolean ValidarCampos()
    {
        panelFeedback.Visible = false;
        blErrores.Items.Clear();
        Boolean error = false;

        if(txtUsuario.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El usuario no puede estar vacío");
            error = true;
        }
        else
        {
            if(!Interfaz.EsEmail(txtUsuario.Text.Trim()))
            {
                blErrores.Items.Add("El email del usuario no tiene el formato correcto");
                error = true;
            }
        }

        if (txtPassword.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("La clave no puede estar vacía");
            error = true;
        }

        return !error;
    }

    protected void cmdAcceder_Click(object sender, EventArgs e)
    {
        if(this.ValidarCampos())
        {
            Cuentas cuenta = new Cuentas(this.txtUsuario.Text.Trim().ToLower());
            Boolean error = false;
            if (cuenta.TieneDatos())
            {
                if (cuenta.GetAutentico(txtPassword.Text.Trim()))
                {
                    //Es un usuario de los que contrata una cuenta
                    Session["idusuario"] = cuenta.GetIDUsuarioPrincipal();
                    Session["idcuenta"] = cuenta.GetID();
                    Session["tipousuario"] = "cuenta";
                    CSesion csesion = new CSesion(cuenta.GetID(), "cuenta", null, cuenta);
                    Session["csesion"] = csesion;
                }
                else
                {
                    error = true;
                }
            }
            else
            {
                //Es un usuario (no final)
                Usuarios usuario = new Usuarios(this.txtUsuario.Text.Trim());
                if (usuario.GetAutentico(txtPassword.Text.Trim()))
                {
                    Session["idusuario"] = usuario.GetID();
                    Session["tipousuario"] = "usuario";
                    Session["idcuentapadre"] = usuario.GetIDCuenta();
                    Session["idcuenta"] = usuario.GetIDCuenta();
                    CSesion csesion = new CSesion(usuario.GetID(), "usuario", usuario, null);
                    Session["csesion"] = csesion;
                }
                else
                {
                    error = true;
                }
            }
            
            if(error)
            {
                Interfaz.InsertarLog("Intento de Acceso Fallido", Request.UserHostAddress,
                    txtUsuario.Text.Trim(), HttpContext.Current.Request.Url.AbsoluteUri);
                literalFeedback.Text = "Usuario o contraseña incorrectos.";
                panelFeedback.Visible = true;
                panelFeedback.CssClass = "alert alert-danger";
            }
            else
            {
                //Si estabamos en otra página, vamos a intentar ir a ella
                if (Request.QueryString["w"]!=null)
                {
                    Response.Redirect(Request.QueryString["w"]);
                }
                else
                {
                    Response.Redirect("inicio.aspx");
                }
            }
        }
        else
        {
            panelFeedback.Visible = true;
            panelFeedback.CssClass = "alert alert-danger";
        }
    }

    protected void cmdRecuperarClave_Click(object sender, EventArgs e)
    {
        
    }

    protected void cmdRecuperarClave_Click1(object sender, EventArgs e)
    {
        panelFeedback.Visible = false;
        blErrores.Items.Clear();
        if (txtUsuario.Text.Trim() == String.Empty)
        {
            literalFeedback.Text = "Para recuperar la contraseña tiene que introducir el correo electrónico.";
        }
        else
        {
            String url = String.Empty;
            Usuarios usuario = new Usuarios(txtUsuario.Text);
            CryptoAES c = new CryptoAES();
            if (!usuario.TieneDatos())
            {
                Cuentas cuenta = new Cuentas(txtUsuario.Text.Trim());
                if (!cuenta.TieneDatos())
                {
                    literalFeedback.Text = "El Correo electrónico introducido no se encuentra en nuestra Base de Datos.";
                }
            }

            if (blErrores.Items.Count == 0)
            {
                String dominio_protocolo = Controlador.Interfaz.GetValor("dominio_protocolo");
                String contenido = Interfaz.GetValor("email_recuperar_clave_contenido");
                String asunto = Interfaz.GetValor("email_recuperar_clave_asunto");
                url = "<a href ='" + dominio_protocolo + "/recuperar-clave.aspx?k="
                    + c.cifrar(txtUsuario.Text.Trim().ToLower(), false) + "&k2="
                    + c.cifrar(DateTime.Now.ToString(), true) + "'>Recuperar contraseña</a>";

                contenido = contenido.Replace("[CONTENIDO]", url);
                contenido = contenido.Replace("[URL_LOGO]", dominio_protocolo + "/Images/logo_efile_coach.jpg");
                Email.sendEmail(Interfaz.GetValor("email_altas"), txtUsuario.Text.Trim(),
                    asunto, contenido, Request.UserHostAddress);
                panelFeedback.Visible = true;
                panelFeedback.CssClass = "alert alert-info";
                literalFeedback.Text = "Si ha proporcionado una cuenta de correo correcta, enviaremos un email con un enlace para cambiar su clave";
            }
            else
            {
                panelFeedback.Visible = true;
                panelFeedback.CssClass = "alert alert-danger";
            }
        }
    }
}