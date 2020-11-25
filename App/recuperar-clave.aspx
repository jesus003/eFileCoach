<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="recuperar-clave.aspx.cs" Inherits="recuperar_clave" %>

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
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="pnlContenido" runat="server">
        <div class="panel panel">
        <div
            class="container-fluid">
            <div class="row" style="margin-top: 30px; margin-bottom: 30px;">
                <div class="offset-md-4 col-md-4 text-center">
                    <asp:Image ID="imgLogo" ImageUrl="~/Images/logo_efile_coach.jpg"
                        Width="300" runat="server" />
                </div>
            </div>
            <div class="row" style="margin-top: 30px; margin-bottom: 30px;">
                <div class="col-sm-12 text-center">
                    <h2>Restablecimiento de Contraseña</h2>
                    <h4>Estimado Usuario, en el siguiente formulario va a poder restablecer una nueva contraseña para su cuenta</h4>
                </div>
            </div>
            <div class="tab-content">
                <asp:Panel ID="pnlRestablecer" role="tabpanel" CssClass="tab-pane active" runat="server" DefaultButton="cmdConfirmar">
                    <div class="form-signin vertical-center">
                        <div class="row">
                            <div class="offset-md-2 col-md-4">
                                <asp:TextBox Style="margin-bottom: 15px" ID="txtPassword" runat="server"
                                    CssClass="form-control text-center" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox Style="margin-bottom: 15px" ID="txtPasswordRepeat" runat="server"
                                    CssClass="form-control text-center" placeholder="Repita la contraseña" 
                                    TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-md-2 col-md-4">
                                
                                <asp:HyperLink ID="hlIniciarSesion" runat="server" 
                                    CssClass="btn btn-outline-primary btn-block btn-lg"
                                    NavigateUrl="~/Default.aspx">
                                    <i class="fas fa-home"></i>&nbsp;Inicio
                                </asp:HyperLink>
                            </div>
                            <div class="col-md-4">
                                <asp:LinkButton ID="cmdConfirmar" runat="server"
                                    CssClass="btn btn-lg btn-primary btn-block" OnClick="cmdConfirmar_Click">
                                    <i class="fas fa-key"></i>&nbsp;Restablecer
                                </asp:LinkButton>
                            </div>
                            <div class="offset-md-2 col-md-8 iconos">
                                <asp:Panel ID="panelFeedback" Visible="false" runat="server" style="margin-top:20px;">
                                    <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                    <asp:Literal ID="literalFeedback" runat="server"></asp:Literal>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    </asp:Panel>
</asp:Content>

