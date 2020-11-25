using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class logo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Int64 idcuenta = Int64.Parse(Cifrado.descifrar(Request.QueryString[0], true));
            Cuentas cuenta = new Cuentas(idcuenta);
            Response.ContentType = "image/jpg";
            Response.BinaryWrite(cuenta.GetLogotipo());
        }
        catch
        { }
    }
}