using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content.ContentModel
{

    public class ContentEnums
    {
        [DefaultValue(0)]
        public enum PlayState
        {
            Undefined,
            Playing,
            Paused,
            Stopped
        };

        [DefaultValue(0)]
        public enum PlayDirection
        {
            Undefined,
            Forward,
            Reverse
        }

        [DefaultValue(0)]
        public enum ContentCategory
        {
            Undefined,
            /// <summary>
            /// Live indicates playing live streaming content
            /// </summary>
            Live,
            /// <summary>
            /// RemoteReview indicates playing historical content from a remote device
            /// </summary>
            Review,
            /// <summary>
            /// Archive indicates playing from a file system
            /// </summary>
            File
        }

        [DefaultValue(0)]
        public enum ContentType
        {
            Undefined,
            Audio,
            Video
        };
    }
}
