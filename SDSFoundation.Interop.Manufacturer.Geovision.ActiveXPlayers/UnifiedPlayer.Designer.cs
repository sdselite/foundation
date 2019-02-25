using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX;
using SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers
{
    partial class UnifiedPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnifiedPlayer));
            this.TestControlsGroupBox = new System.Windows.Forms.GroupBox();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.singlePlayer = new SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer.AxGVSinglePlayer();
            this.liveXPlayer = new SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX.LiveXPlayer();
            this.TestControlsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.liveXPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // TestControlsGroupBox
            // 
            this.TestControlsGroupBox.Controls.Add(this.timePicker);
            this.TestControlsGroupBox.Controls.Add(this.datePicker);
            this.TestControlsGroupBox.Controls.Add(this.button1);
            this.TestControlsGroupBox.Controls.Add(this.button3);
            this.TestControlsGroupBox.Controls.Add(this.button2);
            this.TestControlsGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TestControlsGroupBox.Location = new System.Drawing.Point(0, 548);
            this.TestControlsGroupBox.Name = "TestControlsGroupBox";
            this.TestControlsGroupBox.Size = new System.Drawing.Size(1301, 86);
            this.TestControlsGroupBox.TabIndex = 9;
            this.TestControlsGroupBox.TabStop = false;
            // 
            // timePicker
            // 
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(211, 56);
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(118, 22);
            this.timePicker.TabIndex = 12;
            // 
            // datePicker
            // 
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(94, 56);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(111, 22);
            this.datePicker.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 32);
            this.button1.TabIndex = 10;
            this.button1.Text = "Logon";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1182, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 9;
            this.button3.Text = "Pause";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1101, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 8;
            this.button2.Text = "Play";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // singlePlayer
            // 
            this.singlePlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.singlePlayer.ContentSettings = null;
            this.singlePlayer.Enabled = true;
            this.singlePlayer.Location = new System.Drawing.Point(587, 9);
            this.singlePlayer.Margin = new System.Windows.Forms.Padding(0);
            this.singlePlayer.Name = "singlePlayer";
            this.singlePlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("singlePlayer.OcxState")));
            this.singlePlayer.Size = new System.Drawing.Size(320, 240);
            this.singlePlayer.TabIndex = 14;
            // 
            // liveXPlayer
            // 
            this.liveXPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveXPlayer.ContentSettings = null;
            this.liveXPlayer.Enabled = true;
            this.liveXPlayer.Location = new System.Drawing.Point(9, 9);
            this.liveXPlayer.Margin = new System.Windows.Forms.Padding(0);
            this.liveXPlayer.Name = "liveXPlayer";
            this.liveXPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("liveXPlayer.OcxState")));
            this.liveXPlayer.Size = new System.Drawing.Size(538, 281);
            this.liveXPlayer.TabIndex = 15;
            // 
            // UnifiedPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 634);
            this.Controls.Add(this.liveXPlayer);
            this.Controls.Add(this.singlePlayer);
            this.Controls.Add(this.TestControlsGroupBox);
            this.Name = "UnifiedPlayer";
            this.Text = "UnifiedPlayer";
            this.TestControlsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.singlePlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.liveXPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox TestControlsGroupBox;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        public AxGVSinglePlayer singlePlayer;
        private LiveXPlayer liveXPlayer;
    }
}