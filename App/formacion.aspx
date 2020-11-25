<%@ Page Title="Formación" Language="C#" MasterPageFile="~/MasterPageAuth.master"
    AutoEventWireup="true" CodeFile="formacion.aspx.cs" Inherits="formacion_form" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function desasignarAlumnos(arg) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                if (arg) {
                    ajaxManager.ajaxRequest('okDesasignar');
                }
                else {
                    ajaxManager.ajaxRequest('cancelDesasignar');
                }
            }

            function eliminarFormacion(arg) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                if (arg) {
                    ajaxManager.ajaxRequest('eliminarFormacion');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="usuarioLogueado" />
    <asp:HiddenField ID="hfIDFormacion" runat="server" />
    <asp:HiddenField ID="hfIDCuenta" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlContenido">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlContenido"
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="radVentana" runat="server">
        <Localization OK="Aceptar" No="Cancelar" Cancel="Cancelar" />
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="radVentanaGuardar" runat="server" Modal="true" CenterIfModal="true">
        <Localization OK="Aceptar" />
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"
        Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>
    <asp:Panel ID="pnlContenido" runat="server">
        <div class="row mb-3">
            <div class="col-sm-12">
                <h1>Formación:
                    <asp:Literal ID="literalTitulo" runat="server"></asp:Literal></h1>
            </div>
        </div>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0"
            MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab Text="Datos principales y alumnos" Selected="True"></telerik:RadTab>
                <telerik:RadTab Text="Actividades"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="pvPrincipal" runat="server" Selected="true"
                BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel runat="server" ID="panelContenedor" class="container-fluid p-3"
                    DefaultButton="cmdGuardar">
                    <div class="row">
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Datos principales y alumnos de la formación</h2>
                                </div>
                                <asp:Panel ID="panelContenidoPrincipal"
                                    runat="server" CssClass="card-body">
                                        <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblTitulo" runat="server" Text="Título"
                                                    AssociatedControlID="txtTitulo"></asp:Label>
                                                <asp:TextBox ID="txtTitulo" CssClass="form-control"
                                                    runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-5">
                                                <asp:Label ID="lblFormador" runat="server" Text="Coordinador de la formación"
                                                    AssociatedControlID="cmbFormador"></asp:Label>
                                                <telerik:RadComboBox ID="cmbFormador" Width="100%"
                                                    EmptyMessage="Selección de formador..." runat="server"
                                                    Culture="es-ES" DataSourceID="odsFormadores"
                                                    MarkFirstMatch="True" Filter="Contains"
                                                    DataTextField="formador" DataValueField="id">
                                                </telerik:RadComboBox>
                                                <asp:ObjectDataSource ID="odsFormadores" runat="server"
                                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                    TypeName="Controlador.dsUsuariosTableAdapters.vista_usuarios_formadoresTableAdapter">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hfIDCuenta" PropertyName="Value" Name="idcuenta" Type="Int64"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-sm-4">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <asp:Label ID="lblFechaInicio" runat="server"
                                                            Text="Fecha de inicio" AssociatedControlID="dtFechaInicio"></asp:Label>
                                                        <telerik:RadDatePicker ID="dtFechaInicio" runat="server"
                                                            Width="100%">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-sm-12" style="margin-top: 10px;">
                                                        <asp:Label ID="lblFechaFin" runat="server"
                                                            Text="Fecha final" AssociatedControlID="dtFechaFin"></asp:Label>
                                                        <telerik:RadDatePicker ID="dtFechaFin" runat="server"
                                                            Width="100%">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-sm-9">
                                                        <asp:Label runat="server" ID="lblListaAlumnos"
                                                            AssociatedControlID="listaAlumnos"
                                                            Text="Alumnos asignados a la formación"></asp:Label>
                                                        <telerik:RadListBox ID="listaAlumnos" runat="server"
                                                            Width="100%" Culture="es-ES" DataSourceID="odsListaAlumnos"
                                                            DataTextField="nombre_apellidos"
                                                            DataValueField="id" CheckBoxes="True" ShowCheckAll="True">
                                                            <Localization CheckAll="Seleccionar todos" />
                                                            <ButtonSettings TransferButtons="All" />
                                                        </telerik:RadListBox>
                                                        <asp:ObjectDataSource ID="odsListaAlumnos" runat="server"
                                                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                            TypeName="Controlador.dsFormacionesAlumnosTableAdapters.vista_alumnos_asignados_formacionTableAdapter">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hfIDFormacion" PropertyName="Value"
                                                                    Name="idformacion" Type="Int64"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </div>
                                                    <div class="col-sm-3" style="margin-top: 27px;">
                                                        <asp:LinkButton ID="cmdDesAsignar" runat="server"
                                                            CssClass="btn btn-outline-danger btn-block"
                                                            OnClick="cmdDesAsignar_Click"
                                                            ToolTip="Dar de baja al alum@ de la formación">
                                                        <i class="fas fa-user-times"></i>
                                                        </asp:LinkButton>
                                                    </div>

                                                </div>

                                            </div>
                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-sm-9">
                                                        <asp:Label runat="server" ID="lblAlumnos"
                                                            AssociatedControlID="cmbAlumnos"
                                                            Text="Alumnos disponibles para la formación"></asp:Label>
                                                        <telerik:RadComboBox ID="cmbAlumnos" runat="server"
                                                            Width="100%" CheckBoxes="True" Culture="es-ES" Filter="Contains"
                                                            DataSourceID="odsAlumnosDisponibles"
                                                            DataTextField="nombre_apellidos"
                                                            DataValueField="id">
                                                            <Localization AllItemsCheckedString="Todos los alumnos seleccionados" />
                                                        </telerik:RadComboBox>
                                                        <asp:ObjectDataSource ID="odsAlumnosDisponibles"
                                                            runat="server" OldValuesParameterFormatString="original_{0}"
                                                            SelectMethod="GetData"
                                                            TypeName="Controlador.dsUsuariosFinalesTableAdapters.vista_alumnosTableAdapter">
                                                            <SelectParameters>
                                                                <asp:SessionParameter SessionField="idcuenta" Name="idcuenta"
                                                                    Type="Int64"></asp:SessionParameter>
                                                                <asp:ControlParameter ControlID="hfIDFormacion" PropertyName="Value" Name="idformacion" Type="Int32"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </div>
                                                    <div class="col-sm-3" style="margin-top: 27px;">
                                                        <asp:LinkButton ID="cmdAsignar" runat="server"
                                                            CssClass="btn btn-outline-success btn-block"
                                                            OnClick="cmdAsignar_Click"
                                                            ToolTip="Asignar alumn@"><i class="fas fa-user-plus"></i></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion"
                                                    AssociatedControlID="txtDescripcion"></asp:Label>
                                                <asp:TextBox ID="txtDescripcion" CssClass="form-control" runat="server"
                                                    TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                            <div class="col-sm-8">
                                                <asp:Label ID="lblObjetivos" runat="server" Text="Objetivo de la formación"
                                                    AssociatedControlID="txtObjetivos"></asp:Label>
                                                <asp:TextBox ID="txtObjetivos" CssClass="form-control" runat="server"
                                                    TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Label ID="lblIndicadores" runat="server" Text="Indicadores de seguimiento"
                                                            AssociatedControlID="chkIndicadores" Width="100%" CssClass="mb-2"></asp:Label>
                                                        <telerik:RadListBox runat="server" ID="chkIndicadores" AllowDelete="True" Width="100%"
                                                            Culture="es-ES" DataSourceID="odsIndicadores" DataTextField="indicador"
                                                            DataValueField="id" AutoPostBackOnDelete="True" OnDeleting="chkIndicadores_Deleting">
                                                            <Localization Delete="Eliminar" />
                                                        </telerik:RadListBox>
                                                        <asp:ObjectDataSource ID="odsIndicadores" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDFormacion" TypeName="Controlador.dsFormacionesObjetivosTableAdapters.formaciones_indicadores_objetivosTableAdapter">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hfIDFormacion" PropertyName="Value" Name="idformacion" Type="Int64"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </div>
                                                </div>
                                                <div class="row mt-2">
                                                    <div class="col-md-12">
                                                        <asp:Panel runat="server" CssClass="input-group mb-3"
                                                            ID="panelIndicadores" DefaultButton="cmdNuevoIndicador">
                                                            <asp:TextBox ID="txtNuevoIndicador" runat="server"
                                                                CssClass="form-control" placeholder="Nuevo indicador..."></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <asp:Button ID="cmdNuevoIndicador" runat="server"
                                                                    Text="Añadir indicador"
                                                                    CssClass="btn btn-outline-primary"
                                                                    OnClick="cmdNuevoIndicador_Click" />
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="panelErrores" runat="server" Visible="False">
                                            <div class="row iconos mt-4 mb-0">
                                                <div class="col-sm-12">
                                                    <asp:Panel ID="panelErroresInterior" runat="server">
                                                        <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                </asp:Panel>
                                <asp:Panel CssClass="card-footer" ID="panelPie" runat="server" Visible="">
                                    <div class="row">
                                        <div class="col-sm-4" style="padding-top: 24px">
                                            <asp:HyperLink ID="hlVolver" runat="server"
                                                CssClass="btn btn-outline-primary btn-block"
                                                NavigateUrl="/formaciones.aspx">
                                                <i class="fas fa-arrow-left"></i>&nbsp;&nbsp;Volver a las formaciones
                                            </asp:HyperLink>
                                        </div>
                                        <div class="offset-sm-4 col-sm-2" style="padding-top: 24px">
                                            <asp:LinkButton ID="cmdEliminar" CssClass="btn btn-danger btn-block" runat="server"
                                                OnClick="cmdEliminar_Click">
                                                    Eliminar
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-sm-2" style="padding-top: 24px">
                                            <asp:LinkButton ID="cmdGuardar" CssClass="btn btn-primary btn-block"
                                                runat="server"
                                                OnClick="cmdGuardar_Click">
                                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="pvActividades" BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel runat="server" ID="panel3" class="container-fluid p-3">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Actividades de la formación</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-md-12">
                                            <asp:GridView CssClass="table table-bordered" ID="grdActividades"
                                                runat="server" OnRowDataBound="grdActividades_RowDataBound"
                                                AllowPaging="True" AutoGenerateColumns="False"
                                                OnRowCommand="grdActividades_RowCommand"
                                                GridLines="None" DataKeyNames="idactividad" DataSourceID="odsActividades">
                                                <Columns>
                                                    <asp:TemplateField InsertVisible="False" SortExpression="idactividad">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="hlDetalles" runat="server" CssClass="btn btn-outline-primary btn-block btn-sm"
                                                                NavigateUrl='<%# Eval("idactividad") %>'><i class="fas fa-info-circle"></i> Detalles</asp:HyperLink>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField InsertVisible="False" SortExpression="idactividad">
                                                        <ItemTemplate>
                                                            <asp:Button ID="cmdDuplicar" runat="server" Text="Duplicar" CommandName="duplicar"
                                                                CssClass="btn btn-outline-dark btn-block btn-sm" CommandArgument='<%# Eval("idactividad") %>' 
                                                                OnClientClick="confirmar(this,'¿Desea duplicar la actividad?'); return false;"/>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="titulo" HeaderText="Título" SortExpression="titulo"></asp:BoundField>
                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo"></asp:BoundField>
                                                    <asp:BoundField DataField="profesor" HeaderText="Profesor" SortExpression="profesor"></asp:BoundField>
                                                    <asp:BoundField DataField="fecha_inicio" HeaderText="Desde" SortExpression="fecha_inicio"></asp:BoundField>
                                                    <asp:BoundField DataField="fecha_fin" HeaderText="Hasta" SortExpression="fecha_fin"></asp:BoundField>
                                                    <asp:BoundField DataField="num_horas" HeaderText="Horas" SortExpression="num_horas"></asp:BoundField>
                                                </Columns>

                                                <PagerStyle CssClass="numeros_de_pagina" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsActividades" runat="server"
                                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                TypeName="Controlador.dsFormacionesActividadesTableAdapters.vista_actividadesTableAdapter">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hfIDFormacion" PropertyName="Value"
                                                        Name="idformacion" Type="Int64"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel CssClass="card-footer" ID="panelNuevaActividad" runat="server" Visible="true">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h3>Nueva actividad formativa</h3>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblNuevaActividad" runat="server" Text="Título de la actividad"
                                                AssociatedControlID="txtTituloActividad"></asp:Label>
                                            <asp:TextBox ID="txtTituloActividad" runat="server"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblTipoActividad" runat="server" Text="Tipo"
                                                AssociatedControlID="cmbTipoActividad"></asp:Label>
                                            <asp:DropDownList ID="cmbTipoActividad" runat="server"
                                                DataSourceID="odsTipoActividad" DataTextField="tipo"
                                                DataValueField="id" CssClass="form-control"
                                                OnDataBound="cmbTipoActividad_DataBound">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="odsTipoActividad" runat="server"
                                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                TypeName="Controlador.dsIUFormacionesTiposActividadesTableAdapters.iu_tipos_formaciones_actividadesTableAdapter">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="0" Name="escoaching" Type="Int16"></asp:Parameter>
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblProfesorResponsable" runat="server" Text="Profesor responsable"
                                                AssociatedControlID="cmbProfesorResponsable"></asp:Label>
                                            <telerik:RadComboBox ID="cmbProfesorResponsable" Width="100%"
                                                EmptyMessage="Selección de formador..." runat="server"
                                                Culture="es-ES" DataSourceID="odsFormadores"
                                                MarkFirstMatch="True" Filter="Contains"
                                                DataTextField="formador" DataValueField="id">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-md-3">
                                            <asp:Label ID="lblFechaInicioActividad" runat="server"
                                                Text="Fecha y hora de inicio"
                                                AssociatedControlID="dtFechaInicioActividad"></asp:Label>
                                            <telerik:RadDateTimePicker ID="dtFechaInicioActividad" runat="server"
                                                Width="100%">
                                            </telerik:RadDateTimePicker>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblFechaFinActividad" runat="server"
                                                Text="Fecha y hora de fin"
                                                AssociatedControlID="dtFechaFinActividad"></asp:Label>
                                            <telerik:RadDateTimePicker ID="dtFechaFinActividad" runat="server"
                                                Width="100%">
                                            </telerik:RadDateTimePicker>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblHoras" runat="server"
                                                Text="Número de horas"
                                                AssociatedControlID="txtNumHoras"
                                                Width="100%"></asp:Label>
                                            <telerik:RadNumericTextBox ID="txtNumHoras"
                                                runat="server" NumberFormat-DecimalDigits="0">
                                                <NumberFormat DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                        <div class="col-md-9">
                                            <asp:Label ID="lblInforme" runat="server" Text="Informe"
                                                AssociatedControlID="txtInforme"></asp:Label>
                                            <asp:TextBox ID="txtInforme" CssClass="form-control"
                                                TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3" style="margin-top: 30px;">
                                            <asp:LinkButton ID="cmdGuardarNuevaActividad"
                                                CssClass="btn btn-primary btn-block"
                                                runat="server"
                                                OnClick="cmdGuardarNuevaActividad_Click">
                                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Nueva actividad
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="row iconos">
                                        <div class="col-sm-12">
                                            <asp:Panel ID="panel2" runat="server">
                                                <asp:BulletedList ID="BulletedList1" runat="server"></asp:BulletedList>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </asp:Panel>
    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server"
        RelativeTo="Element" Position="MiddleRight" AutoTooltipify="true"
        ContentScrolling="Default" RenderMode="Lightweight">
    </telerik:RadToolTipManager>
</asp:Content>
