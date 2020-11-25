using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Security.Cryptography;

namespace Controlador
{
    public class CryptoAES
    {

        byte[] Key;
        byte[] IV;

        public CryptoAES()
        {
            Rijndael rijndael = Rijndael.Create();

            int keySize = 32;
            int ivSize = 16;

            this.Key = UTF8Encoding.UTF8.GetBytes("obgfe9aWfY3ds7W8_F7Z");
            this.IV = UTF8Encoding.UTF8.GetBytes("k_fr73mleQ");

            Array.Resize(ref Key, keySize);
            Array.Resize(ref IV, ivSize);
        }

        /// <summary>
        /// Método que cifra un texto
        /// </summary>
        /// <param name="plainMessage">Texto plano</param>
        /// <param name="byurl">Indica si necesitamos un texto para pasar por una URL (quita los caracteres raros)</param>
        /// <returns></returns>
        public string cifrar(String plainMessage, Boolean byurl)
        {
            try
            {
                // Crear una instancia del algoritmo de Rijndael

                Rijndael RijndaelAlg = Rijndael.Create();

                // Establecer un flujo en memoria para el cifrado

                MemoryStream memoryStream = new MemoryStream();

                // Crear un flujo de cifrado basado en el flujo de los datos

                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             RijndaelAlg.CreateEncryptor(Key, IV),
                                                             CryptoStreamMode.Write);

                // Obtener la representación en bytes de la información a cifrar

                byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);

                // Cifrar los datos enviándolos al flujo de cifrado

                cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);

                cryptoStream.FlushFinalBlock();

                // Obtener los datos datos cifrados como un arreglo de bytes

                byte[] cipherMessageBytes = memoryStream.ToArray();

                // Cerrar los flujos utilizados

                memoryStream.Close();
                cryptoStream.Close();

                // Retornar la representación de texto de los datos cifrados
                if (byurl)
                {
                    return HttpServerUtility.UrlTokenEncode(cipherMessageBytes);

                }
                else
                {
                    return Convert.ToBase64String(cipherMessageBytes);
                }
            }
            catch
            {
                return String.Empty;
            }
        }


        /// <summary>
        /// Descifra una cadena texto con el algoritmo de Rijndael
        /// </summary>
        /// <param name="encryptedMessage"></param>
        /// <param name="byurl"></param>
        /// <returns></returns>
        public string descifrar(String encryptedMessage, Boolean byurl)
        {
            try
            {
                byte[] cipherTextBytes;
                // Obtener la representación en bytes del texto cifrado
                if (byurl)
                {
                    cipherTextBytes = HttpServerUtility.UrlTokenDecode(encryptedMessage);
                }
                else
                {
                    cipherTextBytes = Convert.FromBase64String(encryptedMessage);
                }

                // Crear un arreglo de bytes para almacenar los datos descifrados

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Crear una instancia del algoritmo de Rijndael

                Rijndael RijndaelAlg = Rijndael.Create();

                // Crear un flujo en memoria con la representación de bytes de la información cifrada

                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Crear un flujo de descifrado basado en el flujo de los datos

                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             RijndaelAlg.CreateDecryptor(Key, IV),
                                                             CryptoStreamMode.Read);

                // Obtener los datos descifrados obteniéndolos del flujo de descifrado

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Cerrar los flujos utilizados

                memoryStream.Close();
                cryptoStream.Close();

                // Retornar la representación de texto de los datos descifrados

                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
