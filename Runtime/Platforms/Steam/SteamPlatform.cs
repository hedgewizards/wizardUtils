﻿#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.GameSettings;

namespace WizardUtils.Platforms.Steam
{
    public class SteamPlatformService : IPlatformService
    {
        private bool initialized;
        public bool Initialized => initialized;
        public virtual AppId_t AppId => new(480u);
        private SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;
        private Callback<GameOverlayActivated_t> m_OnGameOverlayActivated;

        public string PersistentDataPath { get; private set; }

        public string PlatformURLName => "steam";

        public SteamPlatformService()
        {
            try
            {
                if (SteamAPI.RestartAppIfNecessary(AppId))
                {
                    Debug.LogWarning("SteamApi RestartAppIfNecessary TRUE. Exiting App.");
                    Application.Quit();
                    return;
                }
            }
            catch (DllNotFoundException e)
            {
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. \n" + e);

                Application.Quit();
                return;
            }

            initialized = SteamAPI.Init();
            if (!initialized)
            {
                Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed.");
            }

            SetupPersistentDataPath();
        }

        #region Messages

        public void OnEnable()
        {
            if (!initialized)
            {
                return;
            }

            if (SteamAPIWarningMessageHook == null)
            {
                SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);
            }

            m_OnGameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        }

        public void OnDestroy()
        {
            if (!initialized)
            {
                return;
            }

            SteamAPI.Shutdown();
        }

        #endregion

        private void SetupPersistentDataPath()
        {
            AccountID_t steamId = SteamUser.GetSteamID().GetAccountID();

            // %home%/steamsaves/steam64id
            PersistentDataPath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}steamsaves{Path.DirectorySeparatorChar}{steamId}";
        }
        private void OnGameOverlayActivated(GameOverlayActivated_t callback)
        {
            if (callback.m_bActive != 0)
            {
                // overlay is active
            }
            else
            {

            }
        }

        private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }

    }
}
#endif