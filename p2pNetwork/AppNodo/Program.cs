using EntidadesNodo;
using System;
using System.Collections.Generic;

using TCP;
using Helper;

namespace AppNodo
{
    class Program
    {
        private static List<Peer> peers;
        private static Peer thisPeer;

        private static Server srvr;
        private static Client cl;
        private static bool cerrarApp = false;

        static void Main(string[] args)
        {
            cl = new Client();
            cl.NuevaConexion += Cl_NuevaConexion;
            cl.ConexionTerminada += Cl_ConexionTerminada;
            cl.Error_Conexion += Cl_Error_Conexion;
            cl.DatosRecibidos += Cl_DatosRecibidos;

            srvr = new Server();
            srvr.NuevaConexion += Srvr_NuevaConexion;
            srvr.ConexionTerminada += Srvr_ConexionTerminada;
            srvr.Error_Servidor += Srvr_Error_Servidor;
            srvr.DatosRecibidos += Srvr_DatosRecibidos;

            thisPeer = new Peer()
            {
                ipAddr = srvr.IP_Local,
                macAddr = srvr.MAC_Local,
                id = new Guid().ToString(),
                state = true
            };
            peers = PeerHandler.loadPeersFile();
            PeerHandler.updatePeerData(peers, thisPeer.macAddr, thisPeer);


            srvr.Puerto_Del_servidor = 5000;
            srvr.EscucharConexiones();
            Console.WriteLine($"Esperando conexiones en puerto: {srvr.Puerto_Del_servidor}");

            while (!cerrarApp) ;


        }



        private static void Cl_DatosRecibidos(byte[] Datos, string Datos_str)
        {
            Console.WriteLine($"Datos recibidos desde {cl.IP_Servidor}:{cl.Puerto_Servidor} : {Datos_str}");
        }

        private static void Cl_Error_Conexion(Exception ex)
        {
            Console.WriteLine($"Error en la Conexion {ex.Message}");
            Console.WriteLine($"Conexion Terminada Con {cl.IP_Servidor}:{cl.Puerto_Servidor}");

            cerrarApp = true;
            Console.WriteLine();
            Console.WriteLine("Presione <ENTER> para terminar...");
        }

        private static void Cl_ConexionTerminada()
        {
            Console.WriteLine($"Conexion Terminada Con {cl.IP_Servidor}:{cl.Puerto_Servidor}");

        }

        private static void Cl_NuevaConexion()
        {
            Console.WriteLine($"Conexion establecida con {cl.IP_Servidor}:{cl.Puerto_Servidor}");
            cl.Enviar_Datos(Protocol.buildMessage(thisPeer.ToString()));
        }









        private static void Srvr_DatosRecibidos(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str)
        {
            Console.WriteLine($"Datos recibidos desde {ID_Terminal.Address.ToString()} : {Datos_str}");
            // Se reciben los datos del cliente que se incorpora a la red, se agrega/actualiza la tabla de peers
            if (Protocol.checkCRC16(Datos))
            {
                byte[] msg=Protocol.readMessageData(Datos);
                if (msg!=null)
                {
                   
                    Dictionary<string,string> cmd= ProtocolMessage.getCommands(msg);
                    if (cmd.ContainsKey("senderIdentification"))
                    {
                        try
                        {
                            Peer nodo = new Peer(cmd["senderIdentification"]); // Se crea el objeto en base a la informacion del que esta enviando

                            PeerHandler.updatePeerData(peers, nodo.macAddr, nodo);

                            foreach (var item in cmd.Keys)
                            {
                                switch (item)
                                {
                                    case "peerInformation": // Informacion para actualizar los datos de un nodo
                                        Peer nodeInfo = new Peer(cmd["peerInformation"]);
                                        PeerHandler.updatePeerData(peers, nodeInfo.macAddr, nodeInfo);
                                        break;
                                    case "dataFile":

                                        break;
                                }
                            }
                        }
                        catch (Peer.PeerDataInvalidException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        
                    }
                    
                }
            }
        }

        

        private static void Srvr_Error_Servidor(Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            srvr.CerrarTodasLasConexiones();
            srvr.Detener_EscuchaDeConexiones();
            srvr = null;

            cerrarApp = true;
            Console.WriteLine();
            Console.WriteLine("Presione <ENTER> para terminar...");

        }

        private static void Srvr_ConexionTerminada(System.Net.IPEndPoint ID_Terminal)
        {
            Console.WriteLine("Conexion Terminada: " + ID_Terminal.Address.ToString());
        }

        private static void Srvr_NuevaConexion(System.Net.IPEndPoint ID_Terminal)
        {
            Console.WriteLine("Nueva Conexion Entrante: " + ID_Terminal.Address.ToString());
        }

    }
}
