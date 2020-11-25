<%@ Control Language="C#" AutoEventWireup="true" CodeFile="menu.ascx.cs" Inherits="controles_menu" %>
<div class="container-fluid">
    <nav class="navbar navbar-expand-lg navbar-light bg-light fixed-top justify-content-between">
        <a class="navbar-brand" href="/inicio.aspx">
            <img width="100" src="../Images/logo.png" /></a>
        <button class="navbar-toggler" type="button" data-toggle="collapse"
            data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false"
            aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ml-auto">

                <asp:PlaceHolder ID="phCuenta" runat="server">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="navbarUsuarios"
                            data-toggle="dropdown" aria-haspopup="true"
                            aria-expanded="false"><i class="fas fa-users"></i>Usuarios
                        </a>
                        <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                            <a class="dropdown-item" href="/usuarios.aspx"><i class="fas fa-user-friends"></i>Usuarios</a>
                            <a class="dropdown-item" href="/clientes.aspx"><i class="fas fa-address-card"></i>Clientes</a>
                        </div>
                    </li>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phFormaciones" runat="server">
                    <li class="nav-item">
                        <a class="nav-link" href="/formaciones.aspx" id="navbarFormaciones"
                            aria-haspopup="true"
                            aria-expanded="false"><i class="fas fa-chalkboard-teacher"></i>Formaciones
                        </a>
                    </li>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phProcesos" runat="server">
                    <li class="nav-item">
                        <a class="nav-link" href="/procesos.aspx" id="navbarProcesos"
                            aria-haspopup="true"
                            aria-expanded="false"><i class="fas fa-chalkboard-teacher"></i>Procesos
                        </a>
                    </li>
                </asp:PlaceHolder>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarHerramientas"
                        data-toggle="dropdown" aria-haspopup="true"
                        aria-expanded="false"><i class="fas fa-users"></i>Herramientas
                    </a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                        <a class="dropdown-item" href="/evaluaciones"><i class="fas fa-user-friends"></i>Encuestas</a>
                        <a class="dropdown-item" href="/evaluaciones/plantillas.aspx"><i class="fas fa-user-friends"></i>Plantillas</a>
                        <div class="dropdown-divider"></div>
                    </div>
                </li>
                <asp:PlaceHolder ID="phCalendario" runat="server">
                    <li class="nav-item">
                        <a class="nav-link" href="/calendario.aspx" id="navBarCalendario"
                            aria-haspopup="true"
                            aria-expanded="false"><i class="fas fa-chalkboard-teacher"></i>Mi calendario
                        </a>
                    </li>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="phMiCuenta" runat="server">
                    <li class="nav-item">
                        <a href="/mi-cuenta.aspx" class="nav-link"><i class="fas fa-user-circle"></i>&nbsp;Mi cuenta</a>
                    </li>
                </asp:PlaceHolder>

                <li class="nav-item">
                    <asp:LinkButton ID="cmdSalir" runat="server" CssClass="nav-link" OnClick="cmdSalir_Click">
                        <i class="fas fa-sign-out-alt"></i>&nbsp;Cerrar sesion
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </nav>
</div>
