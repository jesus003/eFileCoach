<%@ Page Title="Listado de encuestas" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="evaluaciones_evaluaciones" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-md-8">
                    <h2>Listado de Encuestas</h2>
                </div>
                <div class="col-md-4 text-right">
                    <asp:Panel runat="server" ID="panelBuscador" class="input-group mb-3"
                        DefaultButton="cmdBuscador">
                        <asp:TextBox ID="txtBuscador" runat="server"
                            placeholder="Buscador..." CssClass="form-control"></asp:TextBox>
                        <div class="input-group-append">
                            <asp:LinkButton ID="cmdBuscador"
                                CssClass="btn btn-outline-secondary"
                                runat="server"
                                OnClick="cmdBuscar_Click"><i class="fas fa-search"></i></asp:LinkButton>
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
            
            <div class="row mt-2">
                <div class="col-sm-12">
                    <div class="alert alert-info">
                        <asp:Literal ID="lResultados" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="row mt-2">
                <div class="col-md-3">
                    <asp:Button ID="btnNuevaEvaluacion" CssClass="btn btn-primary btn-block" runat="server"
                        Text="Nueva Encuesta" OnClick="btnNuevaEvaluacion_Click" />
                </div>
            </div>
            <div class="row mt-2">
                <div class="col-sm-12">
                    <asp:GridView ID="grdResultados" runat="server"
                        CssClass="table table-hover" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False"
                        GridLines="None" DataKeyNames="id" DataSourceID="odsResultados"
                        OnRowDataBound="grdResultados_RowDataBound"
                        OnPageIndexChanged="grdResultados_PageIndexChanged"
                        OnSorting="grdResultados_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Opciones" InsertVisible="False" SortExpression="">
                                <ItemTemplate>

                                    <asp:HyperLink ID="hlDetalles" runat="server"
                                        CssClass="btn btn-outline-primary btn-block btn-sm" NavigateUrl='<%# "evaluacion.aspx?id="
                                            + Controlador.Cifrado.cifrarParaUrl(Eval("id").ToString()) %>'>
                                        <i class="fas fa-info-circle"></i> Detalles
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="170px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre" InsertVisible="False" SortExpression="nombre">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Literal ID="lNombre" runat="server" Text='<%# Eval("nombre") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfEstado" runat="server" Value='<%# Eval("idestado") %>' />
                                    <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" InsertVisible="False" SortExpression="fecha">
                                <ItemTemplate>
                                    <asp:Literal ID="lFecha" runat="server" Text='<%# Eval("fecha") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle Width="200px" />
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsResultados" runat="server"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDUsuario"
                        OnSelected="odsResultados_Selected"
                        TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticosTableAdapter">
                        <SelectParameters>
                            <asp:SessionParameter Name="idusuario" SessionField="idusuario" Type="Int64" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
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
</asp:Content>
