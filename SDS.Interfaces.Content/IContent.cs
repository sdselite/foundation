using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Content
{
    public interface IContent: IContentActions, IContentProperties, IContentEvents
    {
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
    }
}
