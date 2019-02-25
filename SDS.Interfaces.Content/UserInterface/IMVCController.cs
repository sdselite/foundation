using SDSFoundation.Interfaces.Content.ContentHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SDSFoundation.Interfaces.Content.UserInterface
{
    interface IMVCController<TContent> : IController, IContentHost<TContent>
    where TContent : IContent
    {
        
    }
}
