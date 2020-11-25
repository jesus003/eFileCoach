using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class alerta_evaluacion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["alerta"] != null)
        {
            String alerta = Request.QueryString["alerta"];
            String descifrado = Cifrado.descifrar(alerta,true);
            //La primera vez que envia el formulario
            switch (descifrado)
            {
                case "Correcto":
                    pInfo.CssClass = "alert alert-success";
                    lblInformacion.Text = "Evaluación completada correctamente.";

                    break;
                //Si el formulario esta cerrado
                case "Cerrado":
                    pInfo.CssClass = "alert alert-danger";
                    lblInformacion.Text = "La evaluación esta cerrada actualmente, lo sentimos pero no será posible que la complete.";

                    break;
                //Si ya ha respondido anteriormente
                case "Completo":
                    pInfo.CssClass = "alert alert-danger";
                    lblInformacion.Text = "Ya ha respondido anteriormente a esta evaluación.";
                    break;
                default:
                    Response.Redirect("/default.aspx");
                    break;
            }
        }
        else
        {
            Response.Redirect("/default.aspx");
        }
    }
}