using SDSFoundation.Interfaces.Content.ContentHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SDSFoundation.Interfaces.Content.UserInterface
{

    // IContentHost<TContent> 
        
    interface IWPFUserControl<TUserControl, TContent>  : IContentHost<TContent>
    where TUserControl : UIElement
    where TContent : IContent<IContentActions, IContentProperties, IContentEvents>
    {

    }
}
