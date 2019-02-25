using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSFoundation.Interfaces.Interop.ActiveX.UserControl
{

    public interface IAxControl : ISupportInitialize, ICustomTypeDescriptor, IDropTarget, ISynchronizeInvoke, IBindableComponent, IComponent, IDisposable
    {

        event EventHandler HandleCreated;

        event EventHandler HandleDestroyed;

        bool Created { get; }

        void CreateControlHandle();

        void CreateControl();

    }
}
