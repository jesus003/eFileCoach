<%@ Page Title="Rellenar evaluacion" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rellena-evaluacion.aspx.cs" Inherits="rellena_evaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <div class="container text-center">
            <div class="row" style="margin-top: 80px; margin-bottom: 30px;">
                <div class="offset-md-4 col-md-4">
                    <asp:Image ID="imgLogo" ImageUrl="~/Images/logo_efile_coach.jpg"
                        Width="300" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hfIDPersona" runat="server" />
            <asp:HiddenField ID="hfPaginacion" runat="server" />
            <asp:HiddenField ID="hfDimension" runat="server" />
            <div class="container">
            <div class="row">
                <div class="col-md-12 p-4">
                    <div class="card">
                        <div class="card-header" style="text-align: center">
                            <asp:Literal ID="literalTituloDimension" runat="server"></asp:Literal>
                        </div>
                        <!-- Final heading-->
                        <div class="card-body">
                            <asp:DataList ID="dlPreguntas" runat="server" DataKeyField="id"
                                DataSourceID="odsPreguntas" Width="100%" OnItemCommand="dlPreguntas_ItemCommand">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfIdResultado" runat="server" Value='<%# Eval("id") %>' />
                                    <div style="margin-bottom: 10px;">
                                        <asp:Label ID="lblPregunta" runat="server" 
                                            Text='<%# Eval("pregunta") %>' Font-Size="13pt" CssClass="text-left"></asp:Label>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <table class="tablaresultados">
                                                <tr>
                                                    <td>0</td>
                                                    <td>1</td>
                                                    <td>2</td>
                                                    <td>3</td>
                                                    <td>4</td>
                                                    <td>5</td>
                                                    <td>6</td>
                                                    <td>7</td>
                                                    <td>8</td>
                                                    <td>9</td>
                                                    <td>10</td>
                                                </tr>

                                            </table>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:RadioButtonList ID="rblRespuesta" runat="server" CellSpacing="10"
                                                RepeatDirection="Horizontal"
                                                SelectedIndex='<%# Eval("resultado") %>' CssClass="tablaresultados"
                                                CellPadding="10" Width="100%">
                                              <%--  <asp:ListItem Value="0"></asp:ListItem>
                                                <asp:ListItem Value="1"></asp:ListItem>
                                                <asp:ListItem Value="2"></asp:ListItem>
                                                <asp:ListItem Value="3"></asp:ListItem>
                                                <asp:ListItem Value="4"></asp:ListItem>
                                                <asp:ListItem Value="5"></asp:ListItem>
                                                <asp:ListItem Value="6"></asp:ListItem>
                                                <asp:ListItem Value="7"></asp:ListItem>
                                                <asp:ListItem Value="8"></asp:ListItem>
                                                <asp:ListItem Value="9"></asp:ListItem>
                                                <asp:ListItem Value="10"></asp:ListItem>--%>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:ObjectDataSource ID="odsPreguntas" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDataByPersonaIdDimension"
                                TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.vrespuestas_preguntasTableAdapter">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfIDPersona" Name="idpersona" PropertyName="Value" Type="Int64" />
                                    <asp:ControlParameter ControlID="hfDimension" Name="iddimension" PropertyName="Value" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:DataList ID="dlPreguntasAbiertas" runat="server" DataKeyField="id"
                                DataSourceID="odsPreguntas" Width="100%" OnItemDataBound="dlPreguntasAbiertas_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfIDTipo" runat="server" Value='<%# Eval("idtipo") %>' />
                                    <asp:HiddenField ID="hfIdResultado" runat="server" Value='<%# Eval("id") %>' />
                                    <div style="margin-bottom: 10px;">
                                        <asp:Label ID="lblPregunta" runat="server" 
                                            Text='<%# Eval("pregunta") %>' Font-Size="13pt"
                                            CssClass="text-left"></asp:Label>
                                    </div>
                                    <asp:PlaceHolder ID="phRespuestaAbierta" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:TextBox ID="txtRespuesta" Text='<%# Eval("resultado_texto") %>' runat="server"
                                                    CssClass="form-control"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phRespuestaNumerica" runat="server">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table class="tablaresultados">
                                                    <tr runat="server" ID="respuestas_variables"  >
                                                       <%-- <td>0</td>
                                                        <td>1</td>
                                                        <td>2</td>
                                                        <td>3</td>
                                                        <td>4</td>
                                                        <td>5</td>
                                                        <td>6</td>
                                                        <td>7</td>
                                                        <td>8</td>
                                                        <td>9</td>
                                                        <td>10</td>--%>
                                                    </tr>
                                                   
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:RadioButtonList ID="rblRespuesta" runat="server" CellSpacing="10"
                                                    RepeatDirection="Horizontal"
                                                    SelectedIndex='<%# Eval("resultado") %>' CssClass="tablaresultados"
                                                    CellPadding="10" Width="100%">
                                                 <%--   <asp:ListItem Value="0"></asp:ListItem>
                                                    <asp:ListItem Value="1"></asp:ListItem>
                                                    <asp:ListItem Value="2"></asp:ListItem>
                                                    <asp:ListItem Value="3"></asp:ListItem>
                                                    <asp:ListItem Value="4"></asp:ListItem>
                                                    <asp:ListItem Value="5"></asp:ListItem>
                                                    <asp:ListItem Value="6"></asp:ListItem>
                                                    <asp:ListItem Value="7"></asp:ListItem>
                                                    <asp:ListItem Value="8"></asp:ListItem>
                                                    <asp:ListItem Value="9"></asp:ListItem>
                                                    <asp:ListItem Value="10"></asp:ListItem>--%>
                                                </asp:RadioButtonList>
                                                <asp:CheckBoxList ID="ckbRespuesta"  runat="server" CellSpacing="10"
                                                    RepeatDirection="Horizontal"
                                                     CssClass="tablaresultados"
                                                    CellPadding="10" Width="100%">

                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>
                                </ItemTemplate>
                            </asp:DataList>
                            <%--<asp:Panel runat="server" ID="panelObserbaciones" Visible="false">
                                <div class="row alert alert-info" style="margin-top: 20px; margin-bottom: 0px;">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
                                        <asp:TextBox ID="txtObservacionesFinales" runat="server" TextMode="MultiLine" Height="150" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </asp:Panel>--%>

                            <!-- Final body-->
                        </div>
                        <div class="card-footer">
                            <asp:Panel ID="pErroresPreguntas" Visible="false" CssClass="row alert-danger aire" runat="server">
                                <div class="col-sm-12 p-3">
                                    <asp:BulletedList ID="blErroresPreguntas" runat="server"></asp:BulletedList>
                                </div>
                            </asp:Panel>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Button ID="cmdAtras" Style="margin-top: 15px" runat="server" CssClass="btn btn-default btn-block" Text="Atras" OnClick="cmdAtras_Click" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button ID="cmdSiguiente" Style="margin-top: 15px" runat="server" CssClass="btn btn-primary btn-block" Text="Siguiente" OnClick="cmdSiguiente_Click" />
                                </div>
                            </div>
                            <!-- Final footer-->
                        </div>
                        <!-- Final panel -->
                    </div>
                </div>
            </div>
                </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
