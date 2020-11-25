<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="plantilla.aspx.cs" Inherits="evaluaciones_plantilla" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>

<asp:Content ID="ContentCabecera" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <telerik:RadAjaxLoadingPanel ID="panelCarga" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="panelPrincipal" runat="server" LoadingPanelID="panelCarga">
        <asp:HiddenField ID="hfIDPlantilla" runat="server" />
        <div class="card">
            <div class="card-header">
                <h4>Gestión de Plantilla</h4>
            </div>
            <div class="card-body">
                
                <asp:Panel runat="server" ID="panelGuardado" CssClass="row" Visible="false">
                    <div class="col-sm-12">
                        <div class="alert alert-success">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                            <span class="sr-only">Guardado:</span>
                            Guardado con éxito.
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="Nombre" />
                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblTipoEncuesta" runat="server" AssociatedControlID="cmbTipoEncuesta"
                            Text="Tipo de encuesta" />
                        <asp:DropDownList ID="cmbTipoEncuesta" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Seleccionar tipo..."></asp:ListItem>
                            <asp:ListItem Text="Numérica (de 0 - 10 con cálculo de medias)" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Libre (sin cálculo de medias)" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfIDTipo" runat="server" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="lblFecha" AssociatedControlID="dtFecha" runat="server" Text="Fecha" />
                        <telerik:RadDatePicker ID="dtFecha" runat="server" Width="100%"></telerik:RadDatePicker>
                    </div>
                    <div class="col-sm-2" style="padding-top: 20px; padding-left: 40px; margin-top: 16px;">
                        <asp:CheckBox ID="chkDisponible" CssClass="checkbox" Checked="true" Text="Disponible" runat="server" />
                    </div>
                    <div class="col-sm-2" style="padding-top: 20px; padding-left: 40px; margin-top: 16px;">
                        <asp:CheckBox ID="chkPublico" CssClass="checkbox" Text="Público" runat="server" />
                    </div>
                </div>
                <asp:Panel ID="pnlMensajesPlantillas" runat="server" CssClass="row"
                    Visible="false">
                    <div class="col-sm-12">
                        <div class="alert alert-danger">
                            <asp:Label ID="lblPlantillasAlertas" runat="server" Text=""></asp:Label>
                            <asp:BulletedList ID="bllListPlantillas" runat="server">
                            </asp:BulletedList>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlDimensiones" runat="server" Visible="false" DefaultButton="cmdAnadirDimension">

                    <div class="card">
                        <div class="card-header">Dimensiones a evaluar</div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="alert alert-info">
                                        <asp:Literal ID="lResultados" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <asp:GridView ID="grdResultados" runat="server" CssClass="table table-hover tablaDimensiones" AutoGenerateColumns="False" GridLines="None" DataKeyNames="id"
                                        DataSourceID="odsResultados" OnRowDataBound="grdResultados_RowDataBound"
                                        OnPageIndexChanged="grdResultados_PageIndexChanged"
                                        OnSorting="grdResultados_Sorting" OnRowCommand="grdResultados_RowCommand" OnDataBound="grdResultados_DataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="btn-group">
                                                        <asp:HyperLink ID="hlTitulo" CssClass="btn btn-outline-primary"
                                                            runat="server" Text='Detalles'
                                                            NavigateUrl='<%# "pdimension.aspx?id=" + Controlador.Cifrado.cifrarParaUrl(Eval("id").ToString()) %>'></asp:HyperLink>
                                                        <asp:Button ID="cmdEliminar" runat="server"
                                                            CommandArgument='<%# Eval("id") %>'
                                                            CommandName="eliminar" CssClass="btn btn-outline-danger"
                                                            OnClientClick="confirmarb(this,'¿Está seguro de eliminar la dimensión?'); return false;"
                                                            Text="Eliminar" />
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Título" InsertVisible="False" SortExpression="titulo">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtTitulo" CssClass="form-control" runat="server" Text='<%# Eval("titulo") %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Bind("id") %>' />
                                                    <asp:HiddenField ID="hfOrden" runat="server" Value='<%# Eval("orden") %>' />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Literal ID="literalTitulo" 
                                                        runat="server" Text='<%# Eval("titulo") %>'></asp:Literal>
                                                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Bind("id") %>' />
                                                    <asp:HiddenField ID="hfOrden" runat="server" Value='<%# Eval("orden") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Orden">
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="cmdSubir" runat="server"
                                                        CommandName="Subir" ToolTip="Cancelar"
                                                        CssClass="btn btn-default">
                                                                <i class="fas fa-arrow-circle-up"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="cmdBajar" runat="server"
                                                        CommandName="Bajar" ToolTip="Cancelar"
                                                        CssClass="btn btn-default">
                                                                <i class="fas fa-arrow-circle-down"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsResultados" runat="server"
                                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIdPlantilla"
                                        OnSelected="odsResultados_Selected" TypeName="Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters.diagnosticos_plantillas_dimensionesTableAdapter" OnSelecting="odsResultados_Selecting" InsertMethod="Insert" UpdateMethod="Update">
                                        <InsertParameters>
                                            <asp:Parameter Name="p1" Type="Int64" />
                                            <asp:Parameter Name="p2" Type="String" />
                                            <asp:Parameter Name="p3" Type="Int32" />
                                        </InsertParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hfIDPlantilla" DefaultValue="0" Name="idPlantilla" PropertyName="Value" Type="Int64" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="p1" Type="Int64" />
                                            <asp:Parameter Name="p2" Type="String" />
                                            <asp:Parameter Name="p3" Type="Int32" />
                                            <asp:Parameter Name="p4" Type="Int64" />
                                        </UpdateParameters>
                                    </asp:ObjectDataSource>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <h2>Añade dimensiones a tu plantilla</h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-9">
                                    <asp:Label ID="lblTitulo" runat="server" AssociatedControlID="txtTitulo" Text="Título">
                                    </asp:Label><asp:TextBox ID="txtTitulo" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-3" style="margin-top: 28px;">
                                    <asp:Button ID="cmdAnadirDimension" CssClass="btn btn-outline-primary btn-block" runat="server"
                                        Text="Añadir Dimensión" OnClick="cmdAnadirDimension_Click" />
                                </div>
                            </div>
                            <br />
                            <asp:Panel ID="pnlMensajes" runat="server" CssClass="alert alert-danger" Visible="false">
                                <asp:Label ID="lblAlertas" runat="server" Text=""></asp:Label>
                                <asp:BulletedList ID="bllList" runat="server">
                                </asp:BulletedList>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="card-footer">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:HyperLink ID="hlVolver" runat="server"
                            CssClass="btn btn-outline-primary btn-block"
                            NavigateUrl="plantillas.aspx">
                        <i class="fas fa-arrow-left"></i> Volver
                        </asp:HyperLink>
                    </div>
                    <div class="offset-sm-5 col-sm-2">
                        <asp:Button ID="cmdEliminar" OnClick="btnEliminar_Click" CssClass="btn btn-danger btn-block" runat="server" Text="Eliminar"
                            OnClientClick="confirmar(this,'¿Está seguro de eliminar la plantilla?'); return false;" />
                    </div>
                    <div class="col-sm-2">
                        <asp:LinkButton ID="cmdGuardar" CssClass="btn btn-primary btn-block" runat="server"
                            OnClick="btnGuardar_Click">
                            <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>

