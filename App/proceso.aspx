<%@ Page Title="Proceso" Language="C#" MasterPageFile="~/MasterPageAuth.master"
    AutoEventWireup="true" CodeFile="proceso.aspx.cs" Inherits="proceso_form" %>
<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>
<%@ Register Src="~/controles/elementos_telerik.ascx" TagPrefix="uc1" TagName="elementos_telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
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
                    ajaxManager.ajaxRequest('eliminarProceso');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="usuarioLogueado" />
    <asp:HiddenField ID="hfIDProceso" runat="server" />
    <asp:HiddenField ID="hfIDUsuario" runat="server" />
    <asp:HiddenField ID="hfIDCuenta" runat="server" />
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
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
                <h1>Proceso de coaching: <asp:Literal ID="literalTitulo" runat="server"></asp:Literal></h1>
            </div>
        </div>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0"
            MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab Text="Datos principales y coachees" Selected="True"></telerik:RadTab>
                <telerik:RadTab Text="Sesiones"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true"
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
                                    <h2>Datos principales y coachees del proceso de coaching</h2>
                                </div>
                                <asp:Panel runat="server" ID="panelContenidoPrincipal" 
                                    cssclass="card-body">
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblTitulo" runat="server" Text="Título"
                                                AssociatedControlID="txtTitulo"></asp:Label>
                                            <asp:TextBox ID="txtTitulo" CssClass="form-control"
                                                runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-5">
                                            <asp:Label ID="lblFormador" runat="server" Text="Coordinador del proceso de coaching"
                                                AssociatedControlID="cmbCoach"></asp:Label>
                                            <telerik:RadComboBox ID="cmbCoach" Width="100%"
                                                EmptyMessage="Selección de formador..." runat="server"
                                                Culture="es-ES" DataSourceID="odsCoaches"
                                                MarkFirstMatch="True" Filter="Contains"
                                                DataTextField="formador" DataValueField="id">
                                            </telerik:RadComboBox>
                                            <asp:ObjectDataSource ID="odsCoaches" runat="server"
                                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                TypeName="Controlador.dsUsuariosTableAdapters.vista_usuarios_coachesTableAdapter">
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
                                                        Text="Coachees asignados al proceso"></asp:Label>
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
                                                            <asp:ControlParameter ControlID="hfIDProceso" PropertyName="Value"
                                                                Name="idformacion" Type="Int64"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </div>
                                                <div class="col-sm-3" style="margin-top: 27px;">
                                                    <asp:LinkButton ID="cmdDesAsignar" runat="server"
                                                        CssClass="btn btn-outline-danger btn-block"
                                                        OnClick="cmdDesAsignar_Click"
                                                        ToolTip="Dar de baja la persona del proceso">
                                                        <i class="fas fa-user-times"></i>
                                                    </asp:LinkButton>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="col-md-4">
                                            <asp:Panel runat="server" ID="panelDisponiblesParaAsignar"
                                                CssClass="row">
                                                <div class="col-sm-9">
                                                    <asp:Label runat="server" ID="lblAlumnos"
                                                        AssociatedControlID="cmbAlumnos"
                                                        Text="Coachees disponibles para el proceso"></asp:Label>
                                                    <telerik:RadComboBox ID="cmbAlumnos" runat="server"
                                                        Width="100%" CheckBoxes="True" Culture="es-ES" Filter="Contains"
                                                        DataSourceID="odsPersonasDisponibles"
                                                        DataTextField="nombre_apellidos"
                                                        DataValueField="id">
                                                        <Localization AllItemsCheckedString="Todos los coachees seleccionados" />
                                                    </telerik:RadComboBox>
                                                    <asp:ObjectDataSource ID="odsPersonasDisponibles"
                                                        runat="server" OldValuesParameterFormatString="original_{0}"
                                                        SelectMethod="GetData"
                                                        TypeName="Controlador.dsUsuariosFinalesTableAdapters.vista_personasTableAdapter">
                                                        <SelectParameters>
                                                            <asp:SessionParameter SessionField="idcuenta" Name="idcuenta"
                                                                Type="Int64"></asp:SessionParameter>
                                                            <asp:ControlParameter ControlID="hfIDProceso" PropertyName="Value" Name="idformacion" Type="Int32"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </div>
                                                <div class="col-sm-3" style="margin-top: 27px;">
                                                    <asp:LinkButton ID="cmdAsignar" runat="server"
                                                        CssClass="btn btn-outline-success btn-block"
                                                        OnClick="cmdAsignar_Click"
                                                        ToolTip="Asignar persona"><i class="fas fa-user-plus"></i></asp:LinkButton>
                                                </div>
                                            </asp:Panel>
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
                                            <asp:Label ID="lblObjetivos" runat="server" Text="Objetivos del proceso"
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
                                                            <asp:ControlParameter ControlID="hfIDProceso" PropertyName="Value" Name="idformacion" Type="Int64"></asp:ControlParameter>
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
                                                NavigateUrl="/procesos.aspx">
                                                <i class="fas fa-arrow-left"></i>&nbsp;&nbsp;Volver a los procesos de coaching
                                            </asp:HyperLink>
                                        </div>
                                        <div class="offset-sm-4 col-sm-2" style="padding-top: 24px">
                                            <asp:Button ID="cmdEliminar" CssClass="btn btn-danger btn-block" runat="server"
                                                OnClick="cmdEliminar_Click" Text="Eliminar"
                                                OnClientClick="confirmarb(this,'¿Desea eliminar el proceso de coaching? Se eliminará toda la información del mismo, así como los datos de las sesiones.'); return false;"  />
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
            <telerik:RadPageView runat="server" ID="pvActividades">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="card">
                            <div class="card-header bg-light">
                                <h2>Sesiones</h2>
                            </div>
                            <div class="card-body">
                                <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                    <div class="col-md-12">
                                        <asp:GridView CssClass="table table-bordered" ID="grdProcesos"
                                            runat="server" OnRowDataBound="grdProcesos_RowDataBound"
                                            OnRowCommand="grdProcesos_RowCommand"
                                            AllowPaging="True" AutoGenerateColumns="False"
                                            GridLines="None" DataKeyNames="idactividad" DataSourceID="odsProcesos">
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
                                                            OnClientClick="confirmar(this,'¿Desea duplicar la sesión?'); return false;"/>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="numero_sesion" HeaderText="N&#186; sesi&#243;n" SortExpression="numero_sesion"></asp:BoundField>
                                                <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo"></asp:BoundField>
                                                <asp:BoundField DataField="profesor" HeaderText="Coach" SortExpression="profesor"></asp:BoundField>
                                                <asp:BoundField DataField="fecha_inicio" HeaderText="Desde" SortExpression="fecha_inicio"></asp:BoundField>
                                                <asp:BoundField DataField="fecha_fin" HeaderText="Hasta" SortExpression="fecha_fin"></asp:BoundField>
                                                <asp:BoundField DataField="num_horas" HeaderText="Horas" SortExpression="num_horas"></asp:BoundField>
                                            </Columns>

                                            <PagerStyle CssClass="numeros_de_pagina" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsProcesos" runat="server"
                                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                            TypeName="Controlador.dsFormacionesActividadesTableAdapters.vista_actividadesTableAdapter">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hfIDProceso" PropertyName="Value"
                                                    Name="idformacion" Type="Int64"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="odsProcesosUsuario" runat="server"
                                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIDFormacionIDProfesor"
                                            TypeName="Controlador.dsFormacionesActividadesTableAdapters.vista_actividadesTableAdapter">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hfIDProceso" PropertyName="Value"
                                                    Name="idformacion" Type="Int64"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="hfIDUsuario" PropertyName="Value" Name="idprofesor" Type="Int64"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel CssClass="card-footer" ID="panel1" runat="server" Visible="true">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3>Nueva sesión</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="lblNuevaActividad" runat="server" Text="Número de la sesión"
                                            AssociatedControlID="txtTituloSesion"></asp:Label>
                                        <asp:TextBox ID="txtTituloSesion" runat="server"
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblTipoSesion" runat="server" Text="Tipo"
                                            AssociatedControlID="cmbTipoSesion"></asp:Label>
                                        <asp:DropDownList ID="cmbTipoSesion" runat="server"
                                            DataSourceID="odsTipoSesion" DataTextField="tipo"
                                            DataValueField="id" CssClass="form-control"
                                            OnDataBound="cmbTipoSesion_DataBound">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="odsTipoSesion" runat="server"
                                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                            TypeName="Controlador.dsIUFormacionesTiposActividadesTableAdapters.iu_tipos_formaciones_actividadesTableAdapter">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="1" Name="escoaching" Type="Int16"></asp:Parameter>
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblCoachResponsable" runat="server" Text="Coach"
                                            AssociatedControlID="cmbCoachResponsable"></asp:Label>
                                        <telerik:RadComboBox ID="cmbCoachResponsable" Width="100%"
                                            EmptyMessage="Selección de coach..." runat="server"
                                            Culture="es-ES" DataSourceID="odsCoaches"
                                            MarkFirstMatch="True" Filter="Contains"
                                            DataTextField="formador" DataValueField="id">
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-md-3">
                                        <asp:Label ID="lblFechaInicioActividad" runat="server"
                                            Text="Fecha y hora de inicio"
                                            AssociatedControlID="dtFechaInicioSesion"></asp:Label>
                                        <telerik:RadDateTimePicker ID="dtFechaInicioSesion" runat="server"
                                            Width="100%">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="lblFechaFinActividad" runat="server"
                                            Text="Fecha y hora de fin"
                                            AssociatedControlID="dtFechaFinSesion"></asp:Label>
                                        <telerik:RadDateTimePicker ID="dtFechaFinSesion" runat="server"
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
                                        <asp:TextBox ID="txtInforme" TextMode="MultiLine" Rows="3"
                                             runat="server"
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3" style="margin-top: 30px;">
                                        <asp:LinkButton ID="cmdGuardarNuevaSesion"
                                            CssClass="btn btn-primary btn-block"
                                            runat="server"
                                            OnClick="cmdGuardarNuevaSesion_Click">
                                                    <i class="fas fa-save"></i>&nbsp;&nbsp;Guardar sesión
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
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </asp:Panel>
    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server"
        RelativeTo="Element" Position="MiddleRight" AutoTooltipify="true"
        ContentScrolling="Default" RenderMode="Lightweight">
    </telerik:RadToolTipManager>
</asp:Content>
