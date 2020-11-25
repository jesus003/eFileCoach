<%@ Page Title="Plantillas efileCoach" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="plantillas.aspx.cs" Inherits="evaluaciones_plantillas" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <telerik:RadAjaxLoadingPanel ID="panelCarga" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="panelPrincipal" runat="server" LoadingPanelID="panelCarga">

        <div class="card">
            <div class="card-header">
                
                <div class="row">
                    <div class="col-md-8">
                        <h4>Listado de Plantillas</h4>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:Panel runat="server" ID="panelBuscador" class="input-group mb-3"
                            DefaultButton="cmdBuscadorFormaciones">
                            <asp:TextBox ID="txtBuscador" runat="server"
                                placeholder="Buscador..." CssClass="form-control"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:LinkButton ID="cmdBuscadorFormaciones" OnClick="cmdBuscadorFormaciones_Click"
                                    CssClass="btn btn-outline-secondary"
                                    runat="server"><i class="fas fa-search"></i></asp:LinkButton>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <asp:Panel ID="panelResultados"
                    Visible="false" CssClass="row" runat="server">
                    <div class="col-sm-12">
                        <div class="alert alert-info">
                            <asp:Literal ID="lResultados" runat="server"></asp:Literal>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row mt-2">
                    <div class="col-md-3">
                        <asp:Button ID="btnNuevaPlantilla" CssClass="btn btn-primary btn-block"
                            runat="server" Text="Nueva Plantilla" OnClick="btnNuevaPlantilla_Click" />
                    </div>
                </div>
                <asp:GridView ID="grdResultados" runat="server" CssClass="table table-hover" AllowPaging="True"
                    AllowSorting="True" DataKeyNames="id" AutoGenerateColumns="False" GridLines="None"
                    DataSourceID="odsDatos" OnRowDataBound="grdResultados_RowDataBound"
                    OnPageIndexChanged="grdResultados_PageIndexChanged" OnSorting="grdResultados_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Opciones" InsertVisible="False" SortExpression="">
                                <ItemTemplate>

                                    <asp:HyperLink ID="hlDetalles" runat="server"
                                        CssClass="btn btn-outline-primary btn-block btn-sm" NavigateUrl='<%# "plantilla.aspx?id="
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
                                <asp:Literal ID="hlNombre" Text='<%# Eval("nombre") %>'
                                    runat="server"
                                    ></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" InsertVisible="False" SortExpression="fecha">
                            <ItemTemplate>
                                <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("fecha") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Disponible" InsertVisible="False" SortExpression="disponible">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDisponible" Enabled="false" runat="server" Checked='<%# Eval("disponible") == DBNull.Value ? false : Convert.ToBoolean(Eval("disponible")) %>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:ObjectDataSource ID="odsDatos" runat="server"
                    OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetDataByIDUsuario"
                    TypeName="Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters.diagnosticos_plantillasTableAdapter">
                    <SelectParameters>
                        <asp:SessionParameter Name="idusuario" SessionField="idusuario" Type="Int64" />
                    </SelectParameters>
                </asp:ObjectDataSource>

            </div>
            <div class="card-footer">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:HyperLink ID="hlVolver" runat="server"
                            CssClass="btn btn-outline-primary btn-block"
                            NavigateUrl="/inicio.aspx">
                        <i class="fas fa-arrow-left"></i> Volver
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
