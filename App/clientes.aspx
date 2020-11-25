<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true"
    CodeFile="clientes.aspx.cs" Inherits="clientes" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
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
    <telerik:RadWindowManager ID="ventanita" runat="server"></telerik:RadWindowManager>
    <asp:Panel ID="pnlContenido" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <div class="card bg-light" style="width: 100%">
                    <div class="card-header ">
                        <div class="row">
                            <div class="col-md-8">
                                <h2>Clientes</h2>
                            </div>
                            <div class="col-md-4 text-right">
                                <asp:Panel runat="server" ID="panelBuscador" class="input-group mb-3"
                                    DefaultButton="cmdBuscador">
                                    <asp:TextBox ID="txtBuscador" runat="server"
                                        placeholder="Buscador..." CssClass="form-control"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="cmdBuscador"
                                            CssClass="btn btn-outline-secondary"
                                            OnClick="cmdBuscador_Click"
                                            runat="server"><i class="fas fa-search"></i></asp:LinkButton>
                                    </div>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="cmdResetBusqueda" Visible="false"
                                            CssClass="btn btn-outline-secondary"
                                            ToolTip="Eliminar búsqueda"
                                            OnClick="cmdResetBusqueda_Click"
                                            runat="server"><i class="fas fa-times"></i></asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-12 p-3">
                                <telerik:RadPanelBar ID="panelNuevo" runat="server" Width="100%" Skin="Bootstrap">
                                    <Items>
                                        <telerik:RadPanelItem Expanded="false" Text="Nuevo cliente">
                                            <ContentTemplate>
                                                <div class="card">
                                                    <div class="card-header">
                                                        <p>Rellene los siguientes campos para crear nuevos clientes:</p>
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

                                                            <div class="col-sm-3">
                                                                <asp:Label ID="lblApellidos" runat="server" Text="Apellidos" AssociatedControlID="txtApellidos"></asp:Label>
                                                                <asp:TextBox ID="txtApellidos" CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <asp:Label ID="lblTipoUsuario" runat="server" Text="Tipo de cliente" AssociatedControlID="cmbTiposUSuario"></asp:Label>
                                                                <asp:DropDownList ID="cmbTiposUsuario" runat="server" CssClass="form-control"
                                                                    DataSourceID="odsTiposUsuario" DataTextField="tipo"
                                                                    DataValueField="id"
                                                                    OnDataBound="cmbTiposUsuario_DataBound">
                                                                </asp:DropDownList>
                                                                <asp:ObjectDataSource ID="odsTiposUsuario" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="Controlador.dsIUUsuariosFinalesTiposTableAdapters.iu_usuarios_finales_tiposTableAdapter"></asp:ObjectDataSource>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-3">
                                                            <div class="offset-md-9 col-md-3">
                                                                <asp:LinkButton Style="margin-top: .5rem;" ID="cmdNuevaCuenta"
                                                                    CssClass="btn btn-primary btn-block" runat="server"
                                                                    OnClick="cmdNuevaCuenta_Click">
                                                                    <i class="fas fa-user-plus"></i>&nbsp;Nuevo cliente
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-3">
                                                            <div class="col-md-12">
                                                                <div class="card border-info">
                                                                    <div class="card-header">
                                                                        <h5>Importar usuarios</h5>
                                                                        <p>
                                                                            Mediante el siguiente formulario puede subir 
                                                                            una lista de clientes al sistema.
                                                                        </p>
                                                                    </div>
                                                                    <div class="card-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <telerik:RadAsyncUpload ID="Archivo" runat="server"
                                                                                    AllowedFileExtensions=".csv,.txt"
                                                                                    HideFileInput="true"
                                                                                    CssClass="async-attachment"
                                                                                    RenderMode="Lightweight" MultipleFileSelection="Disabled">
                                                                                    <Localization Select="Seleccionar fichero" DropZone="Arrastra y suelta el fichero a subir"
                                                                                        Remove="Eliminar" />
                                                                                </telerik:RadAsyncUpload>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <asp:Button ID="cmdImportar" runat="server" Text="Importar clientes"
                                                                                    CssClass="btn btn-block btn-outline-primary"
                                                                                    ToolTip="Formato fichero CSV: nombre;apellidos;email"
                                                                                    OnClick="cmdImportar_Click" />
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <asp:Literal ID="literalMensajeAyudaCSV" runat="server"></asp:Literal>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="panelErrores" runat="server">
                                                        <div class="row">
                                                            <div class="col-sm-12">
                                                                <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </ContentTemplate>
                                        </telerik:RadPanelItem>
                                    </Items>
                                </telerik:RadPanelBar>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView CssClass="table table-bordered" ID="grdUsuarios" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="odsUsuariosFinales"
                                    OnRowCommand="grdUsuarios_RowCommand" AllowSorting="false"
                                    OnPageIndexChanged="grdUsuarios_PageIndexChanged" PageSize="5" GridLines="None"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Accesos">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="cmdDetalles" runat="server"
                                                    CssClass="btn btn-outline-primary btn-block btn-sm"
                                                    CommandArgument='<%# Eval("id") %>'
                                                    CommandName="detalles"><i class="fas fa-info-circle"></i> Detalles</asp:LinkButton>
                                                <asp:LinkButton ID="cmdSesiones" runat="server"
                                                    CssClass="btn btn-outline-primary btn-block btn-sm"
                                                    CommandArgument='<%# Eval("id") %>'
                                                    CommandName="procesos_coaching"
                                                    Visible='<%# Convert.ToInt32(Eval("idtipo"))>=2 %>'>
                                                    <i class="fas fa-user-friends"></i> Procesos
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="cmdFormaciones" runat="server"
                                                    CssClass="btn btn-outline-primary btn-block btn-sm"
                                                    CommandArgument='<%# Eval("id") %>'
                                                    CommandName="formaciones"
                                                    Visible='<%# Convert.ToInt32(Eval("idtipo"))==0 || Convert.ToInt32(Eval("idtipo"))==3 %>'>
                                                        <i class="fas fa-user-graduate"></i> Formaciones
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Clientes" SortExpression="email">
                                            <ItemTemplate>
                                                <i class="fas fa-user-alt"></i>&nbsp;
                                                <asp:Literal ID="literalNombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Literal>
                                                <asp:Literal ID="literalApellidos" runat="server" Text='<%# Bind("apellidos") %>'></asp:Literal><br />
                                                <i class="fas fa-envelope"></i>&nbsp;
                                                <asp:Literal ID="literalEmail" runat="server" Text='<%# Bind("email") %>'></asp:Literal><br />
                                                <i class="fas fa-phone" title="Teléfono"></i>&nbsp;
                                                <asp:Literal ID="literalTelefono" runat="server" Text='<%# Bind("telefono") %>'></asp:Literal>
                                                &nbsp;<i class="fas fa-mobile-alt"></i>
                                                <asp:Literal ID="literalMovil" runat="server" Text='<%# Bind("movil") %>'></asp:Literal><br />
                                                <i class="fas fa-id-card-alt" title="Tipo de cuenta"></i>&nbsp;
                                                <asp:Literal ID="literalTipoDeCuenta" runat="server" Text='<%# Bind("tipo") %>'></asp:Literal><br />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="numeros_de_pagina" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsUsuariosFinales" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetDataByIDCuenta" TypeName="Controlador.dsUsuariosFinalesTableAdapters.usuarios_finales_grdTableAdapter">
                                    <SelectParameters>
                                        <asp:SessionParameter DefaultValue="1" Name="idcuenta" SessionField="idcuenta" Type="Int64" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <div class="col-2 text-right ml-3">
                                    <asp:DropDownList ID="cmbPaginas" CssClass="form-control" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="cmbPaginas_SelectedIndexChanged">
                                        <asp:ListItem Value="5">5 registros / página</asp:ListItem>
                                        <asp:ListItem Value="10">10 registros / página</asp:ListItem>
                                        <asp:ListItem Value="25">25 registros / página</asp:ListItem>
                                        <asp:ListItem Value="100">100 registros / página</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3" style="padding-top: 24px">
                <asp:HyperLink ID="hlVolver" runat="server"
                    CssClass="btn btn-outline-primary btn-block"
                    NavigateUrl="/inicio.aspx">
                                            <i class="fas fa-arrow-left"></i> Volver
                </asp:HyperLink>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

