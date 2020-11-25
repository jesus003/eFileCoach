<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlContenido">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlContenido" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" 
        Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
    <asp:Panel ID="pnlContenido" runat="server">
        <div class="panel panel" style="margin-bottom:150px;">
            <div class="container-fluid text-center">
                <div class="row" style="margin-top: 80px; margin-bottom: 30px;">
                    <div class="offset-md-4 col-md-4">
                        <asp:Image ID="imgLogo" ImageUrl="~/Images/logo.png"
                             runat="server" />
                    </div>
                </div>
                <div class="row" style="margin-top: 30px; margin-bottom: 30px;">
                    <div class="col-sm-12 text-center">
                        <h2>Acceder a la aplicación</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="offset-md-4 col-md-4">
                        <div class="tab-content">
                            <asp:Panel ID="pnlLogin" role="tabpanel" CssClass="tab-pane active" runat="server" DefaultButton="cmdAcceder">
                                <div class="form-signin vertical-center">
                                    <asp:TextBox Style="margin-bottom: 15px" ID="txtUsuario" runat="server"
                                        CssClass="form-control text-center" placeholder="Usuario..."></asp:TextBox>
                                    <asp:TextBox Style="margin-bottom: 15px" ID="txtPassword" runat="server"
                                        CssClass="form-control text-center" TextMode="Password"
                                        placeholder="Contraseña..."></asp:TextBox>
                                    <asp:Button ID="cmdAcceder" runat="server" Text="ACCEDER"
                                        CssClass="btn btn-lg btn-primary btn-block" OnClick="cmdAcceder_Click" />
                                    <asp:Panel ID="panelFeedback" Visible="false" runat="server" style="margin-top:20px;">
                                        <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                        <asp:Literal ID="literalFeedback" runat="server"></asp:Literal>
                                    </asp:Panel>
                                    <asp:linkbutton ID="cmdRecuperarClave" runat="server" Text="Recupera tu contraseña"
                                        cssclass="btn btn-link btn-lg btn-block"
                                        onclick="cmdRecuperarClave_Click1">
                                    </asp:linkbutton>
                                    
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 text-center">
                        <a href="https://efilecoach.com/">efilecoach.com</a>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

