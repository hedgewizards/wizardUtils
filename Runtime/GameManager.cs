﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WizardUtils.Configurations;
using WizardUtils.Coroutines;
using WizardUtils.SettingWatchers;
using WizardUtils.Dialogs;
using WizardUtils.Platforms;
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
        public Audio.SoundService SoundService;
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
            GameObject soundServiceParent = new GameObject("SoundService Pools");
            soundServiceParent.transform.parent = transform;
            SoundService = new Audio.SoundService(soundServiceParent, Manifests.GlobalSound, Manifests.PooledAudioTypes);
            CurrentSceneLoaders = new List<SceneLoader>();

            InitializeConfigurationService();
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
            return new Platforms.Portable.PortablePlatformService();
#else

#if SPACEWAR
            var platformService = new Platforms.Steam.SteamPlatformService(this);
            if (platformService.Initialized)
            {
                return platformService;
            }
            else
            {
                return new Platforms.Portable.PortablePlatformService();
            }
#else
            return new Platforms.Steam.SteamPlatformService(this);
#endif
#endif
        }
        #endregion

        #region Loading
        public DialogScreen DialogScreen;

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
        private SceneLoadingData CurrentSceneLoadingData;
        private List<SceneLoader> CurrentSceneLoaders;

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

        public virtual void LoadControlSceneAsync(ControlSceneDescriptor newScene, ControlSceneLoadOptions options = null, Action callback = null)
        {
            InternalLoadScenesAsync(new SceneLoadingData()
            {
                InitialScene = CurrentControlScene,
                TargetControlScene = newScene,
                Options = options ?? new ControlSceneLoadOptions(),
                Callback = callback,
                StartTime = Time.unscaledTime,
                TargetSceneBuildIds = new int[]
                {
                    newScene.BuildIndex
                }
            });
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

        private void InternalLoadScenesAsync(SceneLoadingData sceneLoadingData)
        {
            if (DontLoadScenesInEditor) return;
            var initialScene = CurrentControlScene;

            if (CurrentSceneLoadingData != null)
            {
                Debug.LogWarning($"Overriding old ControlScene Loading Data {CurrentSceneLoadingData} with {sceneLoadingData}. pray to god nothing explodes!!!");
                if (CurrentSceneLoadingData.DelayedFinishLoadCoroutine != null)
                {
                    StopCoroutine(CurrentSceneLoadingData.DelayedFinishLoadCoroutine);
                }
            }
            CurrentSceneLoadingData = sceneLoadingData;
            if (CurrentSceneLoadingData.Options.DoDefaultLoadingScreenBehavior)
            {
                DialogScreen.ShowLoading();
            }

            (List<int> scenesToLoad, List<int> scenesToUnload) = GetSceneDifference(sceneLoadingData.TargetSceneBuildIds, CurrentSceneLoaders);

            foreach (var sceneIndex in scenesToLoad)
            {
                var loader = CurrentSceneLoaders.Find(l => l.SceneIndex == sceneIndex);
                if (loader == null)
                {
                    loader = new SceneLoader(this, sceneIndex, false);
                    loader.OnReadyToActivate.AddListener(RecalculateFinishedLoadingControlScene);
                    loader.OnIdle.AddListener(RecalculateFinishedLoadingControlScene);
                }
                loader.MarkNeeded();
            }

            foreach (var sceneIndex in scenesToUnload)
            {
                var loader = CurrentSceneLoaders.Find(l => l.SceneIndex == sceneIndex);

                if (loader == null)
                {
                    loader = new SceneLoader(this, sceneIndex, true);
                    CurrentSceneLoaders.Add(loader);
                    loader.OnReadyToActivate.AddListener(RecalculateFinishedLoadingControlScene);
                    loader.OnIdle.AddListener(RecalculateFinishedLoadingControlScene);
                }
                loader.MarkNotNeeded(true);
            }

            if (scenesToLoad.Count == 0
                && scenesToUnload.Count == 0)
            {
                CurrentSceneLoadingData.DelayedFinishLoadCoroutine = this.StartDelayCoroutineUnscaled(CurrentSceneLoadingData.Options.MinimumLoadDurationSeconds, FinishLoadingControlScene);
            }
        }

        private void FinishLoadingControlScene()
        {
            if (CurrentSceneLoadingData == null)
            {
                Debug.LogWarning("FinishLoadingControlScene was called with no CurrentSceneLoadingData. Ignoring.");
                return;
            }

            SceneLoadingData lastSceneLoadingData = CurrentSceneLoadingData;
            CurrentSceneLoadingData = null;

            CurrentControlScene = lastSceneLoadingData.TargetControlScene;
            if (lastSceneLoadingData.DelayedFinishLoadCoroutine != null)
            {
                StopCoroutine(lastSceneLoadingData.DelayedFinishLoadCoroutine);
            }
            OnControlSceneChanged?.Invoke(this, new ControlSceneEventArgs(lastSceneLoadingData.InitialScene, lastSceneLoadingData.TargetControlScene));
            lastSceneLoadingData.Callback?.Invoke();
            if (lastSceneLoadingData.Options.DoDefaultLoadingScreenBehavior)
            {
                DialogScreen.Hide();
            }
        }

        private void RecalculateFinishedLoadingControlScene()
        {
            if (CurrentSceneLoadingData == null) return;

            for (int i = CurrentSceneLoaders.Count - 1; i >= 0; i--)
            {
                SceneLoader loader = CurrentSceneLoaders[i];
                if (loader.IsIdle)
                {
                    loader.OnReadyToActivate.RemoveAllListeners();
                    loader.OnIdle.RemoveAllListeners();
                    CurrentSceneLoaders.RemoveAt(i);
                }
            }

            if (AllLoadingLevelsFinished())
            {
                float remainingTime = CurrentSceneLoadingData.Options.MinimumLoadDurationSeconds - (Time.unscaledTime - CurrentSceneLoadingData.StartTime);
                if (remainingTime > 0)
                {
                    Debug.Log($"Waiting {remainingTime} seconds before finishing loading control scene...");
                    CurrentSceneLoadingData.DelayedFinishLoadCoroutine = this.StartDelayCoroutineUnscaled(remainingTime, FinishLoadingControlScene);
                }
                else
                {
                    Debug.Log("Finished loading control scene");
                    FinishLoadingControlScene();
                }
            }
        }

        private bool AllLoadingLevelsFinished()
        {
            foreach (var sceneLoader in CurrentSceneLoaders)
            {
                if (sceneLoader.CurrentLoadState == SceneLoader.LoadStates.Loading
                    || sceneLoader.CurrentLoadState == SceneLoader.LoadStates.Unloading)
                {
                    return false;
                }
            }

            return true;
        }

        private (List<int> scenesToLoad, List<int> scenesToUnload) GetSceneDifference(
            IEnumerable<int> targetScenes,
            IEnumerable<SceneLoader> loaders = null)
        {
            List<int> scenesToLoad = new List<int>(targetScenes);
            List<int> scenesToUnload = new List<int>();

            // for each currently loaded sceneIndex
            for (int n = 0; n < SceneManager.sceneCount; n++)
            {
                int sceneIndex = SceneManager.GetSceneAt(n).buildIndex;

                // just ignore the GameScene, Main Menu, and Credits scenes
                if (ControlScene_IgnoreScenes.Contains(sceneIndex)) continue;

                // if we want to have it loaded
                if (scenesToLoad.Contains(sceneIndex))
                {
                    SceneLoader existingLoader = loaders.FirstOrDefault(l => l.SceneIndex == sceneIndex);

                    // if this level is marked as not needed
                    if (existingLoader != null && !existingLoader.CurrentlyNeeded)
                    {
                        // keep it in our scenesToLoad list, so we know to mark it needed
                    }
                    else
                    {
                        // SceneIndex already exists, so we don't need to load it
                        scenesToLoad.Remove(sceneIndex);
                    }
                }
                else
                {
                    // SceneIndex no longer exists. so we need to get rid of it
                    scenesToUnload.Add(sceneIndex);
                }
            }

            return (scenesToLoad, scenesToUnload);
        }

        #endregion

        #region Configuration
        const string settingsConfigFileName = "settings";
        private void InitializeConfigurationService()
        {
            IWritableConfiguration fileConfig = new CfgFileConfiguration(PlatformService, settingsConfigFileName);
            IConfiguration overrideConfiguration = null;
#if DEBUG
            if (DebugOverrideConfig != null)
            {
                overrideConfiguration = new ExplicitConfiguration(DebugOverrideConfig);
            }
#endif
            Configuration = new ConfigurationService(Manifests.IndexedSettings, fileConfig, overrideConfiguration);
        }
        #endregion

        #region Saving

        public string ReadMainSave(SaveValueDescriptor descriptor)
        {
            return Configuration.Read(descriptor.Key);
        }

        public void WriteMainSave(SaveValueDescriptor descriptor, string stringValue)
        {
            Configuration.Write(descriptor.Key, stringValue);
        }
#endregion
    }
}