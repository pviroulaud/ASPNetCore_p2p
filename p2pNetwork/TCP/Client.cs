using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCP
{
    public class Client
    {
        private string idCliente;
        protected Socket SK_Cliente;
        private Thread Th_LecturaCliente;
        private String IP_Srvr;
        private Int32 Puerto_Srvr;
        private Boolean Cliente_Conectado = false;
        private Int32 Tam_BufferRX = 100;
        private Int32 TO_RX = 0;
        public Client()
        {
            idCliente = Guid.NewGuid().ToString();
        }
        public Client(string ID_Cliente)
        {
            idCliente = ID_Cliente;
        }
        public interface I_SK_DatosRX
        {
            void DatosRX(Byte[] Datos, String Datos_str);

        }
        #region " ************** Declaracion de Eventos ************************"
        public delegate void EV_NuevaConexion();
        public event EV_NuevaConexion NuevaConexion;

        public delegate void EV_DatosRecibidos(Byte[] Datos, String Datos_str);
        public event EV_DatosRecibidos DatosRecibidos;

        public delegate void EV_ConexionTerminada();
        public event EV_ConexionTerminada ConexionTerminada;

        public delegate void EV_Error_Servidor(Exception ex);
        public event EV_Error_Servidor Error_Conexion;

        #endregion

        #region " ******************** Propiedades *****************************"

        public Int32 TimeOut_RX
        {
            get
            {
                return TO_RX;
            }
            set
            {
                if (TO_RX >= 0)
                {
                    TO_RX = value;
                    if (SK_Cliente != null)
                    {
                        SK_Cliente.ReceiveTimeout = TO_RX;
                    }
                }
            }
        }
        public Int32 Tamaño_Buffer_Recepcion
        {
            get
            {
                return Tam_BufferRX;
            }
            set
            {
                if (value > 0)
                {
                    Tam_BufferRX = value;
                }
                else
                {
                    Tam_BufferRX = 100;
                }
            }
        }
        public String IP_Servidor
        {
            get
            {
                return IP_Srvr;
            }
            set
            {
                char[] sep = new char[1] { '.' };
                string[] campos;
                try
                {
                    campos = value.Split(sep);
                    if (campos.Length == 4)
                    {
                        if (((Convert.ToInt32(campos[0]) >= 0) && (Convert.ToInt32(campos[0]) <= 255)) &&
                            ((Convert.ToInt32(campos[1]) >= 0) && (Convert.ToInt32(campos[1]) <= 255)) &&
                            ((Convert.ToInt32(campos[2]) >= 0) && (Convert.ToInt32(campos[2]) <= 255)) &&
                            ((Convert.ToInt32(campos[3]) >= 0) && (Convert.ToInt32(campos[3]) <= 255))
                            )
                        {
                            IP_Srvr = value;
                        }
                    }
                }
                catch (Exception ex)
                {

                    IP_Srvr = "127.0.0.1";
                }
            }
        }
        public Int32 Puerto_Servidor
        {
            get
            {
                return Puerto_Srvr;
            }
            set
            {
                Puerto_Srvr = value;
            }
        }
        public Boolean Conectado
        {
            get
            {
                return Cliente_Conectado;
            }
        }
        #endregion

        #region " ********************** Metodos *******************************"
        public void Conectar()
        {
            if (SK_Cliente == null)
            {
                SK_Cliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SK_Cliente.ReceiveTimeout = TO_RX;
            }
            if (!SK_Cliente.Connected)
            {
                if ((IP_Srvr != "") && (Puerto_Srvr > 0))
                {
                    try
                    {
                        SK_Cliente.Connect(IP_Srvr, Puerto_Srvr);
                        Cliente_Conectado = true;
                        if (NuevaConexion != null)
                        {
                            NuevaConexion();
                            Enviar_Datos("->COMANDO:MACaddr=" + GetMacAddress().ToString());
                            Enviar_Datos("->COMANDO:ID=" + idCliente);
                        }

                        Th_LecturaCliente = new Thread(LeerCliente);
                        Th_LecturaCliente.Start();
                    }
                    catch (Exception EX)
                    {
                        if (Error_Conexion != null)
                        {
                            Error_Conexion(EX);
                        }
                    }
                }
            }
        }
        public void Desconectar()
        {
            if (SK_Cliente != null)
            {
                if (SK_Cliente.Connected)
                {
                    Th_LecturaCliente.Abort();
                    SK_Cliente.Close();
                    Cliente_Conectado = false;
                }
            }
        }

        public void Enviar_Datos(Byte[] Datos)
        {
            try
            {
                if (SK_Cliente != null)
                {
                    if (SK_Cliente.Connected == true)
                    {
                        SK_Cliente.Send(Datos);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Error_Conexion != null)
                {
                    Error_Conexion(ex);
                }
            }
        }
        public void Enviar_Datos(String Datos)
        {
            try
            {
                if (SK_Cliente != null)
                {
                    if (SK_Cliente.Connected == true)
                    {
                        SK_Cliente.Send(Encoding.ASCII.GetBytes(Datos));
                    }
                }
            }
            catch (Exception ex)
            {
                if (Error_Conexion != null)
                {
                    Error_Conexion(ex);
                }
            }
        }
        #endregion

        #region " ********************* Funciones ******************************"
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
        private void LeerCliente()
        {
            Byte[] DatosRX;
            Byte[] BufferDatos;
            String DatosRX_str;
            int ret = 0;

            BufferDatos = new Byte[Tam_BufferRX];

            while (true)
            {
                if (SK_Cliente.Connected)
                {
                    Array.Clear(BufferDatos, 0, BufferDatos.Length);
                    try
                    {
                        ret = SK_Cliente.Receive(BufferDatos);
                        if (ret > 0)
                        {
                            DatosRX = new Byte[ret];
                            Array.Copy(BufferDatos, DatosRX, ret);
                            DatosRX_str = Encoding.ASCII.GetString(DatosRX);
                            if (DatosRecibidos != null)
                            {
                                DatosRecibidos(DatosRX, DatosRX_str);
                            }

                        }
                        else
                        {
                            if (ConexionTerminada != null)
                            {
                                ConexionTerminada();
                            }

                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if ((ex.Message != "Subproceso anulado.") &&
                            (ex.Message != "Se ha forzado la interrupción de una conexión existente por el host remoto"))
                        {
                            if (Error_Conexion != null)
                            {
                                Error_Conexion(ex);
                            }

                        }
                        Cliente_Conectado = false;
                        if (ConexionTerminada != null)
                        {
                            ConexionTerminada();
                        }
                        break;
                    }
                }
            }
            //CerrarThread((IPEndPoint)ID_De_Este_Cliente);
            //CerrarConexion((IPEndPoint)ID_De_Este_Cliente);
            if (SK_Cliente.Connected == true)
            {
                SK_Cliente.Close();
            }
            lock (this)
            {
                //SK_Cliente.Shutdown(SocketShutdown.Both);
                SK_Cliente = null;
                Th_LecturaCliente.Abort();
            }
            Cliente_Conectado = false;
        }
        #endregion



    }

}
