using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntidadesNodo
{
    public class Peer
    {
        public class PeerDataInvalidException : Exception
        {
            public PeerDataInvalidException(string message) : base(message)
            {
            }
        }
        public Peer()
        {

        }
        public Peer(string senderIdentification)
        {
            
            string[] fields = senderIdentification.Split(new string[] { "[*]" }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length == 4)
            {
                foreach (var item in fields)
                {
                    string[] kvp = item.Split(new string[] { "[->]" }, StringSplitOptions.RemoveEmptyEntries);
                    if (kvp.Length != 2)
                    {
                        throw new PeerDataInvalidException("Peer Data Invalid.");
                    }
                    try
                    {
                        switch (kvp[0])
                        {
                            case "MAC":
                                macAddr = kvp[1];
                                break;
                            case "IP":
                                ipAddr = kvp[1];
                                break;
                            case "STATUS":
                                state = Convert.ToBoolean(kvp[1]);
                                break;
                            case "ID":
                                id = kvp[1];
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        throw new PeerDataInvalidException("Peer Data Invalid.");
                    }
                    
                }
            }
            else
            {
                throw new PeerDataInvalidException("Peer Data Invalid.");
            }
        }
        public override string ToString()
        {
            return $"peerInformation[=]MAC[->]{macAddr}[*]IP[->]{ipAddr}[*]ID[->]{id}[*]STATUS[->]{state.ToString()}[*][|]";
        }
        public string id { get; set; }
        public string ipAddr { get; set; }
        public bool state { get; set; }
        public string macAddr { get; set; }

    }

    public class PeerHandler
    {
        public static void savePeersFile(List<Peer> peers, string rutaNombreArchivo="")
        {
            if (rutaNombreArchivo == "")
            {
                rutaNombreArchivo = AppDomain.CurrentDomain.BaseDirectory + "\\peersFile.csv";
            }
            StreamWriter wr = new StreamWriter(rutaNombreArchivo);

            foreach (var item in peers)
            {
                wr.WriteLine($"{item.id};{item.macAddr};{item.ipAddr};{item.state}");
            }
            wr.Close();
            wr = null;
        }

        public static List<Peer> loadPeersFile(string rutaNombreArchivo="")
        {
            if (rutaNombreArchivo=="")
            {
                rutaNombreArchivo = AppDomain.CurrentDomain.BaseDirectory + "\\peersFile.csv";
            }
            if (!File.Exists(rutaNombreArchivo))
            {
                var f=File.Create(rutaNombreArchivo);
                f.Close();
            }
            List<Peer> ret = new List<Peer>();
            StreamReader rd = new StreamReader(rutaNombreArchivo);
            while(!rd.EndOfStream)
            {
                string[] line = rd.ReadLine().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length==4)
                {
                    Peer nP = new Peer() 
                    { 
                        id = line[0],
                        macAddr = line[1],
                        ipAddr = line[2],
                        state = Convert.ToBoolean(line[3]) 
                    };
                    ret.Add(nP);
                }
            }
            rd.Close();
            rd = null;

            return ret;
        }

        public static void updatePeerData(List<Peer> peers, string MAC_Address,Peer newPeerData)
        {            
            var peer = (from p in peers where p.macAddr == MAC_Address select p).FirstOrDefault();
            if (peer!=null)
            {
                int n=peers.IndexOf(peer);
                peers[n] = newPeerData;
            }
            else
            {
                Peer newPeer = new Peer() { id = newPeerData.id, ipAddr = newPeerData.ipAddr, macAddr = newPeerData.macAddr, state = newPeerData.state };
                peers.Add(newPeer);
            }
        }

        public static Peer findPeerByIP(List<Peer> peers,string IP_Address)
        {
            return (from p in peers where p.ipAddr == IP_Address select p).FirstOrDefault();

        }
        public static Peer findPeerByMAC(List<Peer> peers, string MAC_Address)
        {
            return (from p in peers where p.macAddr == MAC_Address select p).FirstOrDefault();

        }
        public static Peer findPeerByID(List<Peer> peers, string ID)
        {
            return (from p in peers where p.id == ID select p).FirstOrDefault();

        }
        public static List<Peer> findPeerByState(List<Peer> peers, bool peerSate)
        {
            return (from p in peers where p.state == peerSate select p).ToList();

        }


    }
}
