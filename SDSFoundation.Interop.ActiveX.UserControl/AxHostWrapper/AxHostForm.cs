using SDSFoundation.Interfaces.Interop.ActiveX.Forms;
using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;
using System;
using System.Windows.Forms;

namespace SDSFoundation.Interop.ActiveX.UserControl.Wrapper.AxHostWrapper
{
    public partial class AxHostForm<TActiveXControl, TActiveXControlInterface> : Form, IActiveXHiddenForm<TActiveXControl, TActiveXControlInterface>
         where TActiveXControl : IAxControl, new()
         where TActiveXControlInterface : IAxControl
    {
        public AxHostForm()
        {
            this.InitializeComponent();
        }


        private TActiveXControlInterface activeXControl;
        public TActiveXControlInterface ActiveXControl
        {
            get
            {
                return activeXControl;
            }
            set { activeXControl = value; }
        }


    }
}
