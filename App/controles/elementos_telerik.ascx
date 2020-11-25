<%@ Control Language="C#" AutoEventWireup="true" CodeFile="elementos_telerik.ascx.cs" Inherits="controles_elementos_telerik" %>
<telerik:RadWindowManager runat="server" RenderMode="Lightweight">
    <Localization OK="Aceptar" Cancel="Cancelar" />
</telerik:RadWindowManager>
<telerik:RadCodeBlock ID="codigo" runat="server">
    <script type="text/javascript">
        (function (global, undefined) {
            function confirmar(button, contenido) {
                function aspButtonCallbackFn(arg) {
                    if (arg) {
                        __doPostBack(button.name, "");
                        if (Telerik.Web.Browser.ff) {
                            button.click();
                        }
                    }
                }
                radconfirm(contenido, aspButtonCallbackFn, 330, 180, null, "efileCoach", "/ico/question.png");
            }
            global.confirmar = confirmar;
        })(window);
        (function (global, undefined) {
            function confirmarb(button, contenido) {
                function aspButtonCallbackFn(arg) {
                    if (arg) {
                        __doPostBack(button.name, "");
                        if (Telerik.Web.Browser.ff) {
                            button.click();
                        }
                    }
                }
                radconfirm(contenido, aspButtonCallbackFn, null, null, null, "efileCoach", "/ico/borrar.png");
            }
            global.confirmarb = confirmarb;
        })(window);
    </script>
</telerik:RadCodeBlock>
