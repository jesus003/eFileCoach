<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gredos.aspx.cs" Inherits="gredos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblEvento" runat="server" Text="ID evento" AssociatedControlID="txtIDEvento"></asp:Label>
        <asp:TextBox ID="txtIDEvento" runat="server"></asp:TextBox><br />
        <asp:Label ID="lblTituloEvento" runat="server" Text="Título del evento" AssociatedControlID="txtTituloEvento"></asp:Label>
        <asp:TextBox ID="txtTituloEvento" runat="server"></asp:TextBox><br /><br />
        <asp:Button ID="cmdActualizar" runat="server" Text="Cargar eventos de hoy" OnClick="cmdActualizar_Click" /><br /><br />
        <asp:Button ID="cmdBorrar" runat="server" Text="Borrar" OnClick="cmdBorrar_Click" /><br /><br />
        <asp:Button ID="cmdNuevo" runat="server" Text="Nuevo evento" OnClick="cmdNuevo_Click" /><br /><br />
        <asp:Button ID="cmdEditar" runat="server" Text="Editar" OnClick="cmdEditar_Click" /><br /><br />
        <asp:Button ID="cmdCargarEvento" runat="server" Text="Cargar evento" OnClick="cmdCargarEvento_Click" />
        <br />
        <br />
        <asp:Literal ID="literalEventos" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
