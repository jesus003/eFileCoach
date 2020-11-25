<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAuth.master" AutoEventWireup="true" 
    CodeFile="inicio.aspx.cs" Inherits="inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="Server">
     <div class="container">
        <div class="row" style="text-align: center">
            <div class="offset-md-3 col-md-3">
                <a href="usuarios.aspx">
                    <img src="/Images/usuarios.png" alt="Usuarios" /><br />
                </a>
                <asp:HyperLink ID="cmdUsuarios" PostBackUrl="usuarios" NavigateUrl="usuarios.aspx"
                        CssClass="btn btn-outline-secondary btn-block mt-3" runat="server">Usuarios</asp:HyperLink>
            </div>
            <div class="col-md-3">
                <a href="clientes.aspx">
                    <img src="/Images/clientes.png" alt="Usuarios Finales" /><br />
                    <asp:HyperLink ID="cmdUsuariosFinales" PostBackUrl="usuarios-finales" NavigateUrl="clientes.aspx"
                         CssClass="btn btn-outline-secondary btn-block mt-3" runat="server">Clientes</asp:HyperLink>
                </a>
            </div>
        </div>
    </div>
</asp:Content>