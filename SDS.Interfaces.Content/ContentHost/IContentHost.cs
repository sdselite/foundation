using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SDSFoundation.Interfaces.Content.ContentModel.ContentEnums;

namespace SDSFoundation.Interfaces.Content.ContentHost
{
    /// <summary>
    /// A content host is any device that contains content such as an IPCam, DVR, or security system
    /// A device might host multiple content sources such as cameras in a surveilance system.  Or a device might have a single camera such as an IP camera.
    /// A device might not necessarily be video content.  It could also contain audio content or both
    /// </summary>
    public interface IContentHost<TContent> 
        where TContent : IContent<IContentActions, IContentProperties, IContentEvents>
    {
        /// <summary>
        /// User friendly name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// User friendly description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Indicates the categories this host supports (live, review, file)
        /// </summary>
        IEnumerable<ContentCategory> SupportedContentCategories { get; }

        /// <summary>
        /// Ganz, Geovision, etc
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Used to identify the hardware supported by the ContentHost.  
        /// </summary>
        IEnumerable<string> SupportedHardware { get; }

        /// <summary>
        /// Version 1.1, 2.0, etc.  
        /// </summary>
        decimal ContentVersion { get; }

        /// <summary>
        /// Content sources.  Typically an IPCam will have a single entry and DVRs for security systems will have multiple entries (one per camera)
        /// </summary>
        IEnumerable<TContent> Content { get;  }

        /// <summary>
        /// Used to open file based content
        /// </summary>
        /// <param name="filePath">Required.  Full path and file name</param>
        /// <param name="userName">Optional.  Used to support username and password protected content</param>
        /// <param name="password">Optional.  Used to support password protected content</param>
        void Open(string filePath, string userName = "", string password = "");

        /// <summary>
        /// Provide a Uri with credentials to connect.  
        /// Some devices require multiple connections for video and for service calls
        /// var uri = new Uri("http://www.mydvr.com");
        /// var uriWithCred = new UriBuilder(uri) { UserName = "u", Password = "p", Port = 5000 }.Uri;
        /// </summary>
        /// <param name="primaryUriWithCredentials">Required</param>
        /// <param name="secondaryUriWithCredentials">Optional.  Only necessary for some devices</param>
        void Connect(Uri primaryUriWithCredentials, Uri secondaryUriWithCredentials = null);


        /// <summary>
        /// Indicates if the content is successfully connected to a device
        /// </summary>
        Boolean IsConnected { get; }

        /// <summary>
        /// returns the content host and the file path on success
        /// </summary>
        event Action<IContentHost<TContent>, string> OnOpenFile;

        /// <summary>
        /// returns the content host and the file path on failure
        /// </summary>
        event Action<IContentHost<TContent>, string> OnOpenFileFailure;

        /// <summary>
        /// Returns the content host and Uris on a successful connection.  The second Uri might be null
        /// </summary>
        event Action<IContentHost<TContent>, Uri, Uri> OnConnection;

        /// <summary>
        /// Returns the content host and Uris on a failed connection.  The second Uri might be null
        /// </summary>
        event Action<IContentHost<TContent>, Uri, Uri> OnConnectionFailure;

    }
}
