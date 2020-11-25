<%@ Page Title="Procesos de coaching" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" 
    CodeFile="procesos.aspx.cs" Inherits="procesos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <telerik:RadAjaxPanel ID="panelContenido" runat="server" LoadingPanelID="radPanelCarga">
        <div class="row">
            <div class="col-sm-12">
                <div class="card bg-light">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-8">
                                <h2>Procesos de coaching</h2>
                            </div>
                            <div class="col-md-4 text-right">
                                <asp:Panel runat="server" ID="panelBuscador" class="input-group mb-3"
                                    DefaultButton="cmdBuscadorFormaciones">
                                    <asp:TextBox ID="txtBuscador" runat="server"
                                        placeholder="Buscador..." CssClass="form-control"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="cmdBuscadorFormaciones"
                                            CssClass="btn btn-outline-secondary"
                                            runat="server"
                                            OnClick="cmdBuscadorFormaciones_Click"><i class="fas fa-search"></i></asp:LinkButton>
                                    </div>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="cmdResetBusqueda" Visible="false"
                                            CssClass="btn btn-outline-secondary"
                                            ToolTip="Eliminar búsqueda"
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
                                        <telerik:RadPanelItem Expanded="false" Text="Nuevo proceso de coaching">
                                            <ContentTemplate>
                                                <div class="card">
                                                    <div class="card-header text-center">
                                                        <p>Rellene los siguientes campos para crear nuevos procesos de coaching:</p>
                                                    </div>
                                                    <asp:Panel runat="server" ID="panelCuerpo" CssClass="card-body"
                                                        DefaultButton="cmdNuevoProceso">
                                                        <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                                            <div class="col-sm-5">
                                                                <asp:Label ID="lblTitulo" runat="server" Text="Título del proceso"
                                                                    AssociatedControlID="txtTitulo"></asp:Label>
                                                                <asp:TextBox ID="txtTitulo" CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <asp:Label ID="lblCoordinadores" runat="server" Text="Coordinador del proceso"
                                                                    AssociatedControlID="cmbCoordinadores"></asp:Label>
                                                                <telerik:RadComboBox ID="cmbCoordinadores" Width="100%" runat="server"
                                                                    Culture="es-ES" DataSourceID="odsCoaches" Filter="Contains"
                                                                    DataTextField="formador" EmptyMessage="Seleccionar coordinador"
                                                                    HighlightTemplatedItems="true"
                                                                    DataValueField="id" MarkFirstMatch="True">
                                                                </telerik:RadComboBox>
                                                                <asp:ObjectDataSource ID="odsCoaches" runat="server"
                                                                    OldValuesParameterFormatString="original_{0}"
                                                                    SelectMethod="GetData"
                                                                    TypeName="Controlador.dsUsuariosTableAdapters.vista_usuarios_coachesTableAdapter">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter SessionField="idcuenta" Name="idcuenta"
                                                                            Type="Int64"></asp:SessionParameter>
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </div>
                                                            <div class="col-sm-3" style="padding-top: 21px">
                                                                <asp:LinkButton Style="margin-top: .5rem;" ID="cmdNuevoProceso"
                                                                    CssClass="btn btn-primary btn-block" runat="server"
                                                                    OnClick="cmdNuevoProceso_Click">
                                                                    <i class="fas fa-user-plus"></i> 
                                                                    &nbsp;Nuevo proceso
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>

                                                        <asp:Panel ID="panelErroresExterior" runat="server" CssClass="row">
                                                            <div class="col-md-12">
                                                                <asp:Panel ID="panelErrores" runat="server">
                                                                    <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>
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
                                <asp:GridView CssClass="table table-bordered" ID="grdProcesos"
                                    runat="server"
                                    AllowPaging="True" AutoGenerateColumns="False"
                                    PageSize="5"
                                    GridLines="None" DataKeyNames="id" DataSourceID="odsProcesosCuenta"
                                    OnDataBound="grdProcesos_DataBound"
                                    OnRowDataBound="grdProcesos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" InsertVisible="False"
                                            SortExpression="id">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfID" Value='<%# Bind("id") %>' runat="server" />
                                                <asp:HyperLink ID="hlDetalles" runat="server" CssClass="btn btn-outline-primary btn-block btn-sm"
                                                    NavigateUrl='<%# Eval("id") %>'><i class="fas fa-info-circle"></i> Detalles</asp:HyperLink>
                                            </ItemTemplate>
                                            <ItemStyle Width="150px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="titulo" HeaderText="Título" SortExpression="titulo" />
                                        <asp:TemplateField HeaderText="Coordinador" SortExpression="nombre">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("nombre") + " " + Bind("apellidos") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") + " " + Bind("apellidos") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="fecha_inicio" HeaderText="Fecha de inicio" SortExpression="fecha_inicio" />

                                    </Columns>
                                    <PagerStyle CssClass="numeros_de_pagina" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsProcesosCuenta" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                    TypeName="Controlador.dsFormacionesTableAdapters.vista_formacionesTableAdapter">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="idcuenta" Name="idcuenta"
                                            Type="Int64"></asp:SessionParameter>
                                        <asp:Parameter DefaultValue="1" Name="escoaching" Type="Int16"></asp:Parameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsProcesosUsuario" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDUsuario"
                                    TypeName="Controlador.dsFormacionesTableAdapters.vista_formacionesTableAdapter">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="idusuario" Name="idformador" Type="Int64"></asp:SessionParameter>
                                        <asp:Parameter DefaultValue="1" Name="escoaching" Type="Int16"></asp:Parameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:HiddenField ID="hfIDCliente" runat="server" />
                                <asp:ObjectDataSource ID="odsProcesosCliente" runat="server" OldValuesParameterFormatString="original_{0}" 
                                    SelectMethod="GetDataByIDCliente" TypeName="Controlador.dsFormacionesTableAdapters.vista_formacionesTableAdapter">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="1" Name="escoaching" Type="Int16"></asp:Parameter>
                                        <asp:ControlParameter ControlID="hfIDCliente" PropertyName="Value" DefaultValue="" Name="idcliente" 
                                            Type="Int32"></asp:ControlParameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
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
                    <div class="card-footer">
                        <div class="row">
                            <div class="col-sm-3" style="padding-top: 24px">
                                <asp:HyperLink ID="hlVolver" runat="server"
                                    CssClass="btn btn-outline-primary btn-block"
                                    NavigateUrl="/inicio.aspx">
                                    <i class="fas fa-arrow-left"></i> Volver
                                </asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="radPanelCarga" runat="server"
        Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

