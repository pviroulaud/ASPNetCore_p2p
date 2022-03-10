using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Clientes_COMPACT
{
    public class ClienteTCP_COMPACT
    {
        private Socket SK_Cliente;
        private Thread Th_LecturaCliente;
        private String IP_Srvr;
        private Int32 Puerto_Srvr;
        private Boolean Cliente_Conectado = false;
        private Int32 Tam_BufferRX = 100;
        private Int32 TO_RX = 0;
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

        //public Int32 TimeOut_RX
        //{
        //    get
        //    {
        //        return TO_RX;
        //    }
        //    set
        //    {
        //        if (TO_RX >= 0)
        //        {
        //            TO_RX = value;
        //            if (SK_Cliente != null)
        //            {
        //                SK_Cliente.ReceiveTimeout = TO_RX;
        //            }
        //        }
        //    }
        //}
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
                IP_Srvr = value;
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
            IPEndPoint EP;
            Byte[] IP = new Byte[4];
            String[] IPstr = IP_Servidor.Split('.');
            IP[0] = Convert.ToByte(IPstr[0]);
            IP[1] = Convert.ToByte(IPstr[1]);
            IP[2] = Convert.ToByte(IPstr[2]);
            IP[3] = Convert.ToByte(IPstr[3]);
            IPAddress IPadd = new IPAddress(IP);
            if (SK_Cliente == null)
            {
                SK_Cliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //SK_Cliente.ReceiveTimeout = TO_RX;
            }
            if (!SK_Cliente.Connected)
            {
                if ((IP_Srvr != "") && (Puerto_Srvr > 0))
                {
                    try
                    {
                        EP = new IPEndPoint(IPadd, Puerto_Srvr);
                        SK_Cliente.Connect(EP);//IP_Srvr, Puerto_Srvr);
                        Cliente_Conectado = true;
                        NuevaConexion();
                        Th_LecturaCliente = new Thread(LeerCliente);
                        Th_LecturaCliente.Start();
                    }
                    catch (Exception EX)
                    {
                        Error_Conexion(EX);
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
                Error_Conexion(ex);
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
                Error_Conexion(ex);
            }
        }
        #endregion

        #region " ********************* Funciones ******************************"

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
                            DatosRX_str = Encoding.ASCII.GetString(DatosRX,0,DatosRX.Length);

                            DatosRecibidos(DatosRX, DatosRX_str);
                        }
                        else
                        {
                            ConexionTerminada();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if ((ex.Message != "Subproceso anulado.") &&
                            (ex.Message != "Se ha forzado la interrupción de una conexión existente por el host remoto"))
                        {
                            Error_Conexion(ex);
                        }
                        Cliente_Conectado = false;
                        ConexionTerminada();
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
