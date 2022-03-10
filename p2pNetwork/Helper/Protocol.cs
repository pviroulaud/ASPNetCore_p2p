using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Protocol
    {
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
    }
}
