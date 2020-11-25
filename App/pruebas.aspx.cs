using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pruebas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void cmdPrueba_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        txtPruebas.Text = DateTime.Now.ToString();
    }
}