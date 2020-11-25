using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;
using System.IO;
using System.Collections;

public partial class clientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            literalMensajeAyudaCSV.Text = Interfaz.GetValor("importar_usuarios_ayuda_csv");
            if (this.usuarioLogueado.EsCuenta())
            {
                this.grdUsuarios.DataBind();

                if (Session["clientes_pagina"] != null)
                {
                    grdUsuarios.PageIndex = Int32.Parse(Session["clientes_pagina"].ToString());
                }

                if (Session["clientes_por_pagina"] != null)
                {
                    cmbPaginas.SelectedValue = Session["clientes_por_pagina"].ToString();
                    grdUsuarios.PageSize = Int32.Parse(cmbPaginas.SelectedValue);
                }

                if (Session["buscar_clientes"] != null)
                {
                    txtBuscador.Text = Session["buscar_clientes"].ToString();
                    this.Buscar();
                }
            }
            else
            {
                Response.Redirect("/inicio.aspx");
            }
        }
    }

    private void InsertarNuevoUsuarioFinal()
    {
        if (this.ValidarCampos())
        {
            UsuariosFinales u = new UsuariosFinales();
            Int64 last_id = u.Insertar(usuarioLogueado.GetIDCuenta(), Convert.ToInt64(cmbTiposUsuario.SelectedValue), txtEmail.Text.ToLower().Trim(),
            String.Empty, txtNombre.Text.Trim(), txtApellidos.Text.Trim(), String.Empty, String.Empty, String.Empty,
            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null, String.Empty, DateTime.Now);
            if (last_id != -1)
            {
                CryptoAES c = new CryptoAES();
                String url = c.cifrar(last_id.ToString(), true);
                Response.Redirect("cliente.aspx?i=" + url);
            }
        }
    }

    private Boolean ValidarCampos()
    {
        panelErrores.Visible = false;
        blErrores.Items.Clear();

        if (cmbTiposUsuario.SelectedIndex == 0)
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
            
        }

        if (txtNombre.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El campo nombre no puede estar vacío");
        }

        if (txtApellidos.Text.Trim() == String.Empty)
        {
            blErrores.Items.Add("El campo apellidos no puede estar vacío");
        }

        if (blErrores.Items.Count>0)
        {
            panelErrores.Visible = true;
            panelErrores.CssClass = "alert alert-danger m-3";
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void cmdNuevaCuenta_Click(object sender, EventArgs e)
    {
        this.InsertarNuevoUsuarioFinal();
    }

    protected void grdUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 id = Convert.ToInt64(e.CommandArgument);
        CryptoAES c = new CryptoAES();
        String url = c.cifrar(id.ToString(), true);
        switch (e.CommandName)
        {
            case "detalles":
                Response.Redirect("cliente.aspx?i=" + url);
                break;
            case "procesos_coaching":
                Response.Redirect("procesos.aspx?ic=" + url);
                break;
            case "formaciones":
                Response.Redirect("formaciones.aspx?ic=" + url);
                break;
        }
    }

    protected void cmdBuscador_Click(object sender, EventArgs e)
    {
        this.Buscar();
    }

    private void Buscar()
    {
        String filtro = txtBuscador.Text.Trim();
        if (filtro != String.Empty)
        {
            Session["buscar_clientes"] = filtro;
            odsUsuariosFinales.FilterExpression += " nombre LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " apellidos LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " cod_postal LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " email LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " nif LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " movil LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " localidad LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " provincia LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " tipo LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " telefono LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " observaciones LIKE '%" + filtro + "%' OR ";
            odsUsuariosFinales.FilterExpression += " otra_empresa LIKE '%" + filtro + "%' ";
            cmdResetBusqueda.Visible = true;
        }
        else
        {
            Session["buscar_clientes"] = null;
            odsUsuariosFinales.FilterExpression = String.Empty;
            cmdResetBusqueda.Visible = false;
        }
        grdUsuarios.DataBind();
    }

    protected void cmdResetBusqueda_Click(object sender, EventArgs e)
    {
        Session["buscar_clientes"] = null;
        Session["clientes_pagina"] = 0;
        odsUsuariosFinales.FilterExpression = String.Empty;
        grdUsuarios.DataBind();
        txtBuscador.Text = String.Empty;
        cmdResetBusqueda.Visible = false;
        grdUsuarios.PageIndex = 0;
    }

    protected void grdUsuarios_PageIndexChanged(object sender, EventArgs e)
    {
        this.Buscar();
        Session["clientes_pagina"] = grdUsuarios.PageIndex;
    }

    protected void cmbPaginas_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Buscar();
        Session["clientes_por_pagina"] = cmbPaginas.SelectedValue;
        grdUsuarios.PageSize = Int32.Parse(cmbPaginas.SelectedValue);
    }

    private String GetCampoImportar(Hashtable htCabeceras, String campo, String[] columnas)
    {
        try
        {
            if (htCabeceras.ContainsKey(campo))
            {
                return columnas[(Int32)htCabeceras[campo]].Trim();
            }
            else
            {
                return String.Empty;
            }
        }
        catch
        {
            return String.Empty;
        }
        
    }

    protected void cmdImportar_Click(object sender, EventArgs e)
    {
        if (cmbTiposUsuario.SelectedIndex == 0)
        {
            ventanita.RadAlert("No ha seleccionado el tipo de cliente", null, null, "efileCoach", "", Interfaz.ICO_alert);
        }
        else
        {
            if (Archivo.UploadedFiles.Count > 0)
            {
                int contador = 0;
                Boolean errores_columnas = false;
                Boolean errores_email = false;
                UsuariosFinales usuario = new UsuariosFinales();
                String Extension = Path.GetExtension(Archivo.UploadedFiles[0].FileName);
                StreamReader fichero = new StreamReader(Archivo.UploadedFiles[0].InputStream);
                String linea;
                Boolean primera_linea = true;
                Hashtable htCabeceras = new Hashtable();
                String nombre, apellidos, email, password, otra_empresa, nif, direccion,
                    cod_postal, localidad, provincia, telefono, movil;

                while ((linea = fichero.ReadLine()) != null)
                {
                    if (primera_linea)
                    {
                        //Vamos a buscar las cabeceras
                        String[] cabeceras = linea.ToLower().Trim().Split(';');
                        for (int i=0;i<cabeceras.Length;i++)
                        {
                            htCabeceras.Add(cabeceras[i].Trim(), i);
                        }
                        primera_linea = false;
                    }
                    else
                    {
                        String[] columnas = linea.Trim().Split(';');

                        //La cabecera y la línea deben tener el mismo número de elementos
                        if (columnas.Length==htCabeceras.Count)
                        {
                            nombre = this.GetCampoImportar(htCabeceras, "nombre", columnas);
                            apellidos = this.GetCampoImportar(htCabeceras, "apellidos", columnas);
                            email = this.GetCampoImportar(htCabeceras, "email", columnas);
                            password = this.GetCampoImportar(htCabeceras, "password", columnas);
                            otra_empresa = this.GetCampoImportar(htCabeceras, "otra_empresa", columnas);
                            nif = this.GetCampoImportar(htCabeceras, "nif", columnas);
                            direccion = this.GetCampoImportar(htCabeceras, "direccion", columnas);
                            cod_postal = this.GetCampoImportar(htCabeceras, "cod_postal", columnas);
                            localidad = this.GetCampoImportar(htCabeceras, "localidad", columnas);
                            provincia = this.GetCampoImportar(htCabeceras, "provincia", columnas);
                            telefono = this.GetCampoImportar(htCabeceras, "telefono", columnas);
                            movil = this.GetCampoImportar(htCabeceras, "movil", columnas);
                            if (columnas.Length > 0)
                            {
                                if (Interfaz.EsEmail(email))
                                {
                                    if (!UsuariosFinales.ExisteEmail(email, usuarioLogueado.GetIDCuenta()))
                                    {
                                        if (usuario.Insertar(usuarioLogueado.GetIDCuenta(),
                                            Int64.Parse(cmbTiposUsuario.SelectedValue),
                                            email, Interfaz.GetMD5Hash(password), nombre,
                                            apellidos, otra_empresa, nif, direccion, cod_postal,
                                            localidad, provincia, telefono, movil, null, String.Empty, DateTime.Now) != -1)
                                        {
                                            contador++;
                                        }
                                    }
                                    else
                                    {
                                        errores_email = true;
                                    }
                                }
                                else
                                {
                                    errores_email = true;
                                }

                            }
                            else
                            {
                                errores_columnas = true;
                            }
                        }
                        else
                        {
                            errores_columnas = true;
                        }
                    }
                }
                fichero.Close();
                grdUsuarios.DataBind();
                cmbTiposUsuario.SelectedIndex = 0;
                if (errores_email)
                {
                    ventanita.RadAlert("Alguna dirección de email no tenía el formato correcto, o ya existia un usuario para ese correo",
                        null, null, "efileCoach", "", Interfaz.ICO_alert);
                }

                if (errores_columnas)
                {
                    ventanita.RadAlert("Alguna línea del fichero no contiene información correcta",
                        null, null, "efileCoach", "", Interfaz.ICO_alert);

                }

                ventanita.RadAlert(contador.ToString() + " clientes importados con éxito", null, null, "efileCoach", "", Interfaz.ICO_ok);
            }
            else
            {
                ventanita.RadAlert("No ha seleccionado ningún fichero", null, null, "efileCoach", "", Interfaz.ICO_alert);
            }
        }
    }

    protected void cmbTiposUsuario_DataBound(object sender, EventArgs e)
    {
        cmbTiposUsuario.Items.Insert(0, "Seleccionar tipo");
    }
}
