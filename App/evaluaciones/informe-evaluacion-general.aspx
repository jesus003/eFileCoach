<%@ Page Language="C#" AutoEventWireup="true" CodeFile="informe-evaluacion-general.aspx.cs" Inherits="evaluaciones_informe_evaluacion_general" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>
<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
 
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <asp:HiddenField ID="hfIDAuto" runat="server" />
    <asp:HiddenField ID="hfIDEvaluacion" runat="server" />
    <div class="card">
        <div class="card-header bg-light">
            <h4>Gestión de la Encuesta</h4>
        </div>
        <div class="card-body">
            <rsweb:ReportViewer ID="rpGeneral" runat="server"></rsweb:ReportViewer>
           
           
        </div>
        <div class="card-footer">
            <div class="row">
                <div class="col-sm-3">
                    <asp:HyperLink ID="hlVolver" runat="server"
                        CssClass="btn btn-outline-primary btn-block">
                        <i class="fas fa-arrow-left"></i> Volver
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

