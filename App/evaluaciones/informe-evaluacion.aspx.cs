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

public partial class evaluaciones_informe_evaluacion : System.Web.UI.Page
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
                    rvSinAutoevaluacion.Visible = false;
                    rvConAutoevaluacion.Visible = true;
                    rvConAutoevaluacion.LocalReport.Refresh();
                }
                else
                {
                    dsResultadosDiagnosticoSinAutoevalTableAdapters.media_resultado_diagnostico_sinautoevaluacionTableAdapter q =
                new dsResultadosDiagnosticoSinAutoevalTableAdapters.media_resultado_diagnostico_sinautoevaluacionTableAdapter();
                    dsResultadosDiagnosticoSinAutoeval.media_resultado_diagnostico_sinautoevaluacionDataTable tabla =
                        q.GetData(Int64.Parse(hfIDEvaluacion.Value));
                    ReportDataSource ds = new ReportDataSource("ds",
                           (System.Data.DataTable)tabla);
                    rvSinAutoevaluacion.LocalReport.DataSources.Add(ds);

                    rvSinAutoevaluacion.Visible = true;
                    rvConAutoevaluacion.Visible = false;
                    rvSinAutoevaluacion.LocalReport.Refresh();
                }
            }
            else
            {
                Response.Redirect("/default.aspx");
            }
        }
    }

    protected void cmbTipoGrafico_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.idevaluacion = Convert.ToInt64(Cifrado.descifrar(Request.QueryString["id"], true));
        this.evaluacion = new Diagnosticos(this.idevaluacion);

        if (cmbTipoGrafico.SelectedIndex == 0)
        {
            //Radar
            rvConAutoevaluacion.LocalReport.ReportPath =
                Server.MapPath("~/Informe/resultadosEvaluacion.rdlc");
            rvSinAutoevaluacion.LocalReport.ReportPath =
                Server.MapPath("~/Informe/resultadosEvaluacionSinAutoevaluacion.rdlc");
        }
        else
        {
            //Barras
            
            rvConAutoevaluacion.LocalReport.ReportPath =
                Server.MapPath("~/Informe/resultadosEvaluacionBarras.rdlc");
            rvSinAutoevaluacion.LocalReport.ReportPath =
                Server.MapPath("~/Informe/resultadosEvaluacionSinAutoevaluacionBarras.rdlc");
        }

        if (evaluacion.TieneAutoevaluacion())
        {
            rvSinAutoevaluacion.Visible = false;
            rvConAutoevaluacion.Visible = true;
            rvConAutoevaluacion.LocalReport.Refresh();
        }
        else
        {
            dsResultadosDiagnosticoSinAutoevalTableAdapters.media_resultado_diagnostico_sinautoevaluacionTableAdapter q =
                new dsResultadosDiagnosticoSinAutoevalTableAdapters.media_resultado_diagnostico_sinautoevaluacionTableAdapter();
            dsResultadosDiagnosticoSinAutoeval.media_resultado_diagnostico_sinautoevaluacionDataTable tabla =
                q.GetData(Int64.Parse(hfIDEvaluacion.Value));
            ReportDataSource ds = new ReportDataSource("ds",
                   (System.Data.DataTable)tabla);
            rvSinAutoevaluacion.LocalReport.DataSources.Add(ds);

            rvSinAutoevaluacion.Visible = true;
            rvConAutoevaluacion.Visible = false;
            rvSinAutoevaluacion.LocalReport.Refresh();
        }
    }

    protected void odsResultadosAutoevaluacion_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        System.Data.DataTable tabla = ((System.Data.DataTable)e.ReturnValue);
        if (tabla != null)
        {
            Int32 cuantos = Convert.ToInt32(tabla.Compute("COUNT(titulo)",
                odsClientes.FilterExpression).ToString());
            if (cuantos > 0)
            {
                //Hay resultados de autoevaluacion
                rvConAutoevaluacion.Visible = true;
                rvSinAutoevaluacion.Visible = false;
            }
            else
            {
                //No hay resultados de autoevaluacion
                rvConAutoevaluacion.Visible = false;
                rvSinAutoevaluacion.Visible = true;
            }
        }


    }
}