<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="mi-cuenta.aspx.cs" Inherits="mi_cuenta" %>

<%@ Register Src="~/controles/menu.ascx" TagPrefix="uc1" TagName="menu" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="Server">
    <uc1:menu runat="server" ID="menu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="panelContenido">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panelContenido" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager ID="ventanita" runat="server">
        <Localization OK="Aceptar" No="Cancelar" Cancel="Cancelar" />
    </telerik:RadWindowManager>
    <asp:Panel ID="panelContenido" runat="server">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab Text="Mi perfil" Selected="True"></telerik:RadTab>
                <telerik:RadTab Text="Correo electrónico"></telerik:RadTab>
                <telerik:RadTab Text="Suscripción"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel ID="panelDatosPrincipales" CssClass="container-fluid p-3 iconos"
                    DefaultButton="cmdGuardar" runat="server">
                    <div class="row">
                        <div class="col-sm-9">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Mi perfil</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row mt-1">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblEmail" runat="server" Text="Email"
                                                AssociatedControlID="txtEmail"
                                                Enabled="false"></asp:Label>
                                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblNombre" runat="server" Text="Nombre" AssociatedControlID="txtNombre"></asp:Label>
                                            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-4">
                                            <asp:Label ID="lblApellidos" runat="server" Text="Apellidos" AssociatedControlID="txtApellidos"></asp:Label>
                                            <asp:TextBox ID="txtApellidos" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-2">
                                            <asp:Label ID="lblNIF" runat="server" Text="NIF" AssociatedControlID="txtNIF"></asp:Label>
                                            <asp:TextBox ID="txtNIF" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="txtTelefono"></asp:Label>
                                            <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblMovil" runat="server" Text="Movil" AssociatedControlID="txtMovil"></asp:Label>
                                            <asp:TextBox ID="txtMovil" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblClave1" runat="server" Text="Contraseña"
                                                AssociatedControlID="txtClave1"></asp:Label>
                                            <asp:TextBox ID="txtClave1" runat="server" CssClass="form-control"
                                                TextMode="Password"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblClave2" runat="server" Text="Repetir contraseña"
                                                AssociatedControlID="txtClave2"></asp:Label>
                                            <asp:TextBox ID="txtClave2" runat="server" CssClass="form-control"
                                                TextMode="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblDireccion" runat="server" Text="Direccion" AssociatedControlID="txtDireccion"></asp:Label>
                                            <asp:TextBox ID="txtDireccion" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblPoblacion" runat="server" Text="Poblacion" AssociatedControlID="txtPoblacion"></asp:Label>
                                            <asp:TextBox ID="txtPoblacion" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblProvincia" runat="server" Text="Provincia" AssociatedControlID="txtProvincia"></asp:Label>
                                            <asp:TextBox ID="txtProvincia" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-2">
                                            <asp:Label ID="lblCodPos" runat="server" Text="Código Postal" AssociatedControlID="txtCodPos"></asp:Label>
                                            <asp:TextBox ID="txtCodPos" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <asp:HyperLink ID="hlLogotipo" runat="server">
                                                <asp:Image ID="imgLogotipo" CssClass="img-fluid text-center" runat="server" />
                                            </asp:HyperLink>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblSeleccionarFichero" runat="server"
                                                        Text="Logotipo (Tamaño recomendado 307x95px)" AssociatedControlID="fuLogo"></asp:Label>
                                                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server"
                                                        CssClass="async-attachment foto_perfil"
                                                        ID="fuLogo" MultipleFileSelection="Disabled" MaxFileSize="3000000"
                                                        HideFileInput="true"
                                                        AllowedFileExtensions=".jpeg,.jpg,.png,">
                                                        <Localization Remove="Quitar logo" Select="Seleccionar logo" />
                                                    </telerik:RadAsyncUpload>
                                                </div>
                                                <div class="col-md-6 mt-2">
                                                    <asp:Button ID="cmdBorrarFoto" runat="server" Text="Borrar logo"
                                                        CssClass="btn btn-outline-danger btn-block" OnClick="cmdBorrarFoto_Click" />
                                                </div>
                                                <div class="col-md-6 mt-2">
                                                    <asp:Button ID="cmdGuardarFoto" runat="server" Text="Guardar logo"
                                                        CssClass="btn btn-outline-success btn-block" OnClick="cmdGuardarFoto_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="panelFeedBack" runat="server" CssClass="row mt-3" Visible="false">
                        <div class="col-md-12">
                            <asp:Panel ID="panelFeedBackInterno" runat="server">
                                <asp:BulletedList ID="blFeedBack" runat="server"></asp:BulletedList>
                                <asp:Literal ID="lFeedBack" runat="server"></asp:Literal>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-2" style="padding-top: 24px">
                            <asp:HyperLink ID="hlVolver" runat="server"
                                CssClass="btn btn-outline-primary btn-block"
                                NavigateUrl="/inicio.aspx">
                                <i class="fas fa-arrow-left"></i> Volver
                            </asp:HyperLink>
                        </div>
                        <div class="offset-sm-8 col-sm-2" style="padding-top: 24px">
                            <asp:LinkButton ID="cmdGuardar" CssClass="btn btn-primary btn-block" runat="server" OnClick="cmdGuardar_Click">
                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel ID="panelDatosSMTP" CssClass="container-fluid p-3 " DefaultButton="cmdGuardarSMTP" runat="server">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Configuración del correo electrónico</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row mt-3">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblFirma" runat="server" Text="Firma de todos los emails que el sistema envíe."></asp:Label>
                                            <telerik:RadEditor ID="editorFirma" runat="server" Skin="Silk" Width="100%"
                                                SkinID="DefaultSetOfTools" ImageManager-MaxUploadFileSize="10000"
                                                Language="es-ES">
                                                <Modules>
                                                    <telerik:EditorModule Enabled="False" Name="RadEditorDomInspector" />
                                                    <telerik:EditorModule Enabled="False" Name="RadEditorNodeInspector" />
                                                    <telerik:EditorModule Name="RadEditorStatistics" />
                                                </Modules>
                                                <Content>
                                                </Content>
                                                <TrackChangesSettings CanAcceptTrackChanges="False" />
                                            </telerik:RadEditor>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblSMTPFromTxt" runat="server" Text="Nombre del remitente"
                                                AssociatedControlID="txtSMTPFromTXT"></asp:Label>
                                            <asp:TextBox ID="txtSMTPFromTXT" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblSMTPServidor" runat="server" Text="Servidor smtp"
                                                AssociatedControlID="txtSMTPServidor"></asp:Label>
                                            <asp:TextBox ID="txtSMTPServidor" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-md-3">
                                            <asp:Label ID="lblSMTPEmailFrom" runat="server" Text="Correo electrónico"
                                                AssociatedControlID="txtSMTPEmailFrom"></asp:Label>
                                            <asp:TextBox ID="txtSMTPEmailFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblSMTPFromUsuario" runat="server" Text="Usuario"
                                                AssociatedControlID="txtSMTPFromUsuario"></asp:Label>
                                            <asp:TextBox ID="txtSMTPFromUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblSMTPClave" runat="server" Text="Contraseña"
                                                AssociatedControlID="txtSMTPClave"></asp:Label>
                                            <asp:TextBox ID="txtSMTPClave" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Label ID="lblSMTPPuerto" runat="server" Text="Puerto"
                                                AssociatedControlID="txtSMTPPuerto"></asp:Label>
                                            <telerik:RadNumericTextBox ID="txtSMTPPuerto" runat="server"
                                                NumberFormat-DecimalDigits="0" Width="100%">
                                            </telerik:RadNumericTextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblSMTPSSL" runat="server" Text="¿SSL?"
                                                AssociatedControlID="cmbSMTPSSL"></asp:Label>
                                            <asp:DropDownList ID="cmbSMTPSSL" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="SI (recomendado)"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="NO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row mt-1" style="display: none;">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblAPIMailchimp" runat="server" Text="API de Mailchimp" AssociatedControlID="txtAPIMailchimp"></asp:Label>
                                            <asp:TextBox ID="txtAPIMailchimp" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="panelFeedBackSMTP" runat="server" CssClass="row mt-3 iconos" Visible="false">
                        <div class="col-md-12">
                            <asp:Panel ID="panelFeedBackSMTPInterno" runat="server">
                                <asp:BulletedList ID="blFeedBackSMTP" runat="server"></asp:BulletedList>
                                <asp:Literal ID="lFeedbackSMTP" runat="server"></asp:Literal>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-2" style="padding-top: 24px">
                            <asp:HyperLink ID="hlVolver2" runat="server"
                                CssClass="btn btn-outline-primary btn-block"
                                NavigateUrl="/inicio.aspx">
                                    <i class="fas fa-arrow-left"></i> Volver
                            </asp:HyperLink>
                        </div>
                        <div class="offset-sm-8 col-sm-2" style="padding-top: 24px">
                            <asp:LinkButton ID="cmdGuardarSMTP" CssClass="btn btn-primary btn-block" runat="server" OnClick="cmdGuardarSMTP_Click">
                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView3" runat="server" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel ID="panelSuscripcion" CssClass="container-fluid p-3 iconos"
                    DefaultButton="cmdGuardarSuscripcion" runat="server">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Suscripción</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row mt-1">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblTipoCuenta" runat="server" Text="Tipo de cuenta"
                                                AssociatedControlID="txtTipoCuenta"></asp:Label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtTipoCuenta" runat="server" CssClass="form-control"
                                                    Enabled="false"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <asp:HyperLink ID="cmdMejorarCuenta" runat="server"
                                                        CssClass="btn btn-outline-secondary"
                                                        NavigateUrl="https://efilecoach.com/modificar-suscripcion/" Target="_blank"
                                                        ToolTip="Mejora en cualquier momento tu cuenta de efilecoach">
                                                    <i class="fas fa-plus-circle"></i>&nbsp;Modificar cuenta
                                                    </asp:HyperLink>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOtraEmpresa" runat="server" Text="Cuenta bancaria"
                                                AssociatedControlID="txtIBAN"></asp:Label>
                                            <asp:TextBox ID="txtIBAN" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="panelFeedbackSuscripcion" runat="server" CssClass="row mt-3" Visible="false">
                        <div class="col-md-12">
                            <asp:Panel ID="panelFeedbackSuscripcionInterior" runat="server">
                                <asp:BulletedList ID="blFeedbackSuscripcion" runat="server"></asp:BulletedList>
                                <asp:Literal ID="literalFeedbackSuscripcion" runat="server"></asp:Literal>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-2" style="padding-top: 24px">
                            <asp:HyperLink ID="HyperLink1" runat="server"
                                CssClass="btn btn-outline-primary btn-block"
                                NavigateUrl="/inicio.aspx">
                                    <i class="fas fa-arrow-left"></i> Volver
                            </asp:HyperLink>
                        </div>
                        <div class="offset-sm-8 col-sm-2" style="padding-top: 24px">
                            <asp:LinkButton ID="cmdGuardarSuscripcion" CssClass="btn btn-primary btn-block"
                                runat="server" OnClick="cmdGuardarSuscripcion_Click">
                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </asp:Panel>
    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server"
        RelativeTo="Element" Position="MiddleRight" AutoTooltipify="true"
        ContentScrolling="Default" RenderMode="Lightweight">
    </telerik:RadToolTipManager>
</asp:Content>

