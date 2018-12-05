using SDSFoundation.Interfaces.Content.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SDSFoundation.Interfaces.Content.ContentModel.ContentEnums;

namespace SDSFoundation.Interfaces.Content
{
    public interface IContentEvents
    {
        /// <summary>
        /// OnPlayTimeChanged is raised whenever the current time of the content changes
        /// </summary>
        event Action<IContentProperties> OnPlayTimeChanged;

        event Action<IContentProperties> OnContentCategoryChanged;

        event Action<IContentProperties> OnPlayDirectionChanged;

        event Action<IContentProperties> OnPlayStateChanged;

        event Action<IContentProperties> OnPlaySpeedChanged;

        /// <summary>
        /// Return parameters: content properties, file path and name, and total file size if known
        /// </summary>
        event Action<IContentProperties, String, Decimal> OnSaveContentBegin;

        /// <summary>
        /// Return parameters: content properties, file path and name, and total bytes downloaded if known
        /// </summary>
        event Action<IContentProperties, String, Decimal> OnSaveContentStatus;

        /// <summary>
        /// Return parameters: content properties, file path and name, and total bytes downloaded if known
        /// </summary>
        event Action<IContentProperties, String, Decimal> OnSaveContentComplete;

        /// <summary>
        /// Return parameters: content properties, file path and name, failure message, total bytes downloaded if applicable, and an exception if applicable (might be null)
        /// </summary>
        event Action<IContentProperties, String, String, Decimal, _Exception> OnSaveContentFailed;

        event Action<IContentProperties, _Exception> OnContentException;

    }
}
