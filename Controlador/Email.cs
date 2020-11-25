using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public static class Email
    {
        public static Boolean sendEmail(String remitente, String destinatario,
            String asunto, String contenido, String ip)
        {
            try
            {
                remitente = "mail@efilecoach.com";
                MailMessage mensaje = new MailMessage(remitente, destinatario);
                mensaje.IsBodyHtml = true;
                mensaje.Subject = asunto;
                mensaje.Body = contenido;
                mensaje.BodyEncoding = System.Text.Encoding.GetEncoding(1252);
                SmtpClient cliente_smtp;
                cliente_smtp = new SmtpClient("smtp.ionos.es");
                cliente_smtp.Credentials = new System.Net.NetworkCredential("mail@efilecoach.com",
                    "wV1MtTZqTYwn");
                cliente_smtp.EnableSsl = true;
                cliente_smtp.Port = 587;
                cliente_smtp.Send(mensaje);

                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, ip, String.Empty, String.Empty);
                return false;
            }
        }

        public static Boolean sendEmailCuenta(String remitente, String destinatario,
            String asunto, String contenido, String ip, Cuentas cuenta)
        {
            try
            {
                if (cuenta.GetIDTipo()==1)
                {
                    //Cuenta gratuita, envío genérico
                    return Email.sendEmail(remitente, destinatario, asunto, contenido, ip);
                }
                else
                {
                    if (cuenta.GetSMTPServidor()==String.Empty)
                    {
                        return Email.sendEmail(remitente, destinatario, asunto, contenido, ip);
                    }
                    else
                    {
                        MailMessage mensaje = new MailMessage(cuenta.GetSMTPFromTxt(), destinatario);
                        mensaje.IsBodyHtml = true;
                        mensaje.Subject = asunto;
                        mensaje.Body = contenido;
                        mensaje.BodyEncoding = System.Text.Encoding.GetEncoding(1252);
                        SmtpClient cliente_smtp;
                        cliente_smtp = new SmtpClient(cuenta.GetSMTPServidor());
                        cliente_smtp.Credentials = new System.Net.NetworkCredential(cuenta.GetSMTPUsuario(),
                            cuenta.GetSMTPClave());
                        cliente_smtp.EnableSsl = Convert.ToBoolean(cuenta.GetSMTPSSL());
                        cliente_smtp.Port = cuenta.GetSMTPPuerto();
                        cliente_smtp.Send(mensaje);
                        return true;
                    }
                }
                
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, ip, String.Empty, String.Empty);
                return false;
            }
        }
    }
}
