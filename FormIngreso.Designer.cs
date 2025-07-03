namespace UNFit
{
    partial class FormIngreso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIngreso));
            this.txtCedula = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBuscar = new Guna.UI2.WinForms.Guna2Button();
            this.btnRegresar = new Guna.UI2.WinForms.Guna2Button();
            this.btnRenovar = new Guna.UI2.WinForms.Guna2Button();
            this.lblResumenSocio = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtCedula
            // 
            this.txtCedula.Location = new System.Drawing.Point(62, 117);
            this.txtCedula.Name = "txtCedula";
            this.txtCedula.Size = new System.Drawing.Size(129, 20);
            this.txtCedula.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MV Boli", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Snow;
            this.label1.Location = new System.Drawing.Point(86, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cédula";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("MV Boli", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Snow;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 63);
            this.label2.TabIndex = 4;
            this.label2.Text = "UNFit";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Animated = true;
            this.btnBuscar.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnBuscar.BorderRadius = 2;
            this.btnBuscar.BorderThickness = 2;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBuscar.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBuscar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBuscar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBuscar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnBuscar.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(62, 143);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(129, 45);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnRegresar
            // 
            this.btnRegresar.Animated = true;
            this.btnRegresar.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRegresar.BorderRadius = 2;
            this.btnRegresar.BorderThickness = 2;
            this.btnRegresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegresar.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRegresar.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRegresar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRegresar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRegresar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnRegresar.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegresar.ForeColor = System.Drawing.Color.White;
            this.btnRegresar.Location = new System.Drawing.Point(62, 194);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(129, 45);
            this.btnRegresar.TabIndex = 7;
            this.btnRegresar.Text = "⬅ Regresar";
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click_1);
            // 
            // btnRenovar
            // 
            this.btnRenovar.Animated = true;
            this.btnRenovar.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRenovar.BorderRadius = 2;
            this.btnRenovar.BorderThickness = 2;
            this.btnRenovar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRenovar.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRenovar.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRenovar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRenovar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRenovar.Enabled = false;
            this.btnRenovar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnRenovar.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenovar.ForeColor = System.Drawing.Color.White;
            this.btnRenovar.Location = new System.Drawing.Point(62, 245);
            this.btnRenovar.Name = "btnRenovar";
            this.btnRenovar.Size = new System.Drawing.Size(129, 45);
            this.btnRenovar.TabIndex = 8;
            this.btnRenovar.Text = "Renovar";
            this.btnRenovar.Click += new System.EventHandler(this.btnRenovar_Click);
            // 
            // lblResumenSocio
            // 
            this.lblResumenSocio.BackColor = System.Drawing.Color.Transparent;
            this.lblResumenSocio.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblResumenSocio.Font = new System.Drawing.Font("MV Boli", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenSocio.ForeColor = System.Drawing.Color.Snow;
            this.lblResumenSocio.Location = new System.Drawing.Point(236, 49);
            this.lblResumenSocio.Name = "lblResumenSocio";
            this.lblResumenSocio.Size = new System.Drawing.Size(500, 361);
            this.lblResumenSocio.TabIndex = 9;
            // 
            // FormIngreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblResumenSocio);
            this.Controls.Add(this.btnRenovar);
            this.Controls.Add(this.btnRegresar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCedula);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormIngreso";
            this.Text = "Ingreso";
            this.Load += new System.EventHandler(this.FormIngreso_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCedula;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnBuscar;
        private Guna.UI2.WinForms.Guna2Button btnRegresar;
        private Guna.UI2.WinForms.Guna2Button btnRenovar;
        private System.Windows.Forms.Label lblResumenSocio;
    }
}