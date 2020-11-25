using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace Controlador
{
    static public class Cifrado
    {
        static public string cifrarParaUrl(string strText)
        {
            CryptoAES c = new CryptoAES();
            return c.cifrar(strText, true);
        }

        static public string cifrar(string strText)
        {
            CryptoAES c = new CryptoAES();
            return c.cifrar(strText, false);
        }
        public static string descifrar(string strText, Boolean desdeurl)
        {
            try
            {
                CryptoAES c = new CryptoAES();
                return c.descifrar(strText, desdeurl);
            }
            catch (Exception exc)
            {
                return String.Empty;
            }
        }
    }
}
