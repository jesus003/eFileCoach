using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Controlador
{
    public static class Validador
    {
        public static Boolean EsEmail(String text)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(text, expresion))
            {
                if (Regex.Replace(text, expresion, String.Empty).Length == 0)
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
        public static Boolean EsFecha(String fecha)
        {

            DateTime fechaAux;

            if (DateTime.TryParse(fecha, out fechaAux))
            {
                return true;
            }
            return false;
        }

        public static Boolean EsInt64(String texto)
        {
            try
            {
                Int64.Parse(texto);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
