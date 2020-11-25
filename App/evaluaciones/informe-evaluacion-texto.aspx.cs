using Controlador.Diagnosticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador.DiagnosticosPlantillas;
using Controlador;
using Microsoft.Reporting.WebForms;

public partial class evaluaciones_informe_evaluacion_texto : System.Web.UI.Page
{
    Int64 idevaluacion = -1;
    Int64 idusuario;
    Diagnosticos evaluacion;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.idusuario = Convert.ToInt64(Session["idusuario"].ToString());
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                this.idevaluacion = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"], true));
                hfIDEvaluacion.Value = this.idevaluacion.ToString();
                this.evaluacion = new Diagnosticos(this.idevaluacion);

                hlVolver.NavigateUrl = "evaluacion.aspx?id=" + Cifrado.cifrarParaUrl(idevaluacion.ToString());

                if (evaluacion.GetIDCliente() != -1)
                {
                    Personas persona = new Personas(idevaluacion, evaluacion.GetIDCliente());
                    //cmbClientes.SelectedValue = evaluacion.GetIDCliente().ToString();
                    hfIDAuto.Value = persona.GetIdPersona().ToString();
                }
                else
                {
                    hfIDAuto.Value = "0";
                }

                if (evaluacion.TieneAutoevaluacion())
                {
                    //Puede ser que haya respuestas pero no la autoevaluacion. En este caso mostramos el informe de Sin Autoevaluacion
                    rvInforme.Visible = true;
                    rvInforme.LocalReport.Refresh();
                }
                else
                {
                    
                }
            }
            else
            {
                Response.Redirect("/default.aspx");
            }
        }
    }

}