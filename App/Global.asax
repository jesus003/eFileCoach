<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Código que se ejecuta al iniciarse la aplicación

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Código que se ejecuta al cerrarse la aplicación

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Código que se ejecuta cuando se produce un error sin procesar
        try
        {
            Exception excepcion = Server.GetLastError();
            Controlador.Interfaz.InsertarLog("Error detectado: " + excepcion.InnerException.Message,
            Request.UserHostAddress, "", "Request.UrlReferrer.OriginalString", excepcion.InnerException.StackTrace);

            if (excepcion.InnerException.Message.Contains("no establecida como instancia de un objeto"))
            {
                String url = Request.Url.PathAndQuery;
                Session.Clear();
                Response.Redirect("/default.aspx?w=" + HttpUtility.HtmlEncode(url));
            }
        }
        catch
        {

        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Código que se ejecuta al iniciarse una nueva sesión

    }

    void Session_End(object sender, EventArgs e)
    {
        // Código que se ejecuta cuando finaliza una sesión. 
        // Nota: el evento Session_End se produce solamente con el modo sessionstate
        // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer
        // o SQLServer, el evento no se produce.

    }

</script>
