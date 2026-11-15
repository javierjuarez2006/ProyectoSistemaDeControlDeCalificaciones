using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaDeControlDeCalificaciones
{
    public partial class frmAutores : MetroFramework.Forms.MetroForm
    {
        public frmAutores()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Cierra este formulario de autores y regresa al login que está oculto (o abre uno nuevo)
            this.Close();
        }
        
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private MetroFramework.Controls.MetroButton btnRegresar;
        private System.Windows.Forms.Label label1;

        private void InitializeComponent()
        {
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.btnRegresar = new MetroFramework.Controls.MetroButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.btnRegresar);
            this.metroPanel1.Controls.Add(this.label5);
            this.metroPanel1.Controls.Add(this.label4);
            this.metroPanel1.Controls.Add(this.label3);
            this.metroPanel1.Controls.Add(this.label2);
            this.metroPanel1.Controls.Add(this.label1);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(23, 63);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(749, 504);
            this.metroPanel1.TabIndex = 1;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // btnRegresar
            // 
            this.btnRegresar.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.btnRegresar.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnRegresar.Location = new System.Drawing.Point(299, 423);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(177, 55);
            this.btnRegresar.TabIndex = 4;
            this.btnRegresar.Text = "REGRESAR AL LOGIN";
            this.btnRegresar.UseSelectable = true;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 24);
            this.label5.TabIndex = 3;
            this.label5.Text = "Diego Enmanuel Umaña Rivas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(336, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Yaquelin Ixela Echeverria Vasquez";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(299, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Adan Enrique Cushco Shunico";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Javier Alejandro Echeverria Juarez";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(266, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(284, 24);
            this.label6.TabIndex = 3;
            this.label6.Text = "INTEGRANTES DEL EQUIPO";
            // 
            // frmAutores
            // 
            this.ClientSize = new System.Drawing.Size(795, 579);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.metroPanel1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "frmAutores";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TransparencyKey = System.Drawing.SystemColors.ButtonFace;
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
