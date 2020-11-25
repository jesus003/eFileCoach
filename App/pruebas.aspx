<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pruebas.aspx.cs" Inherits="pruebas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="bs/css/bootstrap.min.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="Panel1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap"></telerik:RadAjaxLoadingPanel>
            <asp:Panel ID="Panel1" runat="server">
                <telerik:RadButton ID="cmdPrueba" runat="server" Text="RadButton"
                    OnClick="cmdPrueba_Click">
                </telerik:RadButton>
                <telerik:RadTextBox ID="txtPruebas" runat="server"></telerik:RadTextBox>
                <br />
                <br />
                <br />
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1">
                    <Tabs>
                        <telerik:RadTab Text="Pestaña 1" Selected="True"></telerik:RadTab>
                        <telerik:RadTab Text="Pestaña 2"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
                    <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" BorderStyle="Solid" BorderColor="#dedede"
                        BorderWidth="1px">
                        <div class="container-fluid p-3">
                                Contenido 1
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server" BorderStyle="Solid" BorderColor="#dedede"
                         BorderWidth="1px">
                        <div class="container-fluid p-3">
                                Contenido 2
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </asp:Panel>



        </div>
    </form>
</body>
</html>
