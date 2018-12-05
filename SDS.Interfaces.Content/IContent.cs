using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content
{
    public interface IContent<TContentActions, TContentProperties, TContentEvents> 
        where TContentActions: IContentActions 
        where TContentProperties : IContentProperties
        where TContentEvents : IContentEvents
    {

        TContentActions Actions { get; set; }

        TContentProperties Properties { get; set; }

        TContentEvents Events { get; set; }
    }
}
