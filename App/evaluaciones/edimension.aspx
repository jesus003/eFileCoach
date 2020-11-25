<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master"
    AutoEventWireup="true" CodeFile="edimension.aspx.cs" Inherits="evaluaciones_edimension" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <asp:HiddenField ID="hfIDDimension" runat="server" />
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <telerik:RadAjaxLoadingPanel ID="panelCarga" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager runat="server" ID="ventanita"></telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="panelContenido" runat="server" LoadingPanelID="panelCarga">
        <div class="card">
            <div class="card-header bg-light">
                <h4>Gestión de la categoría</h4>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre" AssociatedControlID="txtNombre"></asp:Label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
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
                    <div class="col-md-3">
                        <asp:Button ID="cmdGuardar" CssClass="btn btn-block btn-outline-primary" runat="server" Text="Guardar" OnClick="cmdGuardar_Click" />
                    </div>

                </div>
                <div class="card">
                    <div class="card-body">
                        <h4>Preguntas de la categoría</h4>
                        <asp:GridView ID="grdPreguntas" runat="server" CssClass="table" GridLines="None"
                            AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="odsPreguntas"
                            OnRowCommand="grdPreguntas_RowCommand" OnDataBound="grdPreguntas_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="cmdEditar" runat="server" Text="Editar" CssClass="btn btn-default" CommandName="editar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pregunta" SortExpression="pregunta">
                                    <EditItemTemplate>
                                        <asp:Panel ID="panelEdicion" runat="server" CssClass="input-group" DefaultButton="cmdGuardar">
                                            <asp:TextBox ID="txtPregunta" runat="server" Text='<%# Bind("pregunta") %>'
                                                CssClass="form-control"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:Button ID="cmdGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar"
                                                    CommandName="guardar" CommandArgument='<%# Eval("id") %>' />
                                                <asp:Button ID="cmdCancelar" CssClass="btn btn-default" runat="server" Text="Cancelar"
                                                    CommandName="cancelar" CommandArgument='<%# Eval("id") %>' />
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
                                            <span class="glyphicon glyphicon-arrow-up" aria-hidden="true"></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="cmdBajar" runat="server"
                                            CommandName="Bajar" ToolTip="Cancelar"
                                            CssClass="btn btn-default">
                                            <span class="glyphicon glyphicon-arrow-down" aria-hidden="true"></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo" SortExpression="idtipo">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="cmbTipo" SelectedValue='<%# Bind("idtipo") %>'
                                            Enabled="false" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Varias respuestas posibles"></asp:ListItem>
                                                  <asp:ListItem Value="2" Text="Libre (Texto Corto - 255 Caracteres)"></asp:ListItem>
                                                  <asp:ListItem Value="3" Text="Libre (Parrafo - Hasta 1000 Caracteres)"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Opciones">
                                    <ItemTemplate>
                                        <asp:Button ID="cmdEliminar" runat="server"
                                            CommandArgument='<%# Eval("id") %>' CommandName="eliminar"
                                            CssClass="btn btn-outline-danger"
                                            OnClientClick="confirmar(this,'¿Desea eliminar la pregunta?'); return false;" Text="Eliminar" />

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsPreguntas" runat="server" DeleteMethod="Eliminar"
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDDimension"
                            TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter">
                            <DeleteParameters>
                                <asp:Parameter Name="id" Type="Int64" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfIDDimension" PropertyName="Value" Name="iddimension" Type="Int64"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:Panel ID="panelNuevaPregunta" runat="server" CssClass="well" DefaultButton="cmdGuardarPregunta">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <h4>Añadir preguntas a la categoría</h4>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <asp:Label ID="lblTipoPregunta" runat="server" Text="Tipo de pregunta"
                                                        AssociatedControlID="cmbTipoPregunta"></asp:Label>
                                                    <asp:DropDownList ID="cmbTipoPregunta" runat="server"
                                                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cmbTipoPregunta_SelectedIndexChanged">
                                                        <asp:ListItem Text="Seleccionar tipo..."></asp:ListItem>
                                                    
                                                    </asp:DropDownList>
                                                </div>
                                                
                                            </div>
                                            <div class="row" runat="server" id="divPregunta" visible="false">
                                                 <div class="col-sm-12" runat="server" id="div_num_respuestas">
                                                    <asp:Label ID="Label1" runat="server" Text="Numero de respuestas"></asp:Label><br />
                                                    
                                                     <asp:DropDownList ID="cmbxRespuestas" CssClass="form-control" runat="server"></asp:DropDownList>
                                           
                                                    <%-- <asp:RadioButtonList ID="rblNumRespuestas" runat="server" CellSpacing="10"
                                                            RepeatDirection="Horizontal"
                                                             CssClass="tablaresultados"
                                                            CellPadding="10" Width="100%">
                                                            <asp:ListItem Text="0-5" Value="5">0-5</asp:ListItem>
                                                            <asp:ListItem Text="0-10" Selected="True" Value="10">0-10</asp:ListItem>
                                                        </asp:RadioButtonList>--%>
                                                </div>


                                                 <div class="col-sm-12" runat="server" id="divTipoRespuestas">
                                                    <asp:Label ID="Label2" runat="server" Text="Tipo de respuestas"></asp:Label><br />
                                                    
                                                <table class="tablaresultados">
                                                    <tr runat="server" ID="Tr1"  >
                                                      <td>Una sola respuesta correcta</td>
                                                        <td>Varias respuestas correctas</td>
                                                    </tr>
                                                   
                                                </table>
                                           
                                                     <asp:RadioButtonList ID="rblTipoRespuestas" runat="server" CellSpacing="10"
                                                            RepeatDirection="Horizontal"
                                                             CssClass="tablaresultados"
                                                            CellPadding="10" Width="100%">
                                                            <asp:ListItem Text="-1" Value="-1">Una sola respuesta Correcta</asp:ListItem>
                                                            <asp:ListItem Text="-2" Selected="True" Value="-2">Varias respuestas correctas</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                </div>



                                                <div class="col-sm-12">
                                                    <asp:Label ID="lblPregunta" runat="server" Text="Enunciado de la pregunta"></asp:Label>
                                                    <asp:TextBox ID="txtPregunta" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                 
                                            </div>
                                             <div class="row" runat="server" id="divRespuesas" visible="false">

                                                 
                                                <div class="col-sm-12">
                                                    <asp:Label ID="lblRespuestas" runat="server" Text="Ingrese las respuestas, donde cada linea es una respuesta"></asp:Label>
                                                    <asp:TextBox ID="txtRespuestas" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
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
                                                    <asp:Button ID="cmdGuardarPregunta" runat="server" Text="Guardar pregunta"
                                                        CssClass="btn btn-primary btn-block" OnClick="cmdGuardarPregunta_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:HyperLink ID="hlVolver" CssClass="btn btn-outline-primary btn-block" runat="server">Volver</asp:HyperLink>
                    </div>
                </div>

            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
