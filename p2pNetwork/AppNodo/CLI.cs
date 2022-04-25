using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNodo
{
    public class CLI
    {
        private string prompt = "p2p>";
        private string comandoSalida = "quit";
        private TCP.Server srvr;
        private TCP.Client cl;
        private List<EntidadesNodo.Peer> peers;
        public CLI(string PromptText, TCP.Server servidor, TCP.Client cliente, List<EntidadesNodo.Peer> peers)
        {
            prompt = PromptText;
            srvr = servidor;
            cl = cliente;
            this.peers = peers;
        }
        
        

        public void initCLI()
        {
            Console.Clear();
            Console.WriteLine("Iniciando consola P2P.");
            Console.Write(prompt);
            string cmd = "";
           
            string fullCmd = "";
            do
            {
                fullCmd = readLine().ToLower();
                string[] splitCmd = fullCmd.Split(new string[] { " " }, StringSplitOptions.None);
                string[] kvp= new string[1];
                if (splitCmd.Length>=1)
                {
                    cmd = splitCmd[0];// readLine().ToLower();
                    switch (cmd)
                    {
                        case "help":
                            printMenu();
                            break;
                        case "about":
                            printAbout();
                            break;
                        case "peers?":
                            foreach (var item in peers)
                            {
                                printLine($"{item.id}\t{item.ipAddr}\t{item.macAddr}\t{item.state.ToString()}");
                            }
                            break;
                        #region servidor
                        case "server":
                            
                            if ((splitCmd.Length>1)&& (srvr != null))
                            {
                                
                                switch (splitCmd[1])
                                {
                                    case "cerrar":
                                        Console.WriteLine($"Cerrando todas las conexiones de este nodo.");
                                        srvr.CerrarTodasLasConexiones();
                                        break;
                                    case "esperar":
                                        Console.WriteLine($"El nodo esta esperando conexiones en el puerto {srvr.Puerto_Del_servidor}.");
                                        srvr.EscucharConexiones();
                                        break;
                                    case "puerto?":                                        
                                        Console.WriteLine($"El nodo esta esperando conexiones en el puerto {srvr.Puerto_Del_servidor}.");
                                        break;
                                    case "puerto":
                                        
                                        if (splitCmd.Length==3)
                                        {
                                            uint prt=0;
                                            try
                                            {
                                                prt = Convert.ToUInt32(splitCmd[2]);
                                            }
                                            catch (Exception)
                                            {
                                                comandoDesconocido();
                                            }
                                            Console.WriteLine("Cerrando conexiones...");
                                            srvr.CerrarTodasLasConexiones();
                                            srvr.Puerto_Del_servidor = (int)prt;
                                            Console.WriteLine("Reasignando puerto...\nEsperando conexiones...");
                                            srvr.EscucharConexiones();
                                        }
                                        else
                                        {
                                            comandoDesconocido();
                                        }
                                        
                                        break;
                                    case "clientes?":
                                        if (srvr == null)
                                        {
                                            Console.WriteLine("No se puede acceder a la información. El nodo no esta esperando conexiones.");
                                        }
                                        foreach (var item in srvr.ListadoDeClientesConectados.Keys)
                                        {
                                            TCP.Server.InformacionDelCliente info = (TCP.Server.InformacionDelCliente)srvr.ListadoDeClientesConectados[item];
                                            Console.WriteLine(info.TH_Cliente.Name);
                                        }
                                        break;
                                    default:
                                        comandoDesconocido();
                                        break;
                                }
                            }
                            else
                            {
                                if (srvr == null)
                                {
                                    Console.WriteLine("No se puede acceder a la información. El nodo no esta esperando conexiones.");
                                }
                                else
                                {
                                    comandoDesconocido();
                                }
                                
                            }
                            break;
                        #endregion

                        #region cliente
                        case "cliente":
                            if ((splitCmd.Length > 1) && (cl != null))
                            {

                                switch (splitCmd[1])
                                {
                                    case "estado?":
                                        Console.WriteLine($"Estado de conexion:{cl.Conectado.ToString()}");
                                        break;
                                    case "desconectar":
                                        Console.WriteLine("Cerrando conexion...");
                                        cl.Desconectar();
                                        break;
                                    case "puerto?":

                                        Console.WriteLine($"El cliente esta utilizando el puerto {cl.Puerto_Servidor}.");
                                        break;
                                    case "puerto":                                        
                                        if (splitCmd.Length == 3)
                                        {
                                            uint prt = 0;
                                            try
                                            {
                                                prt = Convert.ToUInt32(splitCmd[2]);
                                                Console.WriteLine("Cerrando conexion...");
                                                cl.Desconectar();
                                                cl.Puerto_Servidor = (int)prt;
                                                Console.WriteLine("Reasignando puerto...");                                               
                                            }
                                            catch (Exception ex)
                                            {
                                                comandoDesconocido();
                                            }                                            
                                        }
                                        else
                                        {
                                            comandoDesconocido();
                                        }

                                        break;
                                    case "ipservidor?":
                                        Console.WriteLine($"El cliente esta utilizando la ip de servidor {cl.IP_Servidor}.");
                                        break;
                                    case "ipservidor":
                                        
                                        if (splitCmd.Length == 3)
                                        {
                                            try
                                            {
                                                Console.WriteLine("Cerrando conexion...");
                                                cl.Desconectar();
                                                cl.IP_Servidor = splitCmd[2];

                                                Console.WriteLine("Reasignando IP...");                                                
                                            }
                                            catch (Exception)
                                            {
                                                comandoDesconocido();
                                            }
                                        }
                                        else
                                        {
                                            comandoDesconocido();
                                        }

                                        break;
                                    case "conectarp":
                                        
                                        if (splitCmd.Length == 3)
                                        {
                                            uint prt = 0;
                                            try
                                            {
                                                string[] ipPort = splitCmd[2].Split(new string[] { ":" }, StringSplitOptions.None);
                                                if (ipPort.Length==2)
                                                {
                                                    prt = Convert.ToUInt32(ipPort[1]);
                                                    Console.WriteLine("Cerrando conexion...");
                                                    cl.Desconectar();
                                                    cl.Puerto_Servidor = (int)prt;
                                                    cl.IP_Servidor = ipPort[0];

                                                    Console.WriteLine("Reasignando IP...");
                                                    Console.WriteLine($"Reconectando a{ cl.IP_Servidor}:{cl.Puerto_Servidor}.");
                                                    cl.Conectar();
                                                }
                                                else
                                                {
                                                    comandoDesconocido();
                                                }                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                comandoDesconocido();
                                            }
                                        }
                                        else
                                        {
                                            comandoDesconocido();
                                        }

                                        break;
                                    case "conectar":
  
                                        try
                                        {
                                            Console.WriteLine("Cerrando conexion...");
                                            cl.Desconectar();

                                            Console.WriteLine("Reasignando IP...");
                                            Console.WriteLine($"Reconectando a{ cl.IP_Servidor}:{cl.Puerto_Servidor}.");
                                            cl.Conectar();
                                        }
                                        catch (Exception)
                                        {
                                            comandoDesconocido();
                                        }
                                        break;
                                    default:
                                        comandoDesconocido();
                                        break;
                                }
                            }
                            else
                            {
                                if (cl == null)
                                {
                                    Console.WriteLine("No se puede acceder a la información.");
                                }
                                else
                                {
                                    comandoDesconocido();
                                }

                            }
                            break;
                        #endregion
                        default:
                            if (cmd == comandoSalida)
                            {
                                Console.WriteLine("Saliendo...");
                            }
                            else
                            {
                                comandoDesconocido();
                            }
                            break;
                    }
                }
                else
                {
                    comandoDesconocido();
                }
                
                Console.Write(prompt);
            } while (cmd!=comandoSalida);
            Console.WriteLine("Consola P2P cerrada.");
        }


        private void comandoDesconocido()
        {
            Console.WriteLine("Comando no reconocido. Escriba 'help' para ver los comandos disponibles.");
        }
        public void writeLine(string cmd)
        {
            Console.WriteLine(cmd);
            Console.Write(prompt);
        }
        public string readLine(bool setPrompt=false)
        {
            string tmp = Console.ReadLine();
            
            if (setPrompt) Console.Write(prompt);
            return tmp;
        }
        public void printLine(string info, bool setPrompt = false)
        {
            Console.WriteLine();
            Console.WriteLine(info);
            if (setPrompt) Console.Write(prompt);
        }
        public void printMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Comandos:");
            Console.WriteLine();

            Console.WriteLine("  help\t\tImprime esta ayuda.");
            Console.WriteLine();
            Console.WriteLine("  peers?\t\tImprime la informacion de los nodos de la red.");
            Console.WriteLine();
            Console.WriteLine("  server clientes?\t\tImprime los clientes conectados a este nodo.");
            Console.WriteLine("  server puerto?\t\tImprime el puerto de escucha para conexiones entrantes.");
            Console.WriteLine("  server puerto ####\t\tEstablece el puerto de escucha para conexiones entrantes.");
            Console.WriteLine("  server cerrar\t\tCierra todas las conexiones a este nodo.");
            Console.WriteLine("  server esperarr\t\tHabilita la espera de conexiones entrantes a este nodo.");
            Console.WriteLine();
            Console.WriteLine("  cliente puerto?\t\tImprime el puerto del servidor al que se conectará este nodo.");
            Console.WriteLine("  cliente ipservidor?\t\tImprime la direccion IP del servidor al que se conectará este nodo.");
            Console.WriteLine("  cliente puerto ####\t\tEstablece el puerto del servidor al que se conectará este nodo.");
            Console.WriteLine("  cliente ipservidor ###.###.###.###\t\tEstablece la direccion IP del servidor al que se conectará este nodo.");
            Console.WriteLine("  cliente conectarp ###.###.###.###:####\t\tConecta este nodo a la direccion y puerto especificado.");
            Console.WriteLine("  cliente conectar\t\tConecta este nodo a la direccion y puerto que tiene configurado.");
            Console.WriteLine("  cliente estado?\t\tImprime el estado de conexion de este nodo a otro.");
            Console.WriteLine("  cliente desconectar\t\tFinaliza la conexion con el nodo remoto");
            Console.WriteLine("");
            Console.WriteLine($"  {comandoSalida}\t\tSalir");
        }
        public void printAbout()
        {
            Console.WriteLine();
            Console.WriteLine("************* Nodo de red P2P **************");
            Console.WriteLine("Desarrollado por Pablo Daniel Viroulaud 2022.");
            Console.WriteLine("********************************************");
            Console.WriteLine();
        }
    }
}
