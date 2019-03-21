using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Base.Windows.NetStandard.Service
{
    public class WindowsServiceExceptionEventArgs : EventArgs
    {
        public Exception ServiceException { get; set; }
    }

}
