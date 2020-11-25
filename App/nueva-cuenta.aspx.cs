using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Controlador;
using System.Net;
using System.Web.Security;

public partial class nueva_cuenta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Interfaz.InsertarLog("Entramos nueva-cuenta.aspx",
                            Request.UserHostAddress, "", "nueva-cuenta.aspx");
            String formulario = String.Empty;
            foreach (string name in Request.Form.AllKeys)
            {
                string value = Request.Form[name];
                formulario += name + "=" + value + "; ";
            }
            Interfaz.InsertarLog("Contenido del formulario: " + formulario,
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
            if (Request.Form["email"] != null)
            {
                if (Request.Form["efilekey"] == "7CEF3D6C-6A56-4199-BE7C-57031DE86A4B")
                {
                    Cuentas cuenta = new Cuentas();
                    String email = Request.Form["email"].ToString().Trim().ToLower();

                    if (cuenta.EmailEnUso(email))
                    {
                        Interfaz.InsertarLog("Email " + Request.Form["email"].ToString().Trim().ToLower()
                                + " en uso",
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                    else
                    {
                        if (Request.Form["telefono"] == null)
                        {
                            Interfaz.InsertarLog("telefono es null", "", "", "");
                        }
                        else
                        {
                            Interfaz.InsertarLog("telefono es:" + Request.Form["telefono"], "", "", "");
                        }

                        String clave = Interfaz.GenerarClave(10);

                        Int64 last_id = cuenta.Insertar(Int64.Parse(Request.Form["tipo"]),
                            email,
                            Interfaz.GetMD5Hash(clave),
                            Request.Form["nombre"].ToString(),
                            Request.Form["apellidos"].ToString(),
                            Request.Form["telefono"].ToString(),
                            Request.Form["movil"].ToString(),
                            Request.Form["direccion"].ToString(),
                            Request.Form["cod_postal"].ToString(),
                            Request.Form["poblacion"].ToString(),
                            Request.Form["provincia"].ToString(),
                            Request.Form["nif"].ToString(),
                            String.Empty, DateTime.Now,
                            Request.UserHostAddress, null, 1, String.Empty, String.Empty,
                            String.Empty, String.Empty, 0, 1, String.Empty);
                        if (last_id != -1)
                        {
                            //Vamos a notificar
                            String asunto = Interfaz.GetValor("email_nueva_cuenta_asunto");
                            String cuerpo = Interfaz.GetValor("email_nueva_cuenta_body");
                            cuerpo = cuerpo.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");


                            cuerpo = cuerpo.Replace("[clave]", clave);
                            cuerpo = cuerpo.Replace("[usuario]", email);
                            if (Email.sendEmail(Interfaz.GetValor("email_altas"), email, asunto, cuerpo, String.Empty))
                            {
                                Interfaz.InsertarLog("Email de nueva cuenta para " + Request.Form["email"].ToString().Trim().ToLower()
                                + " enviado con éxito",
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
                            }
                            else
                            {
                                Interfaz.InsertarLog("Error al enviar el email de nueva cuenta para " + Request.Form["email"].ToString().Trim().ToLower(),
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
                            }

                            Interfaz.InsertarLog("Cuenta " + Request.Form["email"].ToString().Trim().ToLower()
                                + " creada con éxito",
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                        }
                        else
                        {
                            Interfaz.InsertarLog("Error al crear el usuario " + Request.Form["email"],
                                Request.UserHostAddress, "", "nueva-cuenta.aspx");
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        }
                    }
                }
                else
                {
                    Interfaz.InsertarLog("Correo token incorrecto", Request.UserHostAddress, "", "nueva-cuenta.aspx");
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                }

            }
        }
        catch (Exception ex)
        {
            Interfaz.InsertarLog("Error: " + ex.Message, Request.UserHostAddress, "", "nueva-cuenta.aspx");
        }
        
    }
}