<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" 
    CodeFile="inicio-cliente.aspx.cs" Inherits="inicio_usuario" %>
<%@ Register Src="~/controles/menu.ascx" TagPrefix="uc1" TagName="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="ContenedorMenu" ContentPlaceHolderID="cphMenu" runat="server">
    <uc1:menu runat="server" ID="menu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
    <div class="container">
        <div class="row" style="text-align: center">
            <div class="col-md-12">
                <p>Zona privada del usuario</p>
            </div>
        </div>
    </div>
</asp:Content>