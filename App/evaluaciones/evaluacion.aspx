<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" 
    CodeFile="evaluacion.aspx.cs" Inherits="evaluaciones_evaluacion" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <telerik:RadWindowManager ID="ventanita" runat="server"></telerik:RadWindowManager>
    <telerik:RadAjaxPanel runat="server" ID="panelPrincipal" LoadingPanelID="panelCarga">
        <asp:HiddenField ID="hfIDAuto" runat="server" />
        <asp:HiddenField ID="hfIDEvaluacion" runat="server" />
        <div class="card">
            <div class="card-header bg-light">
                <h4>Gestión de la Encuesta</h4>
            </div>
            <div class="card-body">
                <asp:Panel ID="panelResultadosGuardar" runat="server" CssClass="row" Visible="false">
                    <div class="col-sm-12">
                        <div class="alert alert-success">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                            <span class="sr-only">Guardado:</span>
                            Guardado con éxito.
                        </div>
                    </div>
                </asp:Panel>
                <div class="row mt-2 mb-2">
                    <div class="col-md-4">
                        <asp:Label ID="lblNombre" AssociatedControlID="txtNombre" runat="server"
                            Text="Nombre de la Encuesta" />
                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblTipoEncuesta" runat="server" AssociatedControlID="cmbTipoEncuesta"
                            Text="Tipo de encuesta" />
                        <asp:DropDownList ID="cmbTipoEncuesta" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="cmbTipoEncuesta_SelectedIndexChanged">
                            <asp:ListItem Text="Seleccionar tipo..."></asp:ListItem>
                            <asp:ListItem Text="Numérica" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Mixta" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfIDTipo" runat="server" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblAlta" runat="server" AssociatedControlID="txtFechaAlta"
                            Text="Fecha de Alta" />
                        <asp:TextBox ID="txtFechaAlta" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblClientes" AssociatedControlID="cmbClientes" runat="server"
                            Text="Persona evaluada: Listado de Clientes" />
                        <asp:DropDownList ID="cmbClientes" CssClass="form-control"
                            runat="server" AppendDataBoundItems="True"
                            OnDataBound="cmbClientes_DataBound" DataSourceID="odsClientes"
                            DataTextField="nombre_apellidos" DataValueField="id" />
                        <asp:ObjectDataSource ID="odsClientes" runat="server"
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                            TypeName="Controlador.dsUsuariosFinalesTableAdapters.vista_usuarios_finales_para_evaluacionesTableAdapter">
                            <SelectParameters>
                                <asp:SessionParameter Name="idcuenta" SessionField="idcuenta" Type="Int64" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </div>
                <asp:Panel ID="panelResultados" runat="server" CssClass="row"
                    Visible="false">
                    <div class="col-sm-12">
                        <div class="alert alert-danger">
                            <asp:BulletedList ID="blResultados" runat="server">
                            </asp:BulletedList>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="cmdGuardar" CssClass="btn btn-outline-primary btn-block"
                            runat="server" Text="Guardar" OnClick="cmdGuardar_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:HyperLink ID="hlEnviarEnlaces" CssClass="btn btn-outline-primary btn-block"
                            runat="server" Text="Enviar enlaces"></asp:HyperLink>
                    </div>
                    <div class="col-md-2">
                        <asp:HyperLink ID="hlInforme" Target="_blank" CssClass="btn btn-outline-primary btn-block"
                            runat="server" Text="Informe"></asp:HyperLink>
                    </div>
                    <div class="offset-md-4 col-md-2">
                        <asp:Button ID="cmdEliminar"
                            OnClientClick="confirmar(this,'¿Desea eliminar la encuesta?'); return false;"
                            CssClass="btn btn-danger btn-block"
                            runat="server" Text="Eliminar" OnClick="btnEliminar_Click" Enabled="true" />
                    </div>
                </div>
                <asp:Panel ID="panelDimensionesSinPregunta" runat="server" CssClass="row" Visible="false">
                    <div class="col-md-12">
                        <div class="alert alert-danger">
                            <p>No se pueden enviar los enlaces porque faltan preguntas en alguna categorías.</p>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <telerik:RadTabStrip ID="tabsCabeceraOpciones" Visible="false" runat="server"
                    SelectedIndex="0" MultiPageID="tabsOpciones">
                    <Tabs>
                        <telerik:RadTab Text="Categorías" Selected="True"></telerik:RadTab>
                        <telerik:RadTab Text="Evaluadores"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="tabsOpciones" Visible="false" runat="server">
                    <telerik:RadPageView ID="pvDimensiones" runat="server" Selected="true"
                        BorderStyle="Solid" BorderColor="#dedede"
                        BorderWidth="1px">
                        <div class="row pl-4 pr-4">
                            <div class="col-sm-8">
                                <asp:Label ID="lblPlantilla" runat="server" AssociatedControlID="cmbPlantillas"
                                    Text="Plantillas de Encuestas" />
                                <asp:DropDownList ID="cmbPlantillas" CssClass="form-control"
                                    runat="server" DataSourceID="odsPlantillas" DataTextField="nombre"
                                    DataValueField="id" OnDataBound="cmbPlantillas_DataBound" AppendDataBoundItems="True" />
                                <asp:ObjectDataSource ID="odsPlantillas" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                    TypeName="Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters.vista_plantillas_disponiblesTableAdapter">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="idusuario" SessionField="idusuario" Type="Int64" />
                                        <asp:ControlParameter ControlID="hfIDTipo" PropertyName="Value"
                                            Name="idtipo" Type="Int64"></asp:ControlParameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                            <div class="col-sm-4">
                                <asp:Button ID="cmdImportarPlantilla" runat="server" Text="Importar"
                                    CssClass="btn btn-outline-primary btn-block btn-default" Style="margin-top: 28px;"
                                    OnClick="cmdImportarPlantilla_Click"
                                    OnClientClick="confirmar(this,'¿Desea importar la plantilla?'); return false;" />
                            </div>
                        </div>
                        <div class="row pl-4 pr-4">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-body">
                                        <asp:Literal runat="server" ID="literalTituloDimensiones">
                                        </asp:Literal>
                                        <asp:GridView ID="grdDimensiones" runat="server"
                                            CssClass="table table-condensed" GridLines="None"
                                            AutoGenerateColumns="False" DataKeyNames="id"
                                            DataSourceID="odsDimensiones" OnRowCommand="grdDimensiones_RowCommand"
                                            OnDataBound="grdDimensiones_DataBound"
                                            OnRowDataBound="grdDimensiones_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:HyperLink ID="hlDetalles" runat="server"
                                                                CssClass="btn btn-outline-secondary btn-sm"
                                                                NavigateUrl='<%# "edimension.aspx?id=" + Controlador.Cifrado.cifrarParaUrl(Eval("id").ToString()) %>'>Detalles</asp:HyperLink>
                                                            <asp:Button ID="cmdEliminar" runat="server"
                                                                CausesValidation="False" CommandName="eliminar"
                                                                CssClass="btn btn-outline-danger btn-sm"
                                                                OnClientClick="confirmar(this,'¿Desea eliminar la dimensión?'); return false;"
                                                                Text="Eliminar" CommandArgument='<%# Eval("id") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="190px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="titulo" HeaderText="Título" SortExpression="titulo" />
                                                <asp:TemplateField HeaderText="Orden">
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hfID" runat="server" Value='<%# Bind("id") %>' />
                                                        <asp:HiddenField ID="hfOrden" runat="server" Value='<%# Eval("orden") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfID" runat="server" Value='<%# Bind("id") %>' />
                                                        <asp:HiddenField ID="hfOrden" runat="server" Value='<%# Eval("orden") %>' />
                                                        <asp:LinkButton ID="cmdSubir" runat="server"
                                                            CommandName="Subir" ToolTip="Cancelar"
                                                            CssClass="btn btn-outline-secondary">
                                                                <i class="fas fa-sort-up"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="cmdBajar" runat="server"
                                                            CommandName="Bajar" ToolTip="Cancelar"
                                                            CssClass="btn btn-outline-secondary">
                                                                <i class="fas fa-sort-down"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsDimensiones" runat="server"
                                            OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetDataByIDDiagnostico"
                                            TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hfIDEvaluacion"
                                                    PropertyName="Value" Name="iddiagnostico"
                                                    Type="Int64"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row pl-4 pr-4">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-body">
                                        <asp:Panel ID="panelValidacionDimension" runat="server" CssClass="row" Visible="false">
                                            <div class="col-sm-12">
                                                <div class="alert alert-danger">
                                                    <asp:BulletedList ID="blValidacionDimension"
                                                        runat="server">
                                                    </asp:BulletedList>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="panelNuevaDimension" runat="server" CssClass="row"
                                            DefaultButton="cmdGuardarDimension">
                                            <div class="col-sm-12">
                                                <h4>Alta de nueva categoría</h4>
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:Label ID="lblNuevaDimension" runat="server" Text="Nombre de la categoría"
                                                    AssociatedControlID="txtNuevaDimension"></asp:Label>
                                                <asp:TextBox ID="txtNuevaDimension" runat="server"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Button ID="cmdGuardarDimension" runat="server" Text="Nueva categoría"
                                                    CssClass="btn btn-outline-primary btn-block fila"
                                                    OnClick="cmdGuardarDimension_Click"
                                                    Style="margin-top: 28px;" />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvEvaluadores" runat="server" Selected="true"
                        BorderStyle="Solid" BorderColor="#dedede"
                        BorderWidth="1px">
                        <div class="row pl-4 pr-4">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h4>Grupos de evaluadores</h4>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:GridView ID="grdGrupos" runat="server" GridLines="None"
                                                    CssClass="table table-condensed" AutoGenerateColumns="False"
                                                    DataKeyNames="id" DataSourceID="odsGrupos"
                                                    OnRowCommand="grdGrupos_RowCommand"
                                                    OnDataBound="grdGrupos_DataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div class="btn-group">
                                                                    <asp:Button ID="cmdEditar" runat="server" Text="Editar" CommandName="Edit"
                                                                        cssclass="btn btn-outline-secondary btn-sm" />
                                                                    <asp:Button ID="cmdEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                                                                         OnClientClick="confirmar(this,'¿Está seguro de  eliminar la el grupo y todos sus miembros?'); return false;"   
                                                                         CssClass="btn btn-outline-danger btn-sm" 
                                                                         CommandArgument='<%# Eval("id") %>'/>
                                                                </div>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="btn-group">
                                                                    <asp:Button ID="cmdGuardar" runat="server" Text="Guardar"
                                                                        cssclass="btn btn-outline-success btn-sm" CommandName="Guardar" 
                                                                        CommandArgument='<%# Eval("id") %>' />
                                                                    <asp:Button ID="cmdCancelar" runat="server" Text="Cancelar" CommandName="Cancel"
                                                                         CssClass="btn btn-sm btn-outline-secondary" />
                                                                </div>
                                                            </EditItemTemplate>
                                                            <ItemStyle Width="200px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nombre del grupo">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="literalTexto" runat="server"
                                                                    text='<%# Eval("nombre") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Panel runat="server" DefaultButton="cmdGuardar">
                                                                    <asp:TextBox ID="txtNombre" CssClass="form-control"
                                                                    Text='<%# Eval("nombre") %>' runat="server"></asp:TextBox>
                                                                </asp:Panel>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Porcentaje (0-100)">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="literalPonderacion"
                                                                    text='<%# Decimal.Parse(Eval("ponderacion").ToString()).ToString("0.00") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Panel runat="server" DefaultButton="cmdGuardar">
                                                                    <telerik:RadNumericTextBox ID="txtPonderacion" runat="server"
                                                                        Width="100%"
                                                                        Value='<%# Decimal.Parse(Eval("ponderacion").ToString()) %>'
                                                                        NumberFormat-DecimalDigits="2">
                                                                        <NumberFormat DecimalDigits="2" />
                                                                    </telerik:RadNumericTextBox>
                                                                </asp:Panel>
                                                            </EditItemTemplate>
                                                            <ItemStyle Width="150px" />
                                                            <HeaderStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="odsGrupos" runat="server" DeleteMethod="Delete" 
                                                    InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDEvaluacion" TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticos_personas_gruposTableAdapter" UpdateMethod="Update">
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="idgrupo" Type="Int64"></asp:Parameter>
                                                    </DeleteParameters>
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hfIDEvaluacion" PropertyName="Value" 
                                                            Name="iddiagnostico" Type="Int64"></asp:ControlParameter>
                                                    </SelectParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="p1" Type="Int64"></asp:Parameter>
                                                        <asp:Parameter Name="p2" Type="String"></asp:Parameter>
                                                        <asp:Parameter Name="p3" Type="String"></asp:Parameter>
                                                        <asp:Parameter Name="p4" Type="Single"></asp:Parameter>
                                                        <asp:Parameter Name="p5" Type="Int64"></asp:Parameter>
                                                    </UpdateParameters>
                                                </asp:ObjectDataSource>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Literal ID="literalContadorPonderacion" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <h5>Alta de un nuevo grupo</h5>
                                            </div>
                                        </div>
                                        <asp:Panel ID="panelNuevoGrupo" CssClass="row" runat="server" DefaultButton="cmdNuevoGrupo">
                                            <div class="col-md-6">
                                                <asp:Label ID="lblNuevoGrupo" runat="server" Text="Nombre del grupo"
                                                    AssociatedControlID="txtNuevoGrupo"></asp:Label>
                                                <asp:TextBox ID="txtNuevoGrupo" CssClass="form-control"
                                                    placeholder="Nombre del grupo de evaluadores..."
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblPonderacion" runat="server" Text="Ponderación (0-100)"
                                                    AssociatedControlID="txtPonderacion"></asp:Label>
                                                <telerik:RadNumericTextBox ID="txtPonderacion" Width="100%"
                                                    NumberFormat-DecimalDigits="2"
                                                    runat="server">
                                                    <NumberFormat DecimalDigits="2" />
                                                </telerik:RadNumericTextBox>
                                            </div>
                                            <div class="col-md-3" style="margin-top: 29px;">
                                                <asp:Button ID="cmdNuevoGrupo" runat="server" Text="Nuevo grupo"
                                                    OnClick="cmdNuevoGrupo_Click"
                                                    CssClass="btn btn-block btn-outline-primary" />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="panelListadoEvaluadores" CssClass="row pl-4 pr-4">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-body">
                                        <asp:Literal runat="server" ID="literalTituloEvaluadores"></asp:Literal>
                                        <asp:GridView ID="grdUsuarios" runat="server" GridLines="None"
                                            CssClass="table table-condensed" AutoGenerateColumns="False" DataKeyNames="id"
                                            DataSourceID="odsUsuarios" OnRowCommand="grdUsuarios_RowCommand"
                                            OnRowDataBound="grdUsuarios_RowDataBound" OnDataBound="grdUsuarios_DataBound">
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:Button ID="cmdActualizar" runat="server"
                                                                CausesValidation="True" CommandName="actualizar" Text="Actualizar"
                                                                CssClass="btn btn-primary " />
                                                            <asp:Button ID="cmdCancelar" runat="server" CssClass="btn btn-default btn-outline-secondary"
                                                                CausesValidation="False" CommandName="Cancel" Text="Cancelar" />
                                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Bind("id") %>' />
                                                        </div>
                                                        <asp:HiddenField ID="hfEstadoEvaluador" runat="server" Value='<%# Eval("cerrado") %>' />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <div class="btn-group">
                                                            <asp:Button ID="cmdEditar" CommandName="Editar"
                                                                runat="server" Text="Editar"
                                                                CssClass="btn btn-outline-secondary" />
                                                            <asp:Button ID="cmdEliminar" runat="server"
                                                                CommandName="eliminar" Text="Eliminar"
                                                                CssClass="btn btn-outline-danger"
                                                                OnClientClick="confirmar(this,'¿Desea eliminar la fila seleccionada?'); return false;"
                                                                CommandArgument='<%# Eval("id") %>' />
                                                        </div>
                                                        <asp:HiddenField ID="hfEstadoEvaluador" runat="server" Value='<%# Eval("cerrado") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="190px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre" SortExpression="nombre">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNombreGrid" runat="server"
                                                            CssClass="form-control"
                                                            Text='<%# Bind("nombre") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Apellidos" SortExpression="apellidos">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtApellidosGrid"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            Text='<%# Bind("apellidos") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server"
                                                            Text='<%# Bind("apellidos") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="e-mail" SortExpression="email">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEmailGrid" runat="server"
                                                            CssClass="form-control" Text='<%# Bind("email") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grupo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrupo" runat="server" Text='<%# Eval("grupo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="cmbGrupo" runat="server" DataSourceID="odsGrupos" 
                                                            DataTextField="nombre" DataValueField='id' SelectedValue='<%# Eval("idgrupo") %>'
                                                            CssClass="form-control"></asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltEstadoEvaluador" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Literal ID="ltEstadoEvaluador" runat="server"></asp:Literal>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsUsuarios" runat="server"
                                            OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData"
                                            TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticos_personas_grdTableAdapter">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hfIDEvaluacion" PropertyName="Value"
                                                    Name="iddiagnostico" Type="Int64"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="panelValidacionPersona" runat="server" CssClass="row pl-4 pr-4" Visible="false">
                            <div class="col-sm-12">
                                <div class="alert alert-danger">
                                    <asp:BulletedList ID="blValidacionPersona"
                                        runat="server">
                                    </asp:BulletedList>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="panelNuevaPersona" runat="server" CssClass="row pl-4 pr-4" DefaultButton="cmdGuardarPersona">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h4>Alta de un nuevo evaluador</h4>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="lblNombrePersona" runat="server" Text="Nombre"
                                                    AssociatedControlID="txtNombrePersona"></asp:Label>
                                                <asp:TextBox ID="txtNombrePersona" runat="server"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label ID="lblApellidos" runat="server" Text="Apellidos"
                                                    AssociatedControlID="txtApellidos"></asp:Label>
                                                <asp:TextBox ID="txtApellidos" runat="server"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="lblEmail" runat="server" Text="Correo electrónico"
                                                    AssociatedControlID="txtEmail"></asp:Label>
                                                <asp:TextBox ID="txtEmail" runat="server"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label ID="lblGrupo" runat="server" Text="Grupo"
                                                    AssociatedControlID="cmbGrupo"></asp:Label>
                                                <asp:DropDownList ID="cmbGrupo" runat="server"
                                                    CssClass="form-control" DataSourceID="odsGrupos" DataTextField="nombre" DataValueField="id" OnDataBound="cmbGrupo_DataBound"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Button ID="cmdGuardarPersona" runat="server" Text="Añadir evaluador"
                                                    CssClass="btn btn-outline-primary btn-block fila"
                                                    Style="margin-top: 28px;"
                                                    OnClick="cmdGuardarPersona_Click" />
                                            </div>
                                            <asp:Panel ID="pErroresFicheros" runat="server" CssClass="col-sm-12 alert alert-danger margen20" Visible="False">
                                                <asp:BulletedList ID="blErroresFicheros" runat="server"></asp:BulletedList>
                                            </asp:Panel>
                                            <div class="col-sm-12 mt-2">
                                                <h3>O sube tu fichero de Evaluadores</h3>
                                                <p>Puedes subir un fichero CSV con el listado de evaluadores, separados por comas o punto y coma.</p>
                                            </div>
                                            <asp:Panel ID="pnlNotificacionesFicheros" runat="server" Visible="False">
                                                <asp:BulletedList ID="blFicherosNotificaciones" runat="server"></asp:BulletedList>
                                            </asp:Panel>
                                            <div class="col-md-12 mt-2">
                                                <telerik:RadAsyncUpload ID="Archivo" runat="server"
                                                    AllowedFileExtensions=".csv,.txt"
                                                    HideFileInput="true"
                                                    CssClass="async-attachment"
                                                    RenderMode="Lightweight" MultipleFileSelection="Disabled">
                                                    <Localization Select="Seleccionar fichero" DropZone="Arrastra y suelta el fichero a subir"
                                                        Remove="Eliminar" />
                                                </telerik:RadAsyncUpload>
                                                
                                            </div>
                                            <div class="col-md-12 mt-2">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <asp:Button ID="cmdInsertarEvaluadoresArchivo" CssClass="btn btn-outline-secondary btn-block" 
                                                            runat="server" Text="Analizar y subir fichero" OnClick="cmdInsertarEvaluadoresArchivo_Click" Enabled="True" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:Literal ID="literalMensajeAyudaCSV" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-md-12 mt-2">
                                                <asp:Literal ID="literalAyudaFichero" runat="server"></asp:Literal>
                                            </div>

                                            <asp:Panel ID="pnlAsignaciones" runat="server" Visible="False" CssClass="col-md-12 mt-2">
                                                <asp:PlaceHolder ID="phControlesDinamicos" runat="server"></asp:PlaceHolder>
                                                <div class="col-md-12 mt-2">
                                                    <asp:Button ID="cmdMostrarAsignaciones" CssClass="btn btn-default btn-block" runat="server" 
                                                        Text="Importar Fichero" OnClick="cmdMostrarAsignaciones_Click" />
                                                </div>
                                            </asp:Panel>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </telerik:RadPageView>

                </telerik:RadMultiPage>
            </div>
            <div class="card-footer">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:HyperLink ID="hlVolver" runat="server"
                            CssClass="btn btn-outline-primary btn-block"
                            NavigateUrl="/evaluaciones">
                        <i class="fas fa-arrow-left"></i> Volver
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel runat="server" ID="panelCarga" Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
