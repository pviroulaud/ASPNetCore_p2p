using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Cryptography;
namespace Helper
{
    public static class AES
    {  
        /// <summary>
        /// Encripta el texto dado utilizando la clave indicada y el vector de inicializacion Addoc (AES.initializationVector())
        /// </summary>
        /// <param name="clave">clave de encriptacion se utilizan 32 bytes (se completa con * si faltan)</param>
        /// <param name="data">texto a encriptar</param>
        /// <returns>Devuelve un base64String</returns>
        ///
        public static string encryptStringDataToString(string key, string data)
        {
            string archivo = data;

            byte[] clavePlainArr = GenerarVectorDeBytes(key, 32);

            byte[] EncriptadoArr = EncryptStringToBytes_Aes(archivo,
                                                                   clavePlainArr,
                                                                   initializationVector());
            archivo = Encoding.ASCII.GetString(EncriptadoArr);           
            return archivo;
        }
        public static byte[] encryptByteDataToByte(string key, byte[] data)
        {
            return Encoding.ASCII.GetBytes(encryptStringDataToString(key, Encoding.ASCII.GetString(data)));
        }

        public static string encryptStringDataToString(byte[] key, string data)
        {
            string archivo = data;

            byte[] EncriptadoArr = EncryptStringToBytes_Aes(archivo,
                                                                   key,
                                                                   initializationVector());
            archivo = Encoding.ASCII.GetString(EncriptadoArr);
            return archivo;
        }
        public static byte[] encryptByteDataToByte(byte[] key, byte[] data)
        {
            return Encoding.ASCII.GetBytes(encryptStringDataToString(key, Encoding.ASCII.GetString(data)));
        }


        public static string decryptStringDataToString(string encryptedData, string key)
        {
            string data = "";

            byte[] clavePlainArr = GenerarVectorDeBytes(key, 32); // se obtiene la clave en formato byte[]

            byte[] encryptedDataByte = Encoding.ASCII.GetBytes(encryptedData);


            data = DecryptStringFromBytes_Aes(encryptedDataByte,
                                                    clavePlainArr,
                                                    initializationVector());
            return data;
        }
        public static string decryptStringDataToString(string encryptedData, byte[] key)
        {
            string data = "";

            byte[] encryptedDataByte = Encoding.ASCII.GetBytes(encryptedData);


            data = DecryptStringFromBytes_Aes(encryptedDataByte,
                                                    key,
                                                    initializationVector());
            return data;
        }

        public static byte[] decryptByteDataToByte(byte[] encryptedData, string key)
        {
            return Encoding.ASCII.GetBytes(decryptStringDataToString(Encoding.ASCII.GetString(encryptedData), key));
        }

        public static byte[] decryptByteDataToByte(byte[] encryptedData, byte[] key)
        {
            return Encoding.ASCII.GetBytes(decryptStringDataToString(Encoding.ASCII.GetString(encryptedData), key));
        }


        public static byte[] initializationVector()
        {            
            return GenerarVectorDeBytes("AES+InitVector000000000001", 16);
        }


        /// <summary>
        /// Adapta la clave especificada a una longitud de 32 bytes, completando con * los bytes faltantes.
        /// si no se especifica una clave se utiliza una aleatorio.
        /// </summary>
        public static byte[] generateSimetricKey()
        {
            return GenerarVectorDeBytes(new Guid().ToString(), 32);
        }

        /// <summary>
        /// Genera un vector de inicializacion de 16 bytes a partir del texto especificado
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private static byte[] GenerarVectorDeBytes(string texto, int longitud)
        {

            texto = texto.Replace('-', '+');
            texto = texto.Replace('_', '+');
            if (texto.Length < longitud)
            {
                for (int n = 0; n < longitud; n++)
                {
                    texto += "*";
                }
            }
            texto = texto.Substring(0, longitud);
            return System.Text.Encoding.ASCII.GetBytes(texto);
        }


        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con el vector de inicializacion dado y genera una clave aleatoria
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave de 32 bytes</param>
        /// <param name="IV">vector de inicializacion de 16 bytes</param>
        /// <returns></returns>
        private static byte[] EncryptStringToBytes_Aes(string plainText, out byte[] Key, byte[] IV)
        {
            if (IV.Length != 16)
            {
                throw new Exception("La clave debe ser de 32 bits");
            }
            Aes myAes = Aes.Create();
            Key = myAes.Key;


            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }



        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con la clave dada y genera el vector de inicializacion
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave de 32 bytes</param>
        /// <param name="IV">vector de inicializacion</param>
        /// <returns></returns>
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, out byte[] IV)
        {
            if (Key.Length != 32)
            {
                throw new Exception("La clave debe ser de 32 bits");
            }
            Aes myAes = Aes.Create();

            IV = myAes.IV;

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES devuelve la clave y el vector de inicializacion generados de forma aleatoria
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave</param>
        /// <param name="IV">vector de inicializacion </param>
        /// <returns></returns>
        private static byte[] EncryptStringToBytes_Aes(string plainText, out byte[] Key, out byte[] IV)
        {
            Aes myAes = Aes.Create();
            Key = myAes.Key;
            IV = myAes.IV;

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con la clave y el vector de inicializacion dado
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave</param>
        /// <param name="IV">vector de inicializacion (debe ser de longitud Key.Lenght/2)</param>
        /// <returns></returns>
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        /// <summary>
        /// Desencripta el byte[] utilizando la clave y el vector de inicializacion dado.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

    }
}
