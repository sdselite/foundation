using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDSFoundation.Interfaces.Content.ContentModel.ContentEnums;

namespace SDSFoundation.Interfaces.Content
{
    public interface IContentProperties
    {
        /// <summary>
        /// Unique Id to 
        /// </summary>
        Guid ContentIdentifier { get; }
        /// <summary>
        /// Order is used to identify cameras and should be 0 based.  For example, camera 1 would be set to DeviceNumber 0.  If an IP camera, set to 0
        /// </summary>
        int Order { get;  }

        /// <summary>
        /// Name is used to identify the name of the camera in a human readable format.  For example "Cash Register 1"
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates the current play status
        /// </summary>
        PlayState CurrentPlayState { get; }

        /// <summary>
        /// Indicates the currently set direction of play
        /// </summary>
        PlayDirection CurrentPlayDirection { get; }

        /// <summary>
        /// A play speed of 1 is considered "normal speed"  If playing in slow motion, the speed would be a negative value.  If playing faster than normal the number would be a value greater than 1. 
        /// A value of 0 shall be considered "stopped"
        /// </summary>
        int Speed { get; }

        /// <summary>
        /// Indicates the current play time or last play time if paused
        /// </summary>
        DateTime PlayTime { get; }

        /// <summary>
        /// Indicates the type of content (such as Live or Review)
        /// </summary>
        ContentCategory Category { get; }

        /// <summary>
        /// Indicates the content type (audio, video, etc)
        /// </summary>
        ContentType ContentType { get; }

        /// <summary>
        /// Setting name and value dictionary.  This collection is to be used for cases that are not supported generically
        /// </summary>
        Dictionary<String, Object> ContentSettings { get; set; }
    }
}
