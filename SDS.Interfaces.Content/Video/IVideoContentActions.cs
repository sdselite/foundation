using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content.Video
{
    /// <summary>
    /// Actions for past video content
    /// </summary>
    public interface IVideoContentActions : IContentActions
    {

        /// <summary>
        /// Moves to the next frame
        /// </summary>
        void NextFrame();

        /// <summary>
        /// Moves to the previous frame
        /// </summary>
        void PreviousFrame();

        /// <summary>
        /// Enables audio playback with video if the content includes audio
        /// </summary>
        void EnableAudio();

        /// <summary>
        /// Disables audio if the content includes audio
        /// </summary>
        void DisableAudio();


    }
}
