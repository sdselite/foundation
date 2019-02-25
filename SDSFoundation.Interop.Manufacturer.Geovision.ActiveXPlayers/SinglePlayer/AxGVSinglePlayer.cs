using SDSFoundation.Interfaces.Content;
using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using SDSFoundation.ExtensionMethods.Threading;
using SDSFoundation.Interfaces.Content.Video;
using SDSFoundation.Interfaces.Content.ContentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using SDSFoundation.ExtensionMethods.Network;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer
{
    public class AxGVSinglePlayer : AxGVSINGLEPLAYERLib.AxGVSinglePlayer, IVideoContent, ISupportInitialize //, IAxGVSinglePlayer
    {

        public AxGVSinglePlayer()
        {
            if (this.DesignMode)
                return;

            //InitializeEvents();
        }

        public void InitializeEvents()
        {
            //Handle Events
            DownloadStart += SinglePlayer_DownloadStart;
            this.DownloadSingleStart += SinglePlayer_DownloadSingleStart;
            this.DownloadEvent += SinglePlayer_DownloadEvent;
            this.DownloadSingleEnd += SinglePlayer_DownloadSingleEnd;
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
            catch (Exception)
            {

            }

            this.Visible = true;

            //((AxHost)this).EndInit();
        }

        public IVideoContent Player {
            get {

                IVideoContent result = null;
                this.InvokeIfRequired(() => {
                    result = this;
                });
                return this;
            }
        }


        internal string _LastDownloadDirectory;
        private new bool Download(bool bAllFiles, string lpDirectory)
        {
            _LastDownloadDirectory = lpDirectory;
            return base.Download(bAllFiles, lpDirectory);
        }
 
 
        private new bool Download2(bool bPopupDlg, bool bAllFiles, string lpDirectory)
        {
            this._LastDownloadDirectory = lpDirectory;
            return base.Download2(bPopupDlg, bAllFiles, lpDirectory);
        }

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid ContentIdentifier { get; set; }

        [DefaultValue(0)]
        public int Order { get; set; }

        [DefaultValue("")]
        public new string Name { get; set; }

        [DefaultValue(0)]
        public ContentEnums.PlayState CurrentPlayState { get; set; }

        [DefaultValue(0)]
        public ContentEnums.PlayDirection CurrentPlayDirection { get; set; }

        [DefaultValue(0)]
        public int Speed { get; set; }

        [DefaultValue(typeof(DateTime), "1/1/0001 12:00:00 AM")]
        public DateTime PlayTime { get; set; }

        [DefaultValue(0)]
        public ContentEnums.ContentCategory Category { get; set; }

        [DefaultValue(0)]
        public ContentEnums.ContentType ContentType { get; set; }

        public Dictionary<string, object> ContentSettings { get; set; }

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
        public event Action<IContentProperties> OnNextFrame;
        public event Action<IContentProperties> OnPreviousFrame;
        public event Action<IContentProperties> OnAudioEnabled;
        public event Action<IContentProperties> OnAudioDisabled;



        private void SinglePlayer_DownloadSingleEnd(object sender, AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadSingleEndEvent e)
        {
            OnSaveContentComplete(this, this._LastDownloadDirectory, downloadSizeRunningTotal);
        }

        private decimal downloadSizeRunningTotal = 0;
        private void SinglePlayer_DownloadEvent(object sender, AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadEvent e)
        {
            //TODO: Keep a running total of the downloaded files
            OnSaveContentStatus(this, e.lpFileName, e.nFiles);
        }

        private void SinglePlayer_DownloadSingleStart(object sender, AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadSingleStartEvent e)
        {
            OnSaveContentStatus(this, e.lpFileName, e.iDownloadSize);
        }

        //content properties, file path and name, and total file size if known
        private void SinglePlayer_DownloadStart(object sender, AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadStartEvent e)
        {
            OnSaveContentBegin(this, "", e.iTotalSize);
        }


        public new void Pause()
        {
            var didPause = base.Pause();

        }

        public new void Play()
        {
            var didPlay = base.Play();
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

        public new void Stop()
        {
            base.Stop();
        }

        public void NextFrame()
        {
            throw new NotImplementedException();
        }

        public void PreviousFrame()
        {
            throw new NotImplementedException();
        }

        public void EnableAudio()
        {
            throw new NotImplementedException();
        }

        public void DisableAudio()
        {
            throw new NotImplementedException();
        }

        public void Open(string filePath, string userName = "", string password = "")
        {
            throw new NotImplementedException();
        }

        public void Connect(Uri primaryUriWithCredentials, Uri secondaryUriWithCredentials = null)
        {

            var didConnect = this.Login2(primaryUriWithCredentials.Host, primaryUriWithCredentials.Port, primaryUriWithCredentials.GetUserName(), primaryUriWithCredentials.GetPassword());
        }

    }
}
