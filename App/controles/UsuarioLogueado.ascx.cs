using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controles_UsuarioLogueado : System.Web.UI.UserControl
{
    Cuentas cuenta;
    Usuarios usuario;
    CSesion csesion;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.csesion = (CSesion)Session["csesion"];
        this.cuenta = csesion.GetCuenta();
        this.usuario = csesion.GetUsuario();
    }

    public Boolean EsCuenta()
    {
        if (this.csesion==null)
        {
            this.csesion = (CSesion)Session["csesion"];
            return true;
        }
        return this.csesion.EsCuenta();
    }

    public Boolean EsUsuario()
    {
        if (this.csesion == null)
        {
            this.csesion = (CSesion)Session["csesion"];
        }
        return this.csesion.EsUsuario();
    }

    public Cuentas GetCuenta()
    {
        this.csesion = (CSesion)Session["csesion"];
        this.cuenta = csesion.GetCuenta();
        return this.cuenta;
    }

    public Usuarios GetUsuario()
    {
        this.csesion = (CSesion)Session["csesion"];
        this.usuario = csesion.GetUsuario();
        return this.usuario;
    }

    public Int64 GetIDCuenta()
    {
        this.csesion = (CSesion)Session["csesion"];
        return (Int64)Session["idcuenta"];
    }
}