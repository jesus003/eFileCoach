using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class procesos : System.Web.UI.Page
{
    CSesion csesion;
    protected void Page_Load(object sender, EventArgs e)
    {
        csesion = (CSesion)Session["csesion"];
        if (!IsPostBack)
        {
            if (Session["procesos_por_pagina"] != null)
            {
                cmbPaginas.SelectedValue = Session["procesos_por_pagina"].ToString();
                grdProcesos.PageSize = Int32.Parse(cmbPaginas.SelectedValue);
            }

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
            grdProcesos.DataSourceID = "odsProcesosCliente";
            hlVolver.NavigateUrl = "clientes.aspx";
        }
        else
        {
            if (csesion.EsUsuario())
            {
                grdProcesos.DataSourceID = "odsProcesosUsuario";
            }
        }
        if (this.GetIDUsuario()!=null)
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
        if (Request.QueryString["iu"]==null)
        {
            return null;
        }
        else
        {
            return Int64.Parse(Cifrado.descifrar(Request.QueryString["iu"], true));
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
            return Int64.Parse(Cifrado.descifrar(Request.QueryString["ic"], true));
        }
    }

    protected void cmdNuevoProceso_Click(object sender, EventArgs e)
    {
        if (this.ValidarCampos())
        {
            Formaciones proceso = new Formaciones();
            Int64 idproceso = proceso.Insertar(true,this.csesion.GetCuenta().GetID(),
                Int64.Parse(cmbCoordinadores.SelectedValue), null, DateTime.Now, null,
                txtTitulo.Text, String.Empty,String.Empty);
            if (idproceso != -1)
            {
                CryptoAES c = new CryptoAES();
                Response.Redirect("proceso.aspx?id=" + c.cifrar(idproceso.ToString(),true));
            }
            else
            {
                panelErroresExterior.Visible = true;
                panelErrores.CssClass = "alert alert-danger";
                blErrores.Items.Add("Se ha producido un error al crear el proceso de coaching");
            }
        }
    }

    private Boolean ValidarCampos()
    {
        blErrores.Items.Clear();
        panelErroresExterior.Visible = false;
        panelErrores.CssClass = String.Empty;
        if (txtTitulo.Text.Trim()==String.Empty)
        {
            blErrores.Items.Add("Debe rellenar el título del proceso de coaching");
        }

        if (cmbCoordinadores.SelectedValue==String.Empty)
        {
            blErrores.Items.Add("Debe seleccionar el coordinador");
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

    protected void grdProcesos_DataBound(object sender, EventArgs e)
    {
        if (this.GetIDUsuario() != null)
        {
            //Aplicamos filtro por idformador
            odsProcesosCuenta.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
            odsProcesosUsuario.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
        }
        else
        {
            if (this.GetIDCliente() != null)
            {
                //Aplicamos filtro por que participe en la formación. Este es más dificil
            }
        }
    }

    protected void grdProcesos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex!=-1)
        {
            CryptoAES c = new CryptoAES();
            HyperLink hlDetalles = (HyperLink)e.Row.FindControl("hlDetalles");
            Int64 idproceso = Convert.ToInt64(hlDetalles.NavigateUrl);
            hlDetalles.NavigateUrl = "/proceso.aspx?id=" + c.cifrar(idproceso.ToString(), true);
        }
    }

    //private void Buscar()
    //{
    //    String filtro = txtBuscador.Text.Trim();
    //    if (filtro != String.Empty)
    //    {
    //        Session["buscar_clientes"] = filtro;
    //        odsUsuariosFinales.FilterExpression += " nombre LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " apellidos LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " cod_postal LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " email LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " nif LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " movil LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " localidad LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " provincia LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " tipo LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " telefono LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " observaciones LIKE '%" + filtro + "%' OR ";
    //        odsUsuariosFinales.FilterExpression += " otra_empresa LIKE '%" + filtro + "%' ";
    //        cmdResetBusqueda.Visible = true;
    //    }
    //    else
    //    {
    //        Session["buscar_clientes"] = null;
    //        odsUsuariosFinales.FilterExpression = String.Empty;
    //        cmdResetBusqueda.Visible = false;
    //    }
    //    grdUsuarios.DataBind();
    //}

    protected void cmdBuscadorFormaciones_Click(object sender, EventArgs e)
    {
        this.Buscar();
    }

    private void Buscar()
    {
        if (txtBuscador.Text.Trim() == String.Empty)
        {
            if (this.GetIDUsuario() != null)
            {
                //Aplicamos filtro por idformador
                odsProcesosCuenta.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
                odsProcesosUsuario.FilterExpression = "idformador=" + this.GetIDUsuario().ToString();
            }
            else
            {
                odsProcesosCuenta.FilterExpression = String.Empty;
                odsProcesosUsuario.FilterExpression = String.Empty;
            }
            odsProcesosCliente.FilterExpression = String.Empty;
        }
        else
        {
            if (this.GetIDUsuario() != null)
            {
                odsProcesosCuenta.FilterExpression = String.Format("idformador={0} AND (titulo LIKE '%{1}%' OR descripcion LIKE '%{1}%')",
                this.GetIDUsuario().ToString(), txtBuscador.Text.Trim());
                odsProcesosUsuario.FilterExpression = String.Format("idformador={0} AND (titulo LIKE '%{1}%' OR descripcion LIKE '%{1}%')",
                this.GetIDUsuario().ToString(), txtBuscador.Text.Trim());
            }
            else
            {
                odsProcesosCuenta.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
                odsProcesosUsuario.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                    txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
            }

            odsProcesosCliente.FilterExpression = String.Format("titulo LIKE '%{0}%' OR descripcion LIKE '%{1}%'",
                txtBuscador.Text.Trim(), txtBuscador.Text.Trim());
        }
        grdProcesos.DataBind();
    }

    protected void cmbPaginas_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Buscar();
        Session["procesos_por_pagina"] = cmbPaginas.SelectedValue;
        grdProcesos.PageSize = Int32.Parse(cmbPaginas.SelectedValue);

    }
}