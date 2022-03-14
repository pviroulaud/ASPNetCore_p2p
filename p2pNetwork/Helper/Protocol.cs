using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{

    /// <summary>
    /// Funciones para analizar el protocolo de comunicacion. Posee dos bytes de encabezado,una region de datos, un byte de pie y dos bytes de CRC
    /// 0XFE 0XFE [bytes de datos] 0x0D [CRC16_high][CRC16_low]
    /// </summary>
    public static class Protocol
    {
        private static byte[] header = new byte[2] { 0xFE, 0xFE };
        private static byte[] footer = new byte[1] { 0x0D };
        /// <summary>
        /// Verifica el calculo del CRC contra los ultimos dos bytes del mensaje
        /// </summary>
        /// <param name="message">El mensaje debe poseer como minimo 4 bytes (dos de datos y dos de CRC) </param>
        /// <returns></returns>
        public static bool checkCRC16(byte[] message)
        {
            if (message.Length>=4) // Para el calculo del CRC16 
            {
                byte[] m= new byte[message.Length - 2];
                Array.Copy(message, m, message.Length - 2);
                byte[] crc = Math.CalculoCRC(m);
                return ((crc[0] == message[message.Length - 1]) && (crc[1] == message[message.Length]));
            }
            return false;
        }

        /// <summary>
        /// Verifica que el encabezado del mensaje sea 0xFE;0XFE;.....        
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool checkHeader(byte[] message)
        {
            if (message.Length >= 4) 
            {               
                return ((message[0] == header[0]) && (message[1] == header[1]));
            }
            return false;
        }

        /// <summary>
        /// Verifica que el encabezado del mensaje sea 0x0D;.....
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool checkFooter(byte[] message)
        {
            if (message.Length > 4) // Para el calculo del CRC16 
            {
                return (message[message.Length - 2] == 0x0D) ;
            }
            return false;
        }


        /// <summary>
        /// Construye el mensaje agregando el encabezado, el pie y calculando el crc16
        /// </summary>
        /// <param name="data">Datos del mensaje</param>
        /// <returns></returns>
        public static byte[] buildMessage(byte[] data)
        {
            byte[] m = new byte[header.Length + data.Length + footer.Length];
            byte[] ret = new byte[header.Length + data.Length + footer.Length + 2];
            Array.Copy(header, m, header.Length);
            Array.Copy(data, m, data.Length);
            Array.Copy(footer, m, footer.Length);
            byte[] crc = Helper.Math.CalculoCRC(m);
        
            Array.Copy(m, ret, m.Length);
            Array.Copy(crc, ret, crc.Length);

            return ret;
        }
        /// <summary>
        /// Construye el mensaje agregando el encabezado, el pie y calculando el crc16
        /// </summary>
        /// <param name="data">Datos del mensaje</param>
        /// <returns></returns>
        public static byte[] buildMessage(string data)
        {
            return buildMessage(Encoding.ASCII.GetBytes(data));
        }

        public static byte[] readMessageData(byte[] message)
        {
            byte[] ret=null;
            if (checkHeader(message) && checkFooter(message))
            {
                ret = new byte[message.Length-header.Length-footer.Length-2];
                Array.ConstrainedCopy(message, header.Length, ret, 0, ret.Length);
            }
            return ret;
        }
    }

    /// <summary>
    /// Clase para el manejo de los comandos por los que estara compuesta la region de datos de los mensajes.
    /// </summary>
    public static class ProtocolMessage
    {
        public static Dictionary<string,string> getCommands(string messageData)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] comandos = messageData.Split(new string[] { "[|]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in comandos)
            {
                string[] kvp = item.Split(new string[] { "[=]" }, StringSplitOptions.None);
                if (kvp.Length==2)
                {
                    ret.Add(kvp[0].Trim(), kvp[1].Trim());
                }
            }

            return ret;
        }
        public static Dictionary<string, string> getCommands(byte[] messageData)
        {
            return getCommands(Encoding.ASCII.GetString(messageData));
        }
        public static string senderIdentification(string MacAddress,string IPAddress,string ID,bool Status)
        {
            return $"senderIdentification[=]MAC[->]{MacAddress}[*]IP[->]{IPAddress}[*]ID[->]{ID}[*]STATUS[->]{Status.ToString()}[*][|]";
        }
        public static string peerInformation(string MacAddress, string IPAddress, string ID, bool Status)
        {
            return $"peerInformation[=]MAC[->]{MacAddress}[*]IP[->]{IPAddress}[*]ID[->]{ID}[*]STATUS[->]{Status.ToString()}[*][|]";
        }
        public static string macAddress(string value)
        {            
            return $"MACaddress[=]{value}[|]";
        }
        public static string peerId(string value)
        {
            return $"peerId[=]{value}[|]";
        }
        public static string ipAddress(string value)
        {
            return $"IPaddress[=]{value}[|]";
        }
        public static string peerStatus(string value)
        {
            return $"peerStatus[=]{value}[|]";
        }
        public static string dataFile(string fileName,string base64Content)
        {
            return $"dataFile[=]fileName[->]{fileName}[*]base64[->]{base64Content}[|]";
        }
    }
}
