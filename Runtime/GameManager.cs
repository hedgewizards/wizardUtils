using Platforms;
using Platforms.Portable;
using Platforms.Steam;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WizardUtils.Configurations;
using WizardUtils.GameSettings;
using WizardUtils.GameSettings.Legacy;
using WizardUtils.Saving;
using WizardUtils.SceneManagement;

namespace WizardUtils
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public ManifestSet Manifests;
        public IPlatformService PlatformService;
        public IConfigurationService Configuration;
        public ExplicitConfigurationData DebugOverrideConfig;
        [HideInInspector]
        public GlobalSounds.GlobalSoundService GlobalSoundService;
        public string PersistentDataPath => Application.persistentDataPath;
        [NonSerialized]
        public UnityEvent OnQuitToMenu = new UnityEvent();
        [NonSerialized]
        public UnityEvent OnQuitToDesktop = new UnityEvent();

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            PlatformService = BuildPlatformService();
            GlobalSoundService = new GlobalSounds.GlobalSoundService(gameObject, Manifests.GlobalSound);
            
            DontDestroyOnLoad(gameObject);
            InitializeConfigurationService();
            InitializeGameSettings();
            SetupSaveData();
            
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnApplicationQuit()
        {
            Configuration.Save();
            OnQuitToDesktop.Invoke();
        }

        public static bool GameInstanceIsValid()
        {
            return Instance != null;
        }

        #region Platforms
        protected virtual IPlatformService BuildPlatformService()
        {
#if DISABLESTEAMWORKS
            return new PortablePlatformService();
#else
            return new SteamPlatformService();
#endif
        }
        #endregion

        #region Pausing
        public virtual bool LockPauseState => false;

        bool paused;

#if UNITY_EDITOR
        public bool BreakOnPause;
#endif

        public bool Paused
        {
            get => paused;
            set
            {
                if (!GameInstanceIsValid()) return;

                if (paused == value || Instance.LockPauseState) return;
                paused = value;
                if (value)
                {
                    pause();
                }
                else
                {
                    resume();
                }
                Instance.OnPauseStateChanged?.Invoke(null, value);
            }
        }
        public EventHandler<bool> OnPauseStateChanged;

        private static void pause()
        {
#if UNITY_EDITOR
            if (Instance.BreakOnPause) Debug.Break();
#endif
            Time.timeScale = 0;
        }

        private static void resume()
        {
            Time.timeScale = 1;
        }
        #endregion

        #region Scenes
        public abstract int[] ControlScene_IgnoreScenes { get; }
        public bool DontLoadScenesInEditor;

        public EventHandler<ControlSceneEventArgs> OnControlSceneChanged;
        public bool InControlScene => CurrentControlScene != null;
        public bool InGameScene => CurrentControlScene == null;

        [HideInInspector]
        public ControlSceneDescriptor CurrentControlScene;

#if UNITY_EDITOR
        public void UnloadControlSceneInEditor()
        {
            var initialScene = CurrentControlScene;

            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                for (int n = 0; n < SceneManager.sceneCount; n++)
                {
                    Scene scene = SceneManager.GetSceneAt(n);
                    if (scene.buildIndex == CurrentControlScene.BuildIndex)
                    {

                        UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
                        CurrentControlScene = null;
                        break;
                    }
                }
                OnControlSceneChanged?.Invoke(this, new ControlSceneEventArgs(initialScene, null));
            }
        }

        public virtual void LoadControlSceneInEditor(ControlSceneDescriptor newScene)
        {
            bool newSceneAlreadyLoaded = false;
            List<Scene> scenesToUnload = new List<Scene>();
            for (int n = 0; n < SceneManager.sceneCount; n++)
            {
                Scene scene = SceneManager.GetSceneAt(n);
                if (!ControlScene_IgnoreScenes.Contains(scene.buildIndex))
                {
                    if (scene.buildIndex == newScene.BuildIndex)
                    {
                        newSceneAlreadyLoaded = true;
                    }
                    else
                    {
                        scenesToUnload.Add(scene);
                    }
                }
            }

            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // load the control scene
                if (!newSceneAlreadyLoaded)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(newScene.ScenePath, UnityEditor.SceneManagement.OpenSceneMode.Additive);
                    CurrentControlScene = newScene;
                }

                // unload all non-ignored scenes
                foreach (Scene scene in scenesToUnload)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
