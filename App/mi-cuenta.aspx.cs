using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;
using Telerik.Web.UI;

public partial class mi_cuenta : System.Web.UI.Page
{
    Cuentas cuenta;
    CSesion sesion;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.sesion = (CSesion)Session["csesion"];
        this.cuenta = sesion.GetCuenta();
        String id = cuenta.GetID().ToString();
        editorFirma.ImageManager.ViewPaths = new string[] { "/imagenes_usuarios/" + id };
        editorFirma.ImageManager.DeletePaths = new string[] { "/imagenes_usuarios/" + id };
        editorFirma.ImageManager.UploadPaths = new string[] { "/imagenes_usuarios/" + id };
        editorFirma.ImageManager.EnableAsyncUpload = true;
        editorFirma.ImageManager.MaxUploadFileSize = 20490000;
        editorFirma.ImageManager.SearchPatterns = new string[] { "*.jpeg", "*.jpg", "*.bmp", "*.gif", "*.png" };
        if (!IsPostBack)
        {
            this.CargarDatos();
            this.CargarDatosSMTP();
            this.CargarDatosSuscripcion();
        }
    }

    private void CargarDatos()
    {
        txtEmail.Text = cuenta.GetEmail();
        txtEmail.Enabled = false;
        txtNombre.Text = cuenta.GetNombre();
        txtApellidos.Text = cuenta.GetApellidos();
        txtNIF.Text = cuenta.GetNIF();
        txtTelefono.Text = cuenta.GetTelefono();
        txtMovil.Text = cuenta.GetMovil();
        txtDireccion.Text = cuenta.GetDireccion();
        txtPoblacion.Text = cuenta.GetPoblacion();
        txtProvincia.Text = cuenta.GetProvincia();
        txtCodPos.Text = cuenta.GetCodigoPostal();
        this.CargarLogo();
    }

    private void CargarLogo()
    {
        if (cuenta.GetLogotipo() == null)
        {
            hlLogotipo.Visible = false;
            imgLogotipo.Visible = false;
            imgLogotipo.ImageUrl = String.Empty;
            cmdBorrarFoto.Visible = false;
        }
        else
        {
            hlLogotipo.Visible = true;
            imgLogotipo.Visible = true;
            imgLogotipo.ImageUrl = "/logo.aspx?" + Cifrado.cifrarParaUrl(cuenta.GetID().ToString());
            cmdBorrarFoto.Visible = true;
        }
    }

    private void CargarDatosSMTP()
    {
        txtAPIMailchimp.Text = cuenta.GetAPIMailchimp();
        txtSMTPClave.Text = cuenta.GetSMTPClave();
        txtSMTPEmailFrom.Text = cuenta.GetSMTPFrom();
        txtSMTPFromUsuario.Text = cuenta.GetSMTPUsuario();
        txtSMTPPuerto.Value = Double.Parse(cuenta.GetSMTPPuerto().ToString());
        txtSMTPFromTXT.Text = cuenta.GetSMTPFromTxt();
        txtSMTPServidor.Text = cuenta.GetSMTPServidor();
        editorFirma.Content = cuenta.GetSMTPFirma();
        if (cuenta.GetSMTPSSL())
        {
            cmbSMTPSSL.SelectedValue = "1";

        }
        else
        {
            cmbSMTPSSL.SelectedValue = "0";
        }
    }

    private void CargarDatosSuscripcion()
    {
        txtTipoCuenta.Text = cuenta.GetTipoDeCuenta();
        txtIBAN.Text = cuenta.GetCuentaBancaria();
    }

    protected void cmdGuardar_Click(object sender, EventArgs e)
    {
        if (this.Validar())
        {
            if (cuenta.ActualizarPrincipal(txtEmail.Text,txtNombre.Text,
                txtApellidos.Text,txtTelefono.Text,
                txtMovil.Text,txtDireccion.Text,txtCodPos.Text,
                txtPoblacion.Text,txtProvincia.Text,txtNIF.Text))
            {
                Boolean error = false;
                
                if (!error)
                {
                    if (txtClave1.Text.Trim() != String.Empty)
                    {
                        //Tengo que actualizar la clave
                        if (!cuenta.ActualizarClave(txtClave1.Text))
                        {
                            panelFeedBack.Visible = true;
                            panelFeedBackInterno.CssClass = "alert alert-danger";
                            lFeedBack.Text = "Se ha producido un error al actualizar la clave de la cuenta";
                            error = true;
                        }
                    }
                    if (!error)
                    {
                        panelFeedBack.Visible = true;
                        panelFeedBackInterno.CssClass = "alert alert-success";
                        lFeedBack.Text = "Datos actualizados con éxito";
                    }
                    this.sesion.ActualizarCuenta(new Cuentas(this.sesion.GetCuenta().GetID()));
                }
                else
                {
                    panelFeedBack.Visible = true;
                    panelFeedBackInterno.CssClass = "alert alert-danger";
                    lFeedBack.Text = "Se ha producido un error al actualizar los datos";
                }
            }
            else
            {
                panelFeedBack.Visible = true;
                panelFeedBackInterno.CssClass = "alert alert-danger";
                lFeedBack.Text = "Se ha producido un error al actualizar los datos del correo";
            }
        }
    }

    protected void cmdGuardarSMTP_Click(object sender, EventArgs e)
    {
        if (this.ValidarSMTP())
        {
            if (cuenta.ActualizarCorreo(txtAPIMailchimp.Text.Trim(),txtSMTPEmailFrom.Text,txtSMTPFromUsuario.Text,
                txtSMTPClave.Text,Convert.ToInt16(cmbSMTPSSL.SelectedValue),
                Convert.ToInt32(txtSMTPPuerto.Value),txtSMTPFromTXT.Text,
                txtSMTPServidor.Text,editorFirma.Content))
            {
                panelFeedBackSMTP.Visible = true;
                panelFeedBackSMTPInterno.CssClass = "alert alert-success";
                lFeedbackSMTP.Text = "Datos del correo actualizados con éxito";
                this.sesion.ActualizarCuenta(new Cuentas(this.sesion.GetCuenta().GetID()));
            }
            else
            {
                panelFeedBackSMTP.Visible = true;
                panelFeedBackSMTPInterno.CssClass = "alert alert-danger";
                lFeedbackSMTP.Text = "Se ha producido un error al actualizar los datos del correo";
            }
        }
    }

    private Boolean Validar()
    {
        panelFeedBack.Visible = false;
        lFeedBack.Text = String.Empty;
        blFeedBack.Items.Clear();

        if (txtClave1.Text!=String.Empty || txtClave2.Text!=String.Empty)
        {
            if (txtClave1.Text!=txtClave2.Text)
            {
                blFeedBack.Items.Add("Las dos claves no coinciden");
            }
            else
            {
                if (txtClave1.Text.Length<6)
                {
                    blFeedBack.Items.Add("La clave debe tener una longitud mínima de 6 caracteres");
                }
            }
        }

        if (blFeedBack.Items.Count==0)
        {
            return true;
        }
        else
        {
            panelFeedBack.Visible = true;
            panelFeedBackInterno.CssClass = "alert alert-danger";
            return false;
        }
    }

    private Boolean ValidarSMTP()
    {
        panelFeedBackSMTP.Visible = false;
        lFeedbackSMTP.Text = String.Empty;
        blFeedBackSMTP.Items.Clear();

        if (!Interfaz.EsEmail(txtSMTPEmailFrom.Text))
        {
            blFeedBackSMTP.Items.Add("El correo electrónico no está bien escrito");
        }

        if (blFeedBackSMTP.Items.Count == 0)
        {
            return true;
        }
        else
        {
            panelFeedBackSMTP.Visible = true;
            panelFeedBackSMTPInterno.CssClass = "alert alert-danger";
            return false;
        }
    }

    private Boolean ValidarSuscripcion()
    {
        panelFeedbackSuscripcion.Visible = false;
        literalFeedbackSuscripcion.Text = String.Empty;
        blFeedbackSuscripcion.Items.Clear();
        if (blFeedbackSuscripcion.Items.Count == 0)
        {
            return true;
        }
        else
        {
            panelFeedbackSuscripcion.Visible = true;
            panelFeedbackSuscripcionInterior.CssClass = "alert alert-danger";
            return false;
        }
    }

    protected void cmdGuardarSuscripcion_Click(object sender, EventArgs e)
    {
        if (this.ValidarSuscripcion())
        {
            if (cuenta.ActualizarSuscripcion(txtIBAN.Text))
            {
                panelFeedbackSuscripcion.Visible = true;
                panelFeedbackSuscripcionInterior.CssClass = "alert alert-success";
                literalFeedbackSuscripcion.Text = "Datos del correo actualizados con éxito";
                this.sesion.ActualizarCuenta(new Cuentas(this.sesion.GetCuenta().GetID()));
            }
            else
            {
                panelFeedbackSuscripcion.Visible = true;
                panelFeedbackSuscripcionInterior.CssClass = "alert alert-danger";
                literalFeedbackSuscripcion.Text = "Se ha producido un error al actualizar los datos del correo";
            }
        }
    }

    protected void cmdBorrarFoto_Click(object sender, EventArgs e)
    {
        if (cuenta.ActualizarLogo(null))
        {
            ventanita.RadAlert("Se ha eliminado la foto con éxito", 
                null, null, "efileCoach", Interfaz.ICO_ok);
            imgLogotipo.ImageUrl = "";
            imgLogotipo.Visible = false;
            hlLogotipo.Visible = false;
        }
        else
        {
            ventanita.RadAlert("Se ha producido un error al eliminar la foto", 
                null, null, "efileCoach", Interfaz.ICO_alert);
        }
    }

    protected void cmdGuardarFoto_Click(object sender, EventArgs e)
    {
        if (fuLogo.UploadedFiles.Count > 0)
        {
            //Subimos el logotipo
            // get uploaded file
            UploadedFile attachment = fuLogo.UploadedFiles[0];
            byte[] attachmentBytes = new byte[attachment.InputStream.Length];
            attachment.InputStream.Read(attachmentBytes, 0, attachmentBytes.Length);
            Boolean error = !cuenta.ActualizarLogo(attachmentBytes);
            if (!error)
            {
                //this.sesion.ActualizarCuenta(new Cuentas(this.sesion.GetCuenta().GetID()));
                //this.CargarLogo();
                imgLogotipo.ImageUrl = "/logo.aspx?" + Cifrado.cifrarParaUrl(cuenta.GetID().ToString());
                imgLogotipo.Visible = true;
                hlLogotipo.Visible = true;
                cmdBorrarFoto.Visible = true;
            }
        }
    }
}