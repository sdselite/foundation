using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content.Video
{
    public interface IVideoContent : IContent, IVideoContentActions, IVideoContentEvents
    {
        IVideoContent Player { get; }
    }
}
