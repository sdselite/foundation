using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;

namespace SDSFoundation.Interfaces.Interop.ActiveX.Forms
{
    public interface IActiveXHiddenForm<TActiveXControl, TActiveXControlInterface>
    where TActiveXControl : IAxControl
    where TActiveXControlInterface : IAxControl
    {
        TActiveXControlInterface ActiveXControl { get; set; }
    }

}