#endif

        public virtual void LoadControlScene(ControlSceneDescriptor newScene, Action callback = null)
        {
#if UNITY_EDITOR
            if (DontLoadScenesInEditor) return;
#endif
            var initialScene = CurrentControlScene;
            List<AsyncOperation> tasks = new List<AsyncOperation>();

            bool newSceneAlreadyLoaded = false;
            List<Scene> scenesToUnload = new List<Scene>();
            for (int n = 0; n < SceneManager.sceneCount; n++)
            {
                Scene scene = SceneManager.GetSceneAt(n);
                if (!ControlScene_IgnoreScenes.Contains(scene.buildIndex))
                {
                    if (scene.buildIndex == newScene.BuildIndex)
                    {
                        newSceneAlreadyLoaded = true;
                    }
                    else
                    {
                        scenesToUnload.Add(scene);
                    }
                }
            }

            // load the control scene
            if (!newSceneAlreadyLoaded)
            {
                tasks.Add(SceneManager.LoadSceneAsync(newScene.BuildIndex, LoadSceneMode.Additive));
                CurrentControlScene = newScene;
            }

            // unload all non-ignored scenes
            foreach (Scene scene in scenesToUnload)
            {
                AsyncOperation task = SceneManager.UnloadSceneAsync(scene);
                if (task != null)
                {
                    tasks.Add(task);
                }
                else
                {
                    Debug.LogWarning($"error unloading scene {scene.name}");
                }
            }

            CurrentControlScene = newScene;
            OnControlSceneChanged?.Invoke(this, new ControlSceneEventArgs(initialScene, newScene));
            if (callback != null)
            {
                StartCoroutine(AsyncSceneLoadingHelper.WaitForScenesLoadAsync(tasks, callback));
            }
        }

        public void ReloadControlScene()
        {
            AsyncOperation task = SceneManager.UnloadSceneAsync(CurrentControlScene.BuildIndex);

            StartCoroutine(Reload_WaitForUnload(task));
        }

        IEnumerator Reload_WaitForUnload(AsyncOperation task)
        {
            while (!task.isDone)
            {
                yield return new WaitForSecondsRealtime(0.05f);
            }

            SceneManager.LoadSceneAsync(CurrentControlScene.BuildIndex, LoadSceneMode.Additive);
        }

        public void UnloadControlScene()
        {
            SceneManager.UnloadSceneAsync(CurrentControlScene.BuildIndex);
            var initialScene = CurrentControlScene;
            CurrentControlScene = null;
            OnControlSceneChanged?.Invoke(this, new ControlSceneEventArgs(initialScene, null));
        }

        public void Quit(bool hardQuit)
        {
            if (hardQuit)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
                Debug.LogError("Can't quit in web builds!!! D:");
#else
                Application.Quit();
#endif
            }
            else
            {
                OnQuitToMenu?.Invoke();
            }
        }
        #endregion

        #region GameSettings
        private Dictionary<string, GameSettingFloat> GameSettingFloats;

        private void InitializeGameSettings()
        {
            GameSettingFloats = new Dictionary<string, GameSettingFloat>();
            foreach(var setting in LoadGameSettingFloats())
            {
                GameSettingFloats[setting.Key] = setting;
            }
        }

        protected virtual List<GameSettingFloat> LoadGameSettingFloats()
        {
            return new List<GameSettingFloat>()
            {
                new GameSettingFloat(Configuration, KEY_VOLUME_MASTER, 100),
                new GameSettingFloat(Configuration, KEY_VOLUME_EFFECTS, 80),
                new GameSettingFloat(Configuration, KEY_VOLUME_AMBIENCE, 80),
                new GameSettingFloat(Configuration, KEY_VOLUME_MUSIC, 80),
                new GameSettingFloat(Configuration, SETTINGKEY_MUTE_ON_ALT_TAB, 0),
            };
        }

        public GameSettingFloat FindGameSetting(string key)
        {
            return GameSettingFloats.GetValueOrDefault(key, null);
        }

        public static string KEY_VOLUME_MASTER = "Volume_Master";
        public static string KEY_VOLUME_EFFECTS = "Volume_Effects";
        public static string KEY_VOLUME_AMBIENCE = "Volume_Ambience";
        public static string KEY_VOLUME_MUSIC = "Volume_Music";

        public static string SETTINGKEY_MUTE_ON_ALT_TAB = "MuteOnAltTab";
        #endregion

        #region Configuration
        const string settingsConfigFileName = "settings";
        private void InitializeConfigurationService()
        {
            CfgFileConfiguration fileConfig = new CfgFileConfiguration(PlatformService, settingsConfigFileName);
#if DEBUG
            Configuration = new ConfigurationService(fileConfig, new ExplicitConfiguration(DebugOverrideConfig));
#else
            Configuration = new ConfigurationService(fileConfig);
#endif
        }
        #endregion

        #region Saving
        public ExplicitSaveData EditorOverrideSaveData;
        public bool DontSaveInEditor;
        SaveDataTracker saveDataTracker;

        private void SetupSaveData()
        {
            if (Manifests.MainSave == null) return;

#if UNITY_EDITOR
            if (EditorOverrideSaveData != null)
            {
                saveDataTracker = new SaveDataTrackerExplicit(Manifests.MainSave, EditorOverrideSaveData);
            }
            else
            {
                saveDataTracker = new SaveDataTrackerFile(Manifests.MainSave);
            }
#else
            saveDataTracker = new SaveDataTrackerFile(MainSaveManifest);
#endif
            saveDataTracker.Load();
        }

        public void SubscribeMainSave(SaveValueDescriptor descriptor, UnityAction<SaveValueChangedEventArgs> action)
        {
            var value = saveDataTracker.GetSaveValue(descriptor);
            value.OnValueChanged.AddListener(action);
        }
        public void UnsubscribeMainSave(SaveValueDescriptor descriptor, UnityAction<SaveValueChangedEventArgs> action)
        {
            var value = saveDataTracker.GetSaveValue(descriptor);
            value.OnValueChanged.RemoveListener(action);
        }

        public string ReadMainSave(SaveValueDescriptor descriptor)
        {
#if UNITY_EDITOR
            if (saveDataTracker == null)
            {
                Debug.LogWarning("Tried so load data without a MainSaveManifest", this);
            }
#endif
            return saveDataTracker?.Read(descriptor)?? null;
        }

        public void WriteMainSave(SaveValueDescriptor descriptor, string stringValue)
        {

#if UNITY_EDITOR
            if (saveDataTracker == null)
            {
                Debug.LogWarning("Tried so save data without a MainSaveManifest", this);
            }
#endif
            saveDataTracker?.Write(descriptor, stringValue);
        }

        public void SaveData()
        {
            saveDataTracker.Save();
        }
#endregion
    }
}