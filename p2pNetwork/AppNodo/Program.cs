using EntidadesNodo;
using System;
using System.Collections.Generic;
using TCP;

namespace AppNodo
{
    class Program
    {
        private static List<Peer> peers;

        private static Server srvr;
        private static Client cl;
        private static bool cerrarApp = false;

        static void Main(string[] args)
        {


            peers = PeerHandler.loadPeersFile();


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

            srvr.Puerto_Del_servidor = 5000;
            srvr.EscucharConexiones();
            Console.WriteLine($"Esperando conexiones en puerto: {srvr.Puerto_Del_servidor}");

            while (!cerrarApp) ;


        }



        private static void Cl_DatosRecibidos(byte[] Datos, string Datos_str)
        {

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

        }

        private static void Cl_NuevaConexion()
        {

        }









        private static void Srvr_DatosRecibidos(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str)
        {
            Console.WriteLine($"Datos recibidos desde {ID_Terminal.Address.ToString()} : {Datos_str}");
            // Se reciben los datos del cliente que se incorpora a la red, se agrega/actualiza la tabla de peers
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
