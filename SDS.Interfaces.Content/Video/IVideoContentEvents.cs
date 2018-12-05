using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content.Video
{
    public interface IVideoContentEvents : IContentEvents
    {
        /// <summary>
        /// Event raised when the next frame is displayed
        /// </summary>
        event Action<IContentProperties> OnNextFrame;

        /// <summary>
        /// Event raised when the previous frame is displayed
        /// </summary>
        event Action<IContentProperties> OnPreviousFrame;

        /// <summary>
        /// Event raised when audio is enabled
        /// </summary>
        event Action<IContentProperties> OnAudioEnabled;

        /// <summary>
        /// Event raised when audio is disabled
        /// </summary>
        event Action<IContentProperties> OnAudioDisabled;

    }
}
