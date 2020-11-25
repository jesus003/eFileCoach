<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" CodeFile="calendario.aspx.cs" Inherits="calendario" %>

<%@ Register Src="~/controles/UsuarioLogueado.ascx" TagPrefix="uc1" TagName="UsuarioLogueado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" Runat="Server">
    <uc1:UsuarioLogueado runat="server" ID="usuario" />
    <telerik:RadAjaxPanel ID="panelContenido" runat="server" LoadingPanelID="radPanelCarga">

        <%--<div class="row">
            <div class="col-md-12">
                <h2>Calendario</h2>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6 text-right">
                <asp:Button ID="cmdSyncGoogle" runat="server" Text="" CssClass="btn btn-primary" OnClick="cmdSyncGoogle_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <telerik:RadScheduler ID="radCalendario" runat="server" AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" Skin="Silk"
                    Culture="es-ES" FirstDayOfWeek="Monday" HoursPanelTimeFormat="H:mm:ss" 
                    LastDayOfWeek="Sunday" 
                    SelectedView="WeekView" TimeZoneID="W. Europe Standard Time" 
                    DataSourceID="odsCalendarioCuenta" TimeZoneOffset="01:00:00" 
                    DataDescriptionField="descripcion" DataEndField="fecha_fin" 
                    DataKeyField="id" 
                    DataStartField="fecha_inicio" DataSubjectField="titulo"
                    OverflowBehavior="Expand" RowHeight="23px" Height="600px" 
                    OnAppointmentClick="radCalendario_AppointmentClick" 
                    DayStartTime="06:00:00" OnAppointmentDataBound="radCalendario_AppointmentDataBound" DayEndTime="22:00:00">
                    <ExportSettings>
                        <Pdf PageBottomMargin="25.4mm" PageHeight="297mm" PageLeftMargin="25.4mm" 
                            PageRightMargin="25.4mm" PageTopMargin="25.4mm" PageWidth="210mm" PaperSize="A4" />
                    </ExportSettings>
                    <TimelineView NumberOfSlots="7" SlotDuration="5.00:00:00"></TimelineView>

                    <MonthView ColumnHeaderDateFormat="dd" />
                    <Localization HeaderToday="Hoy" HeaderWeek="Semana" HeaderDay="Día" HeaderTimeline="Timeline"
                        HeaderMonth="Mes"
                        Show24Hours="Mostrar 24 horas"
                        ShowBusinessHours="Horario habitual" AdvancedSubject="Asunto" 
                        Save="Guardar" />
                    <AgendaView UserSelectable="True" />
                </telerik:RadScheduler>
                <asp:ObjectDataSource ID="odsCalendarioUsuario" runat="server" OldValuesParameterFormatString="original_{0}" 
                    SelectMethod="GetDataByIDUsuario" TypeName="Controlador.dsCalendarioTableAdapters.vista_calendarioTableAdapter">
                    <SelectParameters>
                        <asp:SessionParameter SessionField="idusuario" Name="idprofesor" Type="Int64"></asp:SessionParameter>
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="odsCalendarioCuenta" runat="server" OldValuesParameterFormatString="original_{0}" 
                    SelectMethod="GetDataByIDCuenta" TypeName="Controlador.dsCalendarioTableAdapters.vista_calendarioTableAdapter">
                    <SelectParameters>
                        <asp:SessionParameter SessionField="idcuenta" Name="idcuenta" Type="Int64"></asp:SessionParameter>
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3" style="padding-top: 24px">
                <asp:HyperLink ID="hlVolver" runat="server"
                    CssClass="btn btn-outline-primary btn-block"
                    NavigateUrl="/inicio.aspx">
                    <i class="fas fa-arrow-left"></i> Volver
                </asp:HyperLink>
            </div>
        </div>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="radPanelCarga" runat="server"
        Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>