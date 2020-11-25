<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="pdimension.aspx.cs" Inherits="evaluaciones_pdimension" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <asp:HiddenField ID="hfIDDimension" runat="server" />
    <div class="card">
        <div class="card-header">
            <h4>Gestión de la Dimensión</h4>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-sm-12">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre" AssociatedControlID="txtNombre"></asp:Label>
                    <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <asp:Panel ID="panelValidacionCabecera" runat="server" CssClass="row" Visible="false">
                <div class="col-xs-12">
                    <div class="alert alert-danger">
                        <asp:Literal ID="lResultadoCabecera" runat="server"></asp:Literal>
                    </div>
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-md-12">
                    <div class="card mt-3">
                        <div class="card-header">
                            <h4>Preguntas de la dimensión</h4>
                        </div>
                        <div class="card-body">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="alert alert-info">
                                                <h5 class="text-center">Procesando...</h5>
                                            </div>
                                        </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdPreguntas" runat="server" CssClass="table" GridLines="None"
                                        AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="odsPreguntas"
                                        OnRowCommand="grdPreguntas_RowCommand" OnDataBound="grdPreguntas_DataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <div class="btn-group">
                                                        <asp:Button ID="cmdEditar" runat="server" Text="Editar" CssClass="btn btn-outline-secondary" CommandName="editar" />
                                                        <asp:Button ID="cmdEliminar" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="eliminar" CssClass="btn btn-outline-danger"
                                                            OnClientClick="confirmar(this,'¿Desea eliminar la pregunta?'); return false;"
                                                            Text="Eliminar" />
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pregunta" SortExpression="pregunta">
                                                <EditItemTemplate>
                                                    <asp:Panel ID="panelEdicion" runat="server" CssClass="input-group" DefaultButton="cmdGuardar">
                                                        <asp:TextBox ID="txtPregunta" runat="server" Text='<%# Bind("pregunta") %>'
                                                            CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-btn">
                                                            <div class="btn-group">
                                                                <asp:Button ID="cmdGuardar" CssClass="btn btn-outline-primary" runat="server" Text="Guardar"
                                                                    CommandName="guardar" CommandArgument='<%# Eval("id") %>' />
                                                                <asp:Button ID="cmdCancelar" CssClass="btn btn-outline-secondary" runat="server" Text="Cancelar"
                                                                    CommandName="cancelar" CommandArgument='<%# Eval("id") %>' />
                                                            </div>

                                                        </span>
                                                    </asp:Panel>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPregunta" runat="server" Text='<%# Bind("pregunta") %>'></asp:Label>
                                                    <asp:HiddenField ID="hfOrden" runat="server" Value='<%# Eval("orden") %>' />
                                                    <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Orden" SortExpression="orden">
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
                                            <asp:TemplateField HeaderText="Tipo" SortExpression="idtipo">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="cmbTipo" 
                                                        SelectedValue='<%# Bind("idtipo") %>'
                                                        Enabled="false" CssClass="form-control">
                                                        <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Libre"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:ObjectDataSource ID="odsPreguntas" runat="server" DeleteMethod="Eliminar"
                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDDimension"
                                TypeName="Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters.diagnosticos_plantillas_dimensiones_preguntasTableAdapter" InsertMethod="Insert">
                                <DeleteParameters>
                                    <asp:Parameter Name="id" Type="Int64" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="p1" Type="Int64"></asp:Parameter>
                                    <asp:Parameter Name="p2" Type="String"></asp:Parameter>
                                    <asp:Parameter Name="p3" Type="Int32"></asp:Parameter>
                                </InsertParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfIDDimension" PropertyName="Value" Name="iddimension" Type="Int64"></asp:ControlParameter>

                                </SelectParameters>
                            </asp:ObjectDataSource>

                            <asp:Panel ID="panelNuevaPregunta" runat="server" CssClass="well" DefaultButton="cmdGuardarPregunta">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h3>Añadir preguntas a la dimensión</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:Label ID="lblTipoPregunta" runat="server" Text="Tipo de pregunta"
                                            AssociatedControlID="cmbTipoPregunta"></asp:Label>
                                        <asp:DropDownList ID="cmbTipoPregunta" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Seleccionar tipo..."></asp:ListItem>
                                            <asp:ListItem Text="Numérica (0-10)" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Libre" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                        <div class="col-sm-12" runat="server" id="div_num_respuestas">
                                                    <asp:Label ID="Label1" runat="server" Text="Numero de respuestas"></asp:Label><br />
                                                    
                                                <table class="tablaresultados">
                                                    <tr runat="server" ID="respuestas_variables"  >
                                                      <td>0-5</td>
                                                        <td>0-10</td>
                                                    </tr>
                                                   
                                                </table>
                                           
                                                     <asp:RadioButtonList ID="rblNumRespuestas" runat="server" CellSpacing="10"
                                                            RepeatDirection="Horizontal"
                                                             CssClass="tablaresultados"
                                                            CellPadding="10" Width="100%">
                                                            <asp:ListItem Text="0-5" Value="5">0-5</asp:ListItem>
                                                            <asp:ListItem Text="0-10" Selected="True" Value="10">0-10</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                </div>
                                    <div class="col-sm-12">
                                        <asp:Label ID="lblPregunta" runat="server" Text="Pregunta"></asp:Label>
                                        <asp:TextBox ID="txtPregunta" runat="server" TextMode="MultiLine" 
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="panelValidacionNuevaPregunta" runat="server" Visible="false" CssClass="col-sm-12">
                                        <div class="alert alert-danger">
                                            <asp:Literal ID="literalResultado" runat="server"></asp:Literal>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Button ID="cmdGuardarPregunta" runat="server" Text="Guardar pregunta" CssClass="btn btn-primary btn-block" OnClick="cmdGuardarPregunta_Click" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
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
            </div>
        </div>
    </div>

</asp:Content>

