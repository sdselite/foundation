using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content.ContentModel
{

    public class ContentEnums
    {

        public enum PlayState : int
        {
            Playing = 1,
            Paused = 2,
            Stopped = 3
        };

        public enum PlayDirection : int
        {
            Forward = 1,
            Reverse = 2
        }

        public enum ContentCategory : int
        {
            /// <summary>
            /// Live indicates playing live streaming content
            /// </summary>
            Live = 1,
            /// <summary>
            /// RemoteReview indicates playing historical content from a remote device
            /// </summary>
            Review = 2,
            /// <summary>
            /// Archive indicates playing from a file system
            /// </summary>
            File = 3
        }

        public enum ContentType : int
        {
            Audio = 0,
            Video = 1
        };
    }
}
