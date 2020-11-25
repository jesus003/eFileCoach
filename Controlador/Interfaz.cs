using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Controlador
{
    public static class Interfaz
    {
        public static String ICO_alert = "/ico/atencion.png";
        public static String ICO_ok = "/ico/ok.png";

        public static string GenerarClave(int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static String GetValor(String id)
        {
            try
            {
                dsInterfazTableAdapters.configuracionTableAdapter q = new dsInterfazTableAdapters.configuracionTableAdapter();
                return q.GetDataByClave(id)[0].valor;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        public static Boolean SetValor(String id, String valor)
        {
            try
            {
                dsInterfazTableAdapters.configuracionTableAdapter q =
                    new dsInterfazTableAdapters.configuracionTableAdapter();
                q.Actualizar(valor, id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static String limpiaHTML(String html)
        {
            return Regex.Replace(html, "<.*?>", String.Empty);
        }

        public static Boolean EsEmail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean EsInt64(String texto)
        {
            try
            {
                Int64.Parse(texto);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static Boolean EsFecha(String texto)
        {
            try
            {
                DateTime.Parse(texto);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static Boolean EsNumero(String texto)
        {
            try
            {
                Int32.Parse(texto);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static Boolean EsDecimal(String texto)
        {
            try
            {
                Decimal.Parse(texto);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static void InsertarLog(String mensaje, String ip, String email, String url,
            String pila)
        {
            dsLogsTableAdapters.logsTableAdapter q =
                new dsLogsTableAdapters.logsTableAdapter();

            q.Insertar(mensaje, ip, DateTime.Now, email, url, pila);
        }

        public static void InsertarLog(String mensaje, String ip, String email, String url)
        {
            dsLogsTableAdapters.logsTableAdapter q =
                new dsLogsTableAdapters.logsTableAdapter();

            q.Insertar(mensaje, ip, DateTime.Now, email, url, String.Empty);
        }

    }

    public static class Depuracion
    {
        public static void Registro(String texto)
        {
            try
            {
                dsInterfazTableAdapters.depuracionTableAdapter q =
                    new dsInterfazTableAdapters.depuracionTableAdapter();
                q.Insert(DateTime.Now, texto);
            }
            catch
            {

            }
        }
    }
}
