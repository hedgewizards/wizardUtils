using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WizardUtils.Saving;
using WizardUtils.SceneManagement;

namespace WizardUtils
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager GameInstance;
        public EventHandler OnSoftQuit;
        protected virtual void Awake()
        {
            if (GameInstance != null)
            {
                Destroy(this);
                return;
            }

            GameInstance = this;
            RegisterGameSettings();
            DontDestroyOnLoad(gameObject);
            SetupSaveData();
        }

        protected virtual void Update()
        {

        }

        public static bool GameInstanceIsValid()
        {
            return GameInstance != null;
        }

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

                if (paused == value || GameInstance.LockPauseState) return;
                paused = value;
                if (value)
                {
                    pause();
                }
                else
                {
                    resume();
                }
                GameInstance.OnPauseStateChanged?.Invoke(null, value);
            }
        }
        public EventHandler<bool> OnPauseStateChanged;

        private static void pause()
        {
#if UNITY_EDITOR
            if (GameInstance.BreakOnPause) Debug.Break();
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

        public ControlSceneDescriptor MainMenuControlScene;
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

        public virtual void LoadControlScene(ControlSceneDescriptor newScene, Action<List<AsyncOperation>> callback = null)
        {
            if (DontLoadScenesInEditor) return;
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
            callback?.Invoke(tasks);
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
                OnSoftQuit?.Invoke(this, EventArgs.Empty);
                if (!InControlScene)
                {
                    LoadControlScene(MainMenuControlScene);
                }
            }
        }
        #endregion

        #region GameSettings
        public List<GameSettingDescriptor> SettingDescriptors;
        List<GameSettingFloat> GameSettings;


        void RegisterGameSettings()
        {
            GameSettings = new List<GameSettingFloat>();

            for (int i = 0; i < SettingDescriptors.Count; i++)
            {
                GameSettingDescriptor setting = SettingDescriptors[i];
                if (setting == null)
                {
                    Debug.LogError($"Missing GameSetting in slot {i}");
                }
                else
                {
                    RegisterGameSetting(new GameSettingFloat(setting.Key, setting.DefaultValue));
                }
            }
        }

        protected void RegisterGameSetting(GameSettingFloat setting)
        {
            GameSettings.Add(setting);
        }

        public GameSettingFloat FindGameSetting(string key)
        {
            foreach(GameSettingFloat setting in GameSettings)
            {
                if (setting.Key == key)
                {
                    return setting;
                }
            }
            throw new KeyNotFoundException($"Missing GameSetting \"{key}\"");
        }
        #endregion

        #region Saving
        public SaveManifest MainSaveManifest;
        public ExplicitSaveData EditorOverrideSaveData;
        public bool DontSaveInEditor;
        SaveDataTracker saveDataTracker;

        private void SetupSaveData()
        {
            if (MainSaveManifest == null) return;

#if UNITY_EDITOR
            if (EditorOverrideSaveData != null)
            {
                saveDataTracker = new SaveDataTrackerExplicit(MainSaveManifest, EditorOverrideSaveData);
            }
            else
            {
                saveDataTracker = new SaveDataTrackerFile(MainSaveManifest);
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