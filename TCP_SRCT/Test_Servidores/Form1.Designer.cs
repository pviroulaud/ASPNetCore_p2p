namespace Test_Servidores
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Listado = new System.Windows.Forms.ListBox();
            this.Menu_Listado = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Menu_Desconectar = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Enviar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_INFO = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_Enviar = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_IP = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_Puerto = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Puerto = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Menu_Listado.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Listado);
            this.groupBox1.Location = new System.Drawing.Point(439, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 304);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clientes Conectados";
            // 
            // Listado
            // 
            this.Listado.ContextMenuStrip = this.Menu_Listado;
            this.Listado.FormattingEnabled = true;
            this.Listado.Location = new System.Drawing.Point(6, 13);
            this.Listado.Name = "Listado";
            this.Listado.Size = new System.Drawing.Size(141, 277);
            this.Listado.TabIndex = 5;
            // 
            // Menu_Listado
            // 
            this.Menu_Listado.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Desconectar,
            this.Menu_Enviar,
            this.toolStripSeparator1,
            this.Menu_INFO});
            this.Menu_Listado.Name = "Menu_Listado";
            this.Menu_Listado.Size = new System.Drawing.Size(199, 76);
            // 
            // Menu_Desconectar
            // 
            this.Menu_Desconectar.Name = "Menu_Desconectar";
            this.Menu_Desconectar.Size = new System.Drawing.Size(198, 22);
            this.Menu_Desconectar.Text = "Desconectar";
            this.Menu_Desconectar.Click += new System.EventHandler(this.Menu_Desconectar_Click);
            // 
            // Menu_Enviar
            // 
            this.Menu_Enviar.Name = "Menu_Enviar";
            this.Menu_Enviar.Size = new System.Drawing.Size(198, 22);
            this.Menu_Enviar.Text = "Enviar";
            this.Menu_Enviar.Click += new System.EventHandler(this.Menu_Enviar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // Menu_INFO
            // 
            this.Menu_INFO.Name = "Menu_INFO";
            this.Menu_INFO.Size = new System.Drawing.Size(198, 22);
            this.Menu_INFO.Text = "Informacion del Cliente";
            this.Menu_INFO.Click += new System.EventHandler(this.Menu_INFO_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_Enviar);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Location = new System.Drawing.Point(7, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(426, 231);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Historial de Mensajes";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tb_Enviar
            // 
            this.tb_Enviar.Location = new System.Drawing.Point(6, 200);
            this.tb_Enviar.Name = "tb_Enviar";
            this.tb_Enviar.Size = new System.Drawing.Size(295, 20);
            this.tb_Enviar.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(306, 200);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 19);
            this.button3.TabIndex = 4;
            this.button3.Text = "Broadcast";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(414, 169);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.Label_IP,
            this.toolStripStatusLabel2,
            this.Label_Puerto});
            this.statusStrip1.Location = new System.Drawing.Point(0, 324);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(597, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel1.Text = "IP Local:";
            // 
            // Label_IP
            // 
            this.Label_IP.AutoSize = false;
            this.Label_IP.Name = "Label_IP";
            this.Label_IP.Size = new System.Drawing.Size(80, 17);
            this.Label_IP.Text = "127.0.0.1";
            this.Label_IP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel2.Text = "Puerto";
            // 
            // Label_Puerto
            // 
            this.Label_Puerto.AutoSize = false;
            this.Label_Puerto.Name = "Label_Puerto";
            this.Label_Puerto.Size = new System.Drawing.Size(45, 17);
            this.Label_Puerto.Text = "5000";
            this.Label_Puerto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tb_Puerto);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(7, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 67);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Conexion";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Puerto";
            // 
            // tb_Puerto
            // 
            this.tb_Puerto.Location = new System.Drawing.Point(18, 32);
            this.tb_Puerto.Name = "tb_Puerto";
            this.tb_Puerto.Size = new System.Drawing.Size(110, 20);
            this.tb_Puerto.TabIndex = 6;
            this.tb_Puerto.Text = "5000";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(306, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cerrar Conexiones";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Esperar Conexiones";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 346);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(613, 384);
            this.MinimumSize = new System.Drawing.Size(613, 384);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.Menu_Listado.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox Listado;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel Label_IP;
        private System.Windows.Forms.TextBox tb_Enviar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel Label_Puerto;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Puerto;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip Menu_Listado;
        private System.Windows.Forms.ToolStripMenuItem Menu_Desconectar;
        private System.Windows.Forms.ToolStripMenuItem Menu_Enviar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_INFO;
    }
}

