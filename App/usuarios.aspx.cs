using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;

public partial class usuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (usuarioLogueado.EsCuenta())
            {
                this.grdUsuarios.DataBind();
                if (Session["usuarios_pagina"] != null)
                {
                    grdUsuarios.PageIndex = Int32.Parse(Session["usuarios_pagina"].ToString());
                }

                if (Session["usuarios_por_pagina"] != null)
                {
                    cmbPaginas.SelectedValue = Session["usuarios_por_pagina"].ToString();
                    grdUsuarios.PageSize = Int32.Parse(cmbPaginas.SelectedValue);
                }
                if (Session["buscar_usuarios"] != null)
                {
                    txtBuscador.Text = Session["buscar_usuarios"].ToString();
                    this.Buscar();
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect("/default.aspx");
            }
        }
    }

    protected void cmdNuevaCuenta_Click(object sender, EventArgs e)
    {
        this.InsertarNuevoUsuario();
    }

    private void InsertarNuevoUsuario()
    {
        if (this.ValidarCampos())
        {
            Usuarios u = new Usuarios();
            Int64 last_id = u.Insertar(usuarioLogueado.GetIDCuenta(), 
                Convert.ToInt64(cmbTiposUsuario.SelectedValue), txtEmail.Text.ToLower().Trim(),
                hfClave.Value.ToString().Trim(), txtNombre.Text.Trim(), 
                txtApellidos.Text.Trim(), String.Empty, String.Empty, String.Empty,
                String.Empty, String.Empty, String.Empty, String.Empty, DateTime.Now, 1,false);
            if (last_id != -1)
            {
                CryptoAES c = new CryptoAES();
                String url = c.cifrar(last_id.ToString(), true);
                Response.Redirect("usuario.aspx?i=" + url);
            }
        }
    }

    private Boolean ValidarCampos()
    {
        panelErroresExterior.Visible = false;
        blErrores.Items.Clear();

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
            if (!Interfaz.EsEmail(txtEmail.Text.Trim()))
            {
                blErrores.Items.Add("El email introducido no tiene el formato correcto");
            }
            else
            {
                Usuarios usuario = new Usuarios(txtEmail.Text.Trim().ToLower());
                if(usuario.TieneDatos())
                {
                    blErrores.Items.Add("El email ya está asociado a alguna cuenta");
                }
            }
        }

        if (!txtClave.ReadOnly)
        {
            hfClave.Value = txtClave.Text.Trim();
        }

        if (hfClave.Value.ToString().Trim().Length<6)
        {
            blErrores.Items.Add("Debe introducir una contraseña de al menos 6 caracteres");
        }

        if(txtNombre.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El campo nombre no puede estar vacío");
        }

        if(txtApellidos.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El campo apellidos no puede estar vacío");
        }

        if(blErrores.Items.Count>0)
        {
            panelErroresExterior.Visible = true;
            panelErrores.CssClass = "alert alert-danger";
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void grdUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 id = Convert.ToInt64(e.CommandArgument);
        CryptoAES c = new CryptoAES();
        String url = c.cifrar(id.ToString(), true);
        switch (e.CommandName)
        {
            case "detalles":
                Response.Redirect("usuario.aspx?i=" + url);
                break;
            case "procesos_coaching":
                Response.Redirect("procesos.aspx?iu=" + url);
                break;
            case "formaciones":
                Response.Redirect("formaciones.aspx?iu=" + url);
                break;
        }
    }

    protected void RadButton1_Click(object sender, EventArgs e)
    {

    }

    protected void cmdEditarClave_Click(object sender, EventArgs e)
    {
        if (txtClave.ReadOnly)
        {
            txtClave.ReadOnly = false;
            txtClave.Text = hfClave.Value.ToString();
            txtClave.Focus();
        }
        else
        {
            hfClave.Value = txtClave.Text;
            txtClave.ReadOnly = true;
        }
    }

    protected void cmdBuscadorClientes_Click(object sender, EventArgs e)
    {
        this.Buscar();
    }

    private void Buscar()
    {
        String filtro = txtBuscador.Text.Trim();
        if (filtro!=String.Empty)
        {
            Session["buscar_usuarios"] = filtro;
            odsUsuarios.FilterExpression += " nombre LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " apellidos LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " cod_postal LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " email LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " nif LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " movil LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " poblacion LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " provincia LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " tipo LIKE '%" + filtro + "%' OR ";
            odsUsuarios.FilterExpression += " telefono LIKE '%" + filtro + "%' ";
            cmdResetBusqueda.Visible = true;
        }
        else
        {
            Session["buscar_usuarios"] = null;
            odsUsuarios.FilterExpression = String.Empty;
            cmdResetBusqueda.Visible = false;
        }
        grdUsuarios.DataBind();
    }

    protected void cmdResetBusqueda_Click(object sender, EventArgs e)
    {
        Session["buscar_usuarios"] = null;
        Session["usuarios_pagina"] = 0;
        odsUsuarios.FilterExpression = String.Empty;
        grdUsuarios.DataBind();
        txtBuscador.Text = String.Empty;
        cmdResetBusqueda.Visible = false;
        grdUsuarios.PageIndex = 0;
    }

    protected void grdUsuarios_PageIndexChanged(object sender, EventArgs e)
    {
        this.Buscar();
        Session["usuarios_pagina"] = grdUsuarios.PageIndex;
    }

    protected void cmbPaginas_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Buscar();
        Session["usuarios_por_pagina"] = cmbPaginas.SelectedValue;
        grdUsuarios.PageSize = Int32.Parse(cmbPaginas.SelectedValue);
    }

    protected void grdUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}

 