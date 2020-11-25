<%@ Page Title="Cliente" Language="C#" MasterPageFile="~/MasterPageAuth.master"
     AutoEventWireup="true" CodeFile="cliente.aspx.cs" Inherits="cliente" %>
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
            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <div class="container-fluid p-3">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Cliente</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblEmail" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
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
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-4">
                                            <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="txtTelefono"></asp:Label>
                                            <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label ID="lblMovil" runat="server" Text="Movil" AssociatedControlID="txtMovil"></asp:Label>
                                            <asp:TextBox ID="txtMovil" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label ID="lblTipoUsuario" runat="server" Text="Tipo de usuario" AssociatedControlID="cmbTiposUSuario"></asp:Label>
                                            <asp:DropDownList ID="cmbTiposUsuario" runat="server" CssClass="form-control" DataSourceID="odsTiposUsuario" DataTextField="tipo" DataValueField="id"></asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsTiposUsuario" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="Controlador.dsIUUsuariosFinalesTiposTableAdapters.iu_usuarios_finales_tiposTableAdapter"></asp:ObjectDataSource>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
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
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblOtraEmpresa" runat="server" Text="Empresa" AssociatedControlID="txtOtraEmpresa"></asp:Label>
                                            <asp:TextBox ID="txtOtraEmpresa" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblFechaNacimiento" runat="server"
                                                 Text="Fecha de Nacimiento" AssociatedControlID="dtFechaNac"></asp:Label><br />
                                            <telerik:RadDatePicker ID="dtFechaNac" runat="server"
                                                Width="100%"></telerik:RadDatePicker>
                                        </div>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones" AssociatedControlID="txtObservaciones"></asp:Label>
                                            <asp:TextBox ID="txtObservaciones" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <asp:Panel CssClass="card-footer" ID="pnlErrores" runat="server" Visible="False">
                                    <div class="row">
                                        <div class="col-sm-12 iconos">
                                            <asp:Panel ID="panelErroresInterior" runat="server">
                                                <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2" style="padding-top: 24px">
                            <asp:HyperLink ID="hlVolver" runat="server"
                                CssClass="btn btn-outline-primary btn-block"
                                NavigateUrl="/clientes.aspx">
                                                <i class="fas fa-arrow-left"></i> Volver
                            </asp:HyperLink>
                        </div>
                        <div class="offset-sm-6 col-sm-2" style="padding-top: 24px">
                            <asp:Button ID="cmdEliminar" runat="server" CssClass="btn btn-danger btn-block"
                                OnClick="cmdEliminar_Click"
                                OnClientClick="confirmar(this,'¿Está seguro de eliminar el cliente?'); return false;"
                                Text="Eliminar" />
                        </div>
                        <div class="col-sm-2" style="padding-top: 24px">
                            <asp:LinkButton ID="cmdGuardar" CssClass="btn btn-primary btn-block" runat="server"
                                    OnClick="cmdGuardar_Click">
                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                                </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </asp:Panel>

</asp:Content>

