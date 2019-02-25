﻿using SDSFoundation.Interfaces.Interop.ActiveX.UserControl;
using SDSFoundation.Interop.ActiveX.UserControl.Wrapper;
using System;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers.SinglePlayer
{
    public interface IAxGVSinglePlayer : IAxControl
    {
        bool Add2SearchArray(bool bClearArray, int lConnectID, int lSvrType, int lCam1Base, int lHostType, string lpHostName);
        bool Command(int lCommandID, int lCmdProperty1, int lCmdProperty2, int lCmdProperty3, int lCmdProperty4, int lCmdProperty5, string lpCmdProperty1, string lpCmdProperty2, string lpCmdProperty3, string lpCmdProperty4, string lpCmdProperty5);
        bool DecryptLandS(int lType, string lpDecrypt, bool bAutoPlay);
        bool Download(bool bAllFiles, string lpDirectory);
        bool Download2(bool bPopupDlg, bool bAllFiles, string lpDirectory);
        bool EnableBlowFishKey(bool bEnable, string lpBlowFishKey);
        bool End();
        bool FullScreen();
        bool GetBitmap(bool bShowDlg, string lpDefaultFileName);
        bool GetEventTime(string m_pEventTime, int m_Reserve1, int m_Reserve2, int m_Reserve3);
        bool GetSerachList(bool bEnable);
        bool Home();
        bool Init(int iReserve);
        bool Login();
        bool Login2(string lpIPAddress, int iPort, string lpUserID, string lpPassword);
        bool Login2BackupSvr(string lpAddress, int iPort, string lpUserID, string lpPassword);
        bool Login2BackupSvr2(string lpAddress, int iPort, string lpIDandPWD);
        bool Login3(string lpAddress, int iPort, string lpIDandPWD);
        bool Logout();
        bool LogoutOne(int lConnectID, bool bRemoved);
        bool Merge(string lpExportFileName, bool bAllFiles, int iReserve1, int iReserve2);
        bool Merge2(int lGeneralCodec, string lpStartTime, string lpEndTime, string lpFileName);
        bool Next();
        bool Next1Min();
        bool Next5Min();
        bool Pause();
        bool Play();
        bool Prev();
        bool Prev1Min();
        bool Prev5Min();
        bool RefreshProperty(int m_lProperty);
        bool ResetSpeed();
        bool Rewind();
        bool Search(int nHostType, int nDBType, int nCamera, string lpDateTime);
        bool Search2(int nHostType, int nDBType, int nCamera, string lpStartTime, string lpEndTime);
        bool SearchAndPlay(int nHostType, int nDBType, int nCamera, string lpDateTime);
        bool SearchAndPlay2(int nHostType, int nDBType, int nCamera, string lpStartTime, string lpEndTime);
        bool SearchArray(bool bDST, string lpStartTime, string lpEndTime, bool bPlay);
        bool SearchBackupCenter(bool bDST, int iCamera, int iHostType, string lpHostName, string lpDateTime);
        bool SearchBackupCenter2(bool bDST, int iCamera, int iHostType, string lpHostName, string lpStartTime, string lpEndTime);
        bool SearchDirectory(string lpDirectory, string lpStartTime, string lpEndTime);
        bool SearchDVR(int nDBType, int nCamera, string lpDateTime);
        bool SearchDVR2(int nDBType, int nCamera, string lpStartTime, string lpEndTime);
        bool SearchRecordingSvr(bool bDST, int iCamera, int iHostType, string lpHostName, string lpDateTime);
        bool SearchRecordingSvr2(bool bDST, int iCamera, int iHostType, string lpHostName, string lpStartTime, string lpEndTime);
        bool SearchStorageSys(int nDBType, int nCamera, int nHostType, string lpHostName, string lpDateTime);
        bool SearchStorageSys2(int nDBType, int nCamera, int nHostType, string lpHostName, string lpStartTime, string lpEndTime);
        bool SearchVideoSvr(int nDBType, int nCamera, string lpDateTime);
        bool SearchVideoSvr2(int nDBType, int nCamera, string lpStartTime, string lpEndTime);
        bool SeekTo(string lpTime);
        bool Set2GetEveryFrame(bool bEnable);
        bool SetAudio(bool bEnableAudio);
        bool SetBlueScreenFile(string lpBlueScreenFile);
        bool SetCachePath(string lpCachePath);
        bool SetDisconnectFile(string lpDisconnectFile);
        bool SetLanguage(int nLanguageID);
        bool SetModify(int iRemove, int iAdd);
        bool SetPlayMode(int nPlayMode);
        bool SetPropertyEx(int lPropertyID, int lPropertyValue, string lpPropertyValue);
        bool SetSpeed(bool bSpeedUp);
        void SetWndSize(bool bChangePos, int iPosX, int iPosY, bool bChangeSize, int iWidth, int iHeight);
        bool ShowAudioMenu();
        bool ShowLoginDlg();
        bool ShowLoginMenu();
        bool ShowPlayModeMenu();
        bool ShowSearchDlg();
        bool SnapShot(string lpFileName);
        bool Stop();


        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadSingleEndEventHandler DownloadSingleEnd;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_PictureEventHandler Picture;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_EventTimeEventHandler EventTime;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_MergingEventHandler Merging;
        event EventHandler Merged;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadSingleStartEventHandler DownloadSingleStart;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_SearchListEventHandler SearchList;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadStartEventHandler DownloadStart;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_VideoInfoEventHandler VideoInfo;
        event EventHandler KeyInLoginCancel;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_KeyInLoginOKEventHandler KeyInLoginOK;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_DownloadEventHandler DownloadEvent;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_BitmapEventHandler Bitmap;
        event EventHandler Disconnect;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_SearchEventHandler SearchEvent;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_LoginFailEventHandler LoginFail;
        event EventHandler LoginOK;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_NowTimeEventHandler NowTime;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_ResizingWndEventHandler ResizingWnd;
        event AxGVSINGLEPLAYERLib._DGVSinglePlayerEvents_LoginEventHandler LoginEvent;
    }
}
