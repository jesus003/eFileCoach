<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="evaluacion-envios.aspx.cs" Inherits="evaluaciones_evaluacion_envios" %>
<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="UsuarioLogueado" />
    <telerik:RadAjaxPanel runat="server" ID="panelContenido" LoadingPanelID="panelCarga">
        <asp:HiddenField ID="hfIDDiagnostico" runat="server" />
        <div class="container-fluid">
            <div class="card">
                <div class="card-header">
                    <h4>Envío de los enlaces para realizar la evaluación
                         <asp:Literal runat="server" ID="lNombreEvaluacion"></asp:Literal>
                    </h4>
                </div>
                <div class="card-body">
                    <asp:Label ID="lblInicio" runat="server" Text="Seleccione del siguiente listado a los evaluadores a los que desea enviarle el enlace para cumplimentar la evaluación."></asp:Label>

                    <asp:Panel ID="pInfo" CssClass="alert alert-info" runat="server" Visible="false">
                        <asp:Label ID="lblInformacion" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="progress">
                                <asp:Literal ID="litSuccess" runat="server"></asp:Literal>

                                <asp:Literal ID="litInfo" runat="server"></asp:Literal>

                                <asp:Literal ID="litDanger" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row listado_destinos">
                        <div class="col-md-12">
                            <asp:Panel ID="pAutoevaluacion" runat="server" Visible="false">
                                <asp:HiddenField ID="hfIDAutoeval" runat="server" />
                                <asp:HiddenField ID="hfEmailAutoeval" runat="server" />
                                <div class="autoevaluacion checkbox">
                                    <asp:CheckBox ID="chkNombreAutoeval" Checked="false"
                                        runat="server" />
                                    <asp:Literal ID="litEstadoAutoeval" runat="server"></asp:Literal>

                                </div>
                            </asp:Panel>
                            <asp:DataList ID="listadoEvaluadores" runat="server" DataSourceID="odsEvaluadoresDisponibles" RepeatLayout="Flow" OnItemDataBound="listadoEvaluadores_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfID" Value='<%# Eval("id") %>' runat="server" />
                                    <asp:HiddenField ID="hfEstado" runat="server" Value='<%# Eval("cerrado") %>' />
                                    <asp:HiddenField ID="hfEmail" Value='<%# Eval("email") %>' runat="server" />
                                    <div class="checkbox">
                                        <asp:CheckBox ID="chkNombre" Checked="true"
                                            Text='<%# Eval("nombre").ToString() + " " + Eval("apellidos").ToString() +
                                                   " - " + Eval("email").ToString()%>'
                                            runat="server" />
                                        <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:ObjectDataSource ID="odsEvaluadoresDisponibles"
                                runat="server"
                                OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDataByIDDiagnostico"
                                TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfIDDiagnostico" PropertyName="Value" Name="iddiagnostico" Type="Int64"></asp:ControlParameter>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />
                            <asp:Button runat="server" ID="cmdSeleccionarTodos" Text="Seleccionar todos"
                                CssClass="btn btn-link" OnClick="cmdSeleccionarTodos_Click" />
                            &nbsp;
                                <asp:Button runat="server" ID="cmdDesSeleccionarTodos" Text="Desseleccionar todos"
                                    CssClass="btn btn-link" OnClick="cmdDesSeleccionarTodos_Click" />
                        </div>
                    </div>
                    <asp:Panel ID="panelHistorial" runat="server">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <p>La evaluación ya tiene envios anteriores.</p>
                                        <a class="btn btn-default" role="button" data-toggle="collapse"
                                            href="#collapseExample" aria-expanded="false"
                                            aria-controls="collapseExample">Consultar historial de envíos</a>
                                        <div class="collapse" id="collapseExample" style="margin-top: 20px;">
                                            <asp:GridView ID="grdHistorial" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="odsHistorial"
                                                GridLines="None" CssClass="table table-condensed">
                                                <Columns>
                                                    <asp:BoundField DataField="nombre" HeaderText="nombre" SortExpression="nombre"></asp:BoundField>
                                                    <asp:BoundField DataField="apellidos" HeaderText="apellidos" SortExpression="apellidos"></asp:BoundField>
                                                    <asp:BoundField DataField="para" HeaderText="para" SortExpression="para"></asp:BoundField>
                                                    <asp:BoundField DataField="fecha" HeaderText="fecha" SortExpression="fecha"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsHistorial" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                                                TypeName="Controlador.Diagnosticos.dsDiagnosticosTableAdapters.v_evaluaciones_envio_emailsTableAdapter">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hfIDDiagnostico" PropertyName="Value" 
                                                        Name="idevaluacion" Type="Int64"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:Label runat="server" ID="lblTexto" AssociatedControlID="txtEmail"
                                Text="Introduzca el texto para enviar a los evaluadores"></asp:Label>
                            <asp:TextBox runat="server" ID="txtEmail" TextMode="MultiLine"
                                CssClass="form-control" Rows="8"></asp:TextBox>
                        </div>
                    </div>
                    <asp:Panel ID="panelEnvios" runat="server" CssClass="row" Visible="false">
                        <div class="col-sm-12">
                            <asp:Literal ID="lEnvios" runat="server"></asp:Literal>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-6">
                            <asp:HyperLink ID="hlVolver" runat="server" CssClass="btn btn-default btn-block">
                                    Volver
                            </asp:HyperLink>
                        </div>
                        <div class="col-sm-6">
                            <asp:Button runat="server" ID="cmdEnviar" Text="Enviar"
                                CssClass="btn btn-primary btn-block" OnClick="cmdEnviar_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="panelCarga" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
</asp:Content>