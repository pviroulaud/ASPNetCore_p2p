using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNodo
{
    public class CLI
    {
        public CLI()
        {

        }
        public void printMenu()
        {
            Console.WriteLine("Comandos:");
            Console.WriteLine();

            Console.WriteLine("  help\t\tImprime esta ayuda.");
            Console.WriteLine("  lclientes\t\tImprime los clientes conectados a este nodo.");
            Console.WriteLine("  getPescucha\t\tImprime el puerto de escucha para conexiones entrantes.");
            Console.WriteLine("");
            Console.WriteLine("");
        }
        public void printAbout()
        {
            Console.WriteLine("************* Nodo de red P2P **************");
            Console.WriteLine("Desarrollado por Pablo Daniel Viroulaud 2022.");
        }
    }
}
