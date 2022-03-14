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
        public CLI(string PromptText, TCP.Server servidor, TCP.Client cliente)
        {
            prompt = PromptText;
            srvr = servidor;
            cl = cliente;
        }
        
        

        public void initCLI()
        {
            Console.Clear();
            Console.WriteLine("Iniciando consola P2P.");
            Console.Write(prompt);
            string cmd = "";
            do
            {
                
                cmd = readLine().ToLower();
                switch (cmd)
                {
                    case "help":
                        printMenu();
                        break;
                    case "about":
                        printAbout();
                            break;
                    case "getclientes":
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
                    case "getpescucha":
                        if (srvr==null)
                        {
                            Console.WriteLine("No se puede acceder a la información. El nodo no esta esperando conexiones.");
                        }
                        Console.WriteLine($"El nodo esta esperando conexiones en el puerto {srvr.Puerto_Del_servidor}.");
                        break;
                    default:
                        if (cmd == comandoSalida)
                        {
                            Console.WriteLine("Saliendo...");
                        }
                        else
                        {
                            Console.WriteLine("Comando no reconocido. Escriba 'help' para ver los comandos disponibles.");
                        }
                        break;
                }
                Console.Write(prompt);
            } while (cmd!=comandoSalida);
            Console.WriteLine("Consola P2P cerrada.");
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
            Console.WriteLine("  getclientes\t\tImprime los clientes conectados a este nodo.");
            Console.WriteLine("  getPescucha\t\tImprime el puerto de escucha para conexiones entrantes.");
            Console.WriteLine("");
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
