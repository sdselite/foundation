using SDSFoundation.Interfaces.Interop.ActiveX.Forms;
using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSFoundation.Interop.ActiveX.UserControl.Wrapper.AxHostWrapper
{
    public partial class AxHostForm<TActiveXControl, TActiveXControlInterface> : Form, IActiveXHiddenForm<TActiveXControl, TActiveXControlInterface>
         where TActiveXControl : IAxControl, new()
         where TActiveXControlInterface : IAxControl
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
            //this.ActiveXControl = new TActiveXControl();
            //this.ActiveXControl.CreateControl();
            this.SuspendLayout();
            // 
            // singlePlayerUserControl1
            // 
            //this.ActiveXControl.Location = new System.Drawing.Point(12, 3);
            ////this.ActiveXControl.Visible = false;
            //this.ActiveXControl.Name = "singlePlayerUserControl1";
            //this.ActiveXControl.Size = new System.Drawing.Size(299, 205);
            //this.ActiveXControl.TabIndex = 0;
            // 
            // AxHostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Shown += AxHostForm_Shown;

            //this.Controls.Add(this.ActiveXControl);
            this.Name = "AxHostForm";
            this.Text = "AxHostForm";
            this.ResumeLayout(false);

        }

        private void AxHostForm_Shown(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        #endregion


    }
}