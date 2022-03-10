using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Servidores;
using Clientes;
using System.Net;
using System.IO;

namespace consoleNodeApp
{
    class Program
    {
        private static Servidor_TCP srvr;
        private static Cliente_TCP cl;
        private static bool cerrarApp = false;

        private static Dictionary<string, string> tablaPeers;

        private static void printMenu()
        {
            Console.WriteLine("Seleccione una opcion:");
            Console.WriteLine("[L]istar nodos conectados");
            Console.WriteLine("[C]errar todas las conexiones");
            Console.WriteLine("[M]enu");
            Console.WriteLine("[B]orrar consola");
            Console.WriteLine("[T]ransmitir a todos (Broadcast a todos los nodos)");
            Console.WriteLine("[E]nviar datos a un nodo.");
            Console.WriteLine();
            Console.WriteLine("[S]alir");
        }
        static void Main(string[] args)
        {
            cl = new Cliente_TCP();
            cl.NuevaConexion += Cl_NuevaConexion; 
            cl.ConexionTerminada += Cl_ConexionTerminada;
            cl.Error_Conexion += Cl_Error_Conexion;
            cl.DatosRecibidos += Cl_DatosRecibidos;
            
            srvr = new Servidor_TCP();
            srvr.NuevaConexion += Srvr_NuevaConexion;
            srvr.ConexionTerminada += Srvr_ConexionTerminada;
            srvr.Error_Servidor += Srvr_Error_Servidor;
            srvr.DatosRecibidos += Srvr_DatosRecibidos;

            srvr.Puerto_Del_servidor = 5000;
            srvr.EscucharConexiones();
            Console.WriteLine($"Esperando conexiones en puerto: {srvr.Puerto_Del_servidor}");

            printMenu();
            string opc = "";
            while ((opc!="Q")&&(!cerrarApp))
            {
                opc = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                switch(opc)
                {
                    case "L":
                        Console.WriteLine($"Listado de clientes conectados a este Nodo ({srvr.ListadoDeClientesConectados.Values.Count}):");
                        foreach (Servidor_TCP.InformacionDelCliente Cliente in srvr.ListadoDeClientesConectados.Values)
                        {
                            Console.WriteLine(Cliente.TH_Cliente.Name + "\t" + Cliente.MACAddr + "\t" + Cliente.ID);
                        }
                        break;
                    case "B":
                        Console.Clear();
                        break;
                    case "C":
                        Console.WriteLine($"Cerrando {srvr.ListadoDeClientesConectados.Values.Count} conexiones...");
                        srvr.CerrarTodasLasConexiones();
                        break;
                    case "T":
                        Console.WriteLine("Ingrese el mensaje que desea enviar y presione <ENTER>.");
                        Console.WriteLine("Si desea cancelar esta accion escriba 'cancelar' y presione <ENTER>");
                        string msg = Console.ReadLine();
                        if (msg!="cancelar")
                        {
                            srvr.BroadCast_Datos(msg);
                        }
                        
                        break;
                    case "E":
                        Console.WriteLine("Ingrese el ID del nodo y presione <ENTER>.");
                        string id = Console.ReadLine();
                        Console.WriteLine("Ingrese el mensaje que desea enviar y presione <ENTER>.");
                        string m = Console.ReadLine();
                        foreach (Servidor_TCP.InformacionDelCliente Cliente in srvr.ListadoDeClientesConectados.Values)
                        {
                            if (Cliente.ID==id)
                            {
                                srvr.Enviar_Datos((IPEndPoint)Cliente.SK_Cliente.RemoteEndPoint, m);
                            }
                        }
                        break;
                    case "M":
                    default:
                        printMenu();
                        break;
                }
            }


            srvr.CerrarTodasLasConexiones();
            srvr.Detener_EscuchaDeConexiones();
            srvr = null;

        }





        private static void Cl_DatosRecibidos(byte[] Datos, string Datos_str)
        {
            Console.WriteLine($"Datos recibidos desde el nodo {cl.IP_Servidor}:{cl.Puerto_Servidor} -> {Datos_str}");
            

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
            Console.WriteLine($"Nueva Conexion Con {cl.IP_Servidor}:{cl.Puerto_Servidor}");
        }









        private static void Srvr_DatosRecibidos(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str)
        {
            Console.WriteLine($"Datos recibidos desde {ID_Terminal.Address.ToString()} : {Datos_str}" );
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
            Console.WriteLine("Nueva Conexion Entrante: "+ ID_Terminal.Address.ToString());
        }
    }
}
