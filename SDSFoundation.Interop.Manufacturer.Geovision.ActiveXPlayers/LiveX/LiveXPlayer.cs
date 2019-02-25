using SDSFoundation.ExtensionMethods.Network;
using SDSFoundation.ExtensionMethods.Threading;
using SDSFoundation.Interfaces.Content;
using SDSFoundation.Interfaces.Content.ContentModel;
using SDSFoundation.Interfaces.Content.Video;
using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SDSFoundation.Interfaces.Content.ContentModel.ContentEnums;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.LiveX
{
    public class LiveXPlayer : AxLIVEXLib.AxLiveX, IVideoContent, ISupportInitialize
    {
        public LiveXPlayer()
        {
            if (this.DesignMode)
                return;
            //TODO - Initialize Events
        }



        public void CreateControlHandle()
        {
            //((AxHost)this).BeginInit();

            try
            {
                //this.Parent.Visible = true;
                //this.Visible = true;
                this.Parent.CreateControl();
                ((AxHost)this).CreateControl();
       
            }
            catch (Exception ex)
            {

            }

            this.Visible = true;

            //((AxHost)this).EndInit();
        }
        public IVideoContent Player
        {
            get
            {

                IVideoContent result = null;
                this.InvokeIfRequired(() => {
                    result = this;
                });
                return this;
            }
        }

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid ContentIdentifier { get; set; }

        [DefaultValue(0)]
        public int Order { get; set; }

        [DefaultValue(0)]
        public  PlayState CurrentPlayState { get; set; }

        [DefaultValue(0)]
        public PlayDirection CurrentPlayDirection { get; set; }

        [DefaultValue(0)]
        public int Speed { get; set; }

        [DefaultValue(typeof(DateTime), "1/1/0001 12:00:00 AM")]
        public DateTime PlayTime { get; set; }

        [DefaultValue(0)]
        public ContentCategory Category { get; set; }

        [DefaultValue(0)]
        public ContentType ContentType { get; set; }

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
         
            this.IpAddress = primaryUriWithCredentials.Host;
            this.CommandPort = primaryUriWithCredentials.Port;
            if(secondaryUriWithCredentials != null && secondaryUriWithCredentials.Port > 0)
            {
                this.DataPort = secondaryUriWithCredentials.Port;
            }
            else
            {
                this.DataPort = primaryUriWithCredentials.Port;
            }
            this.UserName = primaryUriWithCredentials.GetUserName();
            this.Password = primaryUriWithCredentials.GetPassword();
            this.AutoLogin = true;
            this.DisablePWD = true;
            this.SetInfo(301, 0, 0, "", null);
            this.AutoReConnect = true;
      
            //this.SetGUIMode(3, 0, 0);

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
            var didPause = base.PauseX();
            if (didPause)
            {
                this.CurrentPlayState = PlayState.Paused;
            }
            
        }

        public void Play()
        {
            var didPlayOrResume = false;
            if(this.CurrentPlayState == PlayState.Paused)
            {
                didPlayOrResume = base.ResumeX();
            }
            else
            {
                didPlayOrResume = base.PlayX();
            }

            if (didPlayOrResume)
            {
                this.CurrentPlayState = PlayState.Playing;
            }
        }

        public void Play(DateTime start)
        {
            Play();
        }

        public void Play(DateTime start, DateTime end)
        {
            Play();
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
