<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="usuario.aspx.cs" Inherits="usuario" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <uc1:UsuarioLogueado runat="server" ID="usuarioLogueado" />
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
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab Text="Datos Personales" Selected="True"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="RadPageView1" runat="server"
                Selected="true" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel runat="server" ID="panelContenedor" class="container-fluid p-3"
                    DefaultButton="cmdGuardar">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Datos Personales</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblEmail" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
                                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblNombre" runat="server" Text="Nombre" AssociatedControlID="txtNombre"></asp:Label>
                                            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-3">
                                            <asp:Label ID="lblApellidos" runat="server" Text="Apellidos" AssociatedControlID="txtApellidos"></asp:Label>
                                            <asp:TextBox ID="txtApellidos" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-2">
                                            <asp:Label ID="lblNIF" runat="server" Text="NIF" AssociatedControlID="txtNIF"></asp:Label>
                                            <asp:TextBox ID="txtNIF" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-2 mt-3 pt-3">
                                            <telerik:RadCheckBox ID="chkActivo" runat="server" Text="Activo"
                                                AutoPostBack="False" ToolTip="Activa o desactiva la cuenta del usuario">
                                            </telerik:RadCheckBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="txtTelefono"></asp:Label>
                                            <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblMovil" runat="server" Text="Movil" AssociatedControlID="txtMovil"></asp:Label>
                                            <asp:TextBox ID="txtMovil" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"></asp:Label>
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password"
                                                    ReadOnly="true"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <asp:HiddenField ID="hfClave" runat="server" />
                                                    <asp:LinkButton ID="cmdEditarClave" CssClass="btn btn-outline-secondary"
                                                        runat="server" OnClick="cmdEditarClave_Click"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="col-sm-3">
                                            <asp:Label ID="lblTipoUsuario" runat="server" Text="Tipo de usuario" AssociatedControlID="cmbTiposUSuario"></asp:Label>
                                            <asp:DropDownList ID="cmbTiposUsuario" runat="server" CssClass="form-control" DataSourceID="odsTiposUsuario" DataTextField="tipo" DataValueField="id"></asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsTiposUsuario" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="Controlador.dsIUUsuariosTiposTableAdapters.iu_usuarios_tiposTableAdapter"></asp:ObjectDataSource>
                                        </div>
                                    </div>
                                    <div class="row">
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
                                <div class="card-footer">
                                    <asp:Panel CssClass="card" ID="panelErrores" runat="server" Visible="False">
                                        <div class="card-body">
                                            <div class="row iconos">
                                                <div class="col-sm-12">
                                                    <asp:Panel ID="panelErroresInterior" runat="server">
                                                        <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <asp:HyperLink ID="hlVolver" runat="server"
                                                CssClass="btn btn-outline-primary btn-block"
                                                NavigateUrl="/usuarios.aspx">
                                                <i class="fas fa-arrow-left"></i>&nbsp;&nbsp;Volver
                                            </asp:HyperLink>
                                        </div>
                                        <div class="offset-sm-6 col-sm-2">
                                            <asp:Button ID="cmdEliminar" runat="server" CssClass="btn btn-danger btn-block"
                                                OnClick="cmdEliminar_Click"
                                                OnClientClick="confirmar(this,'¿Desea eliminar el usuario?'); return false;"
                                                Text="Eliminar" />
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:LinkButton ID="cmdGuardar" CssClass="btn btn-primary btn-block" runat="server"
                                                OnClick="cmdGuardar_Click">
                                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                            </div>
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
