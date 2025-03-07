#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Platforms.Steam
{
    public class SteamPlatformService : IPlatformService
    {
        private bool initialized;
        public bool Initialized => initialized;
        public virtual AppId_t AppId => new(480u); // make sure this matches steam_appid.txt in the root folder.
        private SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;
        private Callback<GameOverlayActivated_t> m_OnGameOverlayActivated;
        protected GameManager GameManager;

        public string PersistentDataPath { get; private set; }

        public string PlatformURLName => "steam";

        public SteamPlatformService(GameManager gameManager)
        {
            GameManager = gameManager;
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
                DoInitializeError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. \n" + e);
                return;
            }

            initialized = SteamAPI.Init();
            if (!initialized)
            {
                DoInitializeError("[Steamworks.NET] SteamAPI_Init() failed.");
                return;
            }

            SetupPersistentDataPath();
            GameManager.StartCoroutine(SpawnRunCallbacksCoroutine());
        }

        private void DoInitializeError(string message)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed.");

#if !SPACEWAR
            Debug.LogWarning(message);
#else
            Debug.LogError(message);
            Application.Quit();
#endif
        }

        private IEnumerator SpawnRunCallbacksCoroutine()
        {
            while (initialized)
            {
                SteamAPI.RunCallbacks();
                yield return new WaitForSecondsRealtime(0.05f);
            }
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