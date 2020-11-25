<%@ Page Title="Sesión de coaching" Language="C#" MasterPageFile="~/MasterPageAuth.master"
    AutoEventWireup="true" CodeFile="sesion.aspx.cs" Inherits="sesion_form" %>
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

            function eliminarSesion(arg) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                if (arg) {
                    ajaxManager.ajaxRequest('eliminarSesion');
                }
            }

            function eliminarValoracion(arg) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                if (arg) {
                    ajaxManager.ajaxRequest('eliminarValoracion');
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <uc1:elementos_telerik runat="server" ID="elementos_telerik" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="usuarioLogueado" />
    <asp:HiddenField ID="hfIDProceso" runat="server" />
    <asp:HiddenField ID="hfIDSesion" runat="server" />
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
            <div class="col-sm-12 mb-4">
                <asp:HyperLink ID="hlVolver2" runat="server"
                    CssClass="btn btn-outline-primary"
                    NavigateUrl="/formaciones.aspx">
                    <i class="fas fa-arrow-left"></i>&nbsp;&nbsp;Volver
                </asp:HyperLink>
            </div>
            <div class="col-sm-12">
                <h1>Sesión: <asp:Literal ID="literalTitulo" runat="server"></asp:Literal></h1>
            </div>
            
        </div>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0"
            MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab Text="Datos principales y coachees" Selected="True"></telerik:RadTab>
                <telerik:RadTab Text="Control de asistencia"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="tabPrincipales" runat="server" Selected="true"
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
                                    <h2>Datos principales y coachees de la sesión</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblTitulo" runat="server" Text="Número de la sesión"
                                                AssociatedControlID="txtTitulo"></asp:Label>
                                            <asp:TextBox ID="txtTitulo" CssClass="form-control"
                                                runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblTipoActividad" runat="server" Text="Tipo"
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
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblFormador" runat="server" Text="Coach"
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
                                        <div class="col-sm-1">
                                            <asp:Label ID="lblNumeroHoras" runat="server" Text="Nº de horas"
                                                AssociatedControlID="txtNumHoras"></asp:Label>
                                            <telerik:RadNumericTextBox ID="txtNumHoras" runat="server" Width="100%"
                                                NumberFormat-DecimalDigits="0">
                                                <NumberFormat DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblColectividadActividad" runat="server"
                                                Text="Tipo de sesión"
                                                AssociatedControlID="rblIndividual"></asp:Label>
                                            <asp:RadioButtonList ID="rblIndividual" runat="server"
                                                RepeatDirection="Vertical">
                                                <asp:ListItem Value="1" Text="Individual"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Colectivo"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="lblNotificacion" runat="server" ToolTip="¿Envía por correo avisos de la sesión?"
                                                Text="Notificaciones"
                                                AssociatedControlID="rblNotificaciones"></asp:Label>
                                            <asp:RadioButtonList ID="rblNotificaciones" runat="server"
                                                RepeatDirection="Vertical" ToolTip="¿Envía por correo avisos de la sesión?">
                                                <asp:ListItem Value="1" Text="Activadas"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Desactivadas"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLugarDeCelebracion" runat="server" Text="Lugar de la sesión" AssociatedControlID="txtLugar"></asp:Label>
                                            <asp:TextBox ID="txtLugar" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblDireccion" runat="server" Text="Dirección" AssociatedControlID="txtDireccion"></asp:Label>
                                            <asp:TextBox ID="txtDireccion" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-md-3">
                                            <asp:Label ID="lblCP" runat="server" Text="Código postal" AssociatedControlID="txtCP"></asp:Label>
                                            <asp:TextBox ID="txtCP" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblPoblacion" runat="server" Text="Población" AssociatedControlID="txtPoblacion"></asp:Label>
                                            <asp:TextBox ID="txtPoblacion" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblProvincia" runat="server" Text="Provincia" AssociatedControlID="txtProvincia"></asp:Label>
                                            <asp:TextBox ID="txtProvincia" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="lblPais" runat="server" Text="Pais" AssociatedControlID="txtPais"></asp:Label>
                                            <asp:TextBox ID="txtPais" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Label ID="lblFechaInicio" runat="server"
                                                        Text="Fecha de inicio" AssociatedControlID="dtFechaInicio"></asp:Label>
                                                    <telerik:RadDateTimePicker ID="dtFechaInicio" runat="server"
                                                        Width="100%">
                                                    </telerik:RadDateTimePicker>
                                                </div>
                                                <div class="col-12" style="margin-top:10px;">
                                                    <asp:Label ID="lblFechaFin" runat="server"
                                                        Text="Fecha final" AssociatedControlID="dtFechaFin"></asp:Label>
                                                    <telerik:RadDateTimePicker ID="dtFechaFin" runat="server"
                                                        Width="100%">
                                                    </telerik:RadDateTimePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-sm-9">
                                                    <asp:Label runat="server" ID="lblListaPersonas"
                                                        AssociatedControlID="listaPersonas"
                                                        Text="Coachees asignadas a la sesión"></asp:Label>
                                                    <telerik:RadListBox ID="listaPersonas" runat="server"
                                                        Width="100%" Culture="es-ES" DataSourceID="odsListaPersonas"
                                                        DataTextField="nombre_apellidos"
                                                        DataValueField="idalumno" CheckBoxes="True" ShowCheckAll="True">
                                                        <Localization CheckAll="Seleccionar todas" />
                                                        <ButtonSettings TransferButtons="All" />
                                                    </telerik:RadListBox>
                                                    <asp:ObjectDataSource ID="odsListaPersonas" runat="server"
                                                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                                        TypeName="Controlador.dsFormacionesActividadesAlumnosTableAdapters.vista_alumnos_asignados_actividadTableAdapter">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="hfIDSesion" PropertyName="Value"
                                                                Name="idactividad" Type="Int64"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </div>
                                                <div class="col-sm-3" style="margin-top: 27px;">
                                                    <asp:LinkButton ID="cmdDesAsignar" runat="server"
                                                        CssClass="btn btn-outline-danger btn-block"
                                                        OnClick="cmdDesAsignar_Click"
                                                        ToolTip="Dar de baja a la persona de la sesión">
                                                        <i class="fas fa-user-times"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-sm-9">
                                                    <asp:Label runat="server" ID="lblPersonas"
                                                        AssociatedControlID="cmbPersonas"
                                                        Text="Coachees disponibles para la sesión"></asp:Label>
                                                    <telerik:RadComboBox ID="cmbPersonas" runat="server"
                                                        Width="100%" CheckBoxes="True" Culture="es-ES" Filter="Contains"
                                                        DataSourceID="odsAlumnosDisponibles"
                                                        DataTextField="nombre_apellidos"
                                                        DataValueField="id">
                                                        <Localization AllItemsCheckedString="Todas los coachees seleccionadas" />
                                                    </telerik:RadComboBox>
                                                    <asp:ObjectDataSource ID="odsAlumnosDisponibles"
                                                        runat="server" OldValuesParameterFormatString="original_{0}"
                                                        SelectMethod="GetData"
                                                        TypeName="Controlador.dsFormacionesActividadesAlumnosTableAdapters.vista_alumnos_disponibles">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="hfIDProceso"
                                                                PropertyName="Value" Name="idformacion"
                                                                Type="Int64"></asp:ControlParameter>
                                                            <asp:ControlParameter ControlID="hfIDSesion" PropertyName="Value" Name="idactividad" Type="Int32"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </div>
                                                <div class="col-sm-3" style="margin-top: 27px;">
                                                    <asp:LinkButton ID="cmdAsignar" runat="server"
                                                        CssClass="btn btn-outline-success btn-block"
                                                        OnClick="cmdAsignar_Click"
                                                        ToolTip="Asignar persona"><i class="fas fa-user-plus"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="panelInformes" runat="server"
                                        CssClass="row" 
                                        style="margin-top: 20px; margin-bottom: 20px">
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblInforme" runat="server" Text="Informe de la sesión"
                                                AssociatedControlID="txtInforme"></asp:Label>
                                            <asp:TextBox ID="txtInforme" CssClass="form-control" runat="server"
                                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblNotasPrivadas" runat="server" Text="Notas privadas"
                                                AssociatedControlID="txtNotasPrivadas" ForeColor="Red"></asp:Label>
                                            <asp:TextBox ID="txtNotasPrivadas" CssClass="form-control" runat="server"
                                                TextMode="MultiLine" Rows="4"></asp:TextBox>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelErrores" runat="server">
                                        <div class="row mt-4 mb-0">
                                            <div class="col-sm-12">
                                                <asp:Panel ID="panelErroresInterior" runat="server">
                                                    <asp:BulletedList ID="blErrores" runat="server"></asp:BulletedList>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <asp:Panel CssClass="card-footer" ID="panelBotonesInferior" runat="server">
                                    <div class="row">
                                        <div class="col-sm-6" style="padding-top: 24px">
                                            <asp:HyperLink ID="hlVolver" runat="server"
                                                CssClass="btn btn-outline-primary"
                                                NavigateUrl="/formaciones.aspx">
                                                <i class="fas fa-arrow-left"></i>&nbsp;&nbsp;Volver
                                            </asp:HyperLink>
                                        </div>
                                        <div class="offset-sm-2 col-sm-2" style="padding-top: 24px">
                                            <asp:Button ID="cmdEliminar" CssClass="btn btn-danger btn-block" runat="server"
                                                OnClick="cmdEliminar_Click" Text="Eliminar"
                                                OnClientClick="confirmar(this,'¿Está seguro de  eliminar la sesión?'); return false;" />
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
            <telerik:RadPageView ID="tabAsistencia" runat="server"
                BorderStyle="Solid" BorderColor="#dedede"
                BorderWidth="1px">
                <asp:Panel runat="server" ID="panel3" class="container-fluid p-3">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header bg-light">
                                    <h2>Asistencia</h2>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <p>Seleccione los Coachees que han asistido a la sesión.</p>
                                            <telerik:RadListBox ID="rlAlumnosFormacionAsistencia" runat="server"
                                                Width="100%" Culture="es-ES" DataSourceID="odsListaPersonas"
                                                DataTextField="nombre_apellidos"
                                                DataValueField="id" CheckBoxes="True" ShowCheckAll="True" AutoPostBack="True" 
                                                OnDataBound="rlAlumnosFormacionAsistencia_DataBound" 
                                                OnItemCheck="rlAlumnosFormacionAsistencia_ItemCheck" 
                                                OnCheckAllCheck="rlAlumnosFormacionAsistencia_CheckAllCheck" 
                                                OnSelectedIndexChanged="rlAlumnosFormacionAsistencia_SelectedIndexChanged">
                                                <Localization CheckAll="Seleccionar todos" />
                                                <ButtonSettings TransferButtons="All" />
                                            </telerik:RadListBox>
                                        </div>
                                    </div>
                                    
                                </div>
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
