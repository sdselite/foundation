using SDSFoundation.Interfaces.Content;
using SDSFoundation.Interfaces.Content.ContentModel;
using SDSFoundation.Interfaces.Content.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers
{
    public partial class UnifiedPlayer : IVideoContent
    {
        private Button testbutton;

        public IVideoContent Player { get; set; }

        public Guid ContentIdentifier { get; set; }
        public int Order { get; set; }
        public ContentEnums.PlayState CurrentPlayState { get; set; }
        public ContentEnums.PlayDirection CurrentPlayDirection { get; set; }
        public int Speed { get; set; }
        public DateTime PlayTime { get; set; }
        public ContentEnums.ContentCategory Category { get; set; }
        public ContentEnums.ContentType ContentType { get; set; }
        public Dictionary<string, object> ContentSettings { get; set; }

        public event Action<IContentProperties> OnNextFrame;
        public event Action<IContentProperties> OnPreviousFrame;
        public event Action<IContentProperties> OnAudioEnabled;
        public event Action<IContentProperties> OnAudioDisabled;
        public event Action<IContentProperties> OnPlayTimeChanged;
        public event Action<IContentProperties> OnContentCategoryChanged;
        public event Action<IContentProperties> OnPlayDirectionChanged;
        public event Action<IContentProperties> OnPlayStateChanged;
        public event Action<IContentProperties> OnPlaySpeedChanged;
        public event Action<IContentProperties, string, decimal> OnSaveContentBegin;
        public event Action<IContentProperties, string, decimal> OnSaveContentStatus;
        public event Action<IContentProperties, string, decimal> OnSaveContentComplete;
        public event Action<IContentProperties, string, string, decimal, _Exception> OnSaveContentFailed;
        public event Action<IContentProperties, _Exception> OnContentException;

        public void Connect(Uri primaryUriWithCredentials, Uri secondaryUriWithCredentials = null)
        {
            throw new NotImplementedException();
        }

        public void DisableAudio()
        {
            throw new NotImplementedException();
        }

        public void EnableAudio()
        {
            throw new NotImplementedException();
        }

        public void InitializeEvents()
        {
            throw new NotImplementedException();
        }

        public void NextFrame()
        {
            throw new NotImplementedException();
        }

        public void Open(string filePath, string userName = "", string password = "")
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Play(DateTime start)
        {
            throw new NotImplementedException();
        }

        public void Play(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public bool PlayFaster()
        {
            throw new NotImplementedException();
        }

        public bool PlaySlower()
        {
            throw new NotImplementedException();
        }

        public void PreviousFrame()
        {
            throw new NotImplementedException();
        }

        public void ResetPlaySpeed()
        {
            throw new NotImplementedException();
        }

        public void ReversePlay()
        {
            throw new NotImplementedException();
        }

        public void SaveContent(DateTime start, DateTime end, string fileName, bool includeAudio = true)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }


    }
}
