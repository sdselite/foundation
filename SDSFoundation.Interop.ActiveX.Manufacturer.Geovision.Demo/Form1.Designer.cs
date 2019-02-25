using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX;
using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.Demo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.singlePlayer = new SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer.AxGVSinglePlayer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.singlePlayer2 = new SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer.AxGVSinglePlayer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.liveXPlayer = new SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX.LiveXPlayer();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer2)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveXPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // singlePlayer
            // 
            this.singlePlayer.ContentSettings = null;
            this.singlePlayer.Enabled = true;
            this.singlePlayer.Location = new System.Drawing.Point(3, 3);
            this.singlePlayer.Name = "singlePlayer";
            this.singlePlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("singlePlayer.OcxState")));
            this.singlePlayer.Size = new System.Drawing.Size(320, 240);
            this.singlePlayer.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 591);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "Logon";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1133, 568);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 2;
            this.button2.Text = "Play";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // datePicker
            // 
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(110, 601);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(111, 22);
            this.datePicker.TabIndex = 3;
            this.datePicker.ValueChanged += new System.EventHandler(this.datePicker_ValueChanged);
            // 
            // timePicker
            // 
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(240, 601);
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(118, 22);
            this.timePicker.TabIndex = 4;
            this.timePicker.ValueChanged += new System.EventHandler(this.timePicker_ValueChanged);
            // 
            // singlePlayer2
            // 
            this.singlePlayer2.ContentSettings = null;
            this.singlePlayer2.Enabled = true;
            this.singlePlayer2.Location = new System.Drawing.Point(329, 3);
            this.singlePlayer2.Name = "singlePlayer2";
            this.singlePlayer2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("singlePlayer2.OcxState")));
            this.singlePlayer2.Size = new System.Drawing.Size(320, 240);
            this.singlePlayer2.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.singlePlayer);
            this.flowLayoutPanel1.Controls.Add(this.singlePlayer2);
            this.flowLayoutPanel1.Controls.Add(this.liveXPlayer);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1277, 550);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // liveXPlayer
            // 
            this.liveXPlayer.ContentSettings = null;
            this.liveXPlayer.Enabled = true;
            this.liveXPlayer.Location = new System.Drawing.Point(655, 3);
            this.liveXPlayer.Name = "liveXPlayer";
            this.liveXPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("liveXPlayer.OcxState")));
            this.liveXPlayer.Size = new System.Drawing.Size(610, 463);
            this.liveXPlayer.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1214, 568);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 7;
            this.button3.Text = "Pause";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 634);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer2)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.liveXPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private AxGVSinglePlayer singlePlayer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.DateTimePicker timePicker;
        private AxGVSinglePlayer singlePlayer2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private LiveXPlayer liveXPlayer;
        private System.Windows.Forms.Button button3;
    }
}

