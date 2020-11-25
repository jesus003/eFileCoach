<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="alerta-evaluacion.aspx.cs" Inherits="alerta_evaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="container text-center">
            <div class="row" style="margin-top: 80px; margin-bottom: 30px;">
                <div class="offset-md-4 col-md-4">
                    <asp:Image ID="imgLogo" ImageUrl="~/Images/logo_efile_coach.jpg"
                        Width="300" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 p-4">
                    <asp:Panel runat="server" ID="pInfo" CssClass="" Style="text-align: center; height: 200px; padding-top: 95px;">
                        <asp:Label ID="lblInformacion" runat="server" Text=""></asp:Label><br />
                        <asp:HyperLink ID="hlInicio" runat="server" NavigateUrl="https://efilecoach.com">Volver al inicio</asp:HyperLink>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>

