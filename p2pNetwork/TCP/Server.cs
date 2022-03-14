using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP
{
    public class Server
    {
        public struct InformacionDelCliente
        {
            public Socket SK_Cliente;
            public Thread TH_Cliente;
            public byte[] UltimosDatos;
            public string MACAddr;
            public string ID;
        }
        // declaro el escuchador de TCP que esperara a los clientes 
        private TcpListener TCP_Listener;
        // declaro la tabla que guardara los datos de los clientes conectados
        private Hashtable ListadoDeClientes = new Hashtable();
        // declaro el thread que esperará a los clientes
        private Thread Thread_TCPListener;
        // delcaro el endpoint del cliente actual
        private IPEndPoint ID_ClienteActual;



        #region " ************** Declaracion de Eventos ************************"   
        public delegate void EV_NuevaConexion(IPEndPoint ID_Terminal);
        public event EV_NuevaConexion NuevaConexion;

        public delegate void EV_DatosRecibidos(IPEndPoint ID_Terminal, Byte[] Datos, String Datos_str);
        public event EV_DatosRecibidos DatosRecibidos;

        public delegate void EV_ConexionTerminada(IPEndPoint ID_Terminal);
        public event EV_ConexionTerminada ConexionTerminada;

        public delegate void EV_Error_Servidor(Exception ex);
        public event EV_Error_Servidor Error_Servidor;

        #endregion

        #region " ******************** Propiedades *****************************"
        private Int32 TamBuff_RX = 100;
        public Int32 TamañoBuffer_Recepcion
        {
            get
            {
                return TamBuff_RX;
            }
            set
            {
                if (value > 0)
                {
                    TamBuff_RX = value;
                }
            }
        }
        private Int32 Puerto;
        public Int32 Puerto_Del_servidor
        {
            get
            {
                return Puerto;
            }
            set
            {
                if ((value > 0) & (value < 99999))
                {
                    Puerto = value;
                }
            }
        }

        private String IPLocal;
        public String IP_Local
        {
            get
            {
                string hostname = Dns.GetHostName();

                IPAddress[] estehost = Dns.GetHostAddresses(hostname);
                IPLocal = estehost[1].ToString();

                return IPLocal;
            }
        }

        private String MACLocal;
        public String MAC_Local 
        { 
            get
            { 
                return MACLocal; 
            }
            set
            {
                MACLocal = GetMacAddress().ToString();
            }
        }

        private Boolean Escuchando;
        public Boolean Esperando_Conexiones
        {
            get
            {
                return Escuchando;
            }
        }

        public Hashtable ListadoDeClientesConectados
        {
            get
            {
                return ListadoDeClientes;
            }
        }
        #endregion


        #region " ********************** Metodos *******************************"
        public String Obtener_Informacion_Del_Cliente(IPEndPoint ID_Del_Cliente)
        {
            String tmp;
            String[] I_P = new String[2];
            int n;
            InformacionDelCliente EsteCliente = (InformacionDelCliente)(ListadoDeClientes[ID_Del_Cliente]); ;
            char[] sep = new char[1] { ':' };
            I_P = EsteCliente.SK_Cliente.RemoteEndPoint.ToString().Split(sep);

            tmp = "IP: " + I_P[0] + "\n" +
                  "Protocolo: " + EsteCliente.SK_Cliente.ProtocolType.ToString() + "\n";
            // + "DNS: " + Dns.GetHostByAddress(I_P[0]);

            return tmp;
        }

        public void EscucharConexiones()
        {
            if (Escuchando == false)
            {
                Escuchando = true;
                Thread_TCPListener = new Thread(EsperarClientes);
                Thread_TCPListener.Name = "TCP Listener";
                Thread_TCPListener.Start();
            }
        }

        public void Detener_EscuchaDeConexiones()
        {
            if (Escuchando == true)
            {
                Escuchando = false;
                TCP_Listener.Stop();
                Thread_TCPListener.Abort();
                Thread_TCPListener = null;

                GC.Collect();

                CerrarTodasLasConexiones();
            }
        }

        public void CerrarConexion(IPEndPoint ID_Del_Cliente)
        {
            InformacionDelCliente EsteCliente;
            try
            {
                EsteCliente = (InformacionDelCliente)ListadoDeClientes[ID_Del_Cliente];
                //EsteCliente.SK_Cliente.Close();
                CerrarThread((IPEndPoint)EsteCliente.SK_Cliente.RemoteEndPoint);
                ListadoDeClientes.Remove(ID_Del_Cliente);
                if (ConexionTerminada != null)
                {
                    ConexionTerminada((IPEndPoint)ID_Del_Cliente);
                }

            }
            catch (Exception ex)
            {
                if (Error_Servidor != null)
                {
                    Error_Servidor(ex);
                }
            }
        }

        public void CerrarTodasLasConexiones()
        {
            foreach (InformacionDelCliente EsteCliente in ListadoDeClientes.Values)
            {
                CerrarThread((IPEndPoint)EsteCliente.SK_Cliente.RemoteEndPoint);
            }
            ListadoDeClientes.Clear();
        }

        public void Enviar_Datos(IPEndPoint ID_Del_Cliente, Byte[] Datos)
        {
            InformacionDelCliente EsteCliente;
            try
            {
                EsteCliente = (InformacionDelCliente)(ListadoDeClientes[ID_Del_Cliente]);
                EsteCliente.SK_Cliente.Send(Datos);
            }
            catch (Exception ex)
            {
                if (Error_Servidor != null)
                {
                    Error_Servidor(ex);
                }
            }
        }
        public void Enviar_Datos(IPEndPoint ID_Del_Cliente, String Datos)
        {
            InformacionDelCliente EsteCliente;
            try
            {
                EsteCliente = (InformacionDelCliente)(ListadoDeClientes[ID_Del_Cliente]);
                EsteCliente.SK_Cliente.Send(Encoding.ASCII.GetBytes(Datos));
            }
            catch (Exception ex)
            {
                if (Error_Servidor != null)
                {
                    Error_Servidor(ex);
                }
            }
        }
        public void BroadCast_Datos(Byte[] Datos)
        {
            foreach (InformacionDelCliente EsteCliente in ListadoDeClientes.Values)
            {
                Enviar_Datos((IPEndPoint)EsteCliente.SK_Cliente.RemoteEndPoint, Datos);
            }
        }
        public void BroadCast_Datos(String Datos)
        {
            foreach (InformacionDelCliente EsteCliente in ListadoDeClientes.Values)
            {
                Enviar_Datos((IPEndPoint)EsteCliente.SK_Cliente.RemoteEndPoint, Datos);
            }
        }
        #endregion

        #region " ********************* Funciones ******************************"
        private void EsperarClientes()
        {
            InformacionDelCliente ClienteActual = new InformacionDelCliente();
            try
            {
                TCP_Listener = new TcpListener(IPAddress.Any, Puerto);
                TCP_Listener.Start();
                while (Escuchando == true)
                {
                    ClienteActual.SK_Cliente = TCP_Listener.AcceptSocket();
                    ID_ClienteActual = (IPEndPoint)ClienteActual.SK_Cliente.RemoteEndPoint;
                    ClienteActual.TH_Cliente = new Thread(LeerSocket);
                    ClienteActual.TH_Cliente.Name = ((IPEndPoint)ID_ClienteActual).Address.ToString();

                    lock (this)
                    {
                        ListadoDeClientes.Add(ID_ClienteActual, ClienteActual);
                    }
                    if (NuevaConexion != null)
                    {
                        NuevaConexion((IPEndPoint)ID_ClienteActual);
                    }

                    ClienteActual.TH_Cliente.Start();
                }
            }
            catch (Exception ex)
            {
                //if (ex == System.Threading.ThreadAbortException)
                //{
                TCP_Listener.Stop();
                TCP_Listener = null;
                GC.Collect();
                //}
            }
        }
        private void LeerSocket()
        {
            IPEndPoint ID_De_Este_Cliente;
            Byte[] DatosRX;
            Byte[] BufferDatos;
            String DatosRX_str;
            int ret = 0;
            InformacionDelCliente EsteCliente;

            ID_De_Este_Cliente = ID_ClienteActual;
            EsteCliente = (InformacionDelCliente)ListadoDeClientes[ID_De_Este_Cliente];
            BufferDatos = new Byte[TamBuff_RX];

            EsteCliente.UltimosDatos = new Byte[TamBuff_RX];

            while (true)
            {
                if (EsteCliente.SK_Cliente.Connected)
                {
                    Array.Clear(BufferDatos, 0, BufferDatos.Length);
                    try
                    {
                        ret = EsteCliente.SK_Cliente.Receive(BufferDatos);
                        if (ret > 0)
                        {
                            DatosRX = new Byte[ret];
                            Array.Copy(BufferDatos, DatosRX, ret);
                            EsteCliente.UltimosDatos = DatosRX;
                            DatosRX_str = Encoding.ASCII.GetString(DatosRX);
                            if (DatosRecibidos != null)
                            {
                                if (DatosRX_str.StartsWith("->COMANDO:"))
                                {
                                    string[] cmd = DatosRX_str.Replace("->COMANDO:", "").Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                                    switch (cmd[0])
                                    {
                                        case "MACaddr":
                                            EsteCliente.MACAddr = cmd[1];
                                            break;
                                        case "ID":
                                            EsteCliente.ID = cmd[1];
                                            break;
                                    }
                                }
                                else
                                {
                                    DatosRecibidos((IPEndPoint)ID_De_Este_Cliente, DatosRX, DatosRX_str);
                                }

                            }
                        }
                        else
                        {
                            if (ConexionTerminada != null)
                            {
                                ConexionTerminada((IPEndPoint)ID_De_Este_Cliente);
                            }

                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if ((ex.Message != "Subproceso anulado.") &&
                            (ex.Message != "Se ha forzado la interrupción de una conexión existente por el host remoto"))
                        {
                            if (Error_Servidor != null)
                            {
                                Error_Servidor(ex);
                            }

                        }
                        if (ConexionTerminada != null)
                        {
                            ConexionTerminada((IPEndPoint)ID_De_Este_Cliente);
                        }
                        break;
                    }
                }
            }
            //CerrarThread((IPEndPoint)ID_De_Este_Cliente);
            //CerrarConexion((IPEndPoint)ID_De_Este_Cliente);
            if (EsteCliente.SK_Cliente.Connected == true)
            {
                EsteCliente.SK_Cliente.Close();
            }
            lock (this)
            {
                ListadoDeClientes.Remove(ID_De_Este_Cliente);
            }
        }

        private void CerrarThread(IPEndPoint ID_Del_Cliente)
        {
            InformacionDelCliente EsteCliente;
            EsteCliente = (InformacionDelCliente)ListadoDeClientes[ID_Del_Cliente];

            try
            {
                if (EsteCliente.SK_Cliente.Connected == true)
                {
                    EsteCliente.SK_Cliente.Close();
                }
                EsteCliente.TH_Cliente.Abort();

            }
            catch (Exception ex)
            {
                if (Error_Servidor != null)
                {
                    Error_Servidor(ex);
                }
            }
        }

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
        #endregion
    }
}
