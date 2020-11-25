<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" 
    CodeFile="informe-evaluacion-texto.aspx.cs" Inherits="evaluaciones_informe_evaluacion_texto" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>
<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <asp:ObjectDataSource ID="odsClientes" runat="server"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
        TypeName="Controlador.dsUsuariosFinalesTableAdapters.vista_usuarios_finales_para_evaluacionesTableAdapter">
        <SelectParameters>
            <asp:SessionParameter Name="idcuenta" SessionField="idcuenta" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <asp:HiddenField ID="hfIDAuto" runat="server" />
    <asp:HiddenField ID="hfIDEvaluacion" runat="server" />
    <div class="card">
        <div class="card-header bg-light">
            <h4>Gestión de la Encuesta</h4>
        </div>
        <div class="card-body">
            <rsweb:ReportViewer ID="rvInforme" runat="server" Width="100%" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="600px">
                <LocalReport ReportPath="Informe\resultadosEvaluacionTexto.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="odsDetallesPreguntas" Name="dsDetalles" />
                        <rsweb:ReportDataSource DataSourceId="odsObservaciones" Name="dsObservaciones" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="odsObservaciones" runat="server" OldValuesParameterFormatString="original_{0}" 
                SelectMethod="GetData" TypeName="dsResultadosDiagnosticoSinAutoevalTableAdapters.diagnosticos_personasTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfIDEvaluacion" PropertyName="Value"
                        Name="iddiagnostico" Type="Int64"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsDetallesPreguntas" runat="server"
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                TypeName="dsResultadosDiagnosticoTableAdapters.vista_dimensiones_resultados_textoTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfIDEvaluacion" Name="idevaluacion" PropertyName="Value" Type="Int64" />
                </SelectParameters>
            </asp:ObjectDataSource>
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

