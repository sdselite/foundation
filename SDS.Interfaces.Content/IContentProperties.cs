using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        Guid ContentIdentifier { get; set; }
        /// <summary>
        /// Order is used to identify cameras and should be 0 based.  For example, camera 1 would be set to DeviceNumber 0.  If an IP camera, set to 0
        /// </summary>
        [DefaultValue(0)]
        int Order { get; set; }

        /// <summary>
        /// Name is used to identify the name of the camera in a human readable format.  For example "Cash Register 1"
        /// </summary>
        [DefaultValue("")]
        string Name { get; set; }

        /// <summary>
        /// Indicates the current play status
        /// </summary>
        [DefaultValue(0)]
        PlayState CurrentPlayState { get; set; }

        /// <summary>
        /// Indicates the currently set direction of play
        /// </summary>
        [DefaultValue(0)]
        PlayDirection CurrentPlayDirection { get; set; }

        /// <summary>
        /// A play speed of 1 is considered "normal speed"  If playing in slow motion, the speed would be a negative value.  If playing faster than normal the number would be a value greater than 1. 
        /// A value of 0 shall be considered "stopped"
        /// </summary>
        [DefaultValue(0)]
        int Speed { get; set; }

        /// <summary>
        /// Indicates the current play time or last play time if paused
        /// </summary>
        [DefaultValue(typeof(DateTime), "1/1/0001 12:00:00 AM")]
        DateTime PlayTime { get; set; }

        /// <summary>
        /// Indicates the type of content (such as Live or Review)
        /// </summary>
        [DefaultValue(0)]
        ContentCategory Category { get; set; }

        /// <summary>
        /// Indicates the content type (audio, video, etc)
        /// </summary>
        [DefaultValue(0)]
        ContentType ContentType { get; set; }

        /// <summary>
        /// Setting name and value dictionary.  This collection is to be used for cases that are not supported generically
        /// </summary>
        [DefaultValue(0)]
        Dictionary<String, Object> ContentSettings { get; set; }
    }
}
