using System;
using System.Security.Cryptography;

namespace Helper
{
    public static class Math
    {
        public static Byte[] CalculoCRC(Byte[] Buffer)
        {
            Byte[] ret = new Byte[2];
            int n, b;

            UInt16 CRC_Reg;
            Byte CRC_hi, CRC_lo;

            //step 1
            CRC_Reg = 0xFFFF;

            for (b = 0; b < (Buffer.Length - 2); b++)
            {
                CRC_hi = Convert.ToByte((CRC_Reg & 0xFF00) >> 8);
                CRC_lo = Convert.ToByte(CRC_Reg & 0x00FF);
                //step 2
                CRC_lo = Convert.ToByte(Buffer[b] ^ CRC_lo);
                CRC_Reg = Convert.ToUInt16(CRC_lo + (CRC_hi * 256));
                for (n = 0; n < 8; n++)
                {
                    // step 3
                    CRC_Reg = Convert.ToUInt16(CRC_Reg >> 1);
                    // step 4
                    if (Convert.ToBoolean(CRC_Reg & 0x0001) == true)
                    {
                        CRC_Reg = Convert.ToUInt16(CRC_Reg ^ 0xA001);
                    }
                }
            }

            ret[1] = Convert.ToByte(CRC_Reg & 0x00FF);
            ret[0] = Convert.ToByte((CRC_Reg & 0xFF00) >> 8);


            return ret;
        }
        public static UInt16 CalculoCRC_16(Byte[] Buffer)
        {
            int n, b;

            UInt16 CRC_Reg;
            Byte CRC_hi, CRC_lo;

            //step 1
            CRC_Reg = 0xFFFF;

            for (b = 0; b < (Buffer.Length - 2); b++)
            {
                CRC_hi = Convert.ToByte((CRC_Reg & 0xFF00) >> 8);
                CRC_lo = Convert.ToByte(CRC_Reg & 0x00FF);
                //step 2
                CRC_lo = Convert.ToByte(Buffer[b] ^ CRC_lo);
                CRC_Reg = Convert.ToUInt16(CRC_lo + (CRC_hi * 256));
                for (n = 0; n < 8; n++)
                {
                    // step 3
                    CRC_Reg = Convert.ToUInt16(CRC_Reg >> 1);
                    // step 4
                    if (Convert.ToBoolean(CRC_Reg & 0x0001) == true)
                    {
                        CRC_Reg = Convert.ToUInt16(CRC_Reg ^ 0xA001);
                    }
                }
            }
            return CRC_Reg;
        }

        public static string GetMD5Hash(string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());

            }
            String hash = s.ToString();
            return hash;
        }
    }
}
