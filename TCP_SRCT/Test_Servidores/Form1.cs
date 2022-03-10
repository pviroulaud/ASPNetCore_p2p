using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Servidores;

namespace Test_Servidores
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private delegate void SetText(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str);
        private void WriteText(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str)
        {
            richTextBox1.AppendText(ID_Terminal.ToString() + "> "+ Datos_str + "\n");
        }

        private delegate void EventoGeneral(System.Net.IPEndPoint ID_Terminal);
        private void Evento_NuevaConexion(System.Net.IPEndPoint ID_Terminal)
        {
            //MessageBox.Show(ID_Terminal.Address.ToString(),"NUEVA CONEXION");
            string tmp;


            tmp = "Nueva conexion establecida: Cliente: " + ID_Terminal.Address.ToString() +
                                    " Puerto: " + SRV.Puerto_Del_servidor.ToString();
            richTextBox1.AppendText(tmp);
            richTextBox1.Select(richTextBox1.TextLength - tmp.Length, tmp.Length);
            richTextBox1.SelectionBackColor = Color.Green;
            richTextBox1.SelectionColor = Color.White;
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            Listado.Items.Add(ID_Terminal);
        }
        private void Evento_ConexionTerminada(System.Net.IPEndPoint ID_Terminal)
        {
            string tmp;
            //MessageBox.Show(ID_Terminal.Address.ToString(),"CONEXION TERMINADA");

            tmp = "Conexion terminada: Cliente: " + ID_Terminal.Address.ToString() +
                        " Puerto: " + SRV.Puerto_Del_servidor.ToString();
            richTextBox1.AppendText(tmp);
            richTextBox1.Select(richTextBox1.TextLength - tmp.Length, tmp.Length);
            richTextBox1.SelectionBackColor = Color.Red;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            Listado.Items.Remove(ID_Terminal);
        }                


        private delegate void ErrorSRV(Exception ex);
        private void Evento_ErrorServidor(Exception ex)
        {
            string tmp;
            //MessageBox.Show(ex.ToString(),"ERROR SERVIDOR");

            tmp = "Error de Servidor:\n" + ex.ToString();
            richTextBox1.AppendText(tmp);
            richTextBox1.Select(richTextBox1.TextLength - tmp.Length, tmp.Length);
            richTextBox1.SelectionBackColor = Color.Red;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        }


        

        Servidor_TCP SRV = new Servidor_TCP();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            SRV.NuevaConexion += new Servidor_TCP.EV_NuevaConexion(SRV_NuevaConexion);
            SRV.DatosRecibidos += new Servidor_TCP.EV_DatosRecibidos(SRV_DatosRecibidos);
            SRV.ConexionTerminada += new Servidor_TCP.EV_ConexionTerminada(SRV_ConexionTerminada);
            SRV.Error_Servidor += new Servidor_TCP.EV_Error_Servidor(SRV_Error_Servidor);

            SRV.Puerto_Del_servidor = 5000;

            Label_Puerto.Text  = SRV.Puerto_Del_servidor.ToString();
            Label_IP.Text  = SRV.IP_Local;
        }

        void SRV_Error_Servidor(Exception ex)
        {
            this.BeginInvoke(new ErrorSRV(Evento_ErrorServidor), new object[] {ex});
        }

        void SRV_ConexionTerminada(System.Net.IPEndPoint ID_Terminal)
        {
            this.BeginInvoke(new EventoGeneral(Evento_ConexionTerminada), new object[] { ID_Terminal });            
        }
        protected void SRV_DatosRecibidos(System.Net.IPEndPoint ID_Terminal, byte[] Datos, string Datos_str)
        {
            this.BeginInvoke(new SetText(WriteText),new object[] {ID_Terminal,Datos,Datos_str});

            //richTextBox1.AppendText(a); // Esto da un error de subprocesos

        }

        void SRV_NuevaConexion(System.Net.IPEndPoint ID_Terminal)
        {
            this.BeginInvoke(new EventoGeneral(Evento_NuevaConexion), new object[] { ID_Terminal });
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SRV.CerrarTodasLasConexiones();
            SRV.Detener_EscuchaDeConexiones();
            SRV = null;
            GC.Collect();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SRV.Puerto_Del_servidor = Convert.ToInt32(tb_Puerto.Text);
            Label_Puerto.Text = SRV.Puerto_Del_servidor.ToString();
            SRV.EscucharConexiones();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //SRV.CerrarTodasLasConexiones(); 
            SRV.Detener_EscuchaDeConexiones();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
 
            SRV.BroadCast_Datos(tb_Enviar.Text);
            richTextBox1.AppendText(tb_Enviar.Text + "\n");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Menu_Desconectar_Click(object sender, EventArgs e)
        {
            SRV.CerrarConexion((System.Net.IPEndPoint)Listado.SelectedItem);
        }

        private void Menu_INFO_Click(object sender, EventArgs e)
        {
            string tmp;

            tmp = SRV.Obtener_Informacion_Del_Cliente((System.Net.IPEndPoint)Listado.SelectedItem);
            MessageBox.Show(tmp, "INFORMACION");
        }

        private void Menu_Enviar_Click(object sender, EventArgs e)
        {
            SRV.Enviar_Datos((System.Net.IPEndPoint)Listado.SelectedItem, tb_Enviar.Text);
            richTextBox1.AppendText(tb_Enviar.Text + "\n");
        }
    }
}
