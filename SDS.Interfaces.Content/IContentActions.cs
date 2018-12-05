using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDSFoundation.Interfaces.Content.ContentModel.ContentEnums;

namespace SDSFoundation.Interfaces.Content
{
    public interface IContentActions
    {

        /// <summary>
        /// Plays content
        /// If live content it shall play in real time
        /// If content is not live it will play from the last play time if paused OR from the beginning of the content if it is not in a playing state
        /// </summary>
        void Play();

        /// <summary>
        /// Plays the content starting at the selected time
        /// </summary>
        /// <param name="start"></param>
        void Play(DateTime start);


        /// <summary>
        /// Plays the content starting at the selected time and pauses play at the selected end time
        /// If the content is paused it resumes play
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void Play(DateTime start, DateTime end);

        /// <summary>
        /// Reverses the direction of play.  If the content is currently playing it will continue playing from the last time, but in reverse
        /// If the content is not currently playing, it will play in reverse the next time the user plays the content
        /// </summary>
        void ReversePlay();


        /// <summary>
        /// Pauses playing content
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops playing content
        /// </summary>
        void Stop();

        /// <summary>
        /// Increases the speed of the playing content
        /// If the content has the ability to play faster, return true
        /// If the content has reached the maximum playable speed, return false
        /// </summary>
        /// <returns></returns>
        bool PlayFaster();


        /// <summary>
        /// Decreases the speed of the playing content
        /// If the content has the ability to play slower, return true
        /// If the content has reached the minimum playable speed, return false
        /// </summary>
        /// <returns></returns>
        bool PlaySlower();


        /// <summary>
        /// If the content is playing at a speed other than normal, it will reduce the speed to normal and continue play
        /// If the content is paused, it will reduce the speed to normal but will not continue play.  The next time the content is played it will be at the normal speed.
        /// </summary>
        void ResetPlaySpeed();

        /// <summary>
        /// Saves content between (and including) the start and end date as the file name provided
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="fileName">Full path to the file</param>
        /// <param name="includeAudio">Optional parameter.  If false, audio content will be excluded.  Included by default</param>
        /// <returns></returns>
         void SaveContent(DateTime start, DateTime end, String fileName, Boolean includeAudio = true);
    }
}
